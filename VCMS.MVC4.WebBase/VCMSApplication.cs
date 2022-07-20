using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace VCMS.MVC4.Web
{
    public class VCMSApplication : System.Web.HttpApplication
    {
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers["X-Powered-By"] = "VCMS 1.0";
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers["Server"] = "VCMS Server";
            HttpContext.Current.Response.Headers["Author"] = "VienNam Solutions";
            
            
        }

       
    }
}
