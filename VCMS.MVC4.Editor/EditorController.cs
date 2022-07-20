using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Data;
using VCMS.MVC4.Data;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class EditorController : Controller
    {
        //
        // GET: /Admin/Editor/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImageUpload(EditorType editorType = EditorType.FROALA, string name = "")
        {
            var file = Request.Files[0];
            var filename = Unichar.UnicodeStrings.UrlString(Path.GetFileNameWithoutExtension(file.FileName).ToString());
            if (!string.IsNullOrWhiteSpace(name))
                filename = name;
            var ext = Path.GetExtension(file.FileName);
            if (Regex.IsMatch(ext, @"jpg|gif|jpeg|png", RegexOptions.IgnoreCase))
            {
                var fileName = EditorUpload.Upload(file, filename);
                if (editorType == EditorType.FROALA)
                    return Json(new { link = fileName });
                else if (editorType == EditorType.CKEDITOR)
                {
                    var ckFuncNum = Request["CKEditorFuncNum"];
                    return Content(string.Format("<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction('{1}', '{0}', '');</script>", fileName, ckFuncNum));

                }
                else
                    return Json(new { filelink = fileName });
            }
            else
            {
                if (editorType == EditorType.CKEDITOR)
                {
                    var ckFuncNum = Request["CKEditorFuncNum"];
                    return Content(string.Format("<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction('{1}', '{0}', 'Extensions not allowed, use PNG, JPG, GIF files only.');</script>", "", ckFuncNum));

                }
                return Content("");
            }
                
        }

        public ActionResult ImageList(EditorType editorType = EditorType.FROALA)
        {

            if (editorType == EditorType.FROALA)
            {
                var ar = Directory.GetFiles(Server.MapPath("~/UserUpload/Editor"), "*.*")
                    .Where(f => Regex.IsMatch(Path.GetExtension(f).ToLower(), @"jpg|gif|jpeg|png"))
                    .Select(f => string.Format("{0}{1}", Url.Content("~/UserUpload/Editor/"), Path.GetFileName(f))).ToArray();
                return Json(ar, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ar = Directory.GetFiles(Server.MapPath("~/UserUpload/Editor"), "*.*")
                 .Where(f => Regex.IsMatch(Path.GetExtension(f).ToLower(), @"jpg|gif|jpeg|png"))
                 .Select(f => new
                 {
                     thumb = string.Format("{0}{1}?width=100", Url.Content("~/UserUpload/Editor/"), Path.GetFileName(f)),
                     image = string.Format("{0}{1}", Url.Content("~/UserUpload/Editor/"), Path.GetFileName(f))
                 }).ToArray();
                return Json(ar, JsonRequestBehavior.AllowGet);
            }
        }

    }
    public enum EditorType
    {
        FROALA,
        REDACTOR,
        CKEDITOR
    }
}
