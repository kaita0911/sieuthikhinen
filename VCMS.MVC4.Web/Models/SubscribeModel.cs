using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web
{
    public class SubscribeModel
    {
        [Required]
        public string Email { get; set; }
        public bool Unsubscribe { get; set; }
    }
}