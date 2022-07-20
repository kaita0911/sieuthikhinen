using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using EntityFramework.Extensions;
using System.IO;
using Newtonsoft.Json.Linq;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class CommentController : VCMSAdminController
    {
        //
        // GET: /ArticleFile/

        public ActionResult List(int id, int pageIndex = 1, int pageSize = 20)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.Article = db.Articles.Include(a => a.ArticleType.ArticleTypeDetail).FirstOrDefault(a => a.Id == id);
                var count = db.Comments.Where(af => af.ArticleId == id).Count();
                ViewBag.ItemCount = count;
                var list = db.Comments.OrderByDescending(c => c.DateCreated).Where(af => af.ArticleId == id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return View(list);
            }
        }

        [HttpPost]
        public ActionResult UpdateAttributes(string json)
        {
            using (DataContext db = new DataContext())
            {
                JObject jsonData = JObject.Parse("{'items':" + json + "}");
                var comment = (from d in jsonData["items"]
                               select new Comment { Id = d["id"].Value<int>(), IsActive = d["isActive"].Value<bool>() }
                           ).ToList();
                var ids = comment.Select(a => a.Id).ToList();
                var list = db.Comments.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(a =>
                {
                    var item = comment.FirstOrDefault(d => d.Id == a.Id);
                    if (item != null)
                    {
                        a.Status = item.Status;
                    }
                });

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult Delete(String id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.Comments.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
