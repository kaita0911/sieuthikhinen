using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using VCMS.MVC4.Web;
using EntityFramework.Extensions;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class LanguageController : VCMSAdminController
    {
        //
        // GET: /Admin/Language/

        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                return View(db.Languages.ToList());
            }
        }
        public ActionResult Create()
        {

            return View();

        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                Language l = new Language();
                TryUpdateModel(l);
                l.Code = form["Code"].ToString();
                l.DisplayName = form["DisplayName"].ToString();
                //l.IsDefault = false;
                db.Languages.Add(l);
                db.SaveChanges();


                var model = db.Websites.Include(a => a.WebsiteDetail).Include(w => w.Languages).FirstOrDefault();
                TryUpdateModel(model);
                model.WebsiteDetail.Add(new WebsiteDetail()
                {
                    //WebsiteId = SiteConfig.SiteId,
                    LanguageId = db.Languages.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    Title = "Viễn Nam",
                    Name = "Viễn Nam",
                });

                db.SaveChanges();
                return Json(new { data = 1 });
            }
            //return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Languages.FirstOrDefault(a => a.Id == id);
                return PartialView("Edit", model);
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var language = db.Languages.Where(l => l.Id == id).Single();
                TryUpdateModel(language);
                language.Code = form["Code"].ToString();
                language.DisplayName = form["DisplayName"].ToString();
                //if (form["IsDefault"].Contains("true"))
                //{
                //    var list = db.Languages.ToList();
                //    list.ForEach(lg => { lg.IsDefault = false; });
                //    language.IsDefault = true;
                //}
                db.SaveChanges();

            }
            //return Json(new { Status = 0, Message = "OK" });
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Update(int id)
        {
            using (DataContext db = new DataContext())
            {
                var language = db.Languages.FirstOrDefault(a => a.Id == id);
                var list = db.Languages.ToList();
                //list.ForEach(df => { df.IsDefault = false; });
                //language.IsDefault = true;
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                db.Languages.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult Locale(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.Language = id;
                return View(db.Locales.Include(l => l.LocaleDetail).OrderBy(l => l.LocaleKey).ToList());
            }
        }
        [HttpPost]
        public ActionResult DeleteLocale(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                db.Locales.Delete(l => ids.Contains((int)l.Id));

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult EditLocale(int id, int language)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.Language = language;
                return View(db.Locales.Include(l => l.LocaleDetail).Where(l => l.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult EditLocale(int id, int language, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var loc = db.Locales.Include(l => l.LocaleDetail).FirstOrDefault(l => l.Id == id);
                TryUpdateModel(loc);
                var details = from d in SiteConfig.Languages
                              select new LocaleDetail()
                              {
                                  LanguageId = d.Id,
                                  Value = form["LocaleDetail[" + d.Id + "].Value"]
                              };

                loc.LocaleDetail = new VList<LocaleDetail>();
                loc.LocaleDetail.AddRange(details);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        //sortorder//
        [HttpPost]
        public ActionResult UpdateOrder(string id, string order)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var orders = order.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var list = db.Properties.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }



    }
}
