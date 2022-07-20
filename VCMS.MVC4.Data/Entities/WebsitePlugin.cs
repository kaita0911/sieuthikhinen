using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VCMS.MVC4.Data
{
    [Table("WebsitePlugins")]
    public class WebsitePlugin
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public WebsitePluginFlags Flag { get; set; }

        [AllowHtml]
        public string Value { get; set; }


        [NotMapped]
        public Boolean IsPlugin
        {
            get { return Flag.HasFlag(WebsitePluginFlags.WEBCONFIG); }
            set { Flag = value ? Flag | WebsitePluginFlags.WEBCONFIG : (Flag & ~WebsitePluginFlags.WEBCONFIG); }
        }
        [NotMapped]
        public Boolean IsCustommer
        {
            get { return Flag.HasFlag(WebsitePluginFlags.CUSTOMMER); }
            set { Flag = value ? Flag | WebsitePluginFlags.CUSTOMMER : (Flag & ~WebsitePluginFlags.CUSTOMMER); }
        }
    }

    [Flags]
    public enum WebsitePluginFlags
    {
        NONE = 0,
        WEBCONFIG=1,
        CUSTOMMER = 2,
    }
}
