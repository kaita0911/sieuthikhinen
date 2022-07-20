using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace VCMS.MVC4.Web
{
    public class ExtendedRazorViewEngine : RazorViewEngine
    {
        public ExtendedRazorViewEngine()
            : this(true)
        { }
        public ExtendedRazorViewEngine(string viewPath)
            : this(true)
        {
            this.nameSpace = viewPath;
        }
        private string nameSpace = string.Empty;
        public ExtendedRazorViewEngine(bool appendViewPath = false)
            : base()
        {
            
            //AreaViewLocationFormats = new[] {
            //    "~/Areas/{2}/Templates/%1/{1}/{0}.cshtml",
            //    "~/Areas/{2}/Templates/%1/{1}/{0}.vbhtml",
            //    "~/Areas/{2}/Templates/%1/Shared/{0}.cshtml",
            //    "~/Areas/{2}/Templates/%1/Shared/{0}.vbhtml"
            //};

            //AreaMasterLocationFormats = new[] {
            //    "~/Areas/{2}/Templates/%1/{1}/{0}.cshtml",
            //    "~/Areas/{2}/Templates/%1/{1}/{0}.vbhtml",
            //    "~/Areas/{2}/Templates/%1/Shared/{0}.cshtml",
            //    "~/Areas/{2}/Templates/%1/Shared/{0}.vbhtml"
            //};

            //AreaPartialViewLocationFormats = new[] {
            //    "~/Areas/{2}/Templates/%1/{1}/{0}.cshtml",
            //    "~/Areas/{2}/Templates/%1/{1}/{0}.vbhtml",
            //    "~/Areas/{2}/Templates/%1/Shared/{0}.cshtml",
            //    "~/Areas/{2}/Templates/%1/Shared/{0}.vbhtml"
            //};
            var oldViewLocationFormats = ViewLocationFormats;
            ViewLocationFormats = new[] {
                "~/Templates/%1/Views/{1}/{0}.cshtml",
                "~/Templates/%1/Views/Shared/{0}.cshtml"
            };
            var oldMasterLocationFormats = MasterLocationFormats;
            MasterLocationFormats = new[] {
                "~/Templates/%1/Views/{1}/{0}.cshtml",
                "~/Templates/%1/Views/Shared/{0}.cshtml",
                "~/Templates/%1/Shared/{0}.cshtml"
            };
            var oldPartialViewLocationFormats = PartialViewLocationFormats;
            PartialViewLocationFormats = new[] {
                "~/Templates/%1/Views/{1}/{0}.cshtml",
                "~/Templates/%1/Views/Shared/{0}.cshtml",
                "~/Templates/%1/Views/{0}.cshtml"
            };
            if (appendViewPath)
            {
                PartialViewLocationFormats = PartialViewLocationFormats.Concat(oldPartialViewLocationFormats).ToArray();
                ViewLocationFormats = ViewLocationFormats.Concat(oldViewLocationFormats).ToArray();
                MasterLocationFormats = MasterLocationFormats.Concat(oldMasterLocationFormats).ToArray();
            }
        }

        //public void AddViewLocationFormat(string paths)
        //{
        //    List<string> existingPaths = new List<string>(ViewLocationFormats);
        //    existingPaths.Insert(0, paths);

        //    ViewLocationFormats = existingPaths.ToArray();
        //}

        //public void AddPartialViewLocationFormat(string paths)
        //{
        //    List<string> existingPaths = new List<string>(PartialViewLocationFormats);
        //    existingPaths.Insert(0,paths);

        //    PartialViewLocationFormats = existingPaths.ToArray();
        //}

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            //nameSpace = SiteConfig.SiteCode;
            return base.CreatePartialView(controllerContext, partialPath.Replace("%1", nameSpace));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            //nameSpace = SiteConfig.SiteCode; 
            return base.CreateView(controllerContext, viewPath.Replace("%1", nameSpace), masterPath.Replace("%1", nameSpace));
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            //nameSpace = SiteConfig.SiteCode; 
            return base.FileExists(controllerContext, virtualPath.Replace("%1", nameSpace));
        }
       
        public static void RegisterViewEngine()
        {
            RegisterViewEngine("Default");
        }
        public static void RegisterViewEngine(string templateName)
        {
            if (!registered || registeredTemplate != templateName)
            {
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new ExtendedRazorViewEngine(templateName));
                registeredTemplate = templateName;
                registered = true;
            }
        }
        private static string registeredTemplate = string.Empty;
        private static bool registered = false;

    }
}