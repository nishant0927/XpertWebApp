using DAL.Common;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace DAL.Farmer
{
    public class Farmer
    {
        public void MPGetList(string code, string MCCCode,string VLCUploaderCode, string guid, string AppUserCode, string AppPwd)
        {
            var responseData = string.Empty;
            try
            {
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + "MPGetList";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&APPName=" + Uri.EscapeDataString(Configuration.APPName);
                postData += "&APPVersion=" + Uri.EscapeDataString(Configuration.APPVersion);
                postData += "&APPUserCode=" + Uri.EscapeDataString(AppUserCode);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(AppPwd);
                postData += "&MCCCode=" + Uri.EscapeDataString(MCCCode);
                postData += "&VLCUploaderCode=" + Uri.EscapeDataString(VLCUploaderCode);
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postDataBytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Read the response content
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                if (responseData.Contains("Error"))
                                {
                                     //return "Error";
                                }
                                else
                                {

                                    ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/Farmer/MPGetList/" + guid + ".json"), responseData);
                                    //return guid.ToString();
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
                catch (WebException ex)
                {
                    // Handle exception if the request fails
                    ErrorHandler.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }

        }
        public void MPMasterGetData(string code, string MPCode, string guid, string AppUserCode, string AppPwd)
        {
            var responseData = string.Empty;
            try
            {
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + "MPMasterGetData";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&APPName=" + Uri.EscapeDataString(Configuration.APPName);
                postData += "&APPVersion=" + Uri.EscapeDataString(Configuration.APPVersion);
                postData += "&APPUserCode=" + Uri.EscapeDataString(AppUserCode);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(AppPwd);
                postData += "&MPCode=" + Uri.EscapeDataString(MPCode);
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postDataBytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Read the response content
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                if (responseData.Contains("Error"))
                                {
                                    //return "Error";
                                }
                                else
                                {

                                    ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/Farmer/MPDetail/" + guid + ".json"), responseData);
                                    //return guid.ToString();
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
                catch (WebException ex)
                {
                    // Handle exception if the request fails
                    ErrorHandler.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }

        }
        public void GetListData(string code,string guid,string MethodName, string AppUserCode, string AppPwd)
        {
            var responseData = string.Empty;
            string fname = string.Empty;
            try
            {
                if (String.Equals(MethodName, "MPGetCastCategory"))
                {
                    fname = "Cast";
                }
                else if (string.Equals(MethodName, "MPDistictList"))
                {
                    fname = "District";
                }
                else if (string.Equals(MethodName, "MPZoneList"))
                {
                    fname = "Zone";
                }
                else if (string.Equals(MethodName, "MPBlockList"))
                {
                    fname = "Block";
                }
                else if (string.Equals(MethodName, "MPRevenueVillageList"))
                {
                    fname = "RVillage";
                }
                else if (string.Equals(MethodName, "MPGramPanchayatList"))
                {
                    fname = "GramPanchayat";
                }
                else if (string.Equals(MethodName, "MPPanchayatSamitiList"))
                {
                    fname = "PanchayatSamiti";
                }
                else if (string.Equals(MethodName, "MPVidhanSabhaList"))
                {
                    fname = "VidhanSabha";
                }
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + MethodName;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&APPName=" + Uri.EscapeDataString(Configuration.APPName);
                postData += "&APPVersion=" + Uri.EscapeDataString(Configuration.APPVersion);
                postData += "&APPUserCode=" + Uri.EscapeDataString(AppUserCode);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(AppPwd);
               
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postDataBytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Read the response content
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                if (responseData.Contains("Error"))
                                {
                                    //return "Error";
                                }
                                else
                                {

                                    ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/Farmer/Lists/" + fname + "/" + guid + ".json"), responseData);
                                    //return guid.ToString();
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
                catch (WebException ex)
                {
                    // Handle exception if the request fails
                    ErrorHandler.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }

        }
        public void GetFixParameter(string code, string guid)
        {
            var responseData = string.Empty;
            try
            {
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + "GetFixedParameter ";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&strType=" + Uri.EscapeDataString(Configuration.strType);
                postData += "&strCode=" + Uri.EscapeDataString(Configuration.strCode);
                
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postDataBytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Read the response content
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                if (responseData.Contains("Error"))
                                {
                                    //return "Error";
                                }
                                else
                                {

                                    ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/Farmer/" + guid + ".json"), responseData);
                                    //return guid.ToString();
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
                catch (WebException ex)
                {
                    // Handle exception if the request fails
                    ErrorHandler.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }

        }
        public string MPMasterSaveData(string code,string CurrUser,string MCCCode,string VLCUploaderCode, MPDetail obj, string AppUserCode, string AppPwd)
        {
            var responseData = string.Empty;
            try
            {
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + "MPMasterSaveData";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&APPName=" + Uri.EscapeDataString(Configuration.APPName);
                postData += "&APPVersion=" + Uri.EscapeDataString(Configuration.APPVersion);
                postData += "&APPUserCode=" + Uri.EscapeDataString(AppUserCode);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(AppPwd);
                postData += "&CurrentUserCode=" + Uri.EscapeDataString(CurrUser);
                postData += "&MCCCode=" + Uri.EscapeDataString(MCCCode);
                postData += "&VLCUploaderCode=" + Uri.EscapeDataString(VLCUploaderCode);
                postData += "&MPUploaderCode=" + Uri.EscapeDataString(obj.MP_CODE_VLC_UPLOADER);
                postData += "&MPName=" + Uri.EscapeDataString(obj.MP_NAME);
                postData += "&FatherName=" + Uri.EscapeDataString(obj.Father_Name!=null? obj.Father_Name:"");
                postData += "&Gender=" + Uri.EscapeDataString(obj.Gender!=null?obj.Gender:"");
                postData += "&Address1=" + Uri.EscapeDataString(obj.Add1!=null?obj.Add1:"");
                postData += "&Address2=" + Uri.EscapeDataString(obj.Add2!=null?obj.Add2:"");
                postData += "&AadharNo=" + Uri.EscapeDataString(obj.AadharNo!=null?obj.AadharNo:"");
                postData += "&BankName=" + Uri.EscapeDataString(obj.BankName!=null?obj.BankName:"");
                postData += "&BankACNo=" + Uri.EscapeDataString(obj.AccountNO);
                postData += "&BankIFSC=" + Uri.EscapeDataString(obj.IFCICode);
                postData += "&Telphone=" + Uri.EscapeDataString(obj.Telphone!=null?obj.Telphone:"");
                postData += "&MPCode=" + Uri.EscapeDataString(obj.MP_CODE_VLC_UPLOADER);
                postData += "&InActive=" + Uri.EscapeDataString(Convert.ToString(obj.InActive));
                postData += "&MPNameHindi=" + Uri.EscapeDataString(obj.MP_Name_Hindi!=null?obj.MP_Name_Hindi:"");
                postData += "&MPJanAadharNo=" + Uri.EscapeDataString(obj.Jan_Aadhar_No!=null?obj.Jan_Aadhar_No:"");
                postData += "&Age=" + Uri.EscapeDataString(obj.Age!=null?obj.Age:"");
                postData += "&CastCategory=" + Uri.EscapeDataString(obj.CAST_CATEGORY_CODE!=null?obj.CAST_CATEGORY_CODE:"");
                postData += "&DISTRICT_Code=" + Uri.EscapeDataString(obj.DISTRICT_Code!=null?obj.DISTRICT_Code:"");
                postData += "&Zone_Code=" + Uri.EscapeDataString(obj.Zone_Code!=null?obj.Zone_Code:"");
                postData += "&BLOCK_CODE=" + Uri.EscapeDataString(obj.BLOCK_CODE!=null?obj.BLOCK_CODE:"");
                postData += "&REVENUE_VILLAGE_CODE=" + Uri.EscapeDataString(obj.REVENUE_VILLAGE_CODE!=null?obj.REVENUE_VILLAGE_CODE:"");
                postData += "&GRAMPANCHAYAT_CODE=" + Uri.EscapeDataString(obj.GRAMPANCHAYAT_CODE!=null?obj.GRAMPANCHAYAT_CODE:"");
                postData += "&PANCHAYAT_SAMITI_CODE=" + Uri.EscapeDataString(obj.PANCHAYAT_SAMITI_CODE!=null?obj.PANCHAYAT_SAMITI_CODE:"");
                postData += "&VIDHAN_SABHA_CODE=" + Uri.EscapeDataString(obj.VIDHAN_SABHA_CODE!=null?obj.VIDHAN_SABHA_CODE:"");


                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postDataBytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Read the response content
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                if (responseData.Contains("Error"))
                                {
                                    return responseData;
                                }
                                else
                                {

                                   // ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/Farmer/MPDetail/" + guid + ".json"), responseData);
                                    return responseData;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
                catch (WebException ex)
                {
                    // Handle exception if the request fails
                    ErrorHandler.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return "Error";
        }
    }
}
