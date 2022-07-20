using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.ComponentModel;

namespace VCMS.MVC4.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="source">image file name</param>
        /// <param name="path">relative folder path</param>
        /// <param name="options"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper helper, string path, ImageOptions options = null, object htmlAttributes = null)
        {
            string img = "~/Content/images/lazy.jpg";
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "~/Content/images/no-images.jpg";
                options.Mode = ImageMode.pad;
            }
            TagBuilder tag = new TagBuilder("img");
            if (options == null) options = new ImageOptions();
            var src = UrlHelper.GenerateContentUrl(path.ToLower(), helper.ViewContext.HttpContext);
            var physical = helper.ViewContext.HttpContext.Server.MapPath(src);
            if (!File.Exists(physical))
            {
                if (options.SearchParentIfNotExists)
                {
                    var parentFolder = Directory.GetParent(physical).Name.ToLower();
                    var root = helper.ViewContext.HttpContext.Server.MapPath("~");
                    parentFolder = string.Format("~{0}", parentFolder.Replace(root, "").Replace("\\", "/"));
                    var filename = Path.GetFileName(physical).ToLower();
                    src = Path.Combine(parentFolder, filename);
                    physical = helper.ViewContext.HttpContext.Server.MapPath(src);
                }

                if (!File.Exists(physical))
                    src = UrlHelper.GenerateContentUrl(options.NotFoundPath, helper.ViewContext.HttpContext);
            }

            if (options.Width > 0)
            {
                src += src.Contains("?") ? string.Format("&width={0}", options.Width) : string.Format("?width={0}", options.Width);
                img += img.Contains("?") ? string.Format("&width={0}", options.Width) : string.Format("?width={0}", options.Width);
            }
            if (options.Height > 0)
            {
                src += src.Contains("?") ? string.Format("&height={0}", options.Height) : string.Format("?height={0}", options.Height);
                img += img.Contains("?") ? string.Format("&height={0}", options.Height) : string.Format("?height={0}", options.Height);

            }
            if (options.Width > 0 || options.Height > 0)
                src += src.Contains("?") ? string.Format("&mode={0}", options.Mode.ToString()) : string.Format("?mode={0}", options.Mode.ToString());


            //src += src.Contains("?") ? string.Format("&quality={0}", options.Quality) : string.Format("?quality={0}", options.Quality);


            if (!string.IsNullOrWhiteSpace(options.Watermark))
                src += src.Contains("?") ? string.Format("&watermark={0}", options.Watermark) : string.Format("?watermark=text&name={0}", options.Watermark);
            if (options.Lazy)
            {
                tag.Attributes["data-src"] = src;
                tag.Attributes["src"] = UrlHelper.GenerateContentUrl(img, helper.ViewContext.HttpContext);
            }
            else
                tag.Attributes["src"] = src;
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            if (options.Lazy)
                tag.AddCssClass("lazy");

            return new MvcHtmlString(tag.ToString().Replace("></img>", "/>"));
        }
        public static MvcHtmlString Website(this HtmlHelper helper, string urlSource, object htmlAttributes = null)
        {
            var urlList = Regex.Split(urlSource, @"[\s;,]+");
            var html = new StringBuilder();
            var url = string.Empty;
            foreach (var item in urlList)
            {
                var tag = new TagBuilder("a");
                if (!Regex.IsMatch(item, @"^https?://"))
                    url = "http://" + item.Trim();
                else
                    url = item.Trim();
                tag.Attributes.Add("href", url);
                tag.Attributes.Add("target", "_blank");
                tag.InnerHtml = item;
                if (htmlAttributes != null)
                    tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                html.Append(tag.ToString() + " ");

            }
            return new MvcHtmlString(html.ToString());
        }
        public static MvcHtmlString Email(this HtmlHelper helper, string emailSource, object htmlAttributes = null)
        {
            var emailList = Regex.Split(emailSource, @"[\s;,]+");
            var html = new StringBuilder();

            foreach (var item in emailList)
            {
                var tag = new TagBuilder("a");

                tag.Attributes.Add("href", "mailto:" + item);

                tag.InnerHtml = item;
                if (htmlAttributes != null)
                    tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                html.Append(tag.ToString() + " ");
            }
            return new MvcHtmlString(html.ToString());
        }
        public static MvcHtmlString Phone(this HtmlHelper helper, string phone, object htmlAttributes = null)
        {
            var html = new TagBuilder("a");
            html.Attributes.Add("href", "tel:" + phone);
            html.InnerHtml = phone;
            return new MvcHtmlString(html.ToString());
        }
        public static IHtmlString MetaKeywords(this HtmlHelper helper, string keywords)
        {
            var html = new TagBuilder("meta");
            html.Attributes.Add("name", "keywords");
            html.Attributes.Add("content", keywords);
            return new HtmlString(html.ToString());
        }
        public static string Support(this HtmlHelper helper, string account, string size, SupportType type, object htmlAttributes = null)
        {
            var html = new TagBuilder("a");
            string jsonString = "";
            var context = new HttpContextWrapper(HttpContext.Current);
            string src = UrlHelper.GenerateContentUrl("~/Content/Images/supports/", context);
            bool isonline = false;
            if (type == SupportType.SKYPE)
            {
                try
                {
                    jsonString = new WebClient().DownloadString("http://mystatus.skype.com/" + account + ".txt");
                }
                catch { }
                isonline = jsonString.ToUpper().Contains("ONLINE");
                src += "skype/";
            }
            else if (type == SupportType.YAHOO)
            {
                try
                {
                    jsonString = new WebClient().DownloadString("http://mail.opi.yahoo.com/online?u=" + account + "&m=t");
                }
                catch { }
                isonline = !jsonString.ToUpper().Contains("NOT ONLINE");
                src += "yahoo/";
            }

            src += size + "/" + (isonline ? "online" : "offline") + ".gif";
            return src;
        }
        public static MvcHtmlString Filter(this HtmlHelper helper, string nameFilter, string actionName, string controllerName, object routeValues, object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("a");
            var url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            RouteInfo info = new RouteInfo(new Uri(url), HttpContext.Current.Request.ApplicationPath);

            if (string.IsNullOrEmpty(actionName))
                actionName = info.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
                controllerName = info.RouteData.Values["controller"].ToString();

            RouteValueDictionary routes = new RouteValueDictionary(routeValues);

            var query = helper.ViewContext.RequestContext.HttpContext.Request.QueryString;
            if (query != null)
            {
                List<string> update = new List<string>() { "pageIndex" };
                List<string> skip = new List<string>() { "sortorder", "sortdirection", "mode" };
                foreach (string item in query.Keys)
                {
                    if (!skip.Contains(item))
                        routes[item] = query[item];
                    if (update.Contains(item))
                        routes[item] = "1";
                }
            }

            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            builder.Attributes.Add("href", Url.Action(actionName, controllerName, routes).ToString());
            builder.InnerHtml = nameFilter;
            if (htmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString Filter(this HtmlHelper html, string nameFilter, object routeValues, object htmlAttributes)
        {
            return Filter(html, nameFilter, null, null, routeValues, htmlAttributes);
        }
        public static MvcHtmlString FilterMode(this HtmlHelper helper, string nameFilter, string actionName, string controllerName, object routeValues, object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("a");
            var url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            RouteInfo info = new RouteInfo(new Uri(url), HttpContext.Current.Request.ApplicationPath);

            if (string.IsNullOrEmpty(actionName))
                actionName = info.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
                controllerName = info.RouteData.Values["controller"].ToString();

            RouteValueDictionary routes = new RouteValueDictionary(routeValues);

            var query = helper.ViewContext.RequestContext.HttpContext.Request.QueryString;
            if (query != null)
            {
                List<string> update = new List<string>() { "pageIndex" };
                List<string> skip = new List<string>() { "mode", "flag" };
                foreach (string item in query.Keys)
                {
                    if (!skip.Contains(item))
                        routes[item] = query[item];
                    if (update.Contains(item))
                        routes[item] = "1";
                }
            }

            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            builder.Attributes.Add("href", Url.Action(actionName, controllerName, routes).ToString());
            builder.InnerHtml = nameFilter;
            if (htmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString FilterMode(this HtmlHelper html, string nameFilter, object routeValues, object htmlAttributes)
        {
            return FilterMode(html, nameFilter, null, null, routeValues, htmlAttributes);
        }
        public static MvcHtmlString FilterLinkString(this HtmlHelper helper, string nameFilter, string actionName, string controllerName, object routeValues)
        {
            var url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            RouteInfo info = new RouteInfo(new Uri(url), HttpContext.Current.Request.ApplicationPath);

            if (string.IsNullOrEmpty(actionName))
                actionName = info.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
                controllerName = info.RouteData.Values["controller"].ToString();

            RouteValueDictionary routes = new RouteValueDictionary(routeValues);

            var query = helper.ViewContext.RequestContext.HttpContext.Request.QueryString;
            if (query != null)
            {
                List<string> update = new List<string>() { "pageIndex" };
                List<string> skip = new List<string>() { "sortorder", "sortdirection", "num", "model" };

                foreach (string item in query.Keys)
                {
                    if (!skip.Contains(item))
                        routes[item] = query[item];

                    if (update.Contains(item))
                        routes[item] = "1";
                }
            }

            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string link = Url.Action(actionName, controllerName, routes);
            return new MvcHtmlString(link.ToString());
        }
        public static MvcHtmlString FilterLinkString(this HtmlHelper html, object routeValues)
        {
            return FilterLinkString(html, null, null, null, routeValues);
        }
        public static MvcHtmlString FilterByCategory(this HtmlHelper html, object routeValues)
        {
            return FilterByCategory(html, null, null, null, routeValues);
        }
        public static MvcHtmlString FilterByCategory(this HtmlHelper helper, string nameFilter, string actionName, string controllerName, object routeValues)
        {
            var url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            RouteInfo info = new RouteInfo(new Uri(url), HttpContext.Current.Request.ApplicationPath);

            if (string.IsNullOrEmpty(actionName))
                actionName = info.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
                controllerName = info.RouteData.Values["controller"].ToString();

            RouteValueDictionary routes = new RouteValueDictionary(routeValues);

            var query = helper.ViewContext.RequestContext.HttpContext.Request.QueryString;
            if (query != null)
            {
                List<string> update = new List<string>() { "pageIndex" };
                List<string> skip = new List<string>() { "brands" };

                foreach (string item in query.Keys)
                {
                    var temp = query[item].ToString().Split('.');
                    if (!skip.Contains(item))
                        routes[item] = query[item];
                    else
                    {
                        if (!temp.Contains(routes[item].ToString()))
                            routes[item] = query[item] + "." + routes[item];
                        else if (temp.Contains(routes[item].ToString()) && temp.Length <= 1)
                            routes[item] = null;
                        else
                        {
                            var crr_item = temp.ToList().Where(c => c != routes[item].ToString());
                            routes[item] = string.Join(".", crr_item);
                        }
                    }

                    if (update.Contains(item))
                        routes[item] = "1";
                }
            }

            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string link = Url.Action(actionName, controllerName, routes);
            return new MvcHtmlString(link.ToString());
        }
        public static MvcHtmlString Canonical(this HtmlHelper helper, string defaultDomain)
        {
            var tag = new TagBuilder("link");
            tag.Attributes.Add("rel", "canonical");
            var url = helper.ViewContext.HttpContext.Request.Url;
            //var temp = url.PathAndQuery.Split('?');
            tag.Attributes.Add("href", string.Format("{0}://{1}{2}", url.Scheme, defaultDomain, url.PathAndQuery));
            //tag.Attributes.Add("href", string.Format("//{0}{1}", defaultDomain, temp[0]));

            return new MvcHtmlString(tag.ToString());
        }
        public static MvcHtmlString QRCode(this HtmlHelper htmlHelper, string data, int size = 80, int margin = 4, QRCodeErrorCorrectionLevel errorCorrectionLevel = QRCodeErrorCorrectionLevel.Low, object htmlAttributes = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (size < 1)
                throw new ArgumentOutOfRangeException("size", size, "Must be greater than zero.");
            if (margin < 0)
                throw new ArgumentOutOfRangeException("margin", margin, "Must be greater than or equal to zero.");
            if (!Enum.IsDefined(typeof(QRCodeErrorCorrectionLevel), errorCorrectionLevel))
                throw new InvalidEnumArgumentException("errorCorrectionLevel", (int)errorCorrectionLevel, typeof(QRCodeErrorCorrectionLevel));

            var url = string.Format("https://chart.apis.google.com/chart?cht=qr&chld={2}|{3}&chs={0}x{0}&chl={1}", size, HttpUtility.UrlEncode(data), errorCorrectionLevel.ToString()[0], margin);

            var tag = new TagBuilder("img");
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("src", url);
            tag.Attributes.Add("width", size.ToString());
            tag.Attributes.Add("height", size.ToString());

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }
    }
    public class ImageOptions
    {
        public ImageOptions()
        {
            Lazy = true;
            Width = 0;
            Height = 0;
            SearchParentIfNotExists = false;
            NotFoundPath = "~/Content/images/no-images.jpg";
            Mode = ImageMode.pad;
            Quality = 80;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Lazy { get; set; }
        public ImageMode Mode { get; set; }
        public bool SearchParentIfNotExists { get; set; }
        public string NotFoundPath { get; set; }
        public string Watermark { get; set; }
        public int Quality { get; set; }
    }
    public enum SupportType
    {
        YAHOO = 0,
        SKYPE = 1
    }
    public enum ImageMode
    {
        carve,
        crop,
        max,
        pad,
        stretch
    }
    public enum QRCodeErrorCorrectionLevel
    {
        /// <summary>Recovers from up to 7% erroneous data.</summary>
        Low,
        /// <summary>Recovers from up to 15% erroneous data.</summary>
        Medium,
        /// <summary>Recovers from up to 25% erroneous data.</summary>
        QuiteGood,
        /// <summary>Recovers from up to 30% erroneous data.</summary>
        High
    }
}
