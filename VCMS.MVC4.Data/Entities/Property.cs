using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VCMS.MVC4.Data
{
    [Table("Properties")]
    public class Property
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20), Column(TypeName = "varchar"), Required]
        public string Code { get; set; }
        public bool MultiLanguage { get; set; }
        public bool MultiValue { get; set; }
        public int? SortOrder { get; set; }
        public PropertyType PropertyType { get; set; }
        public VList<PropertyDetail> PropertyDetail { get; set; }

        [NotMapped]
        public ICollection<PropertyMultiValue> PropertyMultiValues
        {
            get
            {
                using (DataContext db = new DataContext())
                {
                    var pmvs = db.PropertyMultiValues.Include("PropertyMultiValueDetail").Where(p => p.PropertyId == this.Id).ToList();
                    return pmvs;
                }
            }
        }
        public ICollection<ArticleType> ArticleTypes { get; set; }
        [NotMapped]
        public string[] Values
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Choices))
                    return new string[] { };
                return Choices.Split('|');
            }
        }
        [Column(TypeName = "nvarchar")]
        public string Choices { get; set; }

        public PropertyEntityType EntityType { get; set; }
    }
    public enum PropertyType : byte
    {
        STRING = 1,
        TEXT = 2,
        NUMERIC = 3,
        BOOLEAN = 4,
        CHOICE = 5,
        MULTICHOICE = 6,
        FILEUPLOAD = 7,
        HTML = 8,
        HASPRICE = 9
    }
    public enum PropertyEntityType : byte
    {
        ARTICLE,
        ARTICLETYPE,
        CATEGORY,
        WEBSITE
    }

    [Table("PropertyDetail")]
    public class PropertyDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }

    [Table("ArticlePropertyValue")]
    public class ArticlePropertyValue
    {
        [Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Key, Column(Order = 1)]
        public Guid MultiId { get; set; }
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
        public int? LanguageId { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public string Code { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public int? SortOrder { get; set; }
        public bool IsDefault { get; set; }
        public T GetValue<T>()
        {
            try
            {
                return (T)Convert.ChangeType(this.Value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        public void setDefault(bool value)
        {
            this.IsDefault = false;
            using (DataContext db = new DataContext())
            {
                if (value)
                {
                    var result = db.ArticlePropertyValues.Where(apv => apv.MultiId != this.MultiId);
                    result.ToList().ForEach(apv => apv.IsDefault = false);
                    try
                    {
                        db.SaveChanges();
                        this.IsDefault = true;
                    }
                    catch { };
                }
            }
        }
    }

    [Table("PropertyMultiValue")]
    public class PropertyMultiValue
    {
        public PropertyMultiValue()
        {
            this.Articles = new List<Article>();
            this.LanguageId = 1;

        }
        [Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
        public ICollection<Article> Articles { get; set; }
        public VList<PropertyMultiValueDetail> PropertyMultiValueDetail { get; set; }
        [NotMapped]
        public int LanguageId { get; set; }
        //[NotMapped]
        //public string Value
        //{
        //    get
        //    {
        //        if (this.LanguageId == 0) this.LanguageId = 1;
        //        if (this.PropertyMultiValueDetail != null && this.LanguageId > 0)
        //            return this.PropertyMultiValueDetail[this.LanguageId].Value;
        //        else
        //            return "";
        //    }
        //}
    }

    [Table("PropertyMultiValueDetail")]
    public class PropertyMultiValueDetail : LanguageBase
    {
        [Key, Column(Order = 0)]
        public int PropertyMultiValueId { get; set; }
        [ForeignKey("PropertyMultiValueId")]
        public PropertyMultiValue PropertyMultiValue { get; set; }
        public string Value { get; set; }
    }
}
