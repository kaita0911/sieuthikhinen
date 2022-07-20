using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;
using EntityFramework.Extensions;
using VCMS.MVC4.Web.Mailers;
using WebMatrix.WebData;
using System.Web.Security;
namespace VCMS.MVC4.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Code.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase));
                ViewBag.Error = 0;
                if (type.HasPrice && type.HasNumberProduct)
                {
                    foreach (var item in SiteConfig.ShoppingCart.Items.ToList())
                    {
                        if (item.Article.Number == 0 || item.Article.IsMostView || item.Quantity > item.Article.Number)
                        {
                            ViewBag.Error = 1;
                            break;
                        }
                    }
                }
                return View(SiteConfig.ShoppingCart);
            }
        }
        public ActionResult IndexAjax()
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Code.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase));
                ViewBag.Error = 0;
                if (type.HasPrice && type.HasNumberProduct)
                {
                    foreach (var item in SiteConfig.ShoppingCart.Items.ToList())
                    {
                        if (item.Article.Number == 0 || item.Article.IsMostView || item.Quantity > item.Article.Number)
                        {
                            ViewBag.Error = 1;
                            break;
                        }
                    }
                }
                return View();
            }
        }
        public ActionResult IndexFile()
        {
            return View(SiteConfig.ShoppingCartFile);
        }
        public ActionResult History(int pageIndex = 1, int pageSize = 20)
        {
            if (User.Identity.IsAuthenticated && System.Web.Security.Roles.IsUserInRole("Users"))
            {
                OrderStatus status = OrderStatus.ALL;
                using (DataContext db = new DataContext())
                {
                    var order = Order.GetByUser(SiteConfig.UserId, status, pageIndex, pageSize);
                    return View(order);
                }
            }
            else
                Response.Redirect(Url.Action("Login", "Account"));
            return View();
        }
        public ActionResult OrderDetail(int id)
        {
            var model = Order.GetById(id);
            return View(model);
        }
        public ActionResult CartList()
        {
            var cart = Cart.GetById(int.Parse(System.Web.HttpContext.Current.Request.Cookies["Cart_id"].Value));
            //return PartialView("_MiniCartAjax", cart);
            return PartialView("Index", cart);
        }

        public ActionResult AddToCart(int id, int? num = 1, string proName = "")
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var article = Article.GetById(id, SiteConfig.LanguageId);
                if (article != null)
                    SiteConfig.ShoppingCart.AddToCart(article, num, proName);

                //return Redirect("/ShoppingCart/IndexAjax");
                return Redirect("/gio-hang.html");
            }
        }

        public ActionResult AddToCartAjax(int id, int? num = 1, string proName = "")
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var article = Article.GetById(id, SiteConfig.LanguageId);
                if (article != null)
                    SiteConfig.ShoppingCart.AddToCart(article, num, proName);
                return Json(new { Status = 0, Message = string.Format(html.Locale("cart_add_ok").ToHtmlString(), article.ArticleName), Qyt = SiteConfig.ShoppingCart.Items.Sum(a => a.Quantity), Amount = html.Amount(SiteConfig.ShoppingCart.Items.Sum(a => a.Amount)).ToString() });
                //return Json(new { Status = 0, Message = "ok" });
            }
        }

        public ActionResult AddFileToCart(string id, int? num = 1)
        {
            using (DataContext db = new DataContext())
            {
                string url = "/UserUpload/Gallery/" + id;
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var articleF = db.ArticleFiles.FirstOrDefault(f => f.FileName == url);
                if (articleF != null)
                    SiteConfig.ShoppingCart.AddFileToCart(id, num);
                return Redirect("/ShoppingCart/IndexFile");
            }
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Code.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase));
                var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCart.Id);

                cart.Items.ToList().ForEach(i =>
                {
                    var qty = int.Parse(form["qty_" + i.Id.ToString()]);
                    if (type.HasPrice && type.HasNumberProduct)
                    {
                        if (qty > i.Article.Number)
                            qty = int.Parse(i.Article.Number.ToString());

                    }
                    i.Quantity = qty;
                });
                cart.Items.Where(i => i.Quantity == 0).ToList().ForEach(i => db.CartItems.Remove(i));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult UpdateAjax(string id, string quantity)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var quantitys = quantity.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var list = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCart.Id).Items.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.Quantity = quantitys[ids.IndexOf(c.Id)]; });
                db.SaveChanges();
                return Json(new { Status = 0, Qty = SiteConfig.ShoppingCart.Items.Sum(a => a.Quantity), Amount = html.Amount(SiteConfig.ShoppingCart.Items.Sum(a => a.Amount)).ToHtmlString() });
            }
        }
        [HttpPost]
        public ActionResult UpdateFile(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.ItemFiles).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCartFile.Id);

                cart.ItemFiles.ToList().ForEach(i =>
                {
                    var qty = int.Parse(form["qty_" + i.Id.ToString()]);
                    i.Quantity = qty;
                });
                cart.ItemFiles.Where(i => i.Quantity == 0).ToList().ForEach(i => db.CartItemFiles.Remove(i));
                db.SaveChanges();
                return RedirectToAction("IndexFile");
            }
        }
        public ActionResult UpdateCartError()
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCart.Id);
                cart.Items.ToList().ForEach(i =>
                {
                    var article = Article.GetById(i.ArticleId, SiteConfig.LanguageId);
                    if (article != null)
                        if (article.Number < i.Quantity)
                            i.Quantity = int.Parse(article.Number.ToString());
                });
                cart.Items.Where(i => i.Article.Number == 0 || i.Article.IsMostView).ToList().ForEach(i => db.CartItems.Remove(i));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult UpdateQuantity(int id, int number)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCart.Id);
                cart.Items.FirstOrDefault(a => a.ArticleId == id).Quantity += number;
                cart.Items.Where(i => i.Quantity == 0).ToList().ForEach(i => db.CartItems.Remove(i));

                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

                var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCart.Id);
                cart.Items.Where(i => i.ArticleId == id).ToList().ForEach(i => db.CartItems.Remove(i));

                db.SaveChanges();

                return Json(new { Status = 0, Qty = SiteConfig.ShoppingCart.Items.Sum(a => a.Quantity), Amount = html.Amount(SiteConfig.ShoppingCart.Items.Sum(a => a.Amount)).ToHtmlString() });
            }
        }
        public ActionResult DeleteItem(int id)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCart.Id);
                cart.Items.Where(i => i.ArticleId == id).ToList().ForEach(i => db.CartItems.Remove(i));

                db.SaveChanges();
                return RedirectToAction("Index", "ShoppingCart");
            }
        }

        public ActionResult DeleteItemFile(int id)
        {
            using (DataContext db = new DataContext())
            {
                var cart = db.Carts.Include(c => c.ItemFiles).FirstOrDefault(c => c.Id == SiteConfig.ShoppingCartFile.Id);
                cart.ItemFiles.Where(i => i.Id == id).ToList().ForEach(i => db.CartItemFiles.Remove(i));

                db.SaveChanges();
                return RedirectToAction("IndexFile", "ShoppingCart");
            }
        }

        public ActionResult EmptyCart()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            var check = 0;
            if (SiteConfig.ShoppingCart.Items.FirstOrDefault().Article.ArticleType.HasNumberProduct)
            {
                foreach (var item in SiteConfig.ShoppingCart.Items.ToList())
                {
                    if (item.Article.Number == 0 || item.Article.IsMostView || item.Quantity > item.Article.Number)
                    {
                        check = 1;
                        break;
                    }
                }
            }
            if (SiteConfig.ShoppingCart.Items.Count > 0 && check == 0)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult CheckOutFile()
        {
            if (SiteConfig.ShoppingCartFile.ItemFiles.Count > 0)
                return View();
            else
                return RedirectToAction("IndexFile");
        }

        private UserMailer _userMailer = new UserMailer();
        public UserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }
        [HttpPost]
        public ActionResult CheckOut(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (SiteConfig.ShoppingCart.Items.Count > 0)
                {
                    int total = Convert.ToInt32(form["total"]);
                    int ship = Convert.ToInt32(form["ship"]);
                    int pt = Convert.ToInt32(form["pt"]);

                    var model = new CheckOutNoLoginModel();
                    TryUpdateModel(model);
                    model.Cart = SiteConfig.ShoppingCart;

                    foreach (var item in SiteConfig.ShoppingCart.Items)
                    {
                        var article = Article.GetById(item.ArticleId, SiteConfig.LanguageId);
                        if (article.PropertyValues.FirstOrDefault(a => a.Property.Code.Equals("PURCHASE", StringComparison.OrdinalIgnoreCase)) != null)
                        {
                            var Aproperty = db.ArticlePropertyValues.Where(a => a.ArticleId == article.Id && a.Property.Code.Equals("PURCHASE", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            Aproperty.Value = string.IsNullOrWhiteSpace(Aproperty.Value) ? "1" : (int.Parse(Aproperty.Value) + 1).ToString();
                            db.SaveChanges();
                        }
                    }

                    var order = SiteConfig.ShoppingCart.CreateOrder();
                    var orrdernumber = order.OrderNumber;
                    //string temp = "";
                    //if (form["City"] != null)
                    //   temp += form["City"].ToString();
                    //////order.City = int.Parse(form["City"]);
                    //if (form["State"] != null)
                    //    temp += form["State"].ToString();
                    //////order.State = int.Parse(form["State"]);
                    //if (form["Wards"] != null)
                    //   temp += form["Wards"].ToString();
                    //////order.Country = form["Wards"];


                    //if (form["City"] != null)
                    //    order.City = int.Parse(form["City"]);
                    //if (form["State"] != null)
                    //    order.State = int.Parse(form["State"]);
                    //if (form["City"] != null)
                    //    order.Country = form["Wards"];
                    //if (form["hd_st"] != null)
                    //    order.State = int.Parse(form["hd_st"]);
                    //if (form["rdo_nn"] != null)
                    //    order.Country  = form["rdo_nn"].ToString();
                    order.AmountShipping  = Convert.ToDecimal(ship);
                    order.Ttt = pt;

                    //NganLuong
                    var pay = "NONE";
                    if (form["rdo_payment"] != null)
                        pay = form["rdo_payment"];

                    if (order.TotalAmount > 0 && (pay == "VISA" || pay == "ATM_ONLINE"))
                    {
                        if (total > 500000)
                        {
                            total = (total + ship) * pt / 100;
                        }

                        string payment_method = pay;
                        string str_bankcode = form["rdo_cart"];

                        RequestInfoNL info = new RequestInfoNL();
                        info.Merchant_id = form["merchant_id"];
                        info.Merchant_password = form["merchant_password"];
                        info.Receiver_email = Server.UrlEncode(form["receiver_email"]);

                        info.cur_code = form["cur_code"];
                        info.bank_code = str_bankcode;

                        info.Order_code = order.OrderNumber;
                        info.Total_amount = total.ToString();
                        info.fee_shipping = "0";
                        info.Discount_amount = "0";
                        info.order_description = "Thanh toan tes thu dong hang";
                        info.return_url = Server.UrlEncode(form["return_url"]);
                        info.cancel_url = Server.UrlEncode(form["cancel_url"]);

                        info.Buyer_fullname = model.FullName;
                        info.Buyer_email = Server.UrlEncode(model.Email);
                        info.Buyer_mobile = model.Phone;

                        APICheckoutV3NL objNLChecout = new APICheckoutV3NL();
                        ResponseInfoNL result = objNLChecout.GetUrlCheckoutNL(info, payment_method);

                        if (result.Error_code == "00")
                            Response.Redirect(result.Checkout_url);
                        else
                            return RedirectToAction("Error", new { str_error = result.Description });
                    }
                    //End NganLuong

                    TryUpdateModel(order);
                    //order.Address = order.Address + temp;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    model.Order = order;
                    //model.pt = pt;
                    model.ship = ship;
                    UserMailer.CreateMessage("CheckOut", SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name + " - " + orrdernumber.ToString(), model, new string[] {SiteConfig.SiteInfo.Email }).Send();
                    return RedirectToAction("Complete");
                }
                else
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult CheckOutFile(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                if (SiteConfig.ShoppingCartFile.ItemFiles.Count > 0)
                {
                    var model = new CheckOutNoLoginModel();
                    TryUpdateModel(model);
                    model.Cart = SiteConfig.ShoppingCartFile;
                    UserMailer.CreateMessage("CheckOutFile", SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name, model, new string[] {SiteConfig.SiteInfo.Email }).Send();
                    var cart = db.Carts.FirstOrDefault(c => c.Id == model.Cart.Id);
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                    return RedirectToAction("Complete");
                }
                else
                    return RedirectToAction("IndexFile");
            }
        }

        public ActionResult Complete(string id)
        {
            var order = Order.GetByOrderNumber(id);
            if (order == null) return View();
            return View(order);
        }
        public ActionResult Error(string str_error)
        {
            ViewBag.Error = str_error;
            return View();
        }
        public ActionResult Redirect()
        {
            return View();
        }
        public ActionResult Confirm()
        {
            return View();
        }
        public ActionResult Confirmed()
        {
            return View();
        }

        public ActionResult CheckLogin()
        {
            if (Request.IsAuthenticated && System.Web.Security.Roles.IsUserInRole("Users"))
                return Json(new { Status = 0, Message = "OK" });
            return Json(new { Status = -1, Message = "OK" });
        }

        public ActionResult CheckRegister(int status)
        {
            if (status == 0)
                return PartialView("_ShopCartRegister");
            return PartialView("_ShopCartGuest");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Customer()
        {
            using (DataContext db = new DataContext())
            {
                var categorytype = db.CategoryTypes.Where(a => a.NoneType && a.Code.Equals("SHIPPING", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (categorytype != null)
                {
                    var category = Category.GetTree(Category.GetByCateType(categorytype.Id, SiteConfig.LanguageId));
                    ViewBag.Category = category;
                }
                return View();
            }

        }

        public ActionResult Note()
        {
            return View();
        }
        public ActionResult ConfimCart()
        {
            return View();
        }
        public ActionResult ConfimCartNoneShipping()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckOutAjax(FormCollection form, CheckOutModel model)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

                var type = db.ArticleTypes.FirstOrDefault(a => a.Code.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase));
                if (type.HasPrice && type.HasNumberProduct)
                {
                    foreach (var item in SiteConfig.ShoppingCart.Items.ToList())
                    {
                        if (item.Article.Number == 0 || item.Article.IsMostView || item.Quantity > item.Article.Number)
                            return Json(new { redirect = Url.Action("Index", "ShoppingCart") });
                    }
                }

                if (SiteConfig.ShoppingCart.Items.Count > 0)
                {
                    TryUpdateModel(model);
                    model.AmountShipping = 0;

                    model.Cart = SiteConfig.ShoppingCart;
                    foreach (var item in SiteConfig.ShoppingCart.Items)
                    {
                        var article = db.Articles.FirstOrDefault(a => a.Id == item.ArticleId);
                        if (article != null)
                        {
                            article.Number -= item.Quantity;
                            article.Number = article.Number < 0 ? article.Number = 0 : article.Number;
                            db.SaveChanges();
                        }
                    }
                    var user = db.Users.FirstOrDefault();

                    var order = SiteConfig.ShoppingCart.CreateOrder();
                    var orrdernumber = order.OrderNumber;

                    TryUpdateModel(order);

                    if (Request.IsAuthenticated && System.Web.Security.Roles.IsUserInRole("Users"))
                    {
                        user = db.Users.FirstOrDefault(a => a.UserId == SiteConfig.CurrentUser.UserId);
                        order.CustomerId = user.UserId;
                        if (form["checkUser"] == "0")
                        {
                            model.FullName = user.DisplayName;
                            model.Address = user.Address;
                            model.Email = user.Email;
                            model.Phone = user.Phone;
                            model.Fax = user.Fax;
                            model.Company = user.Company;
                            model.City = user.City;
                            model.State = user.State;
                            model.PostalCode = user.PostalCode;
                        }
                    }
                    order.HasPayment = model.HasPayment;
                    order.ShippingTime = model.ShippingTime;
                    order.Notes = model.Notes;
                    order.DateDelivery = model.DateDelivery;

                    if (order.TotalAmount < SiteConfig.SiteInfo.AmountShippingToFree)
                    {
                        var category = db.Categories.FirstOrDefault(a => a.Id == model.State);
                        if (category != null)
                        {
                            var shipping = System.Text.RegularExpressions.Regex.Replace(category.Font, "[^0-9]+", "");
                            model.AmountShipping = decimal.Parse(shipping);
                        }
                    }
                    order.AmountShipping = model.AmountShipping;

                    db.Orders.Add(order);
                    db.SaveChanges();
                    model.Order = order;
                    UserMailer.CreateMessage("CheckOut", SiteConfig.SiteInfo.WebsiteDetail[SiteConfig.LanguageId].Name + " - " + orrdernumber.ToString(), model, new string[] {SiteConfig.SiteInfo.Email }).Send();

                    return Json(new { redirect = Url.Action("Complete", "ShoppingCart", new { id = order.OrderNumber }) });
                }
                else
                    return Json(new { error = new { warning = html.Locale("checkout_error_cart_null").ToHtmlString() } });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection form)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

            var array = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(form["UserName"]))
                array.Add("UserName", html.Locale("account_error_username").ToHtmlString());

            if (string.IsNullOrWhiteSpace(form["Password"]))
                array.Add("Password", html.Locale("account_error_pasword").ToHtmlString());

            if (array.Count == 0)
            {
                if (ModelState.IsValid && WebSecurity.Login(form["UserName"], form["Password"], persistCookie: false))
                    return Json(new { redirect = Url.Action("CheckOut", "ShoppingCart") });
                else
                    return Json(new { error = new { warning = html.Locale("account_login_error").ToHtmlString() } });
            }
            else
                return Json(new { error = array });
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Custommer(FormCollection form)
        {
            var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
            var array = new Dictionary<string, string>();

            if (form["checkUser"].Equals("0"))
                return Json(new { Status = 1 });

            if (string.IsNullOrWhiteSpace(form["FullName"]))
                array.Add("FullName", html.Locale("account_error_fullname").ToHtmlString());
            if (string.IsNullOrWhiteSpace(form["Email"]))
                array.Add("Email", html.Locale("account_error_email").ToHtmlString());
            else if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(form["Email"]))
                array.Add("Email", html.Locale("account_error_email_format").ToHtmlString());
            if (string.IsNullOrWhiteSpace(form["Phone"]))
                array.Add("Phone", html.Locale("account_error_phone").ToHtmlString());
            if (string.IsNullOrWhiteSpace(form["Address"]))
                array.Add("Address", html.Locale("account_error_address").ToHtmlString());
            if (array.Count == 0)
                return Json(new { Status = 1 });
            else
                return Json(new { error = array });
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Note(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                DateTime dt;

                bool parsed = DateTime.TryParse((string)form["DateDelivery"], out dt);
                if (!parsed)
                    return Json(new { error = new { warning = html.Locale("checkout_error_date").ToHtmlString() } });
                decimal price = 0;
                int State = int.Parse(form["State"]);
                if (Request.IsAuthenticated && System.Web.Security.Roles.IsUserInRole("Users"))
                {
                    var user = db.Users.FirstOrDefault(a => a.UserId == SiteConfig.CurrentUser.UserId);
                    if (user != null)
                    {
                        if (form["checkUser"].Equals("0", StringComparison.OrdinalIgnoreCase))
                        {
                            var cate = db.Categories.FirstOrDefault(a => a.Id == user.State);
                            if (cate != null)
                                decimal.TryParse(cate.Font.ToString(), out price);
                        }
                        else
                        {
                            var cate = db.Categories.FirstOrDefault(a => a.Id == State);
                            if (cate != null)
                                decimal.TryParse(cate.Font.ToString(), out price);
                        }
                    }
                }
                else
                {
                    var cate = db.Categories.FirstOrDefault(a => a.Id == State);
                    if (cate != null)
                        decimal.TryParse(cate.Font.ToString(), out price);
                }
                var total = SiteConfig.ShoppingCart.Items.Sum(a => a.UnitPrice * a.Quantity) + price;
                return Json(new { done = new { Value = html.Amount(price).ToHtmlString(), Total = html.Amount(total).ToHtmlString() } });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var array = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(form["FullName"]))
                    array.Add("FullName", html.Locale("account_error_fullname").ToHtmlString());
                if (string.IsNullOrEmpty(form["Email"]))
                    array.Add("Email", html.Locale("account_error_email").ToHtmlString());
                else if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(form["Email"]))
                    array.Add("Email", html.Locale("account_error_email_format").ToHtmlString());
                if (string.IsNullOrEmpty(form["Phone"]))
                    array.Add("Phone", html.Locale("account_error_phone").ToHtmlString());
                if (string.IsNullOrEmpty(form["Address"]))
                    array.Add("Address", html.Locale("account_error_address").ToHtmlString());
                if (Request["PasswordRegister"].Length < 6)
                    array.Add("PasswordRegister", html.Locale("account_error_password_min_6").ToHtmlString());
                if (form["PasswordRegister"] != form["ConfirmPassword"])
                    array.Add("ConfirmPassword", html.Locale("account_error_confirmpassword").ToHtmlString());
                if (array.Count == 0)
                {
                    RegisterModel model = new RegisterModel()
                    {
                        Email = form["Email"],
                        FullName = form["FullName"],
                        Address = form["Address"],
                        Phone = form["Phone"],
                        Fax = form["Fax"],
                        Company = form["Company"],
                        City = int.Parse(form["City"]),
                        State = int.Parse(form["State"]),
                        PostalCode = form["PostalCode"],
                        Password = form["PasswordRegister"],
                        Newsletter = false
                    };
                    var user = db.Users.FirstOrDefault(a => a.UserName == model.Email);
                    if (user != null)
                    {
                        if (System.Web.Security.Roles.IsUserInRole(user.UserName, "Guests"))
                        {
                            user.Email = model.Email;
                            user.UserName = model.Email;
                            user.DisplayName = model.FullName;
                            user.Address = model.Address;
                            user.Phone = model.Phone;
                            user.Fax = model.Fax;
                            user.Company = model.Company;
                            user.City = model.City;
                            user.State = model.State;
                            user.PostalCode = model.PostalCode;
                            user.Newsletter = false;

                            WebSecurity.ChangePassword(user.UserName, "guestpasswordpwd4@vns.com", model.Password);

                            string[] roles = { "Users" };
                            string[] list = Roles.GetRolesForUser(user.UserName);
                            if (list.Count() > 0)
                            {
                                Roles.RemoveUserFromRoles(user.UserName, list);
                            }
                            db.SaveChanges();
                            Roles.AddUserToRoles(user.UserName, roles);
                            WebSecurity.Login(model.Email, model.Password);
                            return Json(new { redirect = Url.Action("CheckOut", "ShoppingCart") });
                        }
                        return Json(new { error = new { warning = html.Locale("account_error_email_is_exit").ToHtmlString() } });
                    }

                    else
                    {
                        WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { DateCreated = DateTime.Now, DisplayName = model.FullName, Email = model.Email, model.Address, Phone = model.Phone, Fax = model.Fax, City = model.City, State = model.State, PostalCode = model.PostalCode, Company = model.Company, Newsletter = model.Newsletter });
                        WebSecurity.Login(model.Email, model.Password);

                        string[] roles = { "Users" };
                        Roles.AddUserToRole(model.Email, "Users");
                        return Json(new { redirect = Url.Action("CheckOut", "ShoppingCart") });
                    }
                }
                else
                    return Json(new { error = array });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Guest(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var array = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(form["FullName"]))
                    array.Add("FullName", html.Locale("account_error_fullname").ToHtmlString());
                if (string.IsNullOrEmpty(form["Email"]))
                    array.Add("Email", html.Locale("account_error_email").ToHtmlString());
                else if (!new VNS.Web.Helpers.UtilitiesHelper().IsValidEmailAddress(form["Email"]))
                    array.Add("Email", html.Locale("account_error_email_format").ToHtmlString());
                if (string.IsNullOrEmpty(form["Phone"]))
                    array.Add("Phone", html.Locale("account_error_phone").ToHtmlString());
                if (string.IsNullOrEmpty(form["Address"]))
                    array.Add("Address", html.Locale("account_error_address").ToHtmlString());

                if (array.Count == 0)
                    return Json(new { done = "OK" });
                else
                    return Json(new { error = array });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ReOrder(int id)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var article = db.Articles.Include(a => a.ArticleType.ArticleTypeDetail).Include(a => a.Categories.Select(c => c.CategoryDetail)).Include(a => a.Prices.Select(c => c.Currency)).FirstOrDefault(a => a.Id == id);
                if (article == null)
                    return Json(new { error = new { warning = html.Locale("order_error_product_not_exit").ToHtmlString() } });

                if (article.Number <= 0 || article.InactivePrice)
                    return Json(new { error = new { warning = html.Locale("order_error_number_product").ToHtmlString() } });

                SiteConfig.ShoppingCart.AddToCart(article, 1);
                article.Number -= 1;
                db.SaveChanges();
                return Json(new { done = new { message = html.Locale("order_success_addtocart").ToHtmlString() } });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CancelOrder(int id)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

                var order = db.Orders.Include(a => a.OrderLines).Include(a => a.History).Include(a => a.Customer).FirstOrDefault(a => a.Id == id);
                if (order == null)
                    return Json(new { error = new { warning = html.Locale("order_not_exit").ToHtmlString() } });
                foreach (var od in order.OrderLines)
                {
                    var article = db.Articles.FirstOrDefault(a => a.Id == od.ProductId);
                    if (article != null)
                    {
                        article.Number += od.Qty;
                        db.SaveChanges();
                    }
                }

                order.Status = OrderStatus.CANCELLED;
                db.SaveChanges();
                return Json(new { redirect = Url.Action("History", "ShoppingCart") });

            }
        }
    }
}
