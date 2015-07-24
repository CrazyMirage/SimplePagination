using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using TestHandler.Authentication;
using TestHandler.DAL;
using TestHandler.Models;

namespace TestHandler.Controllers
{
    public class AccountController : Controller
    {
        DataContext Context = new DataContext();
        //
        // GET: /Account/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = Context.Users.Where(u => u.Username == model.Username).FirstOrDefault();
                if (user != null && Crypto.VerifyHashedPassword(user.Password, model.Password))
                {
                    var roles = user.Roles.Select(m => m.RoleName).ToArray();
                    AuthenticationHelper.AuthUser(Response, user, roles, model.RememberMe);


                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { UserName = user.Username });
                    }

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return Redirect(returnUrl);

                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }
            if (Request.IsAjaxRequest())
            {
                Response.Write(Url.Action("Login"));
                return new HttpStatusCodeResult(400);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                var user = Context.Users.Where(u => u.Username == model.UserName).FirstOrDefault();
                User newUser = null;
                if (user == null)
                {
                    try
                    {
                        newUser = new User()
                        {
                            Username = model.UserName,
                            Email = model.Mail,
                            Roles = new List<Role>() { Context.Roles.Where(x => x.RoleName == "user").FirstOrDefault() },
                            CreateDate = DateTime.Now,
                            Password = Crypto.HashPassword(model.Password)
                        };
                        Context.Users.Add(newUser);
                        Context.SaveChanges();
                        AuthenticationHelper.AuthUser(Response, newUser, newUser.Roles.Select(x=>x.RoleName).ToArray(), false);


                        if (Request.IsAjaxRequest())
                        {
                            return Json(new { UserName = user.Username });
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Ошибка при регистрации");
                    }
                }
            }
            if (Request.IsAjaxRequest())
            {
                Response.Write(Url.Action("Register"));
                return new HttpStatusCodeResult(400);
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }
    }
}
