using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web
{

    public class ContactModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Company { get; set; }
        public string Country { get; set; }
        //[Required, DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]

        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Fax { get; set; }
        [Required]
        public string Lengthvt { get; set; }
        [Required]
        public string Widthvt { get; set; }
        [Required]
        public string Lengthtp { get; set; }
        [Required]
        public string Widthtp { get; set; }
        [Required]
        public string Sizesw { get; set; }
        [Required]
        public string Sizelw { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Vattu { get; set; }
        [Required]
        public string Giacong { get; set; }
        
        public string Address { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string Image { get; set; }
        
        public string CaptCha { get; set; }
    }
    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }
        //[Required]
        //public string Company { get; set; }
        public string Country { get; set; }
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]

        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string TimeFr { get; set; }
        //public slTime Time { get; set; }
        [Required]
        //public string Subject { get; set; }
        //[Required]
        public string Service { get; set; }
        [Required]
        public string Date { get; set; }
     

        //public string Lengthvt { get; set; }
        //[Required]
        //public string Widthvt { get; set; }
        //[Required]
        //public string Lengthtp { get; set; }
        //[Required]
        //public string Widthtp { get; set; }
        //[Required]
        //public string Sizesw { get; set; }
        //[Required]
        //public string Sizelw { get; set; }
        //[Required]
        //public string Number { get; set; }
        //[Required]
        //public string Vattu { get; set; }
        //[Required]
        //public string Giacong { get; set; }
        //[Required]
        //public string Address { get; set; }
        //[Required]


        //public string Body { get; set; }
        //public string Image { get; set; }
        //[Required]
        //public string CaptCha { get; set; }
    }
    //public enum Time
    //{
    //    Muoi=10:00,
    //    Mot=11:00,
        
       

    //}
    public class ContactProductModel
    {

        public string Name { get; set; }
        public string Company { get; set; }
        //public string Sex { get; set; }
        //public string Country { get; set; }
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        public string Title { get; set; }

        public string Phone { get; set; }

        //public string Fax { get; set; }

        //public string Lengthvt { get; set; }
        //public string Widthvt { get; set; }
        //public string Lengthtp { get; set; }
        //public string Widthtp { get; set; }
        //public string Sizesw { get; set; }
        //public string Sizelw { get; set; }
        //public string Number { get; set; }
        //public string vattu { get; set; }
        //public string Giacong { get; set; }

        public string Address { get; set; }

        //public string Subject { get; set; }

        public string Body { get; set; }
        public string Image { get; set; }

        public string CaptCha { get; set; }
    }

}