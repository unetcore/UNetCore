using System;

    /// <summary>
    /// 	Extension methods for the Float data type
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>Checks whether the value is in range</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        public static bool InRange(this float value, float minValue, float maxValue)
        {
            return (value >= minValue && value <= maxValue);
        }

        /// <summary>Checks whether the value is in range or returns the default value</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <param name="defaultValue">The default value</param>
        public static float InRange(this float value, float minValue, float maxValue, float defaultValue)
        {
            return value.InRange(minValue, maxValue) ? value : defaultValue;
        }

        /// <summary>
        /// Gets a TimeSpan from a float number of days.
        /// </summary>
        /// <param name="days">The number of days the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of days.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Days(this float days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Gets a TimeSpan from a float number of hours.
        /// </summary>
        /// <param name="days">The number of hours the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of hours.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Hours(this float hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Gets a TimeSpan from a float number of milliseconds.
        /// </summary>
        /// <param name="days">The number of milliseconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Milliseconds(this float milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Gets a TimeSpan from a float number of minutes.
        /// </summary>
        /// <param name="days">The number of minutes the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of minutes.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Minutes(this float minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Gets a TimeSpan from a float number of seconds.
        /// </summary>
        /// <param name="days">The number of seconds the TimeSpan will contain.</param>
        /// <returns>A TimeSpan containing the specified number of seconds.</returns>
        /// <remarks>
        ///		Contributed by jceddy
        /// </remarks>
        public static TimeSpan Seconds(this float seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }



        #region Single转换
        private static System.Globalization.NumberFormatInfo NumberFormatInfo = (System.Globalization.NumberFormatInfo)System.Globalization.NumberFormatInfo.CurrentInfo.Clone();//初始化数字格式对象

        /// <summary>
        /// displays 12.30%
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSingleString_p(this Single value)
        {
            NumberFormatInfo.PercentPositivePattern = 1;
            return value.ToString("p", NumberFormatInfo);
        }
        /// <summary>
        /// displays 12.3%
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSingleString_p1(this Single value)
        {
            NumberFormatInfo.PercentPositivePattern = 1;
            return value.ToString("p1", NumberFormatInfo);
        }
        /// <summary>
        /// displays (1234567.89)= #1_234_5_67:89
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSingleString_C(this Single value)
        {
            int[] groupsize = { 2, 1, 3 };
            NumberFormatInfo.CurrencySymbol = "#"; //符号
            NumberFormatInfo.CurrencyDecimalSeparator = ":"; //小数点
            NumberFormatInfo.CurrencyGroupSeparator = "_";  //分隔符
            NumberFormatInfo.CurrencyGroupSizes = groupsize;
            return value.ToString("C", NumberFormatInfo);
        }

        #endregion

        #region double转换
        /// <summary>
        /// displays 3.14159265358979
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleString(this double value)
        {
            return value.ToString();
        }
        /// <summary>
        /// displays 3.141593E+000
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleString_E(this double value)
        {
            return value.ToString("E");
        }
        /// <summary>
        /// displays 3.142
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleString_F3(this double value)
        {
            return value.ToString("F3");
        }

        /// <summary>
        /// displays 12345.19
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_f(this double value)
        {
            return string.Format("{0:f}", value);
        }
        /// <summary>
        /// displays ￥12,345.79
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_C(this double value)
        {
            return string.Format("{0:C}", value);
        }
        /// <summary>
        /// displays ￥12,345.789
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_C3(this double value)
        {
            return string.Format("{0:C3}", value);
        }
        /// <summary>
        /// displays 1.234579e+004
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_e(this double value)
        {
            return string.Format("{0:e}", value);
        }
        /// <summary>
        /// displays 12,345.79
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_n(this double value)
        {
            return string.Format("{0:n}", value);
        }
        /// <summary>
        /// displays 12345.789
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_g(this double value)
        {
            return string.Format("{0:g}", value);
        }
        /// <summary>
        /// displays 不损失精度12345.789
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDoubleFormat_r(this double value)
        {
            return string.Format("{0:r}", value);
        }

        #endregion

    }
