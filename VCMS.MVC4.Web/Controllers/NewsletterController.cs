using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VCMS.MVC4.Data;
using WebMatrix.WebData;

namespace VCMS.MVC4.Web.Controllers
{
    public class NewsletterController : Controller
    {
        //
        // GET: /Newsletter/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Subscribe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Subscribe(SubscribeModel model)
        {
            var ROLE_SUBSCRIBERS  ="Subscribers";
            using (DataContext db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
                if (model.Unsubscribe)
                {
                    if (user != null && Roles.IsUserInRole(user.UserName, ROLE_SUBSCRIBERS))
                        Roles.RemoveUserFromRole(user.UserName, ROLE_SUBSCRIBERS);
                }
                else
                {

                    if (!Roles.RoleExists(ROLE_SUBSCRIBERS))
                        Roles.CreateRole(ROLE_SUBSCRIBERS);

                    if (user == null)
                    {
                        WebSecurity.CreateUserAndAccount(model.Email, Membership.GeneratePassword(8, 0), new { DateCreated = DateTime.Now, DisplayName = model.Email, Email = model.Email });
                        Roles.AddUserToRole(model.Email, ROLE_SUBSCRIBERS);
                    }
                    else if (!Roles.IsUserInRole(user.UserName, ROLE_SUBSCRIBERS))

                        Roles.AddUserToRole(user.UserName, "Subscribers");

                }
            }

            if (Request.IsAjaxRequest())
                return Json(new { status = 0, unsub = model.Unsubscribe });
            return View();
        }
       
    }
}
