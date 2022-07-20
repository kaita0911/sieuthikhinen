using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VCMS.MVC4.Web.Controllers
{
    public class AjaxController : Controller
    {
        
        // GET: Ajax
        public ActionResult Partial(string viewName)
        {
            return PartialView(viewName);
        }
    }
}