using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VCMS.MVC4.Data;
using WebMatrix.WebData;
using System.Data.Entity;
using System.IO;
using EntityFramework.Extensions;
using VCMS.MVC4.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Infrastructure;
using VNS.Web.Helpers;
using System.Data;
using System.Data.OleDb;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class PropertyMultiController : VCMSAdminController
    {
        public ActionResult Index(int id)
        {
            using (DataContext db = new DataContext())
            {
                var pro = db.Properties.Include("PropertyDetail").FirstOrDefault(p => p.Id == id);
                ViewBag.PropertyName = pro.PropertyDetail[1].Name;
                var result = db.PropertyMultiValues.Include("PropertyMultiValueDetail").Where(pv => pv.PropertyId == id).ToList();
                return View(result);
            }
        }

        public ActionResult Create(int id)
        {
            using (DataContext db = new DataContext())
            {
                var pro = db.Properties.Include("PropertyDetail").FirstOrDefault(p => p.Id == id);
                ViewBag.PropertyName = pro.PropertyDetail[1].Name;
                ViewBag.MultiLang = pro.MultiLanguage;
                ArticlePropertyValue model = new ArticlePropertyValue();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Create(int id, FormCollection form, int artId = 0)
        {
            using (DataContext db = new DataContext())
            {
                var p = db.Properties.FirstOrDefault(pr => pr.Id == id);
                PropertyMultiValue pv = new PropertyMultiValue();
                pv.PropertyMultiValueDetail = VList<PropertyMultiValueDetail>.Create(SiteConfig.Languages);
                if(artId > 0)
                {
                    List<Article> lstart = new List<Article>();
                    var art = db.Articles.FirstOrDefault(a => a.Id == artId);
                    lstart.Add(art);
                    pv.Articles = lstart;
                }
                    
                pv.PropertyId = id;
                if (p.MultiLanguage)
                {
                    foreach (var l in SiteConfig.Languages)
                    {
                        pv.PropertyMultiValueDetail[l.Id].Value = form["txtValue_" + l.Id.ToString()];
                    }
                }
                else
                {
                    pv.PropertyMultiValueDetail[1].Value = form["txtValue_1"];
                }
                db.PropertyMultiValues.Add(pv);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id });
        }
        public ActionResult CreateAjax()
        {
            PropertyMultiValue p = new PropertyMultiValue() { PropertyMultiValueDetail = VList<PropertyMultiValueDetail>.Create(SiteConfig.Languages) };
            return PartialView(p);
        }
        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.PropertyMultiValues.Include("PropertyMultiValueDetail").FirstOrDefault(pv => pv.Id == id);
                var pro = db.Properties.Include("PropertyDetail").FirstOrDefault(p => p.Id == model.PropertyId);
                ViewBag.PropertyName = pro.PropertyDetail[1].Name;
                ViewBag.MultiLang = pro.MultiLanguage;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form, int artId = 0)
        {
            using (DataContext db = new DataContext())
            {
                var prov = db.PropertyMultiValues.Include(p => p.Articles).Include("PropertyMultiValueDetail").Where(p => p.Id == id).Single();
                TryUpdateModel(prov);
                var pro = db.Properties.FirstOrDefault(p => p.Id == prov.PropertyId);
                if (artId > 0)
                {
                    List<Article> lstart = new List<Article>();
                    var art = db.Articles.FirstOrDefault(a => a.Id == artId);
                    lstart.Add(art);
                    prov.Articles = lstart;
                }
                if (pro.MultiLanguage)
                {
                    foreach (var l in SiteConfig.Languages)
                    {
                        var dt = prov.PropertyMultiValueDetail[l.Id];
                        if (dt == null)
                            prov.PropertyMultiValueDetail.Add(new PropertyMultiValueDetail() { LanguageId = l.Id, Value = form["txtValue_" + l.Id.ToString()] });
                        else
                            dt.Value = form["txtValue_" + l.Id.ToString()];
                    }
                }
                else
                {
                    var dt = prov.PropertyMultiValueDetail[1];
                    if (dt == null)
                        prov.PropertyMultiValueDetail.Add(new PropertyMultiValueDetail() { LanguageId = 1, Value = form["txtValue_1"] });
                    else
                        dt.Value = form["txtValue_1"];
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { id = prov.PropertyId });
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                db.PropertyMultiValues.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult Copy(string id)
        {
            using (DataContext db = new DataContext())
            {
                Guid[] ids = id.Split(new char[] { ',' }).Select(s => Guid.Parse(s)).ToArray();

                foreach (Guid g in ids)
                {
                    var lst = db.ArticlePropertyValues.AsNoTracking().Where(a => a.MultiId == g).ToList();
                    Guid new_g = Guid.NewGuid();
                    lst.ForEach(a =>
                    {
                        a.MultiId = new_g;
                        db.ArticlePropertyValues.Add(a);
                    });
                }
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult UpdateOrder(string id, string order)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => Guid.Parse(s)).ToList();
                var orders = order.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                foreach (Guid g in ids)
                {
                    var list = db.ArticlePropertyValues.Where(c => c.MultiId == g).ToList();
                    list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(g)]; });
                }

                //list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult UpdateDefault(int propertyId, int articleId, string defaultId)
        {
            using (DataContext db = new DataContext())
            {
                Guid g_default = Guid.Parse(defaultId);
                var apv = db.ArticlePropertyValues.Where(a => a.MultiId == g_default && a.PropertyId == propertyId && a.ArticleId == articleId).ToList();
                db.ArticlePropertyValues.Where(a => a.MultiId != g_default && a.PropertyId == propertyId && a.ArticleId == articleId).ToList().ForEach(a => a.IsDefault = false);
                apv.ToList().ForEach(a => a.IsDefault = true);
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
