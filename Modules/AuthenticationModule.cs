using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Security;
using TestHandler.Authentication;

namespace TestHandler.Modules
{
    public class AuthenticationModule : IHttpModule
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
            context.AuthenticateRequest += new EventHandler(OnAuthenticateRequest);
        }

        #endregion

        public void OnAuthenticateRequest(Object source, EventArgs e)
        {
            HttpApplication httpApp = (HttpApplication)source;
            HttpCookie authCookie = httpApp.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                try
                {
                     authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch
                {
                    return;
                }

                if (authTicket.Expired)
                {
                    return;
                }
                FormsAuthenticationTicket newTicket = FormsAuthentication.RenewTicketIfOld(authTicket);

                //Generate User from ticket
                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(newTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(newTicket.Name);
                newUser.UserId = serializeModel.UserId;
                newUser.roles = serializeModel.roles;

                HttpContext.Current.User = newUser;

                //If ticket was renewed set new ticket to cookie
                if (newTicket != authTicket)
                {
                    string encTicket = FormsAuthentication.Encrypt(newTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    httpApp.Context.Response.Cookies.Remove(authCookie.Name);
                    httpApp.Context.Response.Cookies.Add(faCookie);
                }

            }
        }
    }
}
