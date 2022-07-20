using Mvc.Mailer;

namespace VCMS.MVC4.Web.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
			MvcMailMessage PasswordReset();
            MvcMailMessage Contact(ContactModel model);
            MvcMailMessage CheckOut(CheckOutModel model);
	}
}