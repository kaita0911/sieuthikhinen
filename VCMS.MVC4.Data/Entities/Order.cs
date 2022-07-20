using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace VCMS.MVC4.Data
{
    [Table("Orders")]
    public class Order
    {
        public Order()
        {
            DateCreated = DateTime.Now;
            //History = new List<OrderHistory> { new OrderHistory {OrderId = this.Id, Status = OrderStatus.PENDING, DateChanged = DateTime.Now } };
            Status = OrderStatus.PENDING;
            OrderNumber = DateTime.Now.ToString("yyMMdd") + DateTime.Now.Ticks.ToString().Substring(0, 4);
            this.OrderLines = new List<OrderLine>();
            this.History = new List<OrderHistory>();
            this.AmountShipping = 0;
        }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20), Column(TypeName = "varchar")]
        public string OrderNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDelivery { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateViewOrder { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? DateShipped { get; set; }
        public DateTime? DateSuccess { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public UserProfile Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalQty { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string PhoneS { get; set; }
        public string Nhomkhachhang { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public int Ward { get; set; }
        public string PostalCode { get; set; }   
        public string Country { get; set; }
        public bool HasPayment { get; set; }
        public decimal AmountShipping { get; set; }
        public int AccumulatedPoint { get; set; }
        public int ShippingTime { get; set; }
        public string Notes { get; set; }
        public int WebsiteId { get; set; }
        public decimal Ttt { get; set; }
        //public Website Website { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
        public ICollection<OrderHistory> History { get; set; }

        public static OrderSearchResult GetByStatus(OrderStatus status, int pageIndex, int pageSize)
        {
            using (DataContext db = new DataContext())
            {
                var count = db.Orders.Count(o => (o.Status & status) > 0);
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History).Include(o => o.Customer)
                    .Where(o => (o.Status & status) > 0)
                    .OrderByDescending(o => o.DateCreated)
                    .Skip(skip).Take(pageSize).ToList();
                return new OrderSearchResult { ItemCount = count, Items = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }
        public static OrderSearchResult GetByPhoneNumber(string phoneNumber, int pageIndex, int pageSize)
        {
            using (DataContext db = new DataContext())
            {

                var count = db.Orders.Count(o => o.PhoneS == phoneNumber);
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History).Include(o => o.Customer)
                    .Where(o => o.PhoneS == phoneNumber)
                    .OrderByDescending(o => o.DateCreated)
                    .Skip(skip).Take(pageSize).ToList();
                return new OrderSearchResult { ItemCount = count, Items = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }

        public static OrderSearchResult GetByEmail(string email, int pageIndex, int pageSize)
        {
            using (DataContext db = new DataContext())
            {

                var count = db.Orders.Count(o => o.Email.Trim() == email.Trim());
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History).Include(o => o.Customer)
                    .Where(o => o.Email.Trim() == email.Trim())
                    .OrderByDescending(o => o.DateCreated)
                    .Skip(skip).Take(pageSize).ToList();
                return new OrderSearchResult { ItemCount = count, Items = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }

        public static OrderSearchResult Search(OrderStatus status, int pageIndex, int pageSize, string keyword)
        {
            using (DataContext db = new DataContext())
            {
                var count = db.Orders.Count(o => (o.Status & status) > 0);
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History).Include(o => o.Customer)
                    .Where(o => (o.Status & status) > 0 && (o.FullName.Contains(keyword) || o.OrderNumber.Contains(keyword) || o.Customer.DisplayName.Contains(keyword)))
                    .OrderByDescending(o => o.DateCreated)
                    .Skip(skip).Take(pageSize).ToList();
                return new OrderSearchResult { ItemCount = count, Items = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }

        public static OrderSearchResult GetByDate(DateTime date, int pageIndex, int pageSize)
        {
            using (DataContext db = new DataContext())
            {
                var count = db.Orders.Count(o => date.Year == o.DateSuccess.Value.Year && date.Month == o.DateSuccess.Value.Month && date.Day == o.DateSuccess.Value.Day);
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History).Include(o => o.Customer)
                    .Where(o => date.Year == o.DateSuccess.Value.Year && date.Month == o.DateSuccess.Value.Month && date.Day == o.DateSuccess.Value.Day)
                    .OrderByDescending(o => o.DateCreated)
                    .Skip(skip).Take(pageSize).ToList();
                return new OrderSearchResult { ItemCount = count, Items = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }
        public static int CountByStatus(int siteId, OrderStatus status)
        {
            using (DataContext db = new DataContext())
            {
                var count = db.Orders.Count(o => o.WebsiteId == siteId && (o.Status & status) > 0);
                return count;
            }
        }
        public static OrderSearchResult GetByUser(int userId, OrderStatus status, int pageIndex, int pageSize)
        {
            using (DataContext db = new DataContext())
            {
                var count = db.Orders.Count(o => o.CustomerId == userId && (o.Status & status) > 0);
                var skip = (pageIndex - 1) * pageSize;
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History).Include(o => o.Customer)
                    .Where(o => o.CustomerId == userId && (o.Status & status) > 0)
                    .OrderByDescending(o => o.DateCreated)
                    .Skip(skip).Take(pageSize).ToList();
                return new OrderSearchResult { ItemCount = count, Items = lst, PageIndex = pageIndex, PageSize = pageSize };
            }
        }
        public static Order GetById(int orderId)
        {
            using (DataContext db = new DataContext())
            {
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History)
                    .Where(o => o.Id == orderId);
                return lst.FirstOrDefault();
            }
        }
        public static Order GetByOrderNumber(string orderNumber)
        {
            using (DataContext db = new DataContext())
            {
                var lst = db.Orders.Include(o => o.OrderLines).Include(o => o.History)
                    .Where(o => o.OrderNumber == orderNumber);
                return lst.FirstOrDefault();
            }
        }
        [NotMapped]
        public string NameCity
        {
            get
            {
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
        public string NameWard
        {
            get
            {
                using (DataContext db = new DataContext())
                {

                   var category = db.Categories.Include(a => a.CategoryDetail).FirstOrDefault(a => a.Id == this.Ward);
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
        public string TimeShipping
        {
            get
            {
                using (DataContext db = new DataContext())
                {

                    var category = db.Categories.Include(a => a.CategoryDetail).FirstOrDefault(a => a.Id == this.ShippingTime);
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
    }
    public class OrderSearchResult
    {
        public int ItemCount { get; set; }
        public List<Order> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    [Flags]
    public enum OrderStatus
    {
        PENDING = 1 << 0,
        VIEW = 1 << 1,
        WAITING = 1 << 2,
        PAID = 1 << 3,
        SHIPPED = 1 << 4,
        CANCELLED = 1 << 5,
        SUCCESS = 1 << 6,
        ALL = PENDING | WAITING | PAID | SHIPPED | CANCELLED | SUCCESS | VIEW
    }

    public class OrderHistory
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public UserProfile User { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime DateChanged { get; set; }

        public string Note { get; set; }
    }

    public class OrderLine
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? NumberPr { get; set; }
        public string Thoigiancho { get; set; }
        public string Property { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Amount
        {
            get { return Qty * UnitPrice; }

        }
    }
}
