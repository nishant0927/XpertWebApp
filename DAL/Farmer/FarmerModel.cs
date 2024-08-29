using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Farmer
{
    #region FarmerModel MPData

    public class RequestResponse
    {
        public string Reques { get; set; }
        public string Response { get; set; }
    }
    public class FarmerModel
    {
        public List<MPDataList> MPData { get; set; }
    }
    public class MPDataList
    {

        public string MP_Code_VLC_Uploader { get; set; }
        public string MP_Code { get; set; }
        public string MP_Name { get; set; }
        public string MP_Name_Hindi { get; set; } // Assuming this is a stringMP_Name_Hindi
        public string MP_Name1 { get; set; }
        public string Gender { get; set; }
        public string Telphone { get; set; }
        public string Jan_Aadhar_No { get; set; }
        public string Status { get; set; }
        public string Father_Name { get; set; }
        public int Jan_Aadhar_No_Verified { get; set; }
        public string Jan_Aadhar_No_Verified1 { get; set; } // Assuming this is a string
        public string PayeeName { get; set; }
        public string BankName { get; set; }
        public string IFCICode { get; set; }
        public string AccountNO { get; set; }
        public string JA_nameEng { get; set; }
        public string JA_acc { get; set; }
        public string JA_bankName { get; set; }
        public string JA_ifsc { get; set; }
        public string JA_bankBranch { get; set; }
        public string JA_mobile { get; set; }
        public string Result { get; set; }


        //public string MP_Code_VLC_Uploader { get; set; }
        //public string MP_Code { get; set; }
        //public string MP_Name { get; set; }
        //public string MP_Name_Hindi { get; set; }
        //public string MP_Name1 { get; set; }
        //public string Gender { get; set; }
        //public string Telphone { get; set; }
        //public string Jan_Aadhar_No { get; set; }
        //public string Status { get; set; }
        //public string Father_Name { get; set; }
        //public int Jan_Aadhar_No_Verified { get; set; }
        //public string Jan_Aadhar_No_Verified1 { get; set; }
        //public string PayeeName { get; set; }
        //public string BankName { get; set; }
        //public string IFCICode { get; set; }
        //public string AccountNO { get; set; }
        //public string Result { get; set; }
        //public List<MPDataList> lst { get; set; }
    }
    #endregion

    #region MPDetailModel
    public class MPDetailModel
    {
        public List<MPDetail> MPData { get; set; }
    }
    public class MPDetail
    {
        public string MP_NAME { get; set; }
        public string Father_Name { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Telphone { get; set; }
        public string MP_CODE_VLC_UPLOADER { get; set; }
        public string PayeeName { get; set; }
        public string BankName { get; set; }
        public string IFCICode { get; set; }
        public string AccountNO { get; set; }
        public string Gender { get; set; }
        public int InActive { get; set; }
        public string AadharNo { get; set; }
        public string MP_Name_Hindi { get; set; }
        public string Jan_Aadhar_No { get; set; }
        public string IsDBTDone { get; set; }
        public string CAST_CATEGORY_CODE { get; set; }
        public string CAST_CATEGORY_NAME { get; set; }
        public string Age { get; set; }
        public string DISTRICT_Code { get; set; }
        public string District_Name { get; set; }
        public string Zone_Code { get; set; }
        public string Zone_Name { get; set; }
        public string BLOCK_CODE { get; set; }
        public string BLOCK_NAME { get; set; }
        public string REVENUE_VILLAGE_CODE { get; set; }
        public string REVENUE_VILLAGE_NAME { get; set; }
        public string GRAMPANCHAYAT_CODE { get; set; }
        public string GRAMPANCHAYAT_NAME { get; set; }
        public string PANCHAYAT_SAMITI_CODE { get; set; }
        public string PANCHAYAT_SAMITI_NAME { get; set; }
        public string VIDHAN_SABHA_CODE { get; set; }
        public string VIDHAN_SABHA_NAME { get; set; }
        public string Result { get; set; }
    }

    #endregion

    #region Dropdown Lists
    public class MPDistictList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class DistrictList
    {
        public List<MPDistictList> MPDistictList { get; set; }
    }
    public class MPGetCastCategory
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class CastCategory
    {
        public List<MPGetCastCategory> MPGetCastCategory { get; set; }
    }
    public class MPZoneList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class ZoneList
    {
        public List<MPZoneList> MPZoneList { get; set; }
    }
    public class MPBlockList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class BlockList
    {
        public List<MPBlockList> MPBlockList { get; set; }
    }
    public class MPRevenueVillageList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class RevenueVillageList
    {
        public List<MPRevenueVillageList> MPRevenueVillageList { get; set; }
    }
    public class MPGramPanchayatList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class GramPanchayatList
    {
        public List<MPGramPanchayatList> MPGramPanchayatList { get; set; }
    }
    public class MPPanchayatSamitiList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class PanchayatSamitiList
    {
        public List<MPPanchayatSamitiList> MPPanchayatSamitiList { get; set; }
    }
    public class MPVidhanSabhaList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class VidhanSabhaList
    {
        public List<MPVidhanSabhaList> MPVidhanSabhaList { get; set; }
    }

    #endregion

    #region GetFixedParameter
    public class GetFixedParameter
    {
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Result { get; set; }
    }

    public class FixedParameter
    {
        public List<GetFixedParameter> GetFixedParameter { get; set; }
    }
    #endregion

}
