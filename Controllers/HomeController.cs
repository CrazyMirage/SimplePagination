using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestHandler.Authentication;
using TestHandler.Models;

namespace TestHandler.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private static readonly int UsersOnPage;

        static HomeController()
        {
            if (!Int32.TryParse(WebConfigurationManager.AppSettings["UsersOnPage"], out UsersOnPage))
            {
                UsersOnPage = 100;
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadJson()
        {
            return Content(
                System.IO.File.ReadAllText(Server.MapPath(WebConfigurationManager.AppSettings["TestFile"])),
                "application/json");
        }

        public ActionResult Users()
        {
            string file = WebConfigurationManager.AppSettings["TestFile"];

            var json = HttpContext.Cache.Get(file) as List<UserModel>;
            if (json == null)
            {
                var fileText = System.IO.File.ReadAllText(Server.MapPath(file));
                json = new JavaScriptSerializer().Deserialize<List<UserModel>>(fileText);
                HttpContext.Cache.Insert(file, json);
            }
            return View(json);
        }

        public ActionResult UsersPage(int? page)
        {
            var realPage = page ?? 1;
            string file = WebConfigurationManager.AppSettings["TestFile"];

            var users = HttpContext.Cache.Get(file) as List<UserModel>;
            if (users == null)
            {
                var fileText = System.IO.File.ReadAllText(Server.MapPath(file));
                users = new JavaScriptSerializer().Deserialize<List<UserModel>>(fileText);
                if (users != null)
                    HttpContext.Cache.Insert(file, users);
                else
                    return View();
            }

            var pageData = users.Skip((realPage - 1) * UsersOnPage).Take(UsersOnPage);
            var pagination = new PaginationModel()
                {
                    Page = realPage,
                    LastPage = Convert.ToInt32(Math.Ceiling((double)users.Count / UsersOnPage)),
                    NumberOfDisplayedPages = 5,
                    DefaultDestination = new ActionDestination() { Action = "UsersPage", Controller = "Home" }
                };

            if (Request.IsAjaxRequest())
            {
                return Json(new { pageData = pageData , 
                    pagination = pagination.GetShownPages(), 
                    current = realPage, 
                    next = !pagination.IsLast, 
                    previous = !pagination.IsFirst}, 
                    JsonRequestBehavior.AllowGet);
            }
            var model = new UsersPageModel()
            {
                PagginationData = pagination,
                Users = pageData
            };
            return View(model);
        }

        public async Task<ActionResult> AsyncJson()
        {
            return Content(
                await AsyncFileRead(Server.MapPath(WebConfigurationManager.AppSettings["TestFile"])),
                "application/json");
        }

        [ChildActionOnly]
        public async Task<string> AsyncFileRead(string filename)
        {
            using (StreamReader SourceReader = System.IO.File.OpenText(filename))
            {
                return await SourceReader.ReadToEndAsync();
            }
        }

        [CustomAuthorize(Roles = "admin")]
        public ActionResult GenerateJson()
        {
            var names = new[] { "Lex", "Rolo", "Jaiden", "Sam", "Auberon" };
            var users = new List<UserModel>();
            var rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                users.Add(new UserModel() { ID = rand.Next(), Name = names[rand.Next() % names.Length] });
            }

            using (StreamWriter SourceWriter = System.IO.File.CreateText(Server.MapPath(WebConfigurationManager.AppSettings["TestFile"])))
            {
                SourceWriter.Write(new JavaScriptSerializer().Serialize(users));
            }
            return RedirectToAction("LoadJson");
        }

    }
}
