using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace VCMS.MVC4.Extensions
{
    public static class BreadcrumbExtensions
    {
        public static MvcHtmlString Breadcrumb(this HtmlHelper html)
        {
            var breadcrumbs = (BreadcrumbModel)html.ViewData["Breadcrumb"];

            if (breadcrumbs == null) {
                if (html.ViewContext.HttpContext.Items["Breadcrumb"] != null)
                    breadcrumbs = (BreadcrumbModel)html.ViewContext.HttpContext.Items["Breadcrumb"];
                else
                    breadcrumbs = new BreadcrumbModel { Items = new List<BreadcrumbItem>() };
            }
            html.ViewContext.HttpContext.Items["Breadcrumb"] = breadcrumbs;
            return html.Partial("_Breadcrumb",breadcrumbs);
        }
    }
    public class BreadcrumbModel
    {
        public string CssClass { get; set; }
        public ICollection<BreadcrumbItem> Items { get; set; }

        public void AddItem(BreadcrumbItem item)
        {
            if (Items.Count > 0)
                Items.Last().IsLast = false;
            item.IsLast = true;
            Items.Add(item);
        }
    }

    public class BreadcrumbItem
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }

    }
}
