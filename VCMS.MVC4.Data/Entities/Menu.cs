using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    [Table("MenuItems")]
    public class MenuItem
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int WebsiteId { get; set; }

        public Website Website { get; set; }

        public int SortOrder { get; set; }

        public MenuItemType ItemType { get; set; }

        public MenuItemFlag Flag { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public int? ArticleTypeId { get; set; }

        public ArticleType ArticleType { get; set; }

        public VList<MenuItemDetail> MenuItemDetail { get; set; }

        //public int? ParentId { get; set; }
        //public MenuItem Parent { get; set; }

        //public ICollection<MenuItem> Children { get; set; }

        [NotMapped]
        public bool HasSubMenu
        {
            get
            {
                return Flag.HasFlag(MenuItemFlag.SUBMENU);
            }
            set {
                Flag = value ? Flag | MenuItemFlag.SUBMENU : Flag & ~MenuItemFlag.SUBMENU;
            }
        }

        [NotMapped]
        public int LanguageId { get; set; }

        [NotMapped]
        public string ItemText {
            get {
                if (this.LanguageId <= 0) this.LanguageId = 1;
                if (this.MenuItemDetail != null) return this.MenuItemDetail[this.LanguageId].Text;
                else return "";
            }
        }

        public static ICollection<MenuItem> GetMenu(int languageId)
        {
            using (DataContext db = new DataContext())
            {
                var query = (from a in db.MenuItems
                             join d in db.MenuItemDetails on a.Id equals d.MenuItemId
                             join t in db.ArticleTypes on a.ArticleTypeId equals t.Id
                             join td in db.ArticleTypeDetails on t.Id equals td.ArticleTypeId
                             where d.LanguageId == languageId && td.LanguageId == languageId
                             select new
                             {
                                 a = a,
                                 d = d,
                                 t = t,
                                 td = td
                             });
                var article = query.ToList();



                var ret = new List<MenuItem>();
                foreach (var item in article)
                {
                    item.a.MenuItemDetail = new VList<MenuItemDetail>() { item.d };
                    item.a.LanguageId = languageId;
                    item.t.ArticleTypeDetail = new VList<ArticleTypeDetail> { item.td };
                    item.t.LanguageId = languageId;
                    item.a.ArticleType = item.t;
                    ret.Add(item.a);
                }
                return ret.OrderBy(m => m.SortOrder).ToList();
            }
        }
    }

    public class MenuItemDetail : LanguageBase
    {
        [Column(Order=0),Key]
        public int MenuItemId { get; set; }
        public string Text { get; set; }
    }
    public enum MenuItemType
    {
        LINK,
        MODULE
    }
    [Flags]
    public enum MenuItemFlag
    {
        SUBMENU = 1,
        NEWWINDOW = 2
    }
}
