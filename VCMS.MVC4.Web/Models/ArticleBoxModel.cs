using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VCMS.MVC4.Data;
namespace VCMS.MVC4.Web.Models
{
    public class ArticleBoxModel
    {
        public string Caption { get; set; }
        public List<Article> Articles { get; set; }

        public ArticleType  ArticleType { get; set; }
    }
}