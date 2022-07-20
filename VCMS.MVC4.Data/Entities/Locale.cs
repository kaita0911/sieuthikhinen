using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public class Locale
    {
        public Locale()
        {
            this.LocaleDetail = new VList<LocaleDetail>();
            this.LanguageId = 1;
        }
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100), Column(TypeName="varchar")]
        public string LocaleKey { get; set; }

        public string DefaultValue { get; set; }

        public VList<LocaleDetail> LocaleDetail { get; set; }

        [NotMapped]
        public int LanguageId { get; set; }
        [NotMapped]
        public string Value
        {
            get
            {
                if (this.LanguageId > 0 && this.LocaleDetail != null)
                {
                    if (this.LocaleDetail[this.LanguageId] != null)
                        return this.LocaleDetail[this.LanguageId].Value;
                    else
                        return this.DefaultValue;
                }
                else
                    return this.DefaultValue;
            }
        }

        public static Dictionary<string, string> GetLocaleTable(int langId)
        {
            var _localeTable = new Dictionary<string, string>();
            using (DataContext db = new DataContext())
            {
                var locales = from l in db.Locales
                              join ld in db.LocaleDetails on l.Id equals ld.LocaleId 
                              where ld.LanguageId == langId
                              
                              select new { l.LocaleKey, l.DefaultValue, ld.Value };
                locales.ToList().ForEach(l =>
                {
                    if (!_localeTable.ContainsKey(langId.ToString() + l.LocaleKey.ToLower()))
                        _localeTable.Add(langId.ToString() + l.LocaleKey.ToLower(), l.Value);
                });
                return _localeTable;
            }
        }
    }

    public class LocaleDetail :LanguageBase
    {
        [Key, Column(Order=0)]
        public int LocaleId { get; set; }
        [ForeignKey("LocaleId")]
        public Locale Locale { get; set; }
        public string Value { get; set; }
    }
}
