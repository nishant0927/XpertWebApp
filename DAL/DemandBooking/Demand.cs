using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace DAL.DemandBooking
{
  public  class Demand
    {
        public static async Task< string> GetRout(string code)
        {
            try
            {
                string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetRout";
                using (var client = new HttpClient())
                {
                    // Optionally set headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call the API method using the constructed URL
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Return the API response content as a string
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch(Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
        public static string GetDocNo(string routCode, bool rbtnMorning, bool fresh, bool product, bool both, string date, NavigatorType NavType, bool SeparateDemandMilkandProduct,string code)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?routCode={Uri.EscapeDataString(routCode)}" +
                                     $"&rbtnMorning={rbtnMorning}" +
                                     $"&fresh={fresh}" +
                                     $"&product={product}" +
                                     $"&both={both}" +
                                     $"&date={Uri.EscapeDataString(date)}" +
                                     $"&NavType={(int)NavType}" + // Assuming NavType is an enum, casting it to an int
                                    // $"&currentUserLocation={Uri.EscapeDataString(currentUserLocation)}" +
                                     $"&SeparateDemandMilkandProduct={SeparateDemandMilkandProduct}";

                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetDocNo{queryParams}";

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
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }



        public static string GetLoadData(string strDocumentNo, NavigatorType NavType, string currentUserLocation,string code)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"strDocumentNo={Uri.EscapeDataString(strDocumentNo)}" +
                                     $"&NavType={(int)NavType}" + // Assuming NavType is an enum, casting it to an int                                                                 
                                    $"&currentUserLocation={Uri.EscapeDataString(currentUserLocation)}";

                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/LoadData{queryParams}";

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
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
