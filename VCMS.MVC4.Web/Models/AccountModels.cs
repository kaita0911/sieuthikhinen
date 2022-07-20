using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VCMS.MVC4.Web
{
    public class RegisterExternalLoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }
        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]

        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu nhập lại không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống"), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống"), DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống"), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống"), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống"), StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nhập lại mật khẩu không đúng"), DataType(DataType.Password), System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Điện thoại không được để trống")]
        public string Phone { get; set; }
        public string Fax { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }
        public string Sex { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public string Company { get; set; }
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Captcha không được để trống")]
        public string CaptCha { get; set; }
        public bool Newsletter { get; set; }
    }
    public class ResetPasswordModel
    {

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật nhập lại không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }
    public class UpdateModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email không được để trống"), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Điện thoại không được để trống")]
        public string Phone { get; set; }
        public string Fax { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }
        public int City { get; set; }
        public string Sex { get; set; }
        public int State { get; set; }
        public string PostalCode { get; set; }
        public string Company { get; set; }
        //[Required]
        public string CaptCha { get; set; }
        public bool Newsletter { get; set; }
    }
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }

    }
    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }

    }
    public class SupplierModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống"), DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
