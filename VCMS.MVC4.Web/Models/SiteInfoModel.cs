using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VCMS.MVC4.Data;

namespace VCMS.MVC4.Web
{
    public class SiteInfoModel
    {
        public SiteInfoModel()
        { }
        public SiteInfoModel(Website website) {
            SiteId = website.Id;
            Code = website.Code;
        }

        public int SiteId { get; set; }

        public int LanguageId { get; set; }

        public string Code { get; set; }


    }
}