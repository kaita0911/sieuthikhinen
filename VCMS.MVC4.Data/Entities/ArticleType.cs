using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VCMS.MVC4.Data
{
    [Table("ArticleTypes")]
    public partial class ArticleType
    {
        public ArticleType()
        {
            this.ArticleTypeDetail = new VList<ArticleTypeDetail>();
            this.Articles = new List<Article>();
            this.Categories = new List<Category>();
            this.CategoryTypes = new List<CategoryType>();
            this.Properties = new List<Property>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(50), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }
        public ArticleTypeFlags Flag { get; set; }
        public PropertyFlags PropertyFlag { get; set; }
        public DisplayFlags DisplayFlag { get; set; }
        public AttributeFlags AttributeFlag { get; set; }
        public ShoppingCartFlags ShoppingCartFlag { get; set; }
        public TypePosts TypePost { get; set; }
        public int WebsiteId { get; set; }

        public int? SortOrder { get; set; }

        public VList<ArticleTypeDetail> ArticleTypeDetail { get; set; }

        public ICollection<Property> Properties { get; set; }

        public ICollection<Article> Articles { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<CategoryType> CategoryTypes { get; set; }
        public PageType HomePageType { get; set; }

        public PageType CategoryPageType { get; set; }
        public string StrIsNew { get; set; }
        public string StrIsHot { get; set; }
        public string StrIsMostView { get; set; }
        public string StrIsNewCate { get; set; }
        public string StrIsHotCate { get; set; }
        public string StrIsMostViewCate { get; set; }

        [NotMapped]
        public Boolean HasCategory
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.CATEGORY); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.CATEGORY : (PropertyFlag & ~PropertyFlags.CATEGORY); }
        }

        [NotMapped]
        public Boolean HasPrice
        {
            get { return ShoppingCartFlag.HasFlag(ShoppingCartFlags.PRICE); }
            set { ShoppingCartFlag = value ? ShoppingCartFlag | ShoppingCartFlags.PRICE : (ShoppingCartFlag & ~ShoppingCartFlags.PRICE); }
        }

        [NotMapped]
        public Boolean HasNumberProduct
        {
            get { return ShoppingCartFlag.HasFlag(ShoppingCartFlags.NUMBER); }
            set { ShoppingCartFlag = value ? ShoppingCartFlag | ShoppingCartFlags.NUMBER : (ShoppingCartFlag & ~ShoppingCartFlags.NUMBER); }
        }

        [NotMapped]
        public Boolean HasDiscount
        {
            get { return ShoppingCartFlag.HasFlag(ShoppingCartFlags.DISCOUNT); }
            set { ShoppingCartFlag = value ? ShoppingCartFlag | ShoppingCartFlags.DISCOUNT : (ShoppingCartFlag & ~ShoppingCartFlags.DISCOUNT); }
        }

        [NotMapped]
        public Boolean HasPrivateCate
        {
            get { return Flag.HasFlag(ArticleTypeFlags.PRIVATECATE); }
            set { Flag = value ? Flag | ArticleTypeFlags.PRIVATECATE : (Flag & ~ArticleTypeFlags.PRIVATECATE); }
        }

        [NotMapped]
        public Boolean HasPoster
        {
            get { return Flag.HasFlag(ArticleTypeFlags.HASPOSTER); }
            set { Flag = value ? Flag | ArticleTypeFlags.HASPOSTER : (Flag & ~ArticleTypeFlags.HASPOSTER); }
        }

        [NotMapped]
        public Boolean HasImage
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.IMAGE); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.IMAGE : (PropertyFlag & ~PropertyFlags.IMAGE); }
        }

        [NotMapped]
        public Boolean HasAttachment
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.ATTACHEMENT); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.ATTACHEMENT : (PropertyFlag & ~PropertyFlags.ATTACHEMENT); }
        }


        [NotMapped]
        public Boolean HasCopyAndMove
        {
            get { return Flag.HasFlag(ArticleTypeFlags.COPYANDMOVE); }
            set { Flag = value ? Flag | ArticleTypeFlags.COPYANDMOVE : (Flag & ~ArticleTypeFlags.COPYANDMOVE); }
        }

        [NotMapped]
        public Boolean MultiImage
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.MULTI_IMAGE); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.MULTI_IMAGE : (PropertyFlag & ~PropertyFlags.MULTI_IMAGE); }
        }

        [NotMapped]
        public Boolean HasDescription
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.DESCRIPTION); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.DESCRIPTION : (PropertyFlag & ~PropertyFlags.DESCRIPTION); }
        }

        [NotMapped]
        public Boolean HasShorDesc
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.SHORT_DESCRIPTION); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.SHORT_DESCRIPTION : (PropertyFlag & ~PropertyFlags.SHORT_DESCRIPTION); }
        }

        [NotMapped]
        public Boolean HasMeta
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.META); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.META : (PropertyFlag & ~PropertyFlags.META); }
        }

        [NotMapped]
        public Boolean AllowSearch
        {
            get { return Flag.HasFlag(ArticleTypeFlags.SEARCH); }
            set { Flag = value ? Flag | ArticleTypeFlags.SEARCH : (Flag & ~ArticleTypeFlags.SEARCH); }
        }

        [NotMapped]
        public Boolean SiteMap
        {
            get { return Flag.HasFlag(ArticleTypeFlags.SITEMAP); }
            set { Flag = value ? Flag | ArticleTypeFlags.SITEMAP : (Flag & ~ArticleTypeFlags.SITEMAP); }
        }

        [NotMapped]
        public Boolean HasRating
        {
            get { return Flag.HasFlag(ArticleTypeFlags.RATING); }
            set { Flag = value ? Flag | ArticleTypeFlags.RATING : (Flag & ~ArticleTypeFlags.RATING); }
        }
        [NotMapped]
        public Boolean HasImportExPort
        {
            get { return Flag.HasFlag(ArticleTypeFlags.IMPORTEXPORT); }
            set { Flag = value ? Flag | ArticleTypeFlags.IMPORTEXPORT : (Flag & ~ArticleTypeFlags.IMPORTEXPORT); }
        }
        [NotMapped]
        public Boolean HasWidget
        {
            get { return Flag.HasFlag(ArticleTypeFlags.WIDGET); }
            set { Flag = value ? Flag | ArticleTypeFlags.WIDGET : (Flag & ~ArticleTypeFlags.WIDGET); }
        }
        [NotMapped]
        public Boolean IsHot
        {
            get { return AttributeFlag.HasFlag(AttributeFlags.HOT); }
            set { AttributeFlag = value ? AttributeFlag | AttributeFlags.HOT : (AttributeFlag & ~AttributeFlags.HOT); }
        }

        [NotMapped]
        public Boolean IsNew
        {
            get { return AttributeFlag.HasFlag(AttributeFlags.NEW); }
            set { AttributeFlag = value ? AttributeFlag | AttributeFlags.NEW : (AttributeFlag & ~AttributeFlags.NEW); }
        }

        [NotMapped]
        public Boolean IsMostView
        {
            get { return AttributeFlag.HasFlag(AttributeFlags.MOSTVIEW); }
            set { AttributeFlag = value ? AttributeFlag | AttributeFlags.MOSTVIEW : (AttributeFlag & ~AttributeFlags.MOSTVIEW); }
        }

        [NotMapped]
        public Boolean IsHotCate
        {
            get { return AttributeFlag.HasFlag(AttributeFlags.HOTCATE); }
            set { AttributeFlag = value ? AttributeFlag | AttributeFlags.HOTCATE : (AttributeFlag & ~AttributeFlags.HOTCATE); }
        }

        [NotMapped]
        public Boolean IsNewCate
        {
            get { return AttributeFlag.HasFlag(AttributeFlags.NEWCATE); }
            set { AttributeFlag = value ? AttributeFlag | AttributeFlags.NEWCATE : (AttributeFlag & ~AttributeFlags.NEWCATE); }
        }

        [NotMapped]
        public Boolean IsMostViewCate
        {
            get { return AttributeFlag.HasFlag(AttributeFlags.MOSTVIEWCATE); }
            set { AttributeFlag = value ? AttributeFlag | AttributeFlags.MOSTVIEWCATE : (AttributeFlag & ~AttributeFlags.MOSTVIEWCATE); }
        }

        [NotMapped]
        public Boolean IsContent
        {
            get { return Flag.HasFlag(ArticleTypeFlags.CONTENT); }
            set { Flag = value ? Flag | ArticleTypeFlags.CONTENT : (Flag & ~ArticleTypeFlags.CONTENT); }
        }

        [NotMapped]
        public Boolean HiddenUrl
        {
            get { return PropertyFlag.HasFlag(PropertyFlags.HIDDENURL); }
            set { PropertyFlag = value ? PropertyFlag | PropertyFlags.HIDDENURL : (PropertyFlag & ~PropertyFlags.HIDDENURL); }
        }

        [NotMapped]
        public Boolean IsFirstArticle
        {
            get { return HomePageType.HasFlag(PageType.FIRST_ARTICLE); }
            set { HomePageType = value ? HomePageType | PageType.FIRST_ARTICLE : (HomePageType & ~PageType.FIRST_ARTICLE); }
        }

        [NotMapped]
        public Boolean IsListCategory
        {
            get { return HomePageType.HasFlag(PageType.LIST_CATEGORY); }
            set { HomePageType = value ? HomePageType | PageType.LIST_CATEGORY : (HomePageType & ~PageType.LIST_CATEGORY); }
        }

        #region Article Display
        [NotMapped]
        public Boolean HasRequiredLogin
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.REQUIRED); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.REQUIRED : (DisplayFlag & ~DisplayFlags.REQUIRED); }
        }
        [NotMapped]
        public Boolean AllowView
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.VIEW); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.VIEW : (DisplayFlag & ~DisplayFlags.VIEW); }
        }
        [NotMapped]
        public Boolean ShowImage
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWIMAGE); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWIMAGE : (DisplayFlag & ~DisplayFlags.SHOWIMAGE); }
        }
        [NotMapped]
        public Boolean ShowComment
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWCOMMENT); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWCOMMENT : (DisplayFlag & ~DisplayFlags.SHOWCOMMENT); }
        }
        [NotMapped]
        public Boolean ShowShareArticle
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHAREARTICLE); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHAREARTICLE : (DisplayFlag & ~DisplayFlags.SHAREARTICLE); }
        }
        [NotMapped]
        public Boolean OtherArticleAjax
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.OTHERARTICLEAJAX); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.OTHERARTICLEAJAX : (DisplayFlag & ~DisplayFlags.OTHERARTICLEAJAX); }
        }
        #endregion

        #region Article List Display
        [NotMapped]
        public Boolean ShowImageInList
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWIMAGELIST); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWIMAGELIST : (DisplayFlag & ~DisplayFlags.SHOWIMAGELIST); }
        }
        [NotMapped]
        public Boolean ShowDescription
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWDESCRIPTION); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWDESCRIPTION : (DisplayFlag & ~DisplayFlags.SHOWDESCRIPTION); }
        }
        [NotMapped]
        public Boolean ShowDateTime
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWDATETIME); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWDATETIME : (DisplayFlag & ~DisplayFlags.SHOWDATETIME); }
        }
        [NotMapped]
        public Boolean ShowActionLink
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWACTIONLINK); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWACTIONLINK : (DisplayFlag & ~DisplayFlags.SHOWACTIONLINK); }
        }
        [NotMapped]
        public Boolean ShowIconFly
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWICONFLY); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWICONFLY : (DisplayFlag & ~DisplayFlags.SHOWICONFLY); }
        }
        [NotMapped]
        public Boolean ShowRating
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWRATING); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWRATING : (DisplayFlag & ~DisplayFlags.SHOWRATING); }
        }
        [NotMapped]
        public Boolean ShowAttribute
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SHOWATTRIBUTE); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SHOWATTRIBUTE : (DisplayFlag & ~DisplayFlags.SHOWATTRIBUTE); }
        }
        [NotMapped]
        public Boolean ShowFilter
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.FILTER); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.FILTER : (DisplayFlag & ~DisplayFlags.FILTER); }
        }
        [NotMapped]
        public Boolean ShowChildCategory
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.CHILDCATEGORY); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.CHILDCATEGORY : (DisplayFlag & ~DisplayFlags.CHILDCATEGORY); }
        }
        [NotMapped]
        public Boolean DescriptionCategory
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.DESCRIPTION); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.DESCRIPTION : (DisplayFlag & ~DisplayFlags.DESCRIPTION); }
        }
        #endregion

        #region Article Type Post
        [NotMapped]
        public Boolean IsStandard
        {
            get { return TypePost.HasFlag(TypePosts.STANDARD); }
            set { TypePost = value ? TypePost | TypePosts.STANDARD : (TypePost & ~TypePosts.STANDARD); }
        }
        [NotMapped]
        public Boolean IsFullWidth
        {
            get { return TypePost.HasFlag(TypePosts.FULLWIDTH); }
            set { TypePost = value ? TypePost | TypePosts.FULLWIDTH : (TypePost & ~TypePosts.FULLWIDTH); }
        }
        [NotMapped]
        public Boolean IsGallery
        {
            get { return TypePost.HasFlag(TypePosts.GALLERY); }
            set { TypePost = value ? TypePost | TypePosts.GALLERY : (TypePost & ~TypePosts.GALLERY); }
        }
        [NotMapped]
        public Boolean IsVideo
        {
            get { return TypePost.HasFlag(TypePosts.VIDEO); }
            set { TypePost = value ? TypePost | TypePosts.VIDEO : (TypePost & ~TypePosts.VIDEO); }
        }
        [NotMapped]
        public Boolean IsMusic
        {
            get { return TypePost.HasFlag(TypePosts.MUSIC); }
            set { TypePost = value ? TypePost | TypePosts.MUSIC : (TypePost & ~TypePosts.MUSIC); }
        }
        #endregion

        #region Grid Article Column
        [NotMapped]
        public Boolean FourColumn
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.FOURCOLUMN); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.FOURCOLUMN : (DisplayFlag & ~DisplayFlags.FOURCOLUMN); }
        }
        [NotMapped]
        public Boolean FiveColumn
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.FIVECOLUMN); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.FIVECOLUMN : (DisplayFlag & ~DisplayFlags.FIVECOLUMN); }
        }
        [NotMapped]
        public Boolean SixColumn
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.SIXCOLUMN); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.SIXCOLUMN : (DisplayFlag & ~DisplayFlags.SIXCOLUMN); }
        }
        [NotMapped]
        public Boolean TwoColumnSiderBar
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.TWOCOLUMNSIDERBAR); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.TWOCOLUMNSIDERBAR : (DisplayFlag & ~DisplayFlags.TWOCOLUMNSIDERBAR); }
        }
        [NotMapped]
        public Boolean ThreeColumnSiderBar
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.THREECOLUMNSIDERBAR); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.THREECOLUMNSIDERBAR : (DisplayFlag & ~DisplayFlags.THREECOLUMNSIDERBAR); }
        }
        [NotMapped]
        public Boolean FourColumnSiderBar
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.FOURCOLUMNSIDERBAR); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.FOURCOLUMNSIDERBAR : (DisplayFlag & ~DisplayFlags.FOURCOLUMNSIDERBAR); }
        }
        [NotMapped]
        public Boolean FiveColumnSiderBar
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.FIVECOLUMNSIDERBAR); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.FIVECOLUMNSIDERBAR : (DisplayFlag & ~DisplayFlags.FIVECOLUMNSIDERBAR); }
        }
        [NotMapped]
        public Boolean Masony
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.MASONRY); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.MASONRY : (DisplayFlag & ~DisplayFlags.MASONRY); }
        }
        [NotMapped]
        public Boolean MasonySiderBar
        {
            get { return DisplayFlag.HasFlag(DisplayFlags.MASONRYSIDERBAR); }
            set { DisplayFlag = value ? DisplayFlag | DisplayFlags.MASONRYSIDERBAR : (DisplayFlag & ~DisplayFlags.MASONRYSIDERBAR); }
        }
        #endregion

        [NotMapped]
        public int LanguageId { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                if (this.LanguageId <= 0) this.LanguageId = 1;
                if (this.ArticleTypeDetail != null && this.LanguageId >= 1)
                    return this.ArticleTypeDetail[this.LanguageId].Name;
                else return "";
            }
        }

        [NotMapped]
        public string UrlFriendly   
        {
            get
            {
                if (this.LanguageId <= 0) this.LanguageId = 1;
                if (this.ArticleTypeDetail.Count > 0 && this.LanguageId >= 1)
                    return this.ArticleTypeDetail[this.LanguageId].UrlFriendly.ToLower();
                else return "";
            }
        }
    }

    [Table("ArticleTypeDetail")]
    public class ArticleTypeDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int ArticleTypeId { get; set; }
        [ForeignKey("ArticleTypeId")]
        public ArticleType ArticleType { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string UrlFriendly { get; set; }
        public string Description { get; set; }
        public string SEOKeywords { get; set; }

        public string SEODescription { get; set; }
    }

    [Flags]
    public enum AttributeFlags
    {
        NONE = 0,
        NEW = 1 << 0,
        HOT = 1 << 1,
        MOSTVIEW = 1 << 2,
        NEWCATE = 1 << 3,
        HOTCATE = 1 << 4,
        MOSTVIEWCATE = 1 << 5,
        INACTIVE = 1 << 6,
        ALL = NONE | NEW | HOT | MOSTVIEW | NEWCATE | HOTCATE | MOSTVIEWCATE
    }
    [Flags]
    public enum ShoppingCartFlags
    {
        NONE = 0,
        PRICE = 1 << 0,
        DISCOUNT = 1 << 1,
        PROPERTY_HAS_PRICE = 1 << 2,
        NUMBER = 1 << 3,
        ALL = NONE | PRICE | DISCOUNT | PROPERTY_HAS_PRICE | NUMBER
    }
    [Flags]
    public enum PropertyFlags
    {
        NONE = 0,
        CATEGORY = 1 << 0,
        IMAGE = 1 << 1,
        ATTACHEMENT = 1 << 2,
        SHORT_DESCRIPTION = 1 << 3,
        DESCRIPTION = 1 << 4,
        MULTI_IMAGE = 1 << 5,
        META = 1 << 6,
        HIDDENURL = 1 << 7,

        ALL = NONE | CATEGORY | IMAGE | ATTACHEMENT | SHORT_DESCRIPTION | DESCRIPTION | MULTI_IMAGE | META | HIDDENURL
    }

    [Flags]
    public enum DisplayFlags
    {
        NONE = 0,
        REQUIRED = 1 << 1,
        VIEW = 1 << 2,

        /// <summary>
        /// Thuộc tính hiển thị trong chi tiết bài viết
        /// </summary>
        SHOWIMAGE = 1 << 3,
        SHAREARTICLE = 1 << 4,
        SHOWCOMMENT = 1 << 5,
        OTHERARTICLEAJAX = 1 << 6,

        /// <summary>
        /// Thuộc tính hiển thị trong list article
        /// </summary>
        /// 
        SHOWIMAGELIST = 1 << 7,
        SHOWDESCRIPTION = 1 << 8,
        SHOWDATETIME = 1 << 9, //date update + view ....
        SHOWACTIONLINK = 1 << 10,
        SHOWICONFLY = 1 << 11,
        SHOWRATING = 1 << 12,
        SHOWATTRIBUTE = 1 << 13,
        FILTER = 1 << 14,
        CHILDCATEGORY = 1 << 15,
        DESCRIPTION = 1 << 16,

        FOURCOLUMN = 1 << 17,
        FIVECOLUMN = 1 << 18,
        SIXCOLUMN = 1 << 19,

        TWOCOLUMNSIDERBAR = 1 << 20,
        THREECOLUMNSIDERBAR = 1 << 21,
        FOURCOLUMNSIDERBAR = 1 << 22,
        FIVECOLUMNSIDERBAR = 1 << 23,

        MASONRY = 1 << 24,
        MASONRYSIDERBAR = 1 << 25,

        ALL = ~0
    }

    [Flags]
    public enum ArticleTypeFlags
    {
        NONE = 0,
        CONTENT = 1 << 0,
        COPYANDMOVE = 1 << 1,
        PRIVATECATE = 1 << 2,
        SITEMAP = 1 << 3,
        SEARCH = 1 << 4,
        RATING = 1 << 5,
        IMPORTEXPORT = 1 << 6,
        WIDGET = 1 << 7,
        HASPOSTER = 1 << 8,
        ALL = ~0
    }


    [Flags]
    public enum TypePosts
    {
        STANDARD = 0,
        FULLWIDTH = 1 << 0,
        GALLERY = 1 << 1, //slider show
        VIDEO = 1 << 2,
        MUSIC = 1 << 3,

    }

    public enum PageType
    {
        CUSTOM,
        FIRST_ARTICLE,
        LIST_ARTICLE,
        LIST_CATEGORY
    }


}
