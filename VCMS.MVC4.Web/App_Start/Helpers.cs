using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VCMS.MVC4.Web
{
    public class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            
        }
    }

    public static class HtmlExtensions
    {
        public static MvcHtmlString Amount(this HtmlHelper html, decimal amount)
        {
            return new MvcHtmlString(amount.ToString("#,##0"));
        }
    }
}
