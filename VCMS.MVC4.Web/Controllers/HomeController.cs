using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using VCMS.MVC4.Extensions;
using VCMS.MVC4.Web.Mailers;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Text;
using EntityFramework.Extensions;
using System.Globalization;
namespace VCMS.MVC4.Web.Controllers
{
    [HandleError]
    [VCMS.MVC4.Web.WhitespaceFilterAttribute]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var db = new DataContext();
            ViewBag.Keyword = SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].SEOKeywords;
            ViewBag.Description = SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].SEODescription;

            //var article = Article.GetByType(2, SiteConfig.LanguageId,ArticleFlags.ACTIVE, 1, 30,ArticleSortOrder.SORT_ORDER,SortDirection.ASCENDING, ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES);
            var article = db.Articles
                .Include(a => a.ArticleType.ArticleTypeDetail)
                .Include(a => a.ArticleDetail)
                .Include(a => a.PropertyValues.Select(pv => pv.Property))
                .Include(a => a.Prices.Select(c => c.Currency))
                .Where(a => a.ArticleTypeId == 2 && a.ArticleDetail.Any(d => d.LanguageId == 1))
                .OrderBy(a => a.SortOrder).Skip(0).Take(30);
            ViewBag.Slider2 = article.ToList();

           //var adv = Article.GetByTypeCode("ADVTREN", SiteConfig.LanguageId, ArticleFlags.ACTIVE, 1, 2, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.ASCENDING, ArticleIncludeFlags.ARTICLE_TYPE);
            var adv = db.Articles.Include(a => a.ArticleType).Where(a => a.ArticleTypeId == 26).OrderBy(a => a.SortOrder).Skip(0).Take(2);
            ViewBag.FAdv = adv.ToList();

            var cate = Category.GetTree(Category.GetByType(2, SiteConfig.LanguageId)).Where(a => a.Level == 0);
            ViewBag.HomeCate = cate.ToList();

            var harticles = Article.GetByCategories(cate.Select(c => c.Id).ToArray(), SiteConfig.LanguageId, ArticleFlags.ACTIVE, 8, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.ASCENDING, ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.DISCOUNTS | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES);
            ViewBag.HomeArticle = harticles.ToList();

            //var helpfull = Article.GetByTypeCode("advcuoi", SiteConfig.LanguageId, ArticleFlags.ACTIVE, 1, 1, ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection.DESCENDING, ArticleIncludeFlags.ARTICLE_TYPE);      
            //ViewBag.Helpfull = helpfull.List;

            var banner = db.Articles
                .Include(a => a.ArticleType)
                .Include(a => a.PropertyValues.Select(p => p.Property))
                .Where(a => a.ArticleTypeId == 11).OrderBy(a => a.SortOrder).Skip(0).Take(5);
            ViewBag.Banner = banner.ToList();

            var partner = db.Articles
               .Include(a => a.ArticleType)
               .Include(a => a.PropertyValues.Select(p => p.Property))
               .Where(a => a.ArticleTypeId == 8).OrderBy(a => a.SortOrder);

            ViewBag.Partner = partner.ToList();

            //var homenewsgrid = db.Articles
            //   .Include(a => a.ArticleType)
            //   .Include(a => a.ArticleDetail)
            //   .Where(a => a.ArticleTypeId == 1 && a.ArticleDetail.Any(d => d.LanguageId == 1)).OrderBy(a => a.SortOrder).Skip(0).Take(10);
            var homenewsgrid = Article.GetByType(1, SiteConfig.LanguageId,ArticleFlags.ACTIVE, 1, 10,ArticleSortOrder.SORT_ORDER,SortDirection.ASCENDING, ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.ARTICLE_DETAIL);
            ViewBag.Homenewsgrid = homenewsgrid.List;

            var advduoi = db.Articles
               .Include(a => a.ArticleType)
               .Include(a => a.ArticleDetail )
               .Where(a => a.ArticleTypeId == 25 && a.ArticleDetail.Any(d => d.LanguageId == 1)).OrderBy(a => a.SortOrder).Skip(0).Take(3);
            ViewBag.Advduoi = advduoi.ToList();

            //var listitem = advduoi.ToList() + partner.ToList();

            return View();
        }

        public ActionResult AddThis()
        {
            return PartialView();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        public ActionResult CommentUs(int id)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Articles.FirstOrDefault(a => a.Id == id);
                TryUpdateModel(model);
                model.Number = model.Number + 1;
                db.SaveChanges();
                return new EmptyResult();
            }
        }
        [HttpPost]
        public ActionResult ContactAjaxBG(FormCollection form)
        {
            try
            {
                var model = new ContactProductModel();
                model.Name = form["Name2"].ToString();
                model.Company = form["Company2"].ToString();
                model.Phone = form["Number2"].ToString();
                model.Email = form["Email2"].ToString();
                model.Title = form["Subject2"].ToString();
                model.Body = form["Body2"].ToString();
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var file = Uploader.Upload(Request.Files[0], ArticleFileType.IMAGE, "FileAttach");
                    model.Address = file.FileName;
                }

                UserMailer.CreateMessage("ContactBG", "Thông tin yêu cầu báo giá - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                ViewBag.Result = 0;
                model = new ContactProductModel();
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }
            return Json(new { Status = 0, Message = "OK" });
        }
        [HttpPost]
        public ActionResult ContactAjaxHT(FormCollection form)
        {
            try
            {
                var model = new ContactProductModel();
                model.Name = form["Name"].ToString();
                model.Company = form["Company"].ToString();
                model.Phone = form["Number"].ToString();
                model.Email = form["Email"].ToString();
                model.Title = "Liên hệ hợp tác bán hàng";
                model.Body = form["Subject"].ToString();

                UserMailer.CreateMessage("ContactHT", "Thông tin liên hệ hợp tác bán hàng - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                ViewBag.Result = 0;
                model = new ContactProductModel();
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
            }
            return Json(new { Status = 0, Message = "OK" });
        }
        public ActionResult RegisterMail(string mail)
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Code == "NEWLETTER");
                if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(mail))
                    return Content("Invalid");
                else
                {
                    var model = new Article();
                    TryUpdateModel(model);
                    var details = from d in SiteConfig.Languages
                                  select new ArticleDetail()
                                  {
                                      LanguageId = d.Id,
                                      ArticleName = mail,
                                      ShortDesc = "",
                                      Description = "",
                                      SEOKeywords = "",
                                  };

                    model.ArticleDetail = new VList<ArticleDetail>();
                    model.ArticleDetail.AddRange(details);
                    model.DateCreated = DateTime.Now;
                    //model.WebsiteId = SiteConfig.SiteId;
                    model.ArticleTypeId = type.Id;
                    model.UserCreated = 1;
                    db.Articles.Add(model);
                    db.SaveChanges();
                    return new EmptyResult();
                }
            }
        }

        public ActionResult Consulting(string name, string phone, string email, string content)
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Code == "CONSULTING");
                if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(email))
                    return Content("Invalid");
                else
                {
                    var model = new Article();
                    TryUpdateModel(model);
                    var details = from d in SiteConfig.Languages
                                  select new ArticleDetail()
                                  {
                                      LanguageId = d.Id,
                                      ArticleName = name,
                                      ShortDesc = phone,
                                      Description = content,
                                      SEODescription = email,
                                  };

                    model.ArticleDetail = new VList<ArticleDetail>();
                    model.ArticleDetail.AddRange(details);
                    model.DateCreated = DateTime.Now;
                    //model.WebsiteId = SiteConfig.SiteId;
                    model.ArticleTypeId = type.Id;
                    model.UserCreated = 1;
                    db.Articles.Add(model);
                    db.SaveChanges();
                    return Json(new { Status = 0, Message = "OK" });
                }
            }
        }

        public static CultureInfo HCultureInfo(Currency item)
        {
            CultureInfo cultureInfo = new CultureInfo(item.Code);
            cultureInfo = (CultureInfo)cultureInfo.Clone();

            if (item.CurrencyPositivePattern > -1)
                cultureInfo.NumberFormat.CurrencyPositivePattern = item.CurrencyPositivePattern;

            if (!string.IsNullOrWhiteSpace(item.CurrencySymbol))
                cultureInfo.NumberFormat.CurrencySymbol = item.CurrencySymbol;

            if (item.Code.Equals("vi-VN", StringComparison.OrdinalIgnoreCase))
                cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;

            return cultureInfo;
        }
        public static string HPrice(Price item)
        {
            var stringprice = SiteConfig.LanguageId == 1 ? "<span class='price'>Liên hệ</span>" : "<span class='price'>Contact</span>";
            if (item == null)
                return stringprice;

            if (item.Value > 0)
            {
                var currency = item.Currency;
                if (currency.CheckFormat)
                    stringprice = string.Format("{" + currency.Formatting + "}" + currency.CurrencySymbol, item.Value);
                else
                {
                    CultureInfo cultureInfo = HCultureInfo(currency);
                    stringprice = item.Value.ToString("C", cultureInfo);
                }
            }
            return stringprice;
        }
        public static string HDiscountPrice(Price item)
        {
            var stringprice = "";

            if (item == null)
                return stringprice;

            var price = item.Article.DiscountPrice;
            if (item.Value > 0)
            {
                var currency = item.Currency;
                if (currency.CheckFormat)
                    stringprice = string.Format("{" + currency.Formatting + "}" + currency.CurrencySymbol, price);
                else
                {
                    CultureInfo cultureInfo = HCultureInfo(currency);
                    stringprice = price.ToString("C", cultureInfo);
                }
            }
            return stringprice;
        }
        public JsonResult AutoComplete(string keyword)
        {
            using (DataContext db = new DataContext())
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrEmpty(keyword))
                {
                    var query = (from c in db.Articles
                                 join d in db.ArticleDetails on c.Id equals d.ArticleId  //d.LanguageId == 1
                                 where c.ArticleTypeId == 2 && d.LanguageId == 1 && (d.ArticleName.Contains(keyword) || c.IconFont.Contains(keyword))
                                 select new
                                 {
                                     c = c,
                                     d = d
                                 });

                    var ret = new List<Article>();
                    foreach (var item in query.ToList().Take(10))
                    {
                        item.c.ArticleDetail = new VList<ArticleDetail>() { item.d };
                        item.c.LanguageId = 1;
                        item.c.Prices = db.Prices.Include(p => p.Currency).Where(p => p.ArticleId == item.c.Id).ToList();
                        item.c.CurrentDiscount = db.Discounts.Include(d => d.Currency).OrderBy(a => a.DateStart).Where(d => (d.Articles.Select(ad => ad.Id).Contains(item.c.Id) || d.Categories.Any(cd => cd.Articles.Any(a => a.Id == item.c.Id)) || d.AllItems)).OrderByDescending(a => a.DateEnd).FirstOrDefault(a => a.DateStart <= DateTime.Now && a.DateEnd >= DateTime.Now);
                        ret.Add(item.c);
                    }

                    if (ret.Count > 0)
                    {
                        string path = "";
                        string html = "";
                        foreach (var item in ret)
                        {
                            if (item.ImageUrl != null)
                            {
                                var str = item.ImageUrl.Split('/');
                                if (str.Length <= 1)
                                    path = "/UserUpload/Article/";
                            }

                            html += "<div class='aitem'><div class='row'>";//item.ImageUrl != null ? 
                            html += "<div class='col-lg-3 col-md-3 col-sm-6 col-xs-6'><a class='images' href='" + Url.Action("Detail", "Article", new { id = item.Id, code = "san-pham", title = Unichar.UnicodeStrings.UrlString(item.ArticleName) }) + "'><figure><img src='" + Url.Content(path + item.ImageUrl) + "?width=80&height=80' alt='" + item.ArticleName + "' /></figure></a></div>";

                            html += "<div class='col-lg-9 col-md-9 col-sm-6 col-xs-6'>";
                            html += "<a class='name' href='" + Url.Action("Detail", "Article", new { id = item.Id, code = "san-pham", title = Unichar.UnicodeStrings.UrlString(item.ArticleName) }) + "'>" + item.ArticleName + "</a>";
                            //html += "<div class='price-all'><span class='lbl'>Giá: </span>" + (item.DiscountPrice > 0 ? " <span class='price'>" + HDiscountPrice(item.ItemPrice) + "</span><span class='price-old'>" + HPrice(item.ItemPrice) + "</span>" : HPrice(item.ItemPrice)) + "</div>";
                            html += "<div class='code'><span class='lbl'>P/N: </span>" + "<span class='ctn'>" + item.IconFont + "</span>" + "</div>";
                            html += "</div>";

                            html += "</div></div>";
                        }

                        list.Add(html);
                    }
                    else
                        list.Add("<div class='no-data'>Không có kết quả nào hợp lệ.</div>");
                }
                else list.Add("<div class='no-data'>Vui lòng nhâp từ khóa muốn tìm.</div>");
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Search(string keyword, int typeId = 2, int page = 1, int pagesize = 30, int listcate = 0)
        {
            ViewBag.Keyword = keyword;
            
            ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER;
            VCMS.MVC4.Data.SortDirection direction = VCMS.MVC4.Data.SortDirection.DESCENDING;
            ArticleFlags Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
            if (Request["sortorder"] != null)
                sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
            if (Request["sortdirection"] != null)
                direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
            if (Request["flag"] != null)
            {
                if (Request["flag"] == "all")
                    Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
                else
                    Flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
            }
            var articles = Article.Search(typeId, SiteConfig.LanguageId, Unichar.UnicodeStrings.UrlString(keyword).ToLower(), listcate, page, pagesize, sortOrder: sortOrder, direction: direction, includeflags: ArticleIncludeFlags.ARTICLE_DETAIL | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES, flags: Flag);
            //var articles = Article.SearchTag(typeId, SiteConfig.LanguageId, keyword, 0, pageIndex, pageSize, sortOrder, direction, Flag, includeflags: ArticleIncludeFlags.ARTICLE_DETAIL | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES);
            ViewBag.Type = ArticleType.GetById(typeId, SiteConfig.LanguageId);
            ViewBag.ItemCount = articles.TotalItemCount; //Article.SearchCount(typeId, SiteConfig.LanguageId, Unichar.UnicodeStrings.UrlString(keyword).ToLower(), flags: Flag);
            return View("Search", articles);
        }
        public ActionResult SearchIn(string keyword, int typeId = 2, int pageIndex = 1, int pageSize = 8)
        {
            ViewBag.Keyword = keyword;
            if (keyword == "")
                pageSize = 30;

            ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER;

            VCMS.MVC4.Data.SortDirection direction = VCMS.MVC4.Data.SortDirection.DESCENDING;
            ArticleFlags Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
            if (Request["sortorder"] != null)
                sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
            if (Request["sortdirection"] != null)
                direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
            if (Request["flag"] != null)
            {
                if (Request["flag"] == "all")
                    Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
                else
                    Flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
            }
            var articles = Article.Search(typeId, SiteConfig.LanguageId, Unichar.UnicodeStrings.UrlString(keyword).ToLower(), 0, pageIndex, pageSize, sortOrder: sortOrder, direction: direction, flags: Flag);

            ViewBag.ItemCount = Article.SearchCount(typeId, SiteConfig.LanguageId, Unichar.UnicodeStrings.UrlString(keyword).ToLower(), flags: Flag);
            return View("SearchIn", articles);
        }
        
        public ActionResult Tag(string code,string keyword, int typeId = 0, int page = 1, int pagesize = 20)
        {
 
            using (DataContext db = new DataContext())   
            {
                
                ViewBag.Tag = keyword;

                //keyword = Unichar.UnicodeStrings.LatinToAscii(keyword);
                var ids = db.Articles.Where(a => a.Keywords.Any(kw => kw.Tag.Replace(" ", "-").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).OrderBy(a => a.SortOrder).Skip((page - 1) * pagesize).Take(pagesize).Select(a => a.Id).ToArray();

                var items = db.Articles
                    .Include(a => a.ArticleDetail)
                    .Include(a => a.ArticleType.ArticleTypeDetail)
                    .Include(a => a.Prices.Select(c => c.Currency))
                    .Include(a => a.PropertyValues.Select(pv => pv.Property)).Where(a => ids.Contains(a.Id)).ToList();
                var count = db.Articles.Where(a => a.Keywords.Any(kw => kw.Tag.Replace(" ", "-").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).Count();
                var articles = new ArticleResult { List = items, TotalItemCount = count, PageIndex = page, PageSize = pagesize };

                //var items = db.Articles
                //    .Include(a => a.ArticleDetail)
                //    .Include(a => a.Prices.Select(c => c.Currency))
                //    .Include(a => a.PropertyValues.Select(pv => pv.Property))
                //    .Include(a => a.ArticleType).Where(a => a.Keywords.Any(kw => kw.Tag.Replace("-", " ").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).OrderBy(a => a.SortOrder).Skip((page - 1) * pagesize).Take(pagesize).ToList();
                //var count = db.Articles.Where(a => a.Keywords.Any(kw => kw.Tag.Replace("-", " ").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).Count();
                //var articles = new ArticleResult { List = items, TotalItemCount = count, PageIndex = page, PageSize = pagesize };

                //var items = db.Articles.Include(a => a.ArticleDetail).Include(a => a.PropertyValues.Select(pv => pv.Property)).Include(a => a.Prices.Select(c => c.Currency)).Include(a => a.ArticleType).Where(a => a.Keywords.Any(kw => kw.Tag.Replace(" ", "").Replace("-", "").Replace("/", "").Replace(")", "").Replace("(", "").Replace("*", "").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).OrderBy(a => a.ArticleDetail.Min(d => d.ArticleName.Trim())).Skip((page - 1) * pagesize).Take(pagesize).ToList();
                //var count = db.Articles.Include(a => a.ArticleType).Where(a => a.Keywords.Any(kw => kw.Tag.Replace(" ", "").Replace("-", "").Replace("/", "").Replace(")", "").Replace("(", "").Replace("*", "").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).Count();

                //var articles = new ArticleResult { List = items, TotalItemCount = count, PageIndex = page, PageSize = pagesize };


                //var count = db.Articles.Where(a => a.Keywords.Any(kw => kw.Tag.Replace(" ","").Replace("-","").Replace("/","").Replace(")","").Replace("(","").Replace("*","").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase))).Count();
                //var Ids = db.Articles.Where(a => a.Keywords.Any(kw => kw.Tag.Replace(" ","").Replace("-","").Replace("/","").Replace("(","").Replace(")","").Replace("*","").Trim().ToLower().Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                //.OrderBy(a => a.SortOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).Select(a => a.Id).ToArray();

                //var query = db.Articles.Include(a=>a.ArticleDetail.Select(d => d.Body)).Include(a => a.ArticleType.ArticleTypeDetail).Include(a => a.PropertyValues.Select(pv => pv.Property)).Include(a => a.Prices.Select(c => c.Currency))
                //.Where(a => Ids.Contains(a.Id));
                //var articles = new ArticleResult { List = query.ToList(), TotalItemCount = count, PageIndex = pageindex, PageSize = pagesize };
                return View("Search", articles);
            }
           
            //ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER;
            //VCMS.MVC4.Data.SortDirection direction = VCMS.MVC4.Data.SortDirection.DESCENDING;
            //ArticleFlags Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
            //if (Request["sortorder"] != null)
            //    sortOrder = (ArticleSortOrder)Enum.Parse(typeof(ArticleSortOrder), Request["sortorder"]);
            //if (Request["sortdirection"] != null)
            //    direction = (SortDirection)Enum.Parse(typeof(SortDirection), Request["sortdirection"]);
            //if (Request["flag"] != null)
            //{
            //    if (Request["flag"] == "all")
            //        Flag = ArticleFlags.ACTIVE | ArticleFlags.INACTIVE;
            //    else
            //        Flag = (ArticleFlags)Enum.Parse(typeof(ArticleFlags), Request["flag"]);
            //}

            //var articles = Article.SearchTag(typeId, SiteConfig.LanguageId, keyword, 0, pageIndex, pageSize, sortOrder, direction, Flag, includeflags: ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES);
            //ViewBag.ItemCount = Article.SearchCountTag(SiteConfig.LanguageId, Unichar.UnicodeStrings.UrlString(keyword).ToLower());
            //return View("Search", articles);
        }
          
        private UserMailer _userMailer = new UserMailer();
        public UserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }
        public ActionResult Contact(int? ID)
        {
            if (ID != null)
            {
                var product = Article.GetById((int)ID, SiteConfig.LanguageId);
                if (product == null)
                    return HttpNotFound();

                ViewBag.ProductName = product.ArticleName;
                var model = new ContactModel
                {
                    Subject = "Liên hệ " + product.ArticleName
                };
                if (product.ArticleType.Code == "PRODUCT" && product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CONTACT", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    ViewBag.ContactInfo = product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CONTACT", StringComparison.OrdinalIgnoreCase) && a.LanguageId == SiteConfig.LanguageId).Value;
                }
                if (product.ArticleType.Code == "COMPANY" && product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("MAP", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    ViewBag.Map = product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("MAP", StringComparison.OrdinalIgnoreCase)).Value;
                    model.Subject = "";
                }
                return View(model);
            }
            ViewBag.Message = "Your contact page.";

            return View(new ContactModel());
        }
        public ActionResult ContactForm()
        {
           
            ViewBag.Message = "Your contact page.";

            return View(new ContactFormModel());
        }
        public ActionResult ContactABC(int? ID)
        {
            if (ID != null)
            {
                var product = Article.GetById((int)ID, SiteConfig.LanguageId);

                ViewBag.ProductName = product.ArticleName;
                var model = new ContactModel
                {
                    Subject = "Liên hệ " + product.ArticleName
                };
                if (product.ArticleType.Code == "PRODUCT" && product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CONTACT", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    ViewBag.ContactInfo = product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CONTACT", StringComparison.OrdinalIgnoreCase) && a.LanguageId == SiteConfig.LanguageId).Value;
                }
                if (product.ArticleType.Code == "COMPANY" && product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("MAP", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    ViewBag.Map = product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("MAP", StringComparison.OrdinalIgnoreCase)).Value;
                    model.Subject = "";
                }
                return View(model);
            }
            ViewBag.Message = "Your contact page.";

            return View(new ContactModel());
        }

        public ActionResult QuickContact(int? ID)
        {
            //if (ID != null)
            //{
            var product = Article.GetById((int)ID, SiteConfig.LanguageId);

            //ViewBag.ProductName = product.ArticleName;
            //var model = new ContactModel
            //{
            //    Subject = "Liên hệ " + product.ArticleName
            //};
            //if (product.ArticleType.Code == "PRODUCT" && product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CONTACT", StringComparison.OrdinalIgnoreCase)) != null)
            //{
            //    ViewBag.ContactInfo = product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("CONTACT", StringComparison.OrdinalIgnoreCase) && a.LanguageId == SiteConfig.LanguageId).Value;
            //}
            //if (product.ArticleType.Code == "COMPANY" && product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("MAP", StringComparison.OrdinalIgnoreCase)) != null)
            //{
            //    ViewBag.Map = product.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("MAP", StringComparison.OrdinalIgnoreCase)).Value;
            //    model.Subject = "";
            //}
            //return View(model);
            //}
            //ViewBag.Message = "Your contact page.";

            return View(product);//new ContactModel()
        }

        public ActionResult Status(string stype, string Username)
        {
            string isonval = "";
            switch (stype)
            {
                case "yahoo":
                    isonval = PostData("http://mail.opi.yahoo.com/online?u=" + Username.Trim() + "&m=t&t=1");
                    break;
            }

            if (isonval == "00")
            {
                return Json(new { Status = 0 });
            }

            else if (isonval == "01")
            {
                return Json(new { Status = 1 });
            }

            else
            {
                return Json(new { Status = 0 });
            }
        }

        private string PostData(string url)
        {

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse)webrequest.GetResponse();
            string responseHtml;
            using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
            {
                responseHtml = responseStream.ReadToEnd().Trim();
            }

            return responseHtml;
        }
        public ActionResult Sitemaps()
        { return View(); }
        [HttpPost]
        public ActionResult 
            Contact(ContactModel model)
        {

            try
            {
                //To Validate Google recaptcha
                var response = Request["g-recaptcha-response"];
                string secretKey = "6LfDIs4fAAAAAHFni_8uqHWmU1StA65b3TBsK_3g";
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");

                //check the status is true or not
                if (status == true)
                {
                    //ViewBag.Message = "Your Google reCaptcha validation success";
                    model.Body = "đk";
                    model.Subject = model.Subject;
                    UserMailer.CreateMessage("Contact", "Thông tin đăng ký - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email }).Send();
                    ViewBag.Result = 0;
                    model = new ContactModel();
                    return View(new ContactModel());
                }
                else
                {
                    ViewBag.Result = 2;
                    return View(model);
                }

                //if (Request["img"] != null)
                //    model.Image = Request["img"].ToString();
                //if (Session["captchastring"] == null || HttpContext.Session["captchastring"].ToString() != model.CaptCha)
                //{
                //    ViewBag.Result = 2;
                //    return View(model);
                //}
                //else
                //{
                    
                //}
                //if (Request["img"] != null)
                //    model.Image = Request["img"].ToString();
                //if (Session["captchastring"] == null || HttpContext.Session["captchastring"].ToString() != model.CaptCha)
                //{
                //    ViewBag.Result = 2;
                //    return View(model);
                //}
                //else
                //{
                //if (model.Body != null)
                //{
                //    model.Body = model.Body;
                //}
                //else
                //{
                //    model.Body ="";
                //}  
                //if (model.Subject != null) {
                //    model.Subject = model.Subject;
                //}


                //var macode = DateTime.Now.ToString("yyMMddss");
                //model.Country = macode;
                //UserMailer.CreateMessage("Contact", "Thông tin đăng ký - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();

                //ViewBag.Country = macode;
                //ViewBag.Name = model.Name;
                //ViewBag.Subject = model.Subject;
                //ViewBag.Email = model.Email;
                //ViewBag.Phone = model.Phone;
                //ViewBag.Body = model.Body;
                //ViewBag.Result = 0;
                //model = new ContactModel();
                ////return Redirect("/UserMailer/Welcome");
                ////Response.Redirect(Url.Action("UserMailer", "Welcome"));
                //return PartialView("_Complete");

                //return View(new ContactModel());
                //}
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
                return View(model);
            }
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            //        if (Session["CaptCha"] == null || HttpContext.Session["CaptCha"].ToString() != model.CaptCha)
            //        {
            //            ViewBag.Result = 2;
            //            return View(model);
            //        }
            //        else
            //        {
            //            model.Subject = model.Subject;
            //            UserMailer.CreateMessage("Contact", string.Format("{0} - {1}", model.Subject, SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Title), model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();

            //            ViewBag.Result = 0;
            //            model = new ContactModel();
            //            return View(new ContactModel());
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.Result = -1;
            //        ViewBag.Message = ex.Message;
            //        return View(model);
            //    }
            //}
            //else
            //return View(model);
        }
         [HttpPost]


        public ActionResult ContactForm(ContactFormModel model)
        {
            try
            {
                    UserMailer.CreateMessage("ContactForm", "KÝ GỬI BĐS - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                    ViewBag.Result = 0;
                    model = new ContactFormModel();
                    return View(new ContactFormModel());
              
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
                return View(model);
            }
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            //        if (Session["CaptCha"] == null || HttpContext.Session["CaptCha"].ToString() != model.CaptCha)
            //        {
            //            ViewBag.Result = 2;
            //            return View(model);
            //        }
            //        else
            //        {
            //            model.Subject = model.Subject;
            //            UserMailer.CreateMessage("Contact", string.Format("{0} - {1}", model.Subject, SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Title), model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();

            //            ViewBag.Result = 0;
            //            model = new ContactModel();
            //            return View(new ContactModel());
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.Result = -1;
            //        ViewBag.Message = ex.Message;
            //        return View(model);
            //    }
            //}
            //else
            //return View(model);
        }
        [HttpPost]
        public ActionResult ContactAjax(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var model = new ContactModel();
                    var name = SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name;
                    
                        model.Name = form["txt_name"];
                        //model.Address = form["txt_address"];
                        model.Phone = form["txt_phone"];
                        //model.Fax = form["txt_fax"];
                        model.Email = form["txt_email"];

                        //model.Subject = form["txt_subject"];
                        model.Body = form["txt_body"];
                        UserMailer.CreateMessage("Contact", "Thông tin đăng ký - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                        ViewBag.Result = 0;
                        model = new ContactModel();
                        return Json(new { Status = 0 });
                    
                   
                }
                catch (Exception ex)
                {
                    return Json(new { Status = -1, Message = ex.Message });
                }
            }
            else
                return Json(new { Status = 1 });
        }
        public ActionResult ContactAjaxDV(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var model = new ContactModel();
                    var name = SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name;

                    model.Name = form["txt_name"];
                    model.Address = form["txt_address"];
                    model.Company = form["txt_company"];
                    model.Phone = form["txt_phone"];
                    model.Fax = form["txt_fax"];
                    model.Email = form["txt_email"];
                    model.Subject = form["txt_subject"];
                    model.Body = form["txt_body"];
                    UserMailer.CreateMessage("ContactAjax", "Thông tin đăng ký Dịch vụ - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();
                    ViewBag.Result = 0;
                    model = new ContactModel();
                    return Json(new { Status = 0 });


                }
                catch (Exception ex)
                {
                    return Json(new { Status = -1, Message = ex.Message });
                }
            }
            else
                return Json(new { Status = 1 });
        }


        [HttpPost]
        public ActionResult QContact(string name, string phone, string content)
        {
            var model = new ContactModel();
            try
            {
                model.Name = name;
                model.CaptCha = "";
                model.Address = "";
                model.Phone = phone;
                model.Body = content;
                model.Email = "";
                model.Subject = "";
                UserMailer.CreateMessage("Contact", "Đăng ký tư vấn - " + SiteConfig.SiteInfo.DefaultDomain, model, new string[] { SiteConfig.SiteInfo.Email, model.Email }).Send();

                ViewBag.Result = 0;
                ModelState.Clear();
                model = new ContactModel();
                //return PartialView("_Complete");
                return Json(new { Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Result = -1;
                ViewBag.Message = ex.Message;
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult Order(int id, FormCollection form)
        {
            try
            {
                var article = Article.GetById(id, SiteConfig.LanguageId);
                var model = new ContactModel();
                TryUpdateModel(model);
                model.Subject = "Order: " + article.ArticleName;
                UserMailer.Contact(model).Send();
                ViewBag.Result = 0;

                return Json(new { status = 0, message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, message = "Failed: " + ex.Message });
            }
        }
        public ActionResult SiteMapInsite()
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.ArticleTypes = db.ArticleTypes.Include("ArticleTypeDetail").Where(t => ((int)t.Flag & (int)ArticleTypeFlags.SITEMAP) > 0).ToList();
                ViewBag.Categories = db.Categories.Include("CategoryDetail").Include("ArticleType.ArticleTypeDetail").Where(c => ((int)c.ArticleType.Flag & (int)ArticleTypeFlags.SITEMAP) > 0).Take(100).ToList();
                ViewBag.Articles = db.Articles.Include("ArticleDetail").Include("ArticleType.ArticleTypeDetail").Where(a => ((int)a.ArticleType.Flag & (int)ArticleTypeFlags.SITEMAP) > 0).Take(500).ToList();
                return View();
            }
        }
        [EmptyLineFilter]
        public ActionResult SiteMap()
        {
            using (DataContext db = new DataContext())
            {
                Response.Clear();
                Response.ContentType = "text/xml";
                var url = HttpContext.Request.Url;
                string basepath = url.Scheme + "://" + url.Authority;
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml += "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\" xmlns:video=\"http://www.google.com/schemas/sitemap-video/1.1\">";
                var article = db.Articles.Include("ArticleDetail").Include("ArticleType.ArticleTypeDetail").Where(a => ((int)a.ArticleType.Flag & (int)ArticleTypeFlags.SITEMAP) > 0).Take(100).ToList();
                var category = db.Categories.Include("CategoryDetail").Include("ArticleType.ArticleTypeDetail").Where(c => ((int)c.ArticleType.Flag & (int)ArticleTypeFlags.SITEMAP) > 0).Take(100).ToList();
                var articletype = db.ArticleTypes.Include("ArticleTypeDetail").Where(t => ((int)t.Flag & (int)ArticleTypeFlags.SITEMAP) > 0).ToList();

                xml += "<url>";
                xml += "<loc>";
                xml += basepath;
                xml += "</loc>";
                xml += "</url>";
                foreach (var lan in SiteConfig.Languages)
                {
                    foreach (var item in article)
                    {
                        xml += "<url>";
                        xml += "<loc>";
                        xml += basepath + Url.Action("Detail", "Article", new { id = item.Id, code = item.ArticleType.ArticleTypeDetail[lan.Id].UrlFriendly, title = Unichar.UnicodeStrings.UrlString(item.ArticleName).ToLower() });
                        xml += "</loc>";
                        xml += "</url>";
                    }
                    foreach (var item in category)
                    {
                        xml += "<url>";
                        xml += "<loc>";
                        xml += basepath + Url.Action("Detail", "Category", new { id = item.Id, code = item.ArticleType.ArticleTypeDetail[lan.Id].UrlFriendly, title = Unichar.UnicodeStrings.UrlString(item.CategoryName).ToLower() });
                        xml += "</loc>";
                        xml += "</url>";
                    }
                    foreach (var item in articletype)
                    {
                        xml += "<url>";
                        xml += "<loc>";
                        xml += basepath + Url.Action("Index", "ArticleType", new { code = item.ArticleTypeDetail[lan.Id].UrlFriendly.ToLower() });
                        xml += "</loc>";
                        xml += "</url>";
                    }
            //        foreach (var item in article)
            //        {
            //            var file = db.Articles.Include("ArticleDetail").Include("PropertyValues.Property.PropertyDetail").Include("ArticleFiles")
            //.OrderBy(a => a.SortOrder).Where(a => a.Id == item.Id).FirstOrDefault(a => a.ArticleType.Code == item.ArticleType.Code);
            //            if (file.Attachments.Count > 0)
            //            {
            //                foreach (var fi in file.Attachments)
            //                {
            //                    xml += "<url>";
            //                    xml += "<loc>";
            //                    xml += basepath + HttpUtility.UrlDecode(Url.Action("Viewer", "ArticleFile", new { id = fi.Id }));
            //                    xml += "</loc>";
            //                    xml += "</url>";
            //                }
            //            }
            //        }
                }
                xml += "</urlset>";
                Response.Write(xml);
                Response.End();
                return View();
            }
        }


        public ActionResult DeleteMail(string mail)
        {
            if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(mail))
            {
                return Json(new { Status = 0 });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var model = new Article();
                    TryUpdateModel(model);
                    using (DataContext db = new DataContext())
                    {
                        var query = (from a in db.Articles
                                     join d in db.ArticleDetails on a.Id equals d.ArticleId
                                     join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId
                                     where d.ArticleName == mail && d.LanguageId == 1 && td.ArticleTypeId == db.ArticleTypes.FirstOrDefault(at => at.Code == "EMAIL").Id
                                     select new
                                     {
                                         a = a,
                                         d = d
                                     });
                        var article = query.FirstOrDefault();
                        if (article != null)
                        {
                            Session["email"] = "True";
                            db.Articles.Delete(a => a.Id == article.a.Id);
                            db.SaveChanges();
                            return Json(new { Status = 1 });
                        }
                        else
                        {
                            return Json(new { Status = 2 });
                        }
                    }
                }
            }
            return Json(new { Status = 3 });
        }

        [HttpPost]
        public ActionResult StatusRegisterEmail(string status)
        {
            System.Threading.Thread.Sleep(1000);
            Session["email"] = status;
            return Json(new { Status = 0 });
        }

        public ActionResult IFrameLoad(string view)
        {
            if (!string.IsNullOrEmpty(view))
            {
                ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, view, null);
                if (result.View != null)
                    return PartialView(view);
                else
                    return Content("");
            }
            return Content("");
        }
        public ActionResult ViewMap(int id = 0)
        {
            using (DataContext db = new DataContext())
            {
                var includeFlags = ArticleIncludeFlags.FILES | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.PROPERTIES | ArticleIncludeFlags.ARTICLE_DETAIL;
                Article article = Article.GetById(id, SiteConfig.LanguageId, includeFlags, db);
                if (article == null)
                    return HttpNotFound();
                return View(article);
            }
        }
    }
}
