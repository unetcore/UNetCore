using System;
using System.Globalization;
    public static class DateTimeFormatExt
    {
        /// <summary>
        ///     A DateTime extension method that converts this object to a year month string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToYearMonthString(this DateTime @this)
        {
            return @this.ToString("y", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a year month string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToYearMonthString(this DateTime @this, string culture)
        {
            return @this.ToString("y", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a year month string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToYearMonthString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("y", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to an universal sortable long date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToUniversalSortableLongDateTimeString(this DateTime @this)
        {
            return @this.ToString("U", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to an universal sortable long date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToUniversalSortableLongDateTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("U", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to an universal sortable long date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToUniversalSortableLongDateTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("U", culture);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to an universal sortable date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToUniversalSortableDateTimeString(this DateTime @this)
        {
            return @this.ToString("u", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to an universal sortable date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToUniversalSortableDateTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("u", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to an universal sortable date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToUniversalSortableDateTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("u", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a sortable date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToSortableDateTimeString(this DateTime @this)
        {
            return @this.ToString("s", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a sortable date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToSortableDateTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("s", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a sortable date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToSortableDateTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("s", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a short time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortTimeString(this DateTime @this)
        {
            return @this.ToString("t", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("t", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("t", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a short date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateTimeString(this DateTime @this)
        {
            return @this.ToString("g", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("g", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("g", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a short date string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateString(this DateTime @this)
        {
            return @this.ToString("d", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short date string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateString(this DateTime @this, string culture)
        {
            return @this.ToString("d", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short date string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("d", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a short date long time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateLongTimeString(this DateTime @this)
        {
            return @this.ToString("G", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short date long time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateLongTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("G", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a short date long time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToShortDateLongTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("G", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a month day string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToMonthDayString(this DateTime @this)
        {
            return @this.ToString("m", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a month day string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToMonthDayString(this DateTime @this, string culture)
        {
            return @this.ToString("m", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a month day string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToMonthDayString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("m", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a long time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongTimeString(this DateTime @this)
        {
            return @this.ToString("T", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("T", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("T", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a long date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateTimeString(this DateTime @this)
        {
            return @this.ToString("F", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("F", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("F", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a long date string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateString(this DateTime @this)
        {
            return @this.ToString("D", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long date string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateString(this DateTime @this, string culture)
        {
            return @this.ToString("D", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long date string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("D", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a long date short time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateShortTimeString(this DateTime @this)
        {
            return @this.ToString("f", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long date short time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateShortTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("f", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a long date short time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToLongDateShortTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("f", culture);
        }
        /// <summary>
        ///     A DateTime extension method that converts this object to a full date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToFullDateTimeString(this DateTime @this)
        {
            return @this.ToString("F", DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a full date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToFullDateTimeString(this DateTime @this, string culture)
        {
            return @this.ToString("F", new CultureInfo(culture));
        }

        /// <summary>
        ///     A DateTime extension method that converts this object to a full date time string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToFullDateTimeString(this DateTime @this, CultureInfo culture)
        {
            return @this.ToString("F", culture);
        }

        #region 格式化日期
        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        /*  调用：
            example: DateTime dt = DateTime.Now;
         */

        /// <summary>
        /// displays 07/11/2004
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_d(this DateTime value)
        {
            return value.ToString("d", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
       /// <summary>
        /// displays Sunday, 11 July 2004
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
        public static string ToDateTimeString_D(this DateTime value)
        {
            return value.ToString("D", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sunday, 11 July 2004 10:52
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_f(this DateTime value)
        {
            return value.ToString("f", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sunday, 11 July 2004 10:52:36
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_F(this DateTime value)
        {
            return value.ToString("F", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 07/11/2004 10:52
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_g(this DateTime value)
        {
            return value.ToString("g", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 07/11/2004 10:52:36
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_G(this DateTime value)
        {
            return value.ToString("G", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays July 11
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_m(this DateTime value)
        {
            return value.ToString("m", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sun, 11 Jul 2004 10:52:36 GMT
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_r(this DateTime value)
        {
            return value.ToString("r", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2004-07-11T10:52:36
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_s(this DateTime value)
        {
            return value.ToString("s", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 10:52
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_t(this DateTime value)
        {
            return value.ToString("t", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 10:52:36
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_T(this DateTime value)
        {
            return value.ToString("T", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2004-07-11 10:52:36Z
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_u(this DateTime value)
        {
            return value.ToString("u", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sunday, 11 July 2004 02:52:36
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_U(this DateTime value)
        {
            return value.ToString("U", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2004 July
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_y(this DateTime value)
        {
            return value.ToString("y", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sunday, July 11 2004
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_ddddMMMMddyyyy(this DateTime value)
        {
            return value.ToString("dddd, MMMM dd yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sun, Jul 11 '04
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_dddMMMdyy(this DateTime value)
        {
            return value.ToString("ddd, MMM d \"'\"yy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays Sunday, July 11
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_ddddMMMMdd(this DateTime value)
        {
            return value.ToString("dddd, MMMM dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 7/04
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_Myy(this DateTime value)
        {
            return value.ToString("M/yy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 11-07-04
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_ddMMyy(this DateTime value)
        {
            return value.ToString("dd-MM-yy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2010/07/19 04:32:53
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_yyyyMMddhhmmss(this DateTime value)
        {
            return value.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2010-07-19 04:32:53
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_yyyyMMddhhmmss2(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd hh:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2010-07-19
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_yyyyMMdd(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2010年07月19日
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_yyyyMMdd_CN(this DateTime value)
        {
            return value.ToString("yyyy年MM月dd日", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 2010年07月19日 05时12分36秒
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_yyyyMMddhhmmss_CN(this DateTime value)
        {
            return value.ToString("yyyy年MM月dd日 hh时mm分ss秒", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// displays 05时12分36秒
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString_hhmmss_CN(this DateTime value)
        {
            return value.ToString("hh时mm分ss秒", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        #endregion

    }
