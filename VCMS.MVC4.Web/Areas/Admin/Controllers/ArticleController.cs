using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VCMS.MVC4.Data;
using WebMatrix.WebData;
using System.Data.Entity;
using System.IO;
using EntityFramework.Extensions;
using VCMS.MVC4.Web;
using System.Web.Script.Serialization;

using System.Data.Entity.Infrastructure;
using VNS.Web.Helpers;
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;

using VCMS.MVC4.Web.Areas.Admin.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using VCMS.MVC4.Web.Models;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    //[Authorize]
    public class ArticleController : VCMSAdminController
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AddRows(int pageIndex = 1, int id = 0)
        {
            using (DataContext db = new DataContext())
            {
                List<string> list = new List<string>();
                var discount = db.Discounts.Include(a => a.Articles).FirstOrDefault(a => a.Id == id);
                var listId = discount.Articles.Select(a => a.Id).ToList();
                var articles = db.Articles.Include(a => a.ArticleDetail.Select(b => b.Body)).OrderByDescending(c => c.SortOrder).Where(c => c.ArticleTypeId == 2 && !listId.Contains(c.Id)).Skip((pageIndex - 1) * 5).Take(5).ToList();
                if (articles.Count() > 0)
                {
                    string html = "";
                    foreach (var item in articles)
                    {
                        html += "<tr class='rows'><td><label class='checkbox discount'><input name='articles' type='checkbox' value='" + item.Id + "' class='check'/><i></i></label></td>";
                        html += "<td><span>" + item.ArticleName + "</span></td></tr>";
                    }
                    list.Add(html);
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AutoComplete(string keyword, int id)
        {
            using (DataContext db = new DataContext())
            {
                if (string.IsNullOrEmpty(keyword)) keyword = "";
                var query = (from a in db.Articles
                             join d in db.ArticleDetails on a.Id equals d.ArticleId
                             where a.ArticleTypeId == 2 && d.LanguageId == 1 && (d.ArticleName.Contains(keyword) || keyword == "")
                             select new
                             {
                                 a = a,
                                 d = d
                             });

                var ret = new List<Article>();
                foreach (var item in query.ToList())
                {
                    item.a.ArticleDetail = new VList<ArticleDetail>() { item.d };
                    item.a.LanguageId = 1;
                    ret.Add(item.a);
                }
                var dis = db.Discounts.Include(a => a.Articles).FirstOrDefault(a => a.Id == id);

                List<string> list = new List<string>();

                if (ret.Count > 0)
                {
                    var listID = dis != null ? dis.Articles.Select(a => a.Id).ToList() : null;
                    string html = "";
                    if (listID != null)
                    {
                        ret = listID != null ? ret.Where(a => !listID.Contains(a.Id)).ToList() : ret;
                        foreach (var item in listID)
                        {
                            var art = db.Articles.Include(a => a.ArticleDetail.Select(b => b.Body)).FirstOrDefault(a => a.Id == item);
                            html += "<tr class='rows'><td><label class='checkbox discount'><input name='articles' value='" + art.Id + "' type='checkbox' class='check' checked/><i></i></label></td>";
                            html += "<td><span class= 'name'>" + art.ArticleName + "</span></td></tr>";
                        }
                    }

                    foreach (var item in ret)
                    {
                        html += "<tr class='rows'><td><label class='checkbox discount'><input name='articles' value='" + item.Id + "' type='checkbox' class='check'/><i></i></label></td>";
                        html += "<td><span class= 'name'>" + item.ArticleName + "</span></td></tr>";
                    }
                    list.Add(html);
                }
                else
                    list.Add("<tr class='no-data'><td colspan='4'>Không có kết quả nào hợp lệ.</td></tr>");
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult List(int id, int page = 1, int pageSize = 10)
        {
            using (DataContext db = new DataContext())
            {
                //ViewBag.ArticleType = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Single(t => t.Id == id);
                ViewBag.ArticleType = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Include(b=>b.Categories.Select(c=>c.CategoryDetail)).Single(t => t.Id == id);

                //ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER;
                //SortDirection direction = SortDirection.DESCENDING;
                ArticleSortOrder sortOrder = ArticleSortOrder.ARTICLE_NAME;
                SortDirection direction = SortDirection.ASCENDING;
                ArticleFlags flag = ArticleFlags.INACTIVE | ArticleFlags.ACTIVE;

                if (Request["sortorder"] != null)
                    sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
                if (Request["sortdirection"] != null)
                    direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
                if (Request["flag"] != null)
                {
                    if (Request["flag"] == "all")
                        flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
                    else
                        flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
                }

                var items = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType).Where(a => a.ArticleTypeId == id).OrderBy(a => a.SortOrder).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var count = db.Articles.Include(a => a.ArticleType).Where(a => a.ArticleTypeId == id).Count();
                var articles = new ArticleResult { TotalItemCount = count, List = items, PageIndex = page, PageSize = pageSize };
               
                //var articles = Article.GetByType(id, SiteConfig.LanguageId, flag, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, includeflags: ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.ARTICLE_DETAIL, resultFlag: ArticleResultFlags.ALL, admin: true);
                //int count = articles.TotalItemCount;//Article.CountByType(id, SiteConfig.LanguageId, flag, true);

                //if (Roles.IsUserInRole("Moderators"))
                //{
                //    articles = Article.GetByUser(SiteConfig.CurrentUser.UserId, id, SiteConfig.LanguageId, flag, pageIndex: pageIndex, pageSize: pageSize, sortOrder: sortOrder, direction: direction, includeflags: ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.ARTICLE_DETAIL, admin: true);
                //    count = articles.TotalItemCount;//Article.CountByUser(SiteConfig.CurrentUser.UserId, id, SiteConfig.LanguageId, flag, true);
                //}
                ViewBag.ItemCount = count;
                return View(articles);
            }
        }

        public ActionResult Search(int id, string keyword, int categoryId = 0, int page = 1, int pageSize = 20)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.ArticleType = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail)).Include(a => a.ArticleTypeDetail).Single(t => t.Id == id);

                ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER;
                SortDirection direction = SortDirection.DESCENDING;
                ArticleFlags flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;

                if (Request["sortorder"] != null)
                    sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
                if (Request["sortdirection"] != null)
                    direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
                if (Request["flag"] != null)
                {
                    if (Request["flag"] == "all")
                        flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
                    else
                        flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
                }
                var articles = Article.Search(id, SiteConfig.LanguageId, keyword, categoryId, page, pageSize, sortOrder, direction, flag, admin: true);
                ViewBag.ItemCount = articles.TotalItemCount;


                return View("List", articles);

            }
        }

        public ActionResult Create(int id)
        {
            using (DataContext db = new DataContext())
            {
                var has_brand = db.ArticleTypes.FirstOrDefault(a => a.Code == "BRAND");
                if (has_brand != null)
                {
                    ViewBag.ArticleBrand = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail))
                    .Include("ArticleTypeDetail")
                    .Include("Properties.PropertyDetail")
                    .SingleOrDefault(t => t.Id == has_brand.Id);
                }

                var has_groupsx = db.ArticleTypes.FirstOrDefault(a => a.Code == "GROUPPRODUCT");
                if (has_groupsx != null)
                {
                    ViewBag.ArticleGroupsx = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail))
                    .Include("ArticleTypeDetail")
                    .Include("Properties.PropertyDetail")
                    .SingleOrDefault(t => t.Id == has_groupsx.Id);
                }

                ViewBag.ArticleType = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail)).Include(at => at.Properties.Select(p => p.PropertyDetail)).Include(ct => ct.CategoryTypes.Select(ctd => ctd.CategoryTypeDetail)).Include(a => a.ArticleTypeDetail).SingleOrDefault(t => t.Id == id);
                Article model = new Article()
                {
                    ArticleDetail = VList<ArticleDetail>.Create(SiteConfig.Languages),
                    SortOrder = (db.Articles.Where(a => a.ArticleTypeId == id).Max(a => a.SortOrder) ?? 0) + 1
                };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(int id, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var model = new Article();
                TryUpdateModel(model);
                using (DataContext db = new DataContext())
                {
                    ArticleType type = db.ArticleTypes.Include(ct => ct.CategoryTypes).Include(at => at.Categories.Select(c => c.CategoryDetail)).Include(a => a.Properties).Where(t => t.Id == id).Single();

                    var vals = from p in type.Properties.Where(p => !p.MultiLanguage && p.PropertyType != PropertyType.MULTICHOICE && p.PropertyType != PropertyType.CHOICE)
                               select new ArticlePropertyValue() { PropertyId = p.Id, Value = Request["P_" + p.Id.ToString()], Code = p.Code };

                    var multivals = from p in type.Properties.Where(p => p.MultiLanguage && p.PropertyType != PropertyType.MULTICHOICE && p.PropertyType != PropertyType.CHOICE)
                                    join l in SiteConfig.Languages on 1 equals 1
                                    select new ArticlePropertyValue() { LanguageId = l.Id, PropertyId = p.Id, Value = Request[string.Format("ArticleDetail[{0}].P_{1}_{2}", (l.Id - 1).ToString(), p.Id.ToString(), l.Id.ToString())] };

                    var propvals = vals.ToList();
                    propvals.AddRange(multivals.ToList());
                    model.PropertyValues = propvals;

                    if (form["relatedArticles"] != null && model.PropertyValues.FirstOrDefault(v => v.Code.Equals("OTHERARTICLE")) != null)
                        model.PropertyValues.FirstOrDefault(v => v.Code.Equals("OTHERARTICLE")).Value = form["relatedArticles"].ToString();

                    List<Category> lstCate = new List<Category>();
                    if (type.HasCategory && form["categories"] != null)
                    {
                        var cats = form["categories"].Split(new char[] { ',' });
                        var categories = type.Categories.Where(c => cats.Contains(c.Id.ToString())).ToList();
                        lstCate.AddRange(categories);
                    }

                    ArticleType has_brand = db.ArticleTypes.Include(ct => ct.CategoryTypes).Include(at => at.Categories.Select(c => c.CategoryDetail)).FirstOrDefault(a => a.Code == "BRAND");
                    if (has_brand != null && form["categories_b"] != null)
                    {
                        var cats = form["categories_b"].Split(new char[] { ',' });
                        ArticleType t_brand = db.ArticleTypes.Include(at => at.Categories).Where(t => t.Id == has_brand.Id).Single();
                        var categories = t_brand.Categories.Where(c => cats.Contains(c.Id.ToString())).ToList();
                        lstCate.AddRange(categories);
                    }
                    ArticleType has_groupsx = db.ArticleTypes.FirstOrDefault(a => a.Code == "GROUPPRODUCT");
                    if (has_groupsx != null && form["categories2"] != null)
                    {
                        var catsx = form["categories2"].Split(new char[] { ',' });
                        ArticleType t_groupsx = db.ArticleTypes.Include(at => at.Categories).Where(t => t.Id == has_groupsx.Id).Single();
                        var categories = t_groupsx.Categories.Where(c => catsx.Contains(c.Id.ToString())).ToList();
                        lstCate.AddRange(categories);
                    }


                    foreach (var item in type.CategoryTypes)
                    {
                        var cate = db.Categories.Where(c => c.CategoryTypeId == item.Id).ToList();
                        if (form["cate_bycatetype"] != null)
                        {
                            var cats = form["cate_bycatetype"].Split(new char[] { ',' });
                            var categories = cate.Where(c => cats.Contains(c.Id.ToString())).ToList();
                            lstCate.AddRange(categories);
                        }
                    }
                    model.Categories = lstCate;

                    List<PropertyMultiValue> lstPmv = new List<PropertyMultiValue>();
                    var proMultiVal = db.PropertyMultiValues.ToList();

                    if (form["pmv_multichoice"] != null)
                    {
                        var pmvs = form["pmv_multichoice"].Split(new char[] { ',' });
                        var pros = proMultiVal.Where(p => pmvs.Contains(p.Id.ToString())).ToList();
                        lstPmv.AddRange(pros);
                    }

                    if (form["pmv_choice"] != null)
                    {
                        var pmvs = form["pmv_choice"].Split(new char[] { ',' });
                        var pros = proMultiVal.Where(p => pmvs.Contains(p.Id.ToString())).ToList();
                        lstPmv.AddRange(pros);
                    }
                    model.PropertyMultiValues = lstPmv;

                    //string tempmang = model.ArticleName;

                    //ArticleDetail keyword = new ArticleDetail();
                    model.Keywords = new List<Keyword>();
                    
                    if (type.Code == "PRODUCT")  
                    {
                        var temp = "";
                        var cd3 = "";
                        var name = model.ArticleName.Replace(",", ".").Trim();
                        var masp = model.PropertyValues.FirstOrDefault(v => v.PropertyId == 6).Value.Replace(",",".").Trim();
                        var countnhomsp = model.Categories.Where(ed => ed.ArticleTypeId == 2).ToList();
                        var countbrand = model.Categories.Where(eds => eds.ArticleTypeId == 5).ToList();

                        if (masp == "" && countbrand.Count == 0 && countnhomsp.Count == 0)
                        {
                            temp = name;
                        }
                        else
                        {
                            if (masp !=""){

                                if (countbrand.Count == 0 && countnhomsp.Count == 0)
                                {
                                    temp = name + "," + masp;
                                }
                                else if (countbrand.Count > 0 && countnhomsp.Count == 0)
                                {

                                    var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;

                                    cd3 = brand + " " + masp;
                                    temp = name + "," + cd3 + "," + masp;
                                    
                                   
                                }
                                else if(countbrand.Count == 0 && countnhomsp.Count > 0)
                                {
                                    if (countnhomsp.Count == 1)
                                    {
                                        var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom1 + "," + masp;
                                    }
                                    else
                                    {
                                        var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom2 + "," + masp;
                                    }
                                }
                                else
                                {
                                    var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;
                                    cd3 = brand + " " + masp;
                                    if (countnhomsp.Count == 1)
                                    {
                                        var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom1 + "," + cd3 + "," + masp;
                                    }
                                    else
                                    {
                                        var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom2 + "," + cd3 + "," + masp;
                                    }
                                }
                                
                            }
                            else
                            {
                                if (countbrand.Count > 0 && countnhomsp.Count == 0)
                                {
                                    var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;

                                    temp = name + "," + brand;
                                }
                                else if (countbrand.Count == 0 && countnhomsp.Count > 0)
                                {
                                    if (countnhomsp.Count == 1)
                                    {
                                        var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom1;
                                    }
                                    else
                                    {
                                        var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom2;
                                    }
                                }
                                else
                                {
                                    var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;

                                    if (countnhomsp.Count == 1)
                                    {

                                        var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom1 + "," + brand;
                                    }
                                    else
                                    {
                                        var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                        temp = name + "," + nhom2 + "," + brand;
                                    }
                                }
                            }
                            
                        }
                       
                        //var nhom2 = db.Categories.FirstOrDefault(c => c.Id == model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).Id).CategoryName;
                        //var brand = db.Categories.FirstOrDefault(w =>w.Id == model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).Id).CategoryName;
                      
                        (from d in SiteConfig.Languages
                         select (temp).Split(new char[] { ',' })
                         .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Unichar.UnicodeStrings.LatinToAscii(s).Replace("/", " ").Replace(",",".")).ToArray())
                                                       .ToList()
                                                       .ForEach(kwa =>
                                                       {
                                                           var keywords = db.Keywords.Where(k => kwa.Contains(k.Tag)).ToList();
                                                           var newKeywords = kwa.Where(s => !keywords.Exists(k => k.Tag == s)).Select(s => new Keyword { Tag = s }).ToList();
                                                           model.Keywords = model.Keywords.Concat(keywords.Concat(newKeywords.AsEnumerable<Keyword>())).ToList();
                                                       });
                        string url = model.UrlFriendly;
                        string temptring = "";
                        string res = url.Substring(0, 1);
                        if (res == "-")
                        {
                            temptring = url.Remove(0, 1);
                            char res2 = temptring[temptring.Length - 1];
                            if (Char.ToString(res2) == "-")
                            {
                                temptring = temptring.Remove(temptring.Length - 1);

                            }
                            else
                            {
                                temptring = url;
                            }

                        }
                        else
                        {
                            char res2 = url[url.Length - 1];
                            if (Char.ToString(res2) == "-")
                            {
                                temptring = url.Remove(url.Length - 1);

                            }
                            else
                            {
                                temptring = url;
                            }
                        }

                        var dt = model.ArticleDetail.Select(d => new ArticleDetail
                        {
                            LanguageId = d.LanguageId,

                            ArticleName = d.ArticleName.Trim(),
                            ShortDesc = d.ShortDesc,
                            Description = d.Description,
                            SEOKeywords = temp,
                            SEODescription = d.SEODescription,
                            UrlFriendly = temptring,
                           
                    });
                        
                        model.ArticleDetail = new VList<ArticleDetail>();
                        model.ArticleDetail.AddRange(dt);
                    }
                    else
                    {

                        (from d in SiteConfig.Languages
                         select (Request[string.Format("ArticleDetail[{0}].SEOKeywords", d.Id - 1)] ?? "").Split(new char[] { ',' })
                         .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Unichar.UnicodeStrings.LatinToAscii(s)).ToArray())
                                                        .ToList()
                                                        .ForEach(kwa =>
                                                        {
                                                            var keywords = db.Keywords.Where(k => kwa.Contains(k.Tag)).ToList();
                                                            var newKeywords = kwa.Where(s => !keywords.Exists(k => k.Tag == s)).Select(s => new Keyword { Tag = s }).ToList();
                                                            model.Keywords = model.Keywords.Concat(keywords.Concat(newKeywords.AsEnumerable<Keyword>())).ToList();
                                                        });
                        string url = model.UrlFriendly;
                        string temptring = "";
                        string res = url.Substring(0, 1);
                        if (res == "-")
                        {
                            temptring = url.Remove(0, 1);

                        }
                        char res2 = temptring[temptring.Length - 1];
                        if (Char.ToString(res2) == "-")
                        {
                            temptring = temptring.Remove(temptring.Length - 1);

                        }
                        var dt = model.ArticleDetail.Select(d => new ArticleDetail
                        {
                            LanguageId = d.LanguageId,

                            ArticleName = d.ArticleName.Trim(),
                            ShortDesc = d.ShortDesc,
                            Description = d.Description,
                            SEODescription = d.SEODescription,
                            UrlFriendly = temptring
                        });
                        model.ArticleDetail = new VList<ArticleDetail>();
                        model.ArticleDetail.AddRange(dt);
                    }
                    

                    if (type.Code != "ADV")
                    {
                        model.DateCreated = DateTime.Now;
                        model.DateUpdated = DateTime.Now;
                    }

                    //model.WebsiteId = SiteConfig.SiteId;
                    model.ArticleTypeId = id;
                    model.UserCreated = SiteConfig.CurrentUser.UserId;

                    var typeName = type.Code.ToLower().ToString();
                    typeName = typeName.First().ToString().ToUpper() + String.Join("", typeName.Skip(1));
                    if (type.HasImage && Request.Files.Count > 0)
                    {
                        var i = 0;
                        var filenames = form["Upload"].Split(',');

                        var imagefiles = Enumerable.Range(0, Request.Files.Count)
                                  .Where(f => string.Equals("multiple_file", Request.Files.GetKey(f), StringComparison.InvariantCultureIgnoreCase))
                                  .Select(Request.Files.Get);

                        foreach (HttpPostedFileBase file in imagefiles)
                        {
                            if (file.ContentLength > 0)
                            {
                                if (filenames.Contains(file.FileName))
                                {
                                    filenames = filenames.Where(s => !s.Equals(file.FileName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                                    var af = Uploader.Upload(file, ArticleFileType.IMAGE, typeName, model.ArticleDetail.FirstOrDefault().UrlFriendly);
                                    af.DateCreated = DateTime.Now;
                                    af.SortOrder = 0;
                                    var afd = from d in SiteConfig.Languages
                                              select new ArticleFileDetail()
                                              {
                                                  LanguageId = d.Id,
                                                  Title = Request[string.Format("ArticleDetail[{0}].ArticleName", d.Id)],
                                              };
                                    af.ArticleFileDetail = new VList<ArticleFileDetail>();
                                    if (i == 0)
                                    {
                                        af.IsDefault = true;
                                        model.ImageUrl = af.FileName;
                                    }
                                    af.ArticleFileDetail.AddRange(afd);
                                    if (model.ArticleFiles == null) model.ArticleFiles = new List<ArticleFile>();
                                    model.ArticleFiles.Add(af);
                                    i++;
                                }
                            }
                        }
                    }

                    if (type.HasAttachment && Request.Files.Count > 0)
                    {
                        var file = Request.Files["Attachment"];
                        if (file != null && file.ContentLength > 0)
                        {
                            var af = Uploader.Upload(file, ArticleFileType.ATTACHMENT, typeName);
                            var afd = from d in SiteConfig.Languages
                                      select new ArticleFileDetail()
                                      {
                                          LanguageId = d.Id,
                                          Title = Request[string.Format("ArticleDetail[{0}].ArticleName", d.Id)],
                                      };
                            af.ArticleFileDetail = new VList<ArticleFileDetail>();
                            af.IsDefault = true;
                            af.DateCreated = DateTime.Now;
                            af.SortOrder = 0;
                            af.ArticleFileDetail.AddRange(afd);

                            if (model.ArticleFiles == null) model.ArticleFiles = new List<ArticleFile>();
                            model.ArticleFiles.Add(af);
                        }
                    }

                    List<Discount> lstDis = new List<Discount>();
                    if (type.HasPrice)
                    {
                        Price pri = new Price();
                        if (!string.IsNullOrEmpty(Request["Price"]))
                        {
                            pri.IsDefault = true;
                            pri.Title = "Giá chính";
                            pri.DateCreated = DateTime.Now;
                            pri.Value = decimal.Parse(Request["Price"]);
                            pri.CurrencyId = int.Parse(Request["Currency"]);
                            pri.PriceShortOrder = pri.Value;
                            if (Request["chk-inactive"] != null)
                            {
                                bool[] chek = Request["chk-inactive"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                                pri.Inactive = chek[0];
                            }
                            if (model.Prices == null) model.Prices = new List<Price>();
                            model.Prices.Add(pri);

                            if (!string.IsNullOrEmpty(Request["DiscountPrice"]))
                            {
                                pri = new Price();
                                pri.IsDefault = false;
                                pri.IsDiscount = true;
                                pri.Title = "Giá đã giảm";
                                pri.DateCreated = DateTime.Now;
                                pri.Value = decimal.Parse(Request["DiscountPrice"]);
                                pri.CurrencyId = int.Parse(Request["Currency"]);
                                pri.PriceShortOrder = pri.Value;
                                if (Request["chk-inactive"] != null)
                                {
                                    bool[] chek = Request["chk-inactive"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                                    pri.Inactive = chek[0];
                                }

                                model.Prices.Add(pri);
                            }
                        }


                        // Add prices for properties...
                        var pricevals = from p in type.Properties.Where(p => p.PropertyType == PropertyType.HASPRICE)
                                        join pmv in db.PropertyMultiValues.Include("PropertyMultiValueDetail") on p.Id equals pmv.PropertyId
                                        select new Price
                                        {
                                            ArticleId = model.Id,
                                            IsDefault = false,
                                            DateCreated = DateTime.Now,
                                            PropertyMultiValueId = pmv.Id,
                                            Value = decimal.Parse(Request["Price_" + pmv.Id.ToString()]),
                                            Title = pmv.PropertyMultiValueDetail[1].Value,
                                            CurrencyId = int.Parse(Request["Currency_" + pmv.Id.ToString()])
                                        };
                        var prices = pricevals.ToList();
                        prices.AddRange(model.Prices);
                        model.Prices = prices;

                        if (form["discounts"] != null)
                        {
                            var lst = db.Discounts.ToList();
                            var dis = form["discounts"].Split(new char[] { ',' });
                            var discount = lst.Where(c => dis.Contains(c.Id.ToString())).ToList();
                            lstDis.AddRange(discount);
                        }
                    }
                    model.Discounts = lstDis;

                    db.Articles.Add(model);
                    db.SaveChanges();
                }
            }
            return Json(new { Status = 0, Message = "OK" });
        }

        public ActionResult Edit(int id, string returnUrl)
        {
            using (DataContext db = new DataContext())
            {

                var model = db.Articles.Include(a => a.PropertyMultiValues.Select(p => p.PropertyMultiValueDetail)).Include(a => a.Categories).Include(a => a.Prices.Select(p => p.Currency)).Include(a => a.PropertyValues).Include(a => a.ArticleDetail.Select(b => b.Body)).Include(a => a.Discounts).FirstOrDefault(a => a.Id == id);
                var missing = SiteConfig.Languages.Where(l => !model.ArticleDetail.Any(cd => cd.LanguageId == l.Id)).Select(l => l.Id).ToList();
                missing.ForEach(m => { model.ArticleDetail.Add(new ArticleDetail { LanguageId = m }); });
                ViewBag.ArticleType = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail)).Include(ct => ct.CategoryTypes.Select(ctd => ctd.CategoryTypeDetail))
                    .Include(a => a.ArticleTypeDetail)
                    .Include(a => a.Properties.Select(p => p.PropertyDetail))
                    .SingleOrDefault(t => t.Id == model.ArticleTypeId);

                var has_brand = db.ArticleTypes.FirstOrDefault(a => a.Code == "BRAND");
                if (has_brand != null)
                {
                    ViewBag.ArticleBrand = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail))
                    .Include("ArticleTypeDetail")
                    .Include("Properties.PropertyDetail")
                    .SingleOrDefault(t => t.Id == has_brand.Id);
                }
                var has_groupsx = db.ArticleTypes.FirstOrDefault(a => a.Code == "GROUPPRODUCT");
                if (has_groupsx != null)
                {
                    ViewBag.ArticleGroupsx = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail))
                    .Include("ArticleTypeDetail")
                    .Include("Properties.PropertyDetail")
                    .SingleOrDefault(t => t.Id == has_groupsx.Id);
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string returnUrl, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Articles.Include(a => a.PropertyMultiValues).Include(a => a.Categories).Include(a => a.ArticleDetail.Select(b => b.Body)).Include(a => a.PropertyValues.Select(p => p.Property)).Include(a => a.ArticleFiles).Include(a => a.Prices).Include(a => a.Discounts).FirstOrDefault(a => a.Id == id);
                TryUpdateModel(model);
                ArticleType type = db.ArticleTypes.Include(ct => ct.CategoryTypes).Include(at => at.Categories.Select(c => c.CategoryDetail)).Include(at => at.Properties).SingleOrDefault(t => t.Id == model.ArticleTypeId);

                type.Properties.Where(p => !p.MultiLanguage).ToList().ForEach(
                    p =>
                    {
                        var pa = model.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id);
                        if (pa != null)
                        {
                            // Related Article for check, using property
                            if (pa.Property.Code.Equals("OTHERARTICLE") && form["relatedArticles"] != null)
                            {
                                model.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id).Value = form["relatedArticles"];
                            }
                            else
                            {
                                model.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id).Value = Request["P_" + p.Id.ToString()];
                            }
                        }

                        else
                            model.PropertyValues.Add(new ArticlePropertyValue { PropertyId = p.Id, Value = Request["P_" + p.Id.ToString()] });
                    });

                type.Properties.Where(p => p.MultiLanguage).ToList().ForEach(
                      p =>
                      {
                          var pam = model.PropertyValues.Where(pv => pv.PropertyId == p.Id);
                          if (pam.Count() > 0)
                              model.PropertyValues.Where(pv => pv.PropertyId == p.Id).ToList().ForEach(pv1 => pv1.Value = Request["P_" + pv1.PropertyId.ToString() + "_" + pv1.LanguageId.ToString()]);
                          else
                          {
                              foreach (var lang in SiteConfig.Languages)
                              {
                                  model.PropertyValues.Add(new ArticlePropertyValue { LanguageId = lang.Id, PropertyId = p.Id, Value = Request["P_" + p.Id.ToString() + "_" + lang.Id.ToString()] });
                              }
                          }
                      });

                //type.Properties.Where(p => p.MultiLanguage).ToList().ForEach(
                //       p => model.PropertyValues.Where(pv => pv.PropertyId == p.Id).ToList().ForEach(pv1 => pv1.Value = Request["P_" + pv1.PropertyId.ToString() + "_" + pv1.LanguageId.ToString()]));


                //var multivals = from p in type.Properties.Where(p => p.MultiLanguage && p.PropertyType != PropertyType.MULTICHOICE && p.PropertyType != PropertyType.CHOICE)
                //                join l in SiteConfig.Languages on 1 equals 1
                //                select new ArticlePropertyValue() { LanguageId = l.Id, PropertyId = p.Id, Value = Request[string.Format("ArticleDetail[{0}].P_{1}_{2}", (l.Id - 1).ToString(), p.Id.ToString(), l.Id.ToString())] };


                //---Update Price for PropertyMultiValues---
                type.Properties.Where(p => p.PropertyType == PropertyType.HASPRICE).ToList().ForEach(
                    p =>
                    {
                        p.PropertyMultiValues.ToList().ForEach(
                            pmv =>
                            {
                                if (!string.IsNullOrEmpty(Request["Price_" + pmv.Id.ToString()]) && !string.IsNullOrEmpty(Request["Currency_" + pmv.Id.ToString()]))
                                {
                                    if (model.Prices.FirstOrDefault(pv => pv.PropertyMultiValueId == pmv.Id) != null)
                                    {
                                        decimal price_p = 0.0M;
                                        decimal.TryParse(Request["Price_" + pmv.Id.ToString()], out price_p);
                                        model.Prices.FirstOrDefault(pv => pv.PropertyMultiValueId == pmv.Id).Value = price_p;
                                        model.Prices.FirstOrDefault(pv => pv.PropertyMultiValueId == pmv.Id).CurrencyId = int.Parse(Request["Currency_" + pmv.Id.ToString()]);
                                    }
                                    else
                                        model.Prices.Add(new Price { ArticleId = model.Id, PropertyMultiValueId = pmv.Id, CurrencyId = int.Parse(Request["Currency_" + pmv.Id.ToString()]), Value = decimal.Parse(Request["Price_" + pmv.Id.ToString()]), Title = pmv.PropertyMultiValueDetail[1].Value, IsDefault = false, DateCreated = DateTime.Now });
                                }
                            }
                            );
                    }
                    );
                //---End update----

                List<Category> lstCate = new List<Category>();
                if (type.HasCategory && form["categories"] != null)
                {
                    var cats = form["categories"].Split(new char[] { ',' });
                    var categories = type.Categories.Where(c => cats.Contains(c.Id.ToString())).ToList();
                    lstCate.AddRange(categories);
                }

                ArticleType has_brand = db.ArticleTypes.Include(ct => ct.CategoryTypes).Include(at => at.Categories.Select(c => c.CategoryDetail)).FirstOrDefault(a => a.Code == "BRAND");
                if (has_brand != null && form["categories_b"] != null)
                {
                    var cats = form["categories_b"].Split(new char[] { ',' });
                    ArticleType t_brand = db.ArticleTypes.Include(at => at.Categories).Where(t => t.Id == has_brand.Id).Single();
                    var categories = t_brand.Categories.Where(c => cats.Contains(c.Id.ToString())).ToList();
                    lstCate.AddRange(categories);
                }

                ArticleType has_groupsx = db.ArticleTypes.FirstOrDefault(a => a.Code == "GROUPPRODUCT");
                if (has_groupsx != null && form["categories2"] != null)
                {
                    var catsx = form["categories2"].Split(new char[] { ',' });
                    ArticleType t_groupsx = db.ArticleTypes.Include(at => at.Categories).Where(t => t.Id == has_groupsx.Id).Single();
                    var categories = t_groupsx.Categories.Where(c => catsx.Contains(c.Id.ToString())).ToList();
                    lstCate.AddRange(categories);
                }

                foreach (var item in type.CategoryTypes)
                {
                    var cate = db.Categories.Where(c => c.CategoryTypeId == item.Id).ToList();
                    if (form["cate_bycatetype"] != null)
                    {
                        var cats = form["cate_bycatetype"].Split(new char[] { ',' });
                        var categories = cate.Where(c => cats.Contains(c.Id.ToString())).ToList();
                        lstCate.AddRange(categories);
                    }
                }
                model.Categories = lstCate;

                List<PropertyMultiValue> lstPmv = new List<PropertyMultiValue>();
                var proMultiVal = db.PropertyMultiValues.ToList();

                if (form["pmv_multichoice"] != null)
                {
                    var pmvs = form["pmv_multichoice"].Split(new char[] { ',' });
                    var pros = proMultiVal.Where(p => pmvs.Contains(p.Id.ToString())).ToList();
                    lstPmv.AddRange(pros);
                }

                if (form["pmv_choice"] != null)
                {
                    var pmvs = form["pmv_choice"].Split(new char[] { ',' });
                    var pros = proMultiVal.Where(p => pmvs.Contains(p.Id.ToString())).ToList();
                    lstPmv.AddRange(pros);
                }
                model.PropertyMultiValues = lstPmv;



                var details = from d in SiteConfig.Languages
                              select new ArticleDetail()
                              {
                                  LanguageId = d.Id,

                                  ArticleName = Request[string.Format("ArticleDetail[{0}].ArticleName", d.Id)],
                                  ShortDesc = Request[string.Format("ArticleDetail[{0}].ShortDesc", d.Id)],
                                  Description = Request[string.Format("ArticleDetail[{0}].Description", d.Id)],
                                  SEOKeywords = HTMLHelper.KeywordsPrepare(Request[string.Format("ArticleDetail[{0}].SEOKeywords", d.Id)]),
                                  SEODescription = Request[string.Format("ArticleDetail[{0}].SEODescription", d.Id)],
                                  UrlFriendly = Unichar.UnicodeStrings.UrlString(Request[string.Format("ArticleDetail[{0}].UrlFriendly", d.Id)])
                                     
                                  };
                    
                    db.Entry(model).Collection(m => m.Keywords).Load();


                    (from d in SiteConfig.Languages
                     select (Request[string.Format("ArticleDetail[{0}].SEOKeywords", d.Id)] ?? "").Split(new char[] { ',' })
                     .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Unichar.UnicodeStrings.LatinToAscii(s)).ToArray())
                                                   .ToList()
                                                   .ForEach(kwa =>
                                                   {
                                                       var keywords = db.Keywords.Where(k => kwa.Contains(k.Tag)).ToList();
                                                       var newKeywords = kwa.Where(s => !keywords.Exists(k => k.Tag == s)).Select(s => new Keyword { Tag = s }).ToList();
                                                       model.Keywords = model.Keywords.Concat(keywords.Concat(newKeywords.AsEnumerable<Keyword>())).ToList();
                                                   });

                    
                    model.ArticleDetail = new VList<ArticleDetail>();
                    model.ArticleDetail.AddRange(details);


                if (type.Code == "PRODUCT")
                {
                    var temp = "";
                    var cd3 = "";
                    var name = model.ArticleName.Replace(",", ".").Trim();
                    var masp = model.PropertyValues.FirstOrDefault(v => v.PropertyId == 6).Value.Replace(",", ".").Trim();
                    var countnhomsp = model.Categories.Where(ed => ed.ArticleTypeId == 2).ToList();
                    var countbrand = model.Categories.Where(eds => eds.ArticleTypeId == 5).ToList();

                    if (masp == "" && countbrand.Count == 0 && countnhomsp.Count == 0)
                    {
                        temp = name;
                    }
                    else
                    {
                        if (masp != "")
                        {

                            if (countbrand.Count == 0 && countnhomsp.Count == 0)
                            {
                                temp = name + "," + masp;
                            }
                            else if (countbrand.Count > 0 && countnhomsp.Count == 0)
                            {

                                var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;

                                cd3 = brand + " " + masp;
                                temp = name + "," + cd3 + "," + masp;


                            }
                            else if (countbrand.Count == 0 && countnhomsp.Count > 0)
                            {
                                if (countnhomsp.Count == 1)
                                {
                                    var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom1 + "," + masp;
                                }
                                else
                                {
                                    var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom2 + "," + masp;
                                }
                            }
                            else
                            {
                                var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;
                                cd3 = brand + " " + masp;
                                if (countnhomsp.Count == 1)
                                {
                                    var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom1 + "," + cd3 + "," + masp;
                                }
                                else
                                {
                                    var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom2 + "," + cd3 + "," + masp;
                                }
                            }

                        }
                        else
                        {
                            if (countbrand.Count > 0 && countnhomsp.Count == 0)
                            {
                                var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;

                                temp = name + "," + brand;
                            }
                            else if (countbrand.Count == 0 && countnhomsp.Count > 0)
                            {
                                if (countnhomsp.Count == 1)
                                {
                                    var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom1;
                                }
                                else
                                {
                                    var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom2;
                                }
                            }
                            else
                            {
                                var brand = model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).CategoryName;

                                if (countnhomsp.Count == 1)
                                {

                                    var nhom1 = model.Categories.FirstOrDefault(e => e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom1 + "," + brand;
                                }
                                else
                                {
                                    var nhom2 = model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).CategoryName;
                                    temp = name + "," + nhom2 + "," + brand;
                                }
                            }
                        }

                    }

                     //var nhom2 = db.Categories.FirstOrDefault(c => c.Id == model.Categories.FirstOrDefault(e => e.ParentId > 0 && e.ArticleTypeId == 2).Id).CategoryName;
                     //var brand = db.Categories.FirstOrDefault(w =>w.Id == model.Categories.FirstOrDefault(h => h.ArticleTypeId == 5).Id).CategoryName;

                     (from d in SiteConfig.Languages
                      select (temp).Split(new char[] { ',' })
                      .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Unichar.UnicodeStrings.LatinToAscii(s).Replace("/", " ").Replace(",", ".")).ToArray())
                                                    .ToList()
                                                    .ForEach(kwa =>
                                                    {
                                                        var keywords = db.Keywords.Where(k => kwa.Contains(k.Tag)).ToList();
                                                        var newKeywords = kwa.Where(s => !keywords.Exists(k => k.Tag == s)).Select(s => new Keyword { Tag = s }).ToList();
                                                        model.Keywords = model.Keywords.Concat(keywords.Concat(newKeywords.AsEnumerable<Keyword>())).ToList();
                                                    });
                    
                    var dt = model.ArticleDetail.Select(d => new ArticleDetail
                    {
                        LanguageId = d.LanguageId,

                        ArticleName = d.ArticleName.Trim(),
                        ShortDesc = d.ShortDesc,
                        Description = d.Description,
                        SEOKeywords = temp,
                        SEODescription = d.SEODescription,
                        UrlFriendly = d.UrlFriendly
                    });
                    model.ArticleDetail = new VList<ArticleDetail>();
                    model.ArticleDetail.AddRange(dt);
                }





                if (type.Code != "ADV")
                    model.DateUpdated = DateTime.Now;


                var typeName = type.Code.ToLower().ToString();
                typeName = typeName.First().ToString().ToUpper() + String.Join("", typeName.Skip(1));

                if (model.Prices == null) model.Prices = new List<Price>();
                if (model.ArticleFiles == null) model.ArticleFiles = new List<ArticleFile>();
                if (type.HasImage && Request.Files.Count > 0)
                {
                    var i = 0;
                    var filenames = form["Upload"].Split(',');

                    var imagefiles = Enumerable.Range(0, Request.Files.Count)
                              .Where(f => string.Equals("multiple_file", Request.Files.GetKey(f), StringComparison.InvariantCultureIgnoreCase))
                              .Select(Request.Files.Get);
                    foreach (HttpPostedFileBase file in imagefiles)
                    {
                        if (file.ContentLength > 0)
                        {
                            if (filenames.Contains(file.FileName))
                            {
                                filenames = filenames.Where(s => !s.Equals(file.FileName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                                var af = Uploader.Upload(file, ArticleFileType.IMAGE, typeName, model.ArticleDetail.FirstOrDefault().UrlFriendly);
                                var afd = from d in SiteConfig.Languages
                                          select new ArticleFileDetail()
                                          {
                                              LanguageId = d.Id,
                                              Title = Request[string.Format("ArticleDetail[{0}].ArticleName", d.Id)],
                                          };
                                af.DateCreated = DateTime.Now;
                                af.SortOrder = 0;

                                af.ArticleFileDetail = new VList<ArticleFileDetail>();
                                if (i == 0)
                                {
                                    af.IsDefault = true;
                                    model.ArticleFiles.ToList().ForEach(f => { f.IsDefault = false; });
                                    model.ImageUrl = af.FileName;
                                }
                                af.ArticleFileDetail.AddRange(afd);
                                model.ArticleFiles.Add(af);
                                i++;
                            }
                        }
                    }
                }

                if (type.HasAttachment & Request.Files.Count > 0)
                {
                    var file = Request.Files["Attachment"];
                    if (file.ContentLength > 0)
                    {
                        var af = Uploader.Upload(file, ArticleFileType.ATTACHMENT, typeName);
                        var afd = from d in SiteConfig.Languages
                                  select new ArticleFileDetail()
                                  {
                                      LanguageId = d.Id,
                                      Title = Request[string.Format("ArticleDetail[{0}].ArticleName", d.Id)],
                                  };
                        af.DateCreated = DateTime.Now;
                        af.SortOrder = 0;

                        af.ArticleFileDetail = new VList<ArticleFileDetail>();
                        af.ArticleFileDetail.AddRange(afd);
                        model.ArticleFiles.Add(af);
                    }
                }
                List<Discount> lstDis = new List<Discount>();

                if (type.HasPrice)
                {
                    if (!string.IsNullOrEmpty(Request["Price"]))
                    {
                        var pris = model.Prices.Where(prs => prs.IsDefault).FirstOrDefault();
                        if (pris == null)
                        {
                            pris = new Price();
                            pris.IsDefault = true;
                            pris.Title = "Giá chính";
                            pris.DateCreated = DateTime.Now;
                        }
                        pris.Value = decimal.Parse(Request["Price"]);
                        pris.CurrencyId = int.Parse(Request["currencyId"]);
                        pris.PriceShortOrder = pris.Value;
                        if (Request["chk-inactive"] != null)
                        {
                            bool[] chek = Request["chk-inactive"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                            pris.Inactive = chek[0];
                        }
                        model.Prices.Add(pris);
                        if (!string.IsNullOrEmpty(Request["DiscountPrice"]))
                        {
                            pris = model.Prices.Where(prs => prs.IsDiscount).FirstOrDefault();
                            if (pris == null)
                            {
                                pris = new Price();
                                pris.IsDefault = false;
                                pris.IsDiscount = true;
                                pris.Title = "Giá đã giảm";
                                pris.DateCreated = DateTime.Now;
                            }
                            pris.Value = decimal.Parse(Request["DiscountPrice"]);
                            pris.CurrencyId = int.Parse(Request["currencyId"]);
                            pris.PriceShortOrder = pris.Value;
                            if (Request["chk-inactive"] != null)
                            {
                                bool[] chek = Request["chk-inactive"].Split(new char[] { ',' }).Select(s => bool.Parse(s)).ToArray();
                                pris.Inactive = chek[0];
                            }
                            model.Prices.Add(pris);
                        }
                    }

                    if (form["discounts"] != null)
                    {
                        var lst = db.Discounts.ToList();
                        var dis = form["discounts"].Split(new char[] { ',' });
                        var discount = lst.Where(c => dis.Contains(c.Id.ToString())).ToList();
                        lstDis.AddRange(discount);
                    }
                }
                model.Discounts = lstDis;

                db.SaveChanges();
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("List", new { id = model.ArticleTypeId });
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.Articles.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        [HttpPost]
        public ActionResult CopyToType(string id, int typeid)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                var lst = db.Articles.Include(a => a.ArticleDetail.Select(b => b.Body))
                    .Include(a => a.ArticleFiles.Select(f => f.ArticleFileDetail))
                    .AsNoTracking().Where(a => ids.Contains(a.Id)).ToList();

                var filePath = Path.Combine(SiteConfig.UploadFolder, db.ArticleTypes.FirstOrDefault(t => t.Id == typeid).Code);
                lst.ForEach(a =>
                {
                    a.ArticleTypeId = typeid;
                    a.ArticleFiles.ToList().ForEach(f =>
                    {
                        var ext = Path.GetExtension(f.FileName);
                        var newF = Guid.NewGuid() + ext;
                        var newP = Path.Combine(Path.GetDirectoryName(f.FullPath), newF);
                        if (System.IO.File.Exists(f.FullPath))
                        {
                            System.IO.File.Copy(f.FullPath, newP);
                            f.FullPath = newP;
                            f.FileName = Path.Combine(filePath, newF).Replace("\\", "/");
                            if (f.IsDefault)
                                a.ImageUrl = f.FileName;
                        }
                    });
                    db.Articles.Add(a);
                });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        [HttpPost]
        public ActionResult Copy(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                
                var lst = db.Articles.Include(a => a.ArticleDetail.Select(b => b.Body))
                    .Include(a => a.Categories)
                    .Include(a => a.PropertyValues)
                    .Include(a => a.Prices)
                    .Include(a => a.ArticleFiles.Select(f => f.ArticleFileDetail))
                    .AsNoTracking().Where(a => ids.Contains(a.Id)).ToList();
                List<Category> categories = null;
                var typeid = lst.FirstOrDefault().ArticleTypeId;
                var filePath = Path.Combine(SiteConfig.UploadFolder, db.ArticleTypes.FirstOrDefault(t => t.Id == typeid).Code);
                lst.ForEach(a =>
                {
                a.ArticleDetail.ToList().ForEach(d =>
                d.ArticleName = d.ArticleName + "copy"
                    ); 
                    a.ArticleFiles.ToList().ForEach(f =>
                    {
                        var ext = Path.GetExtension(f.FileName);
                        var newF = Guid.NewGuid() + ext;
                        var newP = Path.Combine(Path.GetDirectoryName(f.FullPath), newF);
                        if (System.IO.File.Exists(f.FullPath))
                        {
                            System.IO.File.Copy(f.FullPath, newP);
                            f.FullPath = newP;
                            f.FileName = Path.Combine(filePath, newF).Replace("\\", "/");
                            if (f.IsDefault)
                                a.ImageUrl = f.FileName;
                        }
                    });

                    if (categories == null)
                    {
                        categories = db.Categories.Where(c => c.ArticleTypeId == a.ArticleTypeId).ToList();
                    }
                    a.Categories = categories.Where(c => a.Categories.Select(ac => ac.Id).ToList().Contains(c.Id)).ToList();
                    db.Articles.Add(a);
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
                var list = db.Articles.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.SortOrder = orders[ids.IndexOf(c.Id)]; });
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
                var articles = (from d in jsonData["items"]
                                select new Article { Id = d["id"].Value<int>(), IsNew = d["isNew"].Value<bool>(), IsHot = d["isHot"].Value<bool>(), IsMostView = d["isMostView"].Value<bool>(), InActive = d["inActive"].Value<bool>(), FullWidth = d["isFullWith"].Value<bool>() }
                           ).ToList();
                var ids = articles.Select(a => a.Id).ToList();
                var list = db.Articles.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(a =>
                {
                    var item = articles.FirstOrDefault(d => d.Id == a.Id);
                    if (item != null)
                    {
                        a.IsHot = item.IsHot;
                        a.IsNew = item.IsNew;
                        a.IsMostView = item.IsMostView;
                        a.InActive = item.InActive;
                        a.FullWidth = item.FullWidth;
                    }
                });

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult FBPost(int id)
        {
            var article = Article.GetById(id, SiteConfig.LanguageId);
            if (article == null)
                return Json(new { Status = -1, Message = "Error" });
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            var mesage = html.Raw(VNS.Web.Helpers.HTMLHelper.GetInnerText(!string.IsNullOrWhiteSpace(article.ShortDescription) ? article.ShortDescription : article.Description)).ToString();

            var description = html.Raw(VNS.Web.Helpers.HTMLHelper.GetInnerText(!string.IsNullOrWhiteSpace(article.SEODescription) ? article.SEODescription : mesage.ToString())).ToString();

            var msg = new
            {
                Title = article.ArticleName,
                Description = description,
                Caption = SiteConfig.SiteInfo.DefaultDomain.Replace("www.", "").ToUpper(),
                Message = mesage,
                Link = "http://" + Request.Url.Host.ToString() + Url.Action("Detail", "Article", new { id = article.Id, code = article.ArticleType.UrlFriendly, title = Unichar.UnicodeStrings.UrlString(article.ArticleName), area = "" }).ToString(),
                ImageUrl = article.ImageUrl != null ? ("http://" + Request.Url.Host.ToString() + article.ImageUrl) : ""
            };

            var jsonData = JsonConvert.SerializeObject(msg);

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            client.Headers[HttpRequestHeader.Referer] = Request.Url.AbsoluteUri;
            var ret = client.UploadData("https://autopost.viennam.info/api/post", "POST", Encoding.UTF8.GetBytes(jsonData));
            var result = Encoding.UTF8.GetString(ret);

            return Content(result);
        }
        public ActionResult FBPostEdit(int id)
        {
            var article = Article.GetById(id, SiteConfig.LanguageId);
            if (article == null)
                return Content("");
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            var mesage = html.Raw(VNS.Web.Helpers.HTMLHelper.GetInnerText(!string.IsNullOrWhiteSpace(article.ShortDescription) ? article.ShortDescription : article.Description)).ToString();

            var description = html.Raw(VNS.Web.Helpers.HTMLHelper.GetInnerText(!string.IsNullOrWhiteSpace(article.SEODescription) ? article.SEODescription : mesage.ToString())).ToString();
            AutoPostModels model = new AutoPostModels()
            {
                Title = article.ArticleName,
                Message = mesage,
                Description = description,
                Caption = SiteConfig.SiteInfo.DefaultDomain.Replace("www.", "").ToUpper(),
                Link = "http://" + Request.Url.Host.ToString() + Url.Action("Detail", "Article", new { id = article.Id, code = article.ArticleType.UrlFriendly, title = Unichar.UnicodeStrings.UrlString(article.ArticleName), area = "" }).ToString(),
                ImageUrl = article.ImageUrl != null ? ("http://" + Request.Url.Host.ToString() + article.ImageUrl) : ""
            };

            return PartialView("AutoPost", model);
        }
        [HttpPost]
        public ActionResult FBPostEdit(AutoPostModels model)
        {
            var msg = new
            {
                Title = model.Title,
                Description = model.Description,
                Caption = model.Caption,
                Message = model.Message,
                Link = model.Link,
                ImageUrl = model.ImageUrl
            };

            var jsonData = JsonConvert.SerializeObject(msg);

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            client.Headers[HttpRequestHeader.Referer] = Request.Url.AbsoluteUri;
            var ret = client.UploadData("https://autopost.viennam.info/api/post", "POST", Encoding.UTF8.GetBytes(jsonData));
            var result = Encoding.UTF8.GetString(ret);

            return Content(result);
        }

        public ActionResult Import(int id)
        {
            using (DataContext db = new DataContext())
            {

                ViewBag.ArticleType = db.ArticleTypes.Include(at => at.Categories.Select(c => c.CategoryDetail)).Include(at => at.Properties.Select(p => p.PropertyDetail)).Include(ct => ct.CategoryTypes.Select(ctd => ctd.CategoryTypeDetail)).Include(a => a.ArticleTypeDetail).SingleOrDefault(t => t.Id == id);
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Import(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                //List<Locale> articlelist = new List<Locale>();
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                //var catted = Convert.ToInt32(Request["catedm"]);
                if (Request.Files["Import"] != null)
                {
                    #region Upload file
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
                        var type = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Include(a => a.Categories.Select(c => c.CategoryDetail)).Include(a => a.Properties).Where(a => a.Id == id).FirstOrDefault();

                        int currid = 0;
                        if (type.HasPrice)
                            currid = db.Currencies.FirstOrDefault(a => a.IsDefault) != null ? db.Currencies.FirstOrDefault(a => a.IsDefault).Id : 0;

                        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                        {
                            ExcelWorkbook wkb = xlPackage.Workbook;
                            if (wkb != null)
                            {
                                for (int i = 1; i < wkb.Worksheets.Count + 1; i++)
                                {
                                    int startRow = 6;
                                    ExcelWorksheet currentworksheet = wkb.Worksheets[i];
                                    if (!currentworksheet.Cells["A5"].Text.Equals("ID") ||
                                        !currentworksheet.Cells["B5"].Text.Equals("Tên sản phẩm") ||
                                        !currentworksheet.Cells["C5"].Text.Equals("Mã sản phẩm") ||
                                        !currentworksheet.Cells["D5"].Text.Equals("Thứ tự") ||
                                        !currentworksheet.Cells["E5"].Text.Equals("Số lượng") ||
                                        !currentworksheet.Cells["F5"].Text.Equals("Giá") ||
                                        !currentworksheet.Cells["G5"].Text.Equals("Thời gian đặt hàng") ||
                                        !currentworksheet.Cells["H5"].Text.Equals("Nhà sản xuất"))


                                        for (int rowNumber = startRow; rowNumber <= currentworksheet.Dimension.End.Row; rowNumber++)
                                        {
                                            int col = 1;
                                            int productid = 0;
                                            if (!string.IsNullOrWhiteSpace(currentworksheet.Cells[rowNumber, col].Text))
                                                productid = int.Parse(currentworksheet.Cells[rowNumber, col++].Text);
                                            else col += 1;

                                            var article = db.Articles.Include(a => a.ArticleDetail.Select(b => b.Body)).Include(a => a.ArticleType.ArticleTypeDetail).Include(a => a.Categories.Select(e => e.CategoryDetail)).Include(a => a.Prices.Select(p => p.Currency)).Include(a => a.PropertyValues.Select(p => p.Property)).Include(a => a.Discounts).Where(a => a.Id == productid).FirstOrDefault();

                                            string articlename = currentworksheet.Cells[rowNumber, col].Text;
                                            if (string.IsNullOrWhiteSpace(articlename))
                                                continue;

                                            if (article != null)
                                            {
                                                #region Update Locale
                                                foreach (var lg in SiteConfig.Languages)
                                                {
                                                    article.ArticleDetail[lg.Id].ArticleName = currentworksheet.Cells[rowNumber, col].Text;
                                                    article.ArticleDetail[lg.Id].UrlFriendly = Unichar.UnicodeStrings
                                                                  .UrlString(currentworksheet.Cells[rowNumber, col].Text);
                                                    col++;
                                                }
                                                #region Property
                                                type.Properties.Where(p => !p.MultiLanguage).ToList()
                                                    .ForEach(p =>
                                                    {
                                                        if (p.Code.Equals("ID", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            var pr = article.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id);

                                                            if (pr != null)
                                                                article.PropertyValues
                                                                    .FirstOrDefault(pv => pv.PropertyId == p.Id)
                                                                    .Value = currentworksheet.Cells[rowNumber, col++].Text;
                                                            else
                                                            {
                                                                article.PropertyValues.Add(new ArticlePropertyValue
                                                                {
                                                                    PropertyId = p.Id,
                                                                    Value = currentworksheet.Cells[rowNumber, col++].Text
                                                                });
                                                            }
                                                        }
                                                        else if (p.Code.Equals("CODE", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            var weight = article.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id);

                                                            if (weight != null)
                                                                article.PropertyValues
                                                                    .FirstOrDefault(pv => pv.PropertyId == p.Id)
                                                                    .Value = currentworksheet.Cells[rowNumber, col++].Text;
                                                            else
                                                            {
                                                                article.PropertyValues.Add(new ArticlePropertyValue
                                                                {
                                                                    PropertyId = p.Id,
                                                                    Value = currentworksheet.Cells[rowNumber, col++].Text
                                                                });
                                                            }
                                                        }
                                                        else if (p.Code.Equals("WEIGHT", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            var weight = article.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id);

                                                            if (weight != null)
                                                                article.PropertyValues
                                                                    .FirstOrDefault(pv => pv.PropertyId == p.Id)
                                                                    .Value = currentworksheet.Cells[rowNumber, col++].Text;
                                                            else
                                                            {
                                                                article.PropertyValues.Add(new ArticlePropertyValue
                                                                {
                                                                    PropertyId = p.Id,
                                                                    Value = currentworksheet.Cells[rowNumber, col++].Text
                                                                });
                                                            }
                                                        }
                                                        else if (p.Code.Equals("BARCODE", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            var barcode = article.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id);
                                                            if (barcode != null)
                                                                article.PropertyValues
                                                                    .FirstOrDefault(pv => pv.PropertyId == p.Id)
                                                                    .Value = currentworksheet.Cells[rowNumber, col++].Text;
                                                            else
                                                            {
                                                                article.PropertyValues.Add(new ArticlePropertyValue
                                                                {
                                                                    PropertyId = p.Id,
                                                                    Value = currentworksheet.Cells[rowNumber, col++].Text
                                                                });
                                                            }
                                                        }
                                                        else if (p.Code.Equals("PRICEBUY", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            var pricebuy = article.PropertyValues.FirstOrDefault(pv => pv.PropertyId == p.Id);
                                                            var price = System.Text.RegularExpressions.Regex.Replace(currentworksheet.Cells[rowNumber, col++].Text, "[^0-9]+", "");

                                                            if (pricebuy != null)
                                                                article.PropertyValues
                                                                    .FirstOrDefault(pv => pv.PropertyId == p.Id)
                                                                    .Value = price;
                                                            else
                                                            {
                                                                article.PropertyValues.Add(new ArticlePropertyValue
                                                                {
                                                                    PropertyId = p.Id,
                                                                    Value = price
                                                                });
                                                            }
                                                        }
                                                    });
                                                #endregion

                                                ////Thu tu
                                                if (!string.IsNullOrWhiteSpace(currentworksheet.Cells[rowNumber, col].Text))
                                                    article.SortOrder = int.Parse(currentworksheet.Cells[rowNumber, col++].Text);
                                                else
                                                {
                                                    article.SortOrder = 0;
                                                    col += 1;
                                                }

                                                if (type.HasPrice)
                                                {
                                                    if (type.HasNumberProduct)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(currentworksheet.Cells[rowNumber, col].Text))
                                                            article.Number = int.Parse(currentworksheet.Cells[rowNumber, col++].Text);
                                                        else
                                                        {
                                                            article.Number = 0;
                                                            col += 1;
                                                        }
                                                    }
                                                    if (article.Prices.FirstOrDefault(a => a.IsDefault) != null)
                                                    {
                                                        if (!article.Prices.FirstOrDefault(a => a.IsDefault).Inactive)
                                                        {
                                                            var price = currentworksheet.Cells[rowNumber, col++].Text;
                                                            price = System.Text.RegularExpressions.Regex.Replace(price, "[^0-9]+", "");
                                                            if (string.IsNullOrWhiteSpace(price))
                                                                price = "0";

                                                            var pris = article.Prices.Where(prs => prs.IsDefault).FirstOrDefault();
                                                            pris.Value = decimal.Parse(price);

                                                            pris.CurrencyId = currid;
                                                            article.Prices.Add(pris);
                                                        }
                                                    }
                                                }
                                                ////Thoi gian cho
                                                var kye = Unichar.UnicodeStrings.UrlString(currentworksheet.Cells[rowNumber, col++].Text);
                                                var articles = Article.GetByTypeCode("TIMECHO", SiteConfig.LanguageId, ArticleFlags.ACTIVE, 1, 30, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.ASCENDING).List;
                                                var namechos = articles.Where(a => Unichar.UnicodeStrings
                                                                .UrlString(a.ArticleName).Equals(kye, StringComparison.OrdinalIgnoreCase));
                                                var idnamchos = namechos.FirstOrDefault();
                                                article.timeId = idnamchos.Id;


                                                ////Nhà sản xuat
                                                var namebrand = Unichar.UnicodeStrings.UrlString(currentworksheet.Cells[rowNumber, col++].Text);
                                                int cid = -1;

                                                var cate = db.Categories.Where(d => d.ArticleTypeId == 16).FirstOrDefault(c => c.CategoryDetail.Any(d => d.CategoryName.Equals(namebrand, StringComparison.OrdinalIgnoreCase)));
                                                if (cate != null)
                                                    cid = cate.Id;


                                                List<Category> listCated = new List<Category>();
                                                /////////////////danh muc//////////////////////

                                                ArticleType has_brand = db.ArticleTypes.FirstOrDefault(a => a.Code == "BRAND");
                                                if (has_brand != null)
                                                {
                                                    int cats = cid;
                                                    ArticleType t_brand = db.ArticleTypes.Include(at => at.Categories).Where(t => t.Id == has_brand.Id).Single();
                                                    var categories = t_brand.Categories.Where(c => c.Id == cats).ToList();
                                                    listCated.AddRange(categories);
                                                }

                                                if (type.HasCategory && form["categories"] != null)
                                                {
                                                    var cats = form["categories"].Split(new char[] { ',' });
                                                    var categories = type.Categories.Where(c => cats.Contains(c.Id.ToString())).ToList();
                                                    listCated.AddRange(categories);
                                                }


                                                article.Categories = listCated;


                                                article.UserCreated = SiteConfig.UserId;
                                                article.DateUpdated = DateTime.Now;
                                                db.SaveChanges();
                                                #endregion
                                            }
                                            else
                                            {
                                                col = 2;
                                                #region Insert Locale

                                                var model = new Article();
                                                //TryUpdateModel(model);
                                                model.Flags = ArticleFlags.ACTIVE;
                                                model.ArticleDetail = new VList<ArticleDetail>();
                                                model.PropertyValues = new List<ArticlePropertyValue>();
                                                List<ArticlePropertyValue> propvals = new List<ArticlePropertyValue>();

                                                //var details = from d in SiteConfig.Languages
                                                //              select new ArticleDetail()
                                                //              {
                                                //                  LanguageId = d.Id,
                                                //                  ArticleName = currentworksheet.Cells[rowNumber, col].Text,
                                                //                  UrlFriendly = Unichar.UnicodeStrings
                                                //                  .UrlString(currentworksheet.Cells[rowNumber, col++].Text)
                                                //              };
                                                //model.ArticleDetail.AddRange(details);
                                                foreach (var lg in SiteConfig.Languages)
                                                {
                                                    var detail = new ArticleDetail();
                                                    detail.Body = new ArticleDetailBody();
                                                    detail.LanguageId = lg.Id;
                                                    detail.ArticleName = currentworksheet.Cells[rowNumber, col].Text;
                                                    detail.UrlFriendly = Unichar.UnicodeStrings.UrlString(currentworksheet.Cells[rowNumber, col].Text);
                                                    col++;
                                                    model.ArticleDetail.Add(detail);
                                                }

                                                #region Property
                                                if (type.Properties.FirstOrDefault(a => a.Code.Equals("ID", StringComparison.OrdinalIgnoreCase)) != null)
                                                {
                                                    var vals = from p in type.Properties
                                                                   .Where(p => !p.MultiLanguage && p.Code == "ID")
                                                               select new ArticlePropertyValue()
                                                               {
                                                                   PropertyId = p.Id,
                                                                   Value = currentworksheet.Cells[rowNumber, col++].Text
                                                               };
                                                    propvals.AddRange(vals);
                                                }
                                                if (type.Properties.FirstOrDefault(a => a.Code.Equals("CODE", StringComparison.OrdinalIgnoreCase)) != null)
                                                {
                                                    var vals = from p in type.Properties
                                                                   .Where(p => !p.MultiLanguage && p.Code == "CODE")
                                                               select new ArticlePropertyValue()
                                                               {
                                                                   PropertyId = p.Id,
                                                                   Value = currentworksheet.Cells[rowNumber, col++].Text
                                                               };
                                                    propvals.AddRange(vals);
                                                }
                                                if (type.Properties.FirstOrDefault(a => a.Code.Equals("WEIGHT", StringComparison.OrdinalIgnoreCase)) != null)
                                                {
                                                    var vals = from p in type.Properties
                                                                   .Where(p => !p.MultiLanguage && p.Code == "WEIGHT")
                                                               select new ArticlePropertyValue()
                                                               {
                                                                   PropertyId = p.Id,
                                                                   Value = currentworksheet.Cells[rowNumber, col++].Text
                                                               };
                                                    propvals.AddRange(vals);
                                                }

                                                if (type.Properties.FirstOrDefault(a => a.Code.Equals("BARCODE", StringComparison.OrdinalIgnoreCase)) != null)
                                                {
                                                    var vals = from p in type.Properties
                                                                   .Where(p => !p.MultiLanguage && p.Code == "BARCODE")
                                                               select new ArticlePropertyValue()
                                                               {
                                                                   PropertyId = p.Id,
                                                                   Value = currentworksheet.Cells[rowNumber, col++].Text
                                                               };
                                                    propvals.AddRange(vals);
                                                }

                                                if (type.Properties.FirstOrDefault(a => a.Code.Equals("PRICEBUY", StringComparison.OrdinalIgnoreCase)) != null)
                                                {
                                                    var price = currentworksheet.Cells[rowNumber, col++].Text;
                                                    price = System.Text.RegularExpressions.Regex.Replace(price, "[^0-9]+", "");
                                                    var vals = from p in type.Properties
                                                                   .Where(p => !p.MultiLanguage && p.Code == "PRICEBUY")
                                                               select new ArticlePropertyValue()
                                                               {
                                                                   PropertyId = p.Id,
                                                                   Value = price
                                                               };
                                                    propvals.AddRange(vals);
                                                }
                                                model.PropertyValues = propvals;
                                                #endregion
                                                if (type.HasPrice)
                                                {
                                                    if (type.HasNumberProduct)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(currentworksheet.Cells[rowNumber, col].Text))
                                                            model.Number = int.Parse(currentworksheet.Cells[rowNumber, col++].Text);
                                                        else
                                                        {
                                                            model.Number = 0;
                                                            col += 1;
                                                        }
                                                    }


                                                    var price = currentworksheet.Cells[rowNumber, col++].Text;
                                                    price = System.Text.RegularExpressions.Regex.Replace(price, "[^0-9]+", "");

                                                    if (string.IsNullOrWhiteSpace(price))
                                                        price = "0";
                                                    Price pri = new Price();
                                                    pri.IsDefault = true;
                                                    pri.Title = "Giá chính";
                                                    pri.DateCreated = DateTime.Now;
                                                    pri.Value = decimal.Parse(price);
                                                    pri.CurrencyId = currid;

                                                    if (model.Prices == null)
                                                        model.Prices = new List<Price>();
                                                    model.Prices.Add(pri);
                                                }

                                                model.DateCreated = DateTime.Now;
                                                model.DateUpdated = DateTime.Now;
                                                model.ArticleTypeId = type.Id;
                                                model.SortOrder = (db.Articles.Where(a => a.ArticleTypeId == id).Max(a => a.SortOrder) ?? 0) + 1;
                                                model.UserCreated = SiteConfig.CurrentUser.UserId;
                                                model.WebsiteId = 1;
                                                model.Discounts = new List<Discount>();
                                                db.Articles.Add(model);
                                                db.SaveChanges();
                                                #endregion
                                            }
                                        }

                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else
                    return Json(new { error = html.Locale("locale_error_import_null_file").ToHtmlString() });
                return Json(new { redirect = Url.Action("List", "Article", new { id = id }) });
            }
        }

        public ActionResult ExportCateId(int id, int cateId)
        {
            var db = new DataContext();
            var type = ArticleType.GetById(id, SiteConfig.LanguageId);
            type.LanguageId = SiteConfig.LanguageId;
            string fileName = type.Name + ".xlsx";

            var listarticle = Article.GetByCategory(cateId, SiteConfig.LanguageId, ArticleFlags.ACTIVE, 1, 10000, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.ASCENDING, ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.ALL);
            //var listarticle = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType.ArticleTypeDetail).Include(c => c.Categories.Select(cd => cd.CategoryDetail)).Include(p => p.PropertyValues.Select(pv => pv.Property.PropertyDetail)).Include(a => a.Prices.Select(c => c.Currency)).Where(a => a.ArticleTypeId == type.Id).OrderBy(a => a.SortOrder).ToList();
            //var listarticle = listarticle.Where(a =>a.Categories.Any(cate))
            var filePath = Path.Combine(SiteConfig.UploadFolder, "Data");
            var physicalPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            FileInfo newFile = new FileInfo(Path.Combine(physicalPath, fileName));
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(physicalPath, fileName));
            }

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws1 = package.Workbook.Worksheets.Add(type.Name);
                CreateHeader(ws1, type, null);

                int rowIndexBegin = 6;
                int rowIndexCurrent = rowIndexBegin;

                GenerateData(ws1, rowIndexCurrent, listarticle.List);
                package.SaveAs(newFile);
            }
            var readFile = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(filePath, fileName));
            if (!System.IO.File.Exists(readFile))
                return HttpNotFound();
            return File(readFile, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult Export(int id)
        {
            var db = new DataContext();

            var type = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Include(a => a.Properties.Select(pv => pv.PropertyDetail)).Include(a => a.Categories).Include(a => a.CategoryTypes.Select(c => c.CategoryTypeDetail)).FirstOrDefault(a => a.Id == id);
            //var type = ArticleType.GetById(id,SiteConfig.LanguageId);
            type.LanguageId = SiteConfig.LanguageId;
            string fileName = type.Name + ".xlsx";

            //var listarticle = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType.ArticleTypeDetail).Include(p => p.PropertyValues.Select(pv => pv.Property.PropertyDetail)).Include(a => a.Prices.Select(c => c.Currency)).Where(a => a.ArticleTypeId == type.Id).OrderBy(a => a.SortOrder).ToList();
            var listarticle = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType.ArticleTypeDetail).Include(c => c.Categories.Select(cd => cd.CategoryDetail)).Include(p => p.PropertyValues.Select(pv => pv.Property.PropertyDetail)).Include(a => a.Prices.Select(c => c.Currency)).Where(a => a.ArticleTypeId == type.Id).OrderBy(a => a.SortOrder).ToList();

            var filePath = Path.Combine(SiteConfig.UploadFolder, "Data");
            var physicalPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            FileInfo newFile = new FileInfo(Path.Combine(physicalPath, fileName));
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(physicalPath, fileName));
            }

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws1 = package.Workbook.Worksheets.Add(type.Name);
                CreateHeader(ws1, type, null);

                int rowIndexBegin = 6;
                int rowIndexCurrent = rowIndexBegin;

                GenerateData(ws1, rowIndexCurrent, listarticle);
                package.SaveAs(newFile);
            }
            var readFile = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(filePath, fileName));
            if (!System.IO.File.Exists(readFile))
                return HttpNotFound();
            return File(readFile, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult ExportSelect(int id, string json)
        {
            var db = new DataContext();

            var type = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Include(a => a.Properties.Select(pv => pv.PropertyDetail)).Include(a => a.Categories).Include(a => a.CategoryTypes.Select(c => c.CategoryTypeDetail)).FirstOrDefault(a => a.Id == id);
            type.LanguageId = SiteConfig.LanguageId;
            string fileName = type.Name + ".xlsx";

            var filePath = Path.Combine(SiteConfig.UploadFolder, "Data");
            var physicalPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            FileInfo newFile = new FileInfo(Path.Combine(physicalPath, fileName));
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(physicalPath, fileName));
            }

            JObject jsonData = JObject.Parse("{'items':" + json + "}");
            var articles = (from d in jsonData["items"]
                            select new Locale { Id = d["id"].Value<int>() }
                       ).ToList();
            var ids = articles.Select(a => a.Id).ToList();

            //var listarticle = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType.ArticleTypeDetail).Include(p => p.PropertyValues.Select(pv => pv.Property.PropertyDetail)).Include(a => a.Prices.Select(c => c.Currency)).Where(a => a.ArticleTypeId == type.Id && ids.Contains(a.Id)).OrderBy(a => a.SortOrder).ToList();
            var listarticle = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType.ArticleTypeDetail).Include(c => c.Categories.Select(cd => cd.CategoryDetail)).Include(p => p.PropertyValues.Select(pv => pv.Property.PropertyDetail)).Include(a => a.Prices.Select(c => c.Currency)).Where(a => a.ArticleTypeId == type.Id && ids.Contains(a.Id)).OrderBy(a => a.SortOrder).ToList();
            using (ExcelPackage package = new ExcelPackage())
            {
                var ws1 = package.Workbook.Worksheets.Add(type.Name);
                CreateHeader(ws1, type, null);

                int rowIndexBegin = 6;
                int rowIndexCurrent = rowIndexBegin;

                GenerateData(ws1, rowIndexCurrent, listarticle);
                package.SaveAs(newFile);
            }
            var readFile = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(filePath, fileName));
            if (!System.IO.File.Exists(readFile))
                return Json(new { error = new { warning = "Error" } });
            return Json(new { redirect = Url.Action("DownloadPath", "ArticleFile", new { filePath = filePath, filename = fileName }) });
        }
        public ActionResult FBImport(int id)
        {
            var categories = Category.GetTree(Category.GetByType(id, SiteConfig.LanguageId));
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FBImport(int id, FormCollection form)
        {
            var posts = new PostFormModel();
            TryUpdateModel(posts, form.ToValueProvider());
            using (DataContext db = new DataContext())
            {
                var sort = db.Articles.Max(a => a.SortOrder);
                var articleType = db.ArticleTypes.Include(at => at.Categories).FirstOrDefault(at => at.Id == id);
                foreach (var item in posts.PostModels)
                {

                    var article = new Article();
                    article.ArticleDetail = VList<ArticleDetail>.Create(SiteConfig.Languages);
                    article.ArticleDetail.ForEach(d => { d.ArticleName = item.Title; d.Description = item.Message; d.UrlFriendly = Unichar.UnicodeStrings.UrlString(item.Title); });
                    article.ArticleType = articleType;
                    article.ArticleFiles = new List<ArticleFile>();
                    var af = SaveImage(item.Image, articleType, item.Title);
                    if (af != null)
                    {
                        af.IsDefault = true;
                        article.ArticleFiles.Add(af);
                        article.ImageUrl = af.FileName;
                    }

                    List<Category> lstCate = new List<Category>();
                    if (articleType.HasCategory && form["categories"] != null)
                    {
                        var cats = form["categories"].Split(new char[] { ',' });
                        var categories = articleType.Categories.Where(c => cats.Contains(c.Id.ToString())).ToList();
                        lstCate.AddRange(categories);
                        article.Categories = lstCate;
                    }


                    article.DateCreated = DateTime.Now;
                    article.DateUpdated = DateTime.Now;
                    //article.WebsiteId = SiteConfig.SiteId;
                    article.UserCreated = SiteConfig.CurrentUser.UserId;
                    article.SortOrder = ++sort;
                    db.Articles.Add(article);
                }
                db.SaveChanges();
            }
            return Json(new { status = 0 });
        }

        private ArticleFile SaveImage(string remoteFile, ArticleType articleType, string title)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    var remoteFileName = new Uri(remoteFile).GetLeftPart(UriPartial.Path);
                    var filePath = Path.Combine(SiteConfig.UploadFolder, articleType.Code);
                    var physicalPath = Server.MapPath(filePath);
                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);
                    var ext = Path.GetExtension(remoteFileName);
                    var fileName = Unichar.UnicodeStrings.UrlString(title);
                    if (System.IO.File.Exists(Path.Combine(physicalPath, fileName + ext)))
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
                    client.DownloadFile(remoteFile, Path.Combine(physicalPath, fileName));
                    ArticleFile af = new ArticleFile { FileName = Path.Combine(filePath, fileName).Replace("\\", "/"), OriginalFileName = remoteFile, FileType = ArticleFileType.IMAGE, FullPath = Path.Combine(physicalPath, fileName), DateCreated = DateTime.Now };
                    return af;
                }
            }
            catch (Exception ex)
            { }
            return null;
        }
        #region Create Excel
        private ExcelWorksheet CreateHeader(ExcelWorksheet ws1, ArticleType type, Category category)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            using (var rng = ws1.Cells["A:XFD"])
            {
                rng.Style.Font.SetFromFont(new Font("Arial", 10));
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            ///
            /// header property
            /// 
            ws1.Row(5).Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws1.Row(5).Style.Fill.BackgroundColor.SetColor(Color.Tan);
            ws1.Row(5).Style.Font.Bold = true;
            ws1.Row(5).Style.Fill.PatternType = ExcelFillStyle.Solid;

            ws1.View.FreezePanes(6, 1);
            var rowfix = 1;
            var colfix = 1;
            ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws1.Cells[5, rowfix++].Value = "ID";
            ws1.Column(colfix++).Width = 10;

            foreach (var lg in SiteConfig.Languages)
            {
                ws1.Column(colfix).Style.WrapText = true;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws1.Cells[5, rowfix++].Value = html.Locale(string.Format("Tên sản phẩm", lg.Code.ToLower())).ToHtmlString();
                ws1.Column(colfix++).Width = 35;
            }

            if (type.Properties.FirstOrDefault(a => a.Code.Equals("ID", StringComparison.OrdinalIgnoreCase)) != null)
            {
                ws1.Cells[5, rowfix++].Value = type.Properties.FirstOrDefault(a => a.Code.Equals("ID", StringComparison.OrdinalIgnoreCase)).PropertyDetail[SiteConfig.LanguageId].Name;
                ws1.Column(colfix++).Width = 15;
            }
            if (type.Properties.FirstOrDefault(a => a.Code.Equals("CODE", StringComparison.OrdinalIgnoreCase)) != null)
            {
                ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Cells[5, rowfix++].Value = type.Properties.FirstOrDefault(a => a.Code.Equals("CODE", StringComparison.OrdinalIgnoreCase)).PropertyDetail[SiteConfig.LanguageId].Name;
                ws1.Column(colfix++).Width = 18;
            }
            if (type.Properties.FirstOrDefault(a => a.Code.Equals("WEIGHT", StringComparison.OrdinalIgnoreCase)) != null)
            {
                ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Cells[5, rowfix++].Value = type.Properties.FirstOrDefault(a => a.Code.Equals("WEIGHT", StringComparison.OrdinalIgnoreCase)).PropertyDetail[SiteConfig.LanguageId].Name;
                ws1.Column(colfix++).Width = 18;
            }

            if (type.Properties.FirstOrDefault(a => a.Code.Equals("BARCODE", StringComparison.OrdinalIgnoreCase)) != null)
            {
                ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws1.Cells[5, rowfix++].Value = type.Properties.FirstOrDefault(a => a.Code.Equals("BARCODE", StringComparison.OrdinalIgnoreCase)).PropertyDetail[SiteConfig.LanguageId].Name;
                ws1.Column(colfix++).Width = 15;
            }

            if (type.Properties.FirstOrDefault(a => a.Code.Equals("PRICEBUY", StringComparison.OrdinalIgnoreCase)) != null)
            {
                ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Cells[5, rowfix++].Value = type.Properties.FirstOrDefault(a => a.Code.Equals("PRICEBUY", StringComparison.OrdinalIgnoreCase)).PropertyDetail[SiteConfig.LanguageId].Name;
                ws1.Column(colfix++).Width = 15;
            }
            //if (type.CategoryTypes.Count > 0)
            //{
            //    if (type.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")) != null)
            //    {
            //        ws1.Cells[5, rowfix++].Value = type.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")).Name;
            //        ws1.Column(colfix++).Width = 20;
            //    }
            //}

            ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws1.Cells[5, rowfix++].Value = html.Locale("Thứ tự").ToHtmlString();
            ws1.Column(colfix++).Width = 10;

            if (type.HasNumberProduct)
            {
                ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Cells[5, rowfix++].Value = html.Locale("Số lượng").ToHtmlString();
                ws1.Column(colfix++).Width = 10;
            }

            if (type.HasPrice)
            {
                ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws1.Cells[5, rowfix++].Value = html.Locale("Giá").ToHtmlString();
                ws1.Column(colfix++).Width = 15;
            }

            ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws1.Cells[5, rowfix++].Value = html.Locale("Thời gian đặt hàng").ToHtmlString();
            ws1.Column(colfix++).Width = 25;

            ws1.Cells[5, rowfix].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws1.Column(colfix).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws1.Cells[5, rowfix++].Value = html.Locale("Nhà sản xuất").ToHtmlString();
            ws1.Column(colfix++).Width = 25;

            ///
            /// Export File Info header
            /// 
            ws1.Cells["B1"].Value = SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name;
            ws1.Cells["B1"].Style.WrapText = false;
            ws1.Cells["B1"].Style.Font.Bold = true;
            ws1.Cells["B1"].Style.Font.Size = 15;

            ws1.Row(1).Height = 30;
            ws1.Row(2).Height = 15;
            ws1.Row(3).Height = 15;

            ws1.Cells["B2"].Value = html.Locale("export_date_update") + ": " + DateTime.Now.ToString();
            ws1.Cells["B3"].Value = html.Locale("export_user") + ": " + SiteConfig.CurrentUser.DisplayName;

            ws1.Cells["C2"].Value = html.Locale("export_type") + ": " + type.ArticleTypeDetail[SiteConfig.LanguageId].Name;
            //if (category != null)
            //{
            //    ws1.Cells["C3"].Value = html.Locale("export_category") + ": " + category.CategoryDetail[SiteConfig.LanguageId].CategoryName;
            //    ws1.Cells["C3"].Value = html.Locale("export_category_code") + ": " + category.Code;
            //}

            return ws1;
        }
        private ExcelWorksheet GenerateData(ExcelWorksheet ws1, int rowIndexCurrent, ICollection<Article> list)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            using (DataContext db = new DataContext())
            {
                var type = list.FirstOrDefault().ArticleType;
                foreach (var item in list)
                {
                    var col = 1;
                    item.LanguageId = SiteConfig.LanguageId;
                    ws1.Cells[rowIndexCurrent, col++].Value = item.Id;

                    foreach (var lg in SiteConfig.Languages)
                    {
                        ws1.Cells[rowIndexCurrent, col++].Value = item.ArticleDetail[lg.Id] != null ? item.ArticleDetail[lg.Id].ArticleName : null;
                    }

                    if (type.Properties.FirstOrDefault(a => a.Code.Equals("ID", StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        var pid = item.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("ID", StringComparison.OrdinalIgnoreCase));
                        ws1.Cells[rowIndexCurrent, col++].Value = pid.Value;
                    }
                    if (type.Properties.FirstOrDefault(a => a.Code.Equals("CODE", StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        var weight = item.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CODE", StringComparison.OrdinalIgnoreCase));
                        ws1.Cells[rowIndexCurrent, col++].Value = weight.Value;
                    }
                    if (type.Properties.FirstOrDefault(a => a.Code.Equals("WEIGHT", StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        var weight = item.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("WEIGHT", StringComparison.OrdinalIgnoreCase));
                        ws1.Cells[rowIndexCurrent, col++].Value = weight.Value;
                    }

                    if (type.Properties.FirstOrDefault(a => a.Code.Equals("BARCODE", StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        var weight = item.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("BARCODE", StringComparison.OrdinalIgnoreCase));
                        ws1.Cells[rowIndexCurrent, col++].Value = weight.Value;
                    }

                    if (type.Properties.FirstOrDefault(a => a.Code.Equals("PRICEBUY", StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        var weight = item.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("PRICEBUY", StringComparison.OrdinalIgnoreCase));
                        decimal price = 0;
                        decimal.TryParse(weight.Value, out price);
                        ws1.Cells[rowIndexCurrent, col++].Value = html.Amount(price);
                    }


                    ws1.Cells[rowIndexCurrent, col++].Value = item.SortOrder;

                    if (type.HasNumberProduct)
                        ws1.Cells[rowIndexCurrent, col++].Value = item.Number;
                    if (type.HasPrice)
                        if (item.DiscountPrice > 0)
                        {
                            ws1.Cells[rowIndexCurrent, col++].Value = html.DiscountPrice(item.ItemPrice);
                        }
                        else
                        {
                            ws1.Cells[rowIndexCurrent, col++].Value = html.Price(item.ItemPrice);
                        }

                    if (item.timeId > 0)
                    {

                        var articles = Article.GetByTypeCode("TIMECHO", SiteConfig.LanguageId, ArticleFlags.ACTIVE, 1, 30, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.ASCENDING).List;
                        var namechos = articles.Where(a => a.Id == item.timeId);
                        var idnamchos = namechos.FirstOrDefault();
                        ws1.Cells[rowIndexCurrent, col++].Value = idnamchos.ArticleName;
                    }
                    else
                    {
                        ws1.Cells[rowIndexCurrent, col++].Value = "";
                    }
                    //if (type.CategoryTypes.Count > 0)
                    //{
                    //    if (type.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")) != null)
                    //    {
                    //        var cate = "";
                    //        var category = db.Categories.Include(a => a.CategoryType).Include(a => a.ArticleType.ArticleTypeDetail).Include(a => a.CategoryDetail).Include(a => a.CategoryType.CategoryTypeDetail).Where(a => a.CategoryType.Id == db.CategoryTypes.FirstOrDefault(t => t.Code.Equals("BRAND")).Id && a.Articles.Where(ad => ad.Id == item.Id).FirstOrDefault() != null).ToList();
                    //        if (category.Count > 0)
                    //            category.ForEach(a =>
                    //            {
                    //                cate += a.Code + ",";
                    //            });
                    //        ws1.Cells[rowIndexCurrent, col++].Value = cate;
                    //    }
                    //}

                    //if (type.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")) != null)
                    //{
                    //    var cate_type = item.Categories.Where(c => c.CategoryTypeId == type.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")).Id).FirstOrDefault();
                    //    if (cate_type != null)
                    //        ws1.Cells[rowIndexCurrent, col++].Value = cate_type.CategoryName;
                    //    else
                    //        ws1.Cells[rowIndexCurrent, col++].Value = "";
                    //}
                    //if (db.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")) != null)
                    //{
                    //    var cate_type = item.Categories.Where(c => c.CategoryTypeId == type.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")).Id).FirstOrDefault();
                    //    if (cate_type != null)
                    //        ws1.Cells[rowIndexCurrent, col++].Value = cate_type.CategoryName;
                    //    else
                    //        ws1.Cells[rowIndexCurrent, col++].Value = "";
                    //}
                    //if (db.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")) != null)
                    //{
                    //    var cate_type = item.Categories.Where(c => c.CategoryTypeId == db.CategoryTypes.FirstOrDefault(a => a.Code.Equals("BRAND")).Id).FirstOrDefault();
                    //    if (cate_type != null)
                    //        ws1.Cells[rowIndexCurrent, col++].Value = cate_type.CategoryName;
                    //    else
                    //        ws1.Cells[rowIndexCurrent, col++].Value = "";
                    //}
                    var brandType = db.ArticleTypes.Include(a => a.ArticleTypeDetail).FirstOrDefault(a => a.Code.Equals("BRAND", StringComparison.OrdinalIgnoreCase));
                    if (item.Categories.Where(a => a.ArticleTypeId == brandType.Id).Count() > 0)
                    {
                        var brandList = item.Categories.Where(a => a.ArticleTypeId == brandType.Id).ToList();
                        var brandproduct = brandList.Skip(brandList.Count - 1).FirstOrDefault();
                        ws1.Cells[rowIndexCurrent, col++].Value = brandproduct.CategoryName;
                    }
                    rowIndexCurrent++;
                }
            }

            return ws1;
        }
        #endregion

    }
}
