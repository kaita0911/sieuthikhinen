using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VCMS.MVC4.Web.Controllers
{
    public class RoleController : Controller
    {
        //
        // GET: /Role/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            if (form["txtRoleName"] != null)
            {
                Roles.CreateRole(form["txtRoleName"]);
            }

            return RedirectToAction("Index");
        }
    }
}
