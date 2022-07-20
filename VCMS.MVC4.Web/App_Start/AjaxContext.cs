using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VCMS.MVC4.Web
{
    public static class AjaxContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="partialView"></param>
        /// <param name="callback">The js function/statement called after Ajax loaded</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPartial(this HtmlHelper helper, string partialView, string callback = null)
        {
            var url = new UrlHelper(helper.ViewContext.RequestContext).Action("Partial", "Ajax", new { viewName = partialView });
            var content = string.Format("<div id='ajaxpartial-{0}' data-partial='{1}' data-callback='{2}'></div>", partialView, url,callback);
            
            return MvcHtmlString.Create(content);
        }
    }
}