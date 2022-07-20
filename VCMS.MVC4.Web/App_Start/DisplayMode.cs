using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace VCMS.MVC4.Web.App_Start
{
    public class DisplayMode
    {
        public string GetDeviceType(string device)
        {
            string ret = "";
            if (Regex.IsMatch(device, @"GoogleTV|SmartTV|Internet.TV|NetCast|NETTV|AppleTV|boxee|Kylo|Roku|DLNADOC|CE\-HTML", RegexOptions.IgnoreCase))
            {
                ret = "tv";
            }
            else if (Regex.IsMatch(device, "Xbox|PLAYSTATION.3|Wii", RegexOptions.IgnoreCase))
            {
                ret = "tv";
            }
            else if ((Regex.IsMatch(device, "iP(a|ro)d", RegexOptions.IgnoreCase) || (Regex.IsMatch(device, "tablet", RegexOptions.IgnoreCase)) && (!Regex.IsMatch(device, "RX-34", RegexOptions.IgnoreCase)) || (Regex.IsMatch(device, "FOLIO", RegexOptions.IgnoreCase))))
            {
                ret = "tablet";
            }
            else if ((Regex.IsMatch(device, "Linux", RegexOptions.IgnoreCase)) && (Regex.IsMatch(device, "Android", RegexOptions.IgnoreCase)) && (!Regex.IsMatch(device, "Fennec|mobi|HTC.Magic|HTCX06HT|Nexus.One|SC-02B|fone.945", RegexOptions.IgnoreCase)))
            {
                ret = "tablet";
            }
            else if ((Regex.IsMatch(device, "Kindle", RegexOptions.IgnoreCase)) || (Regex.IsMatch(device, "Mac.OS", RegexOptions.IgnoreCase)) && (Regex.IsMatch(device, "Silk", RegexOptions.IgnoreCase)))
            {
                ret = "tablet";
            }
            else if ((Regex.IsMatch(device, @"GT-P10|SC-01C|SHW-M180S|SGH-T849|SCH-I800|SHW-M180L|SPH-P100|SGH-I987|zt180|HTC(.Flyer|\\_Flyer)|Sprint.ATP51|ViewPad7|pandigital(sprnova|nova)|Ideos.S7|Dell.Streak.7|Advent.Vega|A101IT|A70BHT|MID7015|Next2|nook", RegexOptions.IgnoreCase)) || (Regex.IsMatch(device, "MB511", RegexOptions.IgnoreCase)) && (Regex.IsMatch(device, "RUTEM", RegexOptions.IgnoreCase)))
            {
                ret = "tablet";
            }
            else if ((Regex.IsMatch(device, "BOLT|Fennec|Iris|Maemo|Minimo|Mobi|mowser|NetFront|Novarra|Prism|RX-34|Skyfire|Tear|XV6875|XV6975|Google.Wireless.Transcoder", RegexOptions.IgnoreCase)))
            {
                ret = "mobile";
            }
            else if ((Regex.IsMatch(device, "Opera", RegexOptions.IgnoreCase)) && (Regex.IsMatch(device, "Windows.NT.5", RegexOptions.IgnoreCase)) && (Regex.IsMatch(device, @"HTC|Xda|Mini|Vario|SAMSUNG\-GT\-i8000|SAMSUNG\-SGH\-i9", RegexOptions.IgnoreCase)))
            {
                ret = "mobile";
            }
            else if ((Regex.IsMatch(device, "Windows.(NT|XP|ME|9)")) && (!Regex.IsMatch(device, "Phone", RegexOptions.IgnoreCase)) || (Regex.IsMatch(device, "Win(9|.9|NT)", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            else if ((Regex.IsMatch(device, "Macintosh|PowerPC", RegexOptions.IgnoreCase)) && (!Regex.IsMatch(device, "Silk", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            else if ((Regex.IsMatch(device, "Linux", RegexOptions.IgnoreCase)) && (Regex.IsMatch(device, "X11", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            else if ((Regex.IsMatch(device, "Solaris|SunOS|BSD", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            else if ((Regex.IsMatch(device, "Bot|Crawler|Spider|Yahoo|ia_archiver|Covario-IDS|findlinks|DataparkSearch|larbin|Mediapartners-Google|NG-Search|Snappy|Teoma|Jeeves|TinEye", RegexOptions.IgnoreCase)) && (!Regex.IsMatch(device, "Mobile", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            else
            {
                ret = "mobile";
            }
            return ret;
        }
    }
}