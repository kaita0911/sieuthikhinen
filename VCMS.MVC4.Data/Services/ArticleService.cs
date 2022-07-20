
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
//using log4net;

namespace VCMS.MVC4.Data
{
    
    public partial class Article
    {
        #region static methods

        //public static ArticleResult GetByUser(int userId, int typeId, int siteId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleSearchOption option = ArticleSearchOption.ALL)
        //{
        //    using (DataContext db = new DataContext())
        //    {
        //        var query = (from a in db.Articles
        //                     join d in db.ArticleDetails on a.Id equals d.ArticleId
        //                     join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
        //                     join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId


        //                     where d.LanguageId == languageId && a.ArticleTypeId == typeId
        //                     && a.WebsiteId == siteId && td.LanguageId == languageId
        //                     && (((int)a.Flags & (int)flags) > 0)
        //                     //&& a.Categories.Any(c => ids.Contains(c.Id))

        //                     select new
        //                     {
        //                         a = a,
        //                         d = d,
        //                         t = t,
        //                         td = td
        //                     });

        //        if (option == ArticleSearchOption.HAS_TITLE_ONLY)
        //            query = query.Where(q => q.d.ArticleName.Trim() != "");
        //        if (option == ArticleSearchOption.HAS_SHORTDESC_ONLY)
        //            query = query.Where(q => q.d.ShortDesc.Substring(0, 1).Trim() != "");
        //        if (option == ArticleSearchOption.HAS_DESC_ONLY)
        //            query = query.Where(q => q.d.Description.Substring(0, 1).Trim() != "");
        //        var article = query.Take(0);

        //        switch (sortOrder)
        //        {
        //            case ArticleSortOrder.SORT_ORDER:
        //                if (direction == SortDirection.ASCENDING)
        //                    article = query.OrderBy(x => x.a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //                else
        //                    article = query.OrderByDescending(x => x.a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //                break;
        //            case ArticleSortOrder.DATE_CREATED:
        //                if (direction == SortDirection.ASCENDING)
        //                    article = query.OrderBy(x => x.a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //                else
        //                    article = query.OrderByDescending(x => x.a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //                break;
        //            case ArticleSortOrder.ARTICLE_NAME:
        //                if (direction == SortDirection.ASCENDING)
        //                    article = query.OrderBy(x => x.d.ArticleName).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //                else
        //                    article = query.OrderByDescending(x => x.d.ArticleName).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //                break;

        //            default:
        //                break;
        //        }
        //        var articleList = article.ToList();
        //        var propvals = db.ArticlePropertyValues.Where(pv => article.Select(a => a.a.Id).Contains(pv.ArticleId)).Include(pv => pv.Property).ToList();

        //        var ret = new List<Article>();
        //        foreach (var item in article.ToList())
        //        {
        //            item.a.ArticleDetail = new VList<ArticleDetail>() { item.d };
        //            item.a.PropertyValues = propvals.Where(pv => pv.ArticleId == item.a.Id).ToList();
        //            item.a.Prices = db.Prices.Include(p => p.Currency).Where(p => p.ArticleId == item.a.Id).ToList();
        //            item.a.CurrentDiscount = db.Discounts.Include(d => d.Currency).OrderBy(a => a.DateStart).Where(d => (d.Articles.Select(ad => ad.Id).Contains(item.a.Id) || d.Categories.Any(cd => cd.Articles.Any(a => a.Id == item.a.Id)) || d.AllItems)).OrderByDescending(a => a.DateEnd).FirstOrDefault(a => a.DateStart <= DateTime.Now && a.DateEnd >= DateTime.Now);

        //            item.a.LanguageId = item.t.LanguageId = languageId;
        //            item.a.Categories = db.Categories.Include(c => c.CategoryDetail).Where(c => c.Articles.Select(arc => arc.Id).Contains(item.a.Id)).Take(1).ToList();
        //            (item.a.Categories as List<Category>).ForEach(c => c.LanguageId = languageId);

        //            item.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { item.td };
        //            item.a.ArticleType = item.t;
        //            item.a.ArticleFiles = db.ArticleFiles.Include(f => f.ArticleFileDetail).Where(f => f.ArticleId == item.a.Id).ToList();
        //            ret.Add(item.a);
        //        }
        //        var count = query.Count();
        //        return new ArticleResult { List = ret, TotalItemCount = count, PageIndex = pageIndex, PageSize = pageSize };
        //    }
        //}

        public static ArticleResult GetByMultipleId(int[] ids, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleSearchOption option = ArticleSearchOption.ALL)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.Articles
                             join d in db.ArticleDetails on a.Id equals d.ArticleId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId

                             where d.LanguageId == languageId
                             && td.LanguageId == languageId
                             && (((int)a.Flags & (int)flags) > 0)
                             && ids.Contains(a.Id)
                             select new
                             {
                                 a = a,
                                 d = d,
                                 t = t,
                                 td = td,
                                 f = a.ArticleFiles,
                             });

                if (option == ArticleSearchOption.HAS_TITLE_ONLY)
                    query = query.Where(q => q.d.ArticleName.Trim() != "");
                if (option == ArticleSearchOption.HAS_SHORTDESC_ONLY)
                    query = query.Where(q => q.d.ShortDesc.Substring(0, 1).Trim() != "");
                if (option == ArticleSearchOption.HAS_DESC_ONLY)
                    query = query.Where(q => q.d.Description.Substring(0, 1).Trim() != "");
                var article = query.Take(0);

