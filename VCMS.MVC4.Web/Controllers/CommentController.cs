using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;

namespace VCMS.MVC4.Web.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewByArticle(int articleId, int pageIndex = 1, int pageSize = 20)
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { });
            }
            else
                return PartialView(new PagedList<Comment>());
        }

        public ActionResult Create(int articleId)
        {
            if (ControllerContext.IsChildAction)
                return PartialView();
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(int articleId, Comment model)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { });
                }
                else
                    return View();
            }
            else
                return View(model);
        }

        [HttpPost]
        public ActionResult Rating(int id, Comment model)
        {
            using (DataContext db = new DataContext())
            {
                var cm = new Comment { ArticleId = id, Name = model.Name, Email = model.Email, Message = model.Message, DateCreated = DateTime.Now, Rating = model.Rating, Status = CommentStatus.PENDING };
                db.Comments.Add(cm);
                int error = db.SaveChanges();
                if (error >= 0)
                    return Json(new { Status = 0 });
                else
                    return Json(new { Status = -1 });
            }
        }
    }
}
