using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace VCMS.MVC4.Data
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        {
            this.Newsletter = false;
            this.Accumulated = 0;
            this.Flags = UserFlags.NONE;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Column(TypeName = "varchar"), MaxLength(50), Required]
        public string UserName { get; set; }
        [MaxLength(100), Required]
        public string DisplayName { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100), Required]
        public string Email { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(200)]
        public string Fax { get; set; }
        [MaxLength(200)]
        public string Company { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        [MaxLength(50)]
        public string PostalCode { get; set; }
        public bool Newsletter { get; set; }
        public int Accumulated { get; set; }
        public UserFlags Flags { get; set; }
        public DateTime? DateCreated { get; set; }
        [NotMapped]
        [DataType(DataType.Password), StringLength(30,MinimumLength=6)]
        public string Password { get; set; }
        /// <summary>
        /// Navagation property to articles, list all articles of a specified keyword
        /// </summary>
        public ICollection<Article> Articles { get; set; }
       
        [NotMapped]
        public string NameCity
        {
            get{
                using (DataContext db = new DataContext())
                {

                    var category = db.Categories.Include(a => a.CategoryDetail).FirstOrDefault(a => a.Id == this.City);
                    if (category != null)
                    {
                        if (category.LanguageId == 0)
                            category.LanguageId = 1;
                        return category.CategoryDetail[category.LanguageId].CategoryName;
                    }
                    return "N/A";
                }
            }
        }
        [NotMapped]
        public string NameWard
        {
            get
            {
                using (DataContext db = new DataContext())
                {

                    //var category = db.Categories.Include(a => a.CategoryDetail).FirstOrDefault(a => a.Id == this.Country);
                    //if (category != null)
                    //{
                    //    if (category.LanguageId == 0)
                    //        category.LanguageId = 1;
                    //    return category.CategoryDetail[category.LanguageId].CategoryName;
                    //}
                    return "N/A";
                }
            }
        }
        [NotMapped]
        public string NameState
        {
            get
            {
                using (DataContext db = new DataContext())
                {

                    var category = db.Categories.Include(a => a.CategoryDetail).FirstOrDefault(a => a.Id == this.State);
                    if (category != null)
                    {
                        if (category.LanguageId == 0)
                            category.LanguageId = 1;
                        return category.CategoryDetail[category.LanguageId].CategoryName;
                    }
                    return "N/A";
                }
            }
        }
        [NotMapped]
        public bool HasWidget
        {
            get { return Flags.HasFlag(UserFlags.WIDGET); }
            set { Flags = value ? Flags | UserFlags.WIDGET : Flags & ~UserFlags.WIDGET; }
        }
        [NotMapped]
        public bool HasShopCart
        {
            get { return Flags.HasFlag(UserFlags.SHOPCART); }
            set { Flags = value ? Flags | UserFlags.SHOPCART : Flags & ~UserFlags.SHOPCART; }
        }
        [NotMapped]
        public bool HasOrder
        {
            get { return Flags.HasFlag(UserFlags.ORDER); }
            set { Flags = value ? Flags | UserFlags.ORDER : Flags & ~UserFlags.ORDER; }
        }
        [NotMapped]
        public bool HasCategoryType
        {
            get { return Flags.HasFlag(UserFlags.CATEGORYTYPE); }
            set { Flags = value ? Flags | UserFlags.CATEGORYTYPE : Flags & ~UserFlags.CATEGORYTYPE; }
        }
    }
    [Table("UserArticleTypeCate")]
    public class UserArticleTypeCate
    {
        public UserArticleTypeCate()
        {

        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ArticleTypeId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
    [Flags]
    public enum UserFlags
    {
        NONE = 0,
        WIDGET = 1 << 0,
        SHOPCART = 1 << 1,
        ORDER = 1 << 2,
        CATEGORYTYPE = 1 << 3,
        ALL = NONE | WIDGET | SHOPCART | ORDER | CATEGORYTYPE
    }
}
