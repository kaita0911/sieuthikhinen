using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace VCMS.MVC4.Data
{
    [Table("Categories")]
    public partial class Category
    {
        public Category()
        {
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Articles = new List<Article>();
            this.Children = new List<Category>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal? ShippedPrice { get; set; }
        public int? DiscountPercent { get; set; }
        public bool UsePercent { get; set; }

        public int? UserCreatedId { get; set; }

        public int? UserUpdatedId { get; set; }

        public int Status { get; set; }

        public int SortOrder { get; set; }

        [MaxLength(200), Column(TypeName = "varchar")]
        public string ImageUrl { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Category Parent { get; set; }
        public int WebsiteId { get; set; }
        public int? CategoryTypeId { get; set; }
        [ForeignKey("CategoryTypeId")]
        public CategoryType CategoryType { get; set; }
        public int? ArticleTypeId { get; set; }
        [ForeignKey("ArticleTypeId")]
        public ArticleType ArticleType { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Category> Children { get; set; }
        public VList<CategoryDetail> CategoryDetail { get; set; }
        public CategoryFlags Flags { get; set; }
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return Flags.HasFlag(CategoryFlags.ACTIVE);
            }
            set
            {
                Flags = value ? Flags | CategoryFlags.ACTIVE : Flags & ~CategoryFlags.ACTIVE;
            }
        }
        [NotMapped]
        public bool IsHot
        {
            get
            {
                return Flags.HasFlag(CategoryFlags.HOT);
            }
            set
            {
                Flags = value ? Flags | CategoryFlags.HOT : Flags & ~CategoryFlags.HOT;
            }
        }

        [NotMapped]
        public bool IsMostView
        {
            get
            {
                return Flags.HasFlag(CategoryFlags.MOSTVIEW);
            }
            set
            {
                Flags = value ? Flags | CategoryFlags.MOSTVIEW : Flags & ~CategoryFlags.MOSTVIEW;
            }
        }

        [NotMapped]
        public bool IsNew
        {
            get
            {
                return Flags.HasFlag(CategoryFlags.NEW);
            }
            set
            {
                Flags = value ? Flags | CategoryFlags.NEW : Flags & ~CategoryFlags.NEW;
            }
        }

        [NotMapped]
        public int ArticleCount { get; set; }
        [NotMapped]
        public int LanguageId { get; set; }
        [NotMapped]
        public string CategoryName
        {
            get
            {
                if (this.LanguageId == 0) this.LanguageId = 1;
                if (this.CategoryDetail != null && this.LanguageId > 0)
                    if (this.CategoryDetail[this.LanguageId] != null)
                        return this.CategoryDetail[this.LanguageId].CategoryName;
                    else return "";
                else
                    return "";
            }
        }

        [NotMapped]
        public string Description
        {
            get
            {
                if (this.LanguageId == 0) this.LanguageId = 1;
                if (this.CategoryDetail != null && this.LanguageId > 0)
                {
                    if (!string.IsNullOrWhiteSpace(this.CategoryDetail[this.LanguageId].Description))
                        return this.CategoryDetail[this.LanguageId].Description.Replace("src=\"/UserUpload/Editor/", "data-src=\"/UserUpload/Editor/");
                    return "";
                }
                else
                    return "";
            }
        }
        [NotMapped]
        public string UrlFriendly
        {
             get
            {
                if (this.CategoryDetail != null && this.LanguageId >= 1)
                {
                    if (!string.IsNullOrEmpty(this.CategoryDetail[this.LanguageId].UrlFriendly))
                        return this.CategoryDetail[this.LanguageId].UrlFriendly.ToString().ToLower();
                    else return string.Empty;
                }
                else return string.Empty;
            }
            //get
            //{
            //    if (this.LanguageId == 0) this.LanguageId = 1;
            //    if (this.CategoryDetail != null && this.LanguageId > 0)
            //        return this.CategoryDetail[this.LanguageId].UrlFriendly.ToString().ToLower();
            //    else
            //        return "";
            //}
        }

        [NotMapped]
        public int Level { get; set; }
        [NotMapped]
        public int ChildrenCount { get; set; }

        [MaxLength(200)]
        public string Font { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        [NotMapped]
        public Discount CurrentDiscount { get; set; }


    }

    public partial class CategoryDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


        [MaxLength(200), Required]
        public string CategoryName { get; set; }

        [MaxLength(200), Column(TypeName = "varchar")]
        public string UrlFriendly { get; set; }

        public string Description { get; set; }

        //[MaxLength(100)]
        public string SEOKeywords { get; set; }
        [MaxLength(200)]
        public string SEODescription { get; set; }
    }
    [Table("CategoryTypes")]
    public partial class CategoryType
    {
        public CategoryType()
        {
            this.Categories = new List<Category>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [NotMapped]
        public int LanguageId { get; set; }
        [NotMapped]
        public string Name
        {
            get
            {
                if (this.LanguageId == 0) this.LanguageId = 1;
                if (this.LanguageId > 0 && this.CategoryTypeDetail != null && this.CategoryTypeDetail[this.LanguageId] != null)
                    return this.CategoryTypeDetail[this.LanguageId].Name;
                else
                    return string.Empty;
            }
        }
        public bool NoneType { get; set; }
        public ICollection<ArticleType> ArticleTypes { get; set; }
        public VList<CategoryTypeDetail> CategoryTypeDetail { get; set; }
        public ICollection<Category> Categories { get; set; }
    }

    [Table("CategoryTypeDetail")]
    public partial class CategoryTypeDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int CategoryTypeId { get; set; }
        [ForeignKey("CategoryTypeId")]
        public CategoryType CategoryType { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Flags]
    public enum CategoryFlags
    {
        NONE = 0,
        ACTIVE = 1,
        MOSTVIEW = 2,
        HOT = 4,
        NEW = 8,
        ALL = HOT | MOSTVIEW | NEW | ACTIVE
    }
}
