using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline;
using Newtonsoft.Json.Linq;

namespace XpertWebApp
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.IO;

    public class HeaderFooter : PdfPageEventHelper
    {
        private string _title;
        private string _logoPath;

        public HeaderFooter(string title, string logoPath)
        {
            _title = title;
            _logoPath = logoPath;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable headerTable = new PdfPTable(2)
            {
                WidthPercentage = 100


            };
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

            // Add logo
            Image logo = Image.GetInstance(_logoPath);
            logo.ScaleToFit(40f, 20f);
            PdfPCell logoCell = new PdfPCell(logo);
            logoCell.Border = Rectangle.NO_BORDER;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.AddCell(logoCell);

            // Add title
            PdfPCell titleCell = new PdfPCell(new Phrase(_title, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            titleCell.Border = Rectangle.NO_BORDER;
            titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
            titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.AddCell(titleCell);

            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);
            PdfContentByte cb = writer.DirectContent;
            cb.MoveTo(document.LeftMargin, document.PageSize.Height - 40);  // Adjust the -40 to change the space
            cb.Stroke();
        }
    }





}