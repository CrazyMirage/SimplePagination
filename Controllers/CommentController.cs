using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestHandler.Modules;

namespace TestHandler.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        private static List<CommentModel> Comments = new List<CommentModel>();

        public ActionResult Add(CommentModel comment)
        {
            Comments.Add(comment);
            if (Request.IsAjaxRequest())
            {
                return Json(comment);
            }
            else
            {
                return RedirectToAction("UsersPage", "Home");
            }
        }

        [ChildActionOnly]
        public ActionResult CommentView()
        {
            return PartialView(Comments);
        }

    }
}
