using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
//using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using VCMS.MVC4.Web.Models;
using VCMS.MVC4.Data;
using Microsoft.Web.WebPages.OAuth;
using System.Net.Mail;
using System.Data.Entity;
using VCMS.MVC4.Web.Mailers;
using System.Net;
using System.IO;
using System.Data.Entity.Validation;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace VCMS.MVC4.Web.Controllers
{
    [Authorize]

    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated && !System.Web.Security.Roles.IsUserInRole("Users"))
                Response.Redirect(Url.Action("Login", "Account", new { returnUrl = Url.Action("Index", "Account") }));
            return View();
        }

        public ActionResult SelectMember()
        {
            return View();
        }
        public ActionResult Sms()
        {
            return View();
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.Error = 1;
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            using (DataContext db = new DataContext())
            {
                var categorytype = db.CategoryTypes.Include(a => a.CategoryTypeDetail).Where(a => a.NoneType && a.Code.Equals("SHIPPING", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (categorytype != null)
                {
                    ViewBag.CateType = categorytype;
                    var category = Category.GetTree(Category.GetByCateType(categorytype.Id, SiteConfig.LanguageId));
                    ViewBag.Category = category;
                }
                return View(new RegisterModel());
            }
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, FormCollection form, string returnUrl)
        {
            int state = -1;
            if (form["State"] != null)
                state = int.Parse(form["State"]);
            model.CaptCha = "";
            model.Company = "";
            model.PostalCode = "";
            model.Fax = "";
            model.State = state;

            // Attempt to register the user
            try
            {
                var err = 0;
                var db = new DataContext();
                var guest = 0;
                var user = db.Users.Where(a => a.Email == model.Email.Trim()).FirstOrDefault();

                if (user != null)
                {
                    if (!System.Web.Security.Roles.IsUserInRole(user.UserName, "Guests"))
                    {
                        ViewBag.emailError = 1;
                        err++;
                    }
                    else
                        guest = 1;
                }
                //if (Session["CaptCha"] == null || HttpContext.Session["CaptCha"].ToString() != model.CaptCha)
                //{
                //    ViewBag.captchaError = 1;
                //    err++;
                //}
                if (err > 0)
                {
                    var categorytype = db.CategoryTypes.Include(a => a.CategoryTypeDetail).Where(a => a.NoneType && a.Code.Equals("SHIPPING", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (categorytype != null)
                    {
                        ViewBag.CateType = categorytype;
                        var category = Category.GetTree(Category.GetByCateType(categorytype.Id, SiteConfig.LanguageId));
                        ViewBag.Category = category;
                    }
                    return View();
                }


                if (guest == 1)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.DisplayName = model.FullName;
                    user.Address = model.Address;
                    user.Phone = model.Phone;
                    user.Fax = model.Fax;
                    user.Company = model.Company;
                    user.City = model.City;
                    user.State = state;
                    user.PostalCode = model.PostalCode;
                    user.Newsletter = model.Newsletter;

                    WebSecurity.ChangePassword(user.UserName, "guestpasswordpwd4@vns.com", model.Password);
                    string[] roles = { "Users" };
                    string[] list = Roles.GetRolesForUser(user.UserName);
                    if (list.Count() > 0)
                    {
                        Roles.RemoveUserFromRoles(user.UserName, list);
                    }
                    db.SaveChanges();
                    Roles.AddUserToRoles(user.UserName, roles);
                    WebSecurity.Login(model.Email, model.Password);
                    return RedirectToAction("Index", "Account");
                }

                WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { DateCreated = DateTime.Now, DisplayName = model.FullName, Email = model.Email, Address = model.Address, Phone = model.Phone, Fax = model.Fax, City = model.City, State = model.State, Company = model.Company, PostalCode = model.PostalCode, Newsletter = model.Newsletter, Accumulated = 0, Flags = 0 });

                WebSecurity.Login(model.Email, model.Password);
                Roles.AddUserToRole(model.Email, "Users");

                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                UserMailer.CreateMessage("Welcome", SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name + " - " + html.Locale("email_welcome").ToHtmlString(), model, new string[] { model.Email }).Send();

                return RedirectToAction("Index", "Account");
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
            }

            return View(model);
        }

        public ActionResult Callback()
        {
            string code = Request["code"];
            string app_id = "";
            string app_secret = "";
            string redirectUri = @"http://localhost:4449/callback.aspx";

            //link này sẽ gửi các đối số đi gồm app_id, app_secret, redirectUri và code
            string requestUri = @"https://graph.facebook.com/oauth/access_token?client_id=" + app_id + "&redirect_uri=" + redirectUri + "&client_secret=" + app_secret + "&code=" + code;

            WebRequest wr = WebRequest.Create(requestUri);
            WebResponse ws = wr.GetResponse();
            Stream st = ws.GetResponseStream();
            StreamReader sr = new StreamReader(st);
            //ở đây giá trị str sẽ là "access_token=???&expires=???"
            string str = sr.ReadToEnd();

            //đoạn sau chỉ để lọc ra giá trị của access token còn lại xóa hết đi
            int index1 = str.IndexOf("&");
            str = str.Remove(index1, str.Length - index1);
            string accessToken = str.Replace("access_token=", "");
            return View();
        }
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? html.Locale("account_password_change").ToHtmlString()
                : message == ManageMessageId.SetPasswordSuccess ? html.Locale("account_password_set").ToHtmlString()
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("ChangePassowrd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError",html.Locale("account_password_change_error").ToHtmlString());
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private UserMailer _userMailer = new UserMailer();
        public UserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }
        public ActionResult CloseFacebook()
        {
            using (DataContext db = new DataContext())
            {
                //var user = db.Users.Where(u => u.Id == SiteConfig.CurrentUser.Id).SingleOrDefault();
                //if (user != null)
                //    ViewBag.IsUpdated = string.IsNullOrEmpty(user.MemberName) && string.IsNullOrEmpty(user.TipsterName);    
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            string url = Request.Url.AbsoluteUri;

            if (url.Contains("access_token"))
            {
                string accessToken = Request.QueryString["access_token"];
                string requestUrl = "https://graph.facebook.com/me?fields=name,email&access_token=" + accessToken;
                WebClient client = new WebClient();
                string userInformation = client.DownloadString(requestUrl);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var user_info = jss.Deserialize<FacebookUserInfo>(userInformation);
                if (user_info != null)
                {
                    if (WebSecurity.UserExists(user_info.Email.Trim()))
                        FormsAuthentication.SetAuthCookie(user_info.Email.Trim(), false);
                    else
                    {
                        WebSecurity.CreateUserAndAccount(user_info.Email.Trim(), "vns@123456789", new { DateCreated = DateTime.Now, SportTips = 2, TipsterName = user_info.Name, MemberName = user_info.Name, OddsFormart = 2, TimeZone = "", CountryId = 0, UniqueId = new Guid(), Email = user_info.Email, Image = "" });
                        string[] roles = { "Users" };
                        Roles.AddUserToRoles(user_info.Email.Trim(), roles);
                        WebSecurity.Logout();
                        Session["User"] = null;
                        WebSecurity.Login(user_info.Email.Trim(), "vns@123456789");
                    }
                }
                Response.Redirect("/Account/CloseFacebook");
            }

            else
                if (url.Contains("error"))
                Response.Write("Có lỗi trong quá trình đăng nhập. <a href=\"javascript:window.close();\">Đóng cửa sổ</a>");
            return View();
        }
        [AllowAnonymous]
        public ActionResult Google()
        {
            string authorizationUrl = string.Empty;
            authorizationUrl = "https://accounts.google.com/o/oauth2/auth?client_id=" + System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleClientId"] + "&redirect_uri=" + System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleRedirectURL"] + "&scope=https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email &response_type=token";
            return Redirect(authorizationUrl);
        }
        [AllowAnonymous]
        public ActionResult GoogleCallBack()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ReturnFromGoogle()
        {
            var token = Request.QueryString["access_token"];
            JObject je = new JObject();
            HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + token);
            HttpWebResponse res3 = (HttpWebResponse)req3.GetResponse();
            StreamReader streamReader3 = new StreamReader(res3.GetResponseStream());
            string returnStr3 = streamReader3.ReadToEnd();
            je = JObject.Parse(returnStr3);

            try
            {
                string email = je["email"].ToString().Replace("\"", "");
                string name = je["given_name"].ToString().Replace("\"", "");
                if (WebSecurity.UserExists(email.Trim()))
                {
                    FormsAuthentication.SetAuthCookie(email.Trim(), false);
                }
                else
                {
                    WebSecurity.CreateUserAndAccount(email.Trim(), "vns@123456789", new { DateCreated = DateTime.Now, Email = email, DisplayName=name, City=1, State=1, Newsletter=1, Accumulated=1, Flags= UserFlags.ALL});
                    string[] roles = { "Users" };
                    Roles.AddUserToRoles(email.Trim(), roles);    
                    WebSecurity.Login(email.Trim(), "vns@123456789");       
                }   
                Response.Redirect("/Account/CloseFacebook");
                //ViewBag.SocialNetworkId = je["id"].ToString().Replace("\"", "");
                //ViewBag.FirstName = je["given_name"].ToString().Replace("\"", "");
                //ViewBag.LastName = je["family_name"].ToString().Replace("\"", "");
                //ViewBag.Email = je["email"].ToString().Replace("\"", "");
                //ViewBag.ProfilePicURL = je["picture"].ToString().Replace("\"", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("CloseFacebook");

        }
      
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(FormCollection form)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            var email = form["email"].ToLower();
            using (DataContext db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email.ToLower() == email);
                if (user == null)
                    return Json(new { Status = -1, Message = html.Locale("account_error_email_notfound").ToHtmlString() });
                else
                {
                    var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
                    ForgotPasswordModel model = new ForgotPasswordModel
                    {
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Token = Url.Action("ResetPassword", "Account", new { token = token }, Request.Url.Scheme)
                    };

                    UserMailer.CreateMessage("ForgotPassword", SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name + " - Lấy lại mật khẩu", model, new string[] { model.Email }).Send();

                    return Json(new { Status = 0, Message = html.Locale("account_email_confirm").ToHtmlString() });
                }
            }

        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            int id = WebSecurity.GetUserIdFromPasswordResetToken(token);
            using (DataContext db = new DataContext())
            {
                ViewBag.token = token;
                var user = db.Users.FirstOrDefault(u => u.UserId == id);
                if (user == null) return HttpNotFound();
                else
                    return View();
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token, ResetPasswordModel model)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            if (ModelState.IsValid)
            {
                WebSecurity.ResetPassword(token, model.NewPassword);
                return Json(new { Status = 0, Message = html.Locale("account_password_change").ToHtmlString() });
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors).ToList();
                return Json(new { Status = -1, Message = "Quá trình xảy ra lỗi vui lòng thực hiện lại !", Errors = errors });
            }
        }
        public ActionResult Update()
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Users.Where(u => u.UserId == SiteConfig.CurrentUser.UserId).SingleOrDefault();
                var categorytype = db.CategoryTypes.Include(a => a.CategoryTypeDetail).Where(a => a.NoneType && a.Code.Equals("SHIPPING", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (categorytype != null)
                {
                    var category = Category.GetTree(Category.GetByCateType(categorytype.Id, SiteConfig.LanguageId));
                    ViewBag.Category = category;
                }
                if (model != null)
                {
                    UpdateModel update = new UpdateModel
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FullName = model.DisplayName,
                        Phone = model.Phone,
                        Address = model.Address,
                        Company = model.Company,
                        Fax = model.Fax,
                        City = model.City,
                        State = model.State,
                        PostalCode = model.PostalCode,
                        Newsletter = model.Newsletter
                    };
                    return View(update);
                }
                else
                {
                    return View();
                }

            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = db.Users.Where(a => a.UserId == SiteConfig.CurrentUser.UserId).SingleOrDefault();
                        if (user == null) return HttpNotFound();
                        TryUpdateModel(user);
                        if (user != null)
                        {
                            //var err = 0;
                            //if (Session["CaptCha"] == null || HttpContext.Session["CaptCha"].ToString() != model.CaptCha)
                            //{
                            //    ViewBag.captchaError = 1;
                            //    err++;
                            //}
                            //if (err > 0)
                            //    return View();
                            if (form["FullName"] != null)
                                user.DisplayName = form["FullName"].ToString();

                            if (form["Address"] != null)
                                user.Address = form["Address"].ToString();

                            if (form["Phone"] != null)
                                user.Phone = form["Phone"].ToString();

                            if (form["City"] != null)
                                user.City = Convert.ToInt32(form["City"]);

                            if (form["State"] != null)
                                user.State = Convert.ToInt32(form["State"]);

                            if (form["Company"] != null)
                                user.Company = form["Company"].ToString();

                            if (form["PostalCode"] != null)
                                user.PostalCode = form["PostalCode"].ToString();

                            var email = form["Email"].ToString();
                            if (!email.Trim().Equals(user.Email))
                            {
                                var check = db.Users.Where(a => a.Email.Trim() == email).FirstOrDefault();
                                if (check != null)
                                {
                                    ViewBag.emailError = 1;
                                    return View();
                                }
                                user.Email = email;
                                user.UserName = email;
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }

            }

            return RedirectToAction("Index", "Account", new { index = 1 });
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Newsletter(string email)
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Code.Equals("NEWLETTER", StringComparison.OrdinalIgnoreCase));
                if (type != null)
                {
                    var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                    if (string.IsNullOrWhiteSpace(email))
                        return Json(new { error = new { warning = html.Locale("newsletter_error_email_null").ToHtmlString() } });
                    else if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(email))
                        return Json(new { error = new { warning = html.Locale("newsletter_error_email_format").ToHtmlString() } });
                    else
                    {
                        var model = new Article();
                        TryUpdateModel(model);
                        var query = (from a in db.Articles
                                     join d in db.ArticleDetails on a.Id equals d.ArticleId
                                     where d.LanguageId == SiteConfig.LanguageId
                                     && (d.ArticleName.Contains(email))
                                     select new
                                     {
                                         a = a,
                                         d = d,
                                     });

                        if (query.ToList().FirstOrDefault() != null)
                            return Json(new { error = new { warning = html.Locale("newsletter_error_email_is_exit").ToHtmlString() } });
                        else
                        {
                            var details = from d in SiteConfig.Languages
                                          select new ArticleDetail()
                                          {
                                              LanguageId = d.Id,
                                              ArticleName = email,
                                              ShortDesc = "",
                                              Description = "",
                                              SEOKeywords = "",
                                          };

                            model.ArticleDetail = new VList<ArticleDetail>();
                            model.ArticleDetail.AddRange(details);
                            model.DateCreated = DateTime.Now;
                            //model.WebsiteId = SiteConfig.SiteId;
                            model.ArticleTypeId = type.Id;
                            model.UserCreated = 1;
                            db.Articles.Add(model);
                            db.SaveChanges();
                            return Json(new { done = new { message = html.Locale("newsletter_success_register").ToHtmlString() } });
                        }
                    }
                }
            }
            return new EmptyResult();
        }
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }



        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
        public class FacebookUserInfo
        {
            public FacebookUserInfo()
            {
                //
                // TODO: Add constructor logic here
                //
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Link { get; set; }
            public string UserName { get; set; }
            public string Gender { get; set; }
            public string TimeZone { get; set; }
            public string Locale { get; set; }
            public string Verified { get; set; }
            public string UpdatedTime { get; set; }
            public string Birthday { get; set; }
        }
    }
}
