using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace XpertWebApp
{


    public class CustomPdfPageEvent : PdfPageEventHelper
    {
        private string _companyName;
        private string _reportName;
        private string _reportDate;
        private PdfPTable _headerTable;

        public CustomPdfPageEvent(string companyName, string reportName, string reportDate, PdfPTable headerTable)
        {
            _companyName = companyName;
            _reportName = reportName;
            _reportDate = reportDate;
            _headerTable = headerTable;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Company name (centered at the top)
            //PdfPTable companyTable = new PdfPTable(1);
            //companyTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            //companyTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //PdfPCell companyCell = new PdfPCell(new Phrase(_companyName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)))
            //{
            //    Border = PdfPCell.NO_BORDER,
            //    HorizontalAlignment = Element.ALIGN_CENTER,
            //    PaddingBottom = 5f
            //};
            //companyTable.AddCell(companyCell);

            //// Report name (centered) and date (right aligned) on the same row
            //PdfPTable reportTable = new PdfPTable(2);
            //reportTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            //reportTable.SetWidths(new float[] { 3f, 3f }); // Adjust the column widths
            //reportTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //PdfPCell reportNameCell = new PdfPCell(new Phrase(_reportName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
            //{
            //    Border = PdfPCell.NO_BORDER,
            //    HorizontalAlignment = Element.ALIGN_CENTER,
            //    PaddingBottom = 5f,
            //    //PaddingLeft=45f
            //};
            //reportTable.AddCell(reportNameCell);

            //PdfPCell dateCell = new PdfPCell(new Phrase(_reportDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
            //{
            //    Border = PdfPCell.NO_BORDER,
            //    HorizontalAlignment = Element.ALIGN_CENTER,
            //    PaddingBottom = 5f,
            //    //PaddingRight=25f
            //};
            //reportTable.AddCell(dateCell);

            //// Draw the company name
            //companyTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);

            //// Draw the report name and date below the company name
            //reportTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 30, writer.DirectContent);







            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            // Add the company name row (center aligned)
            PdfPCell companyCell = new PdfPCell(new Phrase(_companyName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 5f
            };
            headerTable.AddCell(companyCell);

            // Add the report name row (center aligned)
            PdfPCell reportNameCell = new PdfPCell(new Phrase(_reportName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 5f
            };
            headerTable.AddCell(reportNameCell);

            // Add the report date row (center aligned)
            PdfPCell dateCell = new PdfPCell(new Phrase(_reportDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 5f
            };
            headerTable.AddCell(dateCell);

            // Write the table to the PDF (starting near the top of the page)
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);
            // Draw the header table below the report name and date
            _headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            _headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 70, writer.DirectContent);
        }
    }




    //public class CustomPdfPageEvent : PdfPageEventHelper
    //{
    //    private PdfPTable _headerTable;

    //    public CustomPdfPageEvent(PdfPTable headerTable)
    //    {
    //        _headerTable = headerTable;
    //    }

    //    public override void OnEndPage(PdfWriter writer, Document document)
    //    {
    //        // Position the header table at the top of each page
    //        _headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

    //        _headerTable.WriteSelectedRows(0, -1, document.Left, document.Top + 30, writer.DirectContent);
    //        //_headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + _headerTable.TotalHeight, writer.DirectContent);
    //    }
    //}





}