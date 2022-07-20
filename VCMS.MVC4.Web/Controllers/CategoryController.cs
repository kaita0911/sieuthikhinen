using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;

namespace VCMS.MVC4.Web.Controllers
{
    [HandleError]
    //[VCMS.MVC4.Web.WhitespaceFilterAttribute]
    public class CategoryController : BaseController
    {
        // 
        // GET: /Category/

        public ActionResult Index(int id = 0)
        {
            var item = Category.GetById(id, SiteConfig.LanguageId);
            if (item == null)
                return HttpNotFound();
            if (item.ArticleType != null && item.ArticleType.CategoryPageType == PageType.FIRST_ARTICLE)
            {
                var articles = Article.GetByCategory(id, SiteConfig.LanguageId, pageIndex: 1, pageSize: 1);
                if (articles.List.Count > 0)
                {
                    var model = articles.List.FirstOrDefault();
                    return RedirectToAction("Detail", "Article", new { code = item.ArticleType.UrlFriendly, title = model.UrlFriendly, id = model.Id });
                }
            }
            if (item.ArticleType != null)
            {
                ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, item.ArticleType.Code, null);
                if (result.View != null)
                    return View(item.ArticleType.Code, item);
            }
            return View(item);
        }
        public ActionResult Brand()
        {
            using (DataContext db = new DataContext())
            {
                var model = Category.GetTree(Category.GetByCateType(1, SiteConfig.LanguageId)).Where(a => a.Level == 0).ToList();
                return View(model);
            }
        }

        public ActionResult ListCateType(string code)
        {
            using (DataContext db = new DataContext())
            {
                var model = Category.GetTree(Category.GetByCateTypeCode(code, SiteConfig.LanguageId)).Where(a => a.Level == 0).ToList();
                return View(model);
            }
        }

        public ActionResult Collection()
        {
            using (DataContext db = new DataContext())
            {
                var model = Category.GetTree(Category.GetByCateType(2, SiteConfig.LanguageId)).Where(a => a.Level == 0).ToList();
                return View(model);
            }
        }
        public ActionResult Detail(int id = 0, int page = 1, int pageSize = 36)
        {
            var item = Category.GetById(id, SiteConfig.LanguageId);
            if (item == null)
                return HttpNotFound();

            //ArticleType articletype = ArticleType.GetById(item.ArticleType.Id, SiteConfig.SiteId, SiteConfig.LanguageId);
            var articletype = item.ArticleType;
            if (articletype.HasRequiredLogin)
            {
                if (!Request.IsAuthenticated)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Detail", "Category", new { id = item.Id, code = item.ArticleType.UrlFriendly, title = item.UrlFriendly });
                    return RedirectToAction("Login", "Account", new { returnUrl = HttpUtility.UrlDecode(url) });
                }
            }
            ViewBag.Category = item;
            //var category = item;
            //while (category.ParentId != null)
            //{
            //    category = Category.GetById((int)category.ParentId, SiteConfig.SiteId, SiteConfig.LanguageId);
            //}
            //if (category != null)
            //    ViewBag.ParentCategory = category;
            if (item.ParentId != null)
            {
                var tree = Category.BuildTreeReverse(id, SiteConfig.LanguageId);
                ViewBag.Tree = tree;

                ViewBag.CategoryImages = tree.FirstOrDefault().ImageUrl;
            }
            ViewBag.Type = item.ArticleType;
            ViewBag.ArticleTypeId = item.ArticleTypeId;

            //Only_Article

