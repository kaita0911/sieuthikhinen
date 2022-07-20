using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using EntityFramework.Extensions;
using Newtonsoft.Json.Linq;
using System.IO;

namespace VCMS.MVC4.Web.Areas.Admin.Controllers
{
    public class WidgetController : VCMSAdminController
    {
        public ActionResult Index()
        {
            using (DataContext db = new DataContext())
            {
                var group = db.WidgetGroup.Include(a => a.Widgets).ToList();
                ViewBag.Widgets = db.Widget.Include(a => a.WidgetDetail).Include(a => a.WidgetGroup).Where(a => a.WidgetGroupId == null).OrderBy(o => o.SortOrder).ToList();
                ViewBag.WidgetsCustomer = db.Widget.Include(a => a.WidgetDetail).Include(a => a.WidgetGroup).Where(a => (((int)a.Flag & (int)WidgetFlag.Custommer) > 0)).OrderBy(o => o.SortOrder).ToList();
                return View(group);
            }
        }

        public ActionResult Create()
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var _group = db.WidgetGroup.ToList();
                var type = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Where(a => (((int)a.Flag & (int)ArticleTypeFlags.WIDGET) > 0)).ToList();
                ViewBag.WidgetType =
                    from WidgetType w in Enum.GetValues(typeof(WidgetType))
                    select new SelectListItem
                    {
                        Text = html.Locale(string.Format("widget_{0}", w.ToString().ToLower())).ToHtmlString(),
                        Value = w.ToString()
                    };
                ViewBag.ArticleType =
                    from t in type
                    select new SelectListItem
                    {
                        Text = t.ArticleTypeDetail[SiteConfig.LanguageId].Name,
                        Value = t.Id.ToString()
                    };
                ViewBag.Group =
                    from g in _group
                    select new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    };
                ViewBag.SortOrder =
                    from ArticleSortOrder w in Enum.GetValues(typeof(ArticleSortOrder))
                    select new SelectListItem
                    {
                        Text = html.Locale(string.Format("widget_{0}", w.ToString().ToLower())).ToHtmlString(),
                        Value = w.ToString()
                    };
                ViewBag.sortDirection =
                    from VCMS.MVC4.Data.SortDirection w in Enum.GetValues(typeof(VCMS.MVC4.Data.SortDirection))
                    select new SelectListItem
                    {
                        Text = html.Locale(string.Format("widget_{0}", w.ToString().ToLower())).ToHtmlString(),
                        Value = w.ToString()
                    };

