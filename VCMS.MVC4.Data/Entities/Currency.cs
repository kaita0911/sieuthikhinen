using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    [Table("Currencies")]
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        [Required, Column(TypeName = "varchar"), MaxLength(5)]
        public string Code { get; set; }
        [Required, MaxLength(20)]
        public string DisplayName { get; set; }
        public string Signal { get; set; }
        public decimal Rates { get; set; }
        public bool Chose { get; set; }
        public Website Websites { get; set; }
        //public ICollection<ArticleDetail> Articles { get; set; }
    }
}
