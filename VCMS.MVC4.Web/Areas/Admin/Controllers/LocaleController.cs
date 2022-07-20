using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using VCMS.MVC4.Web;
using EntityFramework.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Drawing;
namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class LocaleController : VCMSAdminController
    {
        //
        // GET: /Admin/Locale/
        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                return View(db.Locales.Include(l => l.LocaleDetail).OrderBy(l => l.LocaleKey).ToList());
            }
        }
        public ActionResult Create()
        {
            Locale l = new Locale() { LocaleDetail = VList<LocaleDetail>.Create(SiteConfig.Languages) };
            return View(l);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var text = form["LocaleKey"];
                if (string.IsNullOrWhiteSpace(text))
                {
                    return Json(new { Status = 0, Message = "Không để trống Từ khóa." });
                }
                var query = db.Locales.FirstOrDefault(a => a.LocaleKey == text);
                if (query != null)
                {
                    return Json(new { Status = 0, Message = "Từ khóa đã tồn tại." });
                }
                Locale local = new Locale() { LocaleKey = form["LocaleKey"], DefaultValue = form["DefaultValue"], LocaleDetail = VList<LocaleDetail>.Create(SiteConfig.Languages) };
                foreach (var l in SiteConfig.Languages)
                {
                    local.LocaleDetail[l.Id].Value = form["LocaleDetail[" + l.Id + "].Value"];
                }
                db.Locales.Add(local);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Locales.Include(l => l.LocaleDetail).Where(p => p.Id == id).FirstOrDefault();
                var missing = SiteConfig.Languages.Where(l => !model.LocaleDetail.Any(cd => cd.LanguageId == l.Id)).Select(l => l.Id).ToList();
                missing.ForEach(m => { model.LocaleDetail.Add(new LocaleDetail { LanguageId = m }); });
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var loc = db.Locales.Include(l => l.LocaleDetail).FirstOrDefault(l => l.Id == id);
                TryUpdateModel(loc);
                var details = from d in SiteConfig.Languages
                              select new LocaleDetail()
                              {
                                  LanguageId = d.Id,
                                  Value = form["LocaleDetail[" + d.Id + "].Value"]
                              };
                loc.LocaleDetail = new VList<LocaleDetail>();
                loc.LocaleDetail.AddRange(details);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                db.Locales.Delete(l => ids.Contains((int)l.Id));

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Import(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                List<Locale> LocaleList = new List<Locale>();
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

                if (Request.Files["Import"] != null)
                {
                    #region Upload File
                    string[] allowdFile = { ".xls", ".xlsx", ".xml" };
                    string ext = Path.GetExtension(Request.Files["Import"].FileName);
                    string filename = Request.Files["Import"].FileName;

                    bool isValidFile = allowdFile.Contains(ext);
                    if (!isValidFile)
                        return Json(new { error = html.Locale("locale_error_import_file").ToHtmlString() });

                    var filePath = Path.Combine(SiteConfig.UploadFolder, "Data");
                    var physicalPath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    FileInfo newFile = new FileInfo(Path.Combine(physicalPath, filename));
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(Path.Combine(physicalPath, filename));
                    }

                    Request.Files["Import"].SaveAs(Path.Combine(physicalPath, filename));
                    #endregion
                    if (ext == ".xls" || ext == ".xlsx")
                    {
                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorkbook wkb = xlPackage.Workbook;
                            if (wkb != null)
                            {
                                for (int i = 1; i < wkb.Worksheets.Count + 1; i++)
                                {
                                    int startRow = 2;
                                    ExcelWorksheet currentworksheet = wkb.Worksheets[i];
                                    if (!currentworksheet.Cells["A1"].Value.Equals("ID") ||
                                        !currentworksheet.Cells["B1"].Value.Equals("Mã") ||
                                        !currentworksheet.Cells["C1"].Value.Equals("Giá trị mặc định"))
                                        continue;
                                    for (int rowNumber = startRow; rowNumber <= currentworksheet.Dimension.End.Row; rowNumber++)
                                    {
                                        int col = 1;
                                        int id = 0;
                                        if (!string.IsNullOrWhiteSpace(currentworksheet.Cells[rowNumber, col].Text))
                                            id = int.Parse(currentworksheet.Cells[rowNumber, col++].Text.Split('.').FirstOrDefault());
                                        else col += 1;
                                        var locale = db.Locales.Include(a => a.LocaleDetail).Where(a => a.Id == id).FirstOrDefault();

                                        string lkey = currentworksheet.Cells[rowNumber, col].Text;
                                        if (string.IsNullOrWhiteSpace(lkey))
                                            continue;

                                        if (locale != null)
                                        {
                                            #region Update Locale
                                            locale.LocaleKey = currentworksheet.Cells[rowNumber, col++].Text;
                                            locale.DefaultValue = currentworksheet.Cells[rowNumber, col++].Text;
                                            foreach (var lg in SiteConfig.Languages)
                                            {
                                                locale.LocaleDetail[lg.Id].LanguageId = lg.Id;
                                                locale.LocaleDetail[lg.Id].Value = currentworksheet.Cells[rowNumber, col++].Text;
                                            }
                                            db.SaveChanges();
                                            #endregion
                                        }
                                        else
                                        {
                                            col = 2;
                                            #region Insert Locale
                                            var localekey = db.Locales.Where(a => a.LocaleKey == lkey).FirstOrDefault();
                                            if (localekey != null)
                                                continue;
                                            var model = new Locale()
                                            {
                                                LocaleDetail = VList<LocaleDetail>.Create(SiteConfig.Languages)
                                            };

                                            model.LocaleKey = currentworksheet.Cells[rowNumber, col++].Text;
                                            model.DefaultValue = currentworksheet.Cells[rowNumber, col++].Text;
                                            foreach (var lg in SiteConfig.Languages)
                                            {
                                                model.LocaleDetail[lg.Id].LanguageId = lg.Id;
                                                model.LocaleDetail[lg.Id].Value = currentworksheet.Cells[rowNumber, col++].Text;
                                            }
                                            db.Locales.Add(model);
                                            #endregion
                                        }
                                    }
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
                else
                    return Json(new { error = html.Locale("locale_error_import_null_file").ToHtmlString() });
                return Json(new { redirect = Url.Action("Index", "Locale") });
            }
        }

        public ActionResult Export()
        {
            ///
            /// http://www.paragon-inc.com/resources/blogs-posts/easy_excel_interaction_pt6
            /// https://logicalcafe.wordpress.com/2012/03/31/reading-an-excel-2007-xlsx-into-a-datatable-without-installing-offic/
            /// http://stackoverflow.com/questions/27067178/load-large-amount-of-excel-data-with-epplus
            /// https://epplus.codeplex.com/workitem/14884
            /// 
            string fileName = "Locale.xlsx";
            var filePath = Path.Combine(SiteConfig.UploadFolder, "Data");
            var physicalPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            FileInfo newFile = new FileInfo(Path.Combine(physicalPath, fileName));

            using (DataContext db = new DataContext())
            {
                var list = db.Locales.Include(l => l.LocaleDetail).OrderBy(l => l.LocaleKey).ToList();
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(Path.Combine(physicalPath, fileName));
                }
                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws1 = package.Workbook.Worksheets.Add("Locale");

                    CreateHeader(ws1);

                    int rowIndexBegin = 2;
                    int rowIndexCurrent = rowIndexBegin;

                    GenerateData(ws1, rowIndexCurrent, list);

                    package.SaveAs(newFile);
                }
            }
            var readFile = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(filePath, fileName));
            if (!System.IO.File.Exists(readFile))
                return HttpNotFound();
            return File(readFile, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public ActionResult ExportSelect(string json)
        {
            string fileName = "Locale.xlsx";
            var filePath = Path.Combine(SiteConfig.UploadFolder, "Data");
            var physicalPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            FileInfo newFile = new FileInfo(Path.Combine(physicalPath, fileName));

            using (DataContext db = new DataContext())
            {
                JObject jsonData = JObject.Parse("{'items':" + json + "}");
                var locales = (from d in jsonData["items"]
                               select new Locale { Id = d["id"].Value<int>() }
                           ).ToList();
                var ids = locales.Select(a => a.Id).ToList();
                var list = db.Locales.Include(a => a.LocaleDetail).Where(c => ids.Contains(c.Id)).ToList();

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(Path.Combine(physicalPath, fileName));
                }
                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws1 = package.Workbook.Worksheets.Add("Locale");
                    CreateHeader(ws1);

                    int rowIndexBegin = 2;
                    int rowIndexCurrent = rowIndexBegin;

                    GenerateData(ws1, rowIndexCurrent, list);

                    package.SaveAs(newFile);
                }
            }
            var readFile = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(filePath, fileName));
            if (!System.IO.File.Exists(readFile))
                return Json(new { error = new { warning = "Error" } });
            return Json(new { redirect = Url.Action("DownloadPath", "ArticleFile", new { filePath = filePath, filename = fileName }) });
        }

        #region Create Excel
        private ExcelWorksheet CreateHeader(ExcelWorksheet ws1)
        {
            ws1.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws1.Row(1).Style.Fill.BackgroundColor.SetColor(Color.Tan);
            ws1.Row(1).Style.Font.Bold = true;
            ws1.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;

            using (var rng = ws1.Cells["A:XFD"])
            {
                rng.Style.Font.SetFromFont(new Font("Arial", 10));
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            ws1.View.FreezePanes(2, 1);
            var rowfix = 1;
            var colfix = 1;

            ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws1.Cells[1, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws1.Cells[1, rowfix++].Value = "ID";
            ws1.Column(colfix++).Width = 8;


            ws1.Cells[1, rowfix++].Value = "Mã";
            ws1.Column(colfix++).Width = 30;

            ws1.Column(colfix).Style.WrapText = true;
            ws1.Cells[1, rowfix++].Value = "Giá trị mặc định";
            ws1.Column(colfix++).Width = 45;

            foreach (var item in SiteConfig.Languages)
            {
                ws1.Column(colfix).Style.WrapText = true;
                ws1.Cells[1, rowfix++].Value = item.DisplayName;
                ws1.Column(colfix++).Width = 45;
            }
            return ws1;
        }
        private ExcelWorksheet GenerateData(ExcelWorksheet ws1, int rowIndexCurrent, ICollection<Locale> list)
        {
            foreach (var item in list.ToList())
            {
                var col = 1;
                item.LanguageId = SiteConfig.LanguageId;
                ws1.Cells[rowIndexCurrent, col++].Value = item.Id.ToString();
                ws1.Cells[rowIndexCurrent, col++].Value = item.LocaleKey;
                ws1.Cells[rowIndexCurrent, col++].Value = item.DefaultValue;
                foreach (var lg in SiteConfig.Languages)
                {
                    ws1.Cells[rowIndexCurrent, col++].Value = item.LocaleDetail[lg.Id].Value;
                }
                rowIndexCurrent++;
            }
            return ws1;
        }
        #endregion
    }
}
