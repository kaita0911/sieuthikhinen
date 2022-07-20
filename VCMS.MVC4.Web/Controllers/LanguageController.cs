using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using VCMS.MVC4.Extensions;

namespace VCMS.MVC4.Web.Controllers
{
    public class LanguageController : Controller
    {
        //
        // GET: /Language/

        public ActionResult Index(int id)
        {
            var oldLang = SiteConfig.LanguageId;
            SiteConfig.LanguageId = id;
            var url = Request.UrlReferrer == null ? Request.Url.GetLeftPart(UriPartial.Authority) : Request.UrlReferrer.GetLeftPart(UriPartial.Path);
            var query = Request.UrlReferrer == null ? "" : Request.UrlReferrer.Query;
            RouteInfo info = new RouteInfo(new Uri(url), Request.ApplicationPath);
            var code = info.RouteData.Values["code"];
            if (code != null)
            {
                code = TranslateCode(code.ToString(), oldLang, id);
                info.RouteData.Values["code"] = code;
            }
            var title = info.RouteData.Values["title"];
            var detailId = info.RouteData.Values["id"];
            if (title != null && detailId != null)
            {
                var controller = info.RouteData.Values["controller"];
                if (controller.ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
                {
                    title = TranslateCategoryTitle(title.ToString(), int.Parse(detailId.ToString()), oldLang, id);
                }
                else if (controller.ToString().Equals("article", StringComparison.OrdinalIgnoreCase))
                {
                    title = TranslateArticleTitle(title.ToString(), int.Parse(detailId.ToString()), oldLang, id);
                }
                info.RouteData.Values["title"] = title;
            }
            var newUrl = new UrlHelper(ControllerContext.RequestContext).RouteUrl(info.RouteData.Values);
            if (!string.IsNullOrEmpty(query))
                newUrl = newUrl + query;
            if (Request.IsAjaxRequest())
                return Json(new { url = newUrl, status = 0, message = "Language changed" }, JsonRequestBehavior.AllowGet);
            else
                return Redirect(newUrl);
        }
        private string TranslateCategoryTitle(string code, int detailId, int oldLang, int newLang)
        {
            var a = db.Categories.Include("CategoryDetail").Where(at => at.Id == detailId).FirstOrDefault();
            if (a != null)
            {
                if (string.IsNullOrWhiteSpace(a.CategoryDetail[newLang].UrlFriendly))
                    return Unichar.UnicodeStrings.UrlString(a.CategoryDetail[newLang].CategoryName);
                return a.CategoryDetail[newLang].UrlFriendly;
            }
            else
                return code;
        }
        private string TranslateArticleTitle(string code, int detailId, int oldLang, int newLang)
        {
            var a = db.Articles.Include("ArticleDetail").Where(at => at.Id == detailId).FirstOrDefault();
            if (a != null)
            {
                if (string.IsNullOrWhiteSpace(a.ArticleDetail[newLang].UrlFriendly))
                    return Unichar.UnicodeStrings.UrlString(a.ArticleDetail[newLang].ArticleName);
                return a.ArticleDetail[newLang].UrlFriendly;
            }
            else
                return code;
        }
        private string TranslateCode(string code, int oldLang, int newLang)
        {
            var a = db.ArticleTypes.Include("ArticleTypeDetail").Where(at => at.ArticleTypeDetail.Any(ad => ad.LanguageId == oldLang && ad.UrlFriendly.Equals(code, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
            if (a != null)
            {
                return a.ArticleTypeDetail[newLang].UrlFriendly;
            }
            else
                return code;
        }

        DataContext db = new DataContext();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
