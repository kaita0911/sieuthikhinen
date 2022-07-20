using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using System.Web.WebPages;
using VCMS.MVC4.Web.App_Start;


namespace VCMS.MVC4.Web.Controllers
{
    [HandleError]

    public class WidgetController :Controller
    {
        public ActionResult Widget(string name)
        {

            using (DataContext db = new DataContext())
            {
                var widgets = PageContext.Widgets;
                Widget widget = null;
                if (widgets != null && widgets.Any(w => w.Code.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    widget = widgets.FirstOrDefault(w => w.Code.Equals(name, StringComparison.OrdinalIgnoreCase));
                else
                 widget = db.Widget.Include(a => a.WidgetDetail).Include(w=>w.ArticleType).FirstOrDefault(a => a.Code.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (widget == null)
                    return Content(string.Format("Widget {0} not found!", name));
                else
                {
                    widget.LanguageId = SiteConfig.LanguageId;
                    if (widget.WidgetType == WidgetType.HTML)
                        return Content(widget.Value);
                    if (widget.WidgetType == WidgetType.WEBPLUGIN)
                        return Content((!string.IsNullOrWhiteSpace(widget.WidgetDetail[1].Value) ? widget.WidgetDetail[1].Value : ""));
                }
                if (!string.IsNullOrWhiteSpace(widget.WidgetView))
                {
                    var mode = SiteConfig.Mode;
                    var view = "";
                    //var type = db.ArticleTypes.FirstOrDefault(a => a.Id == widget.ArticleTypeId);
                    var type = widget.ArticleType;
                    if (type != null)
                        view = type.Code;
                    var widgetview = widget.WidgetView.Trim().ToString();
                    if (!string.IsNullOrWhiteSpace(mode))
                    {
                        ViewEngineResult resultview = ViewEngines.Engines.FindView(ControllerContext, view + "/" + widget.WidgetView.ToString() + mode, null);
                        widgetview = resultview.View != null ? widgetview += mode : widgetview;
                    }
                    
                    ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, view + "/" + widgetview, null);
                    if (result.View != null)
                        return PartialView(view + "/" + widgetview, widget);

                    result = ViewEngines.Engines.FindView(ControllerContext, widget.WidgetView.ToString(), null);
                    if (result.View != null)
                        return PartialView(widget.WidgetView.ToString(), widget);
                }
                else
                    return Content(string.Format("Widget {0} not found views!", name));
            }
            return Content(string.Format("Widget {0} not found!", name));
        }
        public ActionResult WidgetGroup(string name)
        {
            using (DataContext db = new DataContext())
            {
                //var group = db.WidgetGroup.FirstOrDefault(a => a.Code.Equals(name, StringComparison.OrdinalIgnoreCase));
                //if (group == null)
                //    return Content(string.Format("Widget Group {0} not found!", name));

                var widgets = db.Widget.Include(a => a.WidgetDetail).Include(w=>w.ArticleType)
                    .Where(a => a.WidgetGroup.Code.Equals(name,StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.WidgetsortOrder).ToList();
                
                PageContext.AddWidgets ( widgets);

                ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, "GroupWidget", null);
                if (result.View != null)
                    return PartialView("GroupWidget", widgets);
                else
                    return Content(string.Format("Widget Group {0} not found!", name));

            }
        }
    }
}