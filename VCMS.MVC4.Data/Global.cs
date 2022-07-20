using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.MVC4.Data
{
    public abstract class LanguageBase
    {
        [Key, Column(Order = 1)]
        public virtual int LanguageId { get; set; }
    }
    public class VList<T> : List<T> where T:LanguageBase, new()
    {
        public static VList<T> Create(ICollection<Language> languages)
        {
            var lst = new VList<T>();
            foreach (var l in languages)
            {
                lst.Add(new T { LanguageId = l.Id });
            }
            return lst;
        }
        public static VList<T> FromList(ICollection<T> list)
        {
            var lst = new VList<T>();
            lst.AddRange(list);
            return lst;
        }
        public new T this[int index]
        {
            get {
                return this.Where(t => t.LanguageId == index).SingleOrDefault();
            }
            set { /* set the specified index to value here */ }
        }
       
    }


}
