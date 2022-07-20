using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VCMS.MVC4.Data;

namespace VCMS.MVC4.Web
{
    public class Uploader
    {
        public static VCMS.MVC4.Data.ArticleFile Upload(HttpPostedFileBase file, ArticleFileType fileType, string path, string fileName = "")
        {
            fileName = fileName.ToLower();
            var filePath = Path.Combine(SiteConfig.UploadFolder, path);
            var physicalPath = HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);
            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Guid.NewGuid().ToString();


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

            ArticleFile af = new ArticleFile { FileName = Path.Combine(filePath, fileName).Replace("\\", "/"), OriginalFileName = file.FileName, FileSize = file.ContentLength, FileType = fileType, FullPath = Path.Combine(physicalPath, fileName), DateCreated = DateTime.Now };
            return af;
        }
    }
}