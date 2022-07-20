using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VCMS.MVC4.Web.Controllers
{
    public class InstallController : Controller
    {
        //
        // GET: /Install/

        public ActionResult Index()
        {
            return View();
        }

        public CaptchaImageResult ShowCaptchaImage()
        {
            return new CaptchaImageResult();
        }
    }
}
