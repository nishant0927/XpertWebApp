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

    //public class HeaderFooterPageEvent : PdfPageEventHelper
    //{
    //    private readonly string _headerHtml;

    //    public HeaderFooterPageEvent(string headerHtml)
    //    {
    //        _headerHtml = headerHtml;
    //    }

    //    public override void OnEndPage(PdfWriter writer, Document document)
    //    {
    //        var cb = writer.DirectContent;
    //        var pageSize = document.PageSize;

    //        // Set header position
    //        var headerY = pageSize.Top - 40;

    //        // Create a table for the header, you can use iTextSharp's XMLWorkerHelper to render HTML
    //        var headerTable = new PdfPTable(1)
    //        {
    //            TotalWidth = pageSize.Width - document.LeftMargin - document.RightMargin, // Full width of the page
    //            LockedWidth = true
    //        };

    //        // Parse HTML header content to be added in the header of each page
    //        var headerReader = new StringReader(_headerHtml);
    //        var htmlWorker = XMLWorkerHelper.GetInstance();  // Use the GetInstance() static method to get an instance

    //        // Use a memory stream to hold the parsed content
    //        using (var msHeader = new MemoryStream())
    //        {
    //            // Create a temporary document for the header content
    //            using (var docHeader = new Document(PageSize.A4.Rotate()))
    //            {
    //                var writerHeader = PdfWriter.GetInstance(docHeader, msHeader);
    //                docHeader.Open();
    //                // Parse the header HTML and write it to the temporary document
    //                htmlWorker.ParseXHtml(writerHeader, docHeader, headerReader);
    //                docHeader.Close();
    //            }

    //            // Write the header content to the actual page
    //            PdfImportedPage headerPage = writer.GetImportedPage(new PdfReader(msHeader.ToArray()), 1);
    //            cb.AddTemplate(headerPage, document.LeftMargin, headerY);
    //        }
    //    }

    //}


    public class HeaderFooterPageEvent : PdfPageEventHelper
    {
        private readonly string _headerHtml;

        public HeaderFooterPageEvent(string headerHtml)
        {
            _headerHtml = headerHtml;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            var cb = writer.DirectContent;
            var pageSize = document.PageSize;

            // Parse the HTML header and render it on every page
            using (var sr = new StringReader(_headerHtml))
            {
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
            }
        }
    }


}