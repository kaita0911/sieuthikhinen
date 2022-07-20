using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web
{
    public class OrderModel
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Quantity { get; set; }
        public string Howtobuy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Company { get; set; }
        [Required]
        public string Address { get; set; }
        public string Body { get; set; }
        public string CaptCha { get; set; }
    }
}