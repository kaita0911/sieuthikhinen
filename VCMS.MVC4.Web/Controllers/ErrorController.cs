using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VCMS.MVC4.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HttpError404()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
