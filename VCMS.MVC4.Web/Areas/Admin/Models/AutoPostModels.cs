using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web.Areas.Admin.Models
{
    public class AutoPostModels
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}