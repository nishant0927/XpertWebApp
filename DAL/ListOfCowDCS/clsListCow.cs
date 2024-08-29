using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL.Common;

namespace DAL.ListOfCowDCS
{
  public  class clsListCow
    {
        public static string GetListCow( string mccCode,string methodName,string code)
        {
            //string url = "http://192.168.29.250";
            //string portNo = "800";
            //string Service = "MilkProcurement.asmx";
            //string apiUrl = $"{url}:{portNo}/{Service}/{methodName}";
            // string key = "Tecxpert@MP#123$456%789^";
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";
            //string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + methodName;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },
                  
                    { "mccCode", mccCode }

                };
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    response.EnsureSuccessStatusCode();

                    string responseData = response.Content.ReadAsStringAsync().Result;

                    if (responseData.Contains("false"))
                    {
                        throw new Exception(responseData);
                    }

                    return responseData;
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        public static string GetMCC( string methodName,string code)
        {
            //string lCode = "767";
            string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/{methodName}";
            //    string url = "http://192.168.29.250";
            //    string portNo = "800";
            //string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + methodName;
            //string Service = "MilkProcurement.asmx";
            //string apiUrl = $"{url}:{portNo}/{Service}/{methodName}";
            //string key = "Tecxpert@MP#123$456%789^";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },

                   

                };
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    response.EnsureSuccessStatusCode();

                    string responseData = response.Content.ReadAsStringAsync().Result;

                    if (responseData.Contains("false"))
                    {
                        throw new Exception(responseData);
                    }

                    return responseData;
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        public static string GetSalesData(string methodName, string date)
        {
            string url = "http://192.168.29.250";
            string portNo = "800";
            string Service = "MilkProcurement.asmx";
            string apiUrl = $"{url}:{portNo}/{Service}/{methodName}";
            string key = "Tecxpert@MP#123$456%789^";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", key },

                    { "date", date },

                };
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    response.EnsureSuccessStatusCode();

                    string responseData = response.Content.ReadAsStringAsync().Result;

                    if (responseData.Contains("false"))
                    {
                        throw new Exception(responseData);
                    }

                    return responseData;
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    }


}
