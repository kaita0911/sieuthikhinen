using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using VCMS.MVC4.Web.Mailers;
using VCMS.MVC4.Web.Models;
namespace VCMS.MVC4.Web.Controllers
{
    [HandleError]
    //[VCMS.MVC4.Web.WhitespaceFilterAttribute]
    public class ArticleController : BaseController
    {
        //private DataContext db = new DataContext();
        //
        // GET: /Article/

        public ActionResult Index()
        {
            //var articles = db.Articles.Include(a => a.Author);
            //return View(articles.ToList());
            

            return View();
        }
        private UserMailer _userMailer = new UserMailer();
        public UserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }
        public ActionResult Complete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchLocation(FormCollection form)
        {
            var id = -1;
            try
            {
                if (!string.IsNullOrEmpty(form["Street"]))
                    id = Convert.ToInt32(form["Street"]);
                else if (!string.IsNullOrEmpty(form["Wards"]))
                    id = Convert.ToInt32(form["Wards"]);
                else if (!string.IsNullOrEmpty(form["State"]))
                    id = Convert.ToInt32(form["State"]);
                else if (!string.IsNullOrEmpty(form["City"]))
                    id = Convert.ToInt32(form["City"]);
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }
            if (id > 0)
                return RedirectToAction("Detail", "Category", new { id = id });
            else
                return RedirectToAction("Index", "ArticleType", new { code = "san-pham" });
        }

