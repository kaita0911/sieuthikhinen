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
    //[Authorize(Roles="Super Administrators")]
    public class PropertyController : VCMSAdminController
    {
        //
        // GET: /Property/

        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Properties.Include("PropertyDetail").OrderBy(a=>a.SortOrder).ToList();
                return View(model);
            }
        }

        public ActionResult Create()
        {
            Property p = new Property() {  PropertyDetail =  VList<PropertyDetail>.Create(SiteConfig.Languages) };
            return View(p);
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                Property p = new Property() { Code = form["Code"], PropertyDetail = VList<PropertyDetail>.Create(SiteConfig.Languages) };
                foreach (var l in SiteConfig.Languages)
                {
                    p.PropertyDetail[l.Id].Name = form["txtName_" + l.Id.ToString()];
                }
                db.Properties.Add(p);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateAjax()
        {
            Property p = new Property() { PropertyDetail = VList<PropertyDetail>.Create(SiteConfig.Languages) };
            return PartialView(p);
        }

        [HttpPost]
        public ActionResult CreateAjax(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                Property p = new Property();
                TryUpdateModel(p);
                
                p.PropertyDetail = VList<PropertyDetail>.Create(SiteConfig.Languages);
                foreach (var l in SiteConfig.Languages)
                {
                    p.PropertyDetail[l.Id].Name = form["txtName_" + l.Id.ToString()];
                }
                db.Properties.Add(p);
                db.SaveChanges();
                return Json(new { Id=p.Id, DisplayName=p.PropertyDetail[1].Name });
            }
        }

        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Properties.Include("PropertyDetail").Where(p => p.Id == id).FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var pro = db.Properties.Include("PropertyDetail").Where(p => p.Id == id).Single();
                TryUpdateModel(pro);
                foreach (var l in SiteConfig.Languages)
                {
                    var dt = pro.PropertyDetail[l.Id];
                    if (dt == null)
                        pro.PropertyDetail.Add(new PropertyDetail() { LanguageId = l.Id, Name = form["txtName_" + l.Id.ToString()] });
                    else
                        dt.Name = form["txtName_" + l.Id.ToString()];
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.Properties.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

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
