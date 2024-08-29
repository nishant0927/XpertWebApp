using DAL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DashBoard
{
    public class ResponseData
    {
        public string Result { get; set; }
        public string Response { get; set; }
    }
    public class FarmerDetail
    {
        public string Union_Name { get; set; }
        public string MP_Code { get; set; }
        public string MP_Name { get; set; }
        public string VLC_Code_VLC_Uploader { get; set; }
        public string VLC_Name { get; set; }
        public string Jan_Aadhar_No_Verified { get; set; }
        public string Telphone { get; set; }
        public string BankName { get; set; }
        public string IFCICode { get; set; }
        public string AccountNO { get; set; }
        public string Father_Name { get; set; }
        public string JA_janaadhaarId { get; set; }
        public string JA_aadhar { get; set; }
       
    }


    public class clsDashBoard
    {
        public static async Task<string> GetDataAsync(string methodName,  string fromDate, string toDate, bool chkRJSBNS, string currentUserCode,string code)        
        {

            
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
            {
                { "key", Configuration.key },
                { "fromDate", fromDate },
                { "toDate", toDate },
                { "chkRJSBNS", chkRJSBNS.ToString() },
                { "currentUserCode", currentUserCode },
            };
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();

                    if (responseData.Contains("false"))
                    {
                        throw new Exception(responseData);
                    }

                    return await Task.Run(()=> responseData);
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



        public static string GetMenuPermission(string methodName, string currentUserCode,string code)
        {
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";
            //string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },                   
                    { "currentUserCode", currentUserCode },
                    

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


        public static string CheckLoginDetails(string userId, string Pwd)
        {
            string responseData = string.Empty;
            try
            {
                string key = "Tecxpert@MP#123$456%789^";
                string methodName = "CheckLoginDetails";
                string url = "http://localhost";
                string portNo = "2024";
                string Service = "MISWebService.asmx";
                string apiUrl = $"{url}:{portNo}/{Service}/{methodName}";
                // string apiUrl = Configuration.location + ":" + code + "/" + Configuration.serviceName + "/" + methodName;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "Key=" + Uri.EscapeDataString(key);
                postData += "&APPUserCode=" + Uri.EscapeDataString(userId);
                postData += "&APPUserPwd=" + Uri.EscapeDataString(Pwd);
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postDataBytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Read the response content
                        using (Stream responseStream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseData = reader.ReadToEnd();
                            if (responseData.Contains("false"))
                            {
                                throw new Exception(responseData);
                            }
                            else
                            {


                                return responseData;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return responseData;
        }


        public static string GetDBTData(string fromDate, string toDate, string currentUserCode,string methodName,string code)
        {
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";
            //string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + methodName;
            //string apiUrl = $"{Configuration.MilkUrl}:{portNo}/{Service}/{methodName}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },

                    { "fromDate", fromDate },
                    { "toDate", toDate },                   
                    { "currentUserCode", currentUserCode },

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


        public static string GetSalesAndMarketingData(string fromDate, string toDate, string currentUserCode, string methodName, string code)
        {
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";           

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },

                    { "fromDate", fromDate },
                    { "toDate", toDate },
                    { "currentUserCode", currentUserCode },

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

        public static string GetItemWiseDetails( string fromDate, string toDate, string currentUserCode, bool isSchemeItem,string code,string methodName)
        {
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";
            //string apiUrl = Configuration.MilkUrl.Split(':')[0] + ":" + Configuration.MilkUrl.Split(':')[1] + ":" + code + Configuration.MilkUrl.Split(':')[2] + methodName;
            //string apiUrl = $"{Configuration.MilkUrl}:{portNo}/{Service}/{methodName}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },

                    { "fromDate", fromDate },
                    { "toDate", toDate },
                    { "currentUserCode", currentUserCode },

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


        public static string GetFarmerDetails(string methodName,string code)
        {
            //string key = "Tecxpert@MP#123$456%789^";
            //string methodName = "CheckLoginDetails";
            //string url = "http://localhost";
            //string portNo = "800";
            //string Service = "WebXpertERP.asmx";
            //string apiUrl = $"{url}:{portNo}/{Service}/{methodName}";
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key }

                  

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


        public static string GetUnionByFarmerDetails(string methodName, string code,string unionName)
        {
            //string key = "Tecxpert@MP#123$456%789^";
            //string methodName = "CheckLoginDetails";
            //string url = "http://localhost";
            //string portNo = "800";
            //string Service = "WebXpertERP.asmx";
            //string apiUrl = $"{url}:{portNo}/{Service}/{methodName}";
            string apiUrl = $"{Configuration.DashBoardUrl}:{code}/{Configuration.ServiceName}/{methodName}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                {
                    { "key", Configuration.key },
                    { "unionName", unionName }



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


        public static async Task<string> GetTankerMaster()
        {
            try
            {
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetTankerMaster";
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
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
        public static  string GetDailyQuantity(bool rbtnTranpoterGainLoss, bool rdbSummary, bool rbtnDock, bool rbtnDockDateWise, bool rbtnDockShiftWise, bool rbtnBmcSummary, bool rdbDetails, string fromDate, string toDate, string formId, string status, string fndArea, string mccCode, bool isPickCLRInsteadOfSNF, bool rbtnTranspoterGainlossSummary, decimal allowFat, decimal allowsnf, string tankerNo, bool rdbTankerWise, bool rbtnRouteWise, bool rdbCollectionWise, bool rdbMultiple, bool rbtnBMCTankerCollection)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?rbtnTranpoterGainLoss={(rbtnTranpoterGainLoss)}" +
                                       $"&rdbSummary={(rdbSummary)}" +
                                     $"&rbtnDock={rbtnDock}" +
                                     $"&rbtnDockDateWise={rbtnDockDateWise}" +
                                     $"&rbtnDockShiftWise={rbtnDockShiftWise}" +
                                     $"&rbtnBmcSummary={rbtnBmcSummary}" +
                                     $"&rdbDetails={rdbDetails}" +
                                     $"&fromDate={fromDate}" +
                                     $"&toDate={toDate}" +
                                     $"&formId={formId}" +
                                     $"&status={status}" +
                                     $"&fndArea={fndArea}" +
                                     $"&mccCode={mccCode}" +
                                     $"&isPickCLRInsteadOfSNF={isPickCLRInsteadOfSNF}" +
                                     $"&rbtnTranspoterGainlossSummary={rbtnTranspoterGainlossSummary}" +
                                     $"&allowFat={(decimal)allowFat}" +
                                     $"&allowsnf={(decimal)allowsnf}" +
                                     $"&tankerNo={tankerNo}" +
                                     $"&rdbTankerWise={rdbTankerWise}" +
                                     $"&rbtnRouteWise={rbtnRouteWise}" +
                                     $"&rdbCollectionWise={rdbCollectionWise}" +
                                     $"&rdbMultiple={rdbMultiple}" +
                                     $"&rbtnBMCTankerCollection={rbtnBMCTankerCollection}";

                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetDailyQuantity{queryParams}";

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
                        throw new Exception( $"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception( $"Exception: {ex.Message}");
            }
        }

        public static async Task<string> GetData(string methodName)
        {
            try
            {
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/{methodName}";
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
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }



        public static string GetPaymentCycle(string fromDate, string toDate, string mccCode, string routeCode, string dcsCode, string zoneCode, string bankCode, bool hold, bool unHold, bool all, bool showData, bool outStanding, bool headLoad, bool paymentSummary, string PaymentCycleCode, string CurrComp_Code1, string CurrentCompanyCode, string CurrentCompanyName, string CurrentUser)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?fromDate={(fromDate)}" +
                                       $"&toDate={(toDate)}" +
                                     $"&mccCode={mccCode}" +
                                     $"&routeCode={routeCode}" +
                                     $"&dcsCode={dcsCode}" +
                                     $"&zoneCode={zoneCode}" +
                                     $"&bankCode={bankCode}" +
                                     $"&fromDate={fromDate}" +
                                     $"&hold={hold}" +
                                     $"&unHold={unHold}" +
                                     $"&all={all}" +
                                     $"&showData={showData}" +
                                     $"&outStanding={outStanding}" +
                                     $"&headLoad={headLoad}" +
                                     $"&paymentSummary={paymentSummary}" +
                                     $"&PaymentCycleCode={PaymentCycleCode}" +
                                     $"&CurrComp_Code1={CurrComp_Code1}" +
                                     $"&CurrentCompanyCode={CurrentCompanyCode}" +
                                     $"&CurrentCompanyName={CurrentCompanyName}" +
                                     $"&CurrentUser={CurrentUser}";

                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetPaymentCycleWiseData{queryParams}";

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
                        throw new Exception($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }


        public static string GetMccMilkRegister(string fromDate, string todate, string fromShift, string toShift, string cboSRNAmounType, string mcc, string area, string route, string dcs, bool chkDateShift, bool rbtnCollectionSummary, bool chkRejection, bool chkShiftWise, bool chkOnlyRejection, bool AreaWiseBilling, bool ChkDetailWise, bool rbtnVLCWise, bool chkRoutewise, bool ChkMCCWise, bool rbtnPlantWise, bool rbtnZoneWise, bool chkVLCWisePayable, bool rdbPlantWisePaymentSummary, bool rdoVLCWisePaymentSummary, bool chkDairyMilkReportPrint, bool chkRouteShiftWise, bool rbtnBMC, bool rbtnTotal, bool rbtnShiftWiseTotal, bool rbtnDCS, bool rbtnRoute, string CurrComp_Code1, string cboMilkReceiveUOM, int PricePlan, bool chkShowVLCUploaderData, string CurrentCompanyCode, string currentUserCode)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?fromDate={(fromDate)}" +
                                       $"&todate={(todate)}" +
                                     $"&fromShift={fromShift}" +
                                     $"&toShift={toShift}" +
                                     $"&cboSRNAmounType={cboSRNAmounType}" +
                                     $"&mcc={mcc}" +
                                     $"&area={area}" +
                                     $"&route={route}" +
                                     $"&dcs={dcs}" +
                                     $"&chkDateShift={chkDateShift}" +
                                     $"&rbtnCollectionSummary={rbtnCollectionSummary}" +
                                     $"&chkRejection={chkRejection}" +
                                     $"&chkShiftWise={chkShiftWise}" +
                                     $"&chkOnlyRejection={chkOnlyRejection}" +
                                     $"&AreaWiseBilling={AreaWiseBilling}" +
                                     $"&ChkDetailWise={ChkDetailWise}" +
                                     $"&rbtnVLCWise={rbtnVLCWise}" +
                                     $"&chkRoutewise={chkRoutewise}" +
                                     $"&ChkMCCWise={ChkMCCWise}" +
                                     $"&rbtnPlantWise={rbtnPlantWise}" +
                                     $"&rbtnZoneWise={rbtnZoneWise}" +
                                     $"&chkVLCWisePayable={chkVLCWisePayable}" +
                                     $"&rdbPlantWisePaymentSummary={rdbPlantWisePaymentSummary}" +
                                     $"&chkDairyMilkReportPrint={chkDairyMilkReportPrint}" +
                                     $"&chkRouteShiftWise={chkRouteShiftWise}" +
                                     $"&rbtnBMC={rbtnBMC}" +
                                     $"&rbtnTotal={rbtnTotal}" +
                                     $"&rbtnDCS={rbtnDCS}" +
                                     $"&rbtnRoute={rbtnRoute}" +
                                     $"&CurrComp_Code1={CurrComp_Code1}" +
                                     $"&cboMilkReceiveUOM={cboMilkReceiveUOM}" +
                                     $"&PricePlan={PricePlan}" +
                                     $"&chkShowVLCUploaderData={chkShowVLCUploaderData}" +
                                     $"&CurrentCompanyCode={CurrentCompanyCode}" +
                                     $"&currentUserCode={currentUserCode}";

                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetMCCMilkRegister{queryParams}";

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
                        throw new Exception($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }


        public static string GetLedgerData(string CurrentCompanyCode, string dtpLedgerFromDate, string dtpLedgerToDate, string CurrComp_Code1, string CurrentUser)
        {
            try
            {
                // Construct the query string with parameters
                string queryParams = $"?CurrentCompanyCode={(CurrentCompanyCode)}" +
                                       $"&dtpLedgerFromDate={(dtpLedgerFromDate)}" +
                                     $"&dtpLedgerToDate={dtpLedgerToDate}" +
                                     $"&CurrComp_Code1={CurrComp_Code1}" +
                                     $"&CurrentUser={CurrentUser}";
                                    

                // Construct the full API URL with the query string
                string apiUrl = $"{Configuration.DashBoardUrl}:{Configuration.LCode}/{Configuration.ServiceName}/GetLedgerData{queryParams}";

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
                        throw new Exception($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }
    }

    public class clsDCS
    {
        public int Sno;
        public string Union_Name { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string RegisteredDCS { get; set; }
        public decimal DCSQTY1 { get; set; }
        public decimal FATKG1 { get; set; }
        public decimal SNFKG1 { get; set; }
        public string PDCS { get; set; }
        public decimal DCSQTY2 { get; set; }
        public decimal FATKG2 { get; set; }
        public decimal SNFKG2 { get; set; }
        public decimal TotalDCS { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalFatkg { get; set; }
        public decimal Totalsnfkg { get; set; }
        public string UserName { get; set; }
        public string Union_Contact_Person { get; set; }
        public string Union_Contact_PhoneNo { get; set; }

        public static List<clsDCS> GetDCSData(string methodName, string fromDate, string toDate, bool chkRJSBNS, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                //response = clsDashBoard.GetDataAsync(methodName,fromDate,toDate,chkRJSBNS,currentUserCode,code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var GetDcsData = JsonConvert.DeserializeObject<List<clsDCS>>(responseData.Response);

                return GetDcsData;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class clsRout
    {
        public int Sno { get; set; }
        public string Union_Name { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public decimal RouteCount { get; set; }
        public decimal MCCCount { get; set; }
        public decimal Milk_WeightProc { get; set; }
        public decimal FATKGProc { get; set; }
        public decimal SNFKGProc { get; set; }

        public string UserName { get; set; }
        public string Union_Contact_Person { get; set; }
        public string Union_Contact_PhoneNo { get; set; }

        public static List<clsRout> GetRoutData(string methodName, string fromDate, string toDate, bool chkRJSBNS, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                //response = clsDashBoard.GetDataAsync(methodName, fromDate, toDate, chkRJSBNS, currentUserCode,code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var GetRout = JsonConvert.DeserializeObject<List<clsRout>>(responseData.Response);

                return GetRout;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class clsUnion
    {
        public int Sno { get; set; }
        public string Union_Name { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate;
        public decimal Milk_WeightProc { get; set; }
        public decimal FATKGProc { get; set; }
        public decimal SNFKGProc { get; set; }
        public decimal FATPerProc { get; set; }
        public decimal SNFPerProc { get; set; }
        public string UserName { get; set; }
        public string Union_Contact_Person { get; set; }
        public string Union_Contact_PhoneNo { get; set; }

        public static List<clsUnion> GetUnionData(string methodName, string fromDate, string toDate, bool chkRJSBNS, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                //response = clsDashBoard.GetDataAsync(methodName, fromDate, toDate, chkRJSBNS, currentUserCode,code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var GetUnion = JsonConvert.DeserializeObject<List<clsUnion>>(responseData.Response);

                return GetUnion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class clsDBTUnion
    {
        public int SNo { get; set; }
        public string Union_Name { get; set; }
        public string Union_Contact_Person { get; set; }
        public string Union_Contact_PhoneNo { get; set; }
        public int DCS_Count { get; set; }
        public int Farmer_Count { get; set; }
        public decimal DCS_Billed_Qty { get; set; }
        public decimal Farmer_Qty { get; set; }
        public decimal NEFT_Amount { get; set; }
        public decimal MismatchQty { get; set; }

        public static List<clsDBTUnion> GetUnionWiseDBTSummary(string methodName, string fromDate, string toDate, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                response = clsDashBoard.GetDBTData(fromDate,toDate,currentUserCode,methodName, code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var getUnion = JsonConvert.DeserializeObject<List<clsDBTUnion>>(responseData.Response);

                return getUnion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<clsDBTUnion> GetUnionWiseDBTMisMatchQty(string methodName, string fromDate, string toDate, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                response = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCode, methodName,code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var getUnionWiseMisMatch = JsonConvert.DeserializeObject<List<clsDBTUnion>>(responseData.Response);

                return getUnionWiseMisMatch;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    public class DBTPaymentStatus
    {
        public int SNo { get; set; }
        public string Union_Name { get; set; }
        public string Union_Contact_Person { get; set; }
        public string Union_Contact_PhoneNo { get; set; }
        public int Total_Farmer_Count { get; set; }
        public decimal Total_NEFT_Amount { get; set; }
        public int Success_Farmer_Count { get; set; }
        public decimal Success_NEFT_Amount { get; set; }
        public int Reject_Farmer_Count { get; set; }
        public decimal Reject_NEFT_Amount { get; set; }
        public int NoResponse_Farmer_Count { get; set; }
        public decimal NoResponse_NEFT_Amount { get; set; }


        public static List<DBTPaymentStatus> GetBTPaymentStatuses(string methodName, string fromDate, string toDate, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                response = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCode, methodName,code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var getPaymentStatus = JsonConvert.DeserializeObject<List<DBTPaymentStatus>>(responseData.Response);

                return getPaymentStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class UnionWiseLastDBTJanAadhar
    {
        public int SNo { get; set; }
        public string Union_Name { get; set; }
        public string Union_Contact_Person { get; set; }
        public string Union_Contact_PhoneNo { get; set; }
        public int Farmer_Count { get; set; }
        public string Jan_Aaadhar_Verified_No { get; set; }
        public int Aadhar_Verified { get; set; }
        public string Last_DBT_Cycle { get; set; }
        public string Last_Approved_By_RCDF { get; set; }
        public static List<UnionWiseLastDBTJanAadhar> GetUnionWiseLastDBTJanAadhars(string methodName, string fromDate, string toDate, string currentUserCode,string code)
        {
            string response = string.Empty;
            try
            {
                response = clsDashBoard.GetDBTData(fromDate, toDate, currentUserCode, methodName,code);
                var responseDataList = JsonConvert.DeserializeObject<List<ResponseData>>(response);
                var responseData = responseDataList[0];
                var getJanAadhar = JsonConvert.DeserializeObject<List<UnionWiseLastDBTJanAadhar>>(responseData.Response);

                return getJanAadhar;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    }
