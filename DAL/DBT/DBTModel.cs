using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBT
{
   public class DBTModel
    {
    }
    #region MPIncentiveEntryGetPaymentCycle
    // --------- MPIncentiveEntryGetPaymentCycle ----------------
    public class MPEntryPaymentCycle
    {
        public List<MPIncentiveEntryGetPaymentCycle> MPIncentiveEntryGetPaymentCycle { get; set; }
    }
    public class MPIncentiveEntryGetPaymentCycle
    {
        public string Code { get; set; }
        public string Result { get; set; }
    }
    #endregion
    #region MPIncentiveEntrySummaryTotal

    //-------- MPIncentiveEntrySummaryTotal ------------------
    public class MPIncentiveEntrySummaryTotal
    {
        public List<MPEntrySummary> Loving { get; set; }
    }
    public class MPEntrySummary
    {
        public bool Result { get; set; }
        public List<ResponseCode> Response_Code { get; set; }
    }

    public class ResponseCode
    {
        public string Samples { get; set; }
        public string Qty { get; set; }
        public string Amount { get; set; }
        public string Result { get; set; }
    }

    #endregion

    #region MPIncentiveEntryGetDataBulk

    // -------------- MPIncentiveEntryGetDataBulk ----------------
    public class MPBulkData
    {
        public List<MPIncentiveEntryGetDataBulk> MPIncentiveEntryGetDataBulk { get; set; }
    }
    public class MPIncentiveEntryGetDataBulk
    {
        public string MP_Code_VLC_Uploader { get; set; }
        public string MP_Code { get; set; }
        public string MP_Name { get; set; }
        public string Gender { get; set; }
        public string Telphone { get; set; }
        public string Jan_Aadhar_No { get; set; }
        public string Status { get; set; }
        public string Father_Name { get; set; }
        public string Qty { get; set; }
        public string Result { get; set; }
    }


    #endregion

    #region MPIncentiveEntryGetBilledQty
    //---------- MPIncentiveEntryGetBilledQty  -----------------
    public class MPBilledQty
    {
        public List<MPIncentiveEntryGetBilledQty> MPIncentiveEntryGetBilledQty { get; set; }
    }
    public class MPIncentiveEntryGetBilledQty
    {
        public string Qty { get; set; }
        public string Result { get; set; }
    }

    //public class BillQty
    //{
    //    public string value { get; set; }
    //    public string type { get; set; }
    //}


    #endregion

    #region MPIncentiveEntrySaveDataBulk

    public class MPIncentiveEntrySaveDataBulk
    {
        public string VLCUploaderCode { get; set; }
        public string MCCCode { get; set; }
        public string MPUploaderCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Qty { get; set; }
    }

    #endregion
    #region DBTVM
    public class DBTVM
    {
        public string VLC_UploderCode { get; set; }
        public string VLCName { get; set; }
        public string BilledQty { get; set; }
        public string MPFarmer { get; set; }
        public string MPQty { get; set; }
        public string MPAmount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<VMMPIEBulkData> Farmers { get; set; }
    }
    public class VMMPIEBulkData
    {
        public string Code { get; set; }
        public string Farmer { get; set; }
        public string FatherName { get; set; }
        public string Qty { get; set; }
    }
    #endregion

    #region DBTRegister
    public class Loving
    {
        public bool Result { get; set; }
        public List<DBTRegister> Response_Code { get; set; }
    }

    public class DBTRegister
    {
        public int? PK_Id { get; set; }
        public string VLC_Code { get; set; }
        public string VLC_Code_VLC_Uploader { get; set; }
        public string VLC_Name { get; set; }
        public string MP_Code_VLC_Uploader { get; set; }
        public string MP_Code { get; set; }
        public string MP_Name { get; set; }
        public string Qty { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string Father_Name { get; set; }
        public string MP_IFSC_No { get; set; }
        public string MP_Account_No { get; set; }
        public string MP_Bank { get; set; }
        public string Result { get; set; }
    }

    public class DBTRegisterModel
    {
        public List<Loving> Loving { get; set; }
    }



    #endregion
}
