using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class DemandBookingSaleDetailModel
    {
        public string Document_No { get; set; } = null;
        public string Line_No { get; set; }
        public int Trip_No { get; set; }
        public string Item_Code { get; set; } = null;
        public string Cust_Code { get; set; } = null;
        public string Item_Desc { get; set; } = null;
        public string Unit_code { get; set; } = null;
        public decimal TotalCrates_ItemWise { get; set; } = 0;
        public decimal TotalLtr_ItemWise { get; set; } = 0;
        public decimal ItemNetAmount { get; set; } = 0;
        public string IsGatePassGenerated { get; set; } = "N";
        public string IsTruckSheetGenerated { get; set; } = "N";
        public string Is_Posted { get; set; } = null;
        public double Qty { get; set; } = 0;
        public double Rate { get; set; } = 0;
        public string Price_Code { get; set; } = null;
        public string Vehicle_Code { get; set; } = "";
        public string ShiftType { get; set; } = "";
        public string TR_CODE { get; set; } = null;
        public int IsItemUpdate { get; set; } = 0;
    }
}
