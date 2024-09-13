using DAL.DashBoard;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class MCCMilkRegisterController : Controller
    {
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
                var responseData = clsDashBoard.GetMccMilkRegister(fromDate, todate, fromShift, toShift, cboSRNAmounType, mcc, area, route, dcs, chkDateShift, rbtnCollectionSummary, chkRejection, chkShiftWise, chkOnlyRejection, AreaWiseBilling, ChkDetailWise, rbtnVLCWise, chkRoutewise, ChkMCCWise, rbtnPlantWise, rbtnZoneWise, chkVLCWisePayable, rdbPlantWisePaymentSummary, rdoVLCWisePaymentSummary, chkDairyMilkReportPrint, chkRouteShiftWise, rbtnBMC, rbtnTotal, rbtnShiftWiseTotal, rbtnDCS, rbtnRoute, Session["CompCode"].ToString(), cboMilkReceiveUOM, PricePlan, chkShowVLCUploaderData, Session["CompCode"].ToString(), Session["AppUserCode"].ToString(), Session["Port"].ToString());
                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult ExportGetMCCMilkRegister(string fromDate, string todate, string fromShift, string toShift, string cboSRNAmounType, string mcc, string area, string route, string dcs, bool chkDateShift, bool rbtnCollectionSummary, bool chkRejection, bool chkShiftWise, bool chkOnlyRejection, bool AreaWiseBilling, bool ChkDetailWise, bool rbtnVLCWise, bool chkRoutewise, bool ChkMCCWise, bool rbtnPlantWise, bool rbtnZoneWise, bool chkVLCWisePayable, bool rdbPlantWisePaymentSummary, bool rdoVLCWisePaymentSummary, bool chkDairyMilkReportPrint, bool chkRouteShiftWise, bool rbtnBMC, bool rbtnTotal, bool rbtnShiftWiseTotal, bool rbtnDCS, bool rbtnRoute, string cboMilkReceiveUOM, int PricePlan, bool chkShowVLCUploaderData, bool pdf, bool excel)
        {
            try
            {

                var responseData = clsDashBoard.GetMccMilkRegister(fromDate, todate, fromShift, toShift, cboSRNAmounType, mcc, area, route, dcs, chkDateShift, rbtnCollectionSummary, chkRejection, chkShiftWise, chkOnlyRejection, AreaWiseBilling, ChkDetailWise, rbtnVLCWise, chkRoutewise, ChkMCCWise, rbtnPlantWise, rbtnZoneWise, chkVLCWisePayable, rdbPlantWisePaymentSummary, rdoVLCWisePaymentSummary, chkDairyMilkReportPrint, chkRouteShiftWise, rbtnBMC, rbtnTotal, rbtnShiftWiseTotal, rbtnDCS, rbtnRoute, Session["CompCode"].ToString(), cboMilkReceiveUOM, PricePlan, chkShowVLCUploaderData, Session["CompCode"].ToString(), Session["AppUserCode"].ToString(), Session["Port"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();
                string sheetName = "MCC Milk Register";
                arrHeader.Add("MCC Milk Register");
                arrHeader.Add("Date : " + fromDate + " To " + todate);
                string companyName = Session["CompDesc"].ToString();
                string reportDate = "Date : " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(todate).ToString("dd-MMM-yyyy");
                string reportName = "Daily Summary Report";
                if (jsonArray.Count > 0)
                {

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
                        fileContents = PdfGenerator.GeneratePdf(companyName, reportName, reportDate, rows, 125);
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