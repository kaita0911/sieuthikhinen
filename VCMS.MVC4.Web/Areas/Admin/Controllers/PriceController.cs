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
    public class PriceController : Controller
    {
        //
        // GET: /Admin/Price/

        public ActionResult Index(int id)
        {
            using (DataContext db = new DataContext())
            {
                var art = db.Articles.Include(a => a.ArticleDetail).FirstOrDefault(a => a.Id == id);
                ViewBag.ProductName = art.ArticleName;

                var model = db.Prices.Include(p => p.Currency).Where(p => p.ArticleId == id).OrderByDescending(p => p.SortOrder).ToList();
                return View(model);
            }
        }
        public ActionResult Create(int id)
        {
            using (DataContext db = new DataContext())
            {
                var art = db.Articles.Include(a => a.ArticleDetail).FirstOrDefault(a => a.Id == id);
                ViewBag.ProductName = art.ArticleName;

                Price model = new Price();
                return View(model);
            }   
        }
        public ActionResult CreateCurrency()
        {
            Currency model = new Currency();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateCurrency(Currency model)
        {
            using (DataContext db = new DataContext())
            {
                if (!string.IsNullOrEmpty(Request["Rate"]))
                    model.Rate = decimal.Parse(Request["Rate"]);
                db.Currencies.Add(model);
                db.SaveChanges();
                return RedirectToAction("CurrencyList");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(int id, Price model)
        {
            using (DataContext db = new DataContext())
            {
                var count = db.Prices.Count(p => p.ArticleId == id);
                if (Request["chk-isdefault"] != null)
                {
                    bool[] chek = Request["chk-isdefault"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    model.IsDefault = chek[0];
                }
                if (Request["chk-inactive"] != null)
                {
                    bool[] chek = Request["chk-inactive"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    model.Inactive = chek[0];
                }
                model.SortOrder = count + 1;
                model.ArticleId = id;
                model.DateCreated = DateTime.Now;
                model.CurrencyId = int.Parse(Request["Currency"]);
                if (!string.IsNullOrEmpty(Request["Price"]))
                    model.Value = decimal.Parse(Request["Price"]);
                db.Prices.Add(model);

                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }
        }
        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Prices.Include(p => p.Currency).FirstOrDefault(p => p.Id == id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Prices.Where(p => p.Id == id).Single();
                TryUpdateModel(model);

                if (Request["chk-isdefault"] != null)
                {
                    bool[] chek = Request["chk-isdefault"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    model.IsDefault = chek[0];
                }
                if (Request["chk-inactive"] != null)
                {
                    bool[] chek = Request["chk-inactive"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    model.Inactive = chek[0];
                }
                model.CurrencyId = int.Parse(Request["currencyId"]);
                if (!string.IsNullOrEmpty(Request["Price"]))
                    model.Value = decimal.Parse(Request["Price"]);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = model.ArticleId });
            }
        }

        [HttpPost]
        public ActionResult Update(int articleId, int defaultId)
        {
            using (DataContext db = new DataContext())
            {
                var article = db.Articles.Include(a => a.Prices).FirstOrDefault(a => a.Id == articleId);
                var price = article.Prices.FirstOrDefault(f => f.Id == defaultId);
                var others = article.Prices.Where(af => af.Id != defaultId).ToList();
                others.ForEach(af => { af.IsDefault = false; });
                price.IsDefault = true;
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Đã cập nhật" });
            }
        }

        [HttpPost]
        public ActionResult Delete(String id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                db.Prices.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Xoá thành công." });
            }
        }

        public ActionResult CurrencyList()
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Currencies.ToList();
                return View(model);
            }
        }

        public ActionResult EditCurrency(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Currencies.FirstOrDefault(p => p.Id == id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditCurrency(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Currencies.Where(p => p.Id == id).Single();
                TryUpdateModel(model);

                if (Request["chk-isdefault"] != null)
                {
                    bool[] chek = Request["chk-isdefault"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    model.IsDefault = chek[0];
                }

                if (!string.IsNullOrEmpty(Request["Rate"]))
                    model.Rate = decimal.Parse(Request["Rate"]);
                db.SaveChanges();
                return RedirectToAction("CurrencyList");
            }
        }

        [HttpPost]
        public ActionResult UpdateCurrency(int defaultId)
        {
            using (DataContext db = new DataContext())
            {
                var curr = db.Currencies.FirstOrDefault(f => f.Id == defaultId);
                var others = db.Currencies.Where(af => af.Id != defaultId).ToList();
                others.ForEach(af => { af.IsDefault = false; });
                curr.IsDefault = true;
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Đã cập nhật" });
            }
        }

        [HttpPost]
        public ActionResult UpdateOrder(string id, string order)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var orders = order.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var list = db.Prices.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult DeleteCurrency(String id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                db.Currencies.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "Xoá thành công." });
            }
        }
    }
}
