using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace XpertWebApp
{
    public class ExcelExportHelper
    {
        public static byte[] ExportDataToExcel(List<List<(string ColumnName, object Value)>> dataTable, string sheetName = "Sheet1", List<string> headers = null, bool centerHeaders = true )
        {
           

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                try
                {
                    // Create a new worksheet
                    var worksheet = package.Workbook.Worksheets.Add(sheetName);

                    int currentRow = 1;

                    // Add headers if provided
                    if (headers != null && headers.Count > 0)
                    {
                        foreach (var header in headers)
                        {
                            var headerCell = worksheet.Cells[currentRow, 1];
                            headerCell.Value = header;
                            worksheet.Cells[currentRow, 1, currentRow, dataTable[0].Count].Merge = true; // Merge cells across the row
                            headerCell.Style.HorizontalAlignment = centerHeaders ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Left;
                            currentRow++;
                        }

                        currentRow++; // Add an extra row before data
                    }

                    // Insert column names
                    for (int col = 0; col < dataTable[0].Count; col++)
                    {
                        worksheet.Cells[currentRow, col + 1].Value = dataTable[0][col].ColumnName;
                    }
                    currentRow++;

                    // Insert the DataTable content into the worksheet starting from the current row
                    foreach (var row in dataTable)
                    {
                        for (int col = 0; col < row.Count; col++)
                        {
                            worksheet.Cells[currentRow, col + 1].Value = row[col].Value;
                        }
                        currentRow++;
                    }

                    // Adjust column widths to fit content
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Return the file content as a byte array
                    return package.GetAsByteArray();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
            }


        }
    }
}