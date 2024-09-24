using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DAL.ServiceReference1;
using DAL.Common;
using System.Net.Http;
using System.Net;
using System.IO;
using DAL.Models;
using System.Web.Hosting;
using DAL.User;
using Newtonsoft.Json;

namespace DAL.Location
{
    public class Locations
    {
        public void GetAppLocation()
        {
            var responseData = string.Empty;
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Configuration.MilkUrlLocation);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
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
                        // Check if the request was successful (status code 2xx)
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Read the response content
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                responseData = reader.ReadToEnd();
                                ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/Location/APPLocation.json"), responseData);
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
            //return responseData;
        }




        public string CheckLoginDetails(string userId, string Pwd, string code)
        {
            //string lCode = "2024";
            var responseData = string.Empty;
            try
            {
                string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + "CheckLoginDetails";
               // string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + Configuration.LCode + Configuration.MilkUrl.Split(':')[2] + "CheckLoginDetails";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(Configuration.key);
                postData += "&APPName=" + Uri.EscapeDataString(Configuration.APPName);
                postData += "&APPVersion=" + Uri.EscapeDataString(Configuration.APPVersion);
                postData += "&APPUserCode=" + Uri.EscapeDataString(userId);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(Pwd);
                postData += "&UserID=" + Uri.EscapeDataString(userId);
                postData += "&PWD=" + Uri.EscapeDataString(Pwd);
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
                                    return "Error";
                                }
                                else
                                {
                                    UserModel obj = JsonConvert.DeserializeObject<UserModel>(responseData);
                                    if (string.Equals(obj.LoginData[0].User_APP_Type, "V")|| (string.Equals(obj.LoginData[0].User_APP_Type, "A")))
                                    {
                                        Guid guid = Guid.NewGuid();
                                        ReadWriteFile.SaveFile(HostingEnvironment.MapPath("~/App_Data/User/" + guid + ".json"), responseData);
                                        return guid.ToString();
                                    }
                                    else
                                    {
                                        return "Error";
                                    }

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
