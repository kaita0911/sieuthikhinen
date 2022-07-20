using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    [Table("Languages")]
    public class Language
    {
        [Key]
        public int Id { get; set; }
        [Required, Column(TypeName = "varchar"), MaxLength(5)]
        public string Code { get; set; }
        [Required, Column(TypeName = "varchar"), MaxLength(10)]
        public string Locale { get; set; }
        [Required, MaxLength(20)]
        public string DisplayName { get; set; }
        public ICollection<Website> Websites { get; set; }
        public ICollection<ArticleDetail> Articles { get; set; }
    }
}
