using DAL.Common;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
namespace DAL.DBT
{
    public class DBT
    {
        public string GetDBTEntryData(string code, string MCCCode, string guid, string VLCUploaderCode, string FromDate, string ToDate, string MethodName, string AppUserCode, string AppPwd)
        {
            var responseData = string.Empty;
            string fname = string.Empty;
            try
            {
                if (String.Equals(MethodName, "MPIncentiveEntryGetPaymentCycle"))
                {
                    fname = "PaymentCycle";
                }
                else if (string.Equals(MethodName, "MPIncentiveEntryGetBilledQty"))
                {
                    fname = "BilledQty";
                }
                else if (string.Equals(MethodName, "MPIncentiveEntrySummaryTotal"))
                {
                    fname = "SummayTotal";
                }
                else if (string.Equals(MethodName, "MPIncentiveEntryGetDataBulk"))
                {
                    fname = "BulkData";
                }
                else if (string.Equals(MethodName, "MPIncentiveEntryGetSummary"))
                {
                    fname = "DBTRegister";
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
                postData += "&MCCCode=" + Uri.EscapeDataString(MCCCode);
                if (!string.IsNullOrEmpty(VLCUploaderCode))
                {
                    postData += "&VLCUploaderCode=" + Uri.EscapeDataString(VLCUploaderCode);
                }
                postData += "&FromDate=" + Uri.EscapeDataString(FromDate);
                if (!string.IsNullOrEmpty(VLCUploaderCode))
                {
                    postData += "&ToDate=" + Uri.EscapeDataString(ToDate);
                }
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
                                    ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/DBT/" + fname + "/" + guid + ".json"), responseData);
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
                    ErrorHandler.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
            }
            return "0";
        }
        public string SaveDataBulk(string code, string guid, List<MPIncentiveEntrySaveDataBulk> obj, string AppUserCode, string AppPwd)
        {
            var responseData = string.Empty;
            try
            {
                string jsondata = JsonConvert.SerializeObject(obj);
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + "MPIncentiveEntrySaveDataBulk";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&APPName=" + Uri.EscapeDataString(Configuration.APPName);
                postData += "&APPVersion=" + Uri.EscapeDataString(Configuration.APPVersion);
                postData += "&APPUserCode=" + Uri.EscapeDataString(AppUserCode);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(AppPwd);
                postData += "&JSonData=" + Uri.EscapeDataString(jsondata);
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
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                if (responseData.Contains("Error"))
                                {
                                    return "Error";
                                }
                                else
                                {
                                    //ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/VLC/BilledQty/" + guid + ".json"), responseData);
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
