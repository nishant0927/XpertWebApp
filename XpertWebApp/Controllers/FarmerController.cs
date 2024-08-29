using DAL.Common;
using DAL.Farmer;
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
    public class FarmerController : Controller
    {
        private Farmer _Farmer;
        public FarmerController()
        {
            _Farmer = new Farmer();
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
                    //string FarmerData = System.IO.File.Exists(HostingEnvironment.MapPath("~/App_Data/Farmer/MPGetList/" + Guid + ".json")) ? ReadWriteFile.ReadFile(HostingEnvironment.MapPath("~/App_Data/Farmer/MPGetList/" + Guid + ".json")) : null;
                    string filePath = HostingEnvironment.MapPath("~/App_Data/Farmer/MPGetList/" + Guid + ".json");
                    string FarmerData = System.IO.File.Exists(filePath) ? ReadWriteFile.ReadFile(filePath) : null;
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
        public ActionResult NewRegistration(string Guid, string Code)
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
                ResponseData = _Farmer.MPMasterSaveData(Session["Port"].ToString(), Session["CurrUser"].ToString(), Session["MCCCode"].ToString(), Session["VLCuploaderCode"].ToString(), Obj, Session["AppUserCode"].ToString(), Session["AppPwd"].ToString());
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }


            return Json(new { ResponseData }, JsonRequestBehavior.AllowGet);
        }
    }
}