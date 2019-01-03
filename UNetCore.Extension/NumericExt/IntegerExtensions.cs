/// <summary>
/// Integer Extensions
/// </summary>
public static class IntegerExtensions
{
    #region int转换
    /// <summary>
    /// displays ffff
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_x(this int value)
    {
        return value.ToString("x");
    }
    /// <summary>
    /// displays FFFF
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_X(this int value)
    {
        return value.ToString("X");
    }

    /// <summary>
    /// displays 十进制数 123456789
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_d(this int value)
    {
        return string.Format("{0:d}", value);
    }
    /// <summary>
    /// displays 浮点数 123456789.00
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_f(this int value)
    {
        return string.Format("{0:f}", value);
    }
    /// <summary>
    /// displays ￥123,456,789.00
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_C(this int value)
    {
        return string.Format("{0:C}", value);
    }
    /// <summary>
    /// displays ￥123,456,789.000
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_C3(this int value)
    {
        return string.Format("{0:C3}", value);
    }
    /// <summary>
    /// displays 指数1.234568e+008(7位有效位)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_e(this int value)
    {
        return string.Format("{0:e}", value);
    }
    /// <summary>
    /// displays 数值123,456,789.00
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_n(this int value)
    {
        return string.Format("{0:n}", value);
    }
    /// <summary>
    /// displays 十六进制数75bcd15
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_0x(this int value)
    {
        return string.Format("{0:x}", value);
    }
    /// <summary>
    /// displays 最紧凑123456789
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToIntString_g(this int value)
    {
        return string.Format("{0:g}", value);
    }
    #endregion

}
