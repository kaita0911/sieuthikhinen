using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using EntityFramework.Extensions;
using VCMS.MVC4.Web;
using VNS.Web.Helpers;
using Newtonsoft.Json.Linq;
using System.Web.Security;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class CategoryController : VCMSAdminController
    {
        //
        // GET: /Category/
        public ActionResult List(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.ArticleType = db.ArticleTypes.Include(at => at.ArticleTypeDetail).Single(t => t.Id == id);
                var categories = Category.GetTree(Category.GetByType(id, SiteConfig.LanguageId));
               
                return View(categories);
            }
        }
        public JsonResult AddRows(int pageIndex = 1)
        {
            using (DataContext db = new DataContext())
            {
                var categories = Category.GetTree(db.Categories.Include("CategoryDetail").OrderByDescending(c => c.DateCreated).Where(c => c.ArticleTypeId == 2).Skip((pageIndex - 1) * 5).Take(5).ToList());
                List<string> list = new List<string>();
                if (categories.Count() > 0)
                {
                    string html = "";
                    foreach (var item in categories)
                    {
                        var ca_id = "Category_" + item.Id;
                        html += "<tr class='rows'><td><label class='checkbox discount'><input name='" + ca_id + "' value='" + item.Id + "' type='checkbox' class='check'/><i></i></label></td>";
                        html += "<td><span>" + item.CategoryName + "</span></td></tr>";
                    }
                    list.Add(html);
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AutoComplete(string keyword)
        {
            using (DataContext db = new DataContext())
            {
                if (string.IsNullOrEmpty(keyword)) keyword = "";
                var query = (from c in db.Categories
                             join d in db.CategoryDetails on c.Id equals d.CategoryId
                             where c.ArticleTypeId == 2 && d.LanguageId == 1 && (d.CategoryName.Contains(keyword) || keyword == "")
                             select new
                             {
                                 c = c,
                                 d = d
                             });

                var ret = new List<Category>();
                foreach (var item in query.ToList())
                {
                    item.c.CategoryDetail = new VList<CategoryDetail>() { item.d };
                    item.c.LanguageId = 1;
                    ret.Add(item.c);
                }
                List<string> list = new List<string>();
                if (ret.Count > 0)
                {
                    string html = "";
                    foreach (var item in ret)
                    {
                        html += "<tr class='rows'><td><label class='checkbox discount'><input name='categories' value='" + item.Id + "' type='checkbox' class='check'/><i></i></label></td>";
                        html += "<td><span class= 'name'><span data-level='" + item.Level + "' class='level level_" + item.Level + "'>" + item.CategoryName + "</span></span></td></tr>";
                    }
                    list.Add(html);
                }
                else
                    list.Add("<tr class='no-data'><td colspan='4'>Không có kết quả nào hợp lệ.</td></tr>");
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListByCateType(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.CategoryType = db.CategoryTypes.Include(at => at.CategoryTypeDetail).Single(t => t.Id == id);
                var categories = Category.GetTree(Category.GetByCateType(id, SiteConfig.LanguageId));
                return View(categories);
            }
        }

        public ActionResult CreateCateType()
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.CategoryType = db.CategoryTypes.Include("CategoryTypeDetail").ToList();
                ViewBag.Properties = db.Properties.Include("PropertyDetail").ToList();
                CategoryType model = new CategoryType() { CategoryTypeDetail = new VList<CategoryTypeDetail>() };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateCateType(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                CategoryType p = new CategoryType() { Code = form["Code"], CategoryTypeDetail = VList<CategoryTypeDetail>.Create(SiteConfig.Languages) };
                foreach (var l in SiteConfig.Languages)
                {
                    p.CategoryTypeDetail[l.Id].Name = form["txtName_" + l.Id.ToString()];
                }
                db.CategoryTypes.Add(p);
                db.SaveChanges();
            }
            return RedirectToAction("ListCateType");
        }

        public ActionResult ListCateType()
        {
            using (DataContext db = new DataContext())
            {
                var list = db.CategoryTypes.Include(c => c.CategoryTypeDetail).ToList();
                return View(list);
            }
        }

        public ActionResult Create(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.ArticleType = db.ArticleTypes.Include("ArticleTypeDetail").Single(t => t.Id == id);
                ViewBag.Categories = Category.GetTree(Category.GetByType(id, SiteConfig.LanguageId).Where(c => c.ArticleTypeId == id).ToList());
                var model = new Category { CategoryDetail = VList<CategoryDetail>.Create(SiteConfig.Languages) };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                ArticleType type = db.ArticleTypes.Include(at => at.Categories).Where(t => t.Id == id).Single();
                if (ModelState.IsValid)
                {
                    var model = new Category();
                    TryUpdateModel(model);

                    if (model.ParentId > 0)
                        model.Parent = db.Categories.SingleOrDefault(c => c.Id == model.ParentId);
                    else
                        model.ParentId = null;

                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        var file = Uploader.Upload(Request.Files[0], ArticleFileType.IMAGE, "Category");
                        model.ImageUrl = file.FileName;
                    }

                    //model.Flags = CategoryFlags.ACTIVE;
                    model.IsActive = true;
                    //model.WebsiteId = SiteConfig.SiteId;
                    model.ArticleTypeId = id;
                    model.UserCreatedId = SiteConfig.CurrentUser.UserId;

                    if (!string.IsNullOrEmpty(Request["DiscountPrice"]))
                        model.DiscountPrice = decimal.Parse(Request["DiscountPrice"]);

                    List<Discount> lstDis = new List<Discount>();
                    if (type.HasPrice && type.HasDiscount)
                    {
                        if (form["discounts"] != null)
                        {
                            var lst = db.Discounts.ToList();
                            var dis = form["discounts"].Split(new char[] { ',' });
                            var discount = lst.Where(c => dis.Contains(c.Id.ToString())).ToList();
                            lstDis.AddRange(discount);
                            model.Discounts = lstDis;
                        }
                    }

                    db.Categories.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("List", new { id = id });
                }
                else
                    return View();
            }
        }

        public ActionResult CreateByCateType(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.CategoryType = db.CategoryTypes.Include("CategoryTypeDetail").Single(t => t.Id == id);
                ViewBag.Categories = Category.GetTree(Category.GetByCateType(id, SiteConfig.LanguageId).Where(c => c.CategoryTypeId == id).ToList());
                var model = new Category { CategoryDetail = VList<CategoryDetail>.Create(SiteConfig.Languages) };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateByCateType(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    var model = new Category();
                    TryUpdateModel(model);

                    if (model.ParentId > 0)
                        model.Parent = db.Categories.SingleOrDefault(c => c.Id == model.ParentId);
                    else
                        model.ParentId = null;

                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        var file = Uploader.Upload(Request.Files[0], ArticleFileType.IMAGE, "Category");
                        model.ImageUrl = file.FileName;
                    }
                    if (form["ShippedPrice"] != null)
                        model.ShippedPrice = Convert.ToDecimal(form["ShippedPrice"]);
                    //model.WebsiteId = SiteConfig.SiteId;
                    model.CategoryTypeId = id;
                    model.UserCreatedId = SiteConfig.CurrentUser.UserId;
                    db.Categories.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("ListByCateType", new { id = id });
                }
                else
                    return View();
            }
        }

        public ActionResult CreateAjax()
        {
            CategoryType c = new CategoryType() { CategoryTypeDetail = VList<CategoryTypeDetail>.Create(SiteConfig.Languages) };
            return PartialView(c);
        }

        [HttpPost]
        public ActionResult CreateAjax(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                CategoryType c = new CategoryType();
                TryUpdateModel(c);

                c.CategoryTypeDetail = VList<CategoryTypeDetail>.Create(SiteConfig.Languages);
                foreach (var l in SiteConfig.Languages)
                {
                    c.CategoryTypeDetail[l.Id].Name = form["txtName_" + l.Id.ToString()];
                }
                db.CategoryTypes.Add(c);
                db.SaveChanges();
                return Json(new { Id = c.Id, DisplayName = c.CategoryTypeDetail[1].Name });
            }
        }
        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Categories.Include(c => c.CategoryDetail).SingleOrDefault(c => c.Id == id);
                var missing = SiteConfig.Languages.Where(l => !model.CategoryDetail.Any(cd => cd.LanguageId == l.Id)).Select(l => l.Id).ToList();
                missing.ForEach(m => { model.CategoryDetail.Add(new CategoryDetail { LanguageId = m }); });
                if (model == null)
                    return HttpNotFound();
                ViewBag.ArticleType = db.ArticleTypes.Include("ArticleTypeDetail").Single(t => t.Id == model.ArticleTypeId);
                ViewBag.Categories = Category.GetTree(db.Categories.Include(c => c.CategoryDetail).Where(c => c.ArticleTypeId == model.ArticleTypeId).ToList(), model.Id);
                ViewBag.SelectedId = model.ParentId;

                return View(model);
            }
        }

        public ActionResult EditCateType(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.CategoryTypes.Include("CategoryTypeDetail").Where(p => p.Id == id).FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditCateType(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var cateType = db.CategoryTypes.Include("CategoryTypeDetail").Where(p => p.Id == id).Single();
                TryUpdateModel(cateType);
                foreach (var l in SiteConfig.Languages)
                {
                    var dt = cateType.CategoryTypeDetail[l.Id];
                    if (dt == null)
                        cateType.CategoryTypeDetail.Add(new CategoryTypeDetail() { LanguageId = l.Id, Name = form["txtName_" + l.Id.ToString()] });
                    else
                        dt.Name = form["txtName_" + l.Id.ToString()];
                }
                db.SaveChanges();
            }
            return RedirectToAction("ListCateType");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    var model = db.Categories.Include(c => c.CategoryDetail).Include(c => c.Discounts).SingleOrDefault(c => c.Id == id);
                    if (model == null)
                        return HttpNotFound();
                    var img = model.ImageUrl;
                    TryUpdateModel(model);
                    ArticleType type = db.ArticleTypes.Include(at => at.Categories).SingleOrDefault(t => t.Id == model.ArticleTypeId);

                    if (model.ParentId > 0)
                        model.Parent = db.Categories.SingleOrDefault(c => c.Id == model.ParentId);
                    else
                        model.ParentId = null;

                    //model.WebsiteId = SiteConfig.SiteId;
                    model.UserCreatedId = SiteConfig.CurrentUser.UserId;

                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        var file = Uploader.Upload(Request.Files[0], ArticleFileType.IMAGE, "Category");
                        img = file.FileName;
                    }
                    model.ImageUrl = img;
                    if (!string.IsNullOrEmpty(Request["DiscountPrice"]))
                        model.DiscountPrice = decimal.Parse(Request["DiscountPrice"]);
                    List<Discount> lstDis = new List<Discount>();
                    if (type.HasPrice && type.HasDiscount)
                    {
                        if (form["discounts"] != null)
                        {
                            var lst = db.Discounts.ToList();
                            var dis = form["discounts"].Split(new char[] { ',' });
                            var discount = lst.Where(c => dis.Contains(c.Id.ToString())).ToList();
                            lstDis.AddRange(discount);
                        }
                    }
                    model.Discounts = lstDis;

                    db.SaveChanges();
                    return RedirectToAction("List", new { id = model.ArticleTypeId });
                }
                else
                    return View();
            }
        }

        public ActionResult EditByCateType(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Categories.Include(c => c.CategoryDetail).SingleOrDefault(c => c.Id == id);
                var missing = SiteConfig.Languages.Where(l => !model.CategoryDetail.Any(cd => cd.LanguageId == l.Id)).Select(l => l.Id).ToList();
                missing.ForEach(m => { model.CategoryDetail.Add(new CategoryDetail { LanguageId = m }); });
                if (model == null)
                    return HttpNotFound();
                ViewBag.CategoryType = db.CategoryTypes.Include("CategoryTypeDetail").Single(t => t.Id == model.CategoryTypeId);
                ViewBag.Categories = Category.GetTree(db.Categories.Include(c => c.CategoryDetail).Where(c => c.CategoryTypeId == model.CategoryTypeId).ToList(), model.Id);
                ViewBag.SelectedId = model.ParentId;

                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditByCateType(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    var model = db.Categories.Include(c => c.CategoryDetail).SingleOrDefault(c => c.Id == id);
                    if (model == null) return HttpNotFound();
                    var img = model.ImageUrl;
                    TryUpdateModel(model);
                    if (model.ParentId > 0)
                        model.Parent = db.Categories.SingleOrDefault(c => c.Id == model.ParentId);
                    else
                        model.ParentId = null;
                    if (form["ShippedPrice"] != null)
                        model.ShippedPrice = Convert.ToDecimal(form["ShippedPrice"]);
                    //model.WebsiteId = SiteConfig.SiteId;
                    model.UserCreatedId = SiteConfig.CurrentUser.UserId;
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        var file = Uploader.Upload(Request.Files[0], ArticleFileType.IMAGE, "Category");
                        img = file.FileName;
                    }
                    model.ImageUrl = img;
                    db.SaveChanges();
                    return RedirectToAction("ListByCateType", new { id = model.CategoryTypeId });
                }
                else
                    return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateOrder(string id, string order)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var orders = order.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();

                var list = db.Categories.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
                //db.Categories.Update(c => ids.Contains(c.Id), c2 => new Category { SortOrder = orders[ids.IndexOf(c2.Id)] });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult Delete(string idList)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    int[] ids = idList.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                    var query = db.Categories.Where(c => ids.Contains(c.Id));
                    query.Delete();
                    db.SaveChanges();
                    return Json(new { Status = 0, Message = "OK" });
                }
                catch (Exception ex)
                {
                    return Json(new { Status = -1, Message = ex.Message });
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteCateType(string id)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                    var query = db.CategoryTypes.Where(c => ids.Contains(c.Id));
                    query.Delete();
                    db.SaveChanges();
                    return Json(new { Status = 0, Message = "OK" });
                }
                catch (Exception ex)
                {
                    return Json(new { Status = -1, Message = ex.Message });
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateAttributes(string json)
        {
            using (DataContext db = new DataContext())
            {
                JObject jsonData = JObject.Parse("{'items':" + json + "}");
                var category = (from d in jsonData["items"]
                                select new Category { Id = d["id"].Value<int>(), IsActive = d["isActive"].Value<bool>(), IsNew = d["isNew"].Value<bool>(), IsHot = d["isHot"].Value<bool>(), IsMostView = d["isMostview"].Value<bool>(), }
                           ).ToList();
                var ids = category.Select(a => a.Id).ToList();
                var list = db.Categories.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(a =>
                {
                    var item = category.FirstOrDefault(d => d.Id == a.Id);
                    if (item != null)
                    {
                        a.IsActive = item.IsActive;
                        a.IsNew = item.IsNew;
                        a.IsHot = item.IsHot;
                        a.IsMostView = item.IsMostView;
                    }
                });

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        public ActionResult AllParentID(int pId)
        {
            using (DataContext db = new DataContext())
            {
                var list = new List<string>();
                var category = Category.GetById(pId, SiteConfig.LanguageId);
                if (category != null)
                {
                    while (category.ParentId != null)
                    {
                        list.Add(category.ParentId.ToString());
                        category = Category.GetById((int)category.ParentId, SiteConfig.LanguageId);
                    }
                }
                return Json(new { Status = 0, data = list });
            }
        }
    }
}
