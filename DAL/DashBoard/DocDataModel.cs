using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DocModalDetail
    {
        public List<DocDataModel> DocDataList { get; set; }
    }
    public class DocDataModel
    {
        public int SNo { get; set; }
        public string SDate { get; set; }
        public string Shift { get; set; }
        public decimal Milk_Wtd { get; set; }
        public string BMC_Uploader_Code { get; set; }
        public string BMC_Code { get; set; }
        public string BMC_Name { get; set; }
        public string Area { get; set; }
        public string Route_Code { get; set; }
        public string DCS_Uploader_Code { get; set; }
        public string DCS_Code { get; set; }
        public string DCS_Name { get; set; }
        public int Qty { get; set; }
        public decimal FAT { get; set; }
        public decimal SNF { get; set; }
        public decimal FAT_KG { get; set; }
        public decimal SNF_KG { get; set; }
        public string RATE { get; set; }
        public string Amt { get; set; }
    }
}
