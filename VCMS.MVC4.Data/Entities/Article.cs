using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using System.IO;

namespace VCMS.MVC4.Data
{
    [Table("Articles")]
    public partial class Article
    {
        public Article()
        {
            this.Flags = ArticleFlags.ACTIVE;
            this.Categories = new List<Category>();
            this.Comments = new List<Comment>();
            this.PropertyValues = new List<ArticlePropertyValue>();
            this.Prices = new List<Price>();
            this.Keywords = new List<Keyword>();
            this.ArticleFiles = new List<ArticleFile>();
            this.PropertyMultiValues = new List<PropertyMultiValue>();
            this.LanguageId = 1;
            this.Rating = 0;
            this.Number = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? Status { get; set; }
        public int? SortOrder { get; set; }
        public int? Rating { get; set; }
        public int? Number { get; set; }
        [MaxLength(100)]
        public string IconFont { get; set; }
        public ArticleFlags Flags { get; set; }
        public int UserCreated { get; set; }
        [ForeignKey("UserCreated")]
        public UserProfile Author { get; set; }
        public int? UserLastUpdated { get; set; }
        [MaxLength(200)]
        [Display(Name = "Image Url")]

        public string ImageUrl { get; set; }
        public string Time { get; set; }
        public int? timeId { get; set; }

        public int WebsiteId { get; set; }
        public int ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }
        public int? ParentId { get; set; }
        public virtual Article Parent { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        [NotMapped]
        public Discount CurrentDiscount { get; set; }
        [NotMapped]
        public bool IsHot
        {
            get { return Flags.HasFlag(ArticleFlags.HOT); }
            set { Flags = value ? Flags | ArticleFlags.HOT : Flags & ~ArticleFlags.HOT; }
        }

        [NotMapped]
        public bool IsMostView
        {
            get { return Flags.HasFlag(ArticleFlags.MOSTVIEW); }
            set { Flags = value ? Flags | ArticleFlags.MOSTVIEW : Flags & ~ArticleFlags.MOSTVIEW; }
        }

        [NotMapped]
        public bool IsNew
        {
            get { return Flags.HasFlag(ArticleFlags.NEW); }
            set { Flags = value ? Flags | ArticleFlags.NEW : Flags & ~ArticleFlags.NEW; }
        }

        [NotMapped]
        public bool FullWidth
        {
            get { return Flags.HasFlag(ArticleFlags.FULLWIDTH); }
            set { Flags = value ? Flags | ArticleFlags.FULLWIDTH : Flags & ~ArticleFlags.FULLWIDTH; }
        }

        [NotMapped]
        public bool IsGallery
        {
            get { return Flags.HasFlag(ArticleFlags.GALLERY); }
            set { Flags = value ? Flags | ArticleFlags.GALLERY : Flags & ~ArticleFlags.GALLERY; }
        }

        [NotMapped]
        public bool IsVideo
        {
            get { return Flags.HasFlag(ArticleFlags.VIDEO); }
            set { Flags = value ? Flags | ArticleFlags.VIDEO : Flags & ~ArticleFlags.VIDEO; }
        }
        [NotMapped]
        public bool IsMusic
        {
            get { return Flags.HasFlag(ArticleFlags.MUSIC); }
            set { Flags = value ? Flags | ArticleFlags.MUSIC : Flags & ~ArticleFlags.MUSIC; }
        }

        [NotMapped]
        public int LanguageId { get; set; }

        [NotMapped]
        public string ArticleName
        {
            get
            {
                if (this.LanguageId <= 0) this.LanguageId = 1;
                if (this.ArticleDetail != null && this.LanguageId >= 1)
                    return this.ArticleDetail[this.LanguageId] != null ? this.ArticleDetail[this.LanguageId].ArticleName : "";
                else return "";
            }
        }

        [NotMapped]
        public string ShortDescription
        {
            get
            {
                if (this.ArticleDetail != null && this.LanguageId >= 1)
                {
                    if (!string.IsNullOrWhiteSpace(this.ArticleDetail[this.LanguageId].ShortDesc))
                        return this.ArticleDetail[this.LanguageId].ShortDesc;
                    return "";
                }
                else return "";
            }
        }

        [NotMapped]
        public string Description
        {
            get
            {
                if (this.ArticleDetail != null && this.LanguageId >= 1)
                {
                    if (!string.IsNullOrWhiteSpace(this.ArticleDetail[this.LanguageId].Description))
                        return this.ArticleDetail[this.LanguageId].Description;
                    return "";
                }
                else return "";
            }
        }

        [NotMapped]
        public string Day
        {  
            get
            {    
                
                    var date = this.DateUpdated != null ? this.DateUpdated.Value : this.DateCreated;
                    if (this.LanguageId == 0) this.LanguageId = 1;
                        return date.ToString("dd", System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));

               
            }
        }
        [NotMapped]
        public string Daymore
        {
            get
            {
                
                    var date = this.DateUpdated != null ? this.DateUpdated.Value : this.DateCreated;
                    if (this.LanguageId == 0) this.LanguageId = 1;
                        return date.ToString("MM/yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));
                
            }
        }

        [NotMapped]
        public string UrlFriendly
        {
            get
            {
                if (this.ArticleDetail.Count > 0 && this.LanguageId >= 1)
                {
                    if (!string.IsNullOrEmpty(this.ArticleDetail[this.LanguageId].UrlFriendly))
                        return this.ArticleDetail[this.LanguageId].UrlFriendly.ToString().ToLower().Trim();
                    else return string.Empty;
                }
                else return string.Empty;
            }
        }

        [NotMapped]
        public string SEOKeywords
        {
            get
            {
                if (this.ArticleDetail != null && this.LanguageId >= 1)
                    return this.ArticleDetail[this.LanguageId].SEOKeywords;
                else return "";
            }
        }

        [NotMapped]
        public string SEODescription
        {
            get
            {
                if (this.ArticleDetail != null && this.LanguageId >= 1)
                    return this.ArticleDetail[this.LanguageId].SEODescription;
                else return "";
            }
        }

        [NotMapped]
        public bool InActive
        {
            get
            {
                return Flags.HasFlag(ArticleFlags.INACTIVE);
            }
            set
            {
                Flags = value ? ((Flags & ~ArticleFlags.ACTIVE) | ArticleFlags.INACTIVE) : ((Flags & ~ArticleFlags.INACTIVE) | ArticleFlags.ACTIVE);
            }
        }

        [NotMapped]
        public decimal Price
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDefault).SingleOrDefault();
                    if (price != null)
                        return price.Value;
                }
                return 0.0M;
            }
        }

        [NotMapped]
        public decimal PrivateDiscountPrice
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDiscount).SingleOrDefault();
                    if (price != null)
                        return price.Value;
                }
                return 0.0M;
            }
        }
        [NotMapped]
        public bool InactivePrice
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDefault).SingleOrDefault();
                    if (price != null)
                        return price.Inactive;
                }
                return false;
            }
        }
        [NotMapped]
        public string Currency
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDefault).SingleOrDefault();
                    if (price != null && price.Currency != null)
                        return price.Currency.Code;
                }
                return "";
            }
        }

        [NotMapped]
        public string FormatPrice
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDefault).SingleOrDefault();
                    if (price != null && price.Currency != null)
                        return price.Currency.Formatting;
                }
                return "0:#,##0";
            }
        }

        [NotMapped]
        public decimal Rate
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDefault == true).SingleOrDefault();
                    if (price != null && price.Currency != null)
                        return price.Currency.Rate;
                }
                return 0.0M;
            }
        }

        [NotMapped]
        public int CurrencyId
        {
            get
            {
                if (this.Prices != null)
                {
                    var price = this.Prices.Where(p => p.IsDefault == true).SingleOrDefault();
                    if (price != null)
                        return price.CurrencyId;
                }
                return 0;
            }
        }

        [NotMapped]
        public decimal DiscountPrice
        {
            get
            {
                decimal discount = 0.0M;
                var cate = Categories.FirstOrDefault(c => c.DiscountPrice > 0 || c.DiscountPercent > 0);

                if (this.Prices.Count > 0)
                    if (this.Prices.FirstOrDefault(a => a.IsDefault && !a.Inactive) == null)
                        return discount;

                if (PrivateDiscountPrice > 0)
                    return PrivateDiscountPrice;

                else if (cate != null)
                {
                    if (cate.UsePercent)
                        return (this.Price - (this.Price * (int)cate.DiscountPercent / 100));
                    else
                        return (this.Price - Convert.ToDecimal(cate.DiscountPrice));
                }

                else if (this.CurrentDiscount != null)
                {
                    if (this.CurrentDiscount.IsActive)
                    {
                        if (this.CurrentDiscount.UsePercent)
                            discount = this.Price - (this.Price * this.CurrentDiscount.DiscountPercent.Value / 100);
                        else if (this.CurrentDiscount.IsAmount)
                        {
                            var dis = this.CurrentDiscount.DiscountAmount.Value;
                            if (this.CurrencyId != this.CurrentDiscount.CurrencyId)
                                dis = this.Rate * dis / this.CurrentDiscount.Currency.Rate;
                            discount = this.Price - dis;
                        }
                    }
                }
                return discount > 0 ? discount : 0.0M;
            }
        }
        [NotMapped]
        public decimal DiscountPercent
        {
            get
            {
                int percent = 0;
                if (this.CurrentDiscount != null)
                {
                    if (this.CurrentDiscount.IsActive)
                    {
                        if (this.CurrentDiscount.UsePercent)
                            percent = (int)this.CurrentDiscount.DiscountPercent.Value;
                        else if (this.Price > 0)
                            percent = (int)(100 - ((this.DiscountPrice * 100) / this.Price));
                    }
                }
                return percent;
            }
        }
        [NotMapped]
        public decimal Views
        {
            get
            {
                if (this.PropertyValues != null)
                {
                    var p = this.PropertyValues.Where(pv => pv.Property.Code.Equals("VIEWS", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (p != null)
                        return p.GetValue<decimal>();
                }
                return 0.0M;
            }
        }
        [NotMapped]
        public bool CheckCart
        {
            get
            {
                if (this.Prices != null)
                    if (this.Price <= 0 || this.InactivePrice)
                        return false;

                if (this.ArticleType != null)
                {
                    if (this.ArticleType.HasNumberProduct)
                    {
                        if (this.Number <= 0)
                            return false;
                    }
                    else
                    {
                        /// kiểm tra trường hợp có option
                        return true;
                    }
                }
                return true;
            }
        }
        public string FriendlyImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ImageUrl))
                    return string.Empty;
                else
                    return string.Format("{0}_{1}{2}", this.UrlFriendly, this.Id, Path.GetExtension(ImageUrl));
            }
        }

        public ICollection<Comment> Comments { get; set; }
        public VList<ArticleDetail> ArticleDetail { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<PropertyMultiValue> PropertyMultiValues { get; set; }
        [NotMapped]
        public Category CurrentCategory { get; set; }
        public ICollection<ArticlePropertyValue> PropertyValues { get; set; }
        public ICollection<Keyword> Keywords { get; set; }
        public ICollection<ArticleFile> ArticleFiles { get; set; }
        public ICollection<Price> Prices { get; set; }
        [NotMapped]
        public Price ItemPrice
        {
            get
            {
                if (this.Prices.Count > 0)
                    return this.Prices.FirstOrDefault(a => a.IsDefault && !a.Inactive);
                else
                    return null;
            }
        }

        [NotMapped]
        public ICollection<ArticleFile> Attachments
        {
            get
            {
                if (this.ArticleFiles != null)
                    return this.ArticleFiles.Where(af => af.FileType == ArticleFileType.ATTACHMENT).ToList();
                else return new List<ArticleFile>();
            }
        }

        [NotMapped]
        public ICollection<ArticleFile> ImageList
        {
            get
            {
                if (this.ArticleFiles != null)
                    return this.ArticleFiles.Where(af => af.FileType == ArticleFileType.IMAGE).ToList();
                else return new List<ArticleFile>();
            }
        }

        public string this[string propertyCode]
        {
            get
            {
                var prop = this.PropertyValues.FirstOrDefault(pv => pv.Property.Code.Equals(propertyCode, StringComparison.OrdinalIgnoreCase));
                if (prop == null)
                    return "";
                else
                    return prop.Value;
            }
        }

        public string this[string propertyCode, int langId]
        {
            get
            {
                var prop = this.PropertyValues.FirstOrDefault(pv => pv.Property.Code.Equals(propertyCode, StringComparison.OrdinalIgnoreCase) && pv.LanguageId == langId);
                if (prop == null)
                    return "";
                else
                    return prop.Value;
            }
        }

        public void AddKeywords(string[] kwa)
        {
            if (this.Keywords == null)
                this.Keywords = new List<Keyword>();

            var kwl = kwa.Select(s => new Keyword { Tag = s });
            this.Keywords.Concat(kwl.ToList());
        }
    }

    [Table("ArticleDetail")]
    public partial class ArticleDetail : LanguageBase
    {
        public override int LanguageId
        {
            get
            {
                return base.LanguageId;
            }
            set
            {
                base.LanguageId = value;
                //if (this.Body == null) this.Body = new ArticleDetailBody();
                if (this.Body != null)
                    this.Body.LanguageId = value;
            }
        }
        [Key, Column(Order = 0)]
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        [Required]
        [StringLength(500)]
        public string ArticleName { get; set; }
        [StringLength(500), Column(TypeName = "varchar")]
        public string UrlFriendly { get; set; }
        [Required]
        public virtual ArticleDetailBody Body { get; set; }

        [NotMapped, AllowHtml]
        public string ShortDesc
        {
            get
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                return Body.ShortDesc;
            }
            set
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                Body.ShortDesc = value;
            }
        }

        [NotMapped, AllowHtml]
        public string Description
        {
            get
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                return Body.Description;
            }
            set
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                Body.Description = value;
            }
        }

        [NotMapped]
        public string SEOKeywords
        {
            get
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                return Body.SEOKeywords;
            }
            set
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                Body.SEOKeywords = value;
            }
        }
        [NotMapped, StringLength(200)]
        public string SEODescription
        {
            get
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                return Body.SEODescription;
            }
            set
            {
                if (Body == null)
                    Body = new ArticleDetailBody { LanguageId = this.LanguageId };
                Body.SEODescription = value;
            }
        }
    }

    [Table("ArticleDetail")]
    public partial class ArticleDetailBody
    {
        [Key, ForeignKey("ArticleDetail"), Column(Order = 0)]
        public int ArticleId
        {
            get;
            set;
        }
        [Key, ForeignKey("ArticleDetail"), Column(Order = 1)]
        public int LanguageId
        {
            get;
            set;
        }
        [AllowHtml]
        public string ShortDesc { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        //[StringLength(500)]
        //[AllowHtml]
        public string SEOKeywords { get; set; }
        //[StringLength(500)]
        public string SEODescription { get; set; }

        public ArticleDetail ArticleDetail { get; set; }
    }

    public class ArticleResult : PagedList<Article>
    {
        //public List<Article> List { get; set; }

        //public int PageIndex { get; set; }

        //public int PageSize { get; set; }

        //public int TotalItemCount { get; set; }
    }

    [Flags]
    public enum ArticleFlags
    {
        ACTIVE = 1 << 1,
        DELETED = 1 << 2,
        NEW = 1 << 3,
        HOT = 1 << 4,
        MOSTVIEW = 1 << 5,
        INACTIVE = 1 << 6,
        FULLWIDTH = 1 << 7,
        GALLERY = 1 << 8,
        VIDEO = 1 << 9,
        MUSIC = 1 << 10,
        ALL = ~0
    }

    public enum ArticleSortOrder
    {
        SORT_ORDER,
        DATE_CREATED,
        ARTICLE_NAME,
        PRICE,
        RATING,
        MODEL
    }

    public enum SortDirection
    {
        ASCENDING,
        DESCENDING

    }

    public enum ArticleSearchOption
    {
        HAS_TITLE_ONLY,
        HAS_SHORTDESC_ONLY,
        HAS_DESC_ONLY,
        ALL
    }
}
