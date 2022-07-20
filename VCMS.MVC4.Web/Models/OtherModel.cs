using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VCMS.MVC4.Data;

namespace VCMS.MVC4.Web
{
    public class OtherModel
    {
        public int Id { get; set; }
        public int ArticleTypeId { get; set; }

        public int CategoryId { get; set; }

        public int Size { get; set; }

        public ArticleResult Result { get; set; }
    }
}