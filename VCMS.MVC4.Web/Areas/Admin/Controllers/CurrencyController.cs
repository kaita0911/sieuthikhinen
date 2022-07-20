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
    public class CurrencyController : VCMSAdminController
    {
        //
        // GET: /Admin/currency/

        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                return View(db.Currencies.ToList());
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
                Currency c = new Currency();
                TryUpdateModel(c);
                c.Code = form["Code"].ToString();
                c.DisplayName = form["DisplayName"].ToString();
                c.Rates = Decimal.Parse(form["Rates"].ToString());
                c.Signal = form["Signal"].ToString();
                if (form["Chose"].Contains("true"))
                {
                    var list = db.Currencies.ToList();
                    list.ForEach(cs => { cs.Chose = false; });
                    c.Chose = true;
                }
                db.Currencies.Add(c);
                db.SaveChanges();
                return Json(new { data = 1 });
            }
            //return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Currencies.FirstOrDefault(a => a.Id == id);
                return PartialView("Edit", model);
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var currency = db.Currencies.Where(l => l.Id == id).Single();
                TryUpdateModel(currency);
                currency.Code = form["Code"].ToString();
                currency.DisplayName = form["DisplayName"].ToString();
                currency.Rates = Decimal.Parse(form["Rates"].ToString());
                currency.Signal = form["Signal"].ToString();
                if (form["Chose"].Contains("true"))
                {
                    var list = db.Currencies.ToList();
                    list.ForEach(cs => { cs.Chose = false; });
                    currency.Chose = true;
                }
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
                var currency = db.Currencies.FirstOrDefault(a => a.Id == id);
                var list = db.Currencies.ToList();
                list.ForEach(df => { df.Chose = false; });
                currency.Chose = true;
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
                db.Currencies.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
