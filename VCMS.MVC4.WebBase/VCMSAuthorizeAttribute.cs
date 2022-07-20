using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace VCMS.MVC4.Web
{
    public class VCMSAuthorizeAttribute:AuthorizeAttribute
    {
        public string LoginUrl { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var loginPage = LoginUrl;
            loginPage += "?ReturnUrl=" + filterContext.HttpContext.Request.RawUrl;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect(loginPage);
            }
            else
            {
                if (Roles != null)
                {
                    var roles = Roles.Split(new char[] { ',' });
                    var isInRole = (from role in roles
                                    where filterContext.HttpContext.User.IsInRole(role)
                                        select true).FirstOrDefault();
                    if (!isInRole)
                        filterContext.HttpContext.Response.Redirect(loginPage);
                }
            }
            
            base.OnAuthorization(filterContext);
        }
        
    }
    
    public class AdminAuthorizeAttribute : VCMSAuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.LoginUrl = "~/Admin/Account/Login";
            base.OnAuthorization(filterContext);
            
        }
    }
}