using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DAL.Common;
using DAL.Farmer;
using DAL.Models;
using DAL.User;
using DAL.VLC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace XpertWebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private VLCList _Client;
        private Farmer _Farmer;
       
        public UserController()
        {
            _Client = new VLCList();
            _Farmer = new Farmer();
            
        }
        public ActionResult Index(string Guid)
        {
            UserModel _Model = new UserModel();
            try
            {
                if (Session["Port"] != null)
                {
                    string UserData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/User/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/User/" + Guid + ".json")) : null;
                    if (UserData != null)
                    {
                        _Model = JsonConvert.DeserializeObject<UserModel>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/User/" + Guid + ".json")));
                        Session["UserName"] = _Model.LoginData[0].Current_UserName;
                        Session["MCCCode"] = _Model.LoginData[0].MCC_Code;
                        Session["UserType"] = _Model.LoginData[0].User_APP_Type;
                        Session["Guid"] = Guid;
                        Session["VLCuploaderCode"] = _Model.LoginData[0].VLC_Code_VLC_Uploader;
                        Session["VLCName"] = _Model.LoginData[0].Vendor_Name;
                        Session["CurrUser"] = _Model.LoginData[0].Current_UserCode;
                       // Session["MPCode"] = _Model.LoginData[0].MP_Code;

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return View(_Model);
        }
        public ActionResult VLCList(string Guid)
        {
            VLCListModel _Model = new VLCListModel();
            try
            {
                _Client.GetVLCList(Session["Port"].ToString(),Session["MCCCode"].ToString(),Guid);
                string UserData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/VLC/VLCList/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/VLC/VLCList/" + Guid + ".json")) : null;
                if (UserData != null)
                {
                    _Model = JsonConvert.DeserializeObject<VLCListModel>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/VLC/VLCList/" + Guid + ".json")));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return View(_Model);
        }
        public ActionResult DBTEntry(string Guid)
        {
            if (Session["Port"] != null)
            {
                CVMDBTEntry _Model = new CVMDBTEntry();
                //_Client.GetVLCList(Session["Port"].ToString(), Session["MCCCode"].ToString(), Guid);
                string UserData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/VLC/DBTEntry/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/VLC/DBTEntry/" + Guid + ".json")) : null;
                if (UserData != null)
                {
                    _Model = JsonConvert.DeserializeObject<CVMDBTEntry>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/VLC/DBTEntry/" + Guid + ".json")));
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
                string fdate= Convert.ToString(fromdate.ToString("dd-MMM-yyyy"));
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(),"", fdate,"", "MPIncentiveEntryGetPaymentCycle", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                if (ResponseData.Contains("Error"))
                {
                }
                MPEntryPaymentCycle ObjPC= JsonConvert.DeserializeObject<MPEntryPaymentCycle>(ResponseData);
                ResponseData= _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate,ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntryGetBilledQty", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPBilledQty ObjBQ= JsonConvert.DeserializeObject<MPBilledQty>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntrySummaryTotal", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPIncentiveEntrySummaryTotal ObjSummary= JsonConvert.DeserializeObject<MPIncentiveEntrySummaryTotal>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fdate,"", "MPIncentiveEntryGetDataBulk", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPBulkData ObjBulkData = JsonConvert.DeserializeObject<MPBulkData>(ResponseData);
                CVMDBTEntry ObjCVMDBTEntry = new CVMDBTEntry();
                ObjCVMDBTEntry.VLC_UploderCode = Session["VLCuploaderCode"].ToString();
                ObjCVMDBTEntry.VLCName= Session["VLCName"].ToString();
                ObjCVMDBTEntry.BilledQty = ObjBQ.MPIncentiveEntryGetBilledQty[0].Qty;
                ObjCVMDBTEntry.MPFarmer = ObjSummary.Loving[0].Response_Code[0].Samples;
                ObjCVMDBTEntry.MPQty = ObjSummary.Loving[0].Response_Code[0].Qty;//!=null? (double?)ObjSummary.Loving[0].Response_Code[0].Qty:0;
                ObjCVMDBTEntry.MPAmount = ObjSummary.Loving[0].Response_Code[0].Amount;
                ObjCVMDBTEntry.FromDate = fdate;
                ObjCVMDBTEntry.ToDate = ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code;
                ObjCVMDBTEntry.Farmers = new List<CVMMPIEBulkData>();
                //List<CVMMPIEBulkData> objbd = new List<CVMMPIEBulkData>();
                for (int i = 0; i < ObjBulkData.MPIncentiveEntryGetDataBulk.Count -1; i++)
                {
                    CVMMPIEBulkData objBulkData = new CVMMPIEBulkData();
                    objBulkData.Code = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Code;
                    objBulkData.Farmer = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Name;
                    objBulkData.FatherName = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Father_Name;
                    objBulkData.Qty = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Qty;
                    ObjCVMDBTEntry.Farmers.Add(objBulkData);
                }
                string jsonBookingDetails = JsonConvert.SerializeObject(ObjCVMDBTEntry);
                ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/VLC/DBTEntry/" + Guid + ".json"), jsonBookingDetails);
                ResponseData=Guid;
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return Json(new { ResponseData }, JsonRequestBehavior.AllowGet);
        }
       // [HttpPost]
        public ActionResult SaveBulkData(List<CVMMPIEBulkData> obj)
        {
            string Guid = Session["Guid"].ToString();
            //List<CVMMPIEBulkData> obj = JsonConvert.DeserializeObject<List<CVMMPIEBulkData>>(jsonstring);
            List<MPIncentiveEntrySaveDataBulk> ObjSaveDataBulk = new List<MPIncentiveEntrySaveDataBulk>();
            CVMDBTEntry _Model = new CVMDBTEntry();
            //_Client.GetVLCList(Session["Port"].ToString(), Session["MCCCode"].ToString(), Guid);
            string UserData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/VLC/DBTEntry/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/VLC/DBTEntry/" + Guid + ".json")) : null;
            if (UserData != null)
            {
                _Model = JsonConvert.DeserializeObject<CVMDBTEntry>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/VLC/DBTEntry/" + Guid + ".json")));
            }
            for (int i = 0; i < obj.Count -1; i++)
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
        public void DBTReload(string fromDate,string Guid)
        {
            string ResponseData = string.Empty;
            try
            {
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), "", fromDate, "", "MPIncentiveEntryGetPaymentCycle", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                if (ResponseData.Contains("Error"))
                {
                }
                MPEntryPaymentCycle ObjPC = JsonConvert.DeserializeObject<MPEntryPaymentCycle>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fromDate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntryGetBilledQty", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPBilledQty ObjBQ = JsonConvert.DeserializeObject<MPBilledQty>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fromDate, ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code, "MPIncentiveEntrySummaryTotal", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPIncentiveEntrySummaryTotal ObjSummary = JsonConvert.DeserializeObject<MPIncentiveEntrySummaryTotal>(ResponseData);
                ResponseData = _Client.GetDBTEntryData(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["Guid"].ToString(), Session["VLCuploaderCode"].ToString(), fromDate, "", "MPIncentiveEntryGetDataBulk", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                MPBulkData ObjBulkData = JsonConvert.DeserializeObject<MPBulkData>(ResponseData);
                CVMDBTEntry ObjCVMDBTEntry = new CVMDBTEntry();
                ObjCVMDBTEntry.VLC_UploderCode = Session["VLCuploaderCode"].ToString();
                ObjCVMDBTEntry.VLCName = Session["VLCName"].ToString();
                ObjCVMDBTEntry.BilledQty = ObjBQ.MPIncentiveEntryGetBilledQty[0].Qty;
                ObjCVMDBTEntry.MPFarmer = ObjSummary.Loving[0].Response_Code[0].Samples;
                ObjCVMDBTEntry.MPQty = ObjSummary.Loving[0].Response_Code[0].Qty;//!=null? (double?)ObjSummary.Loving[0].Response_Code[0].Qty:0;
                ObjCVMDBTEntry.MPAmount = ObjSummary.Loving[0].Response_Code[0].Amount;
                ObjCVMDBTEntry.FromDate = fromDate;
                ObjCVMDBTEntry.ToDate = ObjPC.MPIncentiveEntryGetPaymentCycle[0].Code;
                ObjCVMDBTEntry.Farmers = new List<CVMMPIEBulkData>();
                //List<CVMMPIEBulkData> objbd = new List<CVMMPIEBulkData>();
                for (int i = 0; i < ObjBulkData.MPIncentiveEntryGetDataBulk.Count - 1; i++)
                {
                    CVMMPIEBulkData objBulkData = new CVMMPIEBulkData();
                    objBulkData.Code = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Code;
                    objBulkData.Farmer = ObjBulkData.MPIncentiveEntryGetDataBulk[i].MP_Name;
                    objBulkData.FatherName = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Father_Name;
                    objBulkData.Qty = ObjBulkData.MPIncentiveEntryGetDataBulk[i].Qty;
                    ObjCVMDBTEntry.Farmers.Add(objBulkData);
                }
                string jsonBookingDetails = JsonConvert.SerializeObject(ObjCVMDBTEntry);
                ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/VLC/DBTEntry/" + Guid + ".json"), jsonBookingDetails);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
        }
        public ActionResult GetFarmerList(String Guid)
        {
            FarmerModel _Model = new FarmerModel();
            if (Session["Port"] != null)
            {
                string ResponseData = string.Empty;
                try
                {
                    _Farmer.MPGetList(Session["Port"].ToString(), Session["MCCCode"].ToString(), Session["VLCuploaderCode"].ToString(), Guid, Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    string FarmerData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/Farmer/MPGetList/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/Farmer/MPGetList/" + Guid + ".json")) : null;
                    if (FarmerData != null)
                    {
                        _Model = JsonConvert.DeserializeObject<FarmerModel>(FarmerData);
                       // Session["MPCode"]=_Model
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.WriteError(ex);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(_Model);
        }
        public ActionResult NewRegistration(string Guid,string Code)
        {
            MPDetailModel _Model = new MPDetailModel();
            if (Session["Port"] != null)
            {
                try
                {
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPGetCastCategory", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPDistictList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPZoneList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPBlockList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPRevenueVillageList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPGramPanchayatList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPPanchayatSamitiList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetListData(Session["Port"].ToString(), Guid, "MPVidhanSabhaList", Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                    _Farmer.GetFixParameter(Session["Port"].ToString(), Guid);
                    if (!string.IsNullOrEmpty(Code))
                    {
                        _Farmer.MPMasterGetData(Session["Port"].ToString(), Code, Guid, Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
                        string FarmerDetails = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/Farmer/MPDetail/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/Farmer/MPDetail/" + Guid + ".json")) : null;
                        if (FarmerDetails != null)
                        {
                            _Model = JsonConvert.DeserializeObject<MPDetailModel>(FarmerDetails);
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.WriteError(ex);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(_Model);
        }

        public ActionResult Farmer_SaveData(MPDetail Obj)
        {
            string ResponseData = string.Empty;
            try
            {
                ResponseData = _Farmer.MPMasterSaveData(Session["Port"].ToString(),Session["CurrUser"].ToString(),Session["MCCCode"].ToString(), Session["VLCuploaderCode"].ToString(),Obj, Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
            }
            catch(Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }


            return Json(new { ResponseData }, JsonRequestBehavior.AllowGet);
        }
    }
}