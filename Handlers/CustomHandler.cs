using System;
using System.IO;
using System.Web;

namespace TestHandler.Handlers
{
    public class CustomHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(File.ReadAllText(context.Server.MapPath("~/json.json")));
            context.Response.End();
        }

        #endregion
    }
}
