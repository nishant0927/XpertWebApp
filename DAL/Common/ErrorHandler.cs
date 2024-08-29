using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace DAL.Common
{
    public static class MyLockClass
    {
        public static readonly Object LockObj = new Object();
    }
    public sealed class ErrorHandler
    {
        public static string UserName = string.Empty;
        public static void WriteError(Exception exception)
        {
            lock (MyLockClass.LockObj)
            {
                string path = "";
                if (HttpContext.Current != null)
                    path = HttpContext.Current.Server.MapPath("~/ErrorLog/" + DateTime.Now.ToString("dd-MM-yy") + ".txt");
                else
                    path = Path.Combine(HttpRuntime.AppDomainAppPath, "ErrorLog\\" + DateTime.Now.ToString("dd-MM-yy") + ".txt");

                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine("\r\nNew Log Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));

                    Exception _Exception;
                    string _Error = "";
                    _Exception = exception;

                    if (_Exception != null)
                    {
                        _Error = "===========================" + Environment.NewLine;
                        _Error = _Error + "Date              :" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
                        _Error = _Error + "User Name         :" + UserName + Environment.NewLine;
                        if (HttpContext.Current != null)
                        {
                            _Error = _Error + "URL               :" + HttpContext.Current.Request.ServerVariables["URL"] + Environment.NewLine;
                            _Error = _Error + "Query String      :" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"] + Environment.NewLine;
                            _Error = _Error + "Req From          :" + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + Environment.NewLine;
                            _Error = _Error + "HostAddress       :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                        }
                        _Error = _Error + "Error Desc        :" + _Exception.Message + Environment.NewLine;
                        _Error = _Error + "Source            :" + _Exception.Source + Environment.NewLine;
                        _Error = _Error + "Line No           :" + _Exception.StackTrace + Environment.NewLine;
                        _Error = _Error + "=============================";
                    }
                    w.WriteLine(_Error);
                    w.Flush();
                    w.Close();
                    UserName = string.Empty;
                }
            }
        }
        public static void WriteLogWithUserName(string MethodName, string UserName)
        {
            try
            {
                string path = "~/ErrorLog/ULog" + (HttpContext.Current.Request.ServerVariables["URL"].Replace('/', '_')).Replace('.', '_') + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nNew Log Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string _Log = "";
                    _Log = "===========================================================" + Environment.NewLine;
                    _Log = _Log + "UserName            :" + UserName + Environment.NewLine;
                    _Log = _Log + "HostAddress       :" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine;
                    _Log = _Log + "MethodName        :" + MethodName + Environment.NewLine;
                    _Log = _Log + "Time              :" + DateTime.Now.ToString("hh:mm:ss.fff") + Environment.NewLine;
                    _Log = _Log + "===========================================================";
                    w.WriteLine(_Log);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception) { }
        }
    }
}
