using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.User
{
    public class UserModel
    {
        public List<Users> LoginData { get; set; }
    }
    public class Users
    {
        public string MCC_Code { get; set; }
        public string MCC_NAME { get; set; }
        public string Mcc_Code_VLC_Uploader { get; set; }
        public string User_APP_Type { get; set; }
        public string Vendor_Code { get; set; }
        public string VLC_Code_VLC_Uploader { get; set; }
        public string Vendor_Name { get; set; }
        public string Current_UserCode { get; set; }
        public string Current_UserName { get; set; }
        public string MP_Code { get; set; }
        public string MP_Code_VLC_Uploader { get; set; }
        public string MP_Name { get; set; }
        public string Result { get; set; }
        public string Default_Location { get; set; }
    }
}
