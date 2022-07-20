using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Entity;

namespace VCMS.MVC4.Data.Extensions
{
    public static class ArticleExtensions
    {

        public static ArticleResult GetByCategories(int[] categories, ArticleFlags flag, int itemPerGroup, ArticleSortOrder sortOrder, SortDirection direction)
        {
            DataContext db = new DataContext();

            var s = db.Categories.Where(c => categories.Contains(c.Id))
                .SelectMany(c => c.Articles.OrderByDescending(a=>a.DateCreated).Take(itemPerGroup)).ToList();
                    


            return new ArticleResult { List = s };

        }
    }
}
