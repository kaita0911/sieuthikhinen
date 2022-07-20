using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Xml;


namespace VCMS.MVC4.Web
{
    public class MvcRazorToPdf
    {
        public byte[] GeneratePdfOutput(ControllerContext context, object model = null, string viewName = null,
            Action<PdfWriter, Document> configureSettings = null)
        {
            if (viewName == null)
            {
                viewName = context.RouteData.GetRequiredString("action");
            }
            context.Controller.ViewData.Model = model;

            byte[] output;
            using (var document = new Document())
            {
                using (var workStream = new MemoryStream())
                {
                    #region No using Css File
                    PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                    writer.CloseStream = false;

                    if (configureSettings != null)
                    {
                        configureSettings(writer, document);
                    }
                    document.Open();

                    using (var reader = new StringReader(RenderRazorView(context, viewName)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);

                        document.Close();
                        output = workStream.ToArray();
                    }
                    #endregion
                    #region Add css file
                    //PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                    //writer.CloseStream = false;

                    //document.Open();
                    //using (var reader = new StringReader(RenderRazorView(context, viewName)))
                    //{
                    //    var htmlContext = new HtmlPipelineContext(null);
                    //    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                    //    var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                    //    cssResolver.AddCssFile(HttpContext.Current.Server.MapPath("~/Content/Admin/Print.css"), true);

                    //    var pipeline = new CssResolverPipeline(cssResolver,
                    //                                           new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));

                    //    var worker = new XMLWorker(pipeline, true);
                    //    var parser = new XMLParser(worker);
                    //    parser.Parse(reader);
                       
                    //    document.Close();

                    //    var buf = new byte[workStream.Position];
                    //    workStream.Position = 0;
                    //    workStream.Read(buf, 0, buf.Length);
                    //    output = buf;
                    //}
                    #endregion
                }
            }
            return output;
        }

        public string RenderRazorView(ControllerContext context, string viewName)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, viewName, null).View;
            var sb = new StringBuilder();


            using (TextWriter tr = new StringWriter(sb))
            {
                var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                    context.Controller.TempData, tr);
                viewEngineResult.Render(viewContext, tr);
            }
            return sb.ToString();
        }
    }
    public class PdfActionResult : ActionResult
    {
        public PdfActionResult(string viewName, object model)
        {
            ViewName = viewName;
            Model = model;
        }

        public PdfActionResult(object model)
        {
            Model = model;
        }

        public PdfActionResult(object model, Action<PdfWriter, Document> configureSettings)
        {
            if (configureSettings == null) throw new ArgumentNullException("configureSettings");
            Model = model;
            ConfigureSettings = configureSettings;
        }

        public PdfActionResult(string viewName, object model, Action<PdfWriter, Document> configureSettings)
        {
            if (configureSettings == null) throw new ArgumentNullException("configureSettings");
            ViewName = viewName;
            Model = model;
            ConfigureSettings = configureSettings;
        }

        public string ViewName { get; set; }
        public object Model { get; set; }
        public Action<PdfWriter, Document> ConfigureSettings { get; set; }

        public string FileDownloadName { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            //IView viewEngineResult;
            //ViewContext viewContext;

            if (ViewName == null)
            {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = Model;


            if (context.HttpContext.Request.QueryString["html"] != null &&
                context.HttpContext.Request.QueryString["html"].ToLower().Equals("true"))
            {
                RenderHtmlOutput(context);
            }
            else
            {
                if (!String.IsNullOrEmpty(FileDownloadName))
                {
                    context.HttpContext.Response.AddHeader("content-disposition",
                        "attachment; filename=" + FileDownloadName);
                }

                new FileContentResult(context.GeneratePdf(Model, ViewName, ConfigureSettings), "application/pdf")
                    .ExecuteResult(context);
            }
        }

        private void RenderHtmlOutput(ControllerContext context)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
            var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                context.Controller.TempData, context.HttpContext.Response.Output);
            viewEngineResult.Render(viewContext, context.HttpContext.Response.Output);
        }
    }
    public static class MvcRazorToPdfExtensions
    {
        public static byte[] GeneratePdf(this ControllerContext context, object model = null, string viewName = null,
            Action<PdfWriter, Document> configureSettings = null)
        {
            return new MvcRazorToPdf().GeneratePdfOutput(context, model, viewName, configureSettings);
        }
    }
}
