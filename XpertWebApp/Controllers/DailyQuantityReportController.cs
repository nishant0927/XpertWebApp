using DAL.DashBoard;
using DAL.ListOfCowDCS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class DailyQuantityReportController : Controller
    {

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
                var responseData = await clsDashBoard.GetTankerMaster(Session["Port"].ToString());
                return Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetDailyQuantity(bool rbtnTranpoterGainLoss, bool rdbSummary, bool rbtnDock, bool rbtnDockDateWise, bool rbtnDockShiftWise, bool rbtnBmcSummary, bool rdbDetails, string fromDate, string toDate, string formId, string status, string fndArea, string mccCode, bool isPickCLRInsteadOfSNF, bool rbtnTranspoterGainlossSummary, decimal allowFat, decimal allowsnf, string tankerNo, bool rdbTankerWise, bool rbtnRouteWise, bool rdbCollectionWise, bool rdbMultiple, bool rbtnBMCTankerCollection)
        {

            try
            {
                var responseData = clsDashBoard.GetDailyQuantity(rbtnTranpoterGainLoss, rdbSummary, rbtnDock, rbtnDockDateWise, rbtnDockShiftWise, rbtnBmcSummary, rdbDetails, fromDate, toDate, formId, status, fndArea, mccCode, isPickCLRInsteadOfSNF, rbtnTranspoterGainlossSummary, allowFat, allowsnf, tankerNo, rdbTankerWise, rbtnRouteWise, rdbCollectionWise, rdbMultiple, rbtnBMCTankerCollection, Session["Port"].ToString());

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
                var jsonData = clsDashBoard.GetDailyQuantity(rbtnTranpoterGainLoss, rdbSummary, rbtnDock, rbtnDockDateWise, rbtnDockShiftWise, rbtnBmcSummary, rdbDetails, fromDate, toDate, formId, status, fndArea, mccCode, isPickCLRInsteadOfSNF, rbtnTranspoterGainlossSummary, allowFat, allowsnf, tankerNo, rdbTankerWise, rbtnRouteWise, rdbCollectionWise, rdbMultiple, rbtnBMCTankerCollection, Session["Port"].ToString());
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
                string companyName = Session["CompDesc"].ToString();
                string reportDate = "Date : " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
                string reportName = "Daily Summary Report";
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

                    fileContents = PdfGenerator.GeneratePdf(companyName, reportName, reportDate, rows, 85);
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
    }
}