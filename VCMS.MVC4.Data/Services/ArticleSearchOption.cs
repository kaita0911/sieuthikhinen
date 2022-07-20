using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public class ArticleSearchFilter
    {
        public ArticleSearchFilter()
        {
            KeywordMatchType = Data.StringMatchType.CONTAINS;
        }
        public int ArticleId { get; set; }
        public int[] ExcludeIds { get; set; }
        public int TypeId { get; set; }
        public string TypeCode { get; set; }
        public int[] Ids { get; set; }
        public int SiteId { get; set; }

        public string SearchTerm { get; set; }
        public StringMatchType KeywordMatchType { get; set; }
        public int LangId { get; set; }

        public ArticleFlags Flags{ get; set; }

        public PropertySearchCondition[] PropertyConditions { get; set; }

        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string  Tag { get; set; }

        public bool Admin { get; set; }
    }

    public enum StringMatchType
    {
        EQUALS,
        CONTAINS,
        STARTSWITH,
        ENDSWITH
    }
    public enum PropertyCompareType
    {
        EQUALS

    }
    public class PropertySearchCondition
    {
        public int PropertyId { get; set; }

        public string Value { get; set; }

        
    }

  
    [Flags]
    public enum ArticleIncludeFlags
    {
        NONE,
        ARTICLE_DETAIL = 1 << 0, // if not set, include only ArticleName and FriendlyUrl
        ARTICLE_TYPE = 1 << 1,
        CATEGORIES = 1 << 2,
        PROPERTIES = 1 << 3,
        PRICES = 1 << 4,
        DISCOUNTS = 1 << 5,
        COMMENTS = 1 << 6,
        KEYWORDS = 1 << 7,
        FILES = 1 << 8,
        ALL = ~0
    }
    [Flags]

    public enum ArticleResultFlags
    {
        NONE = 0,
        COUNT_ONLY = 1,
        ITEMS_ONLY = 2,
        ALL = ~0

    }
    public class ArticleSearchResultOption
    {
        public ArticleSearchResultOption()
        {
            //this.IncludeFlags = ArticleIncludeFlags.ARTICLE_DETAIL;
        }
        public ArticleSortOrder SortOrder { get; set; }

        public SortDirection SortDirection { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        /// <summary>
        /// Determine how the result returned, only count number or items included.
        /// </summary>
        public ArticleResultFlags ResultFlag { get; set; }
        /// <summary>
        /// if Set, loop thru all child categories to search articles
        /// </summary>
        public bool IncludeChildCategories { get; set; }
        /// <summary>
        /// Determine which child entities will be included in result list, only affect if ResultFlag is ITEMS_ONLY or ALL
        /// </summary>
        public ArticleIncludeFlags IncludeFlags { get; set; }
    }
}
