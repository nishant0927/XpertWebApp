using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL.DashBoard;

namespace XpertWebApp.Controllers
{
    public class SalesSummaryController : Controller
    {
        // GET: SalesSummary
        public ActionResult SalesSummaryReport()
        {
            return View();
        }

        public async Task<JsonResult> GetSalesSummary(string route, string fromDate,string todate)
        {
            try
            {
                var responseData = await clsSalesData.GetSalesSummary(route, fromDate, todate, Session["Port"].ToString());
                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}