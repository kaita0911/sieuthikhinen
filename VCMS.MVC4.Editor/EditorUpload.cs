using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace VCMS.MVC4.Web
{
    public class EditorUpload
    {
        public static string Upload(HttpPostedFileBase file, string fileName = "")
        {
            var folder = "~/UserUpload";
            if (HttpContext.Current.Application["upload:folder"] == null)
            {
                folder = ConfigurationManager.AppSettings["upload:folder"];
                HttpContext.Current.Application["upload:folder"] = folder;
            }
            var filePath = Path.Combine(folder, "Editor");
            var physicalPath = HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);
            var ext = Path.GetExtension(file.FileName);

            if (File.Exists(Path.Combine(physicalPath, fileName + ext)))
            {
                var files = Directory.EnumerateFiles(physicalPath, fileName + "*" + ext)
                    .Where(p => Regex.IsMatch(p, fileName + @"\-(\d+)" + ext))
                    .Select(p => int.Parse(Regex.Match(p, fileName + @"\-(\d+)" + ext).Groups[1].Value)).ToList();

                if (files.Count > 0)
                    fileName += "-" + (files.Max() + 1).ToString();
                else
                    fileName += "-1";
            }
            fileName += ext;
            file.SaveAs(Path.Combine(physicalPath, fileName));

            return VirtualPathUtility.ToAbsolute(Path.Combine(filePath, fileName).Replace(@"\", "/"));
        }
    }
}