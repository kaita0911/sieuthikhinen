using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using VCMS.MVC4.Web.App_Start;

namespace VCMS.MVC4.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            int langId = SiteConfig.LanguageId;
            //detect language based on ArticleType UrlFriendly. It should be unique for accurate data
            var lang = requestContext.HttpContext.Request["lang"];
            if (lang != null)
            {
                int.TryParse(requestContext.HttpContext.Request["lang"], out langId);
            }
            else
            {
                if (requestContext.RouteData.Values["code"] != null)
                {
                    var code = requestContext.RouteData.Values["code"].ToString();
                    using (DataContext db = new DataContext())
                    {
                        var _type = (from a in db.ArticleTypes
                                       join d in db.ArticleTypeDetails on a.Id equals d.ArticleTypeId
                                       where d.UrlFriendly.ToLower() == code.ToLower()
                                       select d).FirstOrDefault();

                        if (_type != null)
                            langId = _type.LanguageId;
                    }
                }
            }
            if (langId >0 && langId != SiteConfig.LanguageId)
            {
                SiteConfig.LanguageId = langId;
                NameValueCollection query = new NameValueCollection(requestContext.HttpContext.Request.QueryString);
                query.Remove("lang");
                var newUrl = requestContext.HttpContext.Request.Url.AbsolutePath + (query.Count > 0 ? "?" + HttpUtility.ParseQueryString(query.ToString()).ToString() : "");
                requestContext.HttpContext.Response.Redirect(newUrl, true);
                return;
            }
            base.Initialize(requestContext);
        }
        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            base.Execute(requestContext);
        }
      
    }
}