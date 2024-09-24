using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.ListOfCowDCS;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using DAL.DashBoard;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Drawing;
using System.Threading.Tasks;
using DAL;
using Rectangle = iTextSharp.text.Rectangle;
using DAL.Common;

namespace XpertWebApp.Controllers
{
    public class RoutePaymentProcessController : Controller
    {
        public ActionResult RoutePaymentProcessReport()
        {
            if (Session["AppUserCode"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ExportRoutePaymentProcessReport(string fromDate, string toDate)
        {
            try
            {
                string formattedFromDate = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                string formatttedDateToDate = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                var responseData = clsDashBoard.GetLedgerData(Session["CompCode"].ToString(), formattedFromDate, formatttedDateToDate, Session["CompCode"].ToString(), Session["AppUserCode"].ToString(), Session["Port"].ToString());
                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseData);
                var mainTable = jsonObject["MainTable"];              
                List<string> arrHeader = new List<string>();               
                arrHeader.Add("Le");
                arrHeader.Add("Date : " + fromDate + " To " + toDate);
                if (jsonObject.Count > 0)
                {
                    StringBuilder sbr = new StringBuilder();
                    if (mainTable != null)
                    {
                        foreach (var dr in mainTable)
                        {

                           

                            //sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif'>");
                            //sbr.Append("<tr>");
                            //sbr.Append("<td style='text-align:center; font-weight:bold; font-size:14px'>");
                            //sbr.Append("<span style='font-size:17px'></span><br>");
                            //sbr.Append("From Date 11/08/2024 to 20/08/2024 for Banswara<br>");
                            //sbr.Append("DCS Ledger Report");
                            //sbr.Append("</td>");
                            //sbr.Append("</tr>");
                            //sbr.Append("</table>");
                            
                            sbr.Append("<table width='100%' style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif; font-size:10px;border-top:1px dashed;'>");
                            sbr.Append("<tr style='text-align:center; font-weight:bold;'>");
                            sbr.Append("<td width='18%' align='left'>Route :</td>");
                            sbr.Append("<td width='11%' align='left'>" + dr["ROUTE_CODE"] + "</td>");
                            sbr.Append("<td width='71%' align='left'" + dr["Route_Name"] + "</td>");
                            sbr.Append("</tr>");
                            sbr.Append("</table>");
                            
                            sbr.Append("<table width='100%' style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif;font-size:10px;border-top:1px dashed; border-bottom:1px dashed;'>");
                            sbr.Append("<tr style='text-align:center; font-weight:bold;'>");
                            sbr.Append("<td width='18%' align='left'>DCS :</td>");
                            sbr.Append("<td width='11%' align='left'>" + dr["VLC_Code_VLC_Uploader"] + "</td>");
                            sbr.Append("<td width='71%' align='left'>" + dr["Vendor_Name"] + "</td>");
                            sbr.Append("</tr>");
                            sbr.Append("</table>");                           
                            sbr.Append("<p style='font-weight:bold; font-family:Verdana, Geneva, sans-serif;font-size:10px;text-decoration:underline;margin-top: 15px;   margin-bottom:5px; '>");
                            sbr.Append("Milk Details :");
                            sbr.Append("</p>");
                            sbr.Append("");
                            sbr.Append("<table width='100%'  style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif;font-size:10px;'>");
                            sbr.Append("<tr style='font-weight:bold;  text-align:center'>");
                            sbr.Append("<td>&nbsp;</td>");
                            sbr.Append("<td>Sweet Qty</td>");
                            sbr.Append("<td>Sour Qty</td>");
                            sbr.Append("<td>Curd Qty</td>");
                            sbr.Append("<td>Total</td>");
                            sbr.Append("<td>Fat %</td>");
                            sbr.Append("<td>SNF %</td>");
                            sbr.Append("<td>Kg Fat</td>");
                            sbr.Append("<td>Kg SNF</td>");
                            sbr.Append("<td>Amount</td>");
                            sbr.Append("</tr>");
                            sbr.Append("<tr>");
                            sbr.Append("<td>Group Name</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SweetQty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SourQty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["CurdQty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["Qty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["FAT_PER"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SNF_PER"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["FATQTY"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SNFQTY"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SRN_Net_Amount"] + "</td>");
                            sbr.Append("</tr>"); sbr.Append("<tr>");
                            sbr.Append("<td height='36'><strong>Total</strong></td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SweetQty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SourQty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["CurdQty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["Qty"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["FAT_PER"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SNF_PER"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["FATQTY"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SNFQTY"] + "</td>");
                            sbr.Append("<td style='text-align:center;'>" + dr["SRN_Net_Amount"] + "</td>");
                            sbr.Append("</tr>");
                            sbr.Append("<tr>");
                            sbr.Append("<td height='36' colspan='10' style='font-weight:bold;'>Pay Deduction Details :</td>");
                            sbr.Append("</tr>");
                            sbr.Append("</table>");                           
                            sbr.Append("<table width='100%' style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif;font-size:10px;'>");
                            sbr.Append("<tr>");
                            sbr.Append("<td>");
                            sbr.Append("<table width='100%' align='center' style='text-align:left; margin-bottom:7px; margin-top:5pxfont-size:10px;'>");
                            sbr.Append("<tr>");
                            sbr.Append("<td width='470'>");
                            sbr.Append("<div>");
                            sbr.Append("<table width='100%' style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif;font-size:10px;border:1px solid;'>");
                            sbr.Append("<tr>");
                            sbr.Append("<td style='font-weight:bold;text-decoration:underline;'>Payment</td>");
                            sbr.Append("<td style='font-weight:bold;text-decoration:underline;'>Amount</td>");
                            string vsp = dr["VSP_CODE"].ToString();
                            //var additionTable = jsonObject["Addittion"].ToList();
                            var deductionTable = jsonObject["Deduction"].Where(x=>x["Vendor_CODE"].ToString()!=null && x["Vendor_CODE"].ToString()==dr["VSP_CODE"].ToString()).Select(x=>new { 
                            dediction=x["Ded_Code"].ToString(),
                            amount=x["Amount"].ToString()
                            }).ToList();
                            var additionTable = jsonObject["Addittion"].Where(item => item["VSP_Code"] != null && item["VSP_Code"].ToString() == dr["VSP_CODE"].ToString()).Select
                                (item => new
                                {
                                    Addition = item["Addition"].ToString(),
                                    Amount=item["Amount"].ToString()
                                })
                                .ToList();
                            //var add = jsonObject["Addittion"].Where(item => item["VSP_CODE"].ToString() == dr["VSP_CODE"].ToString())?.ToList();

                            //var lst = dr["Addition"].Where(addition => dr["VSP_CODE"].ToString() == vspCode).ToList();
                            //sbr.Append("<td></td>");
                            sbr.Append("</tr>");
                            sbr.Append(string.Join("", additionTable.Select(entery=> "<tr><td>"+ entery.Addition+ "</td><td>"+entery.Amount+"</td></tr>")));
                            //sbr.Append("<td></td>");
                            //sbr.Append("<td></td>");
                            //sbr.Append("</tr>");
                            //sbr.Append("<tr>");
                            //sbr.Append("<td>Milk Payment</td>");
                            //sbr.Append("<td>-----</td>");
                            //sbr.Append("</tr>");
                            sbr.Append("<tr>");
                            sbr.Append("<td style='border-top:1px solid;border-bottom:1px solid;font-weight: bold;'>Total Payment</td>");
                            sbr.Append("<td style='font-weight:bold;border-top:1px solid;border-bottom:1px solid;'>" + additionTable.Sum(x=> Convert.ToDecimal(x.Amount))+"</td>");
                            sbr.Append("</tr>");
                            sbr.Append("</table>");
                            sbr.Append("</div>");
                            sbr.Append("</td>");
                            sbr.Append("");
                            sbr.Append("<td width='10'></td>");
                            sbr.Append("");
                            sbr.Append("<td width='470'>");
                            sbr.Append("<div style='padding:4px'>");
                            sbr.Append("<table width='100%' style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif; box-shadow:0 0 3px #cccccc;font-size:10px;border: 1px solid;'>");
                            sbr.Append("<tr>");
                            sbr.Append("<td style='font-weight:bold;text-decoration:underline;'>Deducation</td>");
                            sbr.Append("<td style='font-weight:bold;text-decoration:underline;'>Amount</td>");
                            sbr.Append("</tr>");
                            sbr.Append(string.Join("", deductionTable.Select(entery => "<tr><td>" + entery.dediction + "</td><td>" + entery.amount + "</td></tr>")));
                            sbr.Append("<tr>");
                            sbr.Append("<td style='border-top:1px solid;border-bottom:1px solid;font-weight: bold;'>Total Deducation</td>");
                            sbr.Append("<td style='font-weight:bold;border-top:1px solid;border-bottom:1px solid;'>" + deductionTable.Sum(x=> Convert.ToDecimal(x.amount))+"</td>");
                            sbr.Append("</tr>");
                            //sbr.Append("<tr>");
                            //sbr.Append("<td>&nbsp;</td>");
                            //sbr.Append("<td>&nbsp;</td>");
                            //sbr.Append("</tr>");
                            sbr.Append("</table>");
                            sbr.Append("</div>");
                            sbr.Append("</td>");
                            sbr.Append("</tr>");
                            sbr.Append("</table>");
                            sbr.Append("</td>");
                            sbr.Append("</tr>");
                            sbr.Append("</table>");
                            //sbr.Append("");
                            //sbr.Append("<table width='100%'  style='margin:0; padding:0; font-family:Verdana, Geneva, sans-serif;font-size:10px;'>");
                            //sbr.Append("<tr>");
                            //sbr.Append("<td width='17%'><strong>Net Payable :</strong></td>");
                            //sbr.Append("<td width='83%'>-----</td>"); sbr.Append("</tr>");
                            //sbr.Append("<tr>"); sbr.Append("<td><strong>Net Payable In Words :</strong></td>");
                            //sbr.Append("<td>-----</td>");
                            //sbr.Append("</tr>");
                            //sbr.Append("</table>");
                            //sbr.Append("");
                            //sbr.Append("");
                            //sbr.Append("<div class=''>");
                            //sbr.Append("<div style=' width:100%; border-top:1px dashed #000; padding-top:10px; margin-bottom:10px'>");
                            //sbr.Append("<p style='text-align:center; font-weight:bold; font-size:11px'>");
                            //sbr.Append("This Document is Auto Generated Here Signature is Not Required.");
                            //sbr.Append("</p>"); sbr.Append("</div>");
                            //sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='border-collapse:collapse; font-family:Arial, Helvetica, sans-serif; font-size:10px'>");
                            //sbr.Append("<tr style='font-weight:bold'>"); sbr.Append("<td width='8%'><div align='left'>Prepared By :</div></td>");
                            //sbr.Append("<td width='69%'><div align='left'>------</div></td>");
                            //sbr.Append("<td width='11%'><div align='left'>Check By :</div></td>");
                            //sbr.Append("<td width='12%'><div align='left'>-----</div></td>");
                            //sbr.Append("</tr>");
                            //sbr.Append("<tr style='font-weight:bold'>");
                            //sbr.Append("<td><div align='left'>User :</div></td>");
                            //sbr.Append("<td><div align='left'>------</div></td>");
                            //sbr.Append("<td><div align='left'>Print Date & Time</div></td>");
                            //sbr.Append("<td><div align='left'>------</div></td>");
                            //sbr.Append("</tr>");
                            //sbr.Append("</table>");
                            //sbr.Append("</div>");
                            
                        }
                        
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                    }

                    //string html = sbr.ToString();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4_LANDSCAPE, 25, 25,25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        //CustomPdfPageEvent pageEventHelper = new CustomPdfPageEvent(companyName, reportName, reportDate, headerTable);
                        //writer.PageEvent = pageEventHelper;
                        pdfDoc.Open();

                        // Write the body content
                        var bodyReader = new StringReader(sbr.ToString());
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, bodyReader);

                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "DCS_Ledger.pdf");
                    }






                    // Extract columns excluding "Result"
                    //var columns = jsonArray[0].Children<JProperty>()
                    //                          .Select(jp => jp.Name)
                    //                          .Where(key => key != "Result")
                    //                          .ToList();
                    //var rows = jsonArray.Select
                    //    (item => columns.Select
                    //     (column => (ColumnName: column, Value: (object)(item[column] != null ? item[column].ToString() : string.Empty))).ToList()).ToList();

                    //byte[] fileContents = new byte[0];
                    //string fileType = string.Empty;
                    //string fileName = string.Empty;

                    //// Check if Excel export is requested

                    ////fileContents = PdfGenerator.GeneratePdfWithHeadersAndColumns(arrHeader, rows);
                    //fileType = "application/pdf";
                    //fileName = "PaymentCycleWiseReport.pdf";


                    //return File(fileContents, fileType, fileName);
                }
                else
                {
                    // Handle the case where jsonArray is empty
                    return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ExportDCSSummary(string fromDate, string toDate, string month)
        {
            try
            {
                string compCode = Session["CompCode"].ToString();
                string formattedFromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                string formatttedDateToDate = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
                var responseData = clsDashBoard.GeDCSSummary(Session["CompCode"].ToString(), Session["AppUserCode"].ToString(), formattedFromDate, formatttedDateToDate, month, Session["Port"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                StringBuilder sb = new StringBuilder();
                string companyName = (string)jsonArray[0]["CompName"];
                string slNo = (string)jsonArray[0]["1"];
                string reportDate = "DCS Summary For " + (string)jsonArray[0]["fromDate"] + " To " + (string)jsonArray[0]["todate"];
                string reportName = "Daily Summary Report";
                byte[] k = null;
                if (jsonArray.Count > 0)
                {

                    StringBuilder sbrh = new StringBuilder();
                    StringBuilder sbrb = new StringBuilder();

                 

                    PdfPTable headerTable = new PdfPTable(17);
                    headerTable.WidthPercentage = 100;
                    //headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                    headerTable.LockedWidth = true; // Lock the width so it fits the page
                    headerTable.HorizontalAlignment = Element.ALIGN_CENTER;

                    // First Row
                    headerTable.AddCell(new PdfPCell(new Phrase("")) { Colspan = 17, Border = PdfPCell.TOP_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("")) { Colspan =4, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("QUANTITY: <---MORNING--->", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("<---EVENING--->", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("<---TOTAL--->", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("")) { Colspan = 4, Border = PdfPCell.NO_BORDER });

                    // Second Row (DCS NAME and Columns)
                    headerTable.AddCell(new PdfPCell(new Phrase("DCS NAME", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER ,Border=PdfPCell.NO_BORDER});
                    headerTable.AddCell(new PdfPCell(new Phrase("SWEET", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("SOUR", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("CURD", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("SWEET", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("SOUR", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("CURD", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("SWEET", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("SOUR", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("CURD", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("AVG/DAY", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("FAT%", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("SNF%", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("DAYS", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });

                    // Third Row (SRL and Numbers)
                    headerTable.AddCell(new PdfPCell(new Phrase("SRL", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    for (int i = 1; i <= 16; i++)
                    {
                        headerTable.AddCell(new PdfPCell(new Phrase(i.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    }

                    // Fourth Row (Date and more numbers)
                    headerTable.AddCell(new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    for (int i = 17; i <= 31; i++)
                    {
                        headerTable.AddCell(new PdfPCell(new Phrase(i.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    }
                    headerTable.AddCell(new PdfPCell(new Phrase("TOTAL", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA,8))) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER });
                    headerTable.AddCell(new PdfPCell(new Phrase("")) { Colspan = 17, Border = PdfPCell.BOTTOM_BORDER });
                    string headerHtml = sbrh.ToString();

                    // Body HTML
                    sbrb.Append("<div style='margin-top:40%'>");
                    sbrb.Append("<table width='100%' border='0' cellspacing='0'  style='margin:0;padding:0;font-family:Verdana,Geneva,sans-serif; font-size:8px;margin-top:70%;'>");
                    int srno = 1;
                    foreach (var dr in jsonArray)
                    {
                        sbrb.Append("<tr>");
                        sbrb.Append("<td Colspan='2'></td>");
                        sbrb.Append("<td  style='width:10%'>" + dr["Vendor_Name"] +"</td>");
                        sbrb.Append("<td>"+dr["MorningSweetQty"] +"</td>");
                        sbrb.Append("<td>" + dr["MorningSoreQty"] + "</td>");
                        sbrb.Append("<td>" + dr["MorningCurdQty"] + "</td>");
                        sbrb.Append("<td>" + dr["EveningSweetQty"] + "</td>");
                        sbrb.Append("<td>" + dr["EveningSoreQty"] + "</td>");
                        sbrb.Append("<td>"+dr["EveningCurdQty"] +"</td>");
                        sbrb.Append("<td>"+dr["TotalSweetQty"] + "</td>");
                        sbrb.Append("<td>"+dr["TotalSoreQty"] +"</td>");
                        sbrb.Append("<td>"+dr["TotalCurdQty"] +"</td>");
                        sbrb.Append("<td>"+dr["FATPer"] +"</td>");
                        sbrb.Append("<td>"+dr["SNFPer"] +"</td>");
                        sbrb.Append("<td>"+dr["DAYS_Total"] + "</td>");
                        sbrb.Append("<td>"+dr["AVG_QTY"] + "</td>");
                        sbrb.Append("<td>"+dr["TotalQty"] + "</td>");
                        sbrb.Append("</tr>");                       
                        sbrb.Append("<tr>");
                        sbrb.Append("<td rowspan='2'>" + srno + "</td>");
                        sbrb.Append("<td>" + dr["1"] +"</td>");
                        sbrb.Append("<td>" + dr["2"] + "</td>");
                        sbrb.Append("<td>" + dr["3"] + "</td>");
                        sbrb.Append("<td>" + dr["4"] + "</td>");
                        sbrb.Append("<td>" + dr["5"] + "</td>");
                        sbrb.Append("<td>" + dr["6"] + "</td>");
                        sbrb.Append("<td>" + dr["7"] + "</td>");
                        sbrb.Append("<td>" + dr["8"] + "</td>");
                        sbrb.Append("<td>" + dr["9"] + "</td>");
                        sbrb.Append("<td>" + dr["10"] + "</td>");
                        sbrb.Append("<td>" + dr["11"] + "</td>");
                        sbrb.Append("<td>" + dr["12"] + "</td>");
                        sbrb.Append("<td>" + dr["13"] + "</td>");
                        sbrb.Append("<td>" + dr["14"] + "</td>");
                        sbrb.Append("<td>" + dr["15"] + "</td>");
                        sbrb.Append("<td>" + dr["16"] + "</td>");
                        sbrb.Append("</tr>"); 
                        sbrb.Append("<tr>");
                        sbrb.Append("<td>" + dr["17"] + "</td>");
                        sbrb.Append("<td>" + dr["18"] + "</td>");
                        sbrb.Append("<td>" + dr["19"] +"</td>");                       
                        sbrb.Append("<td>" + dr["20"] +"</td>");                       
                        sbrb.Append("<td>" + dr["21"] +"</td>");                       
                        sbrb.Append("<td>" + dr["22"] +"</td>");                       
                        sbrb.Append("<td>" + dr["23"] + "</td>");                       
                        sbrb.Append("<td>" + dr["24"] + "</td> ");                       
                        sbrb.Append("<td>" + dr["25"] +"</td>");                        
                        sbrb.Append("<td>" + dr["26"] +"</td>");                        
                        sbrb.Append("<td>" + dr["27"] + "</td>");                        
                        sbrb.Append("<td>" + dr["28"] +"</td> ");                        
                        sbrb.Append("<td>" + dr["29"] +"</td>");                        
                        sbrb.Append("<td>" + dr["30"] +"</td>");                        
                        sbrb.Append("<td>" + dr["31"] +"</td>");                        
                        sbrb.Append("<td>&nbsp;</td>");
                        sbrb.Append("</tr>");                       
                        srno++;
                    }

                    sbrb.Append("</table>");
                    sbrb.Append("</div>");
                    string bodyHtml = sbrb.ToString();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 100, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        CustomPdfPageEvent pageEventHelper = new CustomPdfPageEvent(companyName, reportName, reportDate, headerTable);
                        writer.PageEvent = pageEventHelper;
                        pdfDoc.Open();

                        // Write the body content
                        var bodyReader = new StringReader(bodyHtml);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, bodyReader);

                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "DCS_Summary.pdf");
                    }
                   

                }
                else
                {
                    return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                } 
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private PdfPCell CreateHeaderCell(string text)
        {
            return new PdfPCell(new Phrase(text, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)))
            {

                Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                //PaddingLeft = 5f,
                PaddingTop = 5f,
                PaddingBottom = 5f,
                PaddingRight = 35f
            };
        }

        private PdfPCell CreateCell(string text)
        {
            return new PdfPCell(new Phrase(text, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)))
            {
                Border = PdfPCell.NO_BORDER,
                //Border = PdfPCell.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingLeft = 35f,
                PaddingTop = 5f,
                PaddingBottom = 5f,
                PaddingRight = 2f
            };
        }

        public ActionResult ExportPrintDailySummaryRouteWise(string fromDate, string toDate)
        {
            try
            {
                string formattedFromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                string formatttedDateToDate = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
                string methodName = "PrintDailySummaryRouteWise";
                var responseData = clsDashBoard.GetDailySummary(Session["AppUserCode"].ToString(), formattedFromDate, formatttedDateToDate, methodName, Session["Port"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                byte[] k = null;
                StringBuilder sbr = new StringBuilder();
                sbr.Append("<table style='width: 100 %;'>");
                sbr.Append("<tr><td style='font: bold; font-size: 16px; text-align: center;'>" + (string)jsonArray[0]["Comp_Name"] + "</td></tr>");
                sbr.Append("<tr><td>" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "</td></tr>");
                sbr.Append("<tr><td style='text-align:center'>Daily Summary Route Wise</td></tr></table>");
                sbr.Append("<table style='width:100%; font-size:12px; border-collapse:collapse;'>");
                sbr.Append("<thead style='font:bold;'>");
                sbr.Append("<tr><th style='border-top: 1px dotted; border-bottom: 1px dotted'>DATE</th><th style='border-top: 1px dotted; border-bottom: 1px dotted'>ROUTE CODE</th><th style='border-top: 1px dotted; border-bottom: 1px dotted'>ROUTE NAME</th><th style='border-top: 1px dotted; border-bottom: 1px dotted'>QUANTITY</th><th style='border-top: 1px dotted; border-bottom: 1px dotted'>FAT%</th><th style='border-top: 1px dotted; border-bottom: 1px dotted'>SNF%</th></tr>");
                sbr.Append("</thead>");
                sbr.Append("<tbody>");
                foreach (var dr in jsonArray)
                {
                    sbr.Append("<tr>");
                    sbr.Append("<td style='padding:10px;'>" + dr["Doc_Date"].ToString() + "</td>");
                    sbr.Append("<td style='padding:10px;'>" + dr["ROUTE_CODE"].ToString() + "</td>");
                    sbr.Append("<td style='padding:10px;'>" + dr["route_name"].ToString() + "</td>");
                    sbr.Append("<td style='padding:10px;'>" + dr["Quantity"].ToString() + "</td>");
                    sbr.Append("<td style='padding:10px;'>" + dr["FATPer"].ToString() + "</td>");
                    sbr.Append("<td style='padding:10px;'>" + dr["SNFPer"].ToString() + "</td>");
                    sbr.Append("</tr>");
                }

                sbr.Append("</tbody>");
                sbr.Append("</table>");

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader sr = new StringReader(sbr.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    k = stream.ToArray();
                }
                return File(k.ToArray(), "application/pdf", "Grid.pdf");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}