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
    public class DiscountController : VCMSAdminController
    {
        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Discounts.Include(d => d.Currency).ToList();
                return View(model);
            }
        }

        public ActionResult Create()
        {
            using (DataContext db = new DataContext())
            {
                Discount model = new Discount();
                model.Currencies = db.Currencies.ToList();
                model.Categories = Category.GetTree(db.Categories.Include("CategoryDetail").Where(c => c.ArticleTypeId == 2).ToList());
                model.Articles = db.Articles.Include("ArticleDetail").Where(a => a.ArticleTypeId == 2).Take(5).ToList();
                ViewBag.Count = db.Articles.Include("ArticleDetail").Count(c => c.ArticleTypeId == 2);
                ViewBag.CateCount = db.Categories.Include("CategoryDetail").Count(c => c.ArticleTypeId == 2);
                return View(model);
            }  
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Discount model, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                model.Status = 0;
                if (Request["chk-usepercen"] != null)
                {
                    bool[] chek = Request["chk-usepercen"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    model.UsePercent = chek[0];
                }
                model.CurrencyId = int.Parse(Request["Currency"]);
                if (!string.IsNullOrEmpty(Request["DiscountPercent"]))
                    model.DiscountPercent = decimal.Parse(Request["DiscountPercent"]);

                if (!string.IsNullOrEmpty(Request["DiscountAmount"]))
                    model.DiscountAmount = decimal.Parse(Request["DiscountAmount"]);

                List<Category> lstCate = new List<Category>();
                if (form["categories"] != null)
                {
                    var cate = db.Categories.Where(c => c.ArticleTypeId == 2).ToList();
                    var cats = form["categories"].Split(new char[] { ',' });
                    var categories = cate.Where(c => cats.Contains(c.Id.ToString())).ToList();
                    lstCate.AddRange(categories); model.Categories = lstCate;
                }
               
                List<Article> lstArt = new List<Article>();
                if (form["articles"] != null)
                {
                    var art = db.Articles.Where(c => c.ArticleTypeId == 2).ToList();
                    var arts = form["articles"].Split(new char[] { ',' });
                    var articles = art.Where(c => arts.Contains(c.Id.ToString())).ToList();
                    lstArt.AddRange(articles); model.Articles = lstArt;
                }
                
                db.Discounts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Discounts.Include(dc => dc.Currency).Include(dc => dc.Categories).Include(dc => dc.Articles).Where(p => p.Id == id).FirstOrDefault();
                ViewBag.Categories = Category.GetTree(db.Categories.Include("CategoryDetail").Where(c => c.ArticleTypeId == 2).ToList());

                var listID = model.Articles.Select(a => a.Id).ToList();

                var article = db.Articles.Include("ArticleDetail").Where(a => a.ArticleTypeId == 2 && listID.Contains(a.Id)).ToList();
                article.AddRange(db.Articles.Include("ArticleDetail").Where(a => a.ArticleTypeId == 2 && !listID.Contains(a.Id)).Take(5).ToList());
                ViewBag.Articles = article;

                ViewBag.Count = db.Articles.Include("ArticleDetail").Count(c => c.ArticleTypeId == 2);
                ViewBag.CateCount = db.Categories.Include("CategoryDetail").Count(c => c.ArticleTypeId == 2);
                ViewBag.ID = model.Id;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var pro = db.Discounts.Include(dc => dc.Currency).Include(dc => dc.Categories).Include(dc => dc.Articles).Where(p => p.Id == id).Single();
                TryUpdateModel(pro);

                if (Request["chk-usepercen"] != null)
                {
                    bool[] chek = Request["chk-usepercen"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                    pro.UsePercent = chek[0];
                }
                pro.CurrencyId = int.Parse(Request["currencyId"]);
                if (!string.IsNullOrEmpty(Request["DiscountPercent"]))
                    pro.DiscountPercent = decimal.Parse(Request["DiscountPercent"]);

                if (!string.IsNullOrEmpty(Request["DiscountAmount"]))
                    pro.DiscountAmount = decimal.Parse(Request["DiscountAmount"]);

                List<Category> lstCate = new List<Category>();
                if (form["categories"] != null)
                {
                    var cate = db.Categories.Where(c => c.ArticleTypeId == 2).ToList();
                    var cats = form["categories"].Split(new char[] { ',' });
                    var categories = cate.Where(c => cats.Contains(c.Id.ToString())).ToList();
                    lstCate.AddRange(categories); 
                }
                pro.Categories = lstCate;

                List<Article> lstArt = new List<Article>();
                if (form["articles"] != null)
                {
                    var art = db.Articles.Where(c => c.ArticleTypeId == 2).ToList();
                    var arts = form["articles"].Split(new char[] { ',' });
                    var articles = art.Where(c => arts.Contains(c.Id.ToString())).ToList();
                    lstArt.AddRange(articles);
                }
                pro.Articles = lstArt;
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
                db.Discounts.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}