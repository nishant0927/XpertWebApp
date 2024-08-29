using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace XpertWebApp
{
    public class PdfGenerator
    {

        public static byte[] GeneratePdf(List<List<(string ColumnName, object Value)>> dataTable, string title = null, List<string> headers = null, bool centerHeaders = true)
        {

            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Add title
                if (!string.IsNullOrEmpty(title))
                {
                    Font titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
                    Paragraph titleParagraph = new Paragraph(new Phrase(title, titleFont))
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20f
                    };
                    pdfDoc.Add(titleParagraph);
                }

                // Add headers below title
                if (headers != null && headers.Count > 0)
                {
                    Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    PdfPTable headerTable = new PdfPTable(1) // Single column to span the width
                    {
                        WidthPercentage = 100
                    };

                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT, // Align text to the left
                            Padding = 5,
                            Border = PdfPCell.NO_BORDER, // Remove the border to match the look in the image
                            Colspan = 1 // Span full width
                        };
                        headerTable.AddCell(cell);
                    }

                    pdfDoc.Add(headerTable);
                    Paragraph spacing = new Paragraph(new Phrase("\n")) // Add some space after the headers
                    {
                        SpacingAfter = 10f
                    };
                    pdfDoc.Add(spacing);
                }

                // Iterate over each dataTable
                foreach (var d in dataTable)
                {
                    // Determine the number of columns
                    int columnCount = d.Count;

                    PdfPTable pdfTable = new PdfPTable(columnCount)
                    {
                        WidthPercentage = 100
                    };

                    // Add column names as headers from the first row
                    if (dataTable.Count > 0)
                    {
                        foreach (var item in dataTable[0])
                        {
                            var (ColumnName, _) = item;
                            PdfPCell cell = new PdfPCell(new Phrase(ColumnName, FontFactory.GetFont("Arial", 10, Font.BOLD)))
                            {
                                HorizontalAlignment = centerHeaders ? Element.ALIGN_CENTER : Element.ALIGN_LEFT,
                                Padding = 5,
                                BackgroundColor = BaseColor.LIGHT_GRAY
                            };
                            pdfTable.AddCell(cell);
                        }
                    }

                    // Add data rows
                    Font rowFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                    foreach (var row in dataTable)
                    {
                        foreach (var item in row)
                        {
                            var (ColumnName, Value) = item;
                            PdfPCell cell = new PdfPCell(new Phrase(Value?.ToString() ?? string.Empty, rowFont))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 5
                            };
                            pdfTable.AddCell(cell);
                        }
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.NewPage(); // Start a new page for each table
                }

                pdfDoc.Close();
                return stream.ToArray();

                //using (MemoryStream stream = new MemoryStream())
                //{
                //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                //    pdfDoc.Open();

                //    // Add title
                //    if (!string.IsNullOrEmpty(title))
                //    {
                //        Font titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
                //        Paragraph titleParagraph = new Paragraph(new Phrase(title, titleFont))
                //        {
                //            Alignment = Element.ALIGN_CENTER,
                //            SpacingAfter = 20f
                //        };
                //        pdfDoc.Add(titleParagraph);
                //    }

                //    foreach (var d in dataTable)
                //    {
                //        // Determine the number of columns
                //        int columnCount = d.Count ;

                //        PdfPTable pdfTable = new PdfPTable(columnCount)
                //        {
                //            WidthPercentage = 100
                //        };

                //        // Add header row
                //        Font headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                //        //if (headers != null && headers.Count > 0)
                //        //{
                //        //    foreach (var header in headers)
                //        //    {
                //        //        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                //        //        {
                //        //            HorizontalAlignment = centerHeaders ? Element.ALIGN_CENTER : Element.ALIGN_LEFT,
                //        //            Padding = 5,
                //        //            BackgroundColor = BaseColor.LIGHT_GRAY
                //        //        };
                //        //        pdfTable.AddCell(cell);
                //        //    }
                //        //}
                //        //else
                //        //{
                //            // Add column names as headers from the first row
                //            if (dataTable.Count > 0)
                //            {
                //                foreach (var item in dataTable[0])
                //                {
                //                    var (ColumnName, _) = item;
                //                    PdfPCell cell = new PdfPCell(new Phrase(ColumnName, headerFont))
                //                    {
                //                        HorizontalAlignment = centerHeaders ? Element.ALIGN_CENTER : Element.ALIGN_LEFT,
                //                        Padding = 5,
                //                        BackgroundColor = BaseColor.LIGHT_GRAY
                //                    };
                //                    pdfTable.AddCell(cell);
                //                }
                //            }
                //        //}

                //        // Add data rows
                //        Font rowFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                //        foreach (var row in dataTable)
                //        {
                //            foreach (var item in row)
                //            {
                //                var (ColumnName, Value) = item;
                //                PdfPCell cell = new PdfPCell(new Phrase(Value?.ToString() ?? string.Empty, rowFont))
                //                {
                //                    HorizontalAlignment = Element.ALIGN_LEFT,
                //                    Padding = 5
                //                };
                //                pdfTable.AddCell(cell);
                //            }
                //        }

                //        pdfDoc.Add(pdfTable);
                //        pdfDoc.NewPage(); // Start a new page for each table
                //    }

                //    pdfDoc.Close();
                //    return stream.ToArray();
                //}
            }




        }

        public static string GenerateDynamicHtmlTable(List<List<(string ColumnName, object Value)>> dataTable, List<string> headers = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table style=\"width:100%; text-align:center;\">");

            foreach (var header in headers)
            {
                sb.Append("<tr>");
                sb.Append($"<td style=\"font:bold;font-size:12px;\">{header}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            if (dataTable.Count > 0)
            {
                sb.Append("<table style=\"width:100%; font-size:12px; border-collapse: collapse;\">");

                // Add the table header
                sb.Append("<thead style=\"font:bold; border:1px solid\">");
                sb.Append("<tr>");
                foreach (var (ColumnName, _) in dataTable[0])
                {
                    sb.Append($"<th style=\"border:1px solid\">{ColumnName}</th>");
                }
                sb.Append("</tr>");
                sb.Append("</thead>");

                // Add the table body
                sb.Append("<tbody>");
                foreach (var row in dataTable)
                {
                    sb.Append("<tr>");
                    foreach (var (_, Value) in row)
                    {
                        sb.Append($"<td style=\"border:1px solid; text-align:left; padding:5px;\">{Value}</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");

                sb.Append("</table>");
            }
            return sb.ToString();
        }



        public static byte[] GeneratePdfWithHeadersAndColumns(List<string> headers, List<List<(string ColumnName, object Value)>> dataTable)
        {
           

            // Determine the page orientation based on the number of rows
            Rectangle pageSize = dataTable.Count > 20 ? PageSize.A4.Rotate() : PageSize.A4;

           

            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(pageSize, 25, 25, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                //writer.PageEvent = new CustomPdfPageEventHelper(headers);

                pdfDoc.Open();

                StringBuilder sb = new StringBuilder();

                // Append headers
                sb.AppendLine("<table style=\"width:100%; text-align:center;\">");
                foreach (var header in headers)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td style=\"font:bold;font-size:16px;\">{header}</td>");
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table>");

                // Append table with columns and rows
                sb.AppendLine("<table style=\"width:100%; font-size:12px; border:1px solid black; border-collapse: collapse; margin-top:20%;\">");
                sb.AppendLine("<thead style=\"font:bold; border:1px solid\">");
                sb.AppendLine("<tr>");

                // Add column headers
                foreach (var column in dataTable[0])
                {
                    sb.AppendLine($"<th style=\"border:1px solid black; padding:5px;\">{column.ColumnName}</th>");
                }

                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");

                // Add rows
                foreach (var row in dataTable)
                {
                    sb.AppendLine("<tr>");
                    foreach (var cell in row)
                    {
                        sb.AppendLine($"<td style=\"border:1px solid black; padding:5px;\">{cell.Value}</td>");
                    }
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                StringReader sr = new StringReader(sb.ToString());
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

                pdfDoc.Close();
                return stream.ToArray();
            }

            //return fileContents;
        }



    }

    public class CustomPdfPageEventHelper : PdfPageEventHelper
    {
        private readonly List<string> _headers;

        public CustomPdfPageEventHelper(List<string> headers)
        {
            _headers = headers;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable headerTable = new PdfPTable(1)
            {
                TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            foreach (var header in _headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 12, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 3,
                    PaddingBottom = 3
                };
                headerTable.AddCell(cell);
            }
            float headerYPosition = document.PageSize.Height - document.TopMargin +18;
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, headerYPosition, writer.DirectContent);
            // Calculate the correct position for the header table at the top of the page
            //float headerYPosition = document.PageSize.Height - document.TopMargin + headerTable.TotalHeight +10; // Adjusted value
            //headerTable.WriteSelectedRows(0, -1, document.LeftMargin, headerYPosition, writer.DirectContent);
        }
    }


    public class PdfPageEventHelp : PdfPageEventHelper
    {
        public List<string> Headers { get; set; }
        public List<List<(string ColumnName, object Value)>> DataTable { get; set; }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            foreach (var header in Headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                headerTable.AddCell(cell);
            }

            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + headerTable.TotalHeight + 10, writer.DirectContent);

            // Reset for next page
            writer.DirectContent.MoveTo(document.LeftMargin, document.PageSize.Height - document.TopMargin + headerTable.TotalHeight + 10);
        }
    }

}