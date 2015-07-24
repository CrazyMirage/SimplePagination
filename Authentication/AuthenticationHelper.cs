using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TestHandler.DAL;

namespace TestHandler.Authentication
{
    public static class AuthenticationHelper
    {
        public static void AuthUser(HttpResponseBase response, User user, string[] roles, bool remember)
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.UserId = user.UserId;
            serializeModel.roles = roles;

            string userData = JsonConvert.SerializeObject(serializeModel);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    remember,
                    userData
                    );

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            response.Cookies.Add(faCookie);
        }
    }
}