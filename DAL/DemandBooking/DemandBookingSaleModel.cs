using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class DemandBookingSaleModel
    {
       // public string qry { get; set; }
        public string Document_No { get; set; } = null;
        public DateTime Document_Date { get; set; }
        public string Location_Code { get; set; } = null;
        public string Location_Des { get; set; } = null;
        public string City_Code { get; set; } = null;
        public string City_Name { get; set; } = null;
        public int Posted { get; set; } = 0;
        public int IsIndividualCustomer { get; set; } = 0;
        public DateTime? Posting_Date { get; set; } = null;
        public string Price_code { get; set; }
        public string Route_No { get; set; } = null;
        public string Route_Des { get; set; } = null;
        public string ShiftType { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string TripNo { get; set; } = string.Empty;
        public decimal TotalQtyInCrates { get; set; } = 0;
        public decimal TotalQtyInLtr { get; set; } = 0;
        public decimal DocumentAmount { get; set; } = 0;

        public int Posted_Morning { get; set; } = 0;
        public int Posted_Evening { get; set; } = 0;
        public string UploderDocNo { get; set; } = string.Empty;

        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalCrate { get; set; } = 0;
        public decimal TotalLitre { get; set; } = 0;
        public decimal DocumentAmt { get; set; } = 0;


        public List<DemandBookingSaleDetailModel> Arr { get; set; } = null;
    }
}
