using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
namespace VCMS.MVC4.Web
{
    /// <summary>
    /// Summary description for HTMLHelper
    /// </summary>
    /// 
    public class CustomHtmlHelper<T> : HtmlHelper<T>
    {
        public CustomHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) :
            base(viewContext, viewDataContainer)
        {
        }

        //Instance methods will always be called instead of extension methods when both exist with the same signature...

        public MvcHtmlString ActionLink(string linkText, string actionName)
        {
            return ActionLink(linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, object routeValues)
        {
            return ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName)
        {
            return ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return ActionLink(linkText, actionName, null, routeValues, new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return ActionLink(linkText, actionName, null, routeValues, htmlAttributes);
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string template = "";
            if (HttpContext.Current.Request["template"] != null)
                template = HttpContext.Current.Request["template"];

            if (!String.IsNullOrEmpty(template))
            {
                if (!routeValues.ContainsKey("template"))
                    routeValues.Add("template", template);
                else
                {
                    routeValues.Remove("template");
                    routeValues.Add("template", template);
                }
            }
            return MvcHtmlString.Create(GenerateLink(ViewContext.RequestContext, RouteCollection, linkText, null, actionName, controllerName, routeValues, htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string template = "";
            if (HttpContext.Current.Request["template"] != null)
                template = HttpContext.Current.Request["template"];
            
            if (!String.IsNullOrEmpty(template))
            {
                if (!routeValues.ContainsKey("template"))
                    routeValues.Add("template", template);
                else
                {
                    routeValues.Remove("template");
                    routeValues.Add("template", template);
                }
            }
            return MvcHtmlString.Create(GenerateLink(ViewContext.RequestContext, RouteCollection, linkText, null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }
    }

    public class CustomUrlHelper : UrlHelper
    {
        public CustomUrlHelper(RequestContext requestContext) :
            base(requestContext)
        {
        }

        //Instance methods will always be called instead of extension methods when both exist with the same signature...

        public string Action(string actionName)
        {
            return Action(actionName, null, new RouteValueDictionary());
        }

        public string Action(string actionName, object routeValues)
        {
            return Action(actionName, null, new RouteValueDictionary(routeValues));
        }

        public string Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, new RouteValueDictionary());
        }

        public string Action(string actionName, RouteValueDictionary routeValues)
        {
            return Action(actionName, null, routeValues);
        }
        public string Action(string actionName, string controllerName, object routeValues)
        {
            return Action(actionName, controllerName, new RouteValueDictionary(routeValues));
        }
        public string Action(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            string template = "";
            if (HttpContext.Current.Request["template"] != null)
                template = HttpContext.Current.Request["template"];

            if (!String.IsNullOrEmpty(template))
            {
                if (!routeValues.ContainsKey("template"))
                    routeValues.Add("template", template);
                else
                {
                    routeValues.Remove("template");
                    routeValues.Add("template", template);
                }
            }
            //return Action(actionName, controllerName, routeValues);
            return UrlHelper.GenerateUrl("",actionName,controllerName,routeValues,RouteTable.Routes,RequestContext,false);
        }
    }

    public abstract class CustomWebViewPage<T> : WebViewPage<T>
    {
        public new CustomHtmlHelper<T> Html { get; set; }
        public new CustomUrlHelper Url { get; set; }

        public override void InitHelpers()
        {
            Ajax = new AjaxHelper<T>(ViewContext, this);
            Url = new CustomUrlHelper(ViewContext.RequestContext);

            //Load Custom Html Helper instead of Default
            Html = new CustomHtmlHelper<T>(ViewContext, this);
        }
    }
}