using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace VCMS.MVC4.ImageResize
{
    public class ImageResize
    {

        //private ImageHandlerSettings GetConfig()
        //{

        //    ImageHandlerSettings configs = (ImageHandlerSettings)ConfigurationManager.GetSection("ImageHandler");
        //    if (configs == null)
        //        throw new Exception("There is no configuration section named ImageHandler in your configuration file");
        //    return configs;
        //}
        //public string DrawImage(HttpContext context)
        //{
 
        //}
        public string DrawImage(string file, string sWidth, string sHeight, ImageHandlerSettingElement conf, string cachePath)
        {
            int width = 0, height = 0;
            if (!string.IsNullOrEmpty(sWidth)) int.TryParse(sWidth, out width);
            if (!string.IsNullOrEmpty(sHeight)) int.TryParse(sHeight, out height);

            //string noImage = string.Empty;//ConfigurationManager.AppSettings["ImageHandler.DefaultImageVirtualPath"];
            //string matchPattern = string.Empty;//ConfigurationManager.AppSettings["ImageHandler.MatchPattern"];
            int defaultWidth = 0;

            WatermarkSettingElement waterMark = null;

            //noImage = conf.DefaultImageVirtualPath;
            //matchPattern = conf.MatchPattern;
            waterMark = conf.Watermark;
            defaultWidth = conf.DefaultWidth;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (System.Drawing.Image wImg = System.Drawing.Image.FromStream(fs,true,false))
                {

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
                    ImageFormat imgFormat = wImg.RawFormat; //ImageFormat.Jpeg;// wImg.RawFormat;

                    if (frameCount > 1) //animated
                    {
                        //wImg.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //wImg.Dispose();
                        //fs.Close();
                        //fs.Dispose();
                        return file;
                        
                    }

                    using (Bitmap dst = new Bitmap(width, height))
                    {
                        //dst.MakeTransparent(Color.White);
                        using (Graphics g = Graphics.FromImage(dst))
                        {
                            //g.FillRectangle(new SolidBrush(Color.FromArgb(90, 255, 255, 255)), 0, 0, width, height);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.InterpolationMode = InterpolationMode.Default;
                            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                            g.CompositingQuality = CompositingQuality.HighSpeed;
                            //if (imgFormat == ImageFormat.Png)
                            //    ((Bitmap)wImg).MakeTransparent(Color.White);

                            g.DrawImage(wImg, 0, 0, width, height);


                            ImageCodecInfo jpegCodec = GetEncoderInfo(imgFormat);
                            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 60L);

                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = qualityParam;

                            //ImageCodecInfo[] alleCodecs = ImageCodecInfo.GetImageEncoders();
                            
                            
                            if (waterMark != null && !string.IsNullOrEmpty(waterMark.Text))
                            {
                                this.WaterMark(g, waterMark);
                            }
                            using (FileStream diskCacheStream = new FileStream(cachePath, FileMode.CreateNew))
                            {
                                //dst.Save(diskCacheStream, imgFormat);
                                dst.Save(diskCacheStream, jpegCodec, encoderParams);
                            }
                            encoderParams.Dispose();
                            qualityParam.Dispose();

                            //g.Dispose();
                        }

                        //dst.Dispose();
                    }

                    //wImg.Dispose();
                }
                //fs.Close();
                //fs.Dispose();
                return cachePath;
            }
        }

       

        public static ImageCodecInfo GetEncoderInfo(ImageFormat format)
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
        public void WaterMark(Graphics g, WatermarkSettingElement watermarkSetting)
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

                g.RotateTransform((float)angle);
                centerPoint = new Point((int)halfHypotenuse, 0);
                centerPoint2 = new Point((int)halfHypotenuse + 1, +1);

            }
            if (direction == WaterMarkPosition.DiagonalBottomUp)
            {
                g.TranslateTransform(-centerPoint.X, -centerPoint.Y, MatrixOrder.Append);
                g.RotateTransform(360f - (float)angle, MatrixOrder.Append);
                g.TranslateTransform(centerPoint.X, centerPoint.Y, MatrixOrder.Append);

            }
            

            using (GraphicsPath path = new GraphicsPath())
            using (Brush br = new SolidBrush(System.Drawing.ColorTranslator.FromHtml(watermarkSetting.Color)))
            using (Pen p = new Pen(br,1))
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

    }
    //public enum WaterMarkPosition
    //{
    //    HorizontalMiddle = 1,
    //    HorizontalBottom = 9,
    //    HorizontalTop = 17,
    //    VerticalCenter = 2,
    //    VerticalLeft = 10,
    //    VerticalRight = 18,
    //    DiagonalBottomUp = 4,
    //    DiagonalTopDown = 12
    //}

}
