using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.ListOfCowDCS;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using DAL.DashBoard;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Drawing;
using System.Threading.Tasks;
using DAL;
using Rectangle = iTextSharp.text.Rectangle;
using DAL.Common;

namespace XpertWebApp.Controllers
{
    public class DailyStatememtController : Controller
    {
        // GET: DailyStatememt
        public ActionResult DailyStatementReport()
        {

            if (Session["AppUserCode"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetDetailData(bool rbtnProduct, string txtDate, bool rbtnMilkType, bool rbtnDistributorWise, bool rbtnDisRouteWise, bool rbtnRouteWise)
        {
            try
            {
                var responseData = clsSalesData.GetDetailDailyStatement(rbtnProduct, txtDate, rbtnMilkType, rbtnDistributorWise, rbtnDisRouteWise, Session["AppUserCode"].ToString(), rbtnRouteWise, Session["Port"].ToString());
                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseData);
                
                if (jsonObject.Count > 0)
                {
                   
                    StringBuilder sbr = new StringBuilder();
                    
                    if (rbtnRouteWise)
                    {
                        if (rbtnMilkType)
                        {
                            GetDailyStatementRouteAndDistributorWiseDetailCredit(sbr, jsonObject);
                        }
                        else if (rbtnProduct)
                        {
                            DailyStatementRouteWiseDetailcreditProduct(sbr, jsonObject);
                        }
                    }
                    else if (rbtnDistributorWise)
                    {
                        if (rbtnMilkType)
                        {
                            DailyStatementDistributorWiseDetailcreditMilk(sbr, jsonObject);
                        }
                        else if (rbtnProduct)
                        {
                            
                        }
                    }
                    else if (rbtnDisRouteWise)
                    {
                        if (rbtnMilkType)
                        {
                            crptDailyStatementDistributorRouteWiseDetailcreditMilk(sbr, jsonObject);
                        }
                        else if (rbtnProduct)
                        {

                        }
                    }
                   



                    string html = sbr.ToString();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        //CustomPdfPageEvent pageEventHelper = new CustomPdfPageEvent(companyName, reportName, reportDate, headerTable);
                        //writer.PageEvent = pageEventHelper;
                        pdfDoc.Open();

                        // Write the body content
                        var bodyReader = new StringReader(sbr.ToString());
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, bodyReader);

                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "DCS_Ledger.pdf");
                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetSummaryOfDailyStatement(string txtDate)
        {
            try
            {
                var responseData = clsSalesData.GetSummaryOfDailyStatement(txtDate,  Session["AppUserCode"].ToString(), Session["Port"].ToString());
                //var jsonObject = JsonConvert.DeserializeObject<JObject>(responseData);
                JArray jsonObject = JArray.Parse(responseData);
                var columnTotals = new Dictionary<string, decimal>();
                decimal sumFinal = 0;
                decimal sumMAmount = 0;
                if (jsonObject.Count > 0)
                {
                    //var allRoute = jsonObject["All Routes"];
                    //var particular = jsonObject["Particular"];
                    var tableHeader = jsonObject
      .Where(item => item["Short_Description"] != null && item["Sku_Seq"] != null)
      .Select(item => new
      {
          sorfDec = item["Short_Description"].ToString(),
          skuSeq = item["Sku_Seq"].ToString()
      })
      .GroupBy(item => item.sorfDec)
      .Select(group => group.First()) // Take the first element from each group
      .OrderBy(item => item.skuSeq)   // Order by skuSeq
      .ToList();
                    //var tableHeader = allRoute.Select(item => new
                    //{
                    //    sorfDec = item["Short_Description"].ToString()
                    //}).Distinct().OrderBy(item["Sku_Seq"].ToString()).ToList();
                    var groupedData = jsonObject.GroupBy(item => item["ShiftType"].ToString());
                    //var particularGroupedData = particular.GroupBy(item => item["Cust_Code"].ToString());
                    string compCode = jsonObject[0]["Comp_Name"].ToString() + " ," + jsonObject[0]["City_Code"].ToString();
                    StringBuilder sbr = new StringBuilder();
                    sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='margin: 0; padding: 0;'>");
                    sbr.Append("<tr>");
                    sbr.Append(" <td style='text-align:center;font-weight:bold; font-size:14px'><span style='font-size:17px'>" + compCode + "</span><br/>" + jsonObject[0]["Add1"].ToString() + "<br/>(" + jsonObject[0]["State"].ToString() + ".)- " + jsonObject[0]["Pincode"].ToString() + " <br/> Daily Statement Route Wise</td >");
                    sbr.Append("<td style='text-align:right;font-weight:bold; font-size:14px'>Fax:- " + jsonObject[0]["Fax"].ToString() + "<br/>Phone No:" + jsonObject[0]["Phone1"].ToString() + "<br/> Date:-" + jsonObject[0]["Date"].ToString() + "</td>");
                    sbr.Append("</tr>");
                    sbr.Append("</table>");
                    sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;'>");
                    sbr.Append("<thead>");
                    sbr.Append("<tr>");
                    sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particular</th>");
                    foreach (var header in tableHeader)
                    {
                        sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
                    }
                    sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Qty(L) |Total Qty(Kg) </th>");
                    sbr.Append("</tr>");
                    sbr.Append("</thead>");
                    foreach (var group in groupedData)
                    {
                        sbr.Append("<tbody><tr>");
                        sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["ShiftType"].ToString() + "</td>");
                        foreach (var header in tableHeader)
                        {
                            // Check if there's a matching record with this Short_Description for this Route_No
                            var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                            if (matchingRecord != null)
                            {
                                // Get the value of "final" and add it to the cell
                                var finalValue = decimal.Parse(matchingRecord["TotalLtr_ItemWise"].ToString()); // Assuming "final" is numeric

                                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                                // Add the value to the corresponding column total
                                if (columnTotals.ContainsKey(header.sorfDec))
                                {
                                    columnTotals[header.sorfDec] += finalValue;
                                }
                                else
                                {
                                    columnTotals[header.sorfDec] = finalValue;
                                }
                            }
                            else
                            {
                                // No matching record for this header, so add 0 in the cell
                                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                                // Ensure the column total is updated (in case it's the first row)
                                if (!columnTotals.ContainsKey(header.sorfDec))
                                {
                                    columnTotals[header.sorfDec] = 0;
                                }
                            }
                        }

                        var totalfinal = group.Sum(item => (decimal)item["TotalLtr_ItemWise"]);
                        var totalMAmt = group.Sum(item => (decimal)item["Kg_Qty"]);
                        
                        sumFinal += totalfinal;
                        sumMAmount += totalMAmt;
                       
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " </td>");
                        sbr.Append("</tr></tbody>");
                    }
                    sbr.Append("<tfoot>");
                    sbr.Append("<tr>");
                    sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
                    foreach (var header in tableHeader)
                    {
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            // Display the total for this column
                            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                        }
                        else
                        {
                            // If for some reason the column doesn't have a total, just show 0.00
                            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                        }
                    }
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "</td>");
                    sbr.Append("</tr>");
                    sbr.Append("</tfoot>");
                    sbr.Append("</table>");                 



                    string html = sbr.ToString();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        //CustomPdfPageEvent pageEventHelper = new CustomPdfPageEvent(companyName, reportName, reportDate, headerTable);
                        //writer.PageEvent = pageEventHelper;
                        pdfDoc.Open();

                        // Write the body content
                        var bodyReader = new StringReader(sbr.ToString());
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, bodyReader);

                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "DCS_Ledger.pdf");
                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "No data available for the selected criteria." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public void GetDailyStatementRouteAndDistributorWiseDetailCredit(StringBuilder sbr, JObject jsonObject)
        {
            var columnTotals = new Dictionary<string, decimal>();
            decimal sumFinal = 0;
            decimal sumMAmount = 0;
            decimal sumProAmt = 0;
            decimal sumtotalAmount = 0;
            var allRoute = jsonObject["All Routes"];
            var particular = jsonObject["Particular"];
            var tableHeader = allRoute
.Where(item => item["Short_Description"] != null && item["Sku_Seq"] != null)
.Select(item => new
{
  sorfDec = item["Short_Description"].ToString(),
  skuSeq = item["Sku_Seq"].ToString()
})
.GroupBy(item => item.sorfDec)
.Select(group => group.First())
.OrderBy(item => item.skuSeq)
.ToList();

            var groupedData = allRoute.GroupBy(item => item["Route_No"].ToString());
            var particularGroupedData = particular.GroupBy(item => item["Cust_Code"].ToString());
            string compCode = allRoute[0]["Comp_Name"].ToString() + " ," + allRoute[0]["City_Code"].ToString();
            //StringBuilder sbr = new StringBuilder();
            sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='margin: 0; padding: 0;'>");
            sbr.Append("<tr>");
            sbr.Append(" <td style='text-align:center;font-weight:bold; font-size:14px'><span style='font-size:17px'>" + compCode + "</span><br/>" + allRoute[0]["Add1"].ToString() + "<br/>(" + allRoute[0]["State"].ToString() + ".)- " + allRoute[0]["Pincode"].ToString() + " <br/> Daily Statement Route Wise</td >");
            sbr.Append("<td style='text-align:right;font-weight:bold; font-size:14px'>Fax:- " + allRoute[0]["Fax"].ToString() + "<br/>Phone No:" + allRoute[0]["Phone1"].ToString() + "<br/> Date:-" + allRoute[0]["Date"].ToString() + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</table>");
            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>All Route's</th>");
            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            foreach (var group in groupedData)
            {
                sbr.Append("<tbody><tr>");
                //if (rbtnDistributorWise)
                //{
                //    sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Customer_Name"].ToString() + "</td>");
                ////}
                //else if (rbtnRouteWise)
                //{
                    sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Route_Desc1"].ToString() + "</td>");
                //}

                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["final"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }
                }

                var totalfinal = group.Sum(item => (decimal)item["final"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");
            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");
            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;margin-top:10px;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particulars</th>");

            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            sumFinal = 0;
            sumMAmount = 0;
            sumProAmt = 0;
            sumtotalAmount = 0;
            foreach (var group in particularGroupedData)
            {
                sbr.Append("<tbody><tr>");
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Cust_Code"].ToString() + "</td>");
                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["TotalLtr_ItemWise"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }
                    //if (matchingRecord != null)
                    //{
                    //    // Add QTYLtr value if a match is found
                    //    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + matchingRecord["final"].ToString() + "</td>");
                    //}
                    //else
                    //{
                    //    // No matching record for this header, so add 0 in the cell
                    //    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0</td>");
                    //}
                }
                var totalfinal = group.Sum(item => (decimal)item["Final_Qty"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");

            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");

        }


        public void DailyStatementDistributorWiseDetailcreditMilk(StringBuilder sbr, JObject jsonObject)
        {
            var columnTotals = new Dictionary<string, decimal>();
            decimal sumFinal = 0;
            decimal sumMAmount = 0;
            decimal sumProAmt = 0;
            decimal sumtotalAmount = 0;
            var allRoute = jsonObject["All Routes"];
            var particular = jsonObject["Particular"];
            var tableHeader = allRoute
.Where(item => item["Short_Description"] != null && item["Sku_Seq"] != null)
.Select(item => new
{
    sorfDec = item["Short_Description"].ToString(),
    skuSeq = item["Sku_Seq"].ToString()
})
.GroupBy(item => item.sorfDec)
.Select(group => group.First())
.OrderBy(item => item.skuSeq)
.ToList();

            var groupedData = allRoute.GroupBy(item => item["Cust_Code"].ToString());
            var particularGroupedData = particular.GroupBy(item => item["Cust_Code"].ToString());
            string compCode = allRoute[0]["Comp_Name"].ToString() + " ," + allRoute[0]["City_Code"].ToString();
            //StringBuilder sbr = new StringBuilder();
            sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='margin: 0; padding: 0;'>");
            sbr.Append("<tr>");
            sbr.Append(" <td style='text-align:center;font-weight:bold; font-size:14px'><span style='font-size:17px'>" + compCode + "</span><br/>" + allRoute[0]["Add1"].ToString() + "<br/>(" + allRoute[0]["State"].ToString() + ".)- " + allRoute[0]["Pincode"].ToString() + " <br/> Daily Statement Distributor Wise Milk</td >");
            sbr.Append("<td style='text-align:right;font-weight:bold; font-size:14px'>Fax:- " + allRoute[0]["Fax"].ToString() + "<br/>Phone No:" + allRoute[0]["Phone1"].ToString() + "<br/> Date:-" + allRoute[0]["Date"].ToString() + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</table>");
            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particular</th>");
            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            foreach (var group in groupedData)
            {
                sbr.Append("<tbody><tr>");
                //if (rbtnDistributorWise)
                //{
                    sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Customer_Name"].ToString() + "</td>");
                ////}
                //else if (rbtnRouteWise)
                //{
                //sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Route_Desc1"].ToString() + "</td>");
                //}

                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["final"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }
                }

                var totalfinal = group.Sum(item => (decimal)item["final"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");
            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");
            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;margin-top:10px;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particulars</th>");

            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            sumFinal = 0;
            sumMAmount = 0;
            sumProAmt = 0;
            sumtotalAmount = 0;
            foreach (var group in particularGroupedData)
            {
                sbr.Append("<tbody><tr>");
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Cust_Code"].ToString() + "</td>");
                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["TotalLtr_ItemWise"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }
                    //if (matchingRecord != null)
                    //{
                    //    // Add QTYLtr value if a match is found
                    //    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + matchingRecord["final"].ToString() + "</td>");
                    //}
                    //else
                    //{
                    //    // No matching record for this header, so add 0 in the cell
                    //    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0</td>");
                    //}
                }
                var totalfinal = group.Sum(item => (decimal)item["Final_Qty"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");

            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");

        }



        public void crptDailyStatementDistributorRouteWiseDetailcreditMilk(StringBuilder sbr, JObject jsonObject)
        {
            var columnTotals = new Dictionary<string, decimal>();
            decimal sumFinal = 0;
            decimal sumMAmount = 0;
            decimal sumProAmt = 0;
            decimal sumtotalAmount = 0;
            var allRoute = jsonObject["All Routes"];
            var particular = jsonObject["Particular"];
            var tableHeader = allRoute
.Where(item => item["Short_Description"] != null && item["Sku_Seq"] != null)
.Select(item => new
{
    sorfDec = item["Short_Description"].ToString(),
    skuSeq = item["Sku_Seq"].ToString()
})
.GroupBy(item => item.sorfDec)
.Select(group => group.First())
.OrderBy(item => item.skuSeq)
.ToList();

            var groupedData = allRoute.GroupBy(item => item["Route_No"].ToString());
            var particularGroupedData = particular.GroupBy(item => item["Cust_Code"].ToString());
            string compCode = allRoute[0]["Comp_Name"].ToString() + " ," + allRoute[0]["City_Code"].ToString();
            //StringBuilder sbr = new StringBuilder();
            sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='margin: 0; padding: 0;'>");
            sbr.Append("<tr>");
            sbr.Append(" <td style='text-align:center;font-weight:bold; font-size:14px'><span style='font-size:17px'>" + compCode + "</span><br/>" + allRoute[0]["Add1"].ToString() + "<br/>(" + allRoute[0]["State"].ToString() + ".)- " + allRoute[0]["Pincode"].ToString() + " <br/>Daily Statement Distributor and Route Wise Milk</td >");
            sbr.Append("<td style='text-align:right;font-weight:bold; font-size:14px'>Fax:- " + allRoute[0]["Fax"].ToString() + "<br/>Phone No:" + allRoute[0]["Phone1"].ToString() + "<br/> Date:-" + allRoute[0]["Date"].ToString() + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</table>");
            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;' colspan='2'>Particular</th>");
            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            foreach (var group in groupedData)
            {
                sbr.Append("<tbody><tr>");
                //if (rbtnDistributorWise)
                //{
                //    sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Customer_Name"].ToString() + "</td>");
                ////}
                //else if (rbtnRouteWise)
                //{
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Customer_Name"].ToString() + "</td>");
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Route_No"].ToString() + "</td>");
                //}

                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["final"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }
                }

                var totalfinal = group.Sum(item => (decimal)item["final"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");
            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;' colspan='2'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");
            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;margin-top:10px;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particulars</th>");

            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + header.sorfDec + "</th>");
            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            sumFinal = 0;
            sumMAmount = 0;
            sumProAmt = 0;
            sumtotalAmount = 0;
            foreach (var group in particularGroupedData)
            {
                sbr.Append("<tbody><tr>");
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Cust_Code"].ToString() + "</td>");
                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["TotalLtr_ItemWise"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }
                    //if (matchingRecord != null)
                    //{
                    //    // Add QTYLtr value if a match is found
                    //    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + matchingRecord["final"].ToString() + "</td>");
                    //}
                    //else
                    //{
                    //    // No matching record for this header, so add 0 in the cell
                    //    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0</td>");
                    //}
                }
                var totalfinal = group.Sum(item => (decimal)item["Final_Qty"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");

            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");

        }


        public void DailyStatementRouteWiseDetailcreditProduct(StringBuilder sbr, JObject jsonObject)
        {
            var columnTotals = new Dictionary<string, decimal>();
            var columnTotals1 = new Dictionary<string, decimal>();
            decimal sumFinal = 0;
            decimal sumMAmount = 0;
            decimal sumProAmt = 0;
            decimal sumtotalAmount = 0;
            var allRoute = jsonObject["All Routes"];
            var particular = jsonObject["Particular"];
            var tableHeader = allRoute
.Where(item => item["Short_Description"] != null && item["Sku_Seq"] != null)
.Select(item => new
{
    sorfDec = item["Short_Description"].ToString(),
    skuSeq = item["Sku_Seq"].ToString()
})
.GroupBy(item => item.sorfDec)
.Select(group => group.First())
.OrderBy(item => item.skuSeq)
.ToList();

            var groupedData = allRoute.GroupBy(item => item["Route_No"].ToString());
            var particularGroupedData = particular.GroupBy(item => item["Cust_Code"].ToString());
            string compCode = allRoute[0]["Comp_Name"].ToString() + " ," + allRoute[0]["City_Code"].ToString();
            
            sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='margin: 0; padding: 0;'>");
            sbr.Append("<tr>");
            sbr.Append(" <td style='text-align:center;font-weight:bold; font-size:14px'><span style='font-size:17px'>" + compCode + "</span><br/>" + allRoute[0]["Add1"].ToString() + "<br/>(" + allRoute[0]["State"].ToString() + ".)- " + allRoute[0]["Pincode"].ToString() + " <br/>Daily Statement Distributor  Wise Product</td >");
            sbr.Append("<td style='text-align:right;font-weight:bold; font-size:14px'>Fax:- " + allRoute[0]["Fax"].ToString() + "<br/>Phone No:" + allRoute[0]["Phone1"].ToString() + "<br/> Date:-" + allRoute[0]["Date"].ToString() + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</table>");
            // Open the main table
            sbr.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' style='font-size:10px;border-collapse: collapse;'>");

            // <thead> section starts
            sbr.Append("<thead>");
            sbr.Append("<tr>");

            // Main header row for "Particular"
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particular</th>");
            foreach (var header in tableHeader)
            {
                // Header with colspan for each item in the table header
                sbr.Append("<th style='border-right:1px solid;border-top: 1px solid;padding: 10px;' colspan='2'>" + HttpUtility.HtmlEncode(header.sorfDec) + "</th>");
            }
            sbr.Append("</tr>"); // End of header row

            // Sub-header row (for Qty and Amount)
            sbr.Append("<tr>");

            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'></th>");
            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='border-right:1px solid;border-top: 1px solid;padding: 10px;'>Qty</th>");
                sbr.Append("<th style='border-right:1px solid;border-top: 1px solid;padding: 10px;'>Amount</th>");
            }
            sbr.Append("</tr>"); // End of sub-header row
            sbr.Append("</thead>"); // End of <thead>

            // <tbody> starts
            sbr.Append("<tbody>");
            foreach (var group in groupedData)
            {
                sbr.Append("<tr>"); // Row for each group
                                    // First column (Route_Desc)
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Route_Desc"].ToString() + "</td>");

                // Loop through headers to match records
                foreach (var header in tableHeader)
                {
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Add Qty and Amount for matching records
                        var finalValue = decimal.Parse(matchingRecord["final"].ToString());
                        var PAmt = decimal.Parse(matchingRecord["PAmt"].ToString());

                        sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");
                        sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + PAmt.ToString("N2") + "</td>");

                        // Update totals
                        columnTotals[header.sorfDec] = columnTotals.ContainsKey(header.sorfDec) ? columnTotals[header.sorfDec] + finalValue : finalValue;
                        columnTotals1[header.sorfDec] = columnTotals1.ContainsKey(header.sorfDec) ? columnTotals1[header.sorfDec] + PAmt : PAmt;
                    }
                    else
                    {
                        // If no matching record, default to 0.00
                        sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");
                        sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure totals are set to 0 if not already
                        if (!columnTotals.ContainsKey(header.sorfDec)) columnTotals[header.sorfDec] = 0;
                    }
                }
                sbr.Append("</tr>"); // End of group row
            }
            sbr.Append("</tbody>"); // End of <tbody>

            // <tfoot> section for totals
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Output totals in footer
                    sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                    sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals1[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // Default to 0.00 if totals don't exist
                    sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                    sbr.Append("<td style='border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("</tr>");
            sbr.Append("</tfoot>"); // End of <tfoot>

            sbr.Append("</table>"); // End of main table


            sbr.Append("<table width='100%' style='font-size:10px;border-collapse: collapse;margin-top:10px;'>");
            sbr.Append("<thead>");
            sbr.Append("<tr>");
            sbr.Append("<th style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Particulars</th>");

            foreach (var header in tableHeader)
            {
                sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + HttpUtility.HtmlEncode(header.sorfDec) + "</th>");

            }
            sbr.Append("<th style='solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>Total Milk Qty | Milk Amt. | Product Amt. | Total Amt. </th>");
            sbr.Append("</tr>");
            sbr.Append("</thead>");
            sumFinal = 0;
            sumMAmount = 0;
            sumProAmt = 0;
            sumtotalAmount = 0;
            columnTotals.Clear();
            foreach (var group in particularGroupedData)
            {
                sbr.Append("<tbody><tr>");
                sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;'>" + group.First()["Cust_Code"].ToString() + "</td>");
                foreach (var header in tableHeader)
                {
                    // Check if there's a matching record with this Short_Description for this Route_No
                    var matchingRecord = group.FirstOrDefault(item => item["Short_Description"].ToString() == header.sorfDec);
                    if (matchingRecord != null)
                    {
                        // Get the value of "final" and add it to the cell
                        var finalValue = decimal.Parse(matchingRecord["Final_Qty"].ToString()); // Assuming "final" is numeric

                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;'>" + finalValue.ToString("N2") + "</td>");

                        // Add the value to the corresponding column total
                        if (columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] += finalValue;
                        }
                        else
                        {
                            columnTotals[header.sorfDec] = finalValue;
                        }
                    }
                    else
                    {
                        // No matching record for this header, so add 0 in the cell
                        sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>0.00</td>");

                        // Ensure the column total is updated (in case it's the first row)
                        if (!columnTotals.ContainsKey(header.sorfDec))
                        {
                            columnTotals[header.sorfDec] = 0;
                        }
                    }

                }
                var totalfinal = group.Sum(item => (decimal)item["Final_Qty"]);
                var totalMAmt = group.Sum(item => (decimal)item["MAmt"]);
                var totalProAmt = group.Sum(item => (decimal)item["PAmt"]);
                var totalAmount = group.Sum(item => (decimal)item["Total Amount"]);
                sumFinal += totalfinal;
                sumMAmount += totalMAmt;
                sumProAmt += totalProAmt;
                sumtotalAmount += totalAmount;
                sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align:right;'>" + totalfinal + " | " + totalMAmt + " | " + totalProAmt + " |" + totalAmount + "</td>");
                sbr.Append("</tr></tbody>");

            }
            sbr.Append("<tfoot>");
            sbr.Append("<tr>");
            sbr.Append("<td style='border-left:1px solid;border-right: 1px solid;border-top: 1px solid;padding: 10px;border-bottom: 1px solid;'>Total</td>");
            foreach (var header in tableHeader)
            {
                if (columnTotals.ContainsKey(header.sorfDec))
                {
                    // Display the total for this column
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + columnTotals[header.sorfDec].ToString("N2") + "</td>");
                }
                else
                {
                    // If for some reason the column doesn't have a total, just show 0.00
                    sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>0.00</td>");
                }
            }
            sbr.Append("<td style='solid;border-right: 1px solid;border-top: 1px solid;padding:10px;text-align: right;border-bottom: 1px solid;'>" + sumFinal + " |" + sumMAmount + "| " + sumProAmt + " |" + sumtotalAmount + "</td>");
            sbr.Append("</tr>");
            sbr.Append("</tfoot>");
            sbr.Append("</table>");

        }
    }
}