                switch (sortOrder)
                {
                    case ArticleSortOrder.SORT_ORDER:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    case ArticleSortOrder.DATE_CREATED:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    case ArticleSortOrder.ARTICLE_NAME:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.d.ArticleName).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.d.ArticleName).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    case ArticleSortOrder.RATING:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.Rating).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.Rating).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    default:
                        break;
                }
                // var articleList = article.ToList();
                var propvals = db.ArticlePropertyValues.Where(pv => article.Select(a => a.a.Id).Contains(pv.ArticleId)).Include(pv => pv.Property).ToList();

                var ret = new List<Article>();
                foreach (var item in article.ToList())
                {
                    item.a.ArticleDetail = new VList<ArticleDetail>() { item.d };
                    item.a.PropertyValues = propvals.Where(pv => pv.ArticleId == item.a.Id).ToList();
                    item.a.Prices = db.Prices.Include(p => p.Currency).Where(p => p.ArticleId == item.a.Id).ToList();
                    item.a.CurrentDiscount = db.Discounts.Include(d => d.Currency).OrderBy(a => a.DateStart).Where(d => (d.Articles.Select(ad => ad.Id).Contains(item.a.Id) || d.Categories.Any(cd => cd.Articles.Any(a => a.Id == item.a.Id)) || d.AllItems)).OrderByDescending(a => a.DateEnd).FirstOrDefault(a => a.DateStart <= DateTime.Now && a.DateEnd >= DateTime.Now);
                    item.a.LanguageId = item.t.LanguageId = languageId;
                    item.a.Categories = db.Categories.Include(c => c.CategoryDetail).Where(c => c.Articles.Select(arc => arc.Id).Contains(item.a.Id)).ToList();
                    (item.a.Categories as List<Category>).ForEach(c => c.LanguageId = languageId);

                    item.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { item.td };
                    item.a.ArticleType = item.t;
                    ret.Add(item.a);
                }
                var count = query.Count();
                return new ArticleResult { List = ret, TotalItemCount = count, PageIndex = pageIndex, PageSize = pageSize };
            }
        }

        public static ArticleResult GetByUser(int userId, int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
            | ArticleIncludeFlags.ARTICLE_DETAIL
            | ArticleIncludeFlags.ARTICLE_TYPE
            | ArticleIncludeFlags.PROPERTIES
            | ArticleIncludeFlags.FILES
            | ArticleIncludeFlags.DISCOUNTS,

            bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL)
        {
            return Article.ArticleSearch(
                new ArticleSearchFilter
                {
                    UserId = userId,
                    TypeId = typeId,
                    LangId = languageId,
                    Flags = flags,
                    Admin = admin
                },

                new ArticleSearchResultOption
                {
                    SortOrder = sortOrder,
                    SortDirection = direction,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    IncludeFlags = includeflags
                });
        }
        public static int CountByUser(int userId, int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, bool admin = false)
        {
            return Article.ArticleSearch(new ArticleSearchFilter
            {
                UserId = userId,
                TypeId = typeId,
                LangId = languageId,
                Flags = flags,
                Admin = admin
            }, new ArticleSearchResultOption
            {
                ResultFlag = ArticleResultFlags.COUNT_ONLY
            }).TotalItemCount;
        }








        public static ArticleResult GetByMultiId(int[] ids, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING,
           ArticleIncludeFlags includeflags =
           ArticleIncludeFlags.PRICES
            | ArticleIncludeFlags.ARTICLE_DETAIL
            | ArticleIncludeFlags.ARTICLE_TYPE
            | ArticleIncludeFlags.CATEGORIES
            | ArticleIncludeFlags.PROPERTIES
            | ArticleIncludeFlags.FILES
            | ArticleIncludeFlags.DISCOUNTS,

           bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, ArticleResultFlags resultFlag = ArticleResultFlags.ITEMS_ONLY, DataContext db = null)
        {
            return Article.ArticleSearch(
                 new ArticleSearchFilter
                 {
                     Ids = ids,
                     LangId = languageId,
                     Flags = flags,
                     Admin = admin
                 },

                 new ArticleSearchResultOption
                 {
                     SortOrder = sortOrder,
                     SortDirection = direction,
                     PageIndex = pageIndex,
                     PageSize = pageSize,
                     IncludeFlags = includeflags,
                     ResultFlag = resultFlag
                 }, db);
        }

        public static Article GetById(int id, int languageId, ArticleIncludeFlags includeFlags = ArticleIncludeFlags.ALL, DataContext db = null)
        {
            Article ret = null;
            var items = Article.ArticleSearch(new ArticleSearchFilter
            {
                ArticleId = id,
                LangId = languageId
            },
            new ArticleSearchResultOption
            {
                ResultFlag = ArticleResultFlags.ITEMS_ONLY,
                PageIndex = 1,
                PageSize = 1,
                IncludeFlags = includeFlags

            }, db);
            if (items.List.Count > 0)
                ret = items.List.FirstOrDefault();

            return ret;
        }

        public static ArticleResult GetByType2(int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
            | ArticleIncludeFlags.ARTICLE_DETAIL
            | ArticleIncludeFlags.ARTICLE_TYPE
            | ArticleIncludeFlags.CATEGORIES
            | ArticleIncludeFlags.PROPERTIES
            | ArticleIncludeFlags.FILES
            | ArticleIncludeFlags.DISCOUNTS,

            bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, ArticleResultFlags resultFlag = ArticleResultFlags.ITEMS_ONLY, DataContext db = null)
        {
            db = new DataContext();
            var items = db.Articles.Include(a => a.ArticleDetail).Include(a => a.ArticleType).Where(a => a.ArticleTypeId == typeId).OrderBy(a => a.ArticleDetail.Min(d => d.ArticleName.Trim())).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var count = db.Articles.Include(a => a.ArticleType).Where(a => a.ArticleTypeId == typeId).Count();
            var articles = new ArticleResult { TotalItemCount = count, List = items, PageIndex = pageIndex, PageSize = pageSize };
            return articles;
        }
        public static ArticleResult GetByType(int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
            | ArticleIncludeFlags.ARTICLE_DETAIL
            | ArticleIncludeFlags.ARTICLE_TYPE
            | ArticleIncludeFlags.CATEGORIES
            | ArticleIncludeFlags.PROPERTIES
            | ArticleIncludeFlags.FILES
            | ArticleIncludeFlags.DISCOUNTS,

            bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, ArticleResultFlags resultFlag = ArticleResultFlags.ITEMS_ONLY, DataContext db = null)
        {

            return Article.ArticleSearch(
                new ArticleSearchFilter
                {
                    TypeId = typeId,
                    LangId = languageId,
                    Flags = flags,
                    Admin = admin
                },

                new ArticleSearchResultOption
                {
                    SortOrder = sortOrder,
                    SortDirection = direction,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    IncludeFlags = includeflags,
                    ResultFlag = resultFlag
                }, db);
        }

        public static ArticleResult GetByTypeCode(string typeCode, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.PROPERTIES
                                | ArticleIncludeFlags.ARTICLE_DETAIL
                                | ArticleIncludeFlags.ARTICLE_TYPE,
             bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, ArticleResultFlags resultFlag = ArticleResultFlags.ITEMS_ONLY,
             DataContext db = null)
        {
            return Article.ArticleSearch(new ArticleSearchFilter
            {
                TypeCode = typeCode,
                LangId = languageId,
                Flags = flags,
                Admin = admin
            }, new ArticleSearchResultOption
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortOrder = sortOrder,
                SortDirection = direction,
                ResultFlag = resultFlag,
                IncludeFlags = includeflags,

            },
            db);

        }
        public static int CountByType(int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, bool admin = false)
        {
            return Article.ArticleSearch(new ArticleSearchFilter
            {
                TypeId = typeId,
                LangId = languageId,
                Flags = flags,
                Admin = admin
            }, new ArticleSearchResultOption
            {
                ResultFlag = ArticleResultFlags.COUNT_ONLY
            }).TotalItemCount;
        }

        public static int CountBySite(int siteId)
        {
            return Article.ArticleSearch(new ArticleSearchFilter
            {
                SiteId = siteId,
            }, new ArticleSearchResultOption
            {
                ResultFlag = ArticleResultFlags.COUNT_ONLY
            }).TotalItemCount;
        }
        public static ArticleResult GetOther(int id, int typeId, int languageId, int categoryId = 0, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 10, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.ASCENDING,
           ArticleIncludeFlags includeflags =
           ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.DISCOUNTS,
            bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, bool countItems = false)
        {
            return GetOther(id, typeId, languageId, categoryId, flags, pageIndex, pageSize, sortOrder, direction, includeflags, admin, option, countItems ? ArticleResultFlags.COUNT_ONLY : ArticleResultFlags.ALL);
        }
        public static ArticleResult GetOther(int id, int typeId, int languageId, int categoryId = 0, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 10, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.ASCENDING,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.DISCOUNTS,
             bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, ArticleResultFlags resultFlag = ArticleResultFlags.ITEMS_ONLY)
        {
            return Article.ArticleSearch(new ArticleSearchFilter
            {
                TypeId = typeId,
                LangId = languageId,
                CategoryId = categoryId,
                Flags = flags,
                ExcludeIds = new int[] { id },
                Admin = admin
            }, new ArticleSearchResultOption
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortOrder = sortOrder,
                SortDirection = direction,
                ResultFlag = resultFlag,
                IncludeFlags = includeflags
            });

        }


        public static ArticleResult GetByDiscount(int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection direction = SortDirection.DESCENDING, ArticleSearchOption option = ArticleSearchOption.ALL, bool Admin = false)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.Articles
                             join d in db.ArticleDetails on a.Id equals d.ArticleId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId

                             where d.LanguageId == languageId && a.ArticleTypeId == typeId
                             && td.LanguageId == languageId
                             && (((int)a.Flags & (int)flags) > 0)

                             && (a.Discounts.Any(di => di.DateStart <= DateTime.Now && di.DateEnd >= DateTime.Now) && d.LanguageId == languageId && td.LanguageId == languageId)
                             || (a.Categories.Any(ca => ca.Discounts.Any(di => di.DateStart <= DateTime.Now && di.DateEnd >= DateTime.Now)) && d.LanguageId == languageId && td.LanguageId == languageId)
                             select new
                             {
                                 a = a,
                                 d = d,
                                 t = t,
                                 td = td
                             });

                if (!Admin)
                    query = query.Where(a => ((int)a.a.Flags & (int)ArticleFlags.INACTIVE) <= 0);

                if (option == ArticleSearchOption.HAS_TITLE_ONLY)
                    query = query.Where(q => q.d.ArticleName.Trim() != "");
                if (option == ArticleSearchOption.HAS_SHORTDESC_ONLY)
                    query = query.Where(q => q.d.ShortDesc.Trim() != "");
                if (option == ArticleSearchOption.HAS_DESC_ONLY)
                    query = query.Where(q => q.d.Description.Trim() != "");
                var article = query.Take(0);
                switch (sortOrder)
                {
                    case ArticleSortOrder.SORT_ORDER:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    case ArticleSortOrder.DATE_CREATED:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;

                    case ArticleSortOrder.ARTICLE_NAME:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.d.ArticleName).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.d.ArticleName).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    case ArticleSortOrder.PRICE:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.Price).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.Price).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    case ArticleSortOrder.RATING:
                        if (direction == SortDirection.ASCENDING)
                            article = query.OrderBy(x => x.a.Rating).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        else
                            article = query.OrderByDescending(x => x.a.Rating).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        break;
                    default:
                        break;
                }
                var articleList = article.ToList();
                var propvals = db.ArticlePropertyValues.Where(pv => article.Select(a => a.a.Id).Contains(pv.ArticleId)).Include(pv => pv.Property).ToList();
                var ret = new List<Article>();
                foreach (var item in article.ToList())
                {
                    item.a.ArticleDetail = new VList<ArticleDetail>() { item.d };
                    item.a.PropertyValues = propvals.Where(pv => pv.ArticleId == item.a.Id).ToList();
                    item.a.Prices = db.Prices.Include(p => p.Currency).Where(p => p.ArticleId == item.a.Id).ToList();
                    item.a.CurrentDiscount = db.Discounts.Include(d => d.Currency).OrderBy(a => a.DateStart).Where(d => (d.Articles.Select(ad => ad.Id).Contains(item.a.Id) || d.Categories.Any(cd => cd.Articles.Any(a => a.Id == item.a.Id)) || d.AllItems)).OrderByDescending(a => a.DateEnd).FirstOrDefault(a => a.DateStart <= DateTime.Now && a.DateEnd >= DateTime.Now);
                    item.a.LanguageId = item.t.LanguageId = languageId;
                    item.a.Categories = db.Categories.Include(c => c.CategoryDetail).Where(c => c.Articles.Select(arc => arc.Id).Contains(item.a.Id)).ToList();
                    (item.a.Categories as List<Category>).ForEach(c => c.LanguageId = languageId);

                    item.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { item.td };
                    item.a.ArticleType = item.t;
                    ret.Add(item.a);
                }
                var count = query.Count();
                return new ArticleResult { List = ret, TotalItemCount = count, PageIndex = pageIndex, PageSize = pageSize };
            }
        }
        public static int CountByDiscount(int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, bool Admin = false)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.Articles
                             join d in db.ArticleDetails on a.Id equals d.ArticleId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId

                             where d.LanguageId == languageId && td.LanguageId == languageId
                             && a.ArticleTypeId == typeId
                             && (((int)a.Flags & (int)flags) > 0)
                             && a.Discounts.Any(di => di.DateStart <= DateTime.Now && di.DateEnd >= DateTime.Now)
                             || a.Categories.Any(ca => ca.Discounts.Any(di => di.DateStart <= DateTime.Now && di.DateEnd >= DateTime.Now))
                             select a);

                if (!Admin)
                    query = query.Where(a => ((int)a.Flags & (int)ArticleFlags.INACTIVE) <= 0);

                return query.Count();
            }
        }
        class ARC
        {
            public int ArticleId { get; set; }
            public int CategoryId { get; set; }
        }
        public static ICollection<Article> GetByCategories(int[] categories, int langId, ArticleFlags flag, int itemPerGroup, ArticleSortOrder sortOrder, SortDirection direction, ArticleIncludeFlags includeFlag = ArticleIncludeFlags.PROPERTIES)
        {
            using (DataContext db = new DataContext())
            {
                var str = string.Join(",", categories);
                var query1 = "WITH TOPTEN AS ( " +
                    "SELECT Article_Id as ArticleID,Category_Id as CategoryId, Row_Number() " +
                    " over (" +
                    "     PARTITION BY Category_ID" +
                    "     order by Article_Id" +
                    " ) AS RowNo " +
                   " FROM ArticleCategories where category_id IN ( " + str + ") " +
                   "  ) " +
                " SELECT ArticleID,CategoryId FROM TOPTEN WHERE RowNo <= " + itemPerGroup.ToString();

                var arc = db.Database.SqlQuery<ARC>(query1).ToList();
                var ids = arc.Select(a => a.ArticleId).ToList();

                var query = db.Articles.Where(a => ids.Contains(a.Id));

                if (includeFlag.HasFlag(ArticleIncludeFlags.ARTICLE_DETAIL))
                    query = query.Include(a => a.ArticleDetail.Select(d => d.Body));
                else
                    query = query.Include(a => a.ArticleDetail);

                if (includeFlag.HasFlag(ArticleIncludeFlags.ARTICLE_TYPE))
                    query = query.Include(a => a.ArticleType.ArticleTypeDetail);

                if (includeFlag.HasFlag(ArticleIncludeFlags.COMMENTS))
                    query = query.Include(a => a.Comments);
                if (includeFlag.HasFlag(ArticleIncludeFlags.FILES))
                    query = query.Include(a => a.ArticleFiles.Select(f => f.ArticleFileDetail));
                if (includeFlag.HasFlag(ArticleIncludeFlags.KEYWORDS))
                    query = query.Include(a => a.Keywords);
                if (includeFlag.HasFlag(ArticleIncludeFlags.CATEGORIES))
                    query = query.Include(a => a.Categories.Select(c => c.CategoryDetail));
                if (includeFlag.HasFlag(ArticleIncludeFlags.DISCOUNTS))
                    query = query.Include(a => a.Discounts);
                if (includeFlag.HasFlag(ArticleIncludeFlags.PRICES))
                    query = query.Include(a => a.Prices.Select(c => c.Currency));
                if (includeFlag.HasFlag(ArticleIncludeFlags.PROPERTIES))
                    query = query.Include(a => a.PropertyValues.Select(pv => pv.Property));
                var list = query.ToList();


                //if (includeFlag.HasFlag(ArticleIncludeFlags.DISCOUNTS))
                //{
                //    var currentDiscounts = db.Discounts.Include(d => d.Currency).Include(d => d.Articles).Include(d => d.Categories)
                //                    .Where(d => (d.Status == DiscountStatus.ACTIVE && d.DateStart <= DateTime.Now && d.DateEnd >= DateTime.Now)
                //                        && (d.AllItems
                //                        || d.Articles.Any(ad => ids.Contains(ad.Id))
                //                        || d.Categories.Any(c => c.Articles.Any(ac => ids.Contains(ac.Id)))))
                //                    .ToList();

                //    list.ForEach(a =>
                //    {
                //        a.CurrentDiscount = currentDiscounts.OrderBy(d => d.DateStart)
                //            .FirstOrDefault(d => d.AllItems || d.Articles.Any(ad => ad.Id == a.Id) || d.Categories.Any(c => c.Articles.Any(ar => ar.Id == a.Id)));
                //        a.CurrentCategory = new Category { Id = arc.FirstOrDefault(i => i.ArticleId == a.Id).CategoryId };
                //    });
                //}
                //else
                list.ForEach(a =>
                {
                    a.CurrentCategory = new Category { Id = arc.FirstOrDefault(i => i.ArticleId == a.Id).CategoryId };
                });

                var returnList =
                   (from item in list
                    let index = ids.IndexOf(item.Id)
                    orderby index
                    select item).ToList();
                return returnList;
            }
                
                
        }
        public static ICollection<Article> GetByCategories(int[] categories, int langId, ArticleFlags flag = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleIncludeFlags includeFlag = ArticleIncludeFlags.PROPERTIES)
        {
            using (DataContext db = new DataContext())
            {
                var arc = db.Categories.Where(c => categories.Contains(c.Id))
                          .SelectMany(c => c.Articles.Where(a => (a.Flags & flag) > 0)
                              .OrderByDescending(a => a.DateCreated)
                              .Select(a => new { CategoryId = c.Id, ArticleId = a.Id })).ToList();

                var ids = arc.Select(a => a.ArticleId).ToList();
                var query = db.Articles
                    .Include(a => a.ArticleDetail)
                    .Where(a => ids.Contains(a.Id) && a.ArticleDetail.Any(d => d.LanguageId == langId) && a.ArticleType.ArticleTypeDetail.Any(d => d.LanguageId == langId));

                if (includeFlag.HasFlag(ArticleIncludeFlags.ARTICLE_DETAIL))
                    query = query.Include(a => a.ArticleDetail.Select(d => d.Body));
                else
                    query = query.Include(a => a.ArticleDetail);
                if (includeFlag.HasFlag(ArticleIncludeFlags.ARTICLE_TYPE))
                    query = query.Include(a => a.ArticleType.ArticleTypeDetail);
                if (includeFlag.HasFlag(ArticleIncludeFlags.COMMENTS))
                    query = query.Include(a => a.Comments);
                if (includeFlag.HasFlag(ArticleIncludeFlags.FILES))
                    query = query.Include(a => a.ArticleFiles.Select(f => f.ArticleFileDetail));
                if (includeFlag.HasFlag(ArticleIncludeFlags.KEYWORDS))
                    query = query.Include(a => a.Keywords);
                if (includeFlag.HasFlag(ArticleIncludeFlags.CATEGORIES))
                    query = query.Include(a => a.Categories.Select(c => c.CategoryDetail));
                if (includeFlag.HasFlag(ArticleIncludeFlags.DISCOUNTS))
                    query = query.Include(a => a.Discounts);
                if (includeFlag.HasFlag(ArticleIncludeFlags.PRICES))
                    query = query.Include(a => a.Prices.Select(c => c.Currency));
                if (includeFlag.HasFlag(ArticleIncludeFlags.PROPERTIES))
                    query = query.Include(a => a.PropertyValues.Select(pv => pv.Property));


                //int modelId = ArticleType.GetIdByCode("BRAND");
                switch (sortOrder)
                {
                    case ArticleSortOrder.SORT_ORDER:
                        if (direction == SortDirection.ASCENDING)
                            query = query.OrderBy(x => x.SortOrder);
                        else
                            query = query.OrderByDescending(x => x.SortOrder);
                        break;
                    case ArticleSortOrder.DATE_CREATED:
                        if (direction == SortDirection.ASCENDING)
                            query = query.OrderBy(x => x.DateCreated);
                        else
                            query = query.OrderByDescending(x => x.DateCreated);
                        break;
                    case ArticleSortOrder.ARTICLE_NAME:
                        if (direction == SortDirection.ASCENDING)
                            query = query.OrderBy(x => x.ArticleDetail.Min(d => d.ArticleName.Trim()));
                        else
                            query = query.OrderByDescending(x => x.ArticleDetail.Max(d => d.ArticleName.Trim()));
                        break;
                    case ArticleSortOrder.PRICE:
                        if (direction == SortDirection.ASCENDING)
                            query = query.OrderBy(x => x.Prices.Max(pr => pr.PriceShortOrder));
                        else
                            query = query.OrderByDescending(x => x.Prices.Max(pr => pr.PriceShortOrder));
                        break;
                    //case ArticleSortOrder.MODEL:
                    //    if (direction == SortDirection.ASCENDING)
                    //        query = query.OrderBy(x => x.Categories.Where(m => m.ArticleTypeId == modelId).SelectMany(c => c.CategoryDetail).Min(c => c.CategoryName));
                    //    else
                    //        query = query.OrderByDescending(x => x.Categories.Where(m => m.ArticleTypeId == modelId).SelectMany(c => c.CategoryDetail).Max(c => c.CategoryName));
                    //    break;
                    default:
                        query = query.OrderBy(x => x.SortOrder);
                        break;
                }

                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var list = query.ToList();
                return list;
            }
              
        }

        public static int CountByCategories(int[] categories, int langId, int categoryId = 0, ArticleFlags flag = ArticleFlags.ACTIVE)
        {
            using (DataContext db = new DataContext())
            {
                var arc = db.Categories.Where(c => categories.Contains(c.Id))
                                          .SelectMany(c => c.Articles.Where(a => (a.Flags & flag) > 0)
                                              .OrderByDescending(a => a.DateCreated)
                                              .Select(a => new { CategoryId = c.Id, ArticleId = a.Id })).ToList();

                var ids = arc.Select(a => a.ArticleId).ToList();
                var query = db.Articles.Include(a => a.ArticleDetail)
                    .Where(a => ids.Contains(a.Id) && a.ArticleDetail.Any(d => d.LanguageId == langId) && a.ArticleType.ArticleTypeDetail.Any(d => d.LanguageId == langId));
                if (categoryId > 0)
                    query = query.Where(a => a.Categories.Any(c => c.Id == categoryId));
                return query.Count();
            }
              
        }

        public static ArticleResult GetByCategory(int categoryId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, VCMS.MVC4.Data.SortDirection direction = SortDirection.DESCENDING,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.FILES
                            | ArticleIncludeFlags.PRICES
                            | ArticleIncludeFlags.PROPERTIES
                            | ArticleIncludeFlags.CATEGORIES
                            | ArticleIncludeFlags.DISCOUNTS
                            | ArticleIncludeFlags.ARTICLE_TYPE
                            | ArticleIncludeFlags.ARTICLE_DETAIL,
            bool admin = false, ArticleSearchOption option = ArticleSearchOption.ALL, bool includeChildren = false,
            ArticleResultFlags resultFlag = ArticleResultFlags.ITEMS_ONLY)


        {
            using (DataContext db = new DataContext())
            {
                var items = db.Articles
                    .Include(a => a.Categories)
                    .Include(a => a.ArticleDetail)
                    .Include(a => a.ArticleType)
                    .Include(a => a.Prices.Select(c => c.Currency))
                    .Include(a => a.PropertyValues.Select(pv => pv.Property)).Where(a => a.Categories.Any(ac => ac.Id == categoryId)).OrderBy(a => a.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var count = db.Articles.Where(a => a.Categories.Any(ac => ac.Id == categoryId)).Count();
                var articles = new ArticleResult { TotalItemCount = count, List = items, PageIndex = pageIndex, PageSize = pageSize };
                return articles;
                //return ArticleSearch(new ArticleSearchFilter
                //{
                //    CategoryId = categoryId,
                //    LangId = languageId,
                //    Flags = flags,
                //    Admin = admin
                //}, new ArticleSearchResultOption
                //{
                //    PageSize = pageSize,
                //    PageIndex = pageIndex,
                //    SortOrder = sortOrder,
                //    SortDirection = direction,
                //    IncludeChildCategories = includeChildren,
                //    IncludeFlags = includeflags,
                //    ResultFlag = resultFlag
                //},
                //db);
            }

        }

        public static int CountByCategory(int categoryId, int categoryId2, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, bool admin = false)
        {
            using (DataContext db = new DataContext())
            {
                var orderQ = db.Articles.AsNoTracking() as IQueryable<Article>;
                orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.LanguageId == languageId));
                orderQ = orderQ.Where(a => (a.Flags & flags) > 0);
                if (categoryId > 0)
                    orderQ = orderQ.Where(a => a.Categories.Any(c => c.Id == categoryId));
                if (categoryId2 > 0)
                    orderQ = orderQ.Where(a => a.Categories.Any(c => c.Id == categoryId2));
                return orderQ.Count();
            }
                
        }

        public static int CountByCategory(int categoryId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE, bool admin = false)
        {
            return ArticleSearch(new ArticleSearchFilter
            {
                CategoryId = categoryId,
                LangId = languageId,
                Flags = flags,
                Admin = admin
            }, new ArticleSearchResultOption
            {
                ResultFlag = ArticleResultFlags.COUNT_ONLY
            }).TotalItemCount;
        }

        public static ArticleResult Search(int typeId, int languageId, string keyword, int categoryId = 0, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleFlags flags = ArticleFlags.ACTIVE,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.PROPERTIES
                                | ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.DISCOUNTS
                                | ArticleIncludeFlags.ARTICLE_TYPE
                                | ArticleIncludeFlags.ARTICLE_DETAIL,
            bool admin = false)
        {
            return ArticleSearch(new ArticleSearchFilter
            {
                TypeId = typeId,
                Flags = flags,
                LangId = languageId,
                SearchTerm = keyword,
                CategoryId = categoryId,
                Admin = admin
            }, new ArticleSearchResultOption
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortOrder = sortOrder,
                SortDirection = direction,
                IncludeFlags = includeflags
            });

        }


        public static ArticleResult SearchTag(int typeId, int languageId, string keyword, int categoryId = 0, int pageIndex = 1, int pageSize = 17, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleFlags flags = ArticleFlags.ACTIVE,
            ArticleIncludeFlags includeflags =
            ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.PROPERTIES
                                | ArticleIncludeFlags.PRICES
                                | ArticleIncludeFlags.DISCOUNTS
                                | ArticleIncludeFlags.ARTICLE_TYPE
                                | ArticleIncludeFlags.ARTICLE_DETAIL,
            bool admin = false)
        {
            return Article.ArticleSearch(new ArticleSearchFilter
            {
                TypeId = typeId,
                LangId = languageId,
                Tag = keyword,
                CategoryId = categoryId,
                Flags = flags,
                Admin = admin
            }, new ArticleSearchResultOption
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortOrder = sortOrder,
                SortDirection = direction,
                IncludeFlags = includeflags
            });

        }

        public static int SearchCount(int typeId, int languageId, string keyword, int categoryId = 0, ArticleFlags flags = ArticleFlags.ACTIVE)
        {
            using (DataContext db = new DataContext())
            {
                if (string.IsNullOrEmpty(keyword)) keyword = "";
                var query = from a in db.Articles
                            join d in db.ArticleDetails on a.Id equals d.ArticleId
                            where d.LanguageId == languageId && a.ArticleTypeId == typeId
                            && (d.ArticleName.Contains(keyword) || keyword == "")
                            && (categoryId == 0 || a.Categories.Any(c => c.Id == categoryId))
                            && (((int)a.Flags & (int)flags) > 0)
                            select a;
                return query.Count();
            }
        }

        public static ArticleResult Search(int languageId, string keyword, int typeid = 0, int categoryId = 0, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleFlags flags = ArticleFlags.ACTIVE, ArticleIncludeFlags includeflags = ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.DISCOUNTS | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.ARTICLE_DETAIL, bool admin = false)
        {
            return Search(typeid, languageId, keyword, categoryId, pageIndex, pageSize, sortOrder, direction, flags, includeflags, admin);
        }

        public static ArticleResult SearchTag(int languageId, string keyword, int categoryId = 0, int pageIndex = 1, int pageSize = 20, ArticleSortOrder sortOrder = ArticleSortOrder.SORT_ORDER, SortDirection direction = SortDirection.DESCENDING, ArticleFlags flags = ArticleFlags.ACTIVE, ArticleIncludeFlags includeflags = ArticleIncludeFlags.PRICES | ArticleIncludeFlags.PROPERTIES | ArticleIncludeFlags.PRICES | ArticleIncludeFlags.DISCOUNTS | ArticleIncludeFlags.ARTICLE_TYPE | ArticleIncludeFlags.ARTICLE_DETAIL, bool admin = false)
        {
            return SearchTag(0, languageId, keyword, categoryId, pageIndex, pageSize, sortOrder, direction, flags, includeflags, admin);
        }
        public static int SearchCount(int languageId, string keyword, int categoryId = 0, ArticleFlags flags = ArticleFlags.ACTIVE)
        {
            using (DataContext db = new DataContext())
            {

                if (string.IsNullOrEmpty(keyword))
                    keyword = "";

                return ArticleSearch(new ArticleSearchFilter
                {
                    Flags = flags,
                    LangId = languageId,
                    SearchTerm = keyword,
                    CategoryId = categoryId,
                    Admin = false
                }, new ArticleSearchResultOption
                {
                    ResultFlag = ArticleResultFlags.COUNT_ONLY,
                }).TotalItemCount;
            }
        }
        public static int SearchCountTag(int languageId, string keyword, int categoryId = 0)
        {
            using (DataContext db = new DataContext())
            {
                return Article.ArticleSearch(new ArticleSearchFilter
                {
                    LangId = languageId,
                    Tag = keyword,
                    Admin = false
                }, new ArticleSearchResultOption
                {
                    ResultFlag = ArticleResultFlags.COUNT_ONLY
                }).TotalItemCount;
            }
        }
        public static ArticleResult AdvanceSearch(int typeId, string keyword, ArticleIncludeFlags resultOptions, Dictionary<int, string> properties)
        {

            using (DataContext db = new DataContext())
            {
                ArticleSearchFilter conditions = new ArticleSearchFilter { TypeId = typeId, SearchTerm = keyword };

                var query = db.Articles.Where(a => a.ArticleTypeId == typeId).Where(a => a.ArticleDetail.Any(d => d.ArticleName == keyword));

                foreach (var prop in properties)
                {
                    query = query.Where(a => a.PropertyValues.Any(pv => pv.PropertyId == prop.Key && pv.Value == prop.Value));
                }
                return new ArticleResult { };
            }
        }
        private static IQueryable<Article>
            BuildConditionQuery(ArticleSearchFilter cond, ArticleSearchResultOption resultOptions, DataContext db)
        {

            var orderQ = db.Articles.AsNoTracking() as IQueryable<Article>;
            if (cond.UserId > 0)
                orderQ = orderQ.Where(a => a.UserCreated == cond.UserId);
            if (cond.TypeId > 0)
                orderQ = orderQ.Where(a => a.ArticleTypeId == cond.TypeId);
            if (!string.IsNullOrWhiteSpace(cond.TypeCode))
                orderQ = orderQ.Where(a => a.ArticleType.Code.Equals(cond.TypeCode));
            if (cond.LangId > 0)
                orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.LanguageId == cond.LangId));
            if (cond.Flags > 0)
                orderQ = orderQ.Where(a => (a.Flags & cond.Flags) > 0);
            if (!cond.Admin)
                orderQ = orderQ.Where(a => ((int)a.Flags & (int)ArticleFlags.INACTIVE) <= 0);
            if (cond.CategoryId > 0)
            {
                if (resultOptions.IncludeChildCategories)
                {
                    var categories = Category.BuildTreeID(cond.CategoryId);
                    orderQ = orderQ.Where(a => a.Categories.Any(c => categories.Contains(c.Id)));
                }
                else
                    orderQ = orderQ.Where(a => a.Categories.Any(c => c.Id == cond.CategoryId));
            }

            if (cond.ArticleId > 0)
                orderQ = orderQ.Where(a => a.Id == cond.ArticleId);

            if (!string.IsNullOrWhiteSpace(cond.SearchTerm))
            {
                if (!cond.Admin)
                    orderQ = orderQ.Where(a => (((int)a.ArticleType.Flag & (int)ArticleTypeFlags.SEARCH) > 0));

                switch (cond.KeywordMatchType)
                {
                    case StringMatchType.EQUALS:
                        orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.ArticleName.Equals(cond.SearchTerm, StringComparison.OrdinalIgnoreCase) || d.UrlFriendly.Equals(cond.SearchTerm, StringComparison.OrdinalIgnoreCase) || d.Description.Equals(cond.SearchTerm, StringComparison.OrdinalIgnoreCase) || d.ShortDesc.Equals(cond.SearchTerm, StringComparison.OrdinalIgnoreCase)));
                        break;
                    case StringMatchType.CONTAINS:

                        orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.ArticleName.Contains(cond.SearchTerm)
                            || d.UrlFriendly.Contains(cond.SearchTerm)
                            //|| d.Description.Contains(cond.SearchTerm) 
                            //|| d.ShortDesc.Contains(cond.SearchTerm)
                            ));
                        break;
                    case StringMatchType.STARTSWITH:
                        orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.ArticleName.StartsWith(cond.SearchTerm, StringComparison.OrdinalIgnoreCase)));
                        break;
                    case StringMatchType.ENDSWITH:
                        orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.ArticleName.EndsWith(cond.SearchTerm, StringComparison.OrdinalIgnoreCase)));
                        break;
                    default:
                        orderQ = orderQ.Where(a => a.ArticleDetail.Any(d => d.ArticleName.Contains(cond.SearchTerm)));
                        break;
                }
            }

            //if (!string.IsNullOrWhiteSpace(cond.Tag))
            //{
            //    orderQ = orderQ.Where(a => a.Keywords.Any(kw => kw.Tag.Equals(cond.Tag, StringComparison.OrdinalIgnoreCase)));
            //}
            if (!string.IsNullOrWhiteSpace(cond.Tag))
            {
                orderQ = orderQ.Where(a => a.Keywords.Any(kw => kw.Tag.Trim().Equals(cond.Tag.Trim(), StringComparison.OrdinalIgnoreCase)));
            }
            if (cond.ExcludeIds != null)
            {
                if (cond.ExcludeIds.Count() == 1)
                {
                    var exID = cond.ExcludeIds.FirstOrDefault();
                    orderQ = orderQ.Where(a => a.Id != exID);
                }

                else
                    orderQ = orderQ.Where(a => !cond.ExcludeIds.Contains(a.Id));
            }
            if (cond.Ids != null)
                orderQ = orderQ.Where(a => cond.Ids.Contains(a.Id));

            var sortOrder = resultOptions.SortOrder;
            var direction = resultOptions.SortDirection;

            //int modelId = ArticleType.GetIdByCode("BRAND");
            switch (sortOrder)
            {
                case ArticleSortOrder.SORT_ORDER:
                    if (direction == SortDirection.ASCENDING)
                        orderQ = orderQ.OrderBy(x => x.SortOrder);
                    else
                        orderQ = orderQ.OrderByDescending(x => x.SortOrder);
                    break;
                case ArticleSortOrder.DATE_CREATED:
                    if (direction == SortDirection.ASCENDING)
                        orderQ = orderQ.OrderBy(x => x.DateCreated);
                    else
                        orderQ = orderQ.OrderByDescending(x => x.DateCreated);
                    break;
                case ArticleSortOrder.ARTICLE_NAME:
                    if (direction == SortDirection.ASCENDING)
                        orderQ = orderQ.OrderBy(x => x.ArticleDetail.Min(d => d.ArticleName.Trim()));
                    //orderQ = orderQ.OrderBy(x => x.ArticleDetail.OrderBy(d => d.ArticleName).Select(d => d.ArticleName).FirstOrDefault());
                    else
                        orderQ = orderQ.OrderByDescending(x => x.ArticleDetail.Max(d => d.ArticleName.Trim()));
                    //orderQ = orderQ.OrderByDescending(x => x.ArticleDetail.OrderByDescending(d => d.ArticleName).Select(d => d.ArticleName).FirstOrDefault());
                    break;
                case ArticleSortOrder.PRICE:
                    if (direction == SortDirection.ASCENDING)
                        orderQ = orderQ.OrderBy(x => x.Prices.Max(pr => pr.PriceShortOrder));
                    else
                        orderQ = orderQ.OrderByDescending(x => x.Prices.Max(pr => pr.PriceShortOrder));
                    break;
                //case ArticleSortOrder.MODEL:
                //    if (direction == SortDirection.ASCENDING)
                //        orderQ = orderQ.OrderBy(x => x.Categories.Where(m => m.ArticleTypeId == modelId).SelectMany(c => c.CategoryDetail).Min(c => c.CategoryName));
                //    else
                //        orderQ = orderQ.OrderByDescending(x => x.Categories.Where(m => m.ArticleTypeId == modelId).SelectMany(c => c.CategoryDetail).Max(c => c.CategoryName));
                //    break;
                default:
                    orderQ = orderQ.OrderBy(x => x.SortOrder);
                    break;
            }
            return orderQ;
        }

        public static ArticleResult GetProperties(ArticleSearchFilter cond, ArticleSearchResultOption resultOptions)
        {
            using (DataContext db = new DataContext())
            {
                var orderQ = BuildConditionQuery(cond, resultOptions, db);
                var pageIndex = resultOptions.PageIndex;
                var pageSize = resultOptions.PageSize;

                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = 20;
                //var idList = orderQ.Skip(pageSize * (pageIndex - 1)).Take(pageSize).Select(a=>a.Id).ToList();
                var query = orderQ.Skip(pageSize * (pageIndex - 1)).Take(pageSize);// db.Articles.AsNoTracking().Where(a=> idList.Contains( a.Id)) as IQueryable<Article>;
                var xitems = (from a in query

                              select new
                              {

                                  a.Id,
                                  a.DateCreated,
                                  a.ImageUrl,


                                  PropertyValues = db.ArticlePropertyValues.Where(apv => apv.ArticleId == a.Id && (apv.LanguageId == null || apv.LanguageId == cond.LangId)).Select(apv => new { apv.ArticleId, apv.PropertyId, apv.Property.Code, apv.Value }),
                                  ArticleDetail = db.ArticleDetails.Where(ad => ad.ArticleId == a.Id && ad.LanguageId == cond.LangId)
                                  .Select(ad => new { ad.ArticleName, ad.UrlFriendly, ad.LanguageId })

                              }).ToList();
                var items = new List<Article>();
                xitems.ForEach(item =>
                {
                    var ar = new Article();
                    ar.Id = item.Id;
                    ar.ImageUrl = item.ImageUrl;
                    ar.DateCreated = item.DateCreated;
                    ar.ArticleDetail = new VList<Data.ArticleDetail>();

                    foreach (var ad in item.ArticleDetail)
                        ar.ArticleDetail.Add(new Data.ArticleDetail { ArticleId = item.Id, ArticleName = ad.ArticleName, LanguageId = ad.LanguageId, UrlFriendly = ad.UrlFriendly });
                    ar.PropertyValues = new List<ArticlePropertyValue>();
                    foreach (var pv in item.PropertyValues)
                        ar.PropertyValues.Add(new Data.ArticlePropertyValue { ArticleId = item.Id, Value = pv.Value, Property = new Property { Id = pv.PropertyId, Code = pv.Code } });
                    items.Add(ar);
                });
                return new ArticleResult { List = items };
            }
        }

        public static ArticleResult GetDetail(ArticleSearchFilter cond, ArticleSearchResultOption resultOptions)
        {
            using (DataContext db = new DataContext())
            {
                var orderQ = BuildConditionQuery(cond, resultOptions, db);
                var pageIndex = resultOptions.PageIndex;
                var pageSize = resultOptions.PageSize;
                var count = resultOptions.ResultFlag != ArticleResultFlags.ITEMS_ONLY ? orderQ.Count() : 0;
                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = 20;
                //var idList = orderQ.Skip(pageSize * (pageIndex - 1)).Take(pageSize).Select(a => a.Id).ToList();
                var query = orderQ.Skip(pageSize * (pageIndex - 1)).Take(pageSize);// db.Articles.AsNoTracking().Where(a => idList.Contains( a.Id)) as IQueryable<Article>;
                var xitems = (from a in query

                              select new
                              {

                                  a.Id,
                                  a.DateCreated,
                                  a.ImageUrl,
                                  a.Flags,
                                  a.SortOrder,
                                  a.Status,

                                  ArticleDetail = db.ArticleDetails.Where(ad => ad.ArticleId == a.Id && ad.LanguageId == cond.LangId)
                                  .Select(ad => new { ad.ArticleName, ad.UrlFriendly, ad.LanguageId, ad.Body.Description, ad.Body.SEODescription, ad.Body.ShortDesc, ad.Body.SEOKeywords })

                              }).ToList();
                var items = new List<Article>();
                xitems.ForEach(item =>
                {
                    var ar = new Article();
                    ar.Id = item.Id;
                    ar.ImageUrl = item.ImageUrl;
                    ar.DateCreated = item.DateCreated;
                    ar.Status = item.Status;
                    ar.Flags = item.Flags;
                    ar.SortOrder = item.SortOrder;

                    ar.ArticleDetail = new VList<Data.ArticleDetail>();

                    foreach (var ad in item.ArticleDetail)
                        ar.ArticleDetail.Add(new Data.ArticleDetail { ArticleId = item.Id, ArticleName = ad.ArticleName, LanguageId = ad.LanguageId, UrlFriendly = ad.UrlFriendly });
                    items.Add(ar);
                });

                return new ArticleResult { List = items, TotalItemCount = count };
            }
        }
        public static ArticleResult ArticleSearch(ArticleSearchFilter cond, ArticleSearchResultOption resultOptions, DataContext dbContext = null)
        {
            var db = dbContext ?? new DataContext();

            var orderQ = BuildConditionQuery(cond, resultOptions, db);

            var pageIndex = resultOptions.PageIndex;
            var pageSize = resultOptions.PageSize;

            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 20;
            /// count the items matched conditions (not paging)
            var count = 0;
            if (resultOptions.ResultFlag != ArticleResultFlags.ITEMS_ONLY) count = orderQ.Count();

            var items = new List<Article>();
            //var returnList = new List<Article>();
            if (resultOptions.ResultFlag != ArticleResultFlags.COUNT_ONLY)
            {
                //var query = db.Articles.AsNoTracking() as IQueryable<Article>;
                var includeFlags = resultOptions.IncludeFlags;
                //var idList = cond.ArticleId > 0 ? (new[] { new { Id= cond.ArticleId, Index=1} }).AsQueryable() : orderQ.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select((x,i) => new { Id = x.Id, Index = i } );//.ToList();

                var query = orderQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                if (includeFlags.HasFlag(ArticleIncludeFlags.ARTICLE_DETAIL))
                    query = query.Include(a => a.ArticleDetail.Select(d => d.Body));
                else
                    query = query.Include(a => a.ArticleDetail);

                if (includeFlags.HasFlag(ArticleIncludeFlags.ARTICLE_TYPE))
                    query = query.Include(a => a.ArticleType.ArticleTypeDetail);

                if (includeFlags.HasFlag(ArticleIncludeFlags.COMMENTS))
                    query = query.Include(a => a.Comments);
                if (includeFlags.HasFlag(ArticleIncludeFlags.FILES))
                    query = query.Include(a => a.ArticleFiles.Select(f => f.ArticleFileDetail));
                if (includeFlags.HasFlag(ArticleIncludeFlags.KEYWORDS))
                    query = query.Include(a => a.Keywords);
                if (includeFlags.HasFlag(ArticleIncludeFlags.CATEGORIES))
                    query = query.Include(a => a.Categories.Select(c => c.CategoryDetail));
                if (includeFlags.HasFlag(ArticleIncludeFlags.DISCOUNTS))
                    query = query.Include(a => a.Discounts);
                if (includeFlags.HasFlag(ArticleIncludeFlags.PRICES))
                    query = query.Include(a => a.Prices.Select(c => c.Currency));
                if (includeFlags.HasFlag(ArticleIncludeFlags.PROPERTIES))
                    query = query.Include(a => a.PropertyValues.Select(pv => pv.Property));

                //query = query.Where(a => idList.Contains(a.Id)) ;
                items = query.ToList();
                if (includeFlags.HasFlag(ArticleIncludeFlags.CATEGORIES))
                    items.ForEach(a => { a.CurrentCategory = a.Categories.FirstOrDefault(); });

                //chờ chinh sua
                //if (includeFlags.HasFlag(ArticleIncludeFlags.DISCOUNTS))
                //{
                //    var currentDiscounts = db.Discounts.Include(d => d.Currency).Include(d => d.Articles).Include(d => d.Categories)

                //        .Where(d => (d.Status == DiscountStatus.ACTIVE && d.DateStart <= DateTime.Now && d.DateEnd >= DateTime.Now)
                //            && (d.AllItems
                //            || d.Articles.Any(ad => orderQ.Any(id => id.Id == ad.Id))
                //            || d.Categories.Any(c => c.Articles.Any(ac => orderQ.Any(id => id.Id == ac.Id)))))
                //        .ToList();
                //    items.ForEach(item =>
                //    {

                //        item.CurrentDiscount = currentDiscounts.OrderBy(d => d.DateStart)
                //            .FirstOrDefault(d => d.AllItems || d.Articles.Any(ad => ad.Id == item.Id) || d.Categories.Any(c => c.Articles.Any(a => a.Id == item.Id)));

                //    });
                //}

                items.ForEach(item =>
                {
                    item.LanguageId = cond.LangId;
                    if (includeFlags.HasFlag(ArticleIncludeFlags.ARTICLE_TYPE))
                        item.ArticleType.LanguageId = cond.LangId;
                });


                //returnList = items;
                //(from item in items
                // let index = idList.IndexOf(item.Id)
                // orderby index
                // select item).ToList();

            }
            if (dbContext == null)
                db.Dispose();
            return new ArticleResult { TotalItemCount = count, List = items, PageIndex = pageIndex, PageSize = pageSize };
        }

        //private readonly static ILog logger = LogManager.GetLogger(typeof(VCMS.MVC4.Data.Article));
        #endregion
    }
}