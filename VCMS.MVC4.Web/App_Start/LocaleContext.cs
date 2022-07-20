using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Data.Entity;

namespace VCMS.MVC4.Web
{
    public static class LocaleContext
    {
        private static Dictionary<string, string> _localeTable = new Dictionary<string, string>();
        private static Dictionary<int, bool> _localeLoaded = new Dictionary<int, bool>();
        public static void Reset()
        {
            _localeTable = new Dictionary<string, string>();
        }
        public static IHtmlString Locale(this HtmlHelper helper, string localeKey)
        {
            return helper.Raw(Locale(localeKey));
        }

        public static string Locale(string localeKey)
        {
            //var key = SiteConfig.LanguageId.ToString() + localeKey;
            //if (_localeTable.ContainsKey(key))
            //    return _localeTable[key];
            //else
            //{
            //    using (DataContext db = new DataContext())
            //    {
            //        var locale = db.Locales.Include(l => l.LocaleDetail).FirstOrDefault(l => l.LocaleKey.Equals(localeKey, StringComparison.OrdinalIgnoreCase));
            //        if (locale == null)
            //            return localeKey;
            //        locale.LanguageId = SiteConfig.LanguageId;
            //        _localeTable.Add(key, locale.Value);
            //        return locale.Value;
            //    }
            //}
            var key = SiteConfig.LanguageId.ToString() + localeKey;
            if (_localeTable.ContainsKey(key))
                return _localeTable[key];
            else
            {
                if (!_localeLoaded.ContainsKey(SiteConfig.LanguageId)) /// table not loaded 
                {
                    _localeTable = _localeTable.Concat(VCMS.MVC4.Data.Locale.GetLocaleTable(SiteConfig.LanguageId)).ToDictionary(x => x.Key, x => x.Value);
                    if (!_localeLoaded.ContainsKey(SiteConfig.LanguageId))
                        _localeLoaded.Add(SiteConfig.LanguageId, true);
                }
                if (_localeTable.ContainsKey(key))
                    return _localeTable[key];
                else return localeKey;
            }
        }
    }

}