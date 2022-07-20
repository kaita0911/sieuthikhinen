using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace VCMS.MVC4.Data
{
    public partial class Category
    {
        #region static methods
        /// <summary>
        /// Get a Categorory object by its Id and a specified language
        /// </summary>
        /// <param name="id"></param>
        /// <param name="siteId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>

        public static Category GetById(int id, int languageId, bool includeArticles = false)
        {
            Category ret = null;
            using (DataContext db = new DataContext())
            {
                var category = (from a in db.Categories
                                join d in db.CategoryDetails on a.Id equals d.CategoryId
                                join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                                join td in db.ArticleTypeDetails on t.Id equals td.ArticleTypeId
                                where d.LanguageId == languageId && td.LanguageId == languageId && a.Id == id
                                select new
                                {
                                    a = a,
                                    d = d,
                                    t = t,
                                    td = td
                                })
                               .FirstOrDefault();
                if (category != null)
                {
                    ret = category.a;
                    if (ret != null)
                    {
                        ret.CategoryDetail = new VList<CategoryDetail> { category.d };
                        ret.LanguageId = languageId;
                        if (category.t != null)
                        {
                            category.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { category.td };
                            ret.ArticleType = category.t;
                            ret.ArticleType.LanguageId = languageId;
                        }
                    }

                    //if (includeArticles) ret.Articles = Article.GetByCategory(id, siteId, languageId).List;
                }
            }
            return ret;
        }
        public static Category GetByTypeId(int id, int languageId, bool includeArticles = false)
        {
            Category ret = null;
            using (DataContext db = new DataContext())
            {
                var category = (from a in db.Categories
                                join d in db.CategoryDetails on a.Id equals d.CategoryId
                                join c in db.CategoryTypes on a.CategoryTypeId equals c.Id
                                where d.LanguageId == languageId && a.Id == id
                                && c.ArticleTypes.Any()
                                select new
                                {
                                    a = a,
                                    d = d,
                                    c = c
                                })
                               .FirstOrDefault();
                if (category != null)
                {
                    ret = category.a;
                    if (ret != null)
                    {
                        ret.CategoryDetail = new VList<CategoryDetail> { category.d };
                        ret.LanguageId = languageId;
                        if (ret.CategoryType != null)
                        {
                            var cateType = db.CategoryTypes.Include("CategoryTypeDetail").FirstOrDefault(at => at.Id == ret.CategoryTypeId);
                            if (cateType != null)
                                cateType.CategoryTypeDetail = new VList<CategoryTypeDetail> { cateType.CategoryTypeDetail.FirstOrDefault(d => d.LanguageId == languageId) };
                        }
                    }

                    //if (includeArticles) ret.Articles = Article.GetByCategory(id, siteId, languageId).List;
                }
            }
            return ret;
        }
        /// <summary>
        /// This method is not completed yet. It's now working as  same as GetByType(int typeId, int siteId, int languageId)
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="siteId"></param>
        /// <param name="languageId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static ICollection<Category> GetByType(int typeId, int languageId, int level)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.Categories
                             join d in db.CategoryDetails on a.Id equals d.CategoryId
                             where d.LanguageId == languageId && a.ArticleTypeId == typeId
                             select new
                             {
                                 a = a,
                                 d = d,
                             });

                var ret = new List<Category>();
                foreach (var item in query.ToList())
                {
                    item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };

                    item.a.LanguageId = languageId;
                    ret.Add(item.a);
                }
                return ret;


            }
        }
        public static ICollection<Category> GetByUser(int userId, int typeId, int languageId, int count = 0, CategoryFlags flags = CategoryFlags.ALL, int level = 0)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();

                var query = (from a in db.Categories
                             join d in db.CategoryDetails on a.Id equals d.CategoryId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId
                             where d.LanguageId == languageId && a.ArticleTypeId == typeId && td.LanguageId == languageId
                             && (a.Flags & flags) > 0 && a.UserCreatedId == userId
                             select new
                             {
                                 a = a,
                                 d = d,
                                 t = t,
                                 td = td
                             });
                if (count > 0) query = query.Take(count);
                foreach (var item in query.ToList())
                {
                    item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };
                    item.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { item.td };
                    item.t.LanguageId = languageId;
                    item.a.LanguageId = languageId;
                    item.a.ArticleType = item.t;
                    ret.Add(item.a);
                }

                return ret;
            }
        }
        public static ICollection<Category> GetByType(int typeId, int languageId, int count = 0, CategoryFlags flags = CategoryFlags.ALL, int level = 0)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();

                var query = (from a in db.Categories
                             join d in db.CategoryDetails on a.Id equals d.CategoryId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId
                             where d.LanguageId == languageId && a.ArticleTypeId == typeId && td.LanguageId == languageId
                             && (a.Flags & flags) > 0
                             select new
                             {
                                 a = a,
                                 d = d,
                                 t = t,
                                 td = td
                             });
                if (count > 0) query = query.Take(count);
                foreach (var item in query.ToList())
                {
                    item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };
                    item.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { item.td };
                    item.t.LanguageId = languageId;
                    item.a.LanguageId = languageId;
                    item.a.ArticleType = item.t;
                    ret.Add(item.a);
                }
                
                return ret;
            }
        }
        public static ICollection<int> GetIdList(int typeId, int languageId, int count = 0, CategoryFlags flags = CategoryFlags.ALL, int level = 0)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();

                var query = (from a in db.Categories
                             join d in db.CategoryDetails on a.Id equals d.CategoryId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on a.ArticleTypeId equals td.ArticleTypeId
                             where d.LanguageId == languageId && a.ArticleTypeId == typeId && td.LanguageId == languageId
                             && (a.Flags & flags) > 0
                             select a.Id
                             );
                if (count > 0) query = query.Take(count);
                return query.ToList();
            }
        }
        public static ICollection<Category> GetByTypeIncludeArticle(int typeId, int languageId, ArticleFlags flags = ArticleFlags.ACTIVE)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();
                var articleType = db.ArticleTypes.Include("ArticleTypeDetail").FirstOrDefault(at => at.Id == typeId);
                if (articleType != null)
                {
                    articleType.ArticleTypeDetail = new VList<ArticleTypeDetail> { articleType.ArticleTypeDetail.FirstOrDefault(d => d.LanguageId == languageId) };
                    articleType.LanguageId = languageId;
                    var query = (from c in db.Categories
                                 join cd in db.CategoryDetails on c.Id equals cd.CategoryId
                                 where cd.LanguageId == languageId
                                 && c.ArticleTypeId == typeId

                                 select new
                                 {
                                     c = c,
                                     cd = cd,
                                 });
                    foreach (var item in query.ToList())
                    {
                        item.c.CategoryDetail = new VList<CategoryDetail>() { item.cd };
                        item.c.LanguageId = languageId;
                        item.c.ArticleType = articleType;
                        item.c.Articles = db.Articles.Include(a => a.ArticleDetail.Select(d => d.Body)).Where(a => a.Categories.Any(c => c.Id == item.c.Id) && (((int)a.Flags & (int)flags) > 0)).OrderByDescending(a => a.SortOrder).Skip(0).Take(5).ToList();
                        ret.Add(item.c);
                    }
                }
                return ret;
            }
        }

        public static ICollection<Category> GetByCateType(int typeId, int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();
                var cateType = db.CategoryTypes.Include("CategoryTypeDetail").FirstOrDefault(at => at.Id == typeId);
                if (cateType != null)
                {
                    cateType.CategoryTypeDetail = new VList<CategoryTypeDetail> { cateType.CategoryTypeDetail.FirstOrDefault(d => d.LanguageId == languageId) };
                    cateType.LanguageId = languageId;
                    var query = (from a in db.Categories
                                 join d in db.CategoryDetails on a.Id equals d.CategoryId
                                 where d.LanguageId == languageId && a.CategoryTypeId == typeId
                                 select new
                                 {
                                     a = a,
                                     d = d,
                                 });
                    foreach (var item in query.ToList())
                    {
                        item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };
                        item.a.LanguageId = languageId;
                        item.a.CategoryType = cateType;
                        ret.Add(item.a);
                    }
                }
                return ret;
            }
        }

        public static ICollection<Category> GetByCateTypeCode(string catetypeCode, int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();
                var cateType = db.CategoryTypes.Include("CategoryTypeDetail").FirstOrDefault(at => at.Code.Equals(catetypeCode, StringComparison.OrdinalIgnoreCase));
                if (cateType != null)
                {
                    cateType.CategoryTypeDetail = new VList<CategoryTypeDetail> { cateType.CategoryTypeDetail.FirstOrDefault(d => d.LanguageId == languageId) };
                    cateType.LanguageId = languageId;
                    var query = (from a in db.Categories
                                 join d in db.CategoryDetails on a.Id equals d.CategoryId
                                 where d.LanguageId == languageId && a.CategoryTypeId == cateType.Id
                                 select new
                                 {
                                     a = a,
                                     d = d,
                                 });
                    foreach (var item in query.ToList())
                    {
                        item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };
                        item.a.LanguageId = languageId;
                        item.a.CategoryType = cateType;
                        ret.Add(item.a);
                    }
                }
                return ret;
            }
        }

        public static ICollection<Category> GetByParent(int parentId, int typeId, int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();
                var articleType = db.ArticleTypes.Include("ArticleTypeDetail").FirstOrDefault(at => at.Id == typeId);
                if (articleType != null)
                {
                    articleType.ArticleTypeDetail = new VList<ArticleTypeDetail> { articleType.ArticleTypeDetail.FirstOrDefault(d => d.LanguageId == languageId) };
                    articleType.LanguageId = languageId;
                    var query = (from a in db.Categories
                                 join d in db.CategoryDetails on a.Id equals d.CategoryId
                                 where d.LanguageId == languageId && a.ArticleTypeId == typeId && a.ParentId == parentId
                                 select new
                                 {
                                     a = a,
                                     d = d,

                                 });
                    foreach (var item in query.ToList())
                    {
                        item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };
                        item.a.LanguageId = languageId;
                        item.a.ArticleType = articleType;
                        ret.Add(item.a);
                    }
                }
                return ret;
            }
        }

        public static ICollection<Category> GetByTypeCode(string typeCode, int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var ret = new List<Category>();
                var articleType = db.ArticleTypes.Include("ArticleTypeDetail").FirstOrDefault(at => at.Code.Equals(typeCode, StringComparison.OrdinalIgnoreCase));
                if (articleType != null)
                {
                    articleType.ArticleTypeDetail = new VList<ArticleTypeDetail> { articleType.ArticleTypeDetail.FirstOrDefault(d => d.LanguageId == languageId) };
                    articleType.LanguageId = languageId;
                    var query = (from a in db.Categories
                                 join d in db.CategoryDetails on a.Id equals d.CategoryId
                                 where d.LanguageId == languageId && a.ArticleType.Code.Equals(typeCode, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     a = a,
                                     d = d,

                                 });


                    foreach (var item in query.ToList())
                    {
                        item.a.CategoryDetail = new VList<CategoryDetail>() { item.d };

                        item.a.LanguageId = languageId;
                        item.a.ArticleType = articleType;
                        ret.Add(item.a);
                    }
                }
                return ret;


            }
        }

        public static ICollection<Category> GetTree(ICollection<Category> lst, int exclude = 0)
        {
            List<Category> tree = new List<Category>();
            var level0 = lst.Where(c => c.ParentId == null).OrderBy(c => c.CategoryName).ToList();
            foreach (var item in level0)
            {
                item.Level = 0;

                AddTreeItem(tree, item, lst, exclude);
            }
            return tree;
        }

        private static void AddTreeItem(List<Category> tree, Category item, ICollection<Category> lst, int exclude = 0)
        {
            if (item.Id != exclude)
            {
                tree.Add(item);
                var children = lst.Where(c => c.ParentId == item.Id).OrderBy(c => c.SortOrder).ToList();
                item.ChildrenCount = children.Count;
                item.Children = children;
                foreach (var child in children)
                {
                    child.Level = item.Level + 1;

                    AddTreeItem(tree, child, lst, exclude);
                }
            }
        }
        public static string GetCategoryName(int id)
        {
            using (DataContext db = new DataContext())
            {
                var category = db.Categories.Include(a => a.CategoryDetail).FirstOrDefault(a => a.Id == id);
                if (category != null)
                {
                    if (category.LanguageId == 0)
                        category.LanguageId = 1;
                    return category.CategoryDetail[category.LanguageId].CategoryName;
                }
            }
            return "N/A";
        }
        public static List<Category> BuildTreeReverse(int catId, int langId = 1)
        {
            using (DataContext db = new DataContext())
            {
                var ids = db.Database.SqlQuery<int>(string.Format("WITH MyCTE AS ( SELECT Id, ParentId FROM Categories WHERE Id = {0} UNION ALL SELECT Categories.Id, Categories.ParentId FROM Categories INNER JOIN MyCTE ON Categories.Id = MyCTE.ParentId  ) SELECT Id FROM MyCTE", catId)).ToList();
                var categories = (from c in db.Categories
                                  join cd in db.CategoryDetails on c.Id equals cd.CategoryId
                                  where cd.LanguageId == langId && ids.Contains(c.Id)
                                  select new
                                  {
                                      c = c,
                                      cd = cd
                                  }).ToList();
                categories.ForEach(c =>
                {
                    c.c.LanguageId = langId;
                    c.c.CategoryDetail = new VList<CategoryDetail> { c.cd };

                });
                //b.Categories.Include(c => c.CategoryDetail).Where(c => ids.Contains(c.Id)).ToList();
                //for (var i = 0; i < categories.Count - 1; i++)
                //    categories[i].Parent = categories[i + 1];
                var query = from i in ids
                            let c = categories.FirstOrDefault(c => c.c.Id == i)
                            select c.c;
                return query.Reverse().ToList();

            }
        }
        public static int[] BuildTreeReverseID(int catId)
        {
            using (DataContext db = new DataContext())
            {
                var ids = db.Database.SqlQuery<int>(string.Format("WITH MyCTE AS ( SELECT Id, ParentId FROM Categories WHERE Id = {0} UNION ALL SELECT Categories.Id, Categories.ParentId FROM Categories INNER JOIN MyCTE ON Categories.Id = MyCTE.ParentId  ) SELECT Id FROM MyCTE", catId)).ToArray();
                return ids;

            }
        }
        public static ICollection<Category> BuildTree(int catId)
        {
            using (DataContext db = new DataContext())
            {
                var ids = db.Database.SqlQuery<int>(string.Format("WITH MyCTE AS ( SELECT Id, ParentId FROM Categories WHERE Id = {0} UNION ALL SELECT Categories.Id, Categories.ParentId FROM Categories INNER JOIN MyCTE ON Categories.ParentId = MyCTE.Id  ) SELECT Id FROM MyCTE", catId)).ToList();
                var categories = db.Categories.Include(c => c.CategoryDetail).Where(c => ids.Contains(c.Id)).ToList();
                var query = from i in ids
                            let c = categories.FirstOrDefault(c => c.Id == i)
                            select c;
                return query.ToList();

            }
        }
        public static int[] BuildTreeID(int catId)
        {
            using (DataContext db = new DataContext())
            {
                var ids = db.Database.SqlQuery<int>(string.Format("WITH MyCTE AS ( SELECT Id, ParentId FROM Categories WHERE Id = {0} UNION ALL SELECT Categories.Id, Categories.ParentId FROM Categories INNER JOIN MyCTE ON Categories.ParentId = MyCTE.Id  ) SELECT Id FROM MyCTE", catId)).ToArray();
                return ids;

            }
        }
        #endregion
    }
}
