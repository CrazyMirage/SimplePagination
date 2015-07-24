using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TestHandler.Handlers
{
    /// <summary>
    /// Summary description for CustomHandler1
    /// </summary>
    public class CustomHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(File.ReadAllText(context.Server.MapPath("json.json")));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}