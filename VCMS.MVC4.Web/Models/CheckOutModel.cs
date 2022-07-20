using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VCMS.MVC4.Data;
namespace VCMS.MVC4.Web
{
    public class CheckOutModel
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email không được để trống"), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Điện thoại không được để trống")]
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Company { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public bool HasPayment { get; set; }
        public decimal AmountShipping { get; set; }
        public string PostalCode { get; set; }
        public string Notes { get; set; }
        public DateTime DateDelivery { get; set; }
        public int ShippingTime { get; set; }
        public Cart Cart { get; set; }
        public Order Order { get; set; }
        public int CustomerId { get; set; }
    }
    public class CheckOutNoLoginModel
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "Email không được để trống"), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Điện thoại không được để trống")]
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        [Required(ErrorMessage = "Tên công ty không được để trống")]
        public string Company { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public int Ward { get; set; }
        public bool HasPayment { get; set; }
        public decimal AmountShipping { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
        public DateTime DateDelivery { get; set; }
        public int ShippingTime { get; set; }
        public Cart Cart { get; set; }
        public Order Order { get; set; }
        public int CustomerId { get; set; }
        public int pt { get; set; }
        public decimal ship { get; set; }
    }
}