using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Extensions for StringBuilder
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// 追加一行字符串
    /// </summary>
    public static void AppendLine(this StringBuilder builder, string value, params Object[] parameters)
    {
        builder.AppendLine(string.Format(value, parameters));
    }

    /// <summary>
    /// 根据条件追加一行字符串
    /// </summary>
    /// <param name="this"></param>
    /// <param name="condition">The conditional expression to evaluate.</param>
    /// <param name="value"></param>
    public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, object value)
    {
        if (condition) sb.AppendLine(value.ToString());
        return sb;
    }

    /// <summary>
    /// 根据条件追加一行字符串
    /// </summary>
    /// <param name="this"></param>
    /// <param name="condition">The conditional expression to evaluate.</param>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, string format, params object[] args)
    {
        if (condition) sb.AppendFormat(format, args).AppendLine();
        return sb;
    }

    /// <summary>
    /// 根据条件追加一行字符串
    /// </summary>
    /// <param name="this"></param>
    /// <param name="condition"></param>
    /// <param name="value"></param>
    public static StringBuilder AppendIf(this StringBuilder sb, bool condition, object value)
    {
        if (condition) sb.Append(value.ToString());
        return sb;
    }

    /// <summary>
    /// 根据条件追加一行字符串
    /// </summary>
    /// <param name="this"></param>
    /// <param name="condition"></param>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public static StringBuilder AppendFormatIf(this StringBuilder sb, bool condition, string format, params object[] args)
    {
        if (condition) sb.AppendFormat(format, args);
        return sb;
    }
    /// <summary>
    /// 根据条件追加一行数组拼接的字符串
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="values">The values.</param>
    public static StringBuilder AppendJoin<T>(this StringBuilder @this, string separator, IEnumerable<T> values)
    {
        @this.Append(string.Join(separator, values));

        return @this;
    }

    /// <summary>
    /// 根据条件追加一行数组拼接的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="values">The values.</param>
    public static StringBuilder AppendJoin<T>(this StringBuilder @this, string separator, params T[] values)
    {
        @this.Append(string.Join(separator, values));

        return @this;
    }
    /// <summary>
    /// 追加一行格式化字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="format">Describes the format to use.</param>
    /// <param name="args">A variable-length parameters list containing arguments.</param>
    public static StringBuilder AppendLineFormat(this StringBuilder @this, string format, params object[] args)
    {
        @this.AppendLine(string.Format(format, args));

        return @this;
    }

    /// <summary>
    /// 追加一行格式化字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="format">Describes the format to use.</param>
    /// <param name="args">A variable-length parameters list containing arguments.</param>
    public static StringBuilder AppendLineFormat(this StringBuilder @this, string format, List<IEnumerable<object>> args)
    {
        @this.AppendLine(string.Format(format, args));

        return @this;
    }
    /// <summary>
    /// 追加一行数组拼接的字符串
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="values">The values.</param>
    public static StringBuilder AppendLineJoin<T>(this StringBuilder @this, string separator, IEnumerable<T> values)
    {
        @this.AppendLine(string.Join(separator, values));

        return @this;
    }

    /// <summary>
    /// 追加一行数组拼接的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="values">The values.</param>
    public static StringBuilder AppendLineJoin(this StringBuilder @this, string separator, params object[] values)
    {
        @this.AppendLine(string.Join(separator, values));

        return @this;
    }
    /// <summary>
    /// 从指定位置向后截取字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>A string.</returns>
    public static string Substring(this StringBuilder @this, int startIndex)
    {
        return @this.ToString(startIndex, @this.Length - startIndex);
    }

    /// <summary>
    /// 从指定位置向后截取指定长度的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="length">The length.</param>
    /// <returns>A string.</returns>
    public static string Substring(this StringBuilder @this, int startIndex, int length)
    {
        return @this.ToString(startIndex, length);
    }
}
