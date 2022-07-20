using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Xml.Linq;
using VCMS.MVC4.Extensions;
using System.Linq;

namespace VCMS.MVC4.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterJsBundles(bundles);
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/keyword/jquery.keyword*",
            //            "~/Scripts/jquery.pager*",
            //            "~/Scripts/vns.vcms*",
            //            "~/Scripts/latinize.js",
            //            "~/Scripts/jquery.lazy.js",
            //             "~/Scripts/jquery.hoverIntent.*",
            //             "~/Scripts/bootstrap.*",
            //             "~/Scripts/jquery.circle.all",
            //             "~/Scripts/jquery.form.min.js"
            //             ));

            //bundles.Add(new ScriptBundle("~/bundles/admin").Include(
            //             "~/Scripts/admin.min.js"
            //             ));
            //bundles.Add(new ScriptBundle("~/bundles/scrollable").Include(
            //            "~/Scripts/jquery.carouFredSel-{version}.js",
            //             "~/Scripts/jquery.corner.*"));
            //bundles.Add(new ScriptBundle("~/bundles/circle").Include(
            //            "~/Scripts/jquery.circle*"));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/editor").Include(
            //    //"~/Scripts/RedActor/redactor.js",
            //    //"~/Scripts/RedActor/plugins/imagemanager/imagemanager.js",
            //    //"~/Scripts/RedActor/plugins/fontcolor/fontcolor.js",
            //    //"~/Scripts/RedActor/plugins/fontfamily/fontfamily.js",
            //    //"~/Scripts/RedActor/plugins/fontsize/fontsize.js",
            //    //"~/Scripts/RedActor/plugins/table/table.js",
            //    //"~/Scripts/RedActor/plugins/fullscreen/fullscreen.js",
            //             "~/Scripts/Froala/froala_editor.min.js",
            //             "~/Scripts/Froala/plugins/block_styles.min.js",
            //             "~/Scripts/Froala/plugins/colors.min.js",
            //             "~/Scripts/Froala/plugins/font_family.min.js",
            //             "~/Scripts/Froala/plugins/font_size.min.js",
            //             "~/Scripts/Froala/plugins/lists.min.js",
            //             "~/Scripts/Froala/plugins/media_manager.min.js",
            //             "~/Scripts/Froala/plugins/tables.min.js",
            //             "~/Scripts/vns.editor.js"));

            //bundles.Add(new ScriptBundle("~/bundles/dcaccordion").Include(
            //"~/Scripts/jquery.cookie.*",
            //"~/Scripts/jquery.dcjqaccordion.{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/vns.validator*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick olny the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/*.css"));

            /// loop thru content folder and register bundles for each folder
            /// 
            var folders = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/Templates"));
            foreach (var folder in folders)
            {
                var f = new DirectoryInfo(folder).Name;
                if (Directory.Exists(Path.Combine(folder, "Content")))
                    bundles.Add(new LessMinify.LessBundle("~/templates/" + f + "/Content/Less").Include("~/Templates/" + f + "/Content/App.less"));
            }
            foreach (var folder in folders)
            {
                var f = new DirectoryInfo(folder).Name;
                if (Directory.Exists(Path.Combine(folder, "Content")))
                {
                    string directory = HttpContext.Current.Server.MapPath("~/templates/" + f + "/Content");
                    foreach (string fileName in Directory.GetFiles(directory))
                    {
                        var n = new DirectoryInfo(Path.Combine(fileName)).Name;
                        bundles.Add(new LessMinify.LessBundle("~/templates/" + f + "/Content/" + n.Replace(".less", "")).Include("~/Templates/" + f + "/Content/" + n));
                    }
                }
            }
            foreach (var folder in folders)
            {
                var f = new DirectoryInfo(folder).Name;
                if (Directory.Exists(Path.Combine(folder, "Content")))
                    bundles.Add(new ImageRelativeStyleBundle("~/template/" + f).Include("~/Templates/" + f + "/Content/*.css"));
            }

            //bundles.Add(new ScriptBundle("~/bundles/Pager").Include(
            //    //"~/Content/Plugin/jquery-2.1.1.min.js",
            //    //"~/Content/Plugin/bootstrap.min.js",
            //    "~/Content/Plugin/owl.carousel.min.js",
            //    "~/Content/Plugin/jquery.magnific-popup.min.js",
            //    "~/Content/Plugin/jquery.elevatezoom.js",
            //    "~/Content/Plugin/Script.js",
            //    "~/Content/Plugin/Custom.js",
            //    "~/Scripts/latinize.js",
            //    "~/Content/Plugin/LiveDateTime.js"

            //             ));

            //bundles.Add(new ScriptBundle("~/bundles/revolutionSlider").Include(
            //    "~/Content/Plugin/jquery.themepunch.tools.min.js",
            //    "~/Content/Plugin/jquery.themepunch.revolution.min.js",
            //    "~/Content/Plugin/revolution_slider.js"
            //             ));

            //bundles.Add(new ScriptBundle("~/bundles/Fixie").Include(
            //   "~/Content/Plugin/html5.js",
            //   "~/Content/Plugin/respond.min.js"
            //   ));
            //bundles.Add(new ScriptBundle("~/bundles/Isotope").Include(
            //   "~/Content/Plugin/jquery.isotope.js",
            //   "~/Content/Plugin/masonry.js"
            //   ));
            //bundles.Add(new ScriptBundle("~/bundles/CheckOut").Include(
            //   "~/Content/Plugin/CheckOut.js",
            //   "~/Content/Plugin/datepicker.js"
            //   ));
            //bundles.Add(new ScriptBundle("~/bundles/Slider").Include(
            //   "~/Content/Plugin/jquery.easing.1.3.js",
            //   "~/Content/Plugin/camera.js",
            //   "~/Content/Plugin/jquery.themepunch.tools.min.js",
            //    "~/Content/Plugin/jquery.themepunch.revolution.min.js"
            //   ));

            bundles.Add(new ImageRelativeStyleBundle("~/Content/admincss").Include("~/Content/Admin/*.css"));

            bundles.Add(new ImageRelativeStyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }

        private static void RegisterJsBundles(BundleCollection bundles)
        {
            XElement root = XElement.Load(HttpContext.Current.Server.MapPath("~/bundles.config"));
            var scripts = from l in root.Elements()
                          where l.Attribute("type").Value.Equals("js", System.StringComparison.OrdinalIgnoreCase) && bool.Parse(l.Attribute("active").Value)
                          select l;
            foreach (var item in scripts)
            {
                var name = item.Attribute("id").Value;
                var bundle = new ScriptBundle("~/bundles/"+name);
                var files = item.Elements("file").Select(l => l.Attribute("path").Value).ToList();
                foreach (var f in files)
                {
                    bundle.Include(f);
                }
                bundles.Add(bundle);
            }
        }
    }
}