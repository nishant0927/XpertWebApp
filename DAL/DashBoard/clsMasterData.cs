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
  public  class clsMasterData
    {
        public static async Task<string> GetData(string methodName, string code)
        {
            try
            {
                string queryParams = $"?methodName={(methodName)}";
               string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetMasterData{queryParams}";
              // string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetMasterData{queryParams}";
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

                        //return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static string GetAllSaleTransactionType(bool DocFinder, string code)
        {
            try
            {
                string queryParams = $"?DocFinder={(DocFinder)}";
                 string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetAllSaleTransactionType{queryParams}";
                //string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetAllSaleTransactionType{queryParams}";
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

                        //return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public static async Task< string> GetLocationByState(string state, string code)
        {
            try
            {
                string queryParams = $"?state={(state)}";
                 string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetLocationByState{queryParams}";
                //string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetLocationByState{queryParams}";
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

                        //return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static async Task<string> ItemGroup(string code)
        {
            try
            {
                //string queryParams = $"?methodName={(methodName)}";
                 string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/ItemGroup";
               // string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/ItemGroup";
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

                        //return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public static async Task<string> GetZOneByCustomer(string customer, string code)
        {
            try
            {
                string queryParams = $"?customer={(customer)}";
                string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/GetZOneByCustomer{queryParams}";
               // string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetZOneByCustomer{queryParams}";
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

                        //return $"Error: {response.StatusCode}";
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
