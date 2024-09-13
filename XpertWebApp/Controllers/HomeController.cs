using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL.Common;
using DAL.Location;
using DAL.DashBoard;
using System.Web.Optimization;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using DAL.PostData;
using DAL.Models;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;

namespace XpertWebApp.Controllers
{
    public class HomeController : Controller
    {
        private Locations _Client;
        public HomeController()
        {
            _Client = new Locations();
            _Client.GetAppLocation();
        }
        public ActionResult Index()
        {
            LocationModel _Model = new LocationModel();
            try
            {             
                if (ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/Location/APPLocation.json")) != null)
                {
                    _Model = JsonConvert.DeserializeObject<LocationModel>(ReadWriteFile.ReadFile(HostingEnvironment.MapPath("/App_Data/Location/APPLocation.json")));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return View(_Model);
        }
        public ActionResult Login(string UserId,string Pwd,string code)
        {
           
            string ResponseData = string.Empty;
            string methodName = "GetScreenPermission";
            try
            {
                Session["Port"] = code;
                
                ResponseData = _Client.CheckLoginDetails(UserId,Pwd, code);
                //JArray jArray = JArray.Parse(ResponseData);
                if (!ResponseData.Contains("Error") || !ResponseData.Contains("A"))
                {
                    Session["AppUserCode"] = UserId;
                    Session["AppPwd"] = Pwd;
                    var data = clsDashBoard.CheckLoginDetails(UserId, Pwd, Session["Port"].ToString());
                    JArray jsonArrayl = JArray.Parse(data);
                    Session["CompCode"] = (string)jsonArrayl[0]["Comp_Code"];
                    Session["CompDesc"] = (string)jsonArrayl[0]["Comp_Name"];
                    //Session["CurrentLocation"]= (string)jArray[0]["Default_Location"];
                    //Session["MCC_Code"] = (string)jArray[0]["MCC_Code"];
                    var GetMenuPermission = clsDashBoard.GetMenuPermission(methodName, Session["AppUserCode"].ToString(), Session["Port"].ToString());
                    JArray jsonArray = JArray.Parse(GetMenuPermission);
                    if (GetMenuPermission.Contains("true"))
                    {
                        List<string> programCodes = jsonArray.Where(item => item["Program_Code"] != null).Select(item => item["Program_Code"].ToString()).ToList();
                        Session["ProgramCodes"] = programCodes;
                    }
                    else
                    {
                        Session["ProgramCodes"]=null;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }

            return Json(new { ResponseData}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            //Session["Port"] = null;
            //Session["UserName"] = null;
            //Session["MCCCode"] =null;
            //Session["UserType"] =null;
            //Session["Guid"] = null;
            //Session["VLCuploaderCode"] = null;
            //Session["VLCName"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}