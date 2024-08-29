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
        public async Task< JsonResult> GetMcc()
        {
            try
            {
                string methodName = "GetMCC";
                var lst =await clsDashBoard.GetData(methodName);
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetRoute()
        {
            try
            {
                string methodName = "GetRoute";
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
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
                var lst = await clsDashBoard.GetData(methodName);
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}