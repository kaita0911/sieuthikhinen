using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Web;
using Mvc.Mailer;
using System.Text.RegularExpressions;
namespace VCMS.MVC4.Web.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome()
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "Welcome";
                x.ViewName = "Welcome";
                x.To.Add("some-email@example.com");
            });
        }

        public virtual MvcMailMessage PasswordReset()
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "PasswordReset";
                x.ViewName = "PasswordReset";
                x.To.Add("some-email@example.com");
            });
        }

        public virtual MvcMailMessage Contact(ContactModel model)
        {
            ViewData.Model = model;
            return Populate(x =>
            {
                x.Subject = "Contact email:" + model.Subject;
                x.ViewName = "Contact";
                x.To.Add(SiteConfig.SiteInfo.Email);
                x.To.Add(model.Email);
            });
        }
        public virtual MvcMailMessage CheckOut(CheckOutModel model)
        {
            ViewData.Model = model;
            return Populate(x =>
            {
                x.Subject = "Order Information";
                x.ViewName = "CheckOut";
                x.To.Add(SiteConfig.SiteInfo.Email);
                x.To.Add(model.Email);
            });
        }
        public virtual MvcMailMessage ForgotPassword(RegisterModel model)
        {
            ViewData.Model = model;
            return Populate(x =>
            {
                x.Subject = "Lấy lại mật khẩu";
                x.ViewName = "ForgetPassword";
                x.To.Add(SiteConfig.SiteInfo.Email);
                x.To.Add(model.Email);
            });
        }
        public virtual MvcMailMessage Welcome(RegisterModel model)
        {
            ViewData.Model = model;
            return Populate(x =>
            {
                x.Subject = "Chào mừng đến với công ty";
                x.ViewName = "Welcome";
                x.To.Add(SiteConfig.SiteInfo.Email);
                x.To.Add(model.Email);
            });
        }
        /// <summary>
        /// Create email 
        /// </summary>
        /// <param name="viewName">The view path to create email content</param>
        /// <param name="subject">email subject</param>
        /// <param name="model">the model passed to the view, in the view, it must be casted to proper class</param>
        /// <param name="recipients">array of recipient addresses</param>
        /// <returns></returns>
        public virtual MvcMailMessage CreateMessage(string viewName, string subject, object model, string[] recipients)
        {
            ViewData.Model = model;
            return Populate(x =>
            {
                if (Regex.IsMatch(SiteConfig.SiteInfo.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                    x.ReplyToList.Add(SiteConfig.SiteInfo.Email);
                x.Sender = new MailAddress(SiteConfig.SiteInfo.Email, SiteConfig.SiteInfo.Title);
                x.Subject = subject;
                x.ViewName = viewName;
                x.From = new MailAddress(SiteConfig.SiteInfo.Email, SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name);
                foreach (var item in recipients)
                {
                    if (Regex.IsMatch(item, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                        x.To.Add(item);
                }
                //foreach (var item in bbcmail)
                //{
                //    if (Regex.IsMatch(item, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                //        x.Bcc.Add(item);
                //}
            });
        }

    }
}