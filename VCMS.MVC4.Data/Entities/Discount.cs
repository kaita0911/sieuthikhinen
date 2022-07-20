using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public class Discount
    {
        public Discount()
        {
            this.Articles = new List<Article>();
            this.Categories = new List<Category>();
            this.Currencies = new List<Currency>();
            this.Status = DiscountStatus.ACTIVE;
            this.DateStart = DateTime.Now;
            this.DateEnd = DateTime.Now;
        }
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200), Required]
        public string Name { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public DiscountStatus Status { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }

        public bool UsePercent { get; set; }
        public bool AllItems { get; set; }

        public ICollection<Article> Articles { get; set; }

        public ICollection<Category> Categories { get; set; }
        [NotMapped]
        public ICollection<Currency> Currencies { get; set; }
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [NotMapped]
        public bool IsPercent
        {
            get
            {
                return this.DiscountPercent.HasValue;
            }
        }
        [NotMapped]
        public bool IsAmount
        {
            get
            {
                return this.DiscountAmount.HasValue;
            }
        }
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return this.Status == DiscountStatus.ACTIVE && this.DateStart <= DateTime.Now && this.DateEnd >= DateTime.Now;
            }
        }
    }

    public enum DiscountStatus
    {
        ACTIVE,
        INACTIVE
    }
}
