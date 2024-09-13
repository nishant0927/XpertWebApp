using DAL.DashBoard;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class PaymentCycleWiseReportController : Controller
    {
        public ActionResult PaymentCycleWiseReport()
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

        public JsonResult GetPaymentCycleWiseData(string fromDate, string toDate, bool hold, bool unhold, bool all, bool showData, bool outStanding, bool headLoad, string mcc, string route, string vlc, string zone, string bank, bool paymentsummary, string paymentCycleCode)
        {
            try
            {
                string formattedFromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                string formatttedDateToDate = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");


                var responseData = clsDashBoard.GetPaymentCycle(formattedFromDate, formatttedDateToDate, mcc, route, vlc, zone, bank, hold, unhold, all, showData, outStanding, headLoad, paymentsummary, paymentCycleCode, Session["CompCode"].ToString(), Session["CompCode"].ToString(), Session["CompDesc"].ToString(), Session["AppUserCode"].ToString(), Session["Port"].ToString());
                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult ExportGetPaymentCycleWiseData(string fromDate, string toDate, bool hold, bool unhold, bool all, bool showData, bool outStanding, bool headLoad, string mcc, string route, string vlc, string zone, string bank, bool paymentsummary, string paymentCycleCode, bool pdf, bool excel)
        {
            try
            {
                var responseData = clsDashBoard.GetPaymentCycle(fromDate, toDate, mcc, route, vlc, zone, bank, hold, unhold, all, showData, outStanding, headLoad, paymentsummary, paymentCycleCode, Session["CompCode"].ToString(), Session["CompCode"].ToString(), Session["CompDesc"].ToString(), Session["AppUserCode"].ToString(), Session["Port"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();
                string sheetName = "Payment Cycle Wise Report";
                arrHeader.Add("Payment Cycle Wise Report");
                arrHeader.Add("Date : " + fromDate + " To " + toDate);
                string companyName = Session["CompDesc"].ToString();
                string reportDate = "Date : " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
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
                        fileContents = PdfGenerator.GeneratePdf(companyName, reportName, reportDate, rows, 65);
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

    }
}