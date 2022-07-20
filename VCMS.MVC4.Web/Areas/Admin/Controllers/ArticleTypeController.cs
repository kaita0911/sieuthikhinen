using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using VCMS.MVC4.Web;
using EntityFramework.Extensions;
using VNS.Web.Helpers;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    //[Authorize]
    public class ArticleTypeController : VCMSAdminController
    {
        //
        // GET: /ArticleType/

        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                var items = db.ArticleTypes.Include("ArticleTypeDetail").OrderBy(at => at.SortOrder).ToList();
                return View(items);
            }
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            using (DataContext db = new DataContext())
            {
                var items = db.ArticleTypes.Include("ArticleTypeDetail").Include(ct => ct.CategoryTypes.Select(t => t.CategoryTypeDetail)).OrderBy(at => at.SortOrder).ToList();
                return PartialView(items);
            }
        }

        public ActionResult Create()
        {
            using (DataContext db = new DataContext())
            {
                var cateType = db.CategoryTypes.Include("CategoryTypeDetail").Where(a => !a.NoneType).ToList();
                ViewBag.CategoryType = cateType;
                ViewBag.Properties = db.Properties.Include("PropertyDetail").ToList();
                ArticleType model = new ArticleType() { ArticleTypeDetail = new VList<ArticleTypeDetail>(), CategoryTypes = cateType };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Create(ArticleType model)
        {
            using (DataContext db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    model.ArticleTypeDetail = new VList<ArticleTypeDetail>();

                    foreach (var item in SiteConfig.Languages)
                    {
                        model.ArticleTypeDetail.Add(new ArticleTypeDetail() { LanguageId = item.Id, Name = Request["txtName" + item.Id.ToString()], UrlFriendly = Request["txtUrlFriendly" + item.Id.ToString()] });
                    }
                    if (Request["catetype"] != null)
                    {
                        IEnumerable<int> catetypes = from s in Request["catetype"].Split(',')
                                                     where s != "false"
                                                     select int.Parse(s);
                        model.CategoryTypes = db.CategoryTypes.Where(c => catetypes.Contains(c.Id)).ToList();
                    }
                    if (Request["prop"] != null)
                    {
                        IEnumerable<int> props = from s in Request["prop"].Split(',')
                                                 where s != "false"
                                                 select int.Parse(s);
                        model.Properties = db.Properties.Where(p => props.Contains(p.Id)).ToList();
                    }
                    db.ArticleTypes.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Properties = db.Properties.Include("PropertyDetail").ToList();
                    return View(model);
                }
            }
        }

        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.CategoryType = db.CategoryTypes.Include(p => p.CategoryTypeDetail).Where(a => !a.NoneType).ToList();
                ViewBag.Properties = db.Properties.Include(p => p.PropertyDetail).ToList();
                var model = db.ArticleTypes.Include(at => at.ArticleTypeDetail).Include(at => at.Properties).Include(ct => ct.CategoryTypes).SingleOrDefault(at => at.Id == id);
                if (model == null) return HttpNotFound();
                return View(model);
            }
        }

        [HttpPost]
      
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.ArticleTypes.Include(at => at.ArticleTypeDetail).Include(at => at.Properties).Include(ct => ct.CategoryTypes).SingleOrDefault(at => at.Id == id);
                if (ModelState.IsValid)
                {
                    UpdateModel(model);
                    model.ArticleTypeDetail = new VList<ArticleTypeDetail>();

                    foreach (var item in SiteConfig.Languages)
                    {
                        model.ArticleTypeDetail.Add(new ArticleTypeDetail() { LanguageId = item.Id, Name = Request["txtName" + item.Id.ToString()], UrlFriendly = Request["txtUrlFriendly" + item.Id.ToString()] });
                    }
                    if (Request["catetype"] != null)
                    {
                        IEnumerable<int> catetypes = from s in Request["catetype"].Split(',')
                                                     where s != "false"
                                                     select int.Parse(s);
                        model.CategoryTypes = db.CategoryTypes.Where(c => catetypes.Contains(c.Id)).ToList();
                    }
                    if (Request["prop"] != null)
                    {
                        IEnumerable<int> props = from s in Request["prop"].Split(',')
                                                 where s != "false"
                                                 select int.Parse(s);
                        model.Properties = db.Properties.Where(p => props.Contains(p.Id)).ToList();
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Properties = db.Properties.Include("PropertyDetail").ToList();
                    return View(model);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.ArticleTypes.Delete(af => ids.Contains(af.Id));
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

                var list = db.ArticleTypes.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        public ActionResult Detail(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.ArticleTypes.Include(at => at.ArticleTypeDetail).Include(at => at.Properties).SingleOrDefault(at => at.Id == id);
                if (model == null) return HttpNotFound();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detail(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.ArticleTypes.Include(at => at.ArticleTypeDetail).Include(at => at.Properties).SingleOrDefault(at => at.Id == id);
                if (ModelState.IsValid)
                {
                    TryUpdateModel(model);
                    model.ArticleTypeDetail = new VList<ArticleTypeDetail>();

                    foreach (var item in SiteConfig.Languages)
                    {
                        model.ArticleTypeDetail.Add(new ArticleTypeDetail()
                        {
                            LanguageId = item.Id,
                            Name = Request["txtName" + item.Id.ToString()],
                            UrlFriendly = Request["txtUrlFriendly" + item.Id.ToString()],
                            SEOKeywords = HTMLHelper.KeywordsPrepare(Request[string.Format("ArticleTypeDetail[{0}].SEOKeywords", item.Id)]),
                            SEODescription = Request[string.Format("ArticleTypeDetail[{0}].SEODescription", item.Id)],
                            Description = Request[string.Format("ArticleTypeDetail[{0}].Description", item.Id)],
                        });
                    }
                    db.SaveChanges();
                    return Json(new { status = 0, message = "OK" });
                }
                else
                {
                    return Json(new { status = -1, message = "failed" });
                }
            }
        }
    }
}
