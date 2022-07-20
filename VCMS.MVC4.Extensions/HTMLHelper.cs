using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace VNS.Web.Helpers
{
    /// <summary>
    /// Summary description for HTMLHelper
    /// </summary>
    public class HTMLHelper
    {
        public HTMLHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string RemoveTag(string html, string tagName)
        {
            /*tagName = tagName.Replace(",", "|");
            Regex reg = new Regex(string.Format(@"<({0})\b[^>]*?>(.*[\r\n\t]*)*?</\1\s*?>", tagName), RegexOptions.IgnoreCase);
        
            return reg.Replace(html, "");*/
            return RemoveTag(html, tagName, false);
        }

        public static string RemoveComment(string html)
        {
            if (html == null) return "";
           
                Regex reg = new Regex(@"<!--[\d\D]*?-->", RegexOptions.IgnoreCase);
                return reg.Replace(html, "");
            

        }
        public static string RemoveStyle(string html)
        {
            return RemoveTag(html, "style");
        }
        public static string RemoveStyle(string html, string styleAttributes)
        {
            if (html == null) return "";
            styleAttributes = styleAttributes.Replace(",", "|");
            Regex reg = new Regex(string.Format(@"(['""]+).*?(({0}):.*?(;|\1))", styleAttributes), RegexOptions.IgnoreCase);

            return reg.Replace(html, "");
        }
        public static string RemoveScript(string html)
        {
            return RemoveTag(html, "script");
        }
        public static string RemoveFormat(string html)
        {
            if (html == null) return "";
            string clean = RemoveStyle(html);
            clean = RemoveTag(clean, "b,strong,font", true);
            clean = RemoveAttribute(clean, "style,height,width,align,valign,class");
            //clean = Regex.Replace(clean, "<p[^>]*?>(.*)?</p[^>]*?>", @"\1<br />");
            return clean;
        }

        public static string RemoveTag(string html, string tagName, bool keepInnerHtml)
        {
            if (html == null) return "";
            tagName = tagName.Replace(",", "|");
            Regex reg;
            if (keepInnerHtml)
                reg = new Regex(string.Format(@"<({0})[^>]*?>|</\s*{0}\s*>", tagName), RegexOptions.IgnoreCase);
            else
                reg = new Regex(string.Format(@"<({0})[^>]*?>(?:.|\n)*?</\s*\1\s*>", tagName), RegexOptions.IgnoreCase);

            return reg.Replace(html, "");
        }
        public static string RemoveAttribute(string html, string attributeName)
        {
            if (html == null) return "";
            attributeName = attributeName.Replace(",", "|");
            Regex reg = new Regex(string.Format(@"({0})\s*?=\s*?(['""]*).*?\2", attributeName), RegexOptions.IgnoreCase);

            return reg.Replace(html, "");
        }

        public static string RemoveStyleAttribute(string html, string attributeName)
        {
            if (html == null) return "";
            attributeName = attributeName.Replace(",", "|");
            Regex reg = new Regex(string.Format(@"style\s*?=(['""]).*?(({0})\s*?:.+?)\1", attributeName), RegexOptions.IgnoreCase);

            return reg.Replace(html, "");
        }

        public static string CleanUp(string html)
        {
            if (html == null) return "";
            string clean = RemoveComment(html);
            clean = RemoveStyle(clean);
            clean = RemoveScript(clean);
            clean = Regex.Replace(clean, @"<p>(\s|&nbsp;)*?</p>", "");
            return clean;
        }
        public static string RemoveAllTags(string html, string exceptions)
        {
            if (html == null) return ""; 
            exceptions = exceptions.Replace(",", "|");
            Regex reg = new Regex(string.Format(@"<(?!({0}))[^>]*?>|</\s*\1\s*>", exceptions), RegexOptions.IgnoreCase);

            return reg.Replace(html, "");
        }
        public static string LoadTemplate(string templatePath, params string[] args)
        {
            string body = "";
            if (System.IO.File.Exists(templatePath))
            {
                body = System.IO.File.ReadAllText(templatePath);
                if (args.Length > 0)
                {
                    string field = "", value = "";
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            field = args[i];
                        }
                        else
                        {
                            value = args[i];
                            body = body.Replace("{" + field + "}", value);
                        }
                    }
                }

            }
            return body;
        }

        public static string GetInnerText(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            string text = CleanUp(html);// remove style and script
            text = Regex.Replace(text, @"[\r\n]+", " "); //replace CRLF with spaces      
            text = Regex.Replace(text, @"<(p|br|li|tr|h\d+|div)\b[^>]*?>", "\r\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<\w+?\b[^>]*?>|</\b[^>]*?>", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"[\r\n]+(?![\r\n])\s*", "\r\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"^\s+", "", RegexOptions.IgnoreCase);

            text = HttpContext.Current.Server.HtmlDecode(text.Trim());
            text = Regex.Replace(text, @"(\s){2,}", "$1", RegexOptions.IgnoreCase);
            return text;
        }
        public static string ReadBlock(string html, int numword)
        {
            string text = GetInnerText(html).Trim();
            return ReadWords(text,numword);
        }

        public static string ReadWords(string text, int numword)
        {
            Regex reg = new Regex(@"\b\w+\b", RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(text);
            int count = matches.Count;
            if (count > numword)
            {
                if (!(count == numword + 1 && matches[numword].Length < 3))
                    text = text.Substring(0, matches[numword - 1].Index + matches[numword - 1].Length) + "...";
            }
            text = Regex.Replace(text, @"[\r\n]+", "<br/>");
            return text;
        }
        public static string ReadCharacters(string html, int max)
        {
            return ReadCharacters(html, max, false);
        }
        public static string ReadCharacters(string html, int max, bool isHTML)
        {
            string text = isHTML ? GetInnerText(html).Trim() : html.Trim();
            Regex reg = new Regex(@"\b\w+\b", RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(text);
            int count = matches.Count;
            int i = 0; string s = string.Empty;
            while (i < count)
            {
                if (matches[i].Index <= max && matches[i].Index + matches[i].Length >= max)
                {
                    if (matches[i].Index + matches[i].Length == max) i++;
                    break;
                }
                i++;

            }

            if (i > count - 1 || (i == count - 1 && matches[i].Length <= 3))
                s = text.Trim();
            else
            {
                if (i > 0)
                    s = text.Substring(0, matches[i].Index - 1).Trim() + " ...";
                else
                    s = matches[0].Value + " ...";
            }

            s = Regex.Replace(s, @"[\r\n]+", "<br/>");
            return s;
        }


        public static string ReadLines(string text, int maxword, int maxline)
        {
            Regex reg = new Regex(@"\b\w+\b", RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(text);
            int count = matches.Count;
            if (count >= maxword)
                text = text.Substring(0, matches[maxword - 1].Index + matches[maxword - 1].Length);
            text = Regex.Replace(text, @"[\r\n]+", "<br/>");
            matches = Regex.Matches(text, "<br/>");
            if (matches.Count > maxline)
            {
                text = text.Substring(0, matches[maxline].Index) ;
            }
            return text;
        }

        public static string Hightlight(string input, string keyword, string cssClass)
        {
            keyword = Unichar.UnicodeStrings.LatinToAscii(keyword.Replace("\"", ""));
            
            string[] keywords = keyword.Split(' ');
           
            string pattern = @"\b(" + Regex.Replace(keyword, @"[\s+=\*/?><{}\[\]().;,#$%!|]+", "|") + @")\b";
            string[] words = input.Split(' ');

            string test = string.Empty;
            for (int i = 0;i< words.Length ;i++)
            {
                string w = Unichar.UnicodeStrings.LatinToAscii(words[i]);
                if (Regex.IsMatch(w,pattern,RegexOptions.IgnoreCase))
                    test +=  (string.IsNullOrEmpty(test) ? "" : " ") + "<span class='" + cssClass + "'>" + words[i] + "</span>";
                else
                {
                    test += (string.IsNullOrEmpty(test) ? "" : " ") + words[i];
                }
            }
            return test;
            //string pattern = @"\b" + Regex.Replace(keyword.Replace("\"", ""), @"[\s+=\*/?><{}\[\]().;,#$%!|]+", "|") + @"\b";
            //return Regex.Replace(input, pattern, string.Format("<span class='{0}'>{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase);

        }
        public static string HightlightSearch(string input, string keyword, string cssClass, int numword, int maxline)
        {
            input = GetInnerText(input);
            string pattern = @"\b(" + Regex.Replace(keyword.Replace("\"", ""), @"[\s+=\*/?><{}\[\]().;,#$%!|]+", "|") + @")\b";
            Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            if (m != null)
            {
                //get start of the sentence
                m = Regex.Match(input.Substring(0, m.Index), "[\r\n\\.?!;]+", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                int pos = 0;
                if (m != null)
                    pos = m.Index + m.Length;
                return Regex.Replace(ReadLines(input.Substring(pos), numword, maxline), pattern, string.Format("<span class='{0}'>{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase) + "...";
            }
            else
                return ReadLines(input, numword, maxline);
        }

        public static string GetUrlChars(string url)
        {
            string s = url;
            string normalized = s.Normalize(NormalizationForm.FormKD);
            Encoding ascii = Encoding.GetEncoding(
                  "ascii",
                  new EncoderReplacementFallback(string.Empty),
                  new DecoderReplacementFallback(string.Empty));
            byte[] encodedBytes = new byte[ascii.GetByteCount(normalized)];
            int numberOfEncodedBytes = ascii.GetBytes(normalized, 0, normalized.Length,
            encodedBytes, 0);
            string newString = ascii.GetString(encodedBytes);
            return newString;

        }

        public static string ReadTagBlock(string html, string tagName, int num)
        {
            Regex reg = new Regex(string.Format(@"<({0})[^>]*?>.*?</\s*{0}\s*>", tagName), RegexOptions.IgnoreCase);
            MatchCollection matches =  reg.Matches(html);
            string ret = string.Empty;
            int i = 0;
            foreach (Match m in matches)
            {
                if (i > num) break;
                ret += m.Value;
                i++;
            }
            return ret;
        }

        public static string KeywordsPrepare(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return "";

            return string.Join(",", keyword.Split(new char[] { ',' }).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
        }

    }
}