using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing.Text;
using System.Web.Routing;

using System.Linq;

namespace VCMS.MVC4.ImageResize
{
    public class ImageHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }
        protected RequestContext RequestContext { get; set; }

        public ImageHandler() : base() { }

        public ImageHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }
        public void ProcessRequest(HttpContext context)
        {
            string sWidth = context.Request["width"];
            sWidth = sWidth ?? context.Request["w"];
            string sHeight = context.Request["height"];
            sHeight = sHeight ?? context.Request["h"];
            int width = 0, height = 0;
            if (!string.IsNullOrEmpty(sWidth)) int.TryParse(sWidth, out width);
            if (!string.IsNullOrEmpty(sHeight)) int.TryParse(sHeight, out height);

            string noImage = string.Empty;//ConfigurationManager.AppSettings["ImageHandler.DefaultImageVirtualPath"];
            string matchPattern = string.Empty;//ConfigurationManager.AppSettings["ImageHandler.MatchPattern"];

            bool isMatched = false;

            ImageHandlerSettings configs = GetConfig();
            ImageHandlerSettingElement matchedConf = null;
            if (width >= 0)
            {

                foreach (ImageHandlerSettingElement conf in configs.Settings)
                {

                    if (Regex.IsMatch(context.Request.Path, conf.MatchPattern, RegexOptions.IgnoreCase))
                    {
                        isMatched = true;
                        matchedConf = conf;

                        break;
                    }

                }
            }
            string filePath = context.Request.Path;
            string file = context.Request.PhysicalPath;


            context.Response.ContentType = "image/jpeg";

            if (isMatched && width != -1)
            {
                if (!File.Exists(file))
                {
                    file = context.Server.MapPath(noImage);
                    filePath = noImage;
                }

                string cachePath = Path.Combine(HttpRuntime.CodegenDir, context.Server.UrlEncode(width.ToString() + matchedConf.Watermark.Text + filePath));

                if (File.Exists(cachePath))
                {
                    OutputCacheResponse(context, File.GetLastWriteTime(cachePath));
                    context.Response.WriteFile(cachePath);
                }
                else
                {

                    OutputCacheResponse(context, File.GetLastWriteTime(file));
                    DrawImageDirect(context, file, sWidth, sHeight, matchedConf, cachePath);
                }
            }
            else
            {
                OutputCacheResponse(context, File.GetLastWriteTime(file));
                context.Response.WriteFile(file);
            }
            configs = null;
            matchedConf = null;
        }

        private void DrawImageDirect(HttpContext context, string file, string sWidth, string sHeight, ImageHandlerSettingElement conf, string cachePath)
        {
            int width = 0, height = 0;
            if (!string.IsNullOrEmpty(sWidth)) int.TryParse(sWidth, out width);
            if (!string.IsNullOrEmpty(sHeight)) int.TryParse(sHeight, out height);


            int defaultWidth = 0;

            WatermarkSettingElement waterMark = null;


            waterMark = conf.Watermark;
            defaultWidth = conf.DefaultWidth;
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                MemoryStream memStream = new MemoryStream();
                int bytesRead;
                byte[] buffer = new byte[1024];

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }
                
                System.Drawing.Image wImg = System.Drawing.Image.FromStream(memStream);


                if (width == 0 && height == 0)
                {
                    width = defaultWidth > wImg.Width ? wImg.Width : defaultWidth;
                    height = wImg.Height * width / wImg.Width;
                }
                else
                {
                    if (height == 0)
                        height = wImg.Height * width / wImg.Width;

                    else if (width == 0)
                        width = wImg.Width * height / wImg.Height;

                    if (wImg.Width < width)
                        width = wImg.Width;

                    if (width * wImg.Height < height * wImg.Width)
                        height = (int)Math.Ceiling((float)width * wImg.Height / wImg.Width);

                    else if (width * wImg.Height > height * wImg.Width)
                        width = (int)Math.Ceiling((float)height * wImg.Width / wImg.Height);

                }

                System.Drawing.Imaging.FrameDimension dimension = new System.Drawing.Imaging.FrameDimension(wImg.FrameDimensionsList[0]);
                int frameCount = wImg.GetFrameCount(dimension);
                ImageFormat imgFormat = wImg.RawFormat;

                if (frameCount > 1) //animated
                {

                    OutputCacheResponse(context, File.GetLastWriteTime(file));
                    wImg.Save(context.Response.OutputStream, imgFormat);
                    wImg.Dispose();
                    memStream.Dispose();
                    fs.Dispose();
                    return;
                }
                Bitmap dst = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(dst);


                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.Default;
                g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                g.CompositingQuality = CompositingQuality.HighSpeed;

                g.DrawImage(wImg, 0, 0, width, height);


                ImageCodecInfo jpegCodec = GetEncoderInfo(imgFormat);
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;


                if (waterMark != null && !string.IsNullOrEmpty(waterMark.Text))
                {
                    this.WaterMark(g, waterMark);
                }
                using (MemoryStream ms = new MemoryStream())
                using (FileStream diskCacheStream = new FileStream(cachePath, FileMode.CreateNew))
                {

                    dst.Save(ms, jpegCodec, encoderParams);
                    ms.WriteTo(diskCacheStream);
                    OutputCacheResponse(context, File.GetLastWriteTime(file));
                    ms.WriteTo(context.Response.OutputStream);
                }
                encoderParams.Dispose();
                qualityParam.Dispose();

                dst.Dispose();
                g.Dispose();
                wImg.Dispose();
                memStream.Dispose();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/html";
                context.Response.Write(ex.Message + "<br/>");
                context.Response.Write(ex.StackTrace);
            }
        }
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders().ToList().Find(delegate(ImageCodecInfo codec)
            {
                return codec.FormatID == format.Guid;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="waterMark"></param>
        /// <param name="direction">0: horizontal, 1: vertical, 2: rotate 45degree</param>
        private void WaterMark(Graphics g, WatermarkSettingElement watermarkSetting)
        {
            string waterMark = watermarkSetting.Text;
            WaterMarkPosition direction = watermarkSetting.Position;

            float height = g.VisibleClipBounds.Height;
            float width = g.VisibleClipBounds.Width;
            bool isVertical = (direction & WaterMarkPosition.VerticalCenter) == WaterMarkPosition.VerticalCenter;

            //Brush b = new SolidBrush(Color.FromArgb(20, 255, 255, 255));
            StringFormat format = StringFormat.GenericTypographic;
            format.LineAlignment = direction == WaterMarkPosition.HorizontalBottom ? StringAlignment.Far : direction == WaterMarkPosition.HorizontalTop ? StringAlignment.Near : StringAlignment.Center;
            format.Alignment = direction == WaterMarkPosition.VerticalLeft ? StringAlignment.Near : direction == WaterMarkPosition.VerticalRight ? StringAlignment.Far : StringAlignment.Center;
            if (isVertical)
                format.FormatFlags = StringFormatFlags.DirectionVertical;
            else
                format.FormatFlags = StringFormatFlags.NoWrap;

            format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;


            Font f = new Font(watermarkSetting.FontName, 10.0F, FontStyle.Bold);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            SizeF sizeF = g.MeasureString(waterMark, f, (int)width, format);
            f = new Font(watermarkSetting.FontName, (float)(10.0F * 1.15 * width / sizeF.Width), FontStyle.Bold);
            sizeF = g.MeasureString(waterMark, f, (int)width, format);
            if (sizeF.Height > height / 2.5)
            {
                f = new Font(watermarkSetting.FontName, (float)(f.Size * 0.4 * height / f.Height), FontStyle.Bold);
                sizeF = g.MeasureString(waterMark, f, (int)width, format);
            }


            PointF centerPoint = new PointF((float)(width) / 2 + 1, (float)(height) / (direction == WaterMarkPosition.HorizontalBottom ? 1 : 2));
            PointF centerPoint2 = new PointF((float)(width) / 2, (float)(height) / (direction == WaterMarkPosition.HorizontalBottom ? 1 : 2) - 1);
            // Trigonometry: Tangent = Opposite / Adjacent
            double tangent = (double)height /
                             (double)width;

            // convert arctangent to degrees
            double angle = Math.Atan(tangent) * (180 / Math.PI);
            double halfHypotenuse = (Math.Sqrt((height
                        * height) +
                        (width *
                        width))) / 2;
            if (direction == WaterMarkPosition.DiagonalTopDown)
            {
                f = new Font(watermarkSetting.FontName, (float)(f.Size * halfHypotenuse * 2 / width), FontStyle.Bold);
                

                g.RotateTransform((float)angle);
                centerPoint = new Point((int)halfHypotenuse, 0);
                centerPoint2 = new Point((int)halfHypotenuse + 1, +1);

            }
            if (direction == WaterMarkPosition.DiagonalBottomUp)
            {
                f = new Font(watermarkSetting.FontName, (float)(f.Size * halfHypotenuse * 2 / width), FontStyle.Bold);
                
                g.TranslateTransform(-centerPoint.X, -centerPoint.Y, MatrixOrder.Append);
                g.RotateTransform(360f - (float)angle, MatrixOrder.Append);
                g.TranslateTransform(centerPoint.X, centerPoint.Y, MatrixOrder.Append);

            }


            using (GraphicsPath path = new GraphicsPath())
            using (Brush br = new SolidBrush(System.Drawing.ColorTranslator.FromHtml(watermarkSetting.Color)))
            using (Pen p = new Pen(br, 1))
            using (Brush fillbr = new SolidBrush(Color.FromArgb(watermarkSetting.Opacity, System.Drawing.ColorTranslator.FromHtml(watermarkSetting.FillColor))))
            {
                path.AddString(waterMark, f.FontFamily, (int)f.Style, f.Size, centerPoint, format);

                g.FillPath(fillbr, path);
                g.DrawPath(p, path);


                if (direction == WaterMarkPosition.DiagonalBottomUp)
                {
                    g.ResetTransform();

                }
            }
            f.Dispose(); format.Dispose();
            //b.Dispose();
        }
        private ImageHandlerSettings GetConfig()
        {

            ImageHandlerSettings configs = (ImageHandlerSettings)ConfigurationManager.GetSection("ImageHandler");
            if (configs == null)
                throw new Exception("There is no configuration section named ImageHandler in your configuration file");
            return configs;
        }
        private void OutputCacheResponse(HttpContext context, DateTime lastModified)
        {
            HttpCachePolicy cachePolicy = context.Response.Cache;
            cachePolicy.SetCacheability(HttpCacheability.Public);
            cachePolicy.VaryByParams["width"] = true;
            cachePolicy.VaryByParams["w"] = true;
            cachePolicy.VaryByParams["height"] = true;
            cachePolicy.VaryByParams["h"] = true;
            cachePolicy.SetOmitVaryStar(true);
            cachePolicy.SetExpires(DateTime.Now + TimeSpan.FromDays(365));
            cachePolicy.SetValidUntilExpires(true);
            cachePolicy.SetLastModified(lastModified);
        }


    }

    public enum WaterMarkPosition
    {
        HorizontalMiddle = 1,
        HorizontalBottom = 9,
        HorizontalTop = 17,
        VerticalCenter = 2,
        VerticalLeft = 10,
        VerticalRight = 18,
        DiagonalBottomUp = 4,
        DiagonalTopDown = 12
    }
}
