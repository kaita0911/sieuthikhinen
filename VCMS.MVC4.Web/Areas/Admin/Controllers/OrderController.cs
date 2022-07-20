using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using EntityFramework.Extensions;
using iTextSharp.text;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class OrderController : VCMSAdminController
    {
        //
        // GET: /Admin/Order/

        public ActionResult Index(OrderStatus status = OrderStatus.ALL, int pageIndex = 1, int pageSize = 20)
        {
            if (status == OrderStatus.ALL)
                status = OrderStatus.ALL & ~OrderStatus.CANCELLED;
            var search = Order.GetByStatus(status, pageIndex, pageSize);
            return View(search);
        }
       

        public ActionResult Detail(int id, string returnUrl)
        {
            using (DataContext db = new DataContext())
            {
                var model = Order.GetById(id);
                if (model == null)
                    return HttpNotFound();
                if (model.CustomerId != null)
                    ViewBag.User = db.Users.FirstOrDefault(a => a.UserId == (int)model.CustomerId);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
        }
       
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.Orders.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult Edit(int id, string returnUrl)
        {
            using (DataContext db = new DataContext())
            {
                var model = Order.GetById(id);
                if (model.CustomerId != null)
                    ViewBag.User = db.Users.FirstOrDefault(a => a.UserId == (int)model.CustomerId);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
              
        }

        public ActionResult Create(FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = new Order();
                TryUpdateModel(model);
                return View(model);
                
            }

        }


        public ActionResult DeleteOrder(int id, string returnUrl)
        {
            using (DataContext db = new DataContext())
            {

                db.Orders.Delete(af => af.Id == id);
                db.SaveChanges();
                ViewBag.ReturnUrl = returnUrl;
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                Redirect(Url.Action("Index", "Order"));
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var order = db.Orders.Where(a => a.Id == id).SingleOrDefault();
                if (order != null)
                {
                    var model = new Order();
                    TryUpdateModel(model);
                        order.FullName = model.FullName;
                        order.Address = model.Address;
                        order.Email = model.Email;
                        order.Phone = model.Phone;
                        order.Nhomkhachhang = model.Nhomkhachhang;
                        //order.Notes = model.Notes;
                        //order.Status = model.Status;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }
        public ActionResult Update(int id, OrderStatus status, string returnUrl)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

                var model = db.Orders.FirstOrDefault(a => a.Id == id);
                if (model == null)
                    return Json(new { error = new { warning = html.Locale("order_not_exit") } });
                if (status == OrderStatus.WAITING)
                {

                    if (model.Status == OrderStatus.PAID)
                        model.Status = OrderStatus.SHIPPED;
                    else
                        model.Status = status;
                }
                else if (status == OrderStatus.PAID)
                {
                    if (model.Status == OrderStatus.SHIPPED)
                        model.Status = OrderStatus.SUCCESS;
                    else
                        model.Status = status;
                }
                else
                    model.Status = status;
                if (status == OrderStatus.CANCELLED)
                {
                    foreach (var od in model.OrderLines)
                    {
                        var article = db.Articles.FirstOrDefault(a => a.Id == od.ProductId);
                        if (article != null)
                        {
                            article.Number += od.Qty;
                            db.SaveChanges();
                        }
                    }
                }
                db.SaveChanges();
                ViewBag.ReturnUrl = returnUrl;
                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Json(new { redirect = Url.Action("Detail", "Order", new { id = id, returnUrl = returnUrl }) });
                return Json(new { redirect = Url.Action("Detail", "Order", new { id = id }) });
            }
        }
        public ActionResult Statistical()
        {
            return View();
        }
        public ActionResult StatisticalSearch(int pageIndex = 1, int pageSize = 20)
        {
            DateTime dt;
            if (Request["Date"] != null)
            {
                if (string.IsNullOrWhiteSpace(Request["Date"].ToString()))
                    dt = DateTime.Now;
                else
                    dt = DateTime.Parse(Request["Date"].ToString());
                ViewBag.Date = Request["Date"].ToString();
            }
            else
                dt = DateTime.Now;
            var search = Order.GetByDate(dt, pageIndex, pageSize);
            if (search.ItemCount > 0)
            {
                ViewBag.TotalAmount = search.Items.Sum(a => a.TotalAmount);
                ViewBag.Number = search.Items.Sum(a => a.TotalQty);
                ViewBag.AmountShipping = search.Items.Sum(a => a.AmountShipping);
                ViewBag.Total = search.Items.Sum(a => a.TotalAmount - a.AmountShipping);
            }
            return View("Statistical", search);
        }
        public ActionResult Search(string keyword, int pageIndex = 1, int pageSize = 20)
        {
            OrderStatus status = OrderStatus.ALL & ~OrderStatus.CANCELLED;
            if (Request["status"] != null)
                status = (OrderStatus)Enum.Parse(typeof(OrderStatus), Request["status"]);
            if (status == OrderStatus.ALL)
                status = OrderStatus.ALL & ~OrderStatus.CANCELLED;
            var search = Order.Search(status, pageIndex, pageSize, keyword);
            ViewBag.Count = search.ItemCount;
            return View("Index", search);
        }
     
        public ActionResult Print(int id)
        {
            var model = Order.GetById(id);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }
        public ActionResult PrintPdf(int id)
        {
            var model = Order.GetById(id);
            if (model == null)
                return HttpNotFound();
            return new PdfActionResult(model, (writer, document) =>
            {
                document.NewPage();
            })
            {
                FileDownloadName = "Order-" + model.OrderNumber + ".pdf"
            };
        }

        [HttpPost]
        public ActionResult UpdateNumberOrderLine(string id, string num)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var nums = num.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var list = db.OrderLines.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.Qty = nums[ids.IndexOf(c.Id)]; });
                db.SaveChanges();

                //update order table
                var line = list.FirstOrDefault();
                if(line != null)
                {
                    int totalQ = 0;
                    decimal total = 0;

                    var order = db.Orders.Where(od => od.Id == line.OrderId).FirstOrDefault();
                    var lst = db.OrderLines.Where(ol => ol.OrderId == line.OrderId).ToList();
   
                    lst.ForEach(ol => { total += ol.Qty * ol.UnitPrice; totalQ += ol.Qty; });
                    order.TotalQty = totalQ;
                    order.TotalAmount = total;
                    db.SaveChanges();
                }
                // end update order table

                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult DeleteOrderLine(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();
                var line = db.OrderLines.FirstOrDefault(c => ids.Contains(c.Id));
               
                db.OrderLines.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();

                //update order table   
                if (line != null)
                {
                    int totalQ = 0;
                    decimal total = 0;

                    var order = db.Orders.Where(od => od.Id == line.OrderId).FirstOrDefault();
                    var lst = db.OrderLines.Where(ol => ol.OrderId == line.OrderId).ToList();
                    if(lst.Count > 0)
                    {
                        lst.ForEach(ol => { total += ol.Qty * ol.UnitPrice; totalQ += ol.Qty; });
                        order.TotalQty = totalQ;
                        order.TotalAmount = total;
                    }
                    else
                    {
                        order.TotalQty = 0;
                        order.TotalAmount = 0;
                    }       
                    db.SaveChanges();
                }
                
                // end update order table
                return Json(new { Status = 0, Message = "OK" });
            }
        }
    }
}
