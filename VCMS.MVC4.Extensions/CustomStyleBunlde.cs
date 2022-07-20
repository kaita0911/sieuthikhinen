using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace VCMS.MVC4.Extensions
{
    public class RelativePathResolverTransform : IBundleTransform
    {
        public  void Process(BundleContext context, BundleResponse response)
        {
            
            response.Content = String.Empty;

            Regex pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
            // open each of the files
            foreach (var bundleFile in response.Files)
            {
                
                var cssFileInfo = new FileInfo(context.HttpContext.Server.MapPath(bundleFile.VirtualFile.VirtualPath));
                if (cssFileInfo.Exists)
                {
                    // apply the RegEx to the file (to change relative paths)
                    string contents = File.ReadAllText(cssFileInfo.FullName);
                    MatchCollection matches = pattern.Matches(contents);
                    // Ignore the file if no match 
                    if (matches.Count > 0)
                    {
                        string cssFilePath = cssFileInfo.DirectoryName;
                        string cssVirtualPath = RelativeFromAbsolutePath(context.HttpContext, cssFilePath);

                        foreach (Match match in matches)
                        {
                            // this is a path that is relative to the CSS file
                            string relativeToCSS = match.Groups[2].Value.Trim().Replace("?","_QUESTION_");
                            // combine the relative path to the cssAbsolute
                            string absoluteToUrl = Path.GetFullPath(Path.Combine(cssFilePath, relativeToCSS));

                            // make this server relative
                            string serverRelativeUrl = RelativeFromAbsolutePath(context.HttpContext, absoluteToUrl);

                            string quote = match.Groups[1].Value;
                            string replace = String.Format("url({0}{1}{0})", quote, serverRelativeUrl.Replace("_QUESTION_","?"));
                            contents = contents.Replace(match.Groups[0].Value, replace);
                        }
                    }
                    // copy the result into the response.                    
                    response.Content = String.Format("{0}\r\n{1}", response.Content, contents);
                }
            }
            
        }

        private static string RelativeFromAbsolutePath(HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
    public class ImageRelativeStyleBundle : StyleBundle
    {
        public ImageRelativeStyleBundle(string virtualPath)
            : base(virtualPath)
        {
            Transforms.Add(new RelativePathResolverTransform());
            Transforms.Add(new CssMinify());
        }

        public ImageRelativeStyleBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath)
        {
            Transforms.Add(new RelativePathResolverTransform());
            Transforms.Add(new CssMinify());
        }
    }
}