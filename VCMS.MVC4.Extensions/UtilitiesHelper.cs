using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VNS.Web.Helpers
{
    public class UtilitiesHelper
    {
        public UtilitiesHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Check Email Validate
        /// <summary>
        /// Return true/false check format email address
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsValidEmailAddress(string emailaddress)
        {
            bool flag = false;

            try
            {
                MailAddress m = new MailAddress(emailaddress);
                flag = true;
                if (flag)
                {
                    string strRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

                    Regex re = new Regex(strRegex);
                    if (re.IsMatch(emailaddress))
                        flag = true;
                    else
                        flag = false;
                }
            }
            catch (FormatException)
            {
                flag = false;
            }

            return flag;
        }
        #endregion
    }
}
