using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.MVC4.Data;
using System.Web.Mvc.Html;
using System.Globalization;
namespace VCMS.MVC4.Web
{
    public static class FormatContext
    {
        public static MvcHtmlString Price(this HtmlHelper htmlHelper, Price item)
        {
            var stringprice = htmlHelper.Locale("contact").ToHtmlString();
            if (item == null)
                return MvcHtmlString.Create(stringprice);
            //if (item.Inactive)
            //    return MvcHtmlString.Create(stringprice);
            if (item.Value > 0)
            {
                var currency = item.Currency;
                if (currency.CheckFormat)
                    stringprice = string.Format("{" + currency.Formatting + "}" + currency.CurrencySymbol, item.Value);
                else
                {
                    CultureInfo cultureInfo = CultureInfo(currency);
                    stringprice = item.Value.ToString("C", cultureInfo);
                }
            }
            return MvcHtmlString.Create(stringprice);
        }
        public static MvcHtmlString DiscountPrice(this HtmlHelper htmlHelper, Price item)
        {
            var stringprice = "";

            if (item == null)
                return MvcHtmlString.Create(stringprice);

            var price = item.Article.DiscountPrice;
            if (item.Value > 0)
            {
                var currency = item.Currency;
                if (currency.CheckFormat)
                    stringprice = string.Format("{" + currency.Formatting + "}" + currency.CurrencySymbol, price);
                else
                {
                    CultureInfo cultureInfo = CultureInfo(currency);
                    stringprice = price.ToString("C", cultureInfo);
                }
            }
            return MvcHtmlString.Create(stringprice);
        }

        public static MvcHtmlString AdminDiscountFormat(this HtmlHelper htmlHelper, Discount item)
        {
            var stringprice = "0";
            if (item == null)
                return MvcHtmlString.Create(stringprice);
            if (item.IsAmount)
            {
                if (item.DiscountAmount > 0)
                {
                    var currency = item.Currency;
                    if (currency.CheckFormat)
                        stringprice = string.Format("{" + currency.Formatting + "}" + currency.CurrencySymbol, item.DiscountAmount.Value);
                    else
                    {
                        CultureInfo cultureInfo = CultureInfo(currency);
                        cultureInfo.NumberFormat.CurrencyPositivePattern = 3;
                        stringprice = item.DiscountAmount.Value.ToString("C", cultureInfo);
                    }
                }
            }
            return MvcHtmlString.Create(stringprice);
        }
        public static CultureInfo CultureInfo(Currency item)
        {
            CultureInfo cultureInfo = new CultureInfo(SiteConfig.LanguageCode);
            cultureInfo = (CultureInfo)cultureInfo.Clone();

            if (item.CurrencyPositivePattern > -1)
                cultureInfo.NumberFormat.CurrencyPositivePattern = item.CurrencyPositivePattern;

            if (!string.IsNullOrWhiteSpace(item.CurrencySymbol))
                cultureInfo.NumberFormat.CurrencySymbol = item.CurrencySymbol;

            if (item.Code.Equals("vi-VN", StringComparison.OrdinalIgnoreCase))
                cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;

            return cultureInfo;
        }

        public static MvcHtmlString Amount(this HtmlHelper htmlHelper, decimal price)
        {
            var stringprice = "";
            if (price <= 0)
                return MvcHtmlString.Create(stringprice);
            else
            {
                using (DataContext db = new DataContext())
                {
                    var cur = db.Currencies.FirstOrDefault(a => a.IsDefault);
                    if (cur != null)
                    {
                        if (cur.CheckFormat)
                            stringprice = string.Format("{" + cur.Formatting + "}&nbsp;" + cur.CurrencySymbol, price);
                        else
                        {
                            CultureInfo cultureInfo = new CultureInfo(cur.Code);
                            cultureInfo = (CultureInfo)cultureInfo.Clone();

                            if (cur.CurrencyPositivePattern > -1)
                                cultureInfo.NumberFormat.CurrencyPositivePattern = cur.CurrencyPositivePattern;

                            if (!string.IsNullOrWhiteSpace(cur.CurrencySymbol))
                                cultureInfo.NumberFormat.CurrencySymbol = cur.CurrencySymbol;

                            if (cur.Code.Equals("vi-VN", StringComparison.OrdinalIgnoreCase))
                                cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;

                            stringprice = price.ToString("C", cultureInfo);
                        }
                    }
                }
            }
            return MvcHtmlString.Create(stringprice);
        }
        public static MvcHtmlString AmountVat (this HtmlHelper htmlHelper, decimal price)
        {
            var stringprice = "";
            if (price <= 0)
                return MvcHtmlString.Create(stringprice);
            else
            {
                using (DataContext db = new DataContext())
                {
                    var cur = db.Currencies.FirstOrDefault(a => a.IsDefault);
                    if (cur != null)
                    {
                        if (cur.CheckFormat)
                            stringprice = string.Format("{" + cur.Formatting + "}&nbsp;" + cur.CurrencySymbol, price);
                        else
                        {
                            CultureInfo cultureInfo = new CultureInfo(cur.Code);
                            cultureInfo = (CultureInfo)cultureInfo.Clone();

                            if (cur.CurrencyPositivePattern > -1)
                                cultureInfo.NumberFormat.CurrencyPositivePattern = cur.CurrencyPositivePattern;

                            if (!string.IsNullOrWhiteSpace(cur.CurrencySymbol))
                                cultureInfo.NumberFormat.CurrencySymbol = cur.CurrencySymbol;

                            if (cur.Code.Equals("vi-VN", StringComparison.OrdinalIgnoreCase))
                                cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;

                            stringprice = price.ToString("C", cultureInfo);
                        }
                    }
                }
            }
            return MvcHtmlString.Create(stringprice);
        }

    }

}