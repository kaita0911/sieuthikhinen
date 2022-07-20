//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using VCMS.MVC4.Data;
//using WebMatrix.WebData;
//using System.Data.Entity;
//using System.IO;
//using EntityFramework.Extensions;
//using VCMS.MVC4.Web;
//using System.Web.Script.Serialization;
//using Newtonsoft.Json.Linq;
//using System.Data.Entity.Infrastructure;
//using VNS.Web.Helpers;

//namespace VCMS.MVC4.Web.Areas.Admin.Controllers
//{
//    //[Authorize(Roles = "Super Administrators")]
//    public class WebsitePluginController : Controller
//    {
//        //
//        // GET: /Admin/WebsitePlugin/

//        public ActionResult Index()
//        {
//            using (DataContext db = new DataContext())
//            {
//                var model = db.WebsitePlugins.ToList();
//                return View(model);
//            }
//        }

//        public ActionResult Create()
//        {
//            WebsitePlugin wp = new WebsitePlugin();
//            return View(wp);
//        }

//        [HttpPost]
//        public ActionResult Create(WebsitePlugin model)
//        {
//            using (DataContext db = new DataContext())
//            {
//                if (ModelState.IsValid)
//                {
//                    db.WebsitePlugins.Add(model);
//                    db.SaveChanges();
//                    return RedirectToAction("Index");
//                }
//                else
//                    return View(model);
//            }
//        }
//        public ActionResult Edit(int id)
//        {
//            using (DataContext db = new DataContext())
//            {
//                var model = db.WebsitePlugins.Where(p => p.Id == id).FirstOrDefault();
//                return View(model);
//            }
//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Edit(int id, FormCollection form)
//        {
//            using (DataContext db = new DataContext())
//            {
//                var model = db.WebsitePlugins.SingleOrDefault(at => at.Id == id);
//                if (ModelState.IsValid)
//                {
//                    UpdateModel(model);
//                    db.SaveChanges();
//                    return RedirectToAction("Index");
//                }
//                else
//                    return View(model);
//            }
//        }

//        [HttpPost]
//        public ActionResult Delete(string id)
//        {
//            using (DataContext db = new DataContext())
//            {
//                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

//                db.WebsitePlugins.Delete(af => ids.Contains(af.Id));
//                db.SaveChanges();
//                return Json(new { Status = 0, Message = "OK" });
//            }
//        }
//        [HttpPost]
//        public ActionResult UpdateAttributes(string json)
//        {
//            using (DataContext db = new DataContext())
//            {
//                JObject jsonData = JObject.Parse("{'items':" + json + "}");
//                var website = (from d in jsonData["items"]
//                               select new WebsitePlugin { Id = d["id"].Value<int>(), IsCustommer = d["isCustomer"].Value<bool>() }
//                           ).ToList();
//                var ids = website.Select(a => a.Id).ToList();
//                var list = db.WebsitePlugins.Where(c => ids.Contains(c.Id)).ToList();
//                list.ForEach(a =>
//                {
//                    var item = website.FirstOrDefault(d => d.Id == a.Id);
//                    if (item != null)
//                    {
//                        a.IsCustommer = item.IsCustommer;
//                    }
//                });
//                db.SaveChanges();
//                return Json(new { Status = 0, Message = "OK" });
//            }
//        }
//    }
//}
