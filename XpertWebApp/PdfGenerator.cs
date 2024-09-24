using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

        public static byte[] GeneratePdf(string companyName, string reportName, string reportDate, List<List<(string ColumnName, object Value)>> rows,int pageheigt)
        {
            Rectangle pageSize = rows[0].Count >5 ? PageSize.A4.Rotate() : PageSize.A4;
           // Rectangle pageSize= new Rectangle(864, 972);
            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(pageSize, 25, 25, pageheigt, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                // Set the page event helper (we will set it later after creating the header table)
                pdfDoc.Open();

                // Extract columns dynamically using a loop
                PdfPTable headerTable = new PdfPTable(rows[0].Count);  // Initialize the table with the number of columns
                headerTable.WidthPercentage = 100;

                // Loop to set dynamic column widths (here all widths are set to 2f)
                //float[] columnWidths = new float[rows[0].Count];
                //for (int i = 0; i < rows[0].Count; i++)
                //{
                //    columnWidths[i] = 2f;
                //}
                //headerTable.SetWidths(columnWidths);

                // Loop through the first row to get the column names and add them as headers
                foreach (var column in rows[0])
                {
                    headerTable.AddCell(CreateHeaderCell(column.ColumnName));
                }
                //PdfPCell emptyCell = new PdfPCell(new Phrase(" "))
                //{
                //    Border = PdfPCell.NO_BORDER,
                //    Colspan = rows[0].Count,
                //    PaddingTop =1f // Create space between header and data rows
                //};
                //headerTable.AddCell(emptyCell);
                //pdfDoc.Add(headerTable);
                // Set the page event helper after creating the header table
                CustomPdfPageEvent pageEventHelper = new CustomPdfPageEvent(companyName, reportName, reportDate, headerTable);
                writer.PageEvent = pageEventHelper;

                // Create the data table
                PdfPTable dataTable = new PdfPTable(rows[0].Count);
                dataTable.WidthPercentage = 100;
                //dataTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 40, writer.DirectContent);
                //dataTable.SetWidths(columnWidths); // Use the same column widths as the header

                // Add data cells
                foreach (var row in rows)
                {
                    foreach (var cell in row)
                    {
                        dataTable.AddCell(CreateCell(cell.Value?.ToString()));
                    }
                }

                pdfDoc.Add(dataTable);
                pdfDoc.Close();

                return stream.ToArray();
            }
        }

        private static PdfPCell CreateHeaderCell(string text)
        {
            return new PdfPCell(new Phrase(text, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)))
            {
                Border =  PdfPCell.TOP_BORDER| PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER ,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                //FixedHeight = 45f,
                //PaddingLeft = 5f,
                //PaddingTop = 8f,
                PaddingBottom = 8f,
                //PaddingRight = 35f,
                //Padding =15f,
                
            };
        }

        private static PdfPCell CreateCell(string text)
        {
            return new PdfPCell(new Phrase(text, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8)))
            {
                Border = PdfPCell.LEFT_BORDER|PdfPCell.RIGHT_BORDER|PdfPCell.BOTTOM_BORDER| PdfPCell.TOP_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                //FixedHeight = 45f,
                //PaddingLeft =5f,   // Adjust as needed
                //PaddingTop =8f,    // Adjust the top margin here
                //PaddingBottom = 8f,  // Adjust as needed
               // PaddingRight =35f
            };
        }

        //public class CustomPdfPageEvent : PdfPageEventHelper
        //{
        //    private readonly string _companyName;
        //    private readonly string _reportName;
        //    private readonly string _reportDate;
        //    private readonly List<string> _columns;

        //    public CustomPdfPageEvent(string companyName, string reportName, string reportDate, List<string> columns)
        //    {
        //        _companyName = companyName;
        //        _reportName = reportName;
        //        _reportDate = reportDate;
        //        _columns = columns;
        //    }

        //    public override void OnEndPage(PdfWriter writer, Document document)
        //    {
        //        PdfPTable headerTable = new PdfPTable(3);
        //        headerTable.WidthPercentage = 100;
        //        headerTable.SetWidths(new float[] { 1f, 1f, 1f });

        //        PdfPCell companyCell = new PdfPCell(new Phrase(_companyName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)))
        //        {
        //            Border = PdfPCell.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            Colspan = 3,
        //            PaddingBottom = 5f
        //        };
        //        headerTable.AddCell(companyCell);

        //        PdfPCell reportNameCell = new PdfPCell(new Phrase(_reportName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)))
        //        {
        //            Border = PdfPCell.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            Colspan = 2,
        //            PaddingBottom = 5f
        //        };
        //        headerTable.AddCell(reportNameCell);

        //        PdfPCell dateCell = new PdfPCell(new Phrase(_reportDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12)))
        //        {
        //            Border = PdfPCell.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_RIGHT,
        //            PaddingBottom = 5f
        //        };
        //        headerTable.AddCell(dateCell);

        //        headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        //        headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);

        //        // Add column headers
        //        PdfPTable columnHeaderTable = new PdfPTable(_columns.Count);
        //        columnHeaderTable.WidthPercentage = 100;
        //        columnHeaderTable.SetWidths(Enumerable.Repeat(2f, _columns.Count).ToArray());

        //        foreach (var column in _columns)
        //        {
        //            columnHeaderTable.AddCell(CreateHeaderCell(column));
        //        }

        //        columnHeaderTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        //        columnHeaderTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 30, writer.DirectContent);
        //    }
        //}


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

    public class PdfHeaderFooter : PdfPageEventHelper
    {
        private string _companyName;
        private string _reportDate;
        private List<string> _columnHeaders;

        public PdfHeaderFooter(string companyName, string reportDate, List<string> columnHeaders)
        {
            _companyName = companyName;
            _reportDate = reportDate;
            _columnHeaders = columnHeaders;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable headerTable = new PdfPTable(_columnHeaders.Count); // Dynamic number of columns
            headerTable.TotalWidth = document.PageSize.Width - 80; // Adjust margins
            headerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            // Add company name and report date to the header
            PdfPCell cell = new PdfPCell(new Phrase(_companyName, new Font(Font.FontFamily.HELVETICA, 14f, Font.BOLD)));
            cell.Colspan = _columnHeaders.Count;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            headerTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(_reportDate, new Font(Font.FontFamily.HELVETICA, 12f)));
            cell.Colspan = _columnHeaders.Count;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            headerTable.AddCell(cell);

            // Add dashed line under the header
            cell = new PdfPCell(new Phrase("--------------------------------------------------------------", new Font(Font.FontFamily.HELVETICA, 12f)));
            cell.Colspan = _columnHeaders.Count;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            headerTable.AddCell(cell);

            // Add the dynamic column headers
            foreach (var header in _columnHeaders)
            {
                cell = new PdfPCell(new Phrase(header, new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = Rectangle.BOTTOM_BORDER;
                headerTable.AddCell(cell);
            }

            // Write the table to the PDF
            headerTable.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 20, writer.DirectContent);
        }
    }




}