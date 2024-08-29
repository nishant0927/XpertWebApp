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

namespace XpertWebApp.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report

        //Dictionary<string, string> columnMap = new Dictionary<string, string>();
        //DataTable dt = new DataTable();
        public ActionResult ListOfCowDCS()
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

        public JsonResult GetMccCode()
        {
            string methodName = "GetMCCCode";
            string response = string.Empty;
            try
            {
                response = clsListCow.GetMCC(methodName, Session["Port"].ToString());
                return Json(new { response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListOfCowInDCS(string mccNo)
        {
            string methodName = "GetListOfCowDCS";
            string response = string.Empty;
            try
            {
                if (mccNo == "All")
                {
                    mccNo = string.Empty;
                }
                response = clsListCow.GetListCow(mccNo, methodName, Session["Port"].ToString());
                return Json(new { response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult DashBoard()
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
        public async System.Threading.Tasks.Task<JsonResult> GetAllData(string fromDate, string toDate)
        {
            string methodName = string.Empty;
            try
            {
                if (Session["AppUserCode"] != null)
                {
                    bool chkRJSBNS = false;
                    string currentUserCode = Session["AppUserCode"].ToString();
                    methodName = "GetUnionByData";
                    var getUnionWiseData = await clsDashBoard.GetDataAsync(methodName, fromDate, toDate, chkRJSBNS, currentUserCode, Session["Port"].ToString());
                    methodName = "GetRoutData";
                    var getRouteWiseData = await clsDashBoard.GetDataAsync(methodName, fromDate, toDate, chkRJSBNS, currentUserCode, Session["Port"].ToString());
                    methodName = "GetDCSData";
                    var getDCSWiseData = await clsDashBoard.GetDataAsync(methodName, fromDate, toDate, chkRJSBNS, currentUserCode, Session["Port"].ToString());
                    return Json(new { success = true, getUnionWiseData, getRouteWiseData, getDCSWiseData }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, redirect = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DBT_Dashboard()
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
        public JsonResult GetDBTDashBoardData(string fromDate, string toDate)
        {
            try
            {
                string currentUserCOde = Session["AppUserCode"].ToString();
                string code = Session["Port"].ToString();
                string methodName = string.Empty;
                methodName = "GetUnionWiseDBTSummary";
                var getUnionWiseDBTSummary = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCOde, methodName, code);
                methodName = "GetUnionWiseDBTMisMatchQty";
                var getUnionWiseDBTMisMatchQty = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCOde, methodName, code);
                methodName = "GetBTPaymentStatuses";
                var getDBTPaymentStatus = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCOde, methodName, code);
                methodName = "GetUnionWiseLastDBTJanAadhars";
                var getUnionWiseLastDBTJanAadhars = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCOde, methodName, code);
                return Json(new { success = true, getUnionWiseDBTSummary, getUnionWiseDBTMisMatchQty, getDBTPaymentStatus, getUnionWiseLastDBTJanAadhars });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GeneratePdfForDashboard_Milk_Procurement(string fromDate, string toDate, string tableId)
        {
            string methodName = string.Empty;
            bool chkRJSBNS = false;
            string currentUserCode = Session["AppUserCode"].ToString();
            DataTable dataTable = new DataTable();

            try
            {
                if (tableId == "tblUnionWise")
                {
                    methodName = "GetUnionByData";
                    List<clsUnion> lstUnion = clsUnion.GetUnionData(methodName, fromDate, toDate, chkRJSBNS, currentUserCode, Session["Port"].ToString());
                    dataTable = ConvertToDataTable(lstUnion);
                }
                else if (tableId == "tblRouteWise")
                {
                    methodName = "GetRoutData";
                    List<clsRout> lstRout = clsRout.GetRoutData(methodName, fromDate, toDate, chkRJSBNS, currentUserCode, Session["Port"].ToString());
                    dataTable = ConvertToDataTable(lstRout);
                }
                else if (tableId == "tblDCSWise")
                {
                    methodName = "GetDCSData";
                    List<clsDCS> lstDCS = clsDCS.GetDCSData(methodName, fromDate, toDate, chkRJSBNS, currentUserCode, Session["Port"].ToString());
                    dataTable = ConvertToDataTable(lstDCS);
                }

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                }

                Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Add header
                    string headerHtml = @"
                <table>
                    <tr>
                        <td>
                            <img src='" + Url.Content("~/Content/img/SARALog512.jpg") + @"' width='30' />
                        </td>
                        <td>
                            <h2>Microsoft Northwind Traders Company</h2>
                            <p>107, Park site,<br/>Salt Lake Road,<br/>Seattle, USA</p>
                        </td>
                    </tr>
                </table>
                <hr/>
            ";
                    AddHtmlToPdf(writer, document, headerHtml);

                    // Add table data
                    string tableHtml = DataTableToHtml(dataTable);
                    AddHtmlToPdf(writer, document, tableHtml);

                    document.Close();

                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    return File(bytes, "application/pdf", "Dashboard_Milk_Procurement.pdf");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private static string DataTableToHtml(DataTable dataTable)
        {
            string html = "<table border='1' cellpadding='5' cellspacing='0' style='width: 100%; border-collapse: collapse;'>";
            html += "<thead><tr>";

            // Add column headers
            foreach (DataColumn column in dataTable.Columns)
            {
                html += "<th style='background-color: #f2f2f2;'>" + column.ColumnName + "</th>";
            }
            html += "</tr></thead><tbody>";

            // Add rows
            foreach (DataRow row in dataTable.Rows)
            {
                html += "<tr>";
                foreach (DataColumn column in dataTable.Columns)
                {
                    html += "<td>" + row[column].ToString() + "</td>";
                }
                html += "</tr>";
            }

            html += "</tbody></table>";
            return html;
        }

        private static void AddHtmlToPdf(PdfWriter writer, Document document, string html)
        {
            try
            {
                using (var sr = new StringReader(html))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new Exception("Error parsing HTML to PDF: " + ex.Message);
            }
        }

        private DataTable ConvertToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            // Get all the properties
            var Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in Props)
            {
                // Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (var item in items)
            {
                var values = new object[Props.Length];
                for (var i = 0; i < Props.Length; i++)
                {
                    // Inserting property values to data table rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public ActionResult SalesAndMarkingDashboard()
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
        public JsonResult GetSalesAndMarketingData(string fromDate, string toDate)
        {
            try
            {
                string currentUserCOde = Session["AppUserCode"].ToString();
                string code = Session["Port"].ToString();
                string methodName = string.Empty;
                methodName = "GetUnionWiseDemand";
                var getUnionWiseDemand = clsDashBoard.GetSalesAndMarketingData(fromDate, toDate, currentUserCOde, methodName, code);
                methodName = "GetRouteAndBoothWiseDemand";
                var getRouteWiseAndBoothWiseData = clsDashBoard.GetSalesAndMarketingData(fromDate, toDate, currentUserCOde, methodName, code);
                //methodName = "GetItemWiseDetails";
                //var getItemWiseDetails = clsDashBoard.GetItemWiseDetails(fromDate,toDate,currentUserCOde,false,code,methodName);
                //if (getUnionWiseDemand != null || getRouteWiseAndBoothWiseData != null || getItemWiseDetails != null)
                //{
                return Json(new { success = true, getUnionWiseDemand, getRouteWiseAndBoothWiseData }, JsonRequestBehavior.AllowGet);
                //}

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JanaadharStatusReport()
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
        public JsonResult GetFarmerDetails()
        {
            try
            {
                //if (Session["AppUserCode"] != null)
                //{
                string code = Session["Port"].ToString();
                string methodName = "GetFarmerDetails";
                var farmerDetails = clsDashBoard.GetFarmerDetails(methodName, code);
                return Json(new { success = true, farmerDetails }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Home");
                //}

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUnionByFarmerDetails(string unionName, int start = 0, int length = 100)
        {
            try
            {
                string code = Session["Port"].ToString();
                string methodName = "GetUnionWiseFarmerDetail";

                var jsonString = clsDashBoard.GetUnionByFarmerDetails(methodName, code, unionName);
                List<FarmerDetail> unionWiseFarmerDetails = JsonConvert.DeserializeObject<List<FarmerDetail>>(jsonString);

                //dynamic unionWiseFarmerDetails = JArray.Parse(jsonString);

                // Apply pagination
                var pagedData = unionWiseFarmerDetails.Skip(start).Take(length).ToList();

                //var pagedData = unionWiseFarmerDetails.Skip(start).Take(length).ToList();
                //var result = new LargeJsonResult
                //{
                //    Data = new { success = true, unionWiseFarmerDetails },
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};
                return Json(new { success = true, data = pagedData, recordsTotal = unionWiseFarmerDetails.Count }, JsonRequestBehavior.AllowGet);
                //return result;
                //return Json(new { success = true, unionWiseFarmerDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUnionByFarmerDataInExcel(string unionName)
        {
            string code = Session["Port"].ToString();
            string methodName = "GetUnionWiseFarmerDetail";

            var jsonString = clsDashBoard.GetUnionByFarmerDetails(methodName, code, unionName);
            List<FarmerDetail> unionWiseFarmerDetails = JsonConvert.DeserializeObject<List<FarmerDetail>>(jsonString);
            try
            {
                string formattedFromDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");

                string dateRange = formattedFromDate;
                string name = "Union Wise Farmer Details";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(unionName);
                    worksheet.Cells.Style.Font.Name = "Calibri";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells["A1"].Value = "Date Range: " + dateRange;
                    worksheet.Cells["A2"].Value = "Name: " + name;

                    var headerRange = worksheet.Cells["A3:M3"];
                    headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    headerRange.Style.Border.Bottom.Style =
                        headerRange.Style.Border.Top.Style =
                        headerRange.Style.Border.Left.Style =
                        headerRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    worksheet.Cells[3, 1].Value = "Union Name";
                    worksheet.Cells[3, 2].Value = "MP Code";
                    worksheet.Cells[3, 3].Value = "MP Name";
                    worksheet.Cells[3, 4].Value = "Father Name";
                    worksheet.Cells[3, 5].Value = "DCS Code";
                    worksheet.Cells[3, 6].Value = "DCS Name";
                    worksheet.Cells[3, 7].Value = "Jan Aadhar No.Verified";
                    worksheet.Cells[3, 8].Value = "Jan Aadhar No.";
                    worksheet.Cells[3, 9].Value = "Aadhar No.";
                    worksheet.Cells[3, 10].Value = "Telphone";
                    worksheet.Cells[3, 11].Value = "Bank Name";
                    worksheet.Cells[3, 12].Value = "IFSC Code";
                    worksheet.Cells[3, 13].Value = "Account NO.";
                    string verified = string.Empty;
                    int row = 4;
                    foreach (var item in unionWiseFarmerDetails)
                    {
                        if (Convert.ToInt32(item.Jan_Aadhar_No_Verified) == 1)
                        {
                            verified = "Yes";
                        }
                        else
                        {
                            verified = "No";
                        }
                        worksheet.Cells[row, 1].Value = item.Union_Name;
                        worksheet.Cells[row, 2].Value = item.MP_Code;
                        worksheet.Cells[row, 3].Value = item.MP_Name;
                        worksheet.Cells[row, 4].Value = item.Father_Name;
                        worksheet.Cells[row, 5].Value = item.VLC_Code_VLC_Uploader;
                        worksheet.Cells[row, 6].Value = item.VLC_Name;
                        worksheet.Cells[row, 7].Value = verified;
                        worksheet.Cells[row, 8].Value = item.JA_janaadhaarId;
                        worksheet.Cells[row, 9].Value = item.JA_aadhar;
                        worksheet.Cells[row, 10].Value = item.Telphone;
                        worksheet.Cells[row, 11].Value = item.BankName;
                        worksheet.Cells[row, 12].Value = item.IFCICode;
                        //worksheet.Cells[row, 13].Value = item.AccountNO;
                        row++;
                    }
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    var allCellsRange = worksheet.Cells[worksheet.Dimension.Address];
                    allCellsRange.Style.Border.Bottom.Style =
                        allCellsRange.Style.Border.Top.Style =
                        allCellsRange.Style.Border.Left.Style =
                        allCellsRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    // Save the Excel package to a memory stream
                    MemoryStream memoryStream = new MemoryStream();
                    excelPackage.SaveAs(memoryStream);

                    // Convert the memory stream to a base64 string
                    string base64Excel = Convert.ToBase64String(memoryStream.ToArray());

                    // Return the base64 string in JSON response
                    return Json(new { success = true, fileContent = base64Excel, fileName = "Union_Wise_Farmer_Details.xlsx" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUnionByFarmerDataInPDF(string unionName)

        {
            try
            {
                string code = Session["Port"].ToString();
                string methodName = "GetUnionWiseFarmerDetail";

                var unionWiseFarmerDetails = clsDashBoard.GetUnionByFarmerDetails(methodName, code, unionName);
                //var unionWiseFarmerDetails = JArray.Parse(jsonString);
                var result = new LargeJsonResult
                {
                    Data = new { success = true, unionWiseFarmerDetails },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                return result;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }



        public ActionResult DailyQtyReport()
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

        public async Task<JsonResult> GetTanker()
        {
            try
            {
                var responseData = await clsDashBoard.GetTankerMaster();
                return Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDailyQuantity(bool rbtnTranpoterGainLoss, bool rdbSummary, bool rbtnDock, bool rbtnDockDateWise, bool rbtnDockShiftWise, bool rbtnBmcSummary, bool rdbDetails, string fromDate, string toDate, string formId, string status, string fndArea, string mccCode, bool isPickCLRInsteadOfSNF, bool rbtnTranspoterGainlossSummary, decimal allowFat, decimal allowsnf, string tankerNo, bool rdbTankerWise, bool rbtnRouteWise, bool rdbCollectionWise, bool rdbMultiple, bool rbtnBMCTankerCollection)
        {

            try
            {
                var responseData = clsDashBoard.GetDailyQuantity(rbtnTranpoterGainLoss, rdbSummary, rbtnDock, rbtnDockDateWise, rbtnDockShiftWise, rbtnBmcSummary, rdbDetails, fromDate, toDate, formId, status, fndArea, mccCode, isPickCLRInsteadOfSNF, rbtnTranspoterGainlossSummary, allowFat, allowsnf, tankerNo, rdbTankerWise, rbtnRouteWise, rdbCollectionWise, rdbMultiple, rbtnBMCTankerCollection);

                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DailyQuantityExportToExcel(bool rbtnTranpoterGainLoss, bool rdbSummary, bool rbtnDock, bool rbtnDockDateWise, bool rbtnDockShiftWise, bool rbtnBmcSummary, bool rdbDetails, string fromDate, string toDate, string formId, string status, string fndArea, string mccCode, bool isPickCLRInsteadOfSNF, bool rbtnTranspoterGainlossSummary, decimal allowFat, decimal allowsnf, string tankerNo, bool rdbTankerWise, bool rbtnRouteWise, bool rdbCollectionWise, bool rdbMultiple, bool rbtnBMCTankerCollection, bool pdf, bool excel)
        {
            try
            {
                var jsonData = clsDashBoard.GetDailyQuantity(rbtnTranpoterGainLoss, rdbSummary, rbtnDock, rbtnDockDateWise, rbtnDockShiftWise, rbtnBmcSummary, rdbDetails, fromDate, toDate, formId, status, fndArea, mccCode, isPickCLRInsteadOfSNF, rbtnTranspoterGainlossSummary, allowFat, allowsnf, tankerNo, rdbTankerWise, rbtnRouteWise, rdbCollectionWise, rdbMultiple, rbtnBMCTankerCollection);
                JArray jsonArray = JArray.Parse(jsonData);
                List<string> arrHeader = new List<string>();
                var rows = new List<List<(string ColumnName, object Value)>>();
                string sheetName = "Daily Qty Report";
                arrHeader.Add("Daily Qty Report");
                arrHeader.Add("Date : " + fromDate + " To " + toDate);
                if (rdbSummary)
                    arrHeader.Add("Report Type : " + "Summary");
                if (rdbDetails)
                    arrHeader.Add("Report Type : " + "Details");
                if (Common.CompairString(status, "Posted") == CompairStringResult.Equal)
                    arrHeader.Add("Document Status : " + "Posted");
                else if (Common.CompairString(status, "Unposted") == CompairStringResult.Equal)
                    arrHeader.Add("Document Status : " + "Unposted");
                else
                    arrHeader.Add("Document Status : " + "All");
                if (rbtnDock || rbtnDockDateWise || rbtnDockShiftWise)
                {
                    foreach (var dr in jsonArray)
                    {

                        var newRow = new List<(string ColumnName, object Value)>();
                        newRow.Add(("S.No.", dr["SNo"].ToString()));
                        newRow.Add(("Date", dr["SDate"].ToString()));
                        newRow.Add(("Shift", dr["Shift"].ToString()));
                        newRow.Add(("Can", dr["Qty"].ToString()));
                        newRow.Add(("Qty", dr["Milk_Wtd"].ToString()));
                        newRow.Add(("FAT %", dr["FAT"].ToString()));
                        newRow.Add(("SNF %", dr["SNF"].ToString()));
                        newRow.Add(("SNF KG", dr["SNF_KG"].ToString()));
                        newRow.Add(("FAT KG", dr["Fat_KG"].ToString()));
                        newRow.Add(("VLC Code", dr["DCS_Code"].ToString()));
                        newRow.Add(("DCS Uploader Code", dr["DCS_Uploader_Code"].ToString()));
                        newRow.Add(("BMC Uploader Code", dr["BMC_Uploader_Code"].ToString()));
                        newRow.Add(("Area", dr["Area"].ToString()));
                        newRow.Add(("Route No", dr["Route_Code"].ToString()));
                        rows.Add(newRow);
                    }
                }
                else if (rdbMultiple)
                {
                    foreach (var dr in jsonArray)
                    {
                        var row = new List<(string ColumnName, object Value)>();
                        row.Add(("Document No.", dr["Document_No"].ToString()));
                        row.Add(("Document Date", dr["Document_Date"].ToString()));
                        row.Add(("MCC Code", dr["MCC_Code"].ToString()));
                        row.Add(("MCC Name", dr["MCC_NAME"].ToString()));
                        row.Add(("Uploader No.", dr["Mcc_Code_VLC_Uploader"].ToString()));
                        row.Add(("Route Code", dr["Route_Code"]));
                        row.Add(("Route Description", dr["ROUTE_NAME"].ToString()));
                        row.Add(("Tanker No.", dr["Tanker_No"].ToString()));
                        row.Add(("Vehicle No.", dr["Vehicle_No"].ToString()));
                        row.Add(("Entered Qty", dr["Entered_Qty"].ToString()));
                        row.Add(("FATKG", dr["Entered_FATKg"].ToString()));
                        row.Add(("SNFKG", dr["Entered_SNFKg"].ToString()));
                        row.Add(("CLR", dr["CLR"].ToString()));
                        row.Add(("FAT", dr["FAT"].ToString()));
                        row.Add(("VLC Code", dr["VLC_Code"].ToString()));
                        row.Add(("VLCName", dr["VLC_Name"].ToString()));
                        row.Add(("Uploader No.", dr["VLC_Code_VLC_Uploader"].ToString()));
                        row.Add(("Qty", dr["Qty"].ToString()));
                        row.Add(("FATKG", dr["FATKG"].ToString()));
                        row.Add(("SNFKG", dr["SNFKG"].ToString()));
                        row.Add(("FAT%", dr["DCS_FAT"].ToString()));
                        row.Add(("SNF%", dr["DCS_SNF"].ToString()));
                        row.Add(("Qty", dr["Diff_Qty"].ToString()));
                        row.Add(("FATKG", dr["Diff_FAT"].ToString()));
                        row.Add(("SNFKG", dr["Diff_SNF"].ToString()));
                        rows.Add(row);

                    }

                }
                else if (rdbCollectionWise)
                {

                    foreach (var dr in jsonArray)
                    {
                        var row = new List<(string ColumnName, object Value)>();
                        row.Add(("Document No.", dr["Document_No"].ToString()));
                        row.Add(("Document Date", dr["Document_Date"].ToString()));
                        row.Add(("MCC Code", dr["MCC_Code"].ToString()));
                        row.Add(("MCC Name", dr["MCC_NAME"].ToString()));
                        row.Add(("Uploader No.", dr["Mcc_Code_VLC_Uploader"].ToString()));
                        row.Add(("Route Code", dr["Route_Code"].ToString()));
                        row.Add(("Route Description", dr["ROUTE_NAME"].ToString()));
                        row.Add(("Tanker No.", dr["Tanker_No"].ToString()));
                        row.Add(("Vehicle No.", dr["Vehicle_No"].ToString()));
                        row.Add(("Entered Qty", dr["Entered_Qty"].ToString()));
                        row.Add(("FATKG", dr["Entered_FATKg"].ToString()));
                        row.Add(("SNFKG", dr["Entered_SNFKg"].ToString()));
                        row.Add(("CLR", dr["CLR"].ToString()));
                        row.Add(("FAT", dr["DCS_FAT"].ToString()));
                        row.Add(("SNF%", dr["DCS_SNFPER"].ToString()));
                        row.Add(("VLC Code", dr["VLC_Code"].ToString()));
                        row.Add(("VLC Name", dr["VLC_Name"].ToString()));
                        row.Add(("Uploader No.", dr["VLC_Code_VLC_Uploader"].ToString()));
                        row.Add(("Collection Date", dr["Collection_Date"].ToString()));
                        row.Add(("Qty", dr["Qty"].ToString()));
                        row.Add(("FAT", dr["FAT"].ToString()));
                        row.Add(("CLR", dr["SNF"].ToString()));
                        row.Add(("FATKG", dr["FATKG"].ToString()));
                        row.Add(("SNFKG", dr["SNFKG"].ToString()));
                        row.Add(("SNF%", dr["SNFPER"].ToString()));
                        rows.Add(row);

                    }
                }

                else
                {
                    foreach (var dr in jsonArray)
                    {
                        var row = new List<(string ColumnName, object Value)>();
                        if (rdbTankerWise == false && rbtnBMCTankerCollection == false && rbtnRouteWise == false)
                        {
                            row.Add(("Document No.", dr["Document_No"].ToString()));
                            row.Add(("Document Date", dr["Document_Date"].ToString()));
                            row.Add(("Route Code", dr["Route_Code"].ToString()));
                            row.Add(("Route Name", dr["ROUTE_NAME"].ToString()));
                            row.Add(("Vehicle No", dr["Vehicle_No"].ToString()));

                        }
                        if (rbtnBMCTankerCollection == false && rbtnRouteWise == false)
                        {
                            row.Add(("Tanker No", dr["Tanker_No"].ToString()));
                        }
                        if (rdbDetails == true)
                        {
                            row.Add(("MCC Code", dr["MCC_Code"].ToString()));
                            row.Add(("MCC Name", dr["MCC_NAME"].ToString()));
                            row.Add(("Uploader No", dr["UploaderNo"].ToString()));
                        }
                        if (rbtnBmcSummary == true)
                        {
                            row.Add(("MCC Code", dr["MCC_Code"].ToString()));
                            row.Add(("MCC Name", dr["MCC_NAME"].ToString()));
                            row.Add(("Uploader No", dr["UploaderNo"].ToString()));
                        }
                        if (rdbSummary == true)
                        {
                            row.Add(("Qty", dr["Entered_Qty"].ToString()));
                            row.Add(("FAT %", dr["Entered_FAT"].ToString()));
                            row.Add(("SNF %", dr["Entered_SNF"].ToString()));
                            row.Add(("FAT KG", dr["Entered_FATKg"].ToString()));
                            row.Add(("SNF KG", dr["Entered_SNFKg"].ToString()));
                        }
                        if (rbtnTranpoterGainLoss || rbtnTranspoterGainlossSummary)
                        {
                            row.Add(("Qty", dr["Entered_Qty"].ToString()));
                            row.Add(("FAT KG", dr["Entered_FATKg"].ToString()));
                            row.Add(("SNF KG", dr["Entered_SNFKg"].ToString()));
                            row.Add(("FAT KG", dr["MCC_FATKG"].ToString()));
                            row.Add(("SNF KG", dr["MCC_SNFKG"].ToString()));
                            row.Add(("Qty", dr["MCC_Qty"].ToString()));
                        }
                        if (rbtnDock == true)
                        {
                            row.Add(("Qty", dr["Entered_Qty"].ToString()));
                            row.Add(("FAT KG", dr["Entered_FATKg"].ToString()));
                            row.Add(("SNF KG", dr["Entered_SNFKg"].ToString()));
                        }
                        if (rdbTankerWise)
                        {
                            row.Add(("Tankerno", dr["Tanker_No"].ToString()));
                            row.Add(("Quantity", dr["MCC_Qty"].ToString()));
                            row.Add(("KGFAT", dr["MCC_FATKG"].ToString()));
                            row.Add(("KGSNF", dr["MCC_SNFKG"].ToString()));
                            row.Add(("Quantity", dr["Entered_Qty"].ToString()));
                            row.Add(("KGFAT", dr["Entered_FATKg"].ToString()));
                            row.Add(("KGSNF", dr["Entered_SNFKg"].ToString()));
                            row.Add(("Quantity", dr["DiffEnteredVsMCC_Qty"].ToString()));
                            row.Add(("KGFAT", dr["DiffEnteredVsMCC_FAT"].ToString()));
                            row.Add(("KGSNF", dr["DiffEnteredVsMCC_SNF"].ToString()));
                        }
                        if (rbtnTranpoterGainLoss == false && rbtnTranspoterGainlossSummary == false && rdbTankerWise == false && rbtnBMCTankerCollection == false && rbtnRouteWise == false)
                        {
                            row.Add(("Qty", dr["MCC_Qty"].ToString()));
                            row.Add(("FAT %", dr["MCC_FAT"].ToString()));
                            row.Add(("SNF %", dr["MCC_SNF"].ToString()));
                            row.Add(("FAT KG", dr["MCC_FATKG"].ToString()));
                            row.Add(("SNF KG", dr["MCC_SNFKG"].ToString()));
                            row.Add(("FAT KG", dr["DCS_FATKG"].ToString()));
                            row.Add(("SNF KG", dr["DCS_SNFKG"].ToString()));
                            row.Add(("FAT KG", dr["Diff_FATKG"].ToString()));
                            row.Add(("SNF KG", dr["Diff_SNFKG"].ToString()));
                            row.Add(("Qty", dr["DCS_Qty"].ToString()));
                            row.Add(("FAT %", dr["DCS_FAT"].ToString()));
                            row.Add(("SNF %", dr["DCS_SNF"].ToString()));
                            row.Add(("Qty", dr["Diff_Qty"].ToString()));
                            row.Add(("FAT %", dr["Diff_FAT"].ToString()));
                            row.Add(("SNF %", dr["Diff_SNF"].ToString()));

                        }
                        if (rbtnTranpoterGainLoss == true || rbtnTranspoterGainlossSummary == true)
                        {
                            row.Add(("Qty", dr["DiffMCCVsEntered_Qty"].ToString()));
                            row.Add(("FAT Difference", dr["DiffMCCVsEntered_FATKG"].ToString()));
                            row.Add(("FAT KG to be Recovered", dr["FATKG_Recovered"].ToString()));
                            row.Add(("SNF Difference", dr["DiffMCCVsEntered_SNFKG"].ToString()));
                            row.Add(("SNF KG to be Recovered", dr["SNFKG_Recovered"].ToString()));
                            row.Add(("Allow FAT Tolerence", dr["FAT_Tolerence"].ToString()));
                            row.Add(("FAT Amount Recovered", dr["FAT_AMT"].ToString()));
                            row.Add(("Allow SNF Tolerence", dr["SNF_Tolerence"].ToString()));
                            row.Add(("SNF Amount Recovered", dr["SNF_AMT"].ToString()));
                            row.Add(("AMOUNT", dr["AMOUNT"]));
                        }
                        if (rdbSummary == true)
                        {
                            row.Add(("Qty", dr["DiffEnteredVsMCC_Qty"].ToString()));
                            row.Add(("FAT %", dr["DiffEnteredVsMCC_FAT"].ToString()));
                            row.Add(("SNF %", dr["DiffEnteredVsMCC_SNF"].ToString()));
                            row.Add(("FAT KG", dr["DiffEnteredVsMCC_FATKG"].ToString()));
                            row.Add(("SNF KG", dr["DiffEnteredVsMCC_SNFKG"].ToString()));
                        }
                        if (rbtnRouteWise == true)
                        {
                            row.Add(("Qty", dr["ROUTE_NO"].ToString()));
                            row.Add(("Route Name", dr["ROUTE_NAME"].ToString()));
                            row.Add(("Quantity", dr["MCC_Qty"].ToString()));
                            row.Add(("%FAT", dr["MCC_FAT_Per"].ToString()));
                            row.Add(("%SNF", dr["MCC_SNF_Per"].ToString()));
                            row.Add(("KGFAT", dr["MCC_FATKG"].ToString()));
                            row.Add(("KGSNF", dr["MCC_SNFKG"].ToString()));
                            row.Add(("Quantity", dr["Entered_Qty"].ToString()));
                            row.Add(("%FAT", dr["Entered_FAT_Per"].ToString()));
                            row.Add(("%SNF", dr["Entered_SNF_Per"].ToString()));
                            row.Add(("KGFAT", dr["Entered_FATKg"].ToString()));
                            row.Add(("KGSNF", dr["Entered_SNFKg"].ToString()));
                            row.Add(("Quantity", dr["DiffEnteredVsMCC_Qty"].ToString()));
                            row.Add(("KGFAT", dr["DiffEnteredVsMCC_FAT"].ToString()));
                            row.Add(("KGSNF", dr["DiffEnteredVsMCC_SNF"].ToString()));
                        }
                        rows.Add(row);
                    }

                }
                byte[] fileContents = new byte[0];
                string fileype = string.Empty;
                string fileName = string.Empty;
                if (excel == true)
                {
                    fileContents = ExcelExportHelper.ExportDataToExcel(rows, sheetName, arrHeader);
                    fileype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = "DailyQtyReport.xlsx";
                    //return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DailyQtyReport.xlsx");
                }
                else if (pdf == true)
                {

                    fileContents = PdfGenerator.GeneratePdfWithHeadersAndColumns(arrHeader, rows);
                    fileype = "application/pdf";
                    fileName = "DailyQtyReport.pdf";
                    //return File(pdfBytes, "application/pdf", "DailyQtyReport.pdf");
                }
                return File(fileContents, fileype, fileName);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult PaymentCycleWiseReport()
        {
            if (Session["AppUserCode"] != null)
            {
                //string CurrentUser = Session["AppUserCode"].ToString();
                //string Pwd = Session["AppPwd"].ToString();
                //var data = clsDashBoard.CheckLoginDetails(CurrentUser, Pwd);
                //JArray jsonArray = JArray.Parse(data);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public JsonResult GetPaymentCycleWiseData(string fromDate, string toDate, bool hold, bool unhold, bool all, bool showData, bool outStanding, bool headLoad, string mcc, string route, string vlc, string zone, string bank, bool paymentsummary, string paymentCycleCode)
        {
            try
            {
                var responseData = clsDashBoard.GetPaymentCycle(fromDate, toDate, mcc, route, vlc, zone, bank, hold, unhold, all, showData, outStanding, headLoad, paymentsummary, paymentCycleCode, Session["CompCode"].ToString(), Session["CompCode"].ToString(), Session["CompDesc"].ToString(), Session["AppUserCode"].ToString());
                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult ExportGetPaymentCycleWiseData(string fromDate, string toDate, bool hold, bool unhold, bool all, bool showData, bool outStanding, bool headLoad, string mcc, string route, string vlc, string zone, string bank, bool paymentsummary, string paymentCycleCode,bool pdf,bool excel)
        {
            try
            {
                var responseData = clsDashBoard.GetPaymentCycle(fromDate, toDate, mcc, route, vlc, zone, bank, hold, unhold, all, showData, outStanding, headLoad, paymentsummary, paymentCycleCode, Session["CompCode"].ToString(), Session["CompCode"].ToString(), Session["CompDesc"].ToString(), Session["AppUserCode"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();               
                string sheetName = "Payment Cycle Wise Report";
                arrHeader.Add("Payment Cycle Wise Report");
                arrHeader.Add("Date : " + fromDate + " To " + toDate);
                if (jsonArray.Count > 0)
                {
                    // Extract columns excluding "Result"
                    var columns = jsonArray[0].Children<JProperty>()
                                              .Select(jp => jp.Name)
                                              .Where(key => key != "Result")
                                              .ToList();
                    var rows = jsonArray.Select
                        (item => columns.Select
                         (column =>(ColumnName: column, Value: (object)(item[column] != null ? item[column].ToString() : string.Empty))).ToList()).ToList();

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
                        fileContents = PdfGenerator.GeneratePdfWithHeadersAndColumns(arrHeader, rows);
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

        public ActionResult ExportRoutePaymentProcessReport(string fromDate,string toDate)
        {
            try
            {
                var responseData = clsDashBoard.GetLedgerData(Session["CompCode"].ToString(), fromDate, toDate, Session["CompCode"].ToString(), Session["AppUserCode"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();
               // string sheetName = "Payment Cycle Wise Report";
                arrHeader.Add("Le");
                arrHeader.Add("Date : " + fromDate + " To " + toDate);
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
                   
                        fileContents = PdfGenerator.GeneratePdfWithHeadersAndColumns(arrHeader, rows);
                        fileType = "application/pdf";
                        fileName = "PaymentCycleWiseReport.pdf";
                   

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


        public ActionResult ExportDCSSummary(string mont)
        {

        }

        public ActionResult MCCMilkRegister()
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


        public JsonResult GetMCCMilkRegister(string fromDate, string todate, string fromShift, string toShift, string cboSRNAmounType, string mcc, string area, string route, string dcs, bool chkDateShift, bool rbtnCollectionSummary, bool chkRejection, bool chkShiftWise, bool chkOnlyRejection, bool AreaWiseBilling, bool ChkDetailWise, bool rbtnVLCWise, bool chkRoutewise, bool ChkMCCWise, bool rbtnPlantWise, bool rbtnZoneWise, bool chkVLCWisePayable, bool rdbPlantWisePaymentSummary, bool rdoVLCWisePaymentSummary, bool chkDairyMilkReportPrint, bool chkRouteShiftWise, bool rbtnBMC, bool rbtnTotal, bool rbtnShiftWiseTotal, bool rbtnDCS, bool rbtnRoute, string cboMilkReceiveUOM, int PricePlan, bool chkShowVLCUploaderData)
        {
            try
            {
                var responseData = clsDashBoard.GetMccMilkRegister(fromDate, todate, fromShift, toShift, cboSRNAmounType, mcc, area, route, dcs, chkDateShift, rbtnCollectionSummary, chkRejection, chkShiftWise, chkOnlyRejection, AreaWiseBilling, ChkDetailWise, rbtnVLCWise, chkRoutewise, ChkMCCWise, rbtnPlantWise, rbtnZoneWise, chkVLCWisePayable, rdbPlantWisePaymentSummary, rdoVLCWisePaymentSummary, chkDairyMilkReportPrint, chkRouteShiftWise, rbtnBMC, rbtnTotal, rbtnShiftWiseTotal, rbtnDCS, rbtnRoute, Session["CompCode"].ToString(), cboMilkReceiveUOM, PricePlan, chkShowVLCUploaderData, Session["CompCode"].ToString(), Session["AppUserCode"].ToString());
                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult ExportGetMCCMilkRegister(string fromDate, string todate, string fromShift, string toShift, string cboSRNAmounType, string mcc, string area, string route, string dcs, bool chkDateShift, bool rbtnCollectionSummary, bool chkRejection, bool chkShiftWise, bool chkOnlyRejection, bool AreaWiseBilling, bool ChkDetailWise, bool rbtnVLCWise, bool chkRoutewise, bool ChkMCCWise, bool rbtnPlantWise, bool rbtnZoneWise, bool chkVLCWisePayable, bool rdbPlantWisePaymentSummary, bool rdoVLCWisePaymentSummary, bool chkDairyMilkReportPrint, bool chkRouteShiftWise, bool rbtnBMC, bool rbtnTotal, bool rbtnShiftWiseTotal, bool rbtnDCS, bool rbtnRoute, string cboMilkReceiveUOM, int PricePlan, bool chkShowVLCUploaderData,bool pdf,bool excel)
        {
            try
            {
                var responseData = clsDashBoard.GetMccMilkRegister(fromDate, todate, fromShift, toShift, cboSRNAmounType, mcc, area, route, dcs, chkDateShift, rbtnCollectionSummary, chkRejection, chkShiftWise, chkOnlyRejection, AreaWiseBilling, ChkDetailWise, rbtnVLCWise, chkRoutewise, ChkMCCWise, rbtnPlantWise, rbtnZoneWise, chkVLCWisePayable, rdbPlantWisePaymentSummary, rdoVLCWisePaymentSummary, chkDairyMilkReportPrint, chkRouteShiftWise, rbtnBMC, rbtnTotal, rbtnShiftWiseTotal, rbtnDCS, rbtnRoute, Session["CompCode"].ToString(), cboMilkReceiveUOM, PricePlan, chkShowVLCUploaderData, Session["CompCode"].ToString(), Session["AppUserCode"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();
                string sheetName = "MCC Milk Register";
                arrHeader.Add("MCC Milk Register");
                arrHeader.Add("Date : " + fromDate + " To " + todate);
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
                        fileName = "MCCMilkRegister.xlsx";
                    }
                    else if (pdf == true)
                    {
                        fileContents = PdfGenerator.GeneratePdfWithHeadersAndColumns(arrHeader, rows);
                        fileType = "application/pdf";
                        fileName = "MCCMilkRegister.pdf";
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

    }
}