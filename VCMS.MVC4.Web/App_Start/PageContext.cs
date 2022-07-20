using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VCMS.MVC4.Data;
using System.Data.Entity;
using System.Web.Caching;
namespace VCMS.MVC4.Web
{
    public class PageContext
    {
        public static ICollection<Widget> Settings
        {
            get
            {
                if (HttpContext.Current.Cache["SETTINGS"] == null)
                {
                    using (DataContext db = new DataContext())
                    {
                        var settings = db.Widget.Include(c => c.WidgetDetail).Where(w => w.WidgetType == WidgetType.WEBPLUGIN).ToList();
                        HttpContext.Current.Cache.Add("SETTINGS", settings, null, DateTime.Now.AddMinutes(0), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                        return settings;
                    }
                }
                return HttpContext.Current.Cache["SETTINGS"] as ICollection<Widget>;

            }

        }

        internal static Widget GetSetting(string name)
        {
            try
            {
                Widget widget = null;
                if (PageContext.Settings.Any(w => w.Code.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    widget = PageContext.Settings.FirstOrDefault(w => w.Code.Equals(name, StringComparison.OrdinalIgnoreCase));

                return widget;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ICollection<Widget> Widgets
        {
            get
            {
                if (HttpContext.Current.Items["WIDGETS"] == null)
                    HttpContext.Current.Items["WIDGETS"] = new List<Widget>();
                return HttpContext.Current.Items["WIDGETS"] as ICollection<Widget>;

            }
            set
            {
                HttpContext.Current.Items["WIDGETS"] = value;
            }
        }

        public static ICollection<ArticleType> ArticleTypes
        {
            get
            {
                return HttpContext.Current.Items["TYPES"] as ICollection<ArticleType>;
            }
            set
            {
                HttpContext.Current.Items["TYPES"] = value;
            }
        }
        public static ArticleType GetArticleType(int id, int languageId)
        {
            if (PageContext.ArticleTypes == null)
                PageContext.ArticleTypes = new List<ArticleType>();
            if (PageContext.ArticleTypes.Any(t => t.Id == id))
                return PageContext.ArticleTypes.FirstOrDefault(t => t.Id == id);
            else
            {

                var type = ArticleType.GetById(id, languageId);
                if (type != null)
                    PageContext.ArticleTypes.Add(type);
                return type;

            }
        }

        internal static Widget GetWidget(string name)
        {
            using (DataContext db = new DataContext())
            {
                Widget widget = null;
                //if (PageContext.Widgets.Any(w => w.Code.Equals(name, StringComparison.OrdinalIgnoreCase)))
                //    widget = PageContext.Widgets.FirstOrDefault(w => w.Code.Equals(name, StringComparison.OrdinalIgnoreCase));
                //else
                //{
                widget = db.Widget.Include(a => a.WidgetDetail).Include(w => w.ArticleType).FirstOrDefault(a => a.Code.Equals(name, StringComparison.OrdinalIgnoreCase));
                PageContext.Widgets.Add(widget);
                //}
                return widget;
            }
        }

        internal static void AddWidgets(IEnumerable<Widget> widgets)
        {
            (PageContext.Widgets as List<Widget>).AddRange(widgets);
        }


    }
}