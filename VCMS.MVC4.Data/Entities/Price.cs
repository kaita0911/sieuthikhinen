using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    [Table("Price")]
    public class Price
    {
        public Price()
        {
            this.PriceShortOrder = 0;
        }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        public bool IsDiscount { get; set; }
        public bool IsDefault { get; set; }
        public bool Inactive { get; set; }
        public DateTime DateCreated { get; set; }
        public int? SortOrder { get; set; }
        [Required]
        public decimal Value { get; set; }
        public decimal? PriceShortOrder { get; set; }
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
        public int? PropertyMultiValueId { get; set; }
        [ForeignKey("PropertyMultiValueId")]
        public PropertyMultiValue PropertyMultiValue { get; set; }
       
        [NotMapped]
        public string Formating
        {
            get
            {
                if (this.Currency != null)
                    return Currency.Formatting;
                return "0:#,##0";
            }
        }
    }

    [Table("Currency")]
    public partial class Currency
    {
        public Currency()
        {
            this.CurrencyPositivePattern = -1;
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(10), Required]
        public string Code { get; set; }
        [MaxLength(200), Required]
        public string Name { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public bool IsDefault { get; set; }
        public string Formatting { get; set; }
        public bool CheckFormat { get; set; }
        public int CurrencyPositivePattern { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
