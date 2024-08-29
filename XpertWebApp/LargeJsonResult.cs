using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace XpertWebApp
{
    public class LargeJsonResult : JsonResult
    {
        public LargeJsonResult()
        {
            MaxJsonLength = Int32.MaxValue;
            RecursionLimit = 100;
        }

        public int MaxJsonLength { get; set; }
        public int RecursionLimit { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = this.MaxJsonLength, RecursionLimit = this.RecursionLimit };
                response.Write(serializer.Serialize(Data));
            }
        }
    }

}