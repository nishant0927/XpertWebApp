using DAL.Common;
using DAL.DBT;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace XpertWebApp.Controllers
{
    public class DBTController : Controller
    {
        private DBT _Client;
        public DBTController()
        {
            _Client = new DBT();
        }
        public ActionResult DBTEntry(string Guid)
        {
            if (Session["Port"] != null)
            {
               DBTVM _Model = new DBTVM();
                string UserData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/DBT/DBTEntry/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/DBT/DBTEntry/" + Guid + ".json")) : null;
                if (UserData != null)
                {
                    _Model = JsonConvert.DeserializeObject<DBTVM>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/DBT/DBTEntry/" + Guid + ".json")));
                }
                return View(_Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetData(DateTime fromdate)
        {
            string ResponseData = string.Empty;
            try
            {
                string Guid = Session["Guid"].ToString();
                string fdate = Convert.ToString(fromdate.ToString("dd-MMM-yyyy"));
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), "", fdate, "", "MPIncentiveEntryGetPaymentCycle", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                if (!ResponseData.Contains("Error"))
                {
                    MPEntryPaymentCycle ObjPC = JsonConvert.DeserializeObject<MPEntryPaymentCycle>(ResponseData);
                    ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntryGetBilledQty", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    MPBilledQty ObjBQ = JsonConvert.DeserializeObject<MPBilledQty>(ResponseData);
                    ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntrySummaryTotal", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    MPIncentiveEntrySummaryTotal ObjSummary = JsonConvert.DeserializeObject<MPIncentiveEntrySummaryTotal>(ResponseData);
                    ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate, "", "MPIncentiveEntryGetDataBulk", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    MPBulkData ObjBulkData = JsonConvert.DeserializeObject<MPBulkData>(ResponseData);

                    DBTVM ObjDBTVM = new DBTVM();
                    ObjDBTVM.VLC_UploderCode = Session["VLCuploaderCode"].ToString();
                    ObjDBTVM.VLCName = Session["VLCName"].ToString();
                    ObjDBTVM.BilledQty = ObjBQ.MPIncentiveEntryGetBilledQty[0].Qty;
                    ObjDBTVM.MPFarmer = ObjSummary.Loving[0].Response_Code[0].Samples;
                    ObjDBTVM.MPQty = ObjSummary.Loving[0].Response_Code[0].Qty;//!=null? (double?)ObjSummary.Loving[0].Response_Code[0].Qty:0;
                    ObjDBTVM.MPAmount = ObjSummary.Loving[0].Response_Code[0].Amount;
                    ObjDBTVM.FromDate = fdate;
                    ObjDBTVM.ToDate = ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code;
                    ObjDBTVM.Farmers = new List<VMMPIEBulkData>();
                    for (int i = 0; i < ObjBulkData.MPIncentiveEntryGetDataBulk.Count - 1; i++)
                    {
                        VMMPIEBulkData objBulkData = new VMMPIEBulkData();
                        objBulkData.Code = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Code;
                        objBulkData.Farmer = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Name;
                        objBulkData.FatherName = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Father_Name;
                        objBulkData.Qty = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Qty;
                        ObjDBTVM.Farmers.Add(objBulkData);
                    }
                    string jsonDBT = JsonConvert.SerializeObject(ObjDBTVM);
                    ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/DBT/DBTEntry/" + Guid + ".json"), jsonDBT);
                    ResponseData = Guid;
                }
                
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return Json(new { ResponseData }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveBulkData(List<VMMPIEBulkData> obj)
        {
            string Guid = Session["Guid"].ToString();
            List<MPIncentiveEntrySaveDataBulk> ObjSaveDataBulk = new List<MPIncentiveEntrySaveDataBulk>();
            DBTVM _Model = new DBTVM();
            string UserData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/DBT/DBTEntry/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/DBT/DBTEntry/" + Guid + ".json")) : null;
            if (UserData != null)
            {
                _Model = JsonConvert.DeserializeObject<DBTVM>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/DBT/DBTEntry/" + Guid + ".json")));
            }
            for (int i = 0; i < obj.Count - 1; i++)
            {
                MPIncentiveEntrySaveDataBulk ObjSaveData = new MPIncentiveEntrySaveDataBulk();
                ObjSaveData.VLCUploaderCode = _Model.VLC_UploderCode;
                ObjSaveData.MCCCode = Session["MCCCode"].ToString();
                ObjSaveData.MPUploaderCode = obj[i].Code;
                ObjSaveData.FromDate = _Model.FromDate;
                ObjSaveData.ToDate = _Model.ToDate;
                ObjSaveData.Qty = obj[i].Qty;
                //obj
                ObjSaveDataBulk.Add(ObjSaveData);
            }
            string res = _Client.SaveDataBulk(Session["Port"].ToString(), Guid, ObjSaveDataBulk, Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
            DBTReload(_Model.FromDate, Guid);
            return Json(new { res }, JsonRequestBehavior.AllowGet);
        }
        public void DBTReload(string fromDate, string Guid)
        {
            string ResponseData = string.Empty;
            try
            {
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), "", fromDate, "", "MPIncentiveEntryGetPaymentCycle", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPEntryPaymentCycle ObjPC = JsonConvert.DeserializeObject<MPEntryPaymentCycle>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fromDate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntryGetBilledQty", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPBilledQty ObjBQ = JsonConvert.DeserializeObject<MPBilledQty>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fromDate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntrySummaryTotal", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPIncentiveEntrySummaryTotal ObjSummary = JsonConvert.DeserializeObject<MPIncentiveEntrySummaryTotal>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fromDate, "", "MPIncentiveEntryGetDataBulk", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPBulkData ObjBulkData = JsonConvert.DeserializeObject<MPBulkData>(ResponseData);
                DBTVM ObjDBTVM = new DBTVM();
                ObjDBTVM.VLC_UploderCode = Session["VLCuploaderCode"].ToString();
                ObjDBTVM.VLCName = Session["VLCName"].ToString();
                ObjDBTVM.BilledQty = ObjBQ.MPIncentiveEntryGetBilledQty[0].Qty;
                ObjDBTVM.MPFarmer = ObjSummary.Loving[0].Response_Code[0].Samples;
                ObjDBTVM.MPQty = ObjSummary.Loving[0].Response_Code[0].Qty;//!=null? (double?)ObjSummary.Loving[0].Response_Code[0].Qty:0;
                ObjDBTVM.MPAmount = ObjSummary.Loving[0].Response_Code[0].Amount;
                ObjDBTVM.FromDate = fromDate;
                ObjDBTVM.ToDate = ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code;
                ObjDBTVM.Farmers = new List<VMMPIEBulkData>();
                //List<CVMMPIEBulkData> objbd = new List<CVMMPIEBulkData>();
                for (int i = 0; i < ObjBulkData.MPIncentiveEntryGetDataBulk.Count - 1; i++)
                {
                    VMMPIEBulkData objBulkData = new VMMPIEBulkData();
                    objBulkData.Code = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Code;
                    objBulkData.Farmer = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Name;
                    objBulkData.FatherName = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Father_Name;
                    objBulkData.Qty = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Qty;
                    ObjDBTVM.Farmers.Add(objBulkData);
                }
                string jsonDBT = JsonConvert.SerializeObject(ObjDBTVM);
                ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/DBT/DBTEntry/" + Guid + ".json"), jsonDBT);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
        }
        public ActionResult DBTRegister(string Guid)
        {
            string ResponseData = string.Empty;
            
           
                if (Session["Port"] != null)
                {
                    DBTRegisterModel _Model = new DBTRegisterModel();
                    string DBTData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/DBT/DBTRegister/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/DBT/DBTRegister/" + Guid + ".json")) : null;
                    
                if (DBTData != null)
                    {
                    if (!DBTData.Contains("ERROR_Code"))
                    {
                        _Model = JsonConvert.DeserializeObject<DBTRegisterModel>(DBTData);

                    }
                    
                    //_Model.Loving[0].Response_Code
                }
                ViewBag.VLCName = Session["VLCName"].ToString();
                ViewBag.VLCUploaderCode = Session["VLCuploaderCode"].ToString();
                    return View(_Model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                //ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntryGetBilledQty", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());

           
        }
        public ActionResult GetPCycle(DateTime fromdate)
        {
            string ResponseData = string.Empty;
            try
            {
                string Guid = Session["Guid"].ToString();
                string fdate = Convert.ToString(fromdate.ToString("dd-MMM-yyyy"));
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), "", fdate, "", "MPIncentiveEntryGetPaymentCycle", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                if (!ResponseData.Contains("Error"))
                {
                    MPEntryPaymentCycle ObjPC = JsonConvert.DeserializeObject<MPEntryPaymentCycle>(ResponseData);
                    TempData["Fdate"] = fdate;
                    TempData["tdate"] = ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code;
                    ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntryGetSummary", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    ResponseData = Guid;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return Json(new { ResponseData }, JsonRequestBehavior.AllowGet);
        }
    }
}