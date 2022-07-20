using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;

namespace VCMS.MVC4.Web.Controllers
{
    public class ArticleFileController : BaseController
    {
        //
        // GET: /ArticleFile/

        public ActionResult Download(int id)
        {
            using (DataContext db = new DataContext())
            {
                var file = db.ArticleFiles.FirstOrDefault(f => f.Id == id);
                if (file == null) return HttpNotFound();
                var realFile = System.Web.HttpContext.Current.Server.MapPath(file.FileName);
                if (!System.IO.File.Exists(realFile))
                    return HttpNotFound();

                return File(realFile, System.Net.Mime.MediaTypeNames.Application.Octet, file.OriginalFileName);
            }
        }

        public ActionResult Viewer(int id)
        {
            using (DataContext db = new DataContext())
            {
                var file = db.ArticleFiles.FirstOrDefault(f => f.Id == id);
                if (file == null) return HttpNotFound();
                var realFile = System.Web.HttpContext.Current.Server.MapPath(file.FileName);
                if (!System.IO.File.Exists(realFile))
                    return HttpNotFound();

                return File(realFile,MimeMapping.GetMimeMapping(realFile));
            }
        }
    }
}
