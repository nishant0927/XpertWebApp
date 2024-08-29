using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.DemandBooking;
using Newtonsoft.Json;

namespace XpertWebApp.Controllers
{
    public class DairySalesController : Controller
    {
       public ActionResult DemandBooking()
        {
            return View();
        }

        public async Task<JsonResult> GetRoutAsync()
        {
            try
            {
                var responseData = await Demand.GetRout();
                return Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDocNo(string routCode, bool rbtnMorning, bool fresh, bool product, bool both, string date, NavigatorType NavType)
        {
            try
            {
                bool SeparateDemandMilkandProduct = false;
                string DocNo = Demand.GetDocNo(routCode, rbtnMorning, fresh, product, both, date, NavType, SeparateDemandMilkandProduct);
                return Json(new { success = true, DocNo }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
           
        }

        public JsonResult LoadData(string strDocumentNo, NavigatorType NavType)
        {
            try
            {
                string currentUserLocation = string.Empty;
                if (Session["CurrentLocation"].ToString() != null)
                    currentUserLocation = Session["CurrentLocation"].ToString();
                else
                    currentUserLocation = Session["MCC_Code"].ToString();
                string responseData = Demand.GetLoadData(strDocumentNo, NavType, currentUserLocation);
                // DemandBookingSaleModel obj = JsonSerializer.Deserialize<DemandBookingSaleModel>(responseData);
                DemandBookingSaleModel obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DemandBookingSaleModel>(responseData);
                return Json(new { success = true, obj }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}