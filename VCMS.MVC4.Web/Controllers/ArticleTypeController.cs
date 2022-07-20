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
    public class ArticleTypeController : BaseController
    {
        public ActionResult Index(string code, int pageIndex = 1, int pageSize = 9)
        {
            ArticleType item = ArticleType.GetByCode(code, SiteConfig.LanguageId);
            if (item == null)
                return HttpNotFound();

            if (item.HasRequiredLogin)
            {
                if (!Request.IsAuthenticated)
                {
                    UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = u.Action("Index", "ArticleType", new { code = item.UrlFriendly });
                    return RedirectToAction("Login", "Account", new { returnUrl = HttpUtility.UrlDecode(url) });
                }
            }

            ViewBag.Type = item;
            ViewBag.ArticleTypeId = item.Id;
            if (item.HomePageType == PageType.FIRST_ARTICLE)
            {
                var articles = Article.GetByType(item.Id, SiteConfig.LanguageId, pageIndex: 1, pageSize: 1,resultFlag: ArticleResultFlags.ALL);
                if (articles.TotalItemCount > 0)
                {
                    var model = articles.List.FirstOrDefault();
                    return RedirectToAction("Detail", "Article", new { code = code, title = model.UrlFriendly, id = model.Id });
                }
            }

            if (item.HomePageType == PageType.LIST_CATEGORY)
                return RedirectToAction("CategoryList", "Article", new { code = code });

            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, item.Code, null);
            if (result.View != null)
                return View(item.Code, item);
            else
                return View(item);
        }

        public ActionResult Attribute(string code, string attr)
        {
            ArticleType item = ArticleType.GetByCode(code, SiteConfig.LanguageId);
            if (item == null)
                return HttpNotFound();

            ViewBag.Type = item;
            ViewBag.Attribute = attr;
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, item.Code + "Attribute", null);
            if (result.View != null)
                return View(item.Code + "Attribute", item);
            else
                return View(item);
        }
        public ActionResult Discount(string code)
        {
            ArticleType item = ArticleType.GetByCode(code, SiteConfig.LanguageId);
            if (item == null)
                return HttpNotFound();
            ViewBag.Type = item;
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, item.Code + "Discount", null);
            if (result.View != null)
                return View(item.Code + "Discount", item);
            else
                return View(item);
        }
        public ActionResult MenuBox(int typeId = 0, string typeCode = "")
        {
            using (DataContext db = new DataContext())
            {
                var at = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Include(a => a.Categories.Select(c => c.CategoryDetail)).FirstOrDefault(a => a.Id == typeId || a.Code.Equals(typeCode, StringComparison.OrdinalIgnoreCase));
                return PartialView(at);
            }
        }
    }
}
