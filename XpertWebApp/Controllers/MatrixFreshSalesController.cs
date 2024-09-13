using DAL.DashBoard;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class MatrixFreshSalesController : Controller
    {
        public ActionResult MatrixFreshSalesReport()
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
        public JsonResult GetMatrixFreshSalesReport(bool chkMilkPouch, bool rbtnAsPerBooking, string ddlReportType, string fromDate, string ToDate, bool chkFirstAndSecondSpellAbstract, bool chkBookingWise, bool chkSaleInvoiceWise, bool chkSummary, bool chkFirstAndSecondSpell, bool chkRouteBoothWise, bool ChkDayWiseSummary, bool chkFilterByCreatedDate, bool chkGatePass, string txtCustomerGroup, string txtCustomer, string TxtMultiCustomerCategory, string txtItemCode, string txtLocation, string txtZone, string txtLorry, string TxtRoute, string TxtUOM, string txtBookingType, string cboShift, bool rbtnMrng, bool rbtnEvng, bool rbtnBoths, string txtfndCustomer, string txtFndRoute, string ddlTCSShift, bool chkProduct, bool chkRouteSummary, string ddlInvocieType, bool isSchemeItem, bool rdbLtr, bool rbtnDateWise, bool rdbCreate)
        {
            try
            {
                var responseData = clsDashBoard.GetMatrixFreshSalesReport(chkMilkPouch, rbtnAsPerBooking, ddlReportType, fromDate, ToDate, chkFirstAndSecondSpellAbstract, chkBookingWise, chkSaleInvoiceWise, chkSummary, chkFirstAndSecondSpell, chkRouteBoothWise, ChkDayWiseSummary, chkFilterByCreatedDate, chkGatePass, txtCustomerGroup, txtCustomer, TxtMultiCustomerCategory, txtItemCode, txtLocation, txtZone, txtLorry, TxtRoute, TxtUOM, txtBookingType, Session["AppUserCode"].ToString(), cboShift, rbtnMrng, rbtnEvng, rbtnBoths, txtfndCustomer, txtFndRoute, ddlTCSShift, chkProduct, chkRouteSummary, ddlInvocieType, isSchemeItem, rdbLtr, rbtnDateWise, rdbCreate, Session["Port"].ToString());

                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportMatrixFreshSalesReport(bool chkMilkPouch, bool rbtnAsPerBooking, string ddlReportType, string fromDate, string ToDate, bool chkFirstAndSecondSpellAbstract, bool chkBookingWise, bool chkSaleInvoiceWise, bool chkSummary, bool chkFirstAndSecondSpell, bool chkRouteBoothWise, bool ChkDayWiseSummary, bool chkFilterByCreatedDate, bool chkGatePass, string txtCustomerGroup, string txtCustomer, string TxtMultiCustomerCategory, string txtItemCode, string txtLocation, string txtZone, string txtLorry, string TxtRoute, string TxtUOM, string txtBookingType, string cboShift, bool rbtnMrng, bool rbtnEvng, bool rbtnBoths, string txtfndCustomer, string txtFndRoute, string ddlTCSShift, bool chkProduct, bool chkRouteSummary, string ddlInvocieType, bool isSchemeItem, bool rdbLtr, bool rbtnDateWise, bool rdbCreate,bool excel,bool pdf)
        {
            try
            {
                var responseData = clsDashBoard.GetMatrixFreshSalesReport(chkMilkPouch, rbtnAsPerBooking, ddlReportType, fromDate, ToDate, chkFirstAndSecondSpellAbstract, chkBookingWise, chkSaleInvoiceWise, chkSummary, chkFirstAndSecondSpell, chkRouteBoothWise, ChkDayWiseSummary, chkFilterByCreatedDate, chkGatePass, txtCustomerGroup, txtCustomer, TxtMultiCustomerCategory, txtItemCode, txtLocation, txtZone, txtLorry, TxtRoute, TxtUOM, txtBookingType, Session["AppUserCode"].ToString(), cboShift, rbtnMrng, rbtnEvng, rbtnBoths, txtfndCustomer, txtFndRoute, ddlTCSShift, chkProduct, chkRouteSummary, ddlInvocieType, isSchemeItem, rdbLtr, rbtnDateWise, rdbCreate, Session["Port"].ToString());

                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();
                string sheetName = "Payment Cycle Wise Report";
                arrHeader.Add("Payment Cycle Wise Report");
                //arrHeader.Add("Date : " + fromDate + " To " + toDate);
                string companyName = Session["CompDesc"].ToString();
                string reportDate = "Date : " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(ToDate).ToString("dd-MMM-yyyy");
                string reportName = "Daily Summary Report";
                if (jsonArray.Count > 0)
                {
                    // Extract columns excluding "Result"
                    var columns = jsonArray[0].Children<JProperty>()
                                              .Select(jp => jp.Name)
                                              .Where(key => key != "Result")
                                              .ToList();
                    var rows = jsonArray.Select
                        (item => columns.Select
                         (column => (ColumnName: column, Value: (object)(item[column] != null ? item[column].ToString() : string.Empty))).ToList()).ToList();

                    byte[] fileContents = new byte[0];
                    string fileType = string.Empty;
                    string fileName = string.Empty;

                    // Check if Excel export is requested
                    if (excel == true)
                    {
                        fileContents = ExcelExportHelper.ExportDataToExcel(rows, sheetName, arrHeader);
                        fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        fileName = "PaymentCycleWiseReport.xlsx";
                    }
                    else if (pdf == true)
                    {
                        fileContents = PdfGenerator.GeneratePdf(companyName, reportName, reportDate, rows, 85);
                        fileType = "application/pdf";
                        fileName = "PaymentCycleWiseReport.pdf";
                    }

                    return File(fileContents, fileType, fileName);
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


        public async Task<ActionResult> ExportTruckSheetData(string DocDate, string strShift, string route, bool IsFreshItem, bool IsAmbientItem, bool IsIndividualCustomer)
        {
            try
            {
                string companyName = Session["CompDesc"].ToString();
                var responseData =await clsDashBoard.GetTruckSheetData(DocDate, strShift, route, Session["CompCode"].ToString(), IsFreshItem, IsAmbientItem, IsIndividualCustomer);
                JArray jsonArray = JArray.Parse(responseData);
                var groupedRecords = jsonArray.GroupBy(record => (string)record["Credit_Customer"]);
                if (jsonArray.Count > 0)
                {
                    var routes = jsonArray.Select(r => (string)r["Route_No"]).Distinct();
                    StringBuilder sbr = new StringBuilder();
                    foreach(var routeNo in routes)
                    {
                        var routeRecords = jsonArray.Where(r => (string)r["Route_No"] == routeNo);

                        sbr.Append("<div style='text-align: center; font-weight:bold; margin-bottom:20px; font-size:12px;'>");
                        sbr.Append($"{companyName}<br />");
                        sbr.Append($"DAILY TENTATIVE DEMAND SHEET FOR AREA NO: {routeNo} Date: {routeRecords.First()["Document_Date"]} Shift: {strShift} Status: {routeRecords.First()["DocStatus"]}<br />");
                        sbr.Append($"Route: {routeRecords.First()["Route_Desc"]} TRANSPORTER: {routeRecords.First()["TranspoterName"]} DRIVER: {routeRecords.First()["DriverName"]} VEHICLE NO: {routeRecords.First()["Vehicle_No"]}<br />");
                        sbr.Append($"Page No [ 1 ]</div>");
                        //AddTableHeader(sbr);
                        bool headerPrinted = false;
                        //bool headerPrinted = false;

                        AddCustomerTypeDetails(sbr, routeRecords.Where(r => (string)r["Credit_Customer"] == "Normal Booth"), "Normal Booth", ref headerPrinted);
                        AddCustomerTypeDetails(sbr, routeRecords.Where(r => (string)r["Credit_Customer"] == "Department Booth"), "Department Booth", ref headerPrinted);
                        AddCustomerTypeDetails(sbr, routeRecords.Where(r => (string)r["Credit_Customer"] == "Grand Total"), "Grand Total", ref headerPrinted);

                        // AddCustomerTypeDetails(sbr, routeRecords.Where(r => (string)r["Credit_Customer"] == "Normal Booth"), "Normal Booth", ref headerPrinted);                       
                        // var otherTypes = routeRecords.Select(r => (string)r["Credit_Customer"]).Distinct().Where(t => t != "Normal Booth" && t != "Grand Total");
                        //foreach (var type in otherTypes)
                        //{
                        //    AddCustomerTypeDetails(sbr, routeRecords.Where(r => (string)r["Credit_Customer"] == type), type, ref headerPrinted);
                        //}                      
                        //AddCustomerTypeDetails(sbr, routeRecords.Where(r => (string)r["Credit_Customer"] == "Grand Total"), "Grand Total", ref headerPrinted);
                        string html = sbr.ToString();
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();                       
                        var bodyReader = new StringReader(sbr.ToString());
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, bodyReader);
                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "TruckSheetReport.pdf");
                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                }
              
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> GetTruckSheetSummaryRouteWise(string txtPTSDateFrom, string ddlPTSShift, string txtCustMultFnd, string txtMultPTSRoute, bool rbtnMilk, bool rbtnProduct, string CurrComp_Code1)
        {
            try
            {
                var responseData =await clsDashBoard.GetTruckSheetSummaryRouteWise(txtPTSDateFrom, ddlPTSShift, txtCustMultFnd, txtMultPTSRoute, rbtnMilk, rbtnProduct, Session["CompCode"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                if (jsonArray.Count > 0)
                {
                    var tableHeader = jsonArray.Select(item => new
                    {
                        sorfDec = item["Short_Description"].ToString()
                    }).Distinct().ToList();

                    // Group by Route_No (remove ToList() here)
                    var groupedData = jsonArray.GroupBy(item => item["Route_No"].ToString());

                    StringBuilder sbr = new StringBuilder();
                    sbr.Append("<table style='width:100%; border-collapse:collapse;font-size:10px;'>");

                    // Add table headers
                    sbr.Append("<thead><tr><th>Route No</th>");
                    foreach (var header in tableHeader)
                    {
                        sbr.Append("<th>" + header.sorfDec + "</th>");
                    }
                    sbr.Append("<th>Total | Supply | Total Crate | Amount</th>");
                    sbr.Append("</tr></thead>");

                    // Iterate over each group of Route_No
                    foreach (var group in groupedData)
                    {
                        sbr.Append("<tbody><tr>");
                        // Display Route_No
                        sbr.Append("<td>" + group.Key + "</td>"); // Route_No

                        // Iterate over the dynamic headers (Short_Description) and find matches
                        foreach (var header in tableHeader)
                        {
                            // Check if there's a matching record with this Short_Description for this Route_No
                            var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);

                            if (matchingRecord != null)
                            {
                                // Add QTYLtr value if a match is found
                                sbr.Append("<td>" + matchingRecord["QTYLtr"].ToString() + "</td>");
                            }
                            else
                            {
                                // No matching record for this header, so add 0 in the cell
                                sbr.Append("<td>0</td>");
                            }
                        }

                        // If you want to add totals (Supply, Crate, Amount) here for each Route
                        var totalSupply = group.Sum(item => (decimal)item["QTYLtr"]);
                        var totalCrate = group.Sum(item => (decimal)item["Crate"]);
                        var totalAmount = group.Sum(item => (decimal)item["ItemNetAmount"]);

                        // Add totals for the row
                        sbr.Append("<td>" + totalSupply + " | " + totalCrate + " | " + totalAmount + "</td>");

                        sbr.Append("</tr></tbody>"); // End of the row for this Route_No
                    }

                    sbr.Append("</table>"); // End of the table

                    // The final HTML string with the table
                    string tableHtml = sbr.ToString();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        var bodyReader = new StringReader(sbr.ToString());
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, bodyReader);
                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "TruckSheetReport.pdf");
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


        private void AddTableHeader(StringBuilder sbr)
        {

            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>Booth</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>TM 500ML</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>DTM 500 ML</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>SM 500 ML</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>FCM 500 ML</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>DTM 200 ML</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>TM 1Ltr</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>CS 1 LTR</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>TM 6Ltr</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>DTM 6L</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>FCM 6 LTR</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>Total Crate</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>Item Net Amount</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>Amount BE</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>Total TCS Amt</th>");
            sbr.Append("<th style='width: 6%; text-align:center;border-top: 1px dashed black; border-bottom:1px dashed black;'>Total Collect Crate</th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
        }
        private void AddCustomerTypeDetails(StringBuilder sbr, IEnumerable<JToken> records, string customerType, ref bool headerPrinted)
        {
            if (records.Any())
            {
                sbr.Append($"<div style='font-weight: bold; text-align: left; padding: 10px; margin-top:20px; font-size:12px;'>Details of : {customerType}</div>");
                //sbr.Append("<table style='width: 100%; border-collapse:collapse;font-size:12px'>");
                // Only print the table header once
                if (!headerPrinted)
                {
                    sbr.Append("<table style='width: 100%; border-collapse: collapse; font-size: 12px;'>");
                    AddTableHeader(sbr);  // Print table headers only once
                    headerPrinted = true;
                }
                else
                {
                    sbr.Append("<table style='width: 100%; border-collapse: collapse; font-size: 12px;'>");
                    // Add an empty row to ensure alignment across sections
                    sbr.Append("<thead>");
                    sbr.Append("<tr>");
                    sbr.Append("<th colspan='16' style='height: 5px;'>&nbsp;</th>"); // Empty header for spacing/alignment
                    sbr.Append("</tr>");
                    sbr.Append("</thead>");
                }
                // Add Table Header
                //AddTableHeader(sbr);

                sbr.Append("<tbody style='text-align:center; margin-top:2px;'>");
                decimal totalTM500ML = 0, totalDTM500ML = 0, totalSM500ML = 0, totalFCM500ML = 0;
                decimal totalDTM200ML = 0, totalTM1Ltr = 0, totalCS1LTR = 0, totalTM6Ltr = 0;
                decimal totalDTM6L = 0, totalFCM6LTR = 0, totalCrate = 0, totalNetAmount = 0;
                decimal totalAmountBE = 0, totalTCSAmt = 0, totalCollectCrate = 0;
                // Add Table Rows
                foreach (var record in records)
                {
                    sbr.Append("<tr>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["Cust_Code"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["TM 500ML"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["DTM 500 Ml"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["SM 500 ML"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["FCM 500 ML"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["DTM 200 Ml"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["TM 1Ltr"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["CS 1 LTR"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["TM 6Ltr"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["DTM 6L"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["FCM 6 LTR"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["TotalCrate"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["ItemNetAmount"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["AmountBE"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["TotalTCSAmt"]}</td>");
                    sbr.Append($"<td style='width: 6%;padding: 10px;'>{record["TotalCollectCrate"]}</td>");
                    sbr.Append("</tr>");
                    if (customerType == "Normal Booth" || customerType == "Department Booth")
                    {
                        totalTM500ML += Convert.ToDecimal(record["TM 500ML"]);
                        totalDTM500ML += Convert.ToDecimal(record["DTM 500 Ml"]);
                        totalSM500ML += Convert.ToDecimal(record["SM 500 ML"]);
                        totalFCM500ML += Convert.ToDecimal(record["FCM 500 ML"]);
                        totalDTM200ML += Convert.ToDecimal(record["DTM 200 Ml"]);
                        totalTM1Ltr += Convert.ToDecimal(record["TM 1Ltr"]);
                        totalCS1LTR += Convert.ToDecimal(record["CS 1 LTR"]);
                        totalTM6Ltr += Convert.ToDecimal(record["TM 6Ltr"]);
                        totalDTM6L += Convert.ToDecimal(record["DTM 6L"]);
                        totalFCM6LTR += Convert.ToDecimal(record["FCM 6 LTR"]);
                        totalCrate += Convert.ToDecimal(record["TotalCrate"]);
                        totalNetAmount += Convert.ToDecimal(record["ItemNetAmount"]);
                        totalAmountBE += Convert.ToDecimal(record["AmountBE"]);
                        totalTCSAmt += Convert.ToDecimal(record["TotalTCSAmt"]);
                        totalCollectCrate += Convert.ToDecimal(record["TotalCollectCrate"]);
                    }
                    //sbr.Append("<tr>");
                    //sbr.Append($"<td>{record["Cust_Code"]}</td>");
                    //sbr.Append($"<td>{record["TM 500ML"]}</td>");
                    //sbr.Append($"<td>{record["DTM 500 Ml"]}</td>");
                    //sbr.Append($"<td>{record["SM 500 ML"]}</td>");
                    //sbr.Append($"<td>{record["FCM 500 ML"]}</td>");
                    //sbr.Append($"<td>{record["DTM 200 Ml"]}</td>");
                    //sbr.Append($"<td>{record["TM 1Ltr"]}</td>");
                    //sbr.Append($"<td>{record["CS 1 LTR"]}</td>");
                    //sbr.Append($"<td>{record["TM 6Ltr"]}</td>");
                    //sbr.Append($"<td>{record["DTM 6L"]}</td>");
                    //sbr.Append($"<td>{record["FCM 6 LTR"]}</td>");
                    //sbr.Append($"<td>{record["TotalCrate"]}</td>");
                    //sbr.Append($"<td>{record["ItemNetAmount"]}</td>");
                    //sbr.Append($"<td>{record["AmountBE"]}</td>");
                    //sbr.Append($"<td>{record["TotalTCSAmt"]}</td>");
                    //sbr.Append($"<td>{record["TotalCollectCrate"]}</td>");
                    //sbr.Append("</tr>");
                }

                sbr.Append("</tbody>");
                if (customerType == "Normal Booth" || customerType == "Department Booth")
                {
                    sbr.Append("<tfoot style='text-align:center; margin-top:2px;'>");
                    sbr.Append("<tr style='font-weight: bold;'>");
                    sbr.Append("<td  style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'></td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalTM500ML}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalDTM500ML}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalSM500ML}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalFCM500ML}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalDTM200ML}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalTM1Ltr}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalCS1LTR}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalTM6Ltr}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalDTM6L}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalFCM6LTR}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalCrate}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalNetAmount}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalAmountBE}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;'>{totalTCSAmt}</td>");
                    sbr.Append($"<td style='border-top: 1px dashed black; border-bottom:1px dashed black;width: 6%;padding: 10px;'>{totalCollectCrate}</td>");
                    sbr.Append("</tr>");
                    sbr.Append("</tfoot>");
                }
                sbr.Append("</table>");
                sbr.Append("<br/><br/>");
            }
        }

    }
}