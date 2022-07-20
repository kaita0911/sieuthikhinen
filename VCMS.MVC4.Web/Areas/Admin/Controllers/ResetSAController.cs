using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class ResetSAController : Controller
    {
        // GET: Admin/ResetSA
        [AllowAnonymous]
        public ActionResult Index()
        {
            var token = WebSecurity.GeneratePasswordResetToken("sa");
            return View((object)token);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            WebSecurity.ResetPassword(form["token"], form["newPass"]);
            return View();
        }
    }
}