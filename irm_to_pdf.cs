
//css_reference C:\Temp\pdf\dotNet\iText.Kernel.dll;
//css_reference C:\Temp\pdf\dotNet\iText.Layout.dll;
//css_reference C:\Temp\pdf\dotNet\iText.IO.dll;
//css_args /platform:x64 /target:exe

using System;
using System.IO;
using System.Text.RegularExpressions;  
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.IO.Font;

/*
	csc.exe -r:itext.io.dll;itext.kernel.dll;itext.layout.dll irm_to_pdf.cs && irm_to_pdf.exe 1684/2001 " kjt "
*/

class Program
{

	static void Main(String[] args)
	{

		string felirat = args.Length>1 ? args[1] : "";
		string IRM = "0000/0000";
		
		try
		{
			Regex r = new Regex(@"\d{1,4}\/\d{4}\.?");
			do
			{
				if ( args.Length.Equals(0) )
				{
					Console.Write("\nIRM szám: ");
					IRM = Console.ReadLine();
				}
				else
				{
					IRM = args[0];
				}
			}
			while ( ! r.IsMatch(IRM) );
			string SRC = Regex.Replace( IRM.Replace("/","-") , @"\.$" , "" );
			SRC = felirat.Length.Equals(0) ? SRC + ".pdf" : SRC + "+név.pdf";
			
			var pdfWriter = new PdfWriter( new FileStream(SRC, FileMode.Create, FileAccess.Write) );
			var pdfDocument = new PdfDocument(pdfWriter);
			//var document = new Document(pdfDocument, new PageSize(new Rectangle(80,100)).Rotate());
			var document = new Document(pdfDocument, new PageSize(PageSize.A5).Rotate());
			//document.SetMargins(10,10,10,10);

			var font0 = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN, PdfEncodings.CP1250);
			var font1 = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA, PdfEncodings.CP1250);
			//var font2 = PdfFontFactory.CreateFont("Alef-Regular.ttf", PdfEncodings.CP1250, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
			//var font3 = PdfFontFactory.CreateFont("Hack-Regular.ttf", PdfEncodings.CP1250, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
	  
			var p = new Paragraph();
			
			p.Add( felirat.Length.Equals(0) ?
						new Text("Ingatlanrendezõ földmérõ\nminõsítés száma:\n").SetFontSize(15) :
						new Text(felirat+"\n").SetFontSize(30)
				 );
			p.SetFont(font1)
			 //.SetFontSize(40)
			 .SetFontColor(ColorConstants.BLUE)
			 .SetTextAlignment(TextAlignment.CENTER)
			 //.SetVerticalAlignment(VerticalAlignment.MIDDLE)
			 //.SetMargin(50)
			 .SetPadding(10)
			 //.SetBackgroundColor(ColorConstants.LIGHT_GRAY,(float).5)
			 //.SetBorderRadius(new BorderRadius(UnitValue.CreatePointValue(1000)))
			 //.SetBorder(new DottedBorder(ColorConstants.BLUE,2))
			 //.SetBorder(new SolidBorder(ColorConstants.BLUE,2))
			 //.SetFixedLeading(100)
			 //.SetHeight(canvas.getHeight());		
			;
			p.Add(new Text(IRM).SetFontSize(20).SetHorizontalScaling(2));
			//document.Add(p);

			var d = new Div();
			d.SetVerticalAlignment(VerticalAlignment.MIDDLE)
			 //.SetWidth(100)
			 //.SetHeight(100)
			 .SetKeepTogether(true)
			 .SetMargin(100)
			 .SetBorderRadius(new BorderRadius(UnitValue.CreatePointValue(1000)))
			 //.SetProperty(Property.BORDER_RADIUS, (float)50)
			 .SetBorder(new SolidBorder(ColorConstants.BLUE,2))
			;
			d.Add(p);
			document.Add(d);

			/*
			//var page = pdfDocument.pdfDocument.GetPage(1);
			var page = pdfDocument.AddNewPage();
            
			var pageW = page.GetPageSize().GetWidth();
			var pageH = page.GetPageSize().GetHeight(); Console.WriteLine("["+pageW+","+pageH+"]");
			var axial = new PdfShading.Axial(new PdfDeviceCs.Rgb(), 0, page.GetPageSize().GetHeight(), ColorConstants.WHITE.GetColorValue(), page.GetPageSize().GetWidth(), 0, ColorConstants.LIGHT_GRAY.GetColorValue());
            //var radial = new PdfShading.Radial(new PdfDeviceCs.Rgb(), 200, 200, 100, ColorConstants.WHITE.GetColorValue(), 400, 300, 100, ColorConstants.GREEN.GetColorValue());
            var radial = new PdfShading.Radial(new PdfDeviceCs.Rgb(),
													pageW/(float)1.9, pageH/(float)1.9, 0, ColorConstants.LIGHT_GRAY.GetColorValue(), 
													pageW/2, pageH/2, pageH/2, ColorConstants.WHITE.GetColorValue());
            var pattern = new PdfPattern.Shading(radial);
			
			var pdfCanvas = new PdfCanvas(page);
			pdfCanvas.SetColor(ColorConstants.BLUE,false);
			pdfCanvas.SetFillColorShading(pattern);
			pdfCanvas.Ellipse(pageW/8,pageH/4,pageW-(pageW/8),pageH-(pageH/4));
			//pdfCanvas.Fill();
			//pdfCanvas.FillStroke();
			var c = new Canvas(pdfCanvas, page.GetPageSize());
			//var c = new Canvas(page, page.GetPageSize());
			c.SetBorder(new DashedBorder(ColorConstants.RED,2))
			 //.SetBackgroundColor(ColorConstants.LIGHT_GRAY)
			 .SetBorderRadius(new BorderRadius(UnitValue.CreatePointValue(1000)))
			;
			//c.Add(p);
			//c.Add(d);
			c.Close();
			*/
			
			var info = pdfDocument.GetDocumentInfo();
			info.SetTitle("IRM szám")
				.SetAuthor("kjt")
				.SetSubject(IRM)
				.SetCreator("C# & iText")
				.SetKeywords(DateTime.Now.ToString())
			;

			document.Close();
			
			pdfWriter.Close();
			
			Console.WriteLine("The '"+SRC+"' file has been created.\n");
		}
		catch (Exception e) 
		{
			Console.WriteLine(e.Message);
		}
	}
}