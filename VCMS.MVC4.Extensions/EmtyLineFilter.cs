using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VCMS.MVC4.Extensions
{
    public class TranslationFilter : MemoryStream
    {
        private Stream filter = null;

        public TranslationFilter(HttpResponseBase httpResponseBase)
        {
            filter = httpResponseBase.Filter;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var response = UTF8Encoding.UTF8.GetString(buffer);

            // remove all newlines
            //response = response.Replace(System.Environment.NewLine, "");

            response = Regex.Replace(response, @"^\s*[\r\n]+", "", RegexOptions.Multiline);
            

            filter.Write(UTF8Encoding.UTF8.GetBytes(response), offset, UTF8Encoding.UTF8.GetByteCount(response));
        }
    }

    public class EmptyLineFilter : ActionFilterAttribute
    {
        public EmptyLineFilter()
        {
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            filterContext.HttpContext.Response.Filter = new TranslationFilter(filterContext.HttpContext.Response);
        }
    }
}
