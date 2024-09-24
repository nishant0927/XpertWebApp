using DAL.DashBoard;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class DCSOutStandingController : Controller
    {

        public ActionResult DCSOutStandingReport()
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
        public JsonResult GetDCSOutStandingReport(string fromDate, string toDate, string mccCode, string deductionCode, bool chkDCSWise, bool rbtOldOutStanding, bool rbtCurrentOpeningDeduction, bool rbtCurrentOutStanding, bool rbtOnlyOpening, bool rbtOnlyReduceDeduction, bool rbtActive, bool rbtInActive, bool rbtAll, bool btnPrint, bool AreaWiseBilling, string fndArea)
        {
            try
            {
                string formattedFromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                string formatttedDateToDate = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
                var responseData = clsDashBoard.GGetDCSOutStanding(formattedFromDate, formatttedDateToDate, rbtInActive, rbtInActive, rbtOldOutStanding, rbtCurrentOpeningDeduction, rbtCurrentOutStanding, rbtOnlyOpening, rbtOnlyReduceDeduction, mccCode, deductionCode, chkDCSWise, btnPrint, Session["AppUserCode"].ToString(), Session["CompCode"].ToString(), AreaWiseBilling, fndArea, Session["Port"].ToString());
                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ExportGetDCSOutStandingReport(string fromDate, string toDate, string mccCode, string deductionCode, bool chkDCSWise, bool rbtOldOutStanding, bool rbtCurrentOpeningDeduction, bool rbtCurrentOutStanding, bool rbtOnlyOpening, bool rbtOnlyReduceDeduction, bool rbtActive, bool rbtInActive, bool rbtAll, bool btnPrint, bool AreaWiseBilling, string fndArea, bool excel, bool pdf)
        {
            try
            {
                string formattedFromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                string formatttedDateToDate = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
                var responseData = clsDashBoard.GGetDCSOutStanding(formattedFromDate, formatttedDateToDate, rbtInActive, rbtInActive, rbtOldOutStanding, rbtCurrentOpeningDeduction, rbtCurrentOutStanding, rbtOnlyOpening, rbtOnlyReduceDeduction, mccCode, deductionCode, chkDCSWise, btnPrint, Session["AppUserCode"].ToString(), Session["CompCode"].ToString(), AreaWiseBilling, fndArea, Session["Port"].ToString());
                JArray jsonArray = JArray.Parse(responseData);
                List<string> arrHeader = new List<string>();
                string sheetName = "MCC Milk Register";
                arrHeader.Add("MCC Milk Register");
                arrHeader.Add("Date : " + formattedFromDate + " To " + formatttedDateToDate);
                string companyName = Session["CompDesc"].ToString();
                string reportDate = "Date : " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
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
                        fileContents = PdfGenerator.GeneratePdf(companyName, reportName, reportDate, rows, 65);
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