using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VCMS.MVC4.Data
{
    [Table("Keywords")]
    public class Keyword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        //[MaxLength(50)]
        public string Tag { get; set; }

        [NotMapped]
        public int Count { get; set; }

        public ICollection<Article> Articles { get; set; }

        //public static List<Keyword> Get(int count)
        //{
        //    using (DataContext db = new DataContext())
        //    {
        //        return db.Keywords.OrderByDescending(k => k.Articles.Count).Select(k => new Keyword { Tag = k.Tag, Count = k.Articles.Count }).Take(100).ToList();
        //    }
        //}
        public static List<Keyword> Get(int count)
        {
            using (DataContext db = new DataContext())
            {
                return db.Keywords.Select(k => new { Id = k.Id, Tag = k.Tag, Count = k.Articles.Count }).Take(100).ToList().Select(a => new Keyword { Id = a.Id, Tag = a.Tag, Count = a.Count }).ToList();
            }
        }
    }
}
