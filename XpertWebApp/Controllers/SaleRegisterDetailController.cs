using DAL.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class SaleRegisterDetailController : Controller
    {
        // GET: SaleRegisterDetail
        public ActionResult SalesRegisterDetailReport()
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
        public JsonResult GetReportType()
        {
            string compCode = Session["CompCode"].ToString();
            try
            {
                var items = new List<SelectListItem>
            {
                     new SelectListItem { Text = "Total Sale", Value = "Total Sale" },
        new SelectListItem { Text = "Location Wise", Value = "Location Wise" },
        new SelectListItem { Text = "Item Group Wise", Value = "Item Group Wise" },
        new SelectListItem { Text = "Customer Group Wise", Value = "Customer Group Wise" },
        new SelectListItem { Text = "Item Wise", Value = "Item Wise" },
        new SelectListItem { Text = "Customer Wise", Value = "Customer Wise" },
        new SelectListItem { Text = "Document Wise", Value = "Document Wise" },
        new SelectListItem { Text = "Document Detail", Value = "Document Detail" },
        new SelectListItem { Text = "Document Info Level", Value = "Document Info Level" },
        new SelectListItem { Text = "Sale Register With Purchase", Value = "Sale Register With Purchase" },
        new SelectListItem { Text = "Net Sale", Value = "Net Sale" }
        //new SelectListItem { Text = "TCS", Value = "TCS" },
        //new SelectListItem { Text = "Route Booth Wise", Value = "RBW" }
            };
         //       if (Common.CompairString(compCode, "JPR") == CompairStringResult.Equal ||
         //Common.CompairString(compCode, "UDP") == CompairStringResult.Equal)
         //       {
         //           items.Add(new SelectListItem { Text = "Demand Sheet", Value = "Demand Sheet" });
         //       }
                return Json(new { items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> ItemGroup()
        {
            try
            {
                var lst = await clsMasterData.ItemGroup(Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<JsonResult> GetZOneByCustomer(string customer)
        {
            try
            {
                var lst = await clsMasterData.GetZOneByCustomer(customer, Session["Port"].ToString());
                return Json(new { lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetSalesRegisterDetail(string fromDate, string ToDate, string ddlReportType, string txtUOM, string txtItem, string txtTransaction, string txtState, string txtLocation, string txtCustomer, string txtItemGroup, string txtCustGroup, string txtRoute, string txtmultSchemeType, string TxtMultiZone, string TxtMultiASM, string TxtMultiRSM, string TxtMultiASO, string TxtMultiCustomerCategory, string Document_No, bool rbtnCategorySelect, string category, bool btnPosted, bool btnUnposted, bool chk_stockingunit, bool chkIncludeDebitCredit, bool chkQuickLoad, bool chkBatchWise, string itemStructure)
        {
            try
            {

                var responseData = await clsDashBoard.GetSalesRegisterDetail(fromDate, ToDate, ddlReportType, txtUOM, txtItem, txtTransaction, txtState, txtLocation, txtCustomer, txtItemGroup, txtCustGroup, txtRoute, txtmultSchemeType, TxtMultiZone, TxtMultiASM, TxtMultiRSM, TxtMultiASO, TxtMultiCustomerCategory, Session["AppUserCode"].ToString(), Document_No, rbtnCategorySelect, category, btnPosted, btnUnposted, chk_stockingunit, chkIncludeDebitCredit, chkQuickLoad, chkBatchWise, itemStructure,Session["Port"].ToString());

                var jsonResult = Json(new { success = true, responseData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetCategoryLevel()
        {
            try
            {
                string methodName = "GetCategoryLevel";
                var responseData = await clsDashBoard.GetData(methodName, Session["Port"].ToString());

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