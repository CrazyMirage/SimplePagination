using System;
using System.Text;
using System.Web;

namespace TestHandler.Modules
{
    public class CustomModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.BeginRequest += new EventHandler(OnBeginRequest);
            context.EndRequest += new EventHandler(OnEndRequest);
        }

        #endregion

        public void OnBeginRequest(Object sender, EventArgs e)
        {
            HttpApplication httpApp = (HttpApplication)sender;
            httpApp.Context.Response.Output.Write("<div>" + DateTime.Now + "</div>");
        }

        public void OnEndRequest(Object sender, EventArgs e)
        {

            HttpApplication httpApp = (HttpApplication)sender;
            httpApp.Context.Response.Output.Write("<div>" + DateTime.Now + "</div>");

        }

    }
}
