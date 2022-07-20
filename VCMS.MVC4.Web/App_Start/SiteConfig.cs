using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VCMS.MVC4.Data;
using WebMatrix.WebData;

namespace VCMS.MVC4.Web
{
    public class SiteConfig
    {
        public static Website SiteInfo
        {
            get
            {
                if (HttpContext.Current.Items["SITE_INFO"] == null)
                {
                    //int siteID = SiteId;
                    if (HttpContext.Current.Items["SITE_INFO"] == null)
                    {
                        using (DataContext db = new DataContext())
                        {
                            HttpContext.Current.Items["SITE_INFO"] = db.Websites.Include(s => s.WebsiteDetail).Include(s => s.Settings.Select(sc => sc.WebsiteConfig)).Include(s => s.Languages).FirstOrDefault();
                        }
                    }
                }
                return HttpContext.Current.Items["SITE_INFO"] as Website;
            }
        }
        public static ICollection<Language> Languages
        {
            get { return SiteInfo.Languages; }
        }
        public static ICollection<ArticleType> ArticleTypes
        {
            get
            {
                if (HttpContext.Current.Items["ArticleTypes"] == null)
                {

                    HttpContext.Current.Items["ArticleTypes"] = ArticleType.GetBySite(SiteConfig.LanguageId);

                }
                return HttpContext.Current.Items["ArticleTypes"] as ICollection<ArticleType>;
            }

        }

        //public static int SiteId
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Application["SITE_ID"] == null)
        //        {
        //            using (DataContext db = new DataContext())
        //            {

        //                var domain = HttpContext.Current.Request.Url.Host;
        //                var site = db.Websites.Include(s => s.Settings.Select(sv => sv.WebsiteConfig)).Include("WebsiteDetail").Include("Languages").FirstOrDefault(s => s.DefaultDomain == domain);
        //                if (site != null)
        //                {
        //                    HttpContext.Current.Application["SITE_ID"] = site.Id;
        //                    HttpContext.Current.Items["SITE_INFO"] = site;
        //                }

        //                else
        //                    HttpContext.Current.Application["SITE_ID"] = 1;

