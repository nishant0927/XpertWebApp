using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.VLC
{
    public class CVMDBTEntry
    {
        public string VLC_UploderCode { get; set; }
        public string VLCName { get; set; }
        public string BilledQty { get; set; }
        public string MPFarmer { get; set; }
        public string MPQty { get; set; }
        public string MPAmount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<CVMMPIEBulkData> Farmers { get; set; }
    }
    public class CVMMPIEBulkData
    {
        public string Code { get; set; }
        public string Farmer { get; set; }
        public string FatherName { get; set; }
        public string Qty { get; set; }
    }
}
