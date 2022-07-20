using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public partial class ArticleType
    {
        public static ICollection<ArticleType> GetBySite(int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.ArticleTypes
                             join d in db.ArticleTypeDetails on a.Id equals d.ArticleTypeId

                             where d.LanguageId == languageId
                             select new
                             {
                                 a = a,
                                 d = d,
                             });
                var article = query.ToList();
                var ret = new List<ArticleType>();
                foreach (var item in article)
                {
                    item.a.ArticleTypeDetail = new VList<ArticleTypeDetail>() { item.d };
                    item.a.LanguageId = languageId;
                    ret.Add(item.a);
                }
                return ret;
            }
        }

        public static ICollection<ArticleType>  GetBySiteMap(int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.ArticleTypes
                             join d in db.ArticleTypeDetails on a.Id equals d.ArticleTypeId

                             where d.LanguageId == languageId
                             && (((int)a.Flag & (int)ArticleTypeFlags.SITEMAP) > 0)
                             select new
                             {
                                 a = a,
                                 d = d,
                             });
                var article = query.ToList();
                var ret = new List<ArticleType>();
                foreach (var item in article)
                {
                    item.a.ArticleTypeDetail = new VList<ArticleTypeDetail>() { item.d };
                    item.a.LanguageId = languageId;
                    ret.Add(item.a);
                }
                return ret;
            }
        }

        public static ArticleType GetById(int id, int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.ArticleTypes
                             join d in db.ArticleTypeDetails on a.Id equals d.ArticleTypeId
                             where d.LanguageId == languageId && a.Id == id
                             select new
                             {
                                 a = a,
                                 d = d,
                             });
                var article = query.FirstOrDefault();

                if (article != null)
                {
                    article.a.ArticleTypeDetail = new VList<ArticleTypeDetail>() { article.d };
                    article.a.LanguageId = languageId;
                    return article.a;
                }
                else return null;
            }
        }
        public static ArticleType GetByCode(string code, int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.ArticleTypes
                             join d in db.ArticleTypeDetails on a.Id equals d.ArticleTypeId
                             where d.LanguageId == languageId && d.UrlFriendly.ToLower() == code.ToLower()
                             select new
                             {
                                 a = a,
                                 d = d,
                             });
                var article = query.FirstOrDefault();

                if (article != null)
                {
                    article.a.ArticleTypeDetail = new VList<ArticleTypeDetail>() { article.d };
                    article.a.LanguageId = article.d.LanguageId;
                    return article.a;
                }
                else return null;
            }
        }

        public static int GetIdByCode(string code)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.ArticleTypes
                             where a.Code.ToLower() == code.ToLower()
                             select a);
                var article = query.FirstOrDefault();

                if (article != null)
                    return article.Id;
                else return 0;
            }
        }
    }
}
