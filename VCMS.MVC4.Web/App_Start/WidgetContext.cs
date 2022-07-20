using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Web.Mvc.Html;
using System.Data.Entity;
namespace VCMS.MVC4.Web
{
    public static class WidgetContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="ajax"></param>
        /// <param name="callback">The js function/statement called after ajax loaded</param>
        /// <returns></returns>
        public static MvcHtmlString Widget(this HtmlHelper htmlHelper, string name, bool ajax = false, string callback = null)
        {        
            var content = string.Empty;
            if (!ajax)
            {
                Widget widget = PageContext.GetWidget(name);

                if (widget == null)
                    content = string.Format("Widget {0} not found!", name);
                else
                {
                    widget.LanguageId = SiteConfig.LanguageId;
                    if (widget.WidgetType == WidgetType.HTML)
                        content = widget.Value;
                    else if (widget.WidgetType == WidgetType.WEBPLUGIN)
                        content = (!string.IsNullOrWhiteSpace(widget.WidgetDetail[1].Value) ? widget.WidgetDetail[1].Value : "");
                    else if (!string.IsNullOrWhiteSpace(widget.WidgetView))
                    {
                        var mode = SiteConfig.Mode;
                        var view = "";
                        var type = widget.ArticleType;
                        if (type != null)
                            view = type.Code;

                        var widgetview = widget.WidgetView.Trim().ToString();
                        var controllerContext = htmlHelper.ViewContext.Controller.ControllerContext;
                        ViewEngineResult result = ViewEngines.Engines.FindPartialView(controllerContext, "Widget/" + view + "/" + widgetview);
                        if (result.View != null)
                            return htmlHelper.Partial("Widget/" + view + "/" + widgetview, widget);

                        result = ViewEngines.Engines.FindPartialView(controllerContext, "Widget/" + widgetview);
                        if (result.View != null)
                            return htmlHelper.Partial("Widget/" + widgetview, widget);
                        else content = string.Format("Widget '{0}' View '{1}' not found", name, widgetview);
                    }

                    else
                        content = string.Format("Widget {0} not found views!", name);
                }
            }
            else
            {
                var url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action("Widget", "Widget", new { name = name });
                content = string.Format("<div id='widget-{0}' data-widget='{1}' data-callback='{2}'></div>", name, url, callback);
            }
            return new MvcHtmlString(content);

            //return MvcHtmlString.Create(htmlHelper.Action("Widget", "Widget", new { name = name }).ToString());
        }

        [OutputCache(Duration = 60)]
        public static MvcHtmlString WidgetCache(this HtmlHelper htmlHelper, string name, bool ajax = false, string callback = null)
        {
            var content = string.Empty;
            if (!ajax)
            {
                Widget widget = PageContext.GetWidget(name);

                if (widget == null)
                    content = string.Format("Widget {0} not found!", name);
                else
                {
                    widget.LanguageId = SiteConfig.LanguageId;
                    if (widget.WidgetType == WidgetType.HTML)
                        content = widget.Value;
                    else if (widget.WidgetType == WidgetType.WEBPLUGIN)
                        content = (!string.IsNullOrWhiteSpace(widget.WidgetDetail[1].Value) ? widget.WidgetDetail[1].Value : "");
                    else if (!string.IsNullOrWhiteSpace(widget.WidgetView))
                    {
                        var mode = SiteConfig.Mode;
                        var view = "";
                        var type = widget.ArticleType;
                        if (type != null)
                            view = type.Code;

                        var widgetview = widget.WidgetView.Trim().ToString();
                        var controllerContext = htmlHelper.ViewContext.Controller.ControllerContext;
                        ViewEngineResult result = ViewEngines.Engines.FindPartialView(controllerContext, "Widget/" + view + "/" + widgetview);
                        if (result.View != null)
                            return htmlHelper.Partial("Widget/" + view + "/" + widgetview, widget);

                        result = ViewEngines.Engines.FindPartialView(controllerContext, "Widget/" + widgetview);
                        if (result.View != null)
                            return htmlHelper.Partial("Widget/" + widgetview, widget);
                        else content = string.Format("Widget '{0}' View '{1}' not found", name, widgetview);
                    }

                    else
                        content = string.Format("Widget {0} not found views!", name);
                }
            }
            else
            {
                var url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action("Widget", "Widget", new { name = name });
                content = string.Format("<div id='widget-{0}' data-widget='{1}' data-callback='{2}'></div>", name, url, callback);
            }
            return new MvcHtmlString(content);

            //return MvcHtmlString.Create(htmlHelper.Action("Widget", "Widget", new { name = name }).ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="ajax"></param>
        /// <param name="callback">The js function/statement called after ajax loaded</param>
        /// <returns></returns>
        public static MvcHtmlString WidgetGroup(this HtmlHelper htmlHelper, string name, bool ajax=false,string callback=null)
        {
            //return MvcHtmlString.Create(htmlHelper.Action("WidgetGroup", "Widget", new { name = name }).ToString());
            if (!ajax)
            {
                using (DataContext db = new DataContext())
                {
                    var widgets = db.Widget.Include(a => a.WidgetDetail).Include(w => w.ArticleType)
                            .Where(a => a.WidgetGroup.Code.Equals(name, StringComparison.OrdinalIgnoreCase))
                            .OrderBy(a => a.WidgetsortOrder).ToList();

                    PageContext.AddWidgets(widgets);

                    //ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, "GroupWidget", null);
                    //if (result.View != null)
                    //    return PartialView("GroupWidget", widgets);
                    //else
                    //    return Content(string.Format("Widget Group {0} not found!", name));

                    var content = new MvcHtmlString(string.Empty);
                    foreach (var item in widgets)
                    {
                        content = MvcHtmlString.Create(content.ToString() + htmlHelper.Widget(item.Code, ajax).ToString());
                    }
                    return content;
                }
            }
            else
            {
                var url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action("WidgetGroup", "Widget", new { name = name });

                return MvcHtmlString.Create(string.Format("<div id='widgetgroup-{0}' data-widget-group='{1}' data-callback='{2}'></div>", name, url,callback));
            }
        }


        public static MvcHtmlString Setting(this HtmlHelper helper, string name)
        {
            var widget = PageContext.GetSetting(name);
            if (widget != null)
                return MvcHtmlString.Create(widget.Value);
            else return MvcHtmlString.Create("<!-- setting_" + name + " -->");
        }
    }
}