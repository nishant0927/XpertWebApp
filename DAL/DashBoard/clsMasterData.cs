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
               string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetMasterData{queryParams}";
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
