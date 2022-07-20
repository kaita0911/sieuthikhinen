using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VCMS.MVC4.Data;
using WebMatrix.WebData;
using EntityFramework.Extensions;


namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Super Administrators, Administrators")]
    public class UserController : VCMSAdminController
    {
        //
        // GET: /User/
        public ActionResult Index(string role = "", int pageIndex = 1, int pageSize = 20)
        {
            using (DataContext db = new DataContext())
            {
                var query = db.Users.Where(u => u.UserName != "sa");
                if (!string.IsNullOrWhiteSpace(role))
                {
                    var names = Roles.GetUsersInRole(role);
                    query = query.Where(u => names.Contains(u.UserName));
                }
                ViewBag.ItemCount = query.Count();
                var users = query.OrderBy(a => a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return View(users);
            }
        }

        public ActionResult Create(string role, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserProfile model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var err = 0;
                var db = new DataContext();
                var user = db.Users.Where(a => a.UserName == model.UserName.Trim()).SingleOrDefault();
               
                if (user != null)
                {
                    ViewBag.Error = "Tài khoản đã tồn tại";
                    err++;
                }
                var user2 = db.Users.Where(a => a.Email == model.Email.Trim()).FirstOrDefault();
                if (user2 != null)
                {
                    ViewBag.ErrorEmail = "Email đã tồn tại!";
                    err++;
                }
                if (model.Password.Length < 6)
                {
                    ViewBag.ErrorPassword = "Mật khẩu phải lớn hơn 6 ký tự";
                    err++;
                }
                if (err > 0)
                {
                    return View();
                }
                WebSecurity.CreateUserAndAccount(model.UserName.Trim(), model.Password, new { DateCreated = DateTime.Now, DisplayName = model.DisplayName, Email = model.Email.Trim(), Phone = model.Phone, Fax = model.Fax, Address = model.Address, company = model.Company, City = model.City, State = model.State, Newsletter = model.Newsletter, Accumulated = 0, flags = UserFlags.NONE });
                if (Request["chkRole"] != null)
                {
                    string[] roles = Request["chkRole"].Split(',').Where(s => s != "false").ToArray();
                    if (roles.Count() > 0)
                    {
                        Roles.AddUserToRoles(model.UserName, roles);
                    }
                }
                var user3 = db.Users.Where(a => a.UserName == model.UserName.Trim()).SingleOrDefault();
                if (user3 != null)
                {
                    if (Request["chkArticleType"] != null)
                    {
                        var modelu = new UserArticleTypeCate();
                        string[] articleType = Request["chkArticleType"].Split(',').Where(s => s != "false").ToArray();
                        for (int i = 0; i < articleType.Count(); i++)
                        {
                            modelu.UserId = user3.UserId;
                            modelu.ArticleTypeId = Convert.ToInt32(articleType[i]);
                            modelu.CategoryId = 0;
                            db.UserArticleTypeCates.Add(modelu);
                            db.SaveChanges();
                        }
                    }
                    if (Request["chkCategory"] != null)
                    {
                        var uac = new UserArticleTypeCate();
                        string[] cate = Request["chkCategory"].Split(',').Where(s => s != "false").ToArray();
                        for (int i = 0; i < cate.Count(); i++)
                        {
                            uac.UserId = user3.UserId;
                            uac.ArticleTypeId = 0;
                            uac.CategoryId = Convert.ToInt32(cate[i]);
                            db.UserArticleTypeCates.Add(uac);
                            db.SaveChanges();
                        }
                    }
                }
            }
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string role, string returnUrl)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Users.Where(u => u.UserId == id).SingleOrDefault();
                string token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                //string token=WebSecurity.GetLastPasswordFailureDate()
                model.Password = token;
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(UserProfile model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                using (DataContext db = new DataContext())
                {
                    var user = db.Users.Where(a => a.UserId == model.UserId).SingleOrDefault();
                    if (user != null)
                    {
                        user.DisplayName = model.DisplayName;
                        user.Phone = model.Phone;
                        user.Fax = model.Fax;
                        user.Address = model.Address;
                        user.Company = model.Company;
                        user.Newsletter = model.Newsletter;
                        if (model.Email.Trim() != user.Email)
                        {
                            var check = db.Users.Where(a => a.Email == model.Email).FirstOrDefault();
                            if (check != null)
                            {
                                ViewBag.ErrorMail = "Email đã tồn tại";
                                user.Password = token;
                                return View(user);
                            }
                            user.Email = model.Email;
                        }
                        db.SaveChanges();
                        if (!string.Equals(token, model.Password, StringComparison.OrdinalIgnoreCase))
                        {
                            WebSecurity.ResetPassword(token, model.Password);
                        }
                        if (Request["chkRole"] != null)
                        {
                            string[] roles = Request["chkRole"].Split(',').Where(s => s != "false").ToArray();
                            if (roles.Count() > 0)
                            {
                                string[] list = Roles.GetRolesForUser(model.UserName);
                                if (list.Count() > 0)
                                {
                                    Roles.RemoveUserFromRoles(model.UserName, list);
                                }
                                Roles.AddUserToRoles(model.UserName, roles);
                            }
                        }
                        if (Request["chkArticleType"] != null)
                        {
                            db.UserArticleTypeCates.Delete(u => u.UserId == model.UserId);
                            var modelu = new UserArticleTypeCate();
                            string[] articleType = Request["chkArticleType"].Split(',').Where(s => s != "false").ToArray();
                            for (int i = 0; i < articleType.Count(); i++)
                            {
                                modelu.UserId = model.UserId;
                                modelu.ArticleTypeId = Convert.ToInt32(articleType[i]);
                                modelu.CategoryId = 0;
                                db.UserArticleTypeCates.Add(modelu);
                                db.SaveChanges();
                            }
                        }
                        if (Request["chkCategory"] != null)
                        {
                            var uac = new UserArticleTypeCate();
                            string[] cate = Request["chkCategory"].Split(',').Where(s => s != "false").ToArray();
                            for (int i = 0; i < cate.Count(); i++)
                            {
                                uac.UserId = model.UserId;
                                uac.ArticleTypeId = 0;
                                uac.CategoryId = Convert.ToInt32(cate[i]);
                                db.UserArticleTypeCates.Add(uac);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                var list = db.Users.Where(a => ids.Contains(a.UserId)).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        string[] role = Roles.GetRolesForUser(item.UserName);
                        if (list.Count() > 0)
                            Roles.RemoveUserFromRoles(item.UserName, role);
                    }
                    db.Users.Delete(a => ids.Contains(a.UserId));
                }

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
