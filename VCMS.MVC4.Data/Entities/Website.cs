using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VCMS.MVC4.Data
{
    [Table("Websites")]
    public class Website
    {
        public Website()
        {
            this.Settings = new WebsiteConfigValueCollection();
            this.Skin = "skin";
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Skin { get; set; }
        public WebsiteFlags Flag { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        [MaxLength(100), Column(TypeName = "varchar")]
        public string DefaultDomain { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Email { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Hotline { get; set; }
        [MaxLength(50)]
        public string Watermark { get; set; }
        public WatermarkPostion WatermarkPosition { get; set; }
        public WebsiteStatus Status { get; set; }
        public string FaceBookApp { get; set; }
        public string FaceBookAdmin { get; set; }
        [MaxLength(200)]
        public string ApiKey { get; set; }
        /// <summary>
        /// Default address of the website, the head-quater address
        /// </summary>
        public int? WebsiteAddressId { get; set; }
        [ForeignKey("WebsiteAddressId")]
        public WebsiteAddress WebsiteAddress { get; set; }
        public VList<WebsiteDetail> WebsiteDetail { get; set; }

        public ICollection<Language> Languages { get; set; }
        public int DefaultLanguage { get; set; }
        public decimal AmountShippingToFree { get; set; }
        public WebsiteConfigValueCollection Settings { get; set; }
        [NotMapped]
        public Boolean HasShoppingCart
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_SHOPPINGCART); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_SHOPPINGCART : (Flag & ~WebsiteFlags.HAS_SHOPPINGCART); }
        }
        [NotMapped]
        public Boolean HasWatermark
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_WARTERMARK); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_WARTERMARK : (Flag & ~WebsiteFlags.HAS_WARTERMARK); }
        }
        [NotMapped]
        public Boolean HasWatermarkImg
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_WARTERMARK_IMG); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_WARTERMARK_IMG : (Flag & ~WebsiteFlags.HAS_WARTERMARK_IMG); }
        }
        [NotMapped]
        public Boolean HasUser
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_USER); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_USER : (Flag & ~WebsiteFlags.HAS_USER); }
        }
        [NotMapped]
        public Boolean HasNewsletter
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_NEWSLETTER); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_NEWSLETTER : (Flag & ~WebsiteFlags.HAS_NEWSLETTER); }
        }
        [NotMapped]
        public Boolean HasResponsive
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_RESPONSIVE); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_RESPONSIVE : (Flag & ~WebsiteFlags.HAS_RESPONSIVE); }
        }
        [NotMapped]
        public Boolean HasDiscount
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_DISCOUNT); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_DISCOUNT : (Flag & ~WebsiteFlags.HAS_DISCOUNT); }
        }
        [NotMapped]
        public Boolean HasOrder
        {
            get { return Flag.HasFlag(WebsiteFlags.HAS_ORDER); }
            set { Flag = value ? Flag | WebsiteFlags.HAS_ORDER : (Flag & ~WebsiteFlags.HAS_ORDER); }
        }
        [NotMapped]
        public int LanguageId { get; set; }
        [NotMapped]
        public string Title
        {
            get
            {
                if (this.LanguageId <= 0)
                    this.LanguageId = 1;
                if (this.WebsiteDetail != null)
                    return this.WebsiteDetail[this.LanguageId].Title;
                else
                    return "";
            }
        }
        [NotMapped]
        public string Name
        {
            get
            {
                if (this.LanguageId <= 0)
                    this.LanguageId = 1;
                if (this.WebsiteDetail != null)
                    return this.WebsiteDetail[this.LanguageId].Name;
                else
                    return "";
            }
        }
        public string this[string settingCode]
        {
            get { return this.Settings[settingCode].Value; }
        }
    }

    [Table("WebsiteDetail")]
    public class WebsiteDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int WebsiteId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "website title must be less than 200 characters")]
        public string Title { get; set; }
        [StringLength(200, ErrorMessage = "meta description must be less than 200 characters")]
        public string SEODescription { get; set; }
        [StringLength(200, ErrorMessage = "meta keywords must be less than 200 characters")]
        public string SEOKeywords { get; set; }
    }

    [Table("WebsiteAddress")]
    public class WebsiteAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Phone { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Fax { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Email { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Facebook { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Twitter { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string GooglePlus { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string Blog { get; set; }
        public ICollection<WebsiteAddressDetail> WebsiteAddressDetail { get; set; }
    }

    [Table("WebsiteAddressDetail")]
    public class WebsiteAddressDetail
    {
        [Key, Column(Order = 0)]
        public int WebsiteId { get; set; }
        [Key, Column(Order = 1)]
        public int LanguageId { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(200)]
        public string ContactPerson { get; set; }
        [MaxLength(100)]
        public string SEOKeywords { get; set; }
        [MaxLength(200)]
        public string SEODescription { get; set; }
    }

    [Table("WebsiteSupports")]
    public class WebsiteSupport
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string NickName { get; set; }
        [MaxLength(100)]
        public string DisplayName { get; set; }
        public SupportType SupportType { get; set; }
        public int CategoryId { get; set; }
    }

    [Flags]
    public enum WebsiteFlags
    {
        NONE = 0,
        HAS_SHOPPINGCART = 1 << 0,
        HAS_WARTERMARK = 1 << 1,
        HAS_USER = 1 << 2,
        HAS_NEWSLETTER = 1 << 3,
        HAS_RESPONSIVE = 1 << 4,
        HAS_WARTERMARK_IMG = 1 << 5,
        HAS_DISCOUNT = 1<<6,
        HAS_ORDER = 1<<7,
        ALL = NONE | HAS_SHOPPINGCART | HAS_WARTERMARK | HAS_USER | HAS_NEWSLETTER | HAS_RESPONSIVE | HAS_WARTERMARK_IMG | HAS_DISCOUNT | HAS_ORDER
    }
    public enum SupportType : byte
    {
        YAHOO,
        SKYPE,
        PHONE,
        MSN
    }
    public enum WebsiteStatus
    {
        ACTIVE,
        STOPPED,
        PENDING,
        DELETED
    }

    public enum WatermarkPostion : byte
    {
        Top,
        TopLeft,
        TopRight,
        TopFill,
        Center,
        CenterFill,
        Bottom,
        BottomLeft,
        BottomRight,
        BottomFill
    }
}
