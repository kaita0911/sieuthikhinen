using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    [Table("ArticleFile")]
    public class ArticleFile
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? SortOrder { get; set; }
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        public DateTime? DateCreated { get; set; }
        public ArticleFileType FileType { get; set; }
        [MaxLength(300)]
        public string OriginalFileName { get; set; }
        [MaxLength(300)]
        public string FileName { get; set; }
        [MaxLength(300)]
        public string FullPath { get; set; }
        public int FileSize { get; set; }
        public bool IsDefault { get; set; }
        public ArticleFileFlags Flags { get; set; }
        public VList<ArticleFileDetail> ArticleFileDetail { get; set; }
        [NotMapped]
        public int LanguageId { get; set; }
        [NotMapped]
        public string Title
        {
            get
            {
                if (this.LanguageId <= 0) this.LanguageId = 1;
                if (this.ArticleFileDetail != null && this.LanguageId >= 1)
                    return this.ArticleFileDetail[this.LanguageId] != null ? this.ArticleFileDetail[this.LanguageId].Title : "";
                else return "";
            }
        }

        [NotMapped]
        public string Description
        {
            get
            {
                if (this.LanguageId <= 0) this.LanguageId = 1;
                if (this.ArticleFileDetail != null && this.LanguageId >= 1)
                    return this.ArticleFileDetail[this.LanguageId] != null ? this.ArticleFileDetail[this.LanguageId].Description : "";
                else return "";
            }
        }

        [NotMapped]
        public string FilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FileName))
                    return string.Empty;
                else
                    return string.Format("~/UserUpload/{0}/{1}", this.Article.ArticleType.Code.ToLower(), this.FileName);
            }
        }
        [NotMapped]
        public bool IsHot
        {
            get { return Flags.HasFlag(ArticleFileFlags.HOT); }
            set { Flags = value ? Flags | ArticleFileFlags.HOT : Flags & ~ArticleFileFlags.HOT; }
        }

        [NotMapped]
        public bool IsMostView
        {
            get { return Flags.HasFlag(ArticleFileFlags.MOSTVIEW); }
            set { Flags = value ? Flags | ArticleFileFlags.MOSTVIEW : Flags & ~ArticleFileFlags.MOSTVIEW; }
        }

        [NotMapped]
        public bool IsNew
        {
            get { return Flags.HasFlag(ArticleFileFlags.NEW); }
            set { Flags = value ? Flags | ArticleFileFlags.NEW : Flags & ~ArticleFileFlags.NEW; }
        }
    }

    [Table("ArticleFileDetail")]
    public class ArticleFileDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int ArticleFileId { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }
    }

    [Flags]
    public enum ArticleFileType
    {
        ATTACHMENT = 1,
        IMAGE = 2,
        VIDEO = 4,
        CONTENT = 8
    }

    [Flags]
    public enum ArticleFileFlags
    {
        NEW = 1 << 1,
        HOT = 1 << 2,
        MOSTVIEW = 1 << 3,
        ALL = HOT | MOSTVIEW | NEW
    }
}
