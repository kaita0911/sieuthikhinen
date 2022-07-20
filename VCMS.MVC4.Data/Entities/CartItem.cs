using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace VCMS.MVC4.Data
{
    public class CartItem
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int Sodu { get; set; }
        [NotMapped]
        public decimal Amount
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
        
        public decimal AmountThieu
        {
            get
            {
                return Sodu * UnitPrice;
            }
        }
        public int CartId { get; set; }
    }

    public class Cart : IDisposable
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<CartItem> Items { get; set; }
        public ICollection<CartItemFile> ItemFiles { get; set; }
        public int? UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public bool Anonymous { get; set; }
        public Guid UniqueId { get; set; }
        public int WebsiteId { get; set; }
        public void AddToCart(Article article, int? num, string proName = "")
        {
            var cartItem = db.CartItems.SingleOrDefault(c => c.ArticleId == article.Id && c.CartId == this.Id && c.PropertyName == proName);
            if (cartItem != null)
                cartItem.Quantity += num != null ? (int)num : 1;
            else
            {
                if (cartItem == null && !article.InactivePrice && article.Price > 0)
                {
                    var currency = db.Currencies.FirstOrDefault(cr => cr.IsDefault);
                    if (article.DiscountPrice > 0)
                    {
                        var price = article.DiscountPrice;
                        if (currency.Id != article.CurrencyId)
                            price = currency.Rate * price / article.Rate;

                        cartItem = new CartItem { ArticleId = article.Id, CartId = this.Id, Quantity = num != null ? (int)num : 1, UnitPrice = price, PropertyName = proName };
                    }
                    else
                    {
                        var price = article.Price;
                        if (currency.Id != article.CurrencyId)
                            price = currency.Rate * price / article.Rate;

                        cartItem = new CartItem { ArticleId = article.Id, CartId = this.Id, Quantity = num != null ? (int)num : 1, UnitPrice = price, PropertyName = proName };
                    }
                }
                else
                    cartItem = new CartItem { ArticleId = article.Id, CartId = this.Id, Quantity = num != null ? (int)num : 1, UnitPrice = 0.0M, PropertyName = proName };
                db.CartItems.Add(cartItem);
            }

            db.SaveChanges();
        }
        public void AddFileToCart(string articleFile, int? num)
        {
            var cartItem = db.CartItemFiles.SingleOrDefault(c => c.ArticleFile == articleFile && c.CartId == this.Id);

            if (cartItem == null)
            {
                cartItem = new CartItemFile { ArticleFile = articleFile, CartId = this.Id, Quantity = num != null ? (int)num : 1, UnitPrice = 0.0M };
                db.CartItemFiles.Add(cartItem);
            }
            else
                cartItem.Quantity += num != null ? (int)num : 1;
            db.SaveChanges();
        }
        public static Cart CreateCart(Guid id)
        {
            using (DataContext db = new DataContext())
            {
                var cart = new Cart { UniqueId = Guid.NewGuid(), DateCreated = DateTime.Now, Items = new List<CartItem>(), ItemFiles = null };
                db.Carts.Add(cart);
                db.SaveChanges();
                return cart;
            }
        }
        public static Cart CreateCartFile(Guid id)
        {
            using (DataContext db = new DataContext())
            {
                var cart = new Cart { UniqueId = Guid.NewGuid(), DateCreated = DateTime.Now, ItemFiles = new List<CartItemFile>(), Items = null };
                db.Carts.Add(cart);
                db.SaveChanges();
                return cart;
            }
        }
        public void MergeCart(int userId)
        {
            var currentCart = db.Carts.Include("Items").Where(c => c.UserId == userId).SingleOrDefault();
            if (currentCart == null)
            {
                currentCart = this;
                currentCart.UserId = userId;
            }
            else
            {
                (currentCart.Items as List<CartItem>).AddRange(this.Items);
                db.Carts.Remove(this);
            }
            db.SaveChanges();
        }
        public void EmptyCart()
        {
            this.Items.Clear();
            var cart = db.Carts.FirstOrDefault(c => c.Id == this.Id);
            db.Carts.Remove(cart);
            db.SaveChanges();
        }
        public Order CreateOrder()
        {
            var max = db.Orders.Where(o => o.DateCreated >= DateTime.Today).Max(o => o.OrderNumber);
            if (max == null)
                max = DateTime.Now.ToString("yyMMddss") + "0001";
            else
                max = string.Format("{0}", long.Parse(max) + 1);
            var order = new Order
            {
                OrderNumber = max,
                OrderLines = this.Items.Select(c => new OrderLine { ProductId = c.ArticleId, ProductName = c.Article.ArticleName, Thoigiancho=c.Article.Time, NumberPr=c.Article.Number, Property = c.PropertyName, Qty = c.Quantity, UnitPrice = c.UnitPrice }).ToList(),
                CustomerId = this.UserId,
                WebsiteId = this.WebsiteId,
                TotalAmount = this.Items.Sum(i => i.Amount),
                Ttt = this.Items.Sum(i => i.Amount) * 10/100,
                TotalQty = this.Items.Sum(i => i.Quantity),
                DateCreated = DateTime.Now,
                History = new List<OrderHistory> { new OrderHistory { Status = OrderStatus.PENDING, DateChanged = DateTime.Now } }
            };
            this.EmptyCart();
            return order;
        }

        DataContext db = new DataContext();
        public static Cart GetByUserId(int userId)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.FirstOrDefault(c => c.UserId == userId);
                return cart;
            }
        }

        public static Cart GetById(int cartId)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.Items.Select(i => i.Article.ArticleDetail)).Include(c => c.Items.Select(i => i.Article.PropertyValues.Select(pv => pv.Property))).Include(c => c.Items.Select(i => i.Article.ArticleType.ArticleTypeDetail)).FirstOrDefault(c => c.Id == cartId);
                return cart;
            }
        }

        public static Cart GetByIdFile(int cartId)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.ItemFiles).FirstOrDefault(c => c.Id == cartId);
                return cart;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
