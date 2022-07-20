using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    [Table("WebsiteConfigs")]
    public class WebsiteConfig
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }

        public bool MultiLanguage { get; set; }
        public string DefaultValue { get; set; }
    }

    [Table("WebsiteConfigDetail")]
    public class WebsiteConfigDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int WebsiteConfigId { get; set; }
        [ForeignKey("WebsiteConfigId")]
        public Property WebsiteConfig { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }


    }

   
    public class WebsiteConfigValue 
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int WebsiteId { get; set; }
        public Website Website { get; set; }

        public int WebsiteConfigId { get; set; }
        public WebsiteConfig WebsiteConfig { get; set; }


        public int? LanguageId { get; set; }
        public string Value { get; set; }

        [NotMapped]
        public string Code { get; set; }

        [NotMapped]
        public string Name { get; set; }

        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(this.Value, typeof(T));
        }

        
    }

    public class WebsiteConfigValueCollection :List<WebsiteConfigValue>{
        

        public WebsiteConfigValue this[string index] {
            get {
                return this.FirstOrDefault(p => p.WebsiteConfig.Code.Equals(index, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