        //            }
        //            HttpContext.Current.Application["SITE_ID"] = 1;
        //        }
        //        return Convert.ToInt32(HttpContext.Current.Application["SITE_ID"]);
        //    }
        //}
        public static int UserId
        {
            get
            {
                return WebSecurity.CurrentUserId;
            }
        }
        public static string SiteCode
        {
            get
            {
                if (HttpContext.Current.Request["template"] != null)
                {
                    HttpContext.Current.Cache.Remove("SiteCode");
                    HttpContext.Current.Cache.Add("SiteCode", HttpContext.Current.Request["template"], null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else if (HttpContext.Current.Cache["SiteCode"] == null)
                {
                    HttpContext.Current.Cache.Add("SiteCode", SiteInfo.Code, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                return HttpContext.Current.Cache["SiteCode"].ToString();
            }
        }
        public static int LanguageId
        {
            get
            {
                //if (HttpContext.Current.Request.Cookies["LANG_ID"] == null)
                //{
                //    var lang = 1;
                //    if (SiteInfo.DefaultLanguage > 0)
                //        lang = SiteInfo.DefaultLanguage;
                //    SetCulture(lang);
                //}
                //return Convert.ToInt32(HttpContext.Current.Request.Cookies["LANG_ID"].Value);
                return 1;
            }
            set
            {
                //HttpContext.Current.Response.Cookies["LANG_ID"].Value = value.ToString();
                //SetCulture(value);
            }
        }
        public static string LanguageCode
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["LANG_CODE"] == null)
                {
                    var lang = 1;
                    if (SiteInfo.DefaultLanguage > 0)
                        lang = SiteInfo.DefaultLanguage;
                    SetCulture(lang);


                }
                return HttpContext.Current.Request.Cookies["LANG_CODE"].Value;
            }
        }

        public static string LanguageLocale
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["LANG_LOCALE"] == null)
                {
                    var lang = 1;
                    if (SiteInfo.DefaultLanguage > 0)
                        lang = SiteInfo.DefaultLanguage;
                    SetCulture(lang);
                }
                return HttpContext.Current.Request.Cookies["LANG_LOCALE"].Value;
            }
        }

        private static void SetCulture(int langId)
        {
            using (DataContext db = new DataContext())
            {
                var lang = db.Languages.FirstOrDefault(l => l.Id == langId);
                if (lang != null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang.Code);
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang.Code);

                    HttpContext.Current.Response.Cookies["LANG_CODE"].Value = lang.Code;
                    HttpContext.Current.Response.Cookies["LANG_LOCALE"].Value = lang.Locale;
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("LANG_ID", langId.ToString()));
                }

            }
        }
        public static UserProfile CurrentUser
        {
            get
            {
                if (WebSecurity.IsAuthenticated)
                {
                    UserProfile currentUser = null;
                    if (HttpContext.Current.Session["CURRENT_USER"] != null)
                        currentUser = (UserProfile)HttpContext.Current.Session["CURRENT_USER"];
                    if (currentUser == null || currentUser.UserName != WebSecurity.CurrentUserName)
                    {
                        using (DataContext db = new DataContext())
                        {
                            currentUser = db.Users.FirstOrDefault(u => u.UserName == WebSecurity.CurrentUserName);
                            HttpContext.Current.Session["CURRENT_USER"] = currentUser;
                        }
                    }
                    return currentUser;
                }
                else return new UserProfile { DisplayName = "Guest" };
            }
        }

        public static string UploadFolder
        {
            get
            {
                if (HttpContext.Current.Application["upload:folder"] == null)
                {
                    var folder = ConfigurationManager.AppSettings["upload:folder"];
                    HttpContext.Current.Application["upload:folder"] = folder;
                }
                return HttpContext.Current.Application["upload:folder"].ToString();
            }
        }
        public static Cart ShoppingCartFile
        {
            get
            {
                Cart current = null;
                if (HttpContext.Current.Request.Cookies["Cart_Id"] == null)
                {

                    if (WebSecurity.IsAuthenticated)
                        current = Cart.GetByUserId(WebSecurity.CurrentUserId);
                    if (current == null)
                        current = Cart.CreateCartFile(Guid.NewGuid());

                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("Cart_id", current.Id.ToString()));
                }
                else
                {
                    current = Cart.GetByIdFile(int.Parse(HttpContext.Current.Request.Cookies["Cart_id"].Value));
                    if (current == null)
                    {
                        current = Cart.CreateCartFile(Guid.NewGuid());
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("Cart_id", current.Id.ToString()));
                    }
                }
                return current;
            }
        }
        public static Cart ShoppingCart
        {
            get
            {
                Cart current = null;
                if (HttpContext.Current.Request.Cookies["Cart_Id"] == null)
                {

                    if (WebSecurity.IsAuthenticated)
                        current = Cart.GetByUserId(WebSecurity.CurrentUserId);
                    if (current == null)
                        current = Cart.CreateCart(Guid.NewGuid());

                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("Cart_id", current.Id.ToString()));

                }
                else
                {
                    current = Cart.GetById(int.Parse(HttpContext.Current.Request.Cookies["Cart_id"].Value));
                    if (current == null)
                    {
                        current = Cart.CreateCart(Guid.NewGuid());
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("Cart_id", current.Id.ToString()));
                    }
                }
                return current;
            }
        }

        public static SortDirection DefaultSortDirection
        {
            get
            {
                var direction = SortDirection.DESCENDING;
                if (SiteInfo.Settings["SortDirection"] != null)
                {
                    Enum.TryParse<SortDirection>(SiteInfo.Settings["SortDirection"].Value, out direction);
                }
                return direction;
            }
        }
        public static string Image(string name)
        {
            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return Url.Content("~/Templates/" + SiteConfig.SiteInfo.Code + "/Content/Images/" + name);
        }
        public static Currency Currency
        {
            get
            {
                using (DataContext db = new DataContext())
                {
                    var cur = db.Currencies.FirstOrDefault(cu => cu.IsDefault);
                    if (cur != null)
                        return cur;
                    else
                        return new Currency();
                }
            }
        }
        public static List<string> Templates
        {
            get
            {
                if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Templates")))
                {
                    List<string> templates = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/Templates")).ToList<string>();
                    List<string> dir_templates = new List<string>();
                    if (templates.Count > 0)
                    {
                        templates.ForEach(t =>
                        {
                            dir_templates.Add(new DirectoryInfo(Path.GetFileName(t)).Name.ToString());
                        });
                        return dir_templates;
                    }

                }
                return new List<string>();
            }
        }

        public static List<DirectoryInfo> WidgetView
        {
            get
            {
                if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Templates/" + SiteConfig.SiteInfo.Code + "/Views/Widget")))
                {
                    List<string> views = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Templates/" + SiteConfig.SiteInfo.Code + "/Views/Widget")).ToList<string>();
                    List<DirectoryInfo> dir_views = new List<DirectoryInfo>();
                    if (views.Count > 0)
                    {
                        views.ForEach(t =>
                        {
                            var viewname = Path.GetFileName(t);
                            if (!viewname.Contains("Tablet") && !viewname.Contains("Mobile"))
                                dir_views.Add(new DirectoryInfo(viewname.Split('.').FirstOrDefault()));
                        });
                        return dir_views;
                    }
                }
                return new List<DirectoryInfo>();
            }
        }

        public static string Watermark
        {
            get
            {
                string wt = "";
                if (SiteConfig.SiteInfo.HasWatermark)
                {
                    if (SiteConfig.SiteInfo.HasWatermarkImg)
                        wt = string.Format("Logo{0},Text{0},storyBG&name={1}", SiteConfig.SiteInfo.WatermarkPosition.ToString(), SiteConfig.SiteInfo.Watermark);
                    else wt = string.Format("Text{0},storyBG&name={1}", SiteConfig.SiteInfo.WatermarkPosition.ToString(), SiteConfig.SiteInfo.Watermark);
                    return wt;
                }
                return wt;
            }
        }
        public static string WatermarkFill
        {
            get
            {
                string wt = "";
                if (SiteConfig.SiteInfo.HasWatermark)
                {
                    wt = string.Format("Text{0}&name={1}", WatermarkPostion.CenterFill.ToString(), SiteConfig.SiteInfo.Watermark);
                    return wt;
                }
                return wt;
            }
        }
        public static List<string> Skins
        {
            get
            {
                if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Templates/" + SiteConfig.SiteInfo.Code + "/Content")))
                {
                    List<string> views = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Templates/" + SiteConfig.SiteInfo.Code + "/Content")).ToList<string>();
                    List<string> dir_views = new List<string>();
                    if (views.Count > 0)
                    {
                        views.ForEach(t =>
                        {
                            dir_views.Add(new DirectoryInfo(Path.GetFileName(t)).Name.ToString().Replace(".less", ""));
                        });
                        return dir_views;
                    }
                }
                return new List<string>();
            }
        }
        public static string ThemesSkin
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SiteConfig.SiteInfo.Skin) && !SiteConfig.Skins.ToString().Equals("skin-1", StringComparison.OrdinalIgnoreCase))
                    return string.Format("{0}", SiteConfig.SiteInfo.Skin.Replace("skin", ""));
                return "";
            }
        }
        public static string Mode
        {
            get
            {
                App_Start.DisplayMode dmode = new App_Start.DisplayMode();
                if (dmode.GetDeviceType(HttpContext.Current.Request.UserAgent) == "tablet")
                    return ".Tablet";
                else if (dmode.GetDeviceType(HttpContext.Current.Request.UserAgent) == "mobile")
                    return ".Mobile";
                return "";
            }
        }
    }
}