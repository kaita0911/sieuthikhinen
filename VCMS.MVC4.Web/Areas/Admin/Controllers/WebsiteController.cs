using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using System.Text.RegularExpressions;
using VCMS.MVC4.Web;
using System.IO;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class WebsiteController : VCMSAdminController
    {
        //
        // GET: /Website/
        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                //string path = @"H:\MyWebsite\VCMS\VCMS.MVC4.Web\VCMS.MVC4.Web\Templates\";
                //foreach (string s in Directory.GetDirectories(path))
                //{
                //    Console.WriteLine(s.Remove(0, path.Length));
                //}
                
                ViewBag.Languages = db.Languages.ToList();
                var model = db.Websites.Include(w => w.WebsiteDetail).Include(w => w.Languages).FirstOrDefault();
                if (model == null) return HttpNotFound();
                return View(model);    
            }
        }
     
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                //db.WebsitePlugins.Where(a => (((int)a.Flag & (int)WebsitePluginFlags.WEBCONFIG) > 0)).ToList().ForEach(
                //    wp =>
                //    {
                //        wp.Value = Request["wp_" + wp.Id.ToString()];
                //    });

                //db.WebsitePlugins.ToList().ForEach(
                //    wp =>
                //    {
                //        wp.Value = Request["wp_" + wp.Id.ToString()];
                //    });

                var model = db.Websites.Include(a => a.WebsiteDetail).Include(w => w.Languages).FirstOrDefault();
                TryUpdateModel(model);
                if (form["languages"] != null)
                {
                    var langIds = form["languages"].Split(new char[] { ',' }).Where(s => Regex.IsMatch(s, "\\d+")).Select(s => int.Parse(s)).ToList();
                    var languages = db.Languages.Where(l => langIds.Contains(l.Id)).ToList();
                    model.Languages = languages;
                }
                
                //var details = from d in SiteConfig.Languages
                //              select new WebsiteDetail()
                //              {
                //                  LanguageId = d.Id,
                //                  Name = Request[string.Format("WebsiteDetail[{0}].Name", d.Id)],
                //                  Title = Request[string.Format("WebsiteDetail[{0}].Title", d.Id)],
                //                  SEOKeywords = Request[string.Format("WebsiteDetail[{0}].SEOKeywords", d.Id)],
                //                  SEODescription = Request[string.Format("WebsiteDetail[{0}].SEODescription", d.Id)],
                //              };

                //model.WebsiteDetail = new VList<WebsiteDetail>();
                //model.WebsiteDetail.AddRange(details.ToList());

                db.SaveChanges();
                HttpContext.Cache.Remove("SiteCode");
                return RedirectToAction("Index");
            }
        }
        public ActionResult Online()
        {
            return View();
        }
        public ActionResult Config()
        {
            return View();
        }
        public ActionResult Properties()
        {
            using (DataContext db = new DataContext())
            {
                db.WebsiteConfigs.ToList();
                return View();
            }
        }
        public ActionResult UpdateDatabase()
        {

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateDatabase(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    //var ids = db.Database.SqlQuery<int>(string.Format("select * from Articles")).ToList();
                    var query = db.Database.ExecuteSqlCommand(string.Format(form["query"].ToString()));
                    if (query == -1)
                        return Json(new { Status = 0, Message = "OK" });
                    else
                        return Json(new { Status = 1, Message = query });
                }
                catch (Exception e)
                {
                    return Json(new { Status = -1, Message = e.Message });
                }
            }
        }
    }
}
