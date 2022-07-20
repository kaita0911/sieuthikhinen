using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using VCMS.MVC4.Extensions;

namespace VCMS.MVC4.Web.Controllers
{
    public class CurrencyController : Controller
    {
        //
        // GET: /Language/
        
        public ActionResult Index(int id)
        {
            var oldCurr = SiteConfig.CurrencyId;
            SiteConfig.CurrencyId = id;
            var url = Request.UrlReferrer == null ? Request.Url.GetLeftPart(UriPartial.Authority) : Request.UrlReferrer.GetLeftPart(UriPartial.Path);
            var query = Request.UrlReferrer == null ? "" : Request.UrlReferrer.Query;
            RouteInfo info = new RouteInfo(new Uri(url), Request.ApplicationPath);
            var title = info.RouteData.Values["title"];
            var detailId = info.RouteData.Values["id"];
            var newUrl = new UrlHelper(ControllerContext.RequestContext).RouteUrl(info.RouteData.Values);
            if (!string.IsNullOrEmpty(query))
                newUrl = newUrl + query;
            if (Request.IsAjaxRequest())
                return Json(new { url = newUrl, status = 0, message = "Currency changed" }, JsonRequestBehavior.AllowGet);
            else
                return Redirect(newUrl);
        }
    }
}
