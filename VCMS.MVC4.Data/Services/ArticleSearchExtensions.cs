using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public static class ArticleSearchExtensions
    {
        public static IQueryable<Article> ApplyCondition(this IQueryable<Article> articles, ArticleSearchFilter cond)
        {
            var query = articles;
            if (cond.TypeId > 0)
                query = query.Where(a => a.ArticleTypeId == cond.TypeId);
            if (cond.SiteId > 0)
                query = query.Where(a => a.WebsiteId == cond.SiteId);
            if (!string.IsNullOrWhiteSpace(cond.SearchTerm))
            {
                switch (cond.KeywordMatchType)
                {
                    case StringMatchType.EQUALS:
                        query = query.Where(a => a.ArticleDetail.Any(d => d.ArticleName.Equals(cond.SearchTerm, StringComparison.InvariantCultureIgnoreCase)));
                        break;
                    case StringMatchType.CONTAINS:
                        query = query.Where(a => a.ArticleDetail.Any(d => d.ArticleName.Contains(cond.SearchTerm)));
                        break;
                    case StringMatchType.STARTSWITH:
                        query = query.Where(a => a.ArticleDetail.Any(d => d.ArticleName.StartsWith(cond.SearchTerm, StringComparison.InvariantCultureIgnoreCase)));
                        break;
                    case StringMatchType.ENDSWITH:
                        query = query.Where(a => a.ArticleDetail.Any(d => d.ArticleName.EndsWith(cond.SearchTerm, StringComparison.InvariantCultureIgnoreCase)));
                        break;
                    default:
                        query = query.Where(a => a.ArticleDetail.Any(d => d.ArticleName.Contains(cond.SearchTerm)));
                        break;
                }

            }
            return query;
        }
    }
}
