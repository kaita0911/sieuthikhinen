using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using EntityFramework.Extensions;
using System.IO;
using Newtonsoft.Json.Linq;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class ArticleFileController : VCMSAdminController
    {
        //
        // GET: /ArticleFile/

        public ActionResult Index(int id, ArticleFileType fileType)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.Article = db.Articles.Include(a => a.ArticleType.ArticleTypeDetail).FirstOrDefault(a => a.Id == id);
                var list = db.ArticleFiles.Include(af => af.ArticleFileDetail).Where(af => af.ArticleId == id && af.FileType == fileType).ToList();
                return View(list);
            }
        }

        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.ArticleFiles.Include(af => af.ArticleFileDetail).SingleOrDefault(af => af.Id == id);
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    var model = db.ArticleFiles.Include(f => f.Article).Include(af => af.ArticleFileDetail).SingleOrDefault(af => af.Id == id);
                    if (model == null) return HttpNotFound();
                    TryUpdateModel(model);

                    var article = db.Articles.Include(a => a.ArticleType).FirstOrDefault(a => a.Id == model.Article.Id);
                    var i = 0;
                    var typeName = article.ArticleType.Code.ToLower();
                    typeName = typeName.First().ToString().ToUpper() + String.Join("", typeName.Skip(1));

                    if (form["Upload"] != null)
                    {
                        var filenames = form["Upload"].Split(',');
                        var imagefiles = Enumerable.Range(0, Request.Files.Count)
                                          .Where(f => string.Equals("multiple_file", Request.Files.GetKey(f), StringComparison.InvariantCultureIgnoreCase))
                                          .Select(Request.Files.Get);

                        foreach (HttpPostedFileBase file in imagefiles)
                        {
                            if (file.ContentLength > 0)
                            {
                                if (form["Upload"].Split(',').Skip(i).Take(1).Contains(file.FileName))
                                {
                                    filenames = filenames.Where(s => !s.Equals(file.FileName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                                    var af = Uploader.Upload(file, model.FileType, typeName);
                                    model.FileName = af.FileName;
                                    i++;
                                }
                            }
                        }
                    }

                    var details = from d in SiteConfig.Languages
                                  select new ArticleFileDetail()
                                  {
                                      LanguageId = d.Id,
                                      Title = Request[string.Format("ArticleFileDetail[{0}].Title", d.Id)],
                                      Description = Request[string.Format("ArticleFileDetail[{0}].Description", d.Id)],
                                  };
                   
                    model.ArticleFileDetail = new VList<ArticleFileDetail>();
                    model.ArticleFileDetail.AddRange(details);
                    
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = model.ArticleId, fileType = model.FileType });
                }
                else
                    return View();
            }
        }
        public ActionResult Create(int id, ArticleFileType fileType)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType).Include(a => a.ArticleType.ArticleTypeDetail).FirstOrDefault(a => a.Id == id);
                if (model == null) return HttpNotFound();
                ViewBag.Article = model;
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(int id, ArticleFileType fileType, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                using (DataContext db = new DataContext())
                {
                    var i = 0; var uploadError = false;
                    var typeName = form["typeName"].ToString();
                    typeName = typeName.First().ToString().ToUpper() + String.Join("", typeName.Skip(1));

                    if (form["Upload"] != null)
                    {
                        var filenames = form["Upload"].Split(',');
                        var imagefiles = Enumerable.Range(0, Request.Files.Count)
                                          .Where(f => string.Equals("multiple_file", Request.Files.GetKey(f), StringComparison.InvariantCultureIgnoreCase))
                                          .Select(Request.Files.Get);

                        foreach (HttpPostedFileBase file in imagefiles)
                        {
                            if (file.ContentLength > 0)
                            {
                                if (form["Upload"].Split(',').Skip(i).Take(1).Contains(file.FileName))
                                {
                                    filenames = filenames.Where(s => !s.Equals(file.FileName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                                    var af = Uploader.Upload(file, fileType, typeName);
                                    if (af != null)
                                    {
                                        var details = from d in SiteConfig.Languages
                                                      select new ArticleFileDetail()
                                                      {
                                                          LanguageId = d.Id,
                                                          Title = Request[string.Format("ArticleFileDetail[{0}].Title", d.Id)],
                                                          Description = Request[string.Format("ArticleFileDetail[{0}].Description", d.Id)],
                                                      };
                                        af.ArticleFileDetail = new VList<ArticleFileDetail>();
                                        af.ArticleId = id;
                                        af.ArticleFileDetail.AddRange(details);
                                        db.ArticleFiles.Add(af);
                                    }
                                    else
                                    {
                                        uploadError = true;
                                        break;
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                    else
                    {
                        var af = new ArticleFile();
                        TryUpdateModel(af);
                        var details = from d in SiteConfig.Languages
                                      select new ArticleFileDetail()
                                      {
                                          LanguageId = d.Id,
                                          Title = Request[string.Format("ArticleFileDetail[{0}].Title", d.Id)],
                                          Description = Request[string.Format("ArticleFileDetail[{0}].Description", d.Id)],
                                      };
                        af.FileType = fileType;
                        af.DateCreated = DateTime.Now;
                        af.ArticleFileDetail = new VList<ArticleFileDetail>();
                        af.ArticleId = id;
                        af.ArticleFileDetail.AddRange(details);
                        db.ArticleFiles.Add(af);

                        i++;
                    }
                    if (!uploadError)
                        db.SaveChanges();
                    else
                    {
                        ViewBag.Error = true;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index", new { id = id, fileType = fileType });
        }

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
        public ActionResult DownloadPath(string filePath, string filename)
        {
            using (DataContext db = new DataContext())
            {
                var realFile = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(filePath, filename));
                if (!System.IO.File.Exists(realFile))
                    return HttpNotFound();

                return File(realFile, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
        }
        [HttpPost]
        public ActionResult Delete(String id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                //file deletion
                var files = db.ArticleFiles.Where(af => ids.Contains(af.Id)).Select(af => af.FullPath).ToList();
                files.ForEach(f => { if (System.IO.File.Exists(f)) System.IO.File.Delete(f); });
                db.ArticleFiles.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult Update(int articleId, int defaultId)
        {
            using (DataContext db = new DataContext())
            {
                var article = db.Articles.Include(a => a.ArticleFiles).FirstOrDefault(a => a.Id == articleId);
                var file = article.ArticleFiles.FirstOrDefault(f => f.Id == defaultId);
                var others = article.ArticleFiles.Where(af => af.Id != defaultId).ToList();
                others.ForEach(af => { af.IsDefault = false; });
                file.IsDefault = true;
                article.ImageUrl = file.FileName;
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult UpdateAttributes(string json)
        {
            using (DataContext db = new DataContext())
            {
                JObject jsonData = JObject.Parse("{'items':" + json + "}");
                var articlef = (from d in jsonData["items"]
                                select new ArticleFile { Id = d["id"].Value<int>(), IsNew = d["isNew"].Value<bool>() }
                           ).ToList();
                var ids = articlef.Select(a => a.Id).ToList();
                var list = db.ArticleFiles.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(a =>
                {
                    var item = articlef.FirstOrDefault(d => d.Id == a.Id);
                    if (item != null)
                    {
                        a.IsNew = item.IsNew;
                    }
                });

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult UpdateOrder(string id, string order)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var orders = order.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();

                var list = db.ArticleFiles.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