            //if (item.ArticleType != null)
            //{
            //    item.ArticleType.LanguageId = SiteConfig.LanguageId;
            //    var articles = Article.GetByCategory(id, SiteConfig.LanguageId, includeChildren: false, resultFlag: ArticleResultFlags.COUNT_ONLY);
            //    if (articles.TotalItemCount == 1)
            //    {
            //        var model = Article.GetByCategory(id, SiteConfig.LanguageId, pageSize: 1, includeChildren: false, includeflags: ArticleIncludeFlags.NONE).List.FirstOrDefault();
            //        return RedirectToAction("Detail", "Article", new { code = item.ArticleType.UrlFriendly, title = model.UrlFriendly, id = model.Id });
            //    }
            //}
            //check correct url
            var urlFriendly = item.UrlFriendly.ToLower(); //Unichar.UnicodeStrings.UrlString(item.CategoryName.ToLower());
            //if (string.IsNullOrWhiteSpace(urlFriendly))
            //    urlFriendly = Unichar.UnicodeStrings.UrlString(item.CategoryName);
            //var realUrl = Url.Action("Detail", new { id = id, code = item.ArticleType.UrlFriendly, title = urlFriendly });
            var realUrl = Url.Action("Detail", new { id = id, code = item.ArticleType.ArticleTypeDetail[SiteConfig.LanguageId].UrlFriendly, title = urlFriendly });
            if (!Request.Url.PathAndQuery.Equals(realUrl, StringComparison.OrdinalIgnoreCase))
            {
                var kt = 0;
                string[] check = Request.Url.PathAndQuery.Split(new Char[] { '?', '=', '&' }).ToArray();
                check.ToList().ForEach(a =>
                {
                    if (a.ToString().Equals("pageSize") || a.ToString().Equals("page") || a.ToString().Equals("mode") || a.ToString().Equals("sortorder") || a.ToString().Equals("sortdirection") || a.ToString().Equals("flag") || a.ToString().Equals("brands") || a.ToString().Equals("num"))
                    {
                        kt = 1;
                    }
                });
                if (kt != 1)
                {
                    return RedirectToAction("Detail", new { id = id, code = item.ArticleType.UrlFriendly, title = urlFriendly });
                }
            }
            if (item.ArticleType != null)
            {
                ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, item.ArticleType.Code, null);
                if (result.View != null)
                    return View(item.ArticleType.Code, item);
            }
            return View(item);
        }
        public ActionResult DetailType(int id = 0)
        {
            using (DataContext db = new DataContext())
            {
                var item = Category.GetByTypeId(id, SiteConfig.LanguageId);
                if (item == null)
                    return HttpNotFound();
                if (item.CategoryType != null)
                {
                    var type = db.ArticleTypes.Include(a => a.ArticleTypeDetail).FirstOrDefault(a => a.Code.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase));
                    if (type != null)
                        item.ArticleType = type;
                }
                if (item.ArticleType != null)
                {
                    item.ArticleType.LanguageId = SiteConfig.LanguageId;
                    var articles = Article.GetByCategory(id, SiteConfig.LanguageId, pageIndex: 1, pageSize: 2, includeChildren: false);
                    if (articles.List.Count == 1)
                    {
                        var model = articles.List.FirstOrDefault();
                        return RedirectToAction("Detail", "Article", new { code = item.ArticleType.Code, title = model.UrlFriendly, id = model.Id });
                    }
                }
                //check correct url
                var urlFriendly = Unichar.UnicodeStrings.UrlString(item.CategoryName); // item.UrlFriendly;
                //if (string.IsNullOrWhiteSpace(urlFriendly))
                //    urlFriendly = Unichar.UnicodeStrings.UrlString(item.CategoryName);
                var realUrl = Url.Action("DetailType", new { id = id, title = urlFriendly });
                if (!Request.Url.PathAndQuery.Equals(realUrl, StringComparison.OrdinalIgnoreCase))
                {
                    var kt = 0;
                    string[] check = Request.Url.PathAndQuery.Split(new Char[] { '?', '=', '&' }).ToArray();
                    check.ToList().ForEach(a =>
                    {
                        if (a.ToString().Equals("pageSize") || a.ToString().Equals("pageIndex") || a.ToString().Equals("mode") || a.ToString().Equals("sortorder") || a.ToString().Equals("sortdirection") || a.ToString().Equals("flag"))
                        {
                            kt = 1;
                        }
                    });
                    if (kt != 1)
                    {
                        return RedirectToAction("DetailType", new { id = id, title = urlFriendly });
                    }
                }
                if (item.ArticleType != null)
                {
                    ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, item.ArticleType.Code, null);
                    if (result.View != null)
                        return View(item.ArticleType.Code, item);
                }
                return View(item);
            }
        }
        [OutputCache(Duration = 60)]
        public ActionResult Menu(int typeId)
        {
            var categories = Category.GetByType(typeId, SiteConfig.LanguageId);
            var tree = Category.GetTree(categories);
            return PartialView(tree);
        }

        public ActionResult ViewDetail(string category)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Categories.Where(c => c.CategoryDetail.Any(cd => cd.UrlFriendly.Equals(category, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
                return View(model);
            }
        }
        public ActionResult SelectState(int parentid)
        {
            using (DataContext db = new DataContext())
            {
                var category = Category.GetByParent(parentid, 4, SiteConfig.LanguageId);
                return PartialView("_SelectState", category);
            }
        }
        public ActionResult SelectWards(int parentid)
        {
            using (DataContext db = new DataContext())
            {
                var category = Category.GetByParent(parentid, 4, SiteConfig.LanguageId);
                return PartialView("_SelectWards", category);
            }
        }
        public ActionResult SelectStreet(int parentid)
        {
            using (DataContext db = new DataContext())
            {
                var category = Category.GetByParent(parentid, 4, SiteConfig.LanguageId);
                return PartialView("_SelectStreet", category);
            }
        }
    }
}
