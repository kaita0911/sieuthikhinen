using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VCMS.MVC4.Web
{
    public class Program
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "Sitemap",
                url: "sitemap.xml",
                defaults: new { controller = "Home", action = "SiteMap" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
                );

            routes.MapRoute(
                name: "SiteMapInsite",
                url: "sitemap.html",
                defaults: new { controller = "Home", action = "SiteMapInsite" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
                );

            routes.MapRoute(
                name: "ArticleDetail",
                url: "{code}/{title}-ad{id}.html",
                defaults: new { controller = "Article", action = "Detail" },
                constraints: new { id = @"\d+" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }

            );

            routes.MapRoute(
                name: "ReadArticle",
                url: "{code}/{title}-read{id}-unit{uid}.html",
                defaults: new { controller = "Article", action = "Read" },
                constraints: new { id = @"\d+", uid = @"\d+" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }

            );

            routes.MapRoute(
               name: "CategoryDetail",
               url: "{code}/{title}-ac{id}.html",
               defaults: new { controller = "Category", action = "Detail" },
               constraints: new { id = @"\d+" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
           );

            routes.MapRoute(
              name: "ListCategoryType",
              url: "ct-{code}.html",
              defaults: new { controller = "Category", action = "ListCateType", code = UrlParameter.Optional },
              namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }

          );

           // routes.MapRoute(
           //    name: "Brand",
           //    url: "thuong-hieu.html",
           //    defaults: new { controller = "Category", action = "Brand" },
           //    namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }

           //);
            routes.MapRoute(
                name: "Contact",
                url: "contact.html",
                defaults: new { controller = "Home", action = "Contact" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
            );
            routes.MapRoute(
               name: "lienhe",
               url: "lien-he.html",
               defaults: new { controller = "Home", action = "lienhe" },
               namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
           );
            routes.MapRoute(
               name: "giohang",
               url: "gio-hang.html",
               defaults: new { controller = "ShoppingCart", action = "Index" },
               namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
           );
            routes.MapRoute(
               name: "dathang",
               url: "dat-hang.html",
               defaults: new { controller = "ShoppingCart", action = "CheckOut" },
               namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
           );
            routes.MapRoute(
              name: "muanhanh",
              url: "mua-nhanh.html",
              defaults: new { controller = "ShoppingCart", action = "IndexAjax" },
              namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
          );
            routes.MapRoute(
             name: "thanhtoan",
             url: "thanh-toan.html",
             defaults: new { controller = "ShoppingCart", action = "_CartInfo" },
             namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
         );
            routes.MapRoute(
          name: "hoanthanh",
          url: "hoan-thanh.html",
          defaults: new { controller = "ShoppingCart", action = "Complete" },
          namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
      );

            routes.MapRoute(
               name: "ContactForm",
               url: "ky-gui.html",
               defaults: new { controller = "Home", action = "ContactForm" },
               namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
           );

            routes.LowercaseUrls = true;
            routes.MapRoute(
                name: "ArticleType",
                url: "{code}.html",
                defaults: new { controller = "ArticleType", action = "Index", code = UrlParameter.Optional },

                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }

            );

            routes.MapRoute(
              name: "CategoryTypeDetail",
              url: "{code}/{title}-act{id}.html",
              defaults: new { controller = "Category", action = "DetailType" },
              constraints: new { id = @"\d+" },
               namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Search",
                url: "search/keyword={keyword}",
                defaults: new { controller = "Home", action = "Search" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
            );
            routes.MapRoute(
                name: "Tag",
                url: "tags/{keyword}",
                defaults: new { controller = "Home", action = "Tag" },
                constraints: new { keyword = @".+"  },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = @"[a-zA-Z]+", action = @"[a-zA-Z]+" },
                namespaces: new string[] { "VCMS.MVC4.Web.Controllers" }
            );


        }
    }
}