                Widget model = new Widget()
                {
                    WidgetDetail = VList<WidgetDetail>.Create(SiteConfig.Languages)
                };
                return View(model);
            }
        }
        public ActionResult CreatePlugin()
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.ArticleType = db.ArticleTypes.Include(a => a.ArticleTypeDetail).ToList();
                var _group = db.WidgetGroup.ToList();
                ViewBag.Group =
                    from g in _group
                    select new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    };
                Widget model = new Widget()
                {
                    WidgetType = WidgetType.WEBPLUGIN,
                    WidgetDetail = VList<WidgetDetail>.Create(SiteConfig.Languages)
                };
                return View(model);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var model = new Widget();
                TryUpdateModel(model);
                using (DataContext db = new DataContext())
                {
                    var details = from d in SiteConfig.Languages
                                  select new WidgetDetail()
                                  {
                                      LanguageId = d.Id,
                                      Value = Request[string.Format("WidgetDetail[{0}].Value", d.Id)],
                                      Title = Request[string.Format("WidgetDetail[{0}].Title", d.Id)],
                                  };
                    model.WidgetDetail = new VList<WidgetDetail>();
                    model.WidgetDetail.AddRange(details);
                    db.Widget.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return Json(new { Status = 0, Message = "OK" });
        }

        public ActionResult Edit(int id)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());
                var _group = db.WidgetGroup.ToList();
                var type = db.ArticleTypes.Include(a => a.ArticleTypeDetail).Where(a => (((int)a.Flag & (int)ArticleTypeFlags.WIDGET) > 0)).ToList();
                var model = db.Widget.Include(a => a.WidgetDetail).FirstOrDefault(a => a.Id == id);
                if (model == null) return HttpNotFound();
                ViewBag.Widget = model;
                ViewBag.WidgetType =
                    from WidgetType w in Enum.GetValues(typeof(WidgetType))
                    select new SelectListItem
                    {
                        Text = html.Locale(string.Format("widget_{0}", w.ToString().ToLower())).ToHtmlString(),
                        Value = w.ToString()
                    };
                ViewBag.ArticleType =
                    from t in type
                    select new SelectListItem
                    {
                        Text = t.ArticleTypeDetail[SiteConfig.LanguageId].Name,
                        Value = t.Id.ToString()
                    };
                ViewBag.Group =
                    from g in _group
                    select new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    };
                ViewBag.SortOrder =
                    from ArticleSortOrder w in Enum.GetValues(typeof(ArticleSortOrder))
                    select new SelectListItem
                    {
                        Text = html.Locale(string.Format("widget_{0}", w.ToString().ToLower())).ToHtmlString(),
                        Value = w.ToString(),
                        Selected = (w == model.SortOrder)
                    };
                ViewBag.sortDirection =
                    from VCMS.MVC4.Data.SortDirection w in Enum.GetValues(typeof(VCMS.MVC4.Data.SortDirection))
                    select new SelectListItem
                    {
                        Text = html.Locale(string.Format("widget_{0}", w.ToString().ToLower())).ToHtmlString(),
                        Value = w.ToString(),
                        Selected = (w == model.SortDirection)
                    };
                return View(model);
            }
        }
        public ActionResult EditPlugin(int id)
        {
            using (DataContext db = new DataContext())
            {
                ViewBag.ArticleType = db.ArticleTypes.Include(a => a.ArticleTypeDetail).ToList();
                var model = db.Widget.Include(a => a.WidgetDetail).SingleOrDefault(a => a.Id == id);
                if (model == null) return HttpNotFound();
                var _group = db.WidgetGroup.ToList();
                ViewBag.Group =
                    from g in _group
                    select new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    };
                return View(model);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var model = db.Widget.Include(a => a.WidgetDetail).FirstOrDefault(a => a.Id == id);
                TryUpdateModel(model);

                var details = from d in SiteConfig.Languages
                              select new WidgetDetail()
                              {
                                  LanguageId = d.Id,
                                  Value = Request[string.Format("WidgetDetail[{0}].Value", d.Id)],
                                  Title = Request[string.Format("WidgetDetail[{0}].Title", d.Id)],
                              };
                model.WidgetDetail = new VList<WidgetDetail>();
                model.WidgetDetail.AddRange(details);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.Widget.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }

        [HttpPost]
        public ActionResult UpdateAttributes(string json)
        {
            using (DataContext db = new DataContext())
            {
                JObject jsonData = JObject.Parse("{'items':" + json + "}");
                var widget = (from d in jsonData["items"]
                              select new Widget { Id = d["id"].Value<int>(), IsCustommer = d["isCustomer"].Value<bool>() }
                           ).ToList();
                var ids = widget.Select(a => a.Id).ToList();
                var list = db.Widget.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(a =>
                {
                    var item = widget.FirstOrDefault(d => d.Id == a.Id);
                    if (item != null)
                    {
                        a.IsCustommer = item.IsCustommer;
                    }
                });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        [HttpPost]
        public ActionResult UpdateOrder(string id, string order)
        {
            using (DataContext db = new DataContext())
            {
                var ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var orders = order.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList();
                var list = db.Widget.Where(c => ids.Contains(c.Id)).ToList();
                list.ForEach(c => { c.WidgetsortOrder = orders[ids.IndexOf(c.Id)]; });
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        public ActionResult Attribute(int id, int wid = 0)
        {
            using (DataContext db = new DataContext())
            {
                var html = new HtmlHelper(new ViewContext(ControllerContext, new WebFormView(ControllerContext, "empty"), new ViewDataDictionary(), new TempDataDictionary(), new System.IO.StringWriter()), new ViewPage());

                var model = db.ArticleTypes.FirstOrDefault(a => a.Id == id);
                if (model == null)
                    return HttpNotFound();
                var Widget = db.Widget.Include(a => a.WidgetDetail).Where(a => a.Id == wid).FirstOrDefault();

                Dictionary<string, string> attr = new Dictionary<string, string>();
                attr.Add(html.Locale("widget_isactive").ToHtmlString(), ArticleFlags.ACTIVE.ToString());
                if (model.IsNew)
                    attr.Add(!string.IsNullOrWhiteSpace(model.StrIsNew) ? model.StrIsNew : html.Locale("widget_isnew").ToHtmlString(), ArticleFlags.NEW.ToString());
                if (model.IsHot)
                    attr.Add(!string.IsNullOrWhiteSpace(model.StrIsHot) ? model.StrIsHot : html.Locale("widget_ishot").ToHtmlString(), ArticleFlags.HOT.ToString());
                if (model.IsMostView)
                    attr.Add(!string.IsNullOrWhiteSpace(model.StrIsMostView) ? model.StrIsMostView : html.Locale("widget_mostview").ToHtmlString(), ArticleFlags.MOSTVIEW.ToString());

                ViewBag.Attribute =
                    from item in attr
                    select new SelectListItem
                    {
                        Text = item.Key,
                        Value = item.Value,
                        Selected = (item.Value == (Widget != null ? Widget.Attribute.ToString() : ArticleFlags.ACTIVE.ToString()))
                    };

                return View(model);
            }
        }
        public ActionResult View(int id, int wid = 0)
        {
            using (DataContext db = new DataContext())
            {
                var type = db.ArticleTypes.FirstOrDefault(a => a.Id == id);
                if (type == null)
                    return HttpNotFound();

                var widget = db.Widget.Include(a => a.WidgetDetail).FirstOrDefault(a => a.Id == wid);
                if (widget == null)
                    widget = new Widget();
                ViewBag.Widget = widget;

                var _listViewByType = ViewByType(type.Code);
                ViewBag.ViewByType = _listViewByType;

                return View(type);
            }
        }
        #region Group Widget
        public ActionResult Group()
        {
            using (DataContext db = new DataContext())
            {
                var group = db.WidgetGroup.ToList();
                return View(group);
            }
        }
        public ActionResult CreateGroup()
        {
            using (DataContext db = new DataContext())
            {
                var model = new WidgetGroup();
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult CreateGroup(WidgetGroup model)
        {
            using (DataContext db = new DataContext())
            {
                try
                {
                    db.WidgetGroup.Add(model);
                    db.SaveChanges();
                    return Json(new { data = 1 });
                }
                catch
                {
                    return Json(new { data = -1 });
                }
            }
        }
        public ActionResult EditGroup(int id)
        {
            using (DataContext db = new DataContext())
            {
                var group = db.WidgetGroup.FirstOrDefault(a => a.Id == id);
                if (group == null)
                    return HttpNotFound();
                return View(group);
            }
        }
        [HttpPost]
        public ActionResult EditGroup(int id, FormCollection form)
        {
            using (DataContext db = new DataContext())
            {
                var group = db.WidgetGroup.FirstOrDefault(a => a.Id == id);
                TryUpdateModel(group);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteGroup(string id)
        {
            using (DataContext db = new DataContext())
            {
                int[] ids = id.Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray();

                db.WidgetGroup.Delete(af => ids.Contains(af.Id));
                db.SaveChanges();
                return Json(new { Status = 0, Message = "OK" });
            }
        }
        #endregion

        #region Widget View By ArticleType
        public static List<DirectoryInfo> ViewByType(string code)
        {
            if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Templates/" + SiteConfig.SiteInfo.Code + "/Views/Widget/" + code)))
            {
                List<string> views = System.IO.Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/Templates/" + SiteConfig.SiteInfo.Code + "/Views/Widget/" + code)).ToList<string>();
                List<DirectoryInfo> dir_views = new List<DirectoryInfo>();
                if (views.Count > 0)
                {
                    views.ForEach(t =>
                    {
                        var viewname = Path.GetFileName(t);
                        if (!viewname.Contains("Tablet") && !viewname.Contains("Mobile"))
                            dir_views.Add(new DirectoryInfo(viewname.Split('.').FirstOrDefault()));
                    });
                    return dir_views;
                }
            }
            return new List<DirectoryInfo>();
        }
        #endregion
    }
}