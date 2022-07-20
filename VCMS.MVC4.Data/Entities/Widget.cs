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
    [Table("Widget")]
    public class Widget
    {
        public Widget()
        {
            this.Number = 4;
            this.WidgetDetail = new VList<WidgetDetail>();
            this.WidgetsortOrder = 0;
            this.SortOrder = ArticleSortOrder.SORT_ORDER;
            this.SortDirection = SortDirection.DESCENDING;
            this.Attribute = ArticleFlags.ACTIVE;
            this.Includes = ArticleIncludeFlags.ARTICLE_TYPE;
            this.WidgetType = WidgetType.ARTICLE;
            this.NumberText = 20;
        }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }
        public int? ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }
        public int Number { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string IconHeading { get; set; }
        public int? WidgetsortOrder { get; set; }
        [MaxLength(100), Column(TypeName = "varchar")]
        public string WidgetView { get; set; }
        public int NumberText { get; set; }
        public int? WidgetGroupId { get; set; }
        public WidgetGroup WidgetGroup { get; set; }
        public WidgetType WidgetType { get; set; }
        public WidgetFlag Flag { get; set; }
        public ArticleFlags Attribute { get; set; }
        public ArticleIncludeFlags Includes { get; set; }
        public ArticleSortOrder SortOrder { get; set; }
        public SortDirection SortDirection { get; set; }
        public VList<WidgetDetail> WidgetDetail { get; set; }

        //public WidgetStatus Status { get; set; }

        [NotMapped]
        public int LanguageId { get; set; }
        [NotMapped]
        public Boolean IsCustommer
        {
            get { return Flag.HasFlag(WidgetFlag.Custommer); }
            set { Flag = value ? Flag | WidgetFlag.Custommer : (Flag & ~WidgetFlag.Custommer); }
        }


        [NotMapped]
        public string Value
        {
            get
            {
                if (this.LanguageId == 0) this.LanguageId = 1;
                if (this.WidgetDetail != null && this.LanguageId >= 1)
                    return this.WidgetDetail[this.LanguageId].Value;
                else return "";
            }
        }
        [NotMapped]
        public string Title
        {
            get
            {
                if (this.LanguageId == 0) this.LanguageId = 1;
                if (this.WidgetDetail != null && this.LanguageId >= 1)
                    return this.WidgetDetail[this.LanguageId].Title;
                else return "";
            }
        }

        #region Banner - Slider
        [NotMapped]
        public Boolean Pagination
        {
            get { return Flag.HasFlag(WidgetFlag.Pagination); }
            set { Flag = value ? Flag | WidgetFlag.Pagination : (Flag & ~WidgetFlag.Pagination); }
        }
        [NotMapped]
        public Boolean Navigation
        {
            get { return Flag.HasFlag(WidgetFlag.Navigation); }
            set { Flag = value ? Flag | WidgetFlag.Navigation : (Flag & ~WidgetFlag.Navigation); }
        }
        [NotMapped]
        public Boolean Thumbnail
        {
            get { return Flag.HasFlag(WidgetFlag.ThumbNail); }
            set { Flag = value ? Flag | WidgetFlag.ThumbNail : (Flag & ~WidgetFlag.ThumbNail); }
        }
        [NotMapped]
        public Boolean AutoPlay
        {
            get { return Flag.HasFlag(WidgetFlag.AutoPlay); }
            set { Flag = value ? Flag | WidgetFlag.AutoPlay : (Flag & ~WidgetFlag.AutoPlay); }
        }
        [NotMapped]
        public Boolean PlayPause
        {
            get { return Flag.HasFlag(WidgetFlag.PlayPause); }
            set { Flag = value ? Flag | WidgetFlag.PlayPause : (Flag & ~WidgetFlag.PlayPause); }
        }
        [NotMapped]
        public Boolean AutoHeight
        {
            get { return Flag.HasFlag(WidgetFlag.AutoHeight); }
            set { Flag = value ? Flag | WidgetFlag.AutoHeight : (Flag & ~WidgetFlag.AutoHeight); }
        }
        [NotMapped]
        public Boolean HoverStop
        {
            get { return Flag.HasFlag(WidgetFlag.HoverStop); }
            set { Flag = value ? Flag | WidgetFlag.HoverStop : (Flag & ~WidgetFlag.HoverStop); }
        }
        #endregion

        #region Adv
        [NotMapped]
        public Boolean Effect1
        {
            get { return Flag.HasFlag(WidgetFlag.Effect1); }
            set { Flag = value ? Flag | WidgetFlag.Effect1 : (Flag & ~WidgetFlag.Effect1); }
        }
        [NotMapped]
        public Boolean Effect2
        {
            get { return Flag.HasFlag(WidgetFlag.Effect2); }
            set { Flag = value ? Flag | WidgetFlag.Effect2 : (Flag & ~WidgetFlag.Effect2); }
        }
        [NotMapped]
        public Boolean Effect3
        {
            get { return Flag.HasFlag(WidgetFlag.Effect3); }
            set { Flag = value ? Flag | WidgetFlag.Effect3 : (Flag & ~WidgetFlag.Effect3); }
        }
        [NotMapped]
        public Boolean Effect4
        {
            get { return Flag.HasFlag(WidgetFlag.Effect4); }
            set { Flag = value ? Flag | WidgetFlag.Effect4 : (Flag & ~WidgetFlag.Effect4); }
        }
        [NotMapped]
        public Boolean Effect5
        {
            get { return Flag.HasFlag(WidgetFlag.Effect5); }
            set { Flag = value ? Flag | WidgetFlag.Effect5 : (Flag & ~WidgetFlag.Effect5); }
        }
        [NotMapped]
        public Boolean Effect6
        {
            get { return Flag.HasFlag(WidgetFlag.Effect6); }
            set { Flag = value ? Flag | WidgetFlag.Effect6 : (Flag & ~WidgetFlag.Effect6); }
        }
        [NotMapped]
        public Boolean Effect7
        {
            get { return Flag.HasFlag(WidgetFlag.Effect7); }
            set { Flag = value ? Flag | WidgetFlag.Effect7 : (Flag & ~WidgetFlag.Effect7); }
        }
        #endregion

        #region Article
        [NotMapped]
        public Boolean LazyOwl
        {
            get { return Flag.HasFlag(WidgetFlag.LazyOwl); }
            set { Flag = value ? Flag | WidgetFlag.LazyOwl : (Flag & ~WidgetFlag.LazyOwl); }
        }
        [NotMapped]
        public Boolean HiddenImage
        {
            get { return Flag.HasFlag(WidgetFlag.HiddenImage); }
            set { Flag = value ? Flag | WidgetFlag.HiddenImage : (Flag & ~WidgetFlag.HiddenImage); }
        }
        [NotMapped]
        public Boolean ShowDate
        {
            get { return Flag.HasFlag(WidgetFlag.ShowDate); }
            set { Flag = value ? Flag | WidgetFlag.ShowDate : (Flag & ~WidgetFlag.ShowDate); }
        }
        [NotMapped]
        public Boolean ShowDescription
        {
            get { return Flag.HasFlag(WidgetFlag.ShowDescription); }
            set { Flag = value ? Flag | WidgetFlag.ShowDescription : (Flag & ~WidgetFlag.ShowDescription); }
        }
        [NotMapped]
        public Boolean ShowRating
        {
            get { return Flag.HasFlag(WidgetFlag.ShowRating); }
            set { Flag = value ? Flag | WidgetFlag.ShowRating : (Flag & ~WidgetFlag.ShowRating); }
        }
        [NotMapped]
        public Boolean ShowPrice
        {
            get { return Flag.HasFlag(WidgetFlag.ShowPrice); }
            set { Flag = value ? Flag | WidgetFlag.ShowPrice : (Flag & ~WidgetFlag.ShowPrice); }
        }
        [NotMapped]
        public Boolean ShowIconFly
        {
            get { return Flag.HasFlag(WidgetFlag.ShowIconFly); }
            set { Flag = value ? Flag | WidgetFlag.ShowIconFly : (Flag & ~WidgetFlag.ShowIconFly); }
        }
        [NotMapped]
        public Boolean ShowButton
        {
            get { return Flag.HasFlag(WidgetFlag.ShowButton); }
            set { Flag = value ? Flag | WidgetFlag.ShowButton : (Flag & ~WidgetFlag.ShowButton); }
        }
        [NotMapped]
        public Boolean ShowViewMore
        {
            get { return Flag.HasFlag(WidgetFlag.ShowViewMore); }
            set { Flag = value ? Flag | WidgetFlag.ShowViewMore : (Flag & ~WidgetFlag.ShowViewMore); }
        }
        [NotMapped]
        public Boolean ShowViewAll
        {
            get { return Flag.HasFlag(WidgetFlag.ShowViewAll); }
            set { Flag = value ? Flag | WidgetFlag.ShowViewAll : (Flag & ~WidgetFlag.ShowViewAll); }
        }
        [NotMapped]
        public Boolean ShowSkype
        {
            get { return Flag.HasFlag(WidgetFlag.ShowSkype); }
            set { Flag = value ? Flag | WidgetFlag.ShowSkype : (Flag & ~WidgetFlag.ShowSkype); }
        }
        [NotMapped]
        public Boolean ShowYahoo
        {
            get { return Flag.HasFlag(WidgetFlag.ShowYahoo); }
            set { Flag = value ? Flag | WidgetFlag.ShowYahoo : (Flag & ~WidgetFlag.ShowYahoo); }
        }
        [NotMapped]
        public Boolean ShowPhone
        {
            get { return Flag.HasFlag(WidgetFlag.ShowPhone); }
            set { Flag = value ? Flag | WidgetFlag.ShowPhone : (Flag & ~WidgetFlag.ShowPhone); }
        }
        #endregion

        #region Article Include
        [NotMapped]
        public Boolean IncludeArticleDetail
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.ARTICLE_DETAIL); }
            set { Includes = value ? Includes | ArticleIncludeFlags.ARTICLE_DETAIL : (Includes & ~ArticleIncludeFlags.ARTICLE_DETAIL); }
        }
        [NotMapped]
        public Boolean IncludeArticleType
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.ARTICLE_TYPE); }
            set { Includes = value ? Includes | ArticleIncludeFlags.ARTICLE_TYPE : (Includes & ~ArticleIncludeFlags.ARTICLE_TYPE); }
        }
        [NotMapped]
        public Boolean IncludeCategory
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.CATEGORIES); }
            set { Includes = value ? Includes | ArticleIncludeFlags.CATEGORIES : (Includes & ~ArticleIncludeFlags.CATEGORIES); }
        }
        [NotMapped]
        public Boolean IncludeProperty
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.PROPERTIES); }
            set { Includes = value ? Includes | ArticleIncludeFlags.PROPERTIES : (Includes & ~ArticleIncludeFlags.PROPERTIES); }
        }
        [NotMapped]
        public Boolean IncludePrice
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.PRICES); }
            set { Includes = value ? Includes | ArticleIncludeFlags.PRICES : (Includes & ~ArticleIncludeFlags.PRICES); }
        }
        [NotMapped]
        public Boolean IncludeDiscount
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.DISCOUNTS); }
            set { Includes = value ? Includes | ArticleIncludeFlags.DISCOUNTS : (Includes & ~ArticleIncludeFlags.DISCOUNTS); }
        }
        [NotMapped]
        public Boolean IncludeComment
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.COMMENTS); }
            set { Includes = value ? Includes | ArticleIncludeFlags.COMMENTS : (Includes & ~ArticleIncludeFlags.COMMENTS); }
        }
        [NotMapped]
        public Boolean IncludeKeyword
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.KEYWORDS); }
            set { Includes = value ? Includes | ArticleIncludeFlags.KEYWORDS : (Includes & ~ArticleIncludeFlags.KEYWORDS); }
        }
        [NotMapped]
        public Boolean IncludeFile
        {
            get { return Includes.HasFlag(ArticleIncludeFlags.FILES); }
            set { Includes = value ? Includes | ArticleIncludeFlags.FILES : (Includes & ~ArticleIncludeFlags.FILES); }
        }
        #endregion

        [NotMapped]
        public Boolean ShowSubMenu
        {
            get { return Flag.HasFlag(WidgetFlag.ShowSubMenu); }
            set { Flag = value ? Flag | WidgetFlag.ShowSubMenu : (Flag & ~WidgetFlag.ShowSubMenu); }
        }
    }

    [Table("WidgetDetail")]
    public partial class WidgetDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int WidgetId { get; set; }
        [ForeignKey("WidgetId")]

        public Widget Widget { get; set; }
        [Required]
        public string Title { get; set; }

        [AllowHtml]
        public string Value { get; set; }
    }

    [Table("WidgetGroup")]
    public partial class WidgetGroup
    {
        public WidgetGroup()
        {
            this.Widgets = new List<Widget>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Widget> Widgets { get; set; }
    }
    
    public enum WidgetStatus : byte
    {
        ACTIVE,
        PAUSED
    }

    public enum WidgetType
    {
        HTML,
        ARTICLE,
        WEBPLUGIN,
    }

    [Flags]
    public enum WidgetFlag
    {
        NONE = 0,
        Custommer = 1 << 1,
        //BANNER
        AutoPlay = 1 << 2,
        Navigation = 1 << 3,
        Pagination = 1 << 4,
        ThumbNail = 1 << 5,
        PlayPause = 1 << 6,
        AutoHeight = 1 << 7,
        HoverStop = 1 << 8,

        //Article
        LazyOwl = 1 << 9,
        HiddenImage = 1 << 10,
        ShowDate = 1 << 11,
        ShowDescription = 1 << 12,
        ShowRating = 1 << 13,
        ShowPrice = 1 << 14,
        ShowIconFly = 1 << 15,
        ShowButton = 1 << 16,
        ShowViewMore = 1 << 17,
        ShowViewAll = 1 << 18,

        //advtising
        Effect1 = 1 << 19,
        Effect2 = 1 << 20,
        Effect3 = 1 << 21,
        Effect4 = 1 << 22,
        Effect5 = 1 << 23,
        Effect6 = 1 << 24,
        Effect7 = 1 << 25,

        //SUPPORT
        ShowSkype = 1 << 26,
        ShowYahoo = 1 << 27,
        ShowPhone = 1 << 28,

        //Category
        ShowSubMenu = 1 << 29
    }
}
