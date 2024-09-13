using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL.DashBoard;

namespace XpertWebApp.Controllers
{
    public class CommonController : Controller
    {
        public async Task<JsonResult> GetMcc()
        {
            try
            {
                string methodName = "GetMCC";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetRoute()
        {
            try
            {
                string methodName = "GetRoute";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetDCS()
        {
            try
            {
                string methodName = "GetDCS";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetZone()
        {
            try
            {
                string methodName = "GetZone";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetBank()
        {
            try
            {
                string methodName = "GetBank";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetPaymentProcess()
        {
            try
            {
                string methodName = "GetPaymentProcess";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetLocation()
        {
            try
            {
                string methodName = "GetLocation";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetFiscalYea()
        {
            try
            {
                string methodName = "GetFiscalYea";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetMCCForDCSLedger()
        {
            try
            {
                string methodName = "GetMCCForDCSLedger";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetMccForFiscalYearDCS()
        {
            try
            {
                string methodName = "GetMccForFiscalYearDCS";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetDCSForFiscalYearDCS()
        {
            try
            {
                string methodName = "GetDCSForFiscalYearDCS";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetDeduction()
        {
            try
            {
                string methodName = "GetDeduction";
                var lst = await clsDashBoard.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReportType()
        {
            string compCode = Session["CompCode"].ToString();
            try
            {
                var items = new List<SelectListItem>
            {
                     new SelectListItem { Text = "Matrix Fresh Sale", Value = "MFS" },
        new SelectListItem { Text = "Demand Wise", Value = "DW" },
        new SelectListItem { Text = "Truck Sheet", Value = "TS" },
        new SelectListItem { Text = "Milk Gate Pass Demand Detail", Value = "MGPD" },
        new SelectListItem { Text = "Product Gate Pass Demand Detail", Value = "PGPD" },
        new SelectListItem { Text = "Dairy Milk Gate Pass Detail", Value = "DMGPD" },
        new SelectListItem { Text = "Dairy Product Gate Pass Detail", Value = "DPGPD" },
        new SelectListItem { Text = "Milk Sale Report", Value = "MSR" },
        new SelectListItem { Text = "Product Sale Report", Value = "PSR" },
        new SelectListItem { Text = "Credit Sale Report", Value = "CSR" },
        new SelectListItem { Text = "Milk Product Demand Report", Value = "MPDR" },
        new SelectListItem { Text = "TCS", Value = "TCS" },
        new SelectListItem { Text = "Route Booth Wise", Value = "RBW" }
            };
                if (Common.CompairString(compCode, "JPR") == CompairStringResult.Equal ||
         Common.CompairString(compCode, "UDP") == CompairStringResult.Equal)
                {
                    items.Add(new SelectListItem { Text = "Demand Sheet", Value = "Demand Sheet" });
                }
                return Json(new { items }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
           

        }


        public async Task<JsonResult> GetMasterData(string methodName)
        {
            try
            {
                // string methodName = "GetDeduction";
                var lst = await clsMasterData.GetData(methodName, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}