        [HttpPost]
        public ActionResult ContactAjax(FormCollection form)
        {
            try
            {
                int id = Convert.ToInt32(form["contact_id"]);
                var model = new ContactProductModel();
              
                model.Name = form["contact_name"].ToString();
                model.Address = form["contact_address"].ToString();
                model.Phone = form["P_3"].ToString();
                model.Email = form["P_2"].ToString();
                model.Body = form["contact_body"].ToString();

                UserMailer.CreateMessage("ContactProduct", "Contact information - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                ViewBag.Result = 0;

                //using (DataContext db = new DataContext())
                //{

                //    var type = db.ArticleTypes.Include(t => t.Properties).FirstOrDefault(a => a.Code == "CONTACT");
                //    var models = new Article();
                //    TryUpdateModel(models);
                //    var vals = from p in type.Properties.Where(p => !p.MultiLanguage && p.PropertyType != PropertyType.MULTICHOICE && p.PropertyType != PropertyType.CHOICE)
                //               select new ArticlePropertyValue()
                //               {
                //                   PropertyId = p.Id,
                //                   Value = form["P_" + p.Id.ToString()],
                //                   Code = p.Code
                //               };
                //    var propvals = vals.ToList();
                //    models.PropertyValues = propvals;

                //    var details = from d in SiteConfig.Languages
                //                  select new ArticleDetail()
                //                  {
                //                      LanguageId = d.Id,
                //                      ArticleName = model.Name,
                //                      ShortDesc = model.Email,
                //                      Description = model.Phone,
                //                      SEOKeywords = "",

                //                  };

                //    models.ArticleDetail = new VList<ArticleDetail>();
                //    models.ArticleDetail.AddRange(details);
                //    models.DateCreated = DateTime.Now;
                //    models.WebsiteId = SiteConfig.SiteId;
                //    models.ArticleTypeId = type.Id;
                //    models.UserCreated = 1;
                //    db.Articles.Add(models);
                //    db.SaveChanges();
                //}
                model = new ContactProductModel();
                return View(new ContactProductModel());
                
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }

            return Json(new { Status = 0, Message = "OK" });
        }

        [HttpPost]
        public ActionResult ContactProductAjax(FormCollection form)
        {
            try
            {
                int id = Convert.ToInt32(form["contact_id"]);
                var model = new ContactProductModel();
                model.Name = form["contact_name"].ToString();
                model.Company = form["contact_company"].ToString();
                model.Address = form["contact_address"].ToString();
                model.Phone = form["contact_phone"].ToString();
                model.Email = form["contact_email"].ToString();
                model.Body = form["contact_body"].ToString();

                UserMailer.CreateMessage("ContactProduct", "Contact information - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                ViewBag.Result = 0;

                model = new ContactProductModel();
                return PartialView("_Complete");
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }

            return Json(new { Status = 0, Message = "OK" });
        }
        public ActionResult ContactServiceAjax(FormCollection form)
        {
            try
            {
                int id = Convert.ToInt32(form["contact_id"]);
                var model = new ContactProductModel();
                model.Name = form["contact_name"].ToString();
                model.Company = form["contact_company"].ToString();
                model.Phone = form["contact_phone"].ToString();
                model.Email = form["contact_email"].ToString();
                model.Body = form["contact_nd"].ToString();

                UserMailer.CreateMessage("ContactService", "Thông tin liên hệ - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { "inoffset@dptprint.com", model.Email }).Send();
                ViewBag.Result = 0;

                //using (DataContext db = new DataContext())
                //{

                //    var type = db.ArticleTypes.Include(t => t.Properties).FirstOrDefault(a => a.Code == "CONTACT");
                //    var models = new Article();
                //    TryUpdateModel(models);
                //    var vals = from p in type.Properties.Where(p => !p.MultiLanguage && p.PropertyType != PropertyType.MULTICHOICE && p.PropertyType != PropertyType.CHOICE)
                //               select new ArticlePropertyValue()
                //               {
                //                   PropertyId = p.Id,
                //                   Value = form["P_" + p.Id.ToString()],
                //                   Code = p.Code
                //               };
                //    var propvals = vals.ToList();
                //    models.PropertyValues = propvals;

                //    var details = from d in SiteConfig.Languages
                //                  select new ArticleDetail()
                //                  {
                //                      LanguageId = d.Id,
                //                      ArticleName = model.Name,
                //                      ShortDesc = model.Email,
                //                      Description = model.Phone,
                //                      SEOKeywords = "",

                //                  };

                //    models.ArticleDetail = new VList<ArticleDetail>();
                //    models.ArticleDetail.AddRange(details);
                //    models.DateCreated = DateTime.Now;
                //    models.WebsiteId = SiteConfig.SiteId;
                //    models.ArticleTypeId = type.Id;
                //    models.UserCreated = 1;
                //    db.Articles.Add(models);
                //    db.SaveChanges();
                //}
                model = new ContactProductModel();
                return PartialView("_Complete");
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }

            return Json(new { Status = 0, Message = "OK" });
        }
        public ActionResult ContactSupportAjax(FormCollection form)
        {
            try
            {
                int id = Convert.ToInt32(form["contact_id"]);
                var model = new ContactProductModel();
                model.Name = form["contact_name"].ToString();
                model.Company = form["contact_company"].ToString();
                model.Address = form["contact_address"].ToString();
                model.Phone = form["contact_phone"].ToString();
                model.Title = form["contact_title"].ToString();
                model.Email = form["contact_email"].ToString();
                model.Body = form["contact_body"].ToString();

                UserMailer.CreateMessage("ContactSupport", "Thông tin liên hệ - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                ViewBag.Result = 0;

                //using (DataContext db = new DataContext())
                //{

                //    var type = db.ArticleTypes.Include(t => t.Properties).FirstOrDefault(a => a.Code == "CONTACT");
                //    var models = new Article();
                //    TryUpdateModel(models);
                //    var vals = from p in type.Properties.Where(p => !p.MultiLanguage && p.PropertyType != PropertyType.MULTICHOICE && p.PropertyType != PropertyType.CHOICE)
                //               select new ArticlePropertyValue()
                //               {
                //                   PropertyId = p.Id,
                //                   Value = form["P_" + p.Id.ToString()],
                //                   Code = p.Code
                //               };
                //    var propvals = vals.ToList();
                //    models.PropertyValues = propvals;

                //    var details = from d in SiteConfig.Languages
                //                  select new ArticleDetail()
                //                  {
                //                      LanguageId = d.Id,
                //                      ArticleName = model.Name,
                //                      ShortDesc = model.Email,
                //                      Description = model.Phone,
                //                      SEOKeywords = "",

                //                  };

                //    models.ArticleDetail = new VList<ArticleDetail>();
                //    models.ArticleDetail.AddRange(details);
                //    models.DateCreated = DateTime.Now;
                //    models.WebsiteId = SiteConfig.SiteId;
                //    models.ArticleTypeId = type.Id;
                //    models.UserCreated = 1;
                //    db.Articles.Add(models);
                //    db.SaveChanges();
                //}
                model = new ContactProductModel();
                return PartialView("_Complete");
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }

            return Json(new { Status = 0, Message = "OK" });
        }
        public ActionResult Read(int id = 0)
        {
            Article article = Article.GetById(id, SiteConfig.LanguageId);
            if (article == null)
                return HttpNotFound();
            return View(article);
        }
       
        public ActionResult Detail(int id = 0)
        {
            using (DataContext db = new DataContext())
            { 
                var flag = db.ArticleTypes.Where(a => a.Articles.Any(ad => ad.Id == id)).Select(a => new { PropertyFlag = a.PropertyFlag, ShoppingCartFlag = a.ShoppingCartFlag }).FirstOrDefault();
                //var flag = db.ArticleTypes.Select(a => new { PropertyFlag = a.PropertyFlag, ShoppingCartFlag = a.ShoppingCartFlag }).FirstOrDefault();
                if (flag == null)
                    return HttpNotFound();

                var includeFlags = ArticleIncludeFlags.FILES | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PROPERTIES | ArticleIncludeFlags.ARTICLE_DETAIL;
                //if (flag.PropertyFlag.HasFlag(PropertyFlags.DESCRIPTION) || flag.PropertyFlag.HasFlag(PropertyFlags.SHORT_DESCRIPTION))
                //    includeFlags |= ArticleIncludeFlags.ARTICLE_DETAIL;
                if (flag.PropertyFlag.HasFlag(PropertyFlags.CATEGORY))
                    includeFlags |= ArticleIncludeFlags.CATEGORIES;
                if (flag.PropertyFlag.HasFlag(PropertyFlags.MULTI_IMAGE))
                    includeFlags |= ArticleIncludeFlags.FILES;

                if (flag.ShoppingCartFlag.HasFlag(ShoppingCartFlags.PRICE))
                    includeFlags |= ArticleIncludeFlags.PRICES;

                if (flag.ShoppingCartFlag.HasFlag(ShoppingCartFlags.DISCOUNT))
                    includeFlags |= ArticleIncludeFlags.DISCOUNTS;

                Article article = Article.GetById(id, SiteConfig.LanguageId, includeFlags, db);
                if (article == null)
                    return HttpNotFound();

                ArticleType articletype = article.ArticleType;// ArticleType.GetById(article.ArticleTypeId, SiteConfig.SiteId, SiteConfig.LanguageId);
                if (articletype.HasRequiredLogin)
                {
                    if (!Request.IsAuthenticated)
                    {
                        UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                        string url = u.Action("Detail", "Article", new { id = article.Id });
                        return RedirectToAction("Login", "Account", new { returnUrl = HttpUtility.UrlDecode(url) });
                    }
                }

                ViewBag.ArticleID = article.Id;
                if (article.Categories.Count() > 0)
                {
                    var parent = article.Categories.FirstOrDefault(a => (a.ArticleTypeId ?? 0) == articletype.Id && a.CategoryTypeId == null && a.ParentId == null);
                    ViewBag.ParentCategory = parent;
                    ViewBag.Category = article.Categories.Where(c => c.ArticleTypeId == article.ArticleTypeId).OrderByDescending(c => c.Id).FirstOrDefault();
                    //while (parent != null)
                    //{
                    //    var pid = parent.Id;
                    //    ViewBag.Category = parent;
                    //    parent = article.Categories.Where(a => a.ArticleTypeId == article.ArticleType.Id).FirstOrDefault(a => a.ParentId == pid);
                    //}
                }
                ViewBag.Type = articletype;// article.ArticleType;
                ViewBag.ArticleTypeId = articletype.Id;
                var timecho = db.Articles
                  .Include(a => a.ArticleType)
                  .Include(a =>a.ArticleDetail)
                  .Where(a => a.ArticleTypeId == 17).OrderBy(a => a.SortOrder);
                    ViewBag.Timecho = timecho.ToList();

                var brandType = db.ArticleTypes.Include(a => a.ArticleTypeDetail).FirstOrDefault(a => a.Code.Equals("BRAND", StringComparison.OrdinalIgnoreCase));
                ViewBag.Brand = brandType;
                var groupsxqg = db.ArticleTypes.Include(a => a.ArticleTypeDetail).FirstOrDefault(a => a.Code.Equals("GROUPPRODUCT", StringComparison.OrdinalIgnoreCase));
                ViewBag.Groupsxqg = groupsxqg;
                
                //var timecho = Article.GetByTypeCode("TIMECHO", SiteConfig.LanguageId, ArticleFlags.ACTIVE, 1, 10, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.ASCENDING,ArticleIncludeFlags.ARTICLE_DETAIL).List;
                //if (article.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("VIEWS", StringComparison.OrdinalIgnoreCase)) != null)
                //{
                //    using (DataContext db = new DataContext())
                //    {
                //        var Aproperty = db.ArticlePropertyValues.Where(a => a.ArticleId == id && a.Property.Code.Equals("VIEWS", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                //        Aproperty.Value = string.IsNullOrWhiteSpace(Aproperty.Value) ? "1" : (int.Parse(Aproperty.Value) + 1).ToString();
                //        db.SaveChanges();
                //    }
                //}

                if (article.ArticleType.Code.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase))
                {
                    HttpCookie cookie = Request.Cookies["article"];
                    List<string> arr;
                    if (cookie != null)
                        arr = new List<string>(cookie.Value.Split(','));
                        
                    else
                    {
                        arr = new List<string>();
                        cookie = new HttpCookie("article");
                    }
                    if (!arr.Contains(id.ToString()))
                    {
                        arr.Add(id.ToString());
                        cookie.Value = string.Join(",", arr);
                        Response.Cookies.Add(cookie);
                       
                    }      
                }

                // check correct url
                var urlFriendly = article.UrlFriendly;
                //if (string.IsNullOrWhiteSpace(urlFriendly))
                urlFriendly = Unichar.UnicodeStrings.UrlString(article.ArticleName).ToLower();
                urlFriendly = urlFriendly.Replace('\\','-').Replace('"', '-');
                var realUrl = Url.Action("Detail", new { id = id, code = article.ArticleType.ArticleTypeDetail[SiteConfig.LanguageId].UrlFriendly, title = urlFriendly });
                if (!Request.Url.PathAndQuery.Equals(realUrl, StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("Detail", new { id = id, code = article.ArticleType.ArticleTypeDetail[SiteConfig.LanguageId].UrlFriendly, title = urlFriendly });
                //ViewBag.ArticleType = article.ArticleType;
                return View(article);
            }
        }
        public ActionResult CategoryList(string code = "")
        {
            ArticleType item = ArticleType.GetByCode(code, SiteConfig.LanguageId);
            if (item == null)
                return HttpNotFound();
            var category = Category.GetTree(Category.GetByType(item.Id, SiteConfig.LanguageId)).Where(a => a.Level == 0).ToList();
            return View(category);
        }
        public ActionResult QuickView(int id = 0)
        {
            Article article = Article.GetById(id, SiteConfig.LanguageId);
            ArticleType articletype = ArticleType.GetById(article.ArticleTypeId, SiteConfig.LanguageId);
            if (article == null)
                return HttpNotFound();
            ViewBag.ArticleID = article.Id;

            if (article.Categories.Count() > 0)
            {
                var parent = article.Categories.Where(a => a.ArticleTypeId != null && a.ArticleType.Id == article.ArticleType.Id && a.CategoryTypeId == null).FirstOrDefault(a => a.ParentId == null);
                ViewBag.ParentCategory = parent;

                while (parent != null)
                {
                    var pid = parent.Id;
                    ViewBag.Category = parent;
                    parent = article.Categories.Where(a => a.ArticleType.Id == article.ArticleType.Id).FirstOrDefault(a => a.ParentId == pid);
                }
            }
            ViewBag.Type = article.ArticleType;
            if (article.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("VIEWS", StringComparison.OrdinalIgnoreCase)) != null)
            {
                using (DataContext db = new DataContext())
                {
                    var Aproperty = db.ArticlePropertyValues.Where(a => a.ArticleId == id && a.Property.Code.Equals("VIEWS", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    Aproperty.Value = string.IsNullOrWhiteSpace(Aproperty.Value) ? "1" : (int.Parse(Aproperty.Value) + 1).ToString();
                    db.SaveChanges();
                }
            }

            // check correct url
            var urlFriendly = article.UrlFriendly;
            if (string.IsNullOrWhiteSpace(urlFriendly))
                urlFriendly = Unichar.UnicodeStrings.UrlString(article.ArticleName);

            var realUrl = Url.Action("QuickView", new { id = id, code = article.ArticleType.UrlFriendly, title = urlFriendly });
            if (!Request.Url.PathAndQuery.Equals(realUrl, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("QuickView", new { id = id, code = article.ArticleType.UrlFriendly, title = urlFriendly });

            return View(article);
        }
        public ActionResult Other(int id = 0, int typeid = 0, int categoryId = 0, int pageIndex = 1, int pageSize = 10, string viewPath = null, ArticleIncludeFlags includeFlags = ArticleIncludeFlags.PROPERTIES | ArticleIncludeFlags.ARTICLE_TYPE, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING)
        {
            var articleType = (ArticleType)ViewBag.ArticleType;
            ArticleResult result = Article.GetOther(id, typeid, SiteConfig.LanguageId, categoryId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, includeflags: includeFlags, option: ArticleSearchOption.HAS_DESC_ONLY, resultFlag: ArticleResultFlags.ITEMS_ONLY);

            ViewBag.viewPath = viewPath;
            if (Request.IsAjaxRequest() && Request["json"] == "true")
                return Json(result);
            return PartialView(result);
        }
        //ajax//
        public JsonResult GetTweetData( int count, int id = 0,int pageSize = 10)
        {
            try
            {
                using (var context = new DataContext())
                {
                    Article article = Article.GetById(id, SiteConfig.LanguageId);
                    var counted = context.Articles.Count();
                    var countData = counted - pageSize; //it will exclude the previous 6 records

                    var dataContainer = article.Categories.Where(x => x.ArticleTypeId <= countData && x.ArticleTypeId >= 1).OrderByDescending(x => x.ArticleTypeId);
                    var dataContainer2 = dataContainer.Take(6).ToList();
                    return Json(dataContainer2, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, ex = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
        
        
        public ActionResult ViewByCategory(int categoryId, string viewPath = "", int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING)
        {
            if (!string.IsNullOrEmpty(Request["sortorder"]))
                sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);

            if (!string.IsNullOrEmpty(Request["sortdirection"]))
                direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);

            if (!string.IsNullOrEmpty(Request["num"]))
            {
                var num = Convert.ToInt32(Request["num"]);
                if (num > 0)
                    pageSize = num;
            }
            if (!string.IsNullOrEmpty(Request["page"]))
            {
                var num = Convert.ToInt32(Request["page"]);
                if (num > 0)
                    pageIndex = num;
            }
            var model = Article.GetByCategory(categoryId, SiteConfig.LanguageId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, option: ArticleSearchOption.ALL, includeChildren: false);
            if (!string.IsNullOrEmpty(Request["brands"]))
            {
                var ids = Request["brands"].Split('.').Select(Int32.Parse).ToArray();
                var lst = Article.GetByCategories(ids, SiteConfig.LanguageId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, includeFlag: ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.CATEGORIES | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.DISCOUNTS);
                model.List = lst.ToList();
                model.TotalItemCount = Article.CountByCategories(ids, SiteConfig.LanguageId);
            }
            ViewBag.ViewPath = viewPath;
            return PartialView(model);
        }

        public ActionResult ViewByType(int typeId, string viewPath = "", ArticleFlags Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE, int pageIndex = 1, int pageSize = 15, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleIncludeFlags includeFlags = ArticleIncludeFlags.ARTICLE_DETAIL | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PROPERTIES)
        {
            var type = ArticleType.GetById(typeId, SiteConfig.LanguageId);
            ViewBag.Type = type;
            if (!string.IsNullOrEmpty(Request["sortorder"]))
                sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);

            if (!string.IsNullOrEmpty(Request["sortdirection"]))
                direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);

            if (!string.IsNullOrEmpty(Request["num"]))
            {
                var num = Convert.ToInt32(Request["num"]);
                if (num > 0)
                    pageSize = num;
            }

            if (!string.IsNullOrEmpty(Request["page"]))
            {
                var num = Convert.ToInt32(Request["page"]);
                if (num > 0)
                    pageIndex = num;
            }

            if (!string.IsNullOrEmpty(Request["flag"]))
            {
                if (Request["flag"] == "all")
                    Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
                else
                    Flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
            }
            var model = Article.GetByType(typeId, SiteConfig.LanguageId, flags: Flag, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, includeflags: includeFlags, resultFlag: ArticleResultFlags.ALL);
            //var items = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType).Where(a => a.ArticleTypeId == id).OrderBy(a => a.ArticleDetail.Min(d => d.ArticleName.Trim())).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //var count = db.Articles.Include(a => a.ArticleType).Where(a => a.ArticleTypeId == id).Count();
            //var articles = new ArticleResult { TotalItemCount = count, List = items, PageIndex = page, PageSize = pageSize };

            if (!string.IsNullOrEmpty(Request["brands"]))
            {
                var ids = Request["brands"].Split('.').Select(Int32.Parse).ToArray();
                var lst = Article.GetByCategories(ids, SiteConfig.LanguageId, flag: Flag, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, includeFlag: ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.CATEGORIES | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.DISCOUNTS);
                model.List = lst.ToList();
                model.TotalItemCount = Article.CountByCategories(ids, SiteConfig.LanguageId, flag: Flag);
            }
            if (!string.IsNullOrEmpty(Request["isDiscount"]))
                model = Article.GetByDiscount(typeId, SiteConfig.LanguageId, flags: Flag, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction);

            ViewBag.ViewPath = viewPath;
            return PartialView(model);
        }

        //public ActionResult ViewByDiscount(int typeId, string viewPath = "", int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING)
        //{
        //    ArticleFlags Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
        //    if (Request["sortorder"] != null)
        //        sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
        //    if (Request["sortdirection"] != null)
        //        direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
        //    if (Request["flag"] != null)
        //    {
        //        if (Request["flag"] == "all")
        //            Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
        //        else
        //            Flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
        //    }
        //    var model = Article.GetByDiscount(typeId, SiteConfig.SiteId, SiteConfig.LanguageId, flags: Flag, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction);
        //    ViewBag.ViewPath = viewPath;
        //    return PartialView(model);
        //}

        public ActionResult ViewByTypeCode(string typeCode, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING)
        {
            var model = Article.GetByTypeCode(typeCode, SiteConfig.LanguageId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CountClickNumber(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Articles.FirstOrDefault(u => u.Id == id);
                if (model != null)
                {
                    model.Rating = model.Rating + 1;
                    db.SaveChanges();
                }
            }
            return Json(new { Status = 1 });
        }

        public ActionResult Attribute(int typeId, string viewPath = "", int pageIndex = 1, int pageSize = 20, ArticleFlags Flag = ArticleFlags.ACTIVE, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING)
        {
            if (Request["sortorder"] != null)
                sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
            if (Request["sortdirection"] != null)
                direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
            if (Request["flag"] != null)
            {
                if (Request["flag"] == "all")
                    Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
                else
                    Flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
            }

            var model = Article.GetByType(typeId, SiteConfig.LanguageId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, flags: Flag);
            ViewBag.ViewPath = viewPath;
            return PartialView(model);
        }

        public ActionResult Box(int typeId = 0, string typeCode = "", string viewName = "Box", string caption = "", int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER)
        {

            var model = new ArticleBoxModel { Articles = new List<Article>(), ArticleType = null, Caption = caption };

            if (typeId > 0)
            {
                model.Articles = Article.GetByType(typeId, SiteConfig.LanguageId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder).List;

            }
            else if (!string.IsNullOrWhiteSpace(typeCode))
                model.Articles = Article.GetByTypeCode(typeCode, SiteConfig.LanguageId, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder).List;
            if (model.Articles.Count > 0) model.ArticleType = model.Articles[0].ArticleType;

            return PartialView(viewName, model);

        }
        [HttpGet]
        public ActionResult InfinateScrollCategory(int cateId, int pageSize = 20)
        {
            System.Threading.Thread.Sleep(1000);
            var str_html = "";
            ArticleFlags Flag = ArticleFlags.ACTIVE;
            var model = Article.GetByCategory(cateId, SiteConfig.LanguageId, flags: Flag, pageIndex: 1, pageSize: pageSize);
            if (model.List.Count > 0)
            {
                var type = ArticleType.GetById(model.List.FirstOrDefault().ArticleType.Id, SiteConfig.LanguageId);
                ViewBag.Type = type;
                var viewName = "AjaxList";
                str_html = RenderPartialViewToString(viewName, type, model.List);
            }
            else
                str_html = "";

            return Json(str_html, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        protected string RenderPartialViewToString(string viewName, ArticleType type, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;
            var code = "";
            if (type != null)
                code = type.Code + "/";
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, code + viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

    }
}