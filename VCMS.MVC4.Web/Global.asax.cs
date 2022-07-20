using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VCMS.MVC4.Data;
using System.Data.Entity;
using VCMS.MVC4.Initializer;
using WebMatrix.WebData;
using System.Collections.Specialized;
using System.Configuration;
using VCMS.MVC4.Web.Controllers;
using System.Web.WebPages;

namespace VCMS.MVC4.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : VCMS.MVC4.Web.VCMSApplication
    {
        protected void Application_Start()
        {
            if (ConfigurationManager.AppSettings["database:init"] == "0")
                Database.SetInitializer<DataContext>(new VCMS.MVC4.Initializer.VCMSDataInitializer());
            else
                Database.SetInitializer<DataContext>(null);

            using (DataContext db = new DataContext())
            {
                db.Database.Initialize(true);
            }
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("VCMS.DataConnection", "UserProfile", "UserId", "UserName", false);

            AreaRegistration.RegisterAllAreas();
            ImageHandler.DefaultImage();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Program.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            EvaluateDisplayMode();
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
           
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string template = "";
            if (HttpContext.Current.Request["template"] != null)
                template = HttpContext.Current.Request["template"];
            else
                template = SiteConfig.SiteCode;
            ExtendedRazorViewEngine.RegisterViewEngine(template);
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;

            if (Context.IsCustomErrorEnabled)
                ShowCustomErrorPage(exception);
        }
        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 403:
                    routeData.Values.Add("action", "HttpError403");
                    break;

                case 404:
                    routeData.Values.Add("action", "HttpError404");
                    break;

                case 500:
                    routeData.Values.Add("action", "HttpError500");
                    break;

                default:
                    routeData.Values.Add("action", "GeneralError");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    break;
            }

            Server.ClearError();
            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
        private void EvaluateDisplayMode()
        {
            VCMS.MVC4.Web.App_Start.DisplayMode mode = new VCMS.MVC4.Web.App_Start.DisplayMode();
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Tablet")
            {
                ContextCondition = (context => mode.GetDeviceType(context.GetOverriddenUserAgent()).Equals("tablet", StringComparison.OrdinalIgnoreCase))
            });

            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Tv")
            {
                ContextCondition = (context =>mode.GetDeviceType(context.GetOverriddenUserAgent()).Equals("tv", StringComparison.OrdinalIgnoreCase))
            });
            //DisplayModeProvider.Instance.Modes.Insert(2, new DefaultDisplayMode("Mobile")
            //{
            //    ContextCondition = (context => mode.GetDeviceType(context.GetOverriddenUserAgent()).Equals("mobile", StringComparison.OrdinalIgnoreCase))
            //});

            DisplayModeProvider.Instance.Modes.Insert(2, new DefaultDisplayMode("Mobile")
            {
                ContextCondition = (context => context.Request.Browser.IsMobileDevice)
            });
        }
    }
}