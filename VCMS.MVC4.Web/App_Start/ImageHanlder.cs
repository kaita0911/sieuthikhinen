using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web
{
    public class ImageHandler
    {
        public static void DefaultImage()
        {
            ImageResizer.Configuration.Config.Current.Pipeline.RewriteDefaults +=

    delegate(IHttpModule m, HttpContext c, ImageResizer.Configuration.IUrlEventArgs args)
    {

        if (args.VirtualPath.IndexOf("/UserUpload/", StringComparison.OrdinalIgnoreCase) > -1)
            args.QueryString["404"] = "~/images/404.png";

    };
        }
    }
}