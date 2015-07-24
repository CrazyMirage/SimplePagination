using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestHandler.Authentication
{
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var authorizedUsers = WebConfigurationManager.AppSettings[UsersConfigKey];
                var authorizedRoles = WebConfigurationManager.AppSettings[RolesConfigKey];

                Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

                if (!String.IsNullOrEmpty(Roles))
                {
                    if (!CurrentUser.IsInRole(Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login" , returnUrl = filterContext.HttpContext.Request.RawUrl}));
                    }
                }

                if (!String.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(CurrentUser.UserId.ToString()))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login" , returnUrl = filterContext.HttpContext.Request.RawUrl }));
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                       RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.RawUrl }));
            }
        }
    }
}