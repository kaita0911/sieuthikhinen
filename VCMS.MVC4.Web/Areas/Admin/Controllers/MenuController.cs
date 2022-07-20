using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using VCMS.MVC4.Web;
using EntityFramework.Extensions;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class MenuController : VCMSAdminController
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                var items = db.MenuItems.Include("MenuItemDetail").OrderBy(m=>m.SortOrder).ToList();
                return View(items);
            }
        }

        public ActionResult Create() {
            using (DataContext db = new DataContext())
            {
                var model = new MenuItem { MenuItemDetail = VList<MenuItemDetail>.Create(SiteConfig.Languages) };
                ViewBag.ArticleTypeList = new SelectList(db.ArticleTypes.Include("ArticleTypeDetail").ToList(), "Id", "Name");
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                using (DataContext db = new DataContext())
                {
                    MenuItem model = new MenuItem();
                    TryUpdateModel(model);
                    model.MenuItemDetail = new VList<MenuItemDetail>();

                    foreach (var item in SiteConfig.Languages)
                    {
                        model.MenuItemDetail.Add(new MenuItemDetail() { LanguageId = item.Id, Text = Request["txtName" + item.Id.ToString()] });
                    }
                    db.MenuItems.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.MenuItems.Include("MenuItemDetail").FirstOrDefault(m => m.Id == id);

                if (model == null)
                    return HttpNotFound();
                var missing = SiteConfig.Languages.Where(l => !model.MenuItemDetail.Any(cd => cd.LanguageId == l.Id)).Select(l => l.Id).ToList();
                missing.ForEach(m => { model.MenuItemDetail.Add(new MenuItemDetail { LanguageId = m }); });
                ViewBag.ArticleTypeList = new SelectList(db.ArticleTypes.Include("ArticleTypeDetail").ToList(), "Id", "Name");
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                using (DataContext db = new DataContext())
                {
                    MenuItem model = db.MenuItems.Include("MenuItemDetail").FirstOrDefault(m => m.Id == id);
                    TryUpdateModel(model);
                    model.MenuItemDetail = new VList<MenuItemDetail>();

                    foreach (var item in SiteConfig.Languages)
                    {
                        model.MenuItemDetail.Add(new MenuItemDetail() { LanguageId = item.Id, Text = Request["txtName" + item.Id.ToString()] });
                    }
                    //db.MenuItems.Attach(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.MenuItems.Delete(af => ids.Contains(af.Id));
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

                var list = db.MenuItems.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
                //db.Categories.Update(c => ids.Contains(c.Id), c2 => new Category { SortOrder = orders[ids.IndexOf(c2.Id)] });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
