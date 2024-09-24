using DAL.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DashBoard
{
   public class clsSalesData
    {
        public static async Task<string> GetSalesSummary(string txtRoute, string fromDate, string dtpToDate,string code)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?txtRoute={txtRoute}" +
                                     $"&fromDate={fromDate}" +
                                     $"&dtpToDate={dtpToDate}";


                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetSalesSummary{queryParams}";
                //string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetSalesSummary{queryParams}";

                using (var client = new HttpClient())
                {
                    // Optionally set headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Increase timeout if needed
                    client.Timeout = TimeSpan.FromMinutes(5); // Adjust if required

                    // Call the API method asynchronously
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Return the API response content as a string
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();

                        // Parse the JSON content
                        JObject errorObject = JObject.Parse(errorContent);

                        // Access the "Error" property directly
                        string errorMessage = errorObject["error"]?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new Exception(errorMessage);
                        }
                        else
                        {
                            throw new Exception(errorContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string GetDetailDailyStatement(bool rbtnProduct, string txtDate, bool rbtnMilkType, bool rbtnDistributorWise, bool rbtnDisRouteWise, string CurrentUser, bool rbtnRouteWise,string code)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?rbtnProduct={(rbtnProduct)}" +
                                       $"&txtDate={(txtDate)}" +
                                     $"&rbtnMilkType={rbtnMilkType}" +
                                     $"&rbtnDistributorWise={rbtnDistributorWise}" +
                                     $"&rbtnDisRouteWise={rbtnDisRouteWise}" +
                                     $"&CurrentUser={CurrentUser}" +
                                     $"&rbtnRouteWise={rbtnRouteWise}";


                // Construct the full API URL with the query string
               // string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetDetailDailyStatement{queryParams}";
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetDetailDailyStatement{queryParams}";

                using (var client = new HttpClient())
                {
                    // Optionally set headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call the API method using the constructed URL
                    HttpResponseMessage response = client.GetAsync(apiUrl).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        // Return the API response content as a string
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                    else
                    {
                        string errorContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        // Parse the JSON content
                        JObject errorObject = JObject.Parse(errorContent);

                        // Access the "Error" property directly
                        string errorMessage = errorObject["error"]?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new Exception(errorMessage);
                        }
                        else
                        {
                            throw new Exception("Error:" + response.StatusCode + "Message:" + errorContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static string GetSummaryOfDailyStatement(string txtDate, string CurrentUser, string code)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?txtDate={(txtDate)}" +
                                       $"&CurrentUser={(CurrentUser)}";


                // Construct the full API URL with the query string
                // string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetSummaryOfDailyStatement{queryParams}";
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetSummaryOfDailyStatement{queryParams}";

                using (var client = new HttpClient())
                {
                    // Optionally set headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call the API method using the constructed URL
                    HttpResponseMessage response = client.GetAsync(apiUrl).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        // Return the API response content as a string
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                    else
                    {
                        string errorContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        // Parse the JSON content
                        JObject errorObject = JObject.Parse(errorContent);

                        // Access the "Error" property directly
                        string errorMessage = errorObject["error"]?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            throw new Exception(errorMessage);
                        }
                        else
                        {
                            throw new Exception("Error:" + response.StatusCode + "Message:" + errorContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
