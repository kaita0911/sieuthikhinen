using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public class PagedList<T> where T:new()
    {

        public PagedList()
        {
            List = new List<T>();
        }
        public List<T> List { get; set; }
        public int TotalItemCount { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 0;
                else
                {
                    var remain = TotalItemCount % PageSize;
                    return (TotalItemCount / PageSize) + (remain == 0 ? 0 : 1);
                }
            }
            
        }

        public PagedList<TResult> Convert<TResult>() where TResult :new()
        {
            return new PagedList<TResult> { List = new List<TResult>(), PageIndex = this.PageIndex, PageSize = this.PageSize, TotalItemCount = this.TotalItemCount };
        }
    }
}
