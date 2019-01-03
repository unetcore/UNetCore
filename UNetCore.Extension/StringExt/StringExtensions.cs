using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

/// <summary>
/// 	Extension methods for the string data type
/// </summary>
public static class StringExtensions
{

    #region TO adapt chinese

    /// <summary>
    /// 替换为Unicode编码格式
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string DeUnicode(this string source)
    {
        Regex regex = new Regex(@"(?<code>\\u[a-z0-9]{4})", RegexOptions.IgnoreCase);
        for (Match match = regex.Match(source); match.Success; match = match.NextMatch())
        {
            string oldValue = match.Result("${code}");
            int num = int.Parse(oldValue.Substring(2, 4), NumberStyles.HexNumber);
            string newValue = string.Format("{0}", (char)num);
            source = source.Replace(oldValue, newValue);
        }
        return source;
    }

    /// <summary>
    /// 获取Ansi字符串长度
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static int GetAnsiLength(this string source)
    {
        int count = Regex.Matches(source, "([一-龥])").Count;
        return ((source.Length - count) + (count * 2));
    }
    /// <summary>
    /// 获取字符串行数
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static int GetLines(this string source)
    {
        Regex regex = new Regex("(\r\n)");
        return (regex.Matches(source).Count + 1);
    }
    /// <summary>
    /// 是否整数
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsInteger(this string source)
    {
        return Regex.IsMatch(source, @"^([+-]?)\d+$");
    }
    /// <summary>
    /// 是否符合正则表达式规则
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pattern"></param>
    /// <param name="ignorCase"></param>
    /// <returns></returns>
    public static bool IsMatch(this string source, string pattern, bool ignorCase = false)
    {
        return Regex.IsMatch(source, pattern, ignorCase ? RegexOptions.IgnoreCase : RegexOptions.None);
    }


    private static bool IsSample(string word)
    {
        string[] source = new string[] { "people", "deer", "sheep" };
        return source.Contains<string>(word.ToLower());
    }
     
    /// <summary>
    /// 是否符合正则表达式规则
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool Like(this string source, string pattern)
    {
        return Regex.IsMatch(source, pattern);
    }


    /// <summary>
    /// 转换字符串为半角
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string ToDBC(this string source)
    {
        char[] chArray = source.ToCharArray();
        for (int i = 0; i < chArray.Length; i++)
        {
            if (chArray[i] == '　')
            {
                chArray[i] = ' ';
            }
            else if ((chArray[i] > 0xff00) && (chArray[i] < 0xff5f))
            {
                chArray[i] = (char)(chArray[i] - 0xfee0);
            }
        }
        return new string(chArray);
    }

    /// <summary>
    /// 转换为汉语拼音字符串
    /// </summary>
    /// <param name="source"></param>
    /// <param name="containRare"></param>
    /// <returns></returns>
    public static string ToPinyin(this string source, bool containRare = false)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }
        StringBuilder builder = new StringBuilder(source.Length);
        foreach (char ch in source)
        {
            if (ch.IsChinese())
            {
                int asciiCode = ch.GetAsciiCode();
                char ch2 = ChineseSpellHelper.FindBaseDictionary(asciiCode, ch);
                if ((ch2 == ch) && containRare)
                {
                    ch2 = ChineseSpellHelper.FindRareDictionary(asciiCode, ch);
                }
                builder.Append(ch2);
            }
            else
            {
                builder.Append(ch);
            }
        }
        return builder.ToString();
    }

     

    /// <summary>
    /// 转换字符串为全角
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string ToSBC(this string source)
    {
        char[] chArray = source.ToCharArray();
        for (int i = 0; i < chArray.Length; i++)
        {
            if (chArray[i] == ' ')
            {
                chArray[i] = '　';
            }
            else if (chArray[i] < '\x007f')
            {
                chArray[i] = (char)(chArray[i] + 0xfee0);
            }
        }
        return new string(chArray);
    }


    /// <summary>
    /// 转换字符串为英文单数词语
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string ToSingular(this string word)
    {
        if (!IsSample(word))
        {
            Regex regex = new Regex("(?<keep>[^aeiou])ies$");
            Regex regex2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex regex3 = new Regex("(?<keep>[sxzh])es$");
            Regex regex4 = new Regex("(?<keep>[^sxzhyu])s$");
            if (regex.IsMatch(word))
            {
                return regex.Replace(word, "${keep}y");
            }
            if (regex2.IsMatch(word))
            {
                return regex2.Replace(word, "${keep}");
            }
            if (regex3.IsMatch(word))
            {
                return regex3.Replace(word, "${keep}");
            }
            if (regex4.IsMatch(word))
            {
                return regex4.Replace(word, "${keep}");
            }
        }
        return word;
    }

#if NET45
    /// <summary>
    /// 转换字符串为繁体中文
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string ToTraditional(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return Microsoft.VisualBasic.Strings.StrConv(source, VbStrConv.TraditionalChinese, 0);
        }


        /// <summary>
        /// 转换字符串为简体中文
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSimplified(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return Microsoft.VisualBasic.Strings.StrConv(source, VbStrConv.SimplifiedChinese, 0);
        }
#endif

    #endregion

    #region Common string extensions

    /// <summary>
    /// 是否为空 null或者 ""
    /// </summary>
    /// <param name = "value">The string value to check.</param>
    public static bool IsEmpty(this string value)
    {
        return ((value == null) || (value.Length == 0));
    }

    /// <summary>
    /// 是否不为空
    /// </summary>
    /// <param name = "value">The string value to check.</param>
    public static bool IsNotEmpty(this string value)
    {
        return (value.IsEmpty() == false);
    }

    /// <summary>
    /// 如果为空则返回默认值
    /// </summary>
    /// <param name = "value">The string to check.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>Either the string or the default value.</returns>
    public static string IfEmpty(this string value, string defaultValue)
    {
        return (value.IsNotEmpty() ? value : defaultValue);
    }

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "parameters">The parameters.</param>
    /// <returns></returns>
    public static string FormatWith(this string value, params object[] parameters)
    {
        return string.Format(value, parameters);
    }

    /// <summary>
    /// 将文本截取为所提供的最大长度
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "maxLength">Maximum length.</param>
    /// <returns></returns>
    /// <remarks>
    /// 	Proposed by Rene Schulte
    /// </remarks>
    public static string TrimToMaxLength(this string value, int maxLength)
    {
        return (value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength));
    }

    /// <summary>
    /// 将文本截取为提供的最大长度，并根据需要添加后缀
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "maxLength">Maximum length.</param>
    /// <param name = "suffix">The suffix.</param>
    /// <returns></returns>
    /// <remarks>
    /// 	Proposed by Rene Schulte
    /// </remarks>
    public static string TrimToMaxLength(this string value, int maxLength, string suffix)
    {
        return (value == null || value.Length <= maxLength ? value : string.Concat(value.Substring(0, maxLength), suffix));
    }

    /// <summary>
    /// 是否包含某个字符串
    /// </summary>
    /// <param name = "inputValue">The input value.</param>
    /// <param name = "comparisonValue">The comparison value.</param>
    /// <param name = "comparisonType">Type of the comparison to allow case sensitive or insensitive comparison.</param>
    /// <returns>
    /// 	<c>true</c> if input value contains the specified value, otherwise, <c>false</c>.
    /// </returns>
    public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
    {
        return (inputValue.IndexOf(comparisonValue, comparisonType) != -1);
    }

    /// <summary>
    /// 是否等于或者包含于指定字符串
    /// </summary>
    /// <param name = "inputValue">The input value.</param>
    /// <param name = "comparisonValue">The comparison value.  Case insensitive</param>
    /// <returns>
    /// 	<c>true</c> if input value contains the specified value (case insensitive), otherwise, <c>false</c>.
    /// </returns>
    public static bool ContainsEquivalenceTo(this string inputValue, string comparisonValue)
    {
        return BothStringsAreEmpty(inputValue, comparisonValue) || StringContainsEquivalence(inputValue, comparisonValue);
    }

    /// <summary>
    /// 左右两边填充字符串
    /// </summary>
    /// <param name="value">Instance value.</param>
    /// <param name="width">The number of characters in the resulting string, 
    /// equal to the number of original characters plus any additional padding characters.
    /// </param>
    /// <param name="padChar">A Unicode padding character.</param>
    /// <param name="truncate">Should get only the substring of specified width if string width is 
    /// more than the specified width.</param>
    /// <returns>A new string that is equivalent to this instance, 
    /// but center-aligned with as many paddingChar characters as needed to create a 
    /// length of width paramether.</returns>
    public static string PadBoth(this string value, int width, char padChar, bool truncate = false)
    {
        int diff = width - value.Length;
        if (diff == 0 || diff < 0 && !(truncate))
        {
            return value;
        }
        else if (diff < 0)
        {
            return value.Substring(0, width);
        }
        else
        {
            return value.PadLeft(width - diff / 2, padChar).PadRight(width, padChar);
        }
    }

    /// <summary>
    /// 转换为XDocument
    /// </summary>
    /// <param name = "xml">The XML string.</param>
    /// <returns>The XML document object model (XDocument)</returns>
    public static XDocument ToXDocument(this string xml)
    {
        return XDocument.Parse(xml);
    }

    /// <summary>
    /// 转换为XmlDocument
    /// </summary>
    /// <param name = "xml">The XML string.</param>
    /// <returns>The XML document object model (XmlDocument)</returns>
    public static XmlDocument ToXmlDOM(this string xml)
    {
        var document = new XmlDocument();
        document.LoadXml(xml);
        return document;
    }

    /// <summary>
    /// 转换为 XPathNavigator
    /// </summary>
    /// <param name = "xml">The XML string.</param>
    /// <returns>The XML XPath document object model (XPathNavigator)</returns>
    public static XPathNavigator ToXPath(this string xml)
    {
        var document = new XPathDocument(new StringReader(xml));
        return document.CreateNavigator();
    }

    /// <summary>
    /// 转换为 XElement
    /// </summary>
    /// <param name = "xml">The XML string.</param>
    /// <returns>The XML element object model (XElement)</returns>
    public static XElement ToXElement(this string xml)
    {
        return XElement.Parse(xml);
    }

    /// <summary>
    /// 逆序转换字符串
    /// </summary>
    /// <param name = "value">The string to be reversed.</param>
    /// <returns>The reversed string</returns>
    public static string Reverse(this string value)
    {
        if (value.IsEmpty() || (value.Length == 1))
            return value;

        var chars = value.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    /// <summary>
    /// 确保字符串以某个字符串开头
    /// </summary>
    /// <param name = "value">The string value to check.</param>
    /// <param name = "prefix">The prefix value to check for.</param>
    /// <returns>The string value including the prefix</returns> 
    public static string EnsureStartsWith(this string value, string prefix)
    {
        return value.StartsWith(prefix) ? value : string.Concat(prefix, value);
    }

    /// <summary>
    /// 确保字符串以某个字符串结尾
    /// </summary>
    /// <param name = "value">The string value to check.</param>
    /// <param name = "suffix">The suffix value to check for.</param>
    /// <returns>The string value including the suffix</returns> 
    public static string EnsureEndsWith(this string value, string suffix)
    {
        return value.EndsWith(suffix) ? value : string.Concat(value, suffix);
    }



    #region Extract



    /// <summary>
    /// 截取所有数字
    /// </summary>
    /// <param name = "value">String containing digits to extract</param>
    /// <returns>
    /// 	All digits contained within the input string
    /// </returns>
    /// <remarks>
    /// 	Contributed by Kenneth Scott
    /// </remarks>

    public static string ExtractDigits(this string value)
    {
        return value.Where(Char.IsDigit).Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c)).ToString();
    }



    #endregion

    /// <summary>
    /// 将指定的字符串值与传递的附加字符串连接起来
    /// </summary>
    /// <param name = "value">The original value.</param>
    /// <param name = "values">The additional string values to be concatenated.</param>
    /// <returns>The concatenated string.</returns>
    public static string ConcatWith(this string value, params string[] values)
    {
        return string.Concat(value, string.Concat(values));
    }

    /// <summary>
    /// 转换为Guid对象
    /// </summary>
    /// <param name = "value">The original string value.</param>
    /// <returns>The Guid</returns>
    public static Guid ToGuid(this string value)
    {
        return new Guid(value);
    }

    /// <summary>
    /// 转换为安全的Guid对象
    /// </summary>
    /// <param name = "value">The original string value.</param>
    /// <returns>The Guid</returns>
    public static Guid ToGuidSafe(this string value)
    {
        return value.ToGuidSafe(Guid.Empty);
    }

    /// <summary>
    /// 转换为安全的Guid对象
    /// </summary>
    /// <param name = "value">The original string value.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The Guid</returns>
    public static Guid ToGuidSafe(this string value, Guid defaultValue)
    {
        if (value.IsEmpty())
            return defaultValue;

        try
        {
            return value.ToGuid();
        }
        catch { }

        return defaultValue;
    }

    /// <summary>
    /// 获取指定字符串前面的字符串.
    /// </summary>
    /// <param name = "value">The default value.</param>
    /// <param name = "x">The given string parameter.</param>
    /// <returns></returns>
    /// <remarks>Unlike GetBetween and GetAfter, this does not Trim the result.</remarks>
    public static string GetBefore(this string value, string x)
    {
        var xPos = value.IndexOf(x);
        return xPos == -1 ? String.Empty : value.Substring(0, xPos);
    }

    /// <summary>
    /// 获取指定指定范围内的字符串
    /// </summary>
    /// <param name = "value">The source value.</param>
    /// <param name = "x">The left string sentinel.</param>
    /// <param name = "y">The right string sentinel</param>
    /// <returns></returns>
    /// <remarks>Unlike GetBefore, this method trims the result</remarks>
    public static string GetBetween(this string value, string x, string y)
    {
        var xPos = value.IndexOf(x);
        var yPos = value.LastIndexOf(y);

        if (xPos == -1 || xPos == -1)
            return String.Empty;

        var startIndex = xPos + x.Length;
        return startIndex >= yPos ? String.Empty : value.Substring(startIndex, yPos - startIndex).Trim();
    }

    /// <summary>
    /// 获取指定字符串后面的字符串
    /// </summary>
    /// <param name = "value">The default value.</param>
    /// <param name = "x">The given string parameter.</param>
    /// <returns></returns>
    /// <remarks>Unlike GetBefore, this method trims the result</remarks>
    public static string GetAfter(this string value, string x)
    {
        var xPos = value.LastIndexOf(x);

        if (xPos == -1)
            return String.Empty;

        var startIndex = xPos + x.Length;
        return startIndex >= value.Length ? String.Empty : value.Substring(startIndex).Trim();
    }

    /// <summary>
    /// 组装数组字符串
    /// </summary>
    /// <typeparam name = "T">
    /// 	The type of the array to join
    /// </typeparam>
    /// <param name = "separator">
    /// 	The separator to appear between each element
    /// </param>
    /// <param name = "value">
    /// 	An array of values
    /// </param>
    /// <returns>
    /// 	The join.
    /// </returns>
    /// <remarks> 
    /// </remarks>
    public static string Join<T>(this T[] value, string separator)
    {
        if (value == null || value.Length == 0)
            return string.Empty;
        if (separator == null)
            separator = string.Empty;
        Converter<T, string> converter = o => o.ToString();
        return string.Join(separator, Array.ConvertAll(value, converter));
    }

    /// <summary>
    /// 移除指定字符
    /// </summary>
    /// <param name = "value">
    /// 	The input.
    /// </param>
    /// <param name = "removeCharc">
    /// 	The remove char.
    /// </param> 
    public static string Remove(this string value, params char[] removeCharc)
    {
        var result = value;
        if (!string.IsNullOrEmpty(result) && removeCharc != null)
            Array.ForEach(removeCharc, c => result = result.Remove(c.ToString()));

        return result;

    }

    /// <summary>
    /// 移除指定字符串
    /// </summary>
    /// <param name="value">The input.</param>
    /// <param name="strings">The strings.</param>
    /// <returns></returns>
    public static string Remove(this string value, params string[] strings)
    {
        return strings.Aggregate(value, (current, c) => current.Replace(c, string.Empty));
    }


    /// <summary>
    /// 是否为空字符串
    /// </summary>
    /// <param name = "value">The input string</param>
    public static bool IsEmptyOrWhiteSpace(this string value)
    {
        return (value.IsEmpty() || value.All(t => char.IsWhiteSpace(t)));
    }

    /// <summary>
    /// 是否不为空字符串
    /// </summary>
    /// <param name = "value">The string value to check</param>
    public static bool IsNotEmptyOrWhiteSpace(this string value)
    {
        return (value.IsEmptyOrWhiteSpace() == false);
    }

    /// <summary>
    /// 是否为空字符串，如果是返回默认值
    /// </summary>
    /// <param name = "value">The string to check</param>
    /// <param name = "defaultValue">The default value</param>
    /// <returns>Either the string or the default value</returns>
    public static string IfEmptyOrWhiteSpace(this string value, string defaultValue)
    {
        return (value.IsEmptyOrWhiteSpace() ? defaultValue : value);
    }

    /// <summary>
    /// 转换第一个字符为大写
    /// </summary>
    /// <param name = "value">The string value to process</param>
    public static string ToUpperFirstLetter(this string value)
    {
        if (value.IsEmptyOrWhiteSpace()) return string.Empty;

        char[] valueChars = value.ToCharArray();
        valueChars[0] = char.ToUpper(valueChars[0]);

        return new string(valueChars);
    }

    /// <summary>
    /// 返回左边指定长度字符串
    /// </summary>
    /// <param name="value">The original string.</param>
    /// <param name="characterCount">The character count to be returned.</param>
    /// <returns>The left part</returns>
    public static string Left(this string value, int characterCount)
    {
        if (value == null)
            throw new ArgumentNullException("value");
        if (characterCount >= value.Length)
            throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
        return value.Substring(0, characterCount);
    }

    /// <summary>
    /// 返回右边指定长度字符串
    /// </summary>
    /// <param name="value">The original string.</param>
    /// <param name="characterCount">The character count to be returned.</param>
    /// <returns>The right part</returns>
    public static string Right(this string value, int characterCount)
    {
        if (value == null)
            throw new ArgumentNullException("value");
        if (characterCount >= value.Length)
            throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
        return value.Substring(value.Length - characterCount);
    }

    /// <summary>
    /// 截取指定位置后面的字符串
    /// </summary>
    /// <param name="value">The original value.</param>
    /// <param name="index">The start index for substringing.</param>
    /// <returns>The right part.</returns>
    public static string SubstringFrom(this string value, int index)
    {
        return index < 0 ? value : value.Substring(index, value.Length - index);
    }


    /// <summary>
    /// 将文本大小写转换为标题大小写
    /// </summary>
    /// <remarks>UppperCase characters is the source string after the first of each word are lowered, unless the word is exactly 2 characters</remarks>
    public static string ToTitleCase(this string value)
    {
        return ToTitleCase(value, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 将文本大小写转换为标题大小写
    /// </summary>
    /// <remarks>UppperCase characters is the source string after the first of each word are lowered, unless the word is exactly 2 characters</remarks>
    public static string ToTitleCase(this string value, CultureInfo culture)
    {
        return culture.TextInfo.ToTitleCase(value);
    }
    /// <summary>
    /// 转换为复数
    /// </summary>
    /// <param name="singular"></param>
    /// <returns></returns>
    public static string ToPlural(this string singular)
    {
        // Multiple words in the form A of B : Apply the plural to the first word only (A)
        int index = singular.LastIndexOf(" of ");
        if (index > 0) return (singular.Substring(0, index)) + singular.Remove(0, index).ToPlural();

        // single Word rules
        //sibilant ending rule
        if (singular.EndsWith("sh")) return singular + "es";
        if (singular.EndsWith("ch")) return singular + "es";
        if (singular.EndsWith("us")) return singular + "es";
        if (singular.EndsWith("ss")) return singular + "es";
        //-ies rule
        if (singular.EndsWith("y")) return singular.Remove(singular.Length - 1, 1) + "ies";
        // -oes rule
        if (singular.EndsWith("o")) return singular.Remove(singular.Length - 1, 1) + "oes";
        // -s suffix rule
        return singular + "s";
    }

    /// <summary>
    /// 转换为HTML
    /// </summary>
    /// <param name="s">The current instance.</param>
    /// <returns>An HTML safe string.</returns>
    public static string ToHtmlSafe(this string s)
    {
        return s.ToHtmlSafe(false, false);
    }

    /// <summary>
    /// 转换为HTML
    /// </summary>
    /// <param name="s">The current instance.</param>
    /// <param name="all">Whether to make all characters entities or just those needed.</param>
    /// <returns>An HTML safe string.</returns>
    public static string ToHtmlSafe(this string s, bool all)
    {
        return s.ToHtmlSafe(all, false);
    }

    /// <summary>
    /// 转换为HTML
    /// </summary>
    /// <param name="s">The current instance.</param>
    /// <param name="all">Whether to make all characters entities or just those needed.</param>
    /// <param name="replace">Whether or not to encode spaces and line breaks.</param>
    /// <returns>An HTML safe string.</returns>
    public static string ToHtmlSafe(this string s, bool all, bool replace)
    {
        if (s.IsEmptyOrWhiteSpace())
            return string.Empty;
        var entities = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 28, 29, 30, 31, 34, 39, 38, 60, 62, 123, 124, 125, 126, 127, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 215, 247, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804, 8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 338, 339, 352, 353, 376, 402, 710, 732, 8194, 8195, 8201, 8204, 8205, 8206, 8207, 8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242, 8243, 8249, 8250, 8254, 8364, 8482, 8592, 8593, 8594, 8595, 8596, 8629, 8968, 8969, 8970, 8971, 9674, 9824, 9827, 9829, 9830 };
        var sb = new StringBuilder();
        foreach (var c in s)
        {
            if (all || entities.Contains(c))
                sb.Append("&#" + ((int)c) + ";");
            else
                sb.Append(c);
        }

        return replace ? sb.Replace("", "<br />").Replace("\n", "<br />").Replace(" ", "&nbsp;").ToString() : sb.ToString();
    }

    /// <summary>
    /// 是否相等
    /// </summary>
    public static bool EquivalentTo(this string s, string whateverCaseString)
    {
        return string.Equals(s, whateverCaseString, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// 替换所有字符串
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <param name="oldValues">List of old values, which must be replaced</param>
    /// <param name="replacePredicate">Function for replacement old values</param>
    /// <returns>Returns new string with the replaced values</returns>
    /// <example>
    /// 	<code>
    ///         var str = "White Red Blue Green Yellow Black Gray";
    ///         var achromaticColors = new[] {"White", "Black", "Gray"};
    ///         str = str.ReplaceAll(achromaticColors, v => "[" + v + "]");
    ///         // str == "[White] Red Blue Green Yellow [Black] [Gray]"
    /// 	</code>
    /// </example>
    /// <remarks>
    /// </remarks>
    public static string ReplaceAll(this string value, IEnumerable<string> oldValues, Func<string, string> replacePredicate)
    {
        var sbStr = new StringBuilder(value);
        foreach (var oldValue in oldValues)
        {
            var newValue = replacePredicate(oldValue);
            sbStr.Replace(oldValue, newValue);
        }

        return sbStr.ToString();
    }

    /// <summary>
    /// 替换所有字符串
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <param name="oldValues">List of old values, which must be replaced</param>
    /// <param name="newValue">New value for all old values</param>
    /// <returns>Returns new string with the replaced values</returns> 
    public static string ReplaceAll(this string value, IEnumerable<string> oldValues, string newValue)
    {
        var sbStr = new StringBuilder(value);
        foreach (var oldValue in oldValues)
            sbStr.Replace(oldValue, newValue);

        return sbStr.ToString();
    }

    /// <summary>
    /// 替换所有字符串
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <param name="oldValues">List of old values, which must be replaced</param>
    /// <param name="newValues">List of new values</param>
    /// <returns>Returns new string with the replaced values</returns> 
    public static string ReplaceAll(this string value, IEnumerable<string> oldValues, IEnumerable<string> newValues)
    {
        var sbStr = new StringBuilder(value);
        var newValueEnum = newValues.GetEnumerator();
        foreach (var old in oldValues)
        {
            if (!newValueEnum.MoveNext())
                throw new ArgumentOutOfRangeException("newValues", "newValues sequence is shorter than oldValues sequence");
            sbStr.Replace(old, newValueEnum.Current);
        }
        if (newValueEnum.MoveNext())
            throw new ArgumentOutOfRangeException("newValues", "newValues sequence is longer than oldValues sequence");

        return sbStr.ToString();
    }

    #endregion

    #region Regex based extension methods

    /// <summary>
    /// 	Uses regular expressions to determine if the string matches to a given regex pattern.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <returns>
    /// 	<c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		var isMatching = s.IsMatchingTo(@"^\d+$");
    /// 	</code>
    /// </example>
    public static bool IsMatchingTo(this string value, string regexPattern)
    {
        return IsMatchingTo(value, regexPattern, RegexOptions.None);
    }

    /// <summary>
    /// 	Uses regular expressions to determine if the string matches to a given regex pattern.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "options">The regular expression options.</param>
    /// <returns>
    /// 	<c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		var isMatching = s.IsMatchingTo(@"^\d+$");
    /// 	</code>
    /// </example>
    public static bool IsMatchingTo(this string value, string regexPattern, RegexOptions options)
    {
        return Regex.IsMatch(value, regexPattern, options);
    }

    /// <summary>
    /// 	Uses regular expressions to replace parts of a string.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "replaceValue">The replacement value.</param>
    /// <returns>The newly created string</returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
    /// 	</code>
    /// </example>
    public static string ReplaceWith(this string value, string regexPattern, string replaceValue)
    {
        return ReplaceWith(value, regexPattern, replaceValue, RegexOptions.None);
    }

    /// <summary>
    /// 	Uses regular expressions to replace parts of a string.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "replaceValue">The replacement value.</param>
    /// <param name = "options">The regular expression options.</param>
    /// <returns>The newly created string</returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
    /// 	</code>
    /// </example>
    public static string ReplaceWith(this string value, string regexPattern, string replaceValue, RegexOptions options)
    {
        return Regex.Replace(value, regexPattern, replaceValue, options);
    }

    /// <summary>
    /// 	Uses regular expressions to replace parts of a string.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "evaluator">The replacement method / lambda expression.</param>
    /// <returns>The newly created string</returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
    /// 	</code>
    /// </example>
    public static string ReplaceWith(this string value, string regexPattern, MatchEvaluator evaluator)
    {
        return ReplaceWith(value, regexPattern, RegexOptions.None, evaluator);
    }

    /// <summary>
    /// 	Uses regular expressions to replace parts of a string.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "options">The regular expression options.</param>
    /// <param name = "evaluator">The replacement method / lambda expression.</param>
    /// <returns>The newly created string</returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
    /// 	</code>
    /// </example>
    public static string ReplaceWith(this string value, string regexPattern, RegexOptions options, MatchEvaluator evaluator)
    {
        return Regex.Replace(value, regexPattern, evaluator, options);
    }

    /// <summary>
    /// 	Uses regular expressions to determine all matches of a given regex pattern.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <returns>A collection of all matches</returns>
    public static MatchCollection GetMatches(this string value, string regexPattern)
    {
        return GetMatches(value, regexPattern, RegexOptions.None);
    }

    /// <summary>
    /// 	Uses regular expressions to determine all matches of a given regex pattern.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "options">The regular expression options.</param>
    /// <returns>A collection of all matches</returns>
    public static MatchCollection GetMatches(this string value, string regexPattern, RegexOptions options)
    {
        return Regex.Matches(value, regexPattern, options);
    }

    /// <summary>
    /// 	Uses regular expressions to determine all matches of a given regex pattern and returns them as string enumeration.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <returns>An enumeration of matching strings</returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		foreach(var number in s.GetMatchingValues(@"\d")) {
    /// 		Console.WriteLine(number);
    /// 		}
    /// 	</code>
    /// </example>
    public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern)
    {
        return GetMatchingValues(value, regexPattern, RegexOptions.None);
    }

    /// <summary>
    /// 	Uses regular expressions to determine all matches of a given regex pattern and returns them as string enumeration.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "options">The regular expression options.</param>
    /// <returns>An enumeration of matching strings</returns>
    /// <example>
    /// 	<code>
    /// 		var s = "12345";
    /// 		foreach(var number in s.GetMatchingValues(@"\d")) {
    /// 		Console.WriteLine(number);
    /// 		}
    /// 	</code>
    /// </example>
    public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern, RegexOptions options)
    {
        foreach (Match match in GetMatches(value, regexPattern, options))
        {
            if (match.Success) yield return match.Value;
        }
    }
    /// <summary>
    /// 分割并移除空元素
    /// </summary>
    /// <param name="value"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] SplitWithoutEmpty(this string value, string separator)
    {

        return value.Split(separator.ToArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// 	Uses regular expressions to split a string into parts.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <returns>The splitted string array</returns>
    public static string[] Split(this string value, string regexPattern)
    {
        return value.Split(regexPattern, RegexOptions.None);
    }


    /// <summary>
    /// 	Uses regular expressions to split a string into parts.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "regexPattern">The regular expression pattern.</param>
    /// <param name = "options">The regular expression options.</param>
    /// <returns>The splitted string array</returns>
    public static string[] Split(this string value, string regexPattern, RegexOptions options)
    {
        return Regex.Split(value, regexPattern, options);
    }
    /// <summary>
    ///     Returns a String array containing the substrings in this string that are delimited by elements of a specified
    ///     String array. A parameter specifies whether to return empty array elements.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="separator">A string that delimit the substrings in this string.</param>
    /// <param name="option">
    ///     (Optional) Specify RemoveEmptyEntries to omit empty array elements from the array returned,
    ///     or None to include empty array elements in the array returned.
    /// </param>
    /// <returns>
    ///     An array whose elements contain the substrings in this string that are delimited by the separator.
    /// </returns>
    public static string[] Split(this string @this, string separator, StringSplitOptions option = StringSplitOptions.None)
    {
        return @this.Split(new[] { separator }, option);
    }
    /// <summary>
    /// 	Splits the given string into words and returns a string array.
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <returns>The splitted string array</returns>
    public static string[] GetWords(this string value)
    {
        return value.Split(@"\W");
    }

    /// <summary>
    /// 	Gets the nth "word" of a given string, where "words" are substrings separated by a given separator
    /// </summary>
    /// <param name = "value">The string from which the word should be retrieved.</param>
    /// <param name = "index">Index of the word (0-based).</param>
    /// <returns>
    /// 	The word at position n of the string.
    /// 	Trying to retrieve a word at a position lower than 0 or at a position where no word exists results in an exception.
    /// </returns>
    /// <remarks>
    /// 	Originally contributed by MMathews
    /// </remarks>
    public static string GetWordByIndex(this string value, int index)
    {
        var words = value.GetWords();

        if ((index < 0) || (index > words.Length - 1))
            throw new IndexOutOfRangeException("The word number is out of range.");

        return words[index];
    }

    /// <summary>
    /// Removed all special characters from the string.
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <returns>The adjusted string.</returns>
    [Obsolete("Please use RemoveAllSpecialCharacters instead")]
    public static string AdjustInput(this string value)
    {
        return string.Join(null, Regex.Split(value, "[^a-zA-Z0-9]"));
    }

    /// <summary>
    /// Removed all special characters from the string.
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <returns>The adjusted string.</returns>
    /// <remarks>
    /// 	This implementation is roughly equal to the original in speed, and should be more robust, internationally.
    /// </remarks>
    public static string RemoveAllSpecialCharacters(this string value)
    {
        var sb = new StringBuilder(value.Length);
        foreach (var c in value.Where(c => Char.IsLetterOrDigit(c)))
            sb.Append(c);
        return sb.ToString();
    }


    /// <summary>
    /// Add space on every upper character
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <returns>The adjusted string.</returns>
    /// <remarks>

    /// </remarks>
    public static string SpaceOnUpper(this string value)
    {
        return Regex.Replace(value, "([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)", " $1$2").TrimStart();
    }

    #region ExtractArguments extension

    /// <summary>
    /// 比较类型
    /// </summary>
    public enum ComparsionTemplateOptions
    {
        /// <summary>
        /// Free template comparsion
        /// </summary>
        Default,

        /// <summary>
        /// Template compared from beginning of input string
        /// </summary>
        FromStart,

        /// <summary>
        /// Template compared with the end of input string
        /// </summary>
        AtTheEnd,

        /// <summary>
        /// Template compared whole with input string
        /// </summary>
        Whole,
    }

    private const RegexOptions _defaultRegexOptions = RegexOptions.None;
    private const ComparsionTemplateOptions _defaultComparsionTemplateOptions = ComparsionTemplateOptions.Default;
    private static readonly string[] _reservedRegexOperators = new[] { @"\", "^", "$", "*", "+", "?", ".", "(", ")" };

    private static string GetRegexPattern(string template, ComparsionTemplateOptions compareTemplateOptions)
    {
        template = template.ReplaceAll(_reservedRegexOperators, v => @"\" + v);

        bool comparingFromStart = compareTemplateOptions == ComparsionTemplateOptions.FromStart ||
                                  compareTemplateOptions == ComparsionTemplateOptions.Whole;
        bool comparingAtTheEnd = compareTemplateOptions == ComparsionTemplateOptions.AtTheEnd ||
                                 compareTemplateOptions == ComparsionTemplateOptions.Whole;
        var pattern = new StringBuilder();

        if (comparingFromStart)
            pattern.Append("^");

        pattern.Append(Regex.Replace(template, @"\{[0-9]+\}",
                                        delegate (Match m)
                                        {
                                            var argNum = m.ToString().Replace("{", "").Replace("}", "");
                                            return String.Format("(?<{0}>.*?)", int.Parse(argNum) + 1);
                                        }
                      ));

        if (comparingAtTheEnd || (template.LastOrDefault() == '}' && compareTemplateOptions == ComparsionTemplateOptions.Default))
            pattern.Append("$");

        return pattern.ToString();
    }

    /// <summary>
    /// 根据模板截取参数
    /// </summary>
    /// <param name="value">The input string. For example, "My name is Aleksey".</param>
    /// <param name="template">Template with arguments in the format {№}. For example, "My name is {0} {1}.".</param>
    /// <param name="compareTemplateOptions">Template options for compare with input string.</param>
    /// <param name="regexOptions">Regex options.</param>
    /// <returns>Returns parsed arguments.</returns>
    /// <example>
    /// 	<code>
    /// 		var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
    /// 		var args = str.ExtractArguments(@"My name is {1} {0}. I'm from {2}.");
    ///         // args[i] is [Nagovitsyn, Aleksey, Russia]
    /// 	</code>
    /// </example>
    /// <remarks>
    /// </remarks>
    public static IEnumerable<string> ExtractArguments(this string value, string template,
                                                       ComparsionTemplateOptions compareTemplateOptions = _defaultComparsionTemplateOptions,
                                                       RegexOptions regexOptions = _defaultRegexOptions)
    {
        return ExtractGroupArguments(value, template, compareTemplateOptions, regexOptions).Select(g => g.Value);
    }

    /// <summary>
    /// 根据模板截取参数
    /// </summary>
    /// <param name="value">The input string. For example, "My name is Aleksey".</param>
    /// <param name="template">Template with arguments in the format {№}. For example, "My name is {0} {1}.".</param>
    /// <param name="compareTemplateOptions">Template options for compare with input string.</param>
    /// <param name="regexOptions">Regex options.</param>
    /// <returns>Returns parsed arguments as regex groups.</returns>
    /// <example>
    /// 	<code>
    /// 		var str = "My name is Aleksey Nagovitsyn. I'm from Russia.";
    /// 		var groupArgs = str.ExtractGroupArguments(@"My name is {1} {0}. I'm from {2}.");
    ///         // groupArgs[i].Value is [Nagovitsyn, Aleksey, Russia]
    /// 	</code>
    /// </example>
    /// <remarks> 
    /// </remarks>
    public static IEnumerable<Group> ExtractGroupArguments(this string value, string template,
                                                           ComparsionTemplateOptions compareTemplateOptions = _defaultComparsionTemplateOptions,
                                                           RegexOptions regexOptions = _defaultRegexOptions)
    {
        var pattern = GetRegexPattern(template, compareTemplateOptions);
        var regex = new Regex(pattern, regexOptions);
        var match = regex.Match(value);

        return Enumerable.Cast<Group>(match.Groups).Skip(1);
    }

    #endregion ExtractArguments extension

    #endregion

    #region String to Enum

    /// <summary>
    ///  解析字符串为枚举
    /// </summary>
    /// <typeparam name="TEnum">The Enum type.</typeparam>
    /// <param name="dataToMatch">The data will use to convert into give enum</param>
    /// <param name="ignorecase">Whether the enum parser will ignore the given data's case or not.</param>
    /// <returns>Converted enum.</returns>
    /// <example>
    /// 	<code>
    /// 		public enum EnumTwo {  None, One,}
    /// 		object[] items = new object[] { "One".ParseStringToEnum<EnumTwo>(), "Two".ParseStringToEnum<EnumTwo>() };
    /// 	</code>
    /// </example>
    public static TEnum ParseStringToEnum<TEnum>(this string dataToMatch, bool ignorecase = default(bool))
            where TEnum : struct
    {
        return dataToMatch.IsItemInEnum<TEnum>()() ? default(TEnum) : (TEnum)Enum.Parse(typeof(TEnum), dataToMatch, default(bool));
    }

    /// <summary>
    ///  判断字符串是否在指定枚举中
    /// </summary>
    /// <typeparam name="TEnum">The enum will use to check, the data defined.</typeparam>
    /// <param name="dataToCheck">To match against enum.</param>
    /// <returns>Anonoymous method for the condition.</returns>
    public static Func<bool> IsItemInEnum<TEnum>(this string dataToCheck)
        where TEnum : struct
    {
        return () => { return string.IsNullOrEmpty(dataToCheck) || !Enum.IsDefined(typeof(TEnum), dataToCheck); };
    }

    #endregion

    private static bool StringContainsEquivalence(string inputValue, string comparisonValue)
    {
        return (inputValue.IsNotEmptyOrWhiteSpace() && inputValue.Contains(comparisonValue, StringComparison.InvariantCultureIgnoreCase));
    }

    private static bool BothStringsAreEmpty(string inputValue, string comparisonValue)
    {
        return (inputValue.IsEmptyOrWhiteSpace() && comparisonValue.IsEmptyOrWhiteSpace());
    }

    /// <summary>
    /// 向左移除字符串长度
    /// </summary>
    /// <param name="str">The string being extended</param>
    /// <param name="number_of_characters">The number of characters to remove.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static String RemoveLeft(this String str, int number_of_characters)
    {
        if (str.Length <= number_of_characters) return "";
        return str.Substring(number_of_characters);
    }

    /// <summary>
    /// 向右移除字符串长度
    /// </summary>
    /// <param name="str">The string being extended</param>
    /// <param name="number_of_characters">The number of characters to remove.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static String RemoveRight(this String str, int number_of_characters)
    {
        if (str.Length <= number_of_characters) return "";
        return str.Substring(0, str.Length - number_of_characters);
    }

    /// <summary>
    /// 解密字符串到字节数组
    /// </summary>
    /// <param name="plain_text">The string being extended and that will be encrypted.</param>
    /// <param name="password">The password to use then encrypting the string.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static byte[] EncryptToByteArray(this String plain_text, String password)
    {
        var ascii_encoder = new ASCIIEncoding();
        byte[] plain_bytes = ascii_encoder.GetBytes(plain_text);
        return CryptBytes(password, plain_bytes, true);
    }

    /// <summary>
    /// 解密字节数组到字符串
    /// </summary>
    /// <param name="encrypted_bytes">The byte array to decrypt.</param>
    /// <param name="password">The password to use when decrypting.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static String DecryptFromByteArray(this byte[] encrypted_bytes, String password)
    {
        byte[] decrypted_bytes = CryptBytes(password, encrypted_bytes, false);
        var ascii_encoder = new ASCIIEncoding();
        return new String(ascii_encoder.GetChars(decrypted_bytes));
    }

    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="plain_text">The string being extended and that will be encrypted.</param>
    /// <param name="password">The password to use then encrypting the string.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static String EncryptToString(this String plain_text, String password)
    {
        return plain_text.EncryptToByteArray(password).BytesToHexString();
    }

    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="encrypted_bytes_string">The hexadecimal string to decrypt.</param>
    /// <param name="password">The password to use then encrypting the string.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static String DecryptFromString(this String encrypted_bytes_string, String password)
    {
        return encrypted_bytes_string.HexStringToBytes().DecryptFromByteArray(password);
    }

    /// <summary>
    /// Encrypt or decrypt a byte array using the TripleDESCryptoServiceProvider crypto provider and Rfc2898DeriveBytes to build the key and initialization vector.
    /// </summary>
    /// <param name="password">The password string to use in encrypting or decrypting.</param>
    /// <param name="in_bytes">The array of bytes to encrypt.</param>
    /// <param name="encrypt">True to encrypt, False to decrypt.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    private static byte[] CryptBytes(String password, byte[] in_bytes, bool encrypt)
    {
        // Make a triple DES service provider.
        var des_provider = new TripleDESCryptoServiceProvider();

        // Find a valid key size for this provider.
        int key_size_bits = 0;
        for (int i = 1024; i >= 1; i--)
        {
            if (des_provider.ValidKeySize(i))
            {
                key_size_bits = i;
                break;
            }
        }

        // Get the block size for this provider.
        int block_size_bits = des_provider.BlockSize;

        // Generate the key and initialization vector.
        byte[] key = null;
        byte[] iv = null;
        byte[] salt = { 0x10, 0x20, 0x12, 0x23, 0x37, 0xA4, 0xC5, 0xA6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };
        MakeKeyAndIV(password, salt, key_size_bits, block_size_bits, ref key, ref iv);

        // Make the encryptor or decryptor.
        ICryptoTransform crypto_transform = encrypt
                                                ? des_provider.CreateEncryptor(key, iv)
                                                : des_provider.CreateDecryptor(key, iv);

        // Create the output stream.
        var out_stream = new MemoryStream();

        // Attach a crypto stream to the output stream.
        var crypto_stream = new CryptoStream(out_stream,
                                             crypto_transform, CryptoStreamMode.Write);

        // Write the bytes into the CryptoStream.
        crypto_stream.Write(in_bytes, 0, in_bytes.Length);
        try
        {
            crypto_stream.FlushFinalBlock();
        }
        catch (CryptographicException)
        {
            // Ignore this one. The password is bad.
        }

        // Save the result.
        byte[] result = out_stream.ToArray();

        // Close the stream.
        try
        {
            crypto_stream.Close();
        }
        catch (CryptographicException)
        {
            // Ignore this one. The password is bad.
        }
        out_stream.Close();

        return result;
    }

    /// <summary>
    /// Use the password to generate key bytes and an initialization vector with Rfc2898DeriveBytes.
    /// </summary>
    /// <param name="password">The input password to use in generating the bytes.</param>
    /// <param name="salt">The input salt bytes to use in generating the bytes.</param>
    /// <param name="key_size_bits">The input size of the key to generate.</param>
    /// <param name="block_size_bits">The input block size used by the crypto provider.</param>
    /// <param name="key">The output key bytes to generate.</param>
    /// <param name="iv">The output initialization vector to generate.</param>
    /// <remarks></remarks>
    private static void MakeKeyAndIV(String password, byte[] salt, int key_size_bits, int block_size_bits,
                                     ref byte[] key, ref byte[] iv)
    {
        var derive_bytes =
            new Rfc2898DeriveBytes(password, salt, 1234);

        key = derive_bytes.GetBytes(key_size_bits / 8);
        iv = derive_bytes.GetBytes(block_size_bits / 8);
    }

    /// <summary>
    /// 将字节数组转换为十六进制的字符串。
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static String BytesToHexString(this byte[] bytes)
    {
        String result = "";
        foreach (byte b in bytes)
        {
            result += " " + b.ToString("X").PadLeft(2, '0');
        }
        if (result.Length > 0) result = result.Substring(1);
        return result;
    }

    /// <summary>
    /// 将包含十六进制的字符串转换为字节数组。
    /// </summary>
    /// <param name="str">The hexadecimal string to convert.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static byte[] HexStringToBytes(this String str)
    {
        str = str.Replace(" ", "");
        int max_byte = str.Length / 2 - 1;
        var bytes = new byte[max_byte + 1];
        for (int i = 0; i <= max_byte; i++)
        {
            bytes[i] = byte.Parse(str.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
        }

        return bytes;
    }

    /// <summary>
    /// 如果为空字符串则返回默认值，否则返回自己
    /// </summary>
    /// <param name="s">Original String</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static string DefaultIfNullOrEmpty(this string s, string defaultValue)
    {
        return String.IsNullOrEmpty(s) ? defaultValue : s;
    }

    /// <summary>
    /// 如果为空字符串则抛出移除
    /// </summary>
    /// <param name="obj">The value to test.</param>
    /// <param name="message">The message to display if the value is null.</param>
    /// <param name="name">The name of the parameter being tested.</param>
    public static string ExceptionIfNullOrEmpty(this string obj, string message, string name)
    {
        if (String.IsNullOrEmpty(obj))
            throw new ArgumentException(message, name);
        return obj;
    }

    /// <summary>
    /// 拼接所有非空的字符串
    /// </summary>
    /// <param name="objs">The string array used for joining.</param>
    /// <param name="separator">The separator to use in the joined string.</param>
    /// <returns></returns>
    public static string JoinNotNullOrEmpty(this string[] objs, string separator)
    {
        var items = new List<string>();
        foreach (string s in objs)
        {
            if (!String.IsNullOrEmpty(s))
                items.Add(s);
        }
        return String.Join(separator, items.ToArray());
    }

    /// <summary>
    /// 解析命令行参数
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A StringDictionary type object of command line parameters.</returns>
    public static StringDictionary ParseCommandlineParams(this string[] value)
    {
        var parameters = new StringDictionary();
        var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        string parameter = null;

        // Valid parameters forms:
        // {-,/,--}param{ ,=,:}((",')value(",'))
        // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
        foreach (string txt in value)
        {
            // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
            string[] Parts = spliter.Split(txt, 3);
            switch (Parts.Length)
            {
                // Found a value (for the last parameter found (space separator))
                case 1:
                    if (parameter != null)
                    {
                        if (!parameters.ContainsKey(parameter))
                        {
                            Parts[0] = remover.Replace(Parts[0], "$1");
                            parameters.Add(parameter, Parts[0]);
                        }
                        parameter = null;
                    }
                    // else Error: no parameter waiting for a value (skipped)
                    break;
                // Found just a parameter
                case 2:
                    // The last parameter is still waiting. With no value, set it to true.
                    if (parameter != null)
                    {
                        if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
                    }
                    parameter = Parts[1];
                    break;
                // Parameter with enclosed value
                case 3:
                    // The last parameter is still waiting. With no value, set it to true.
                    if (parameter != null)
                    {
                        if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
                    }
                    parameter = Parts[1];
                    // Remove possible enclosing characters (",')
                    if (!parameters.ContainsKey(parameter))
                    {
                        Parts[2] = remover.Replace(Parts[2], "$1");
                        parameters.Add(parameter, Parts[2]);
                    }
                    parameter = null;
                    break;
            }
        }
        // In case a parameter is still waiting
        if (parameter != null)
        {
            if (!parameters.ContainsKey(parameter)) parameters.Add(parameter, "true");
        }

        return parameters;
    }

    /// <summary>
    /// 解密邮箱地址
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns></returns>
    public static string EncodeEmailAddress(this string emailAddress)
    {
        int i;
        string repl;
        string tempHtmlEncode = emailAddress;
        for (i = tempHtmlEncode.Length; i >= 1; i--)
        {
            int acode = Convert.ToInt32(tempHtmlEncode[i - 1]);
            if (acode == 32)
            {
                repl = " ";
            }
            else if (acode == 34)
            {
                repl = "\"";
            }
            else if (acode == 38)
            {
                repl = "&";
            }
            else if (acode == 60)
            {
                repl = "<";
            }
            else if (acode == 62)
            {
                repl = ">";
            }
            else if (acode >= 32 && acode <= 127)
            {
                repl = "&#" + Convert.ToString(acode) + ";";
            }
            else
            {
                repl = "&#" + Convert.ToString(acode) + ";";
            }
            if (repl.Length > 0)
            {
                tempHtmlEncode = tempHtmlEncode.Substring(0, i - 1) +
                                 repl + tempHtmlEncode.Substring(i);
            }
        }
        return tempHtmlEncode;
    }

    /// <summary>
    /// 计算提供的字符串的SHA1哈希值并返回一个基数为64的字符串。
    /// </summary>
    /// <param name="stringToHash">String that must be hashed.</param>
    /// <returns>The hashed string or null if hashing failed.</returns>
    /// <exception cref="ArgumentException">Occurs when stringToHash or key is null or empty.</exception>
    public static string GetSHA1Hash(this string stringToHash)
    {
        if (string.IsNullOrEmpty(stringToHash)) return null;
        //{
        //    throw new ArgumentException("An empty string value cannot be hashed.");
        //}

        Byte[] data = Encoding.UTF8.GetBytes(stringToHash);
        Byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(data);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// 确定字符串是否包含任何提供的值。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny(this string value, params string[] values)
    {
        return value.ContainsAny(StringComparison.CurrentCulture, values);
    }

    /// <summary>
    /// 确定字符串是否包含任何提供的值。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny(this string value, StringComparison comparisonType, params string[] values)
    {
        return values.Any(v => value.IndexOf(v, comparisonType) > -1);

    }

    /// <summary>
    /// 确定字符串是否包含所有提供的值。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAll(this string value, params string[] values)
    {
        return value.ContainsAll(StringComparison.CurrentCulture, values);
    }

    /// <summary>
    /// 确定字符串是否包含所有提供的值。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAll(this string value, StringComparison comparisonType, params string[] values)
    {
        return values.All(v => value.IndexOf(v, comparisonType) > -1);
    }

    /// <summary>
    /// 确定字符串是否等于任何提供的值。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool EqualsAny(this string value, StringComparison comparisonType, params string[] values)
    {
        return values.Any(v => value.Equals(v, comparisonType));
    }

    /// <summary>
    /// 是否符合正则表达式 
    /// </summary>
    /// <param name="value">The current <see cref="System.String"/> object</param>
    /// <param name="patterns">The array of string patterns</param>
    /// <returns></returns>
    public static bool IsLikeAny(this string value, params string[] patterns)
    {
        return patterns.Any(p => value.IsLike(p));
    }

    /// <summary>
    /// 是否符合正则表达式
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool IsLike(this string value, string pattern)
    {
        if (value == pattern) return true;

        if (pattern[0] == '*' && pattern.Length > 1)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if (value.Substring(index).IsLike(pattern.Substring(1)))
                    return true;
            }
        }
        else if (pattern[0] == '*')
        {
            return true;
        }
        else if (pattern[0] == value[0])
        {
            return value.Substring(1).IsLike(pattern.Substring(1));
        }
        return false;
    }

    /// <summary>
    /// 截断一个字符串，并添加可选的省略号
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="useElipses"></param>
    /// <returns></returns>
    public static string Truncate(this string value, int length, bool useElipses = false)
    {
        int e = useElipses ? 3 : 0;
        if (length - e <= 0) throw new InvalidOperationException(string.Format("Length must be greater than {0}.", e));

        if (string.IsNullOrEmpty(value) || value.Length <= length) return value;

        return value.Substring(0, length - e) + new String('.', e);
    }
    /// <summary>
    /// 截断一个字符串 
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <returns>A string.</returns>
    public static string Truncate(this string @this, int maxLength)
    {
        const string suffix = "...";

        if (@this == null || @this.Length <= maxLength)
        {
            return @this;
        }

        int strLength = maxLength - suffix.Length;
        return @this.Substring(0, strLength) + suffix;
    }

    /// <summary>
    /// 截断一个字符串，并添加后缀
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <param name="suffix">The suffix.</param>
    /// <returns>A string.</returns>
    public static string Truncate(this string @this, int maxLength, string suffix)
    {
        if (@this == null || @this.Length <= maxLength)
        {
            return @this;
        }

        int strLength = maxLength - suffix.Length;
        return @this.Substring(0, strLength) + suffix;
    }
    #region Z.EXT
    /// <summary>
    ///  比较两个指定对象的数值
    /// </summary>
    /// <param name="strA">The first string to compare.</param>
    /// <param name="strB">The second string to compare.</param>
    /// <returns>
    ///     An integer that indicates the lexical relationship between the two comparands.ValueCondition Less than zero
    ///     is less than . Zero  and  are equal. Greater than zero  is greater than .
    /// </returns>
    public static Int32 CompareOrdinal(this String strA, String strB)
    {
        return String.CompareOrdinal(strA, strB);
    }

    /// <summary>
    ///     Compares substrings of two specified  objects by evaluating the numeric values of the corresponding  objects
    ///     in each substring.
    /// </summary>
    /// <param name="strA">The first string to use in the comparison.</param>
    /// <param name="indexA">The starting index of the substring in .</param>
    /// <param name="strB">The second string to use in the comparison.</param>
    /// <param name="indexB">The starting index of the substring in .</param>
    /// <param name="length">The maximum number of characters in the substrings to compare.</param>
    /// <returns>
    ///     A 32-bit signed integer that indicates the lexical relationship between the two comparands.ValueCondition
    ///     Less than zero The substring in  is less than the substring in . Zero The substrings are equal, or  is zero.
    ///     Greater than zero The substring in  is greater than the substring in .
    /// </returns>
    public static Int32 CompareOrdinal(this String strA, Int32 indexA, String strB, Int32 indexB, Int32 length)
    {
        return String.CompareOrdinal(strA, indexA, strB, indexB, length);
    }
    /// <summary>
    ///  复制字符串
    /// </summary>
    /// <param name="str">The string to copy.</param>
    /// <returns>A new string with the same value as .</returns>
    public static String Copy(this String str)
    {
        return String.Copy(str);
    }
    /// <summary>
    /// 连接两个字符串
    /// </summary>
    /// <param name="str0">The first string to concatenate.</param>
    /// <param name="str1">The second string to concatenate.</param>
    /// <returns>The concatenation of  and .</returns>
    public static String Concat(this String str0, String str1)
    {
        return String.Concat(str0, str1);
    }

    /// <summary>
    ///  连接三个字符串
    /// </summary>
    /// <param name="str0">The first string to concatenate.</param>
    /// <param name="str1">The second string to concatenate.</param>
    /// <param name="str2">The third string to concatenate.</param>
    /// <returns>The concatenation of , , and .</returns>
    public static String Concat(this String str0, String str1, String str2)
    {
        return String.Concat(str0, str1, str2);
    }

    /// <summary>
    ///  连接四个字符串
    /// </summary>
    /// <param name="str0">The first string to concatenate.</param>
    /// <param name="str1">The second string to concatenate.</param>
    /// <param name="str2">The third string to concatenate.</param>
    /// <param name="str3">The fourth string to concatenate.</param>
    /// <returns>The concatenation of , , , and .</returns>
    public static String Concat(this String str0, String str1, String str2, String str3)
    {
        return String.Concat(str0, str1, str2, str3);
    }
    /// <summary>
    ///  格式化字符串
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The object to format.</param>
    /// <returns>A copy of  in which any format items are replaced by the string representation of .</returns>
    public static String Format(this String format, Object arg0)
    {
        return String.Format(format, arg0);
    }

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The first object to format.</param>
    /// <param name="arg1">The second object to format.</param>
    /// <returns>A copy of  in which format items are replaced by the string representations of  and .</returns>
    public static String Format(this String format, Object arg0, Object arg1)
    {
        return String.Format(format, arg0, arg1);
    }

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The first object to format.</param>
    /// <param name="arg1">The second object to format.</param>
    /// <param name="arg2">The third object to format.</param>
    /// <returns>
    ///     A copy of  in which the format items have been replaced by the string representations of , , and .
    /// </returns>
    public static String Format(this String format, Object arg0, Object arg1, Object arg2)
    {
        return String.Format(format, arg0, arg1, arg2);
    }

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <returns>
    ///     A copy of  in which the format items have been replaced by the string representation of the corresponding
    ///     objects in .
    /// </returns>
    public static String Format(this String format, Object[] args)
    {
        return String.Format(format, args);
    }
    /// <summary>
    ///     Retrieves the system&#39;s reference to the specified .
    /// </summary>
    /// <param name="str">A string to search for in the intern pool.</param>
    /// <returns>
    ///     The system&#39;s reference to , if it is interned; otherwise, a new reference to a string with the value of .
    /// </returns>
    public static String Intern(this String str)
    {
        return String.Intern(str);
    }
    /// <summary>
    ///     Retrieves a reference to a specified .
    /// </summary>
    /// <param name="str">The string to search for in the intern pool.</param>
    /// <returns>A reference to  if it is in the common language runtime intern pool; otherwise, null.</returns>
    public static String IsInterned(this String str)
    {
        return String.IsInterned(str);
    }

    /// <summary>
    /// 是否为空字符串，null 、''或者空格
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>true if the  parameter is null or , or if  consists exclusively of white-space characters.</returns>
    public static Boolean IsNullOrWhiteSpace(this String value)
    {
        return String.IsNullOrWhiteSpace(value);
    }
    /// <summary>
    ///   使用每个元素之间指定的分隔符连接字符串数组的所有元素。
    /// </summary>
    /// <param name="separator">
    ///     The string to use as a separator.  is included in the returned string only if  has more
    ///     than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    ///     A string that consists of the elements in  delimited by the  string. If  is an empty array, the method
    ///     returns .
    /// </returns>
    public static String Join(this String separator, String[] value)
    {
        return String.Join(separator, value);
    }

    /// <summary>
    ///   使用每个元素之间指定的分隔符连接字符串数组的所有元素。
    /// </summary>
    /// <param name="separator">
    ///     The string to use as a separator.  is included in the returned string only if  has more
    ///     than one element.
    /// </param>
    /// <param name="values">An array that contains the elements to concatenate.</param>
    /// <returns>
    ///     A string that consists of the elements of  delimited by the  string. If  is an empty array, the method
    ///     returns .
    /// </returns>
    public static String Join(this String separator, Object[] values)
    {
        return String.Join(separator, values);
    }

    /// <summary>
    /// 使用每个元素之间指定的分隔符连接数组的所有元素。
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="separator">
    ///     The string to use as a separator.  is included in the returned string only if  has more
    ///     than one element.
    /// </param>
    /// <param name="values">An array that contains the elements to concatenate.</param>
    /// <returns>A String.</returns>
    public static String Join<T>(this String separator, IEnumerable<T> values)
    {
        return String.Join(separator, values);
    }

    /// <summary>
    ///   使用每个元素之间指定的分隔符连接字符串数组的所有元素。
    /// </summary>
    /// <param name="separator">
    ///     The string to use as a separator.  is included in the returned string only if  has more
    ///     than one element.
    /// </param>
    /// <param name="values">An array that contains the elements to concatenate.</param>
    /// <returns>
    ///     A string that consists of the elements in  delimited by the  string. If  is an empty array, the method
    ///     returns .
    /// </returns>
    public static String Join(this String separator, IEnumerable<String> values)
    {
        return String.Join(separator, values);
    }

    /// <summary>
    ///  使用每个元素之间指定的分隔符连接字符串数组的所有元素。
    /// </summary>
    /// <param name="separator">
    ///     The string to use as a separator.  is included in the returned string only if  has more
    ///     than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <param name="startIndex">The first element in  to use.</param>
    /// <param name="count">The number of elements of  to use.</param>
    /// <returns>
    ///     A string that consists of the strings in  delimited by the  string. -or- if  is zero,  has no elements, or
    ///     and all the elements of  are .
    /// </returns>
    public static String Join(this String separator, String[] value, Int32 startIndex, Int32 count)
    {

        return String.Join(separator, value, startIndex, count);
    }
    #endregion

    #region Format
    /// <summary>
    ///  替换所有Html换行BR 为\r\n
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    public static string Br2Nl(this string @this)
    {
        return @this.Replace("<br />", "\r\n").Replace("<br>", "\r\n");
    }
    /// <summary>
    ///  转换XML的特殊字符
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    public static string EscapeXml(this string @this)
    {
        return @this.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
    }
    /// <summary>
    /// 替换所有\r\n 为Html换行BR 
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    public static string Nl2Br(this string @this)
    {
        return @this.Replace("\r\n", "<br />").Replace("\n", "<br />");
    }

    #endregion

    /// <summary>
    ///  组合多个字符串路径
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="paths">A variable-length parameters list containing paths.</param>
    /// <returns>
    ///     The combined paths. If one of the specified paths is a zero-length string, this method returns the other path.
    /// </returns>
    public static string PathCombine(this string @this, params string[] paths)
    {
        List<string> list = paths.ToList();
        list.Insert(0, @this);
        return Path.Combine(list.ToArray());
    }
    #region Encrypt

    #region Bytes 

    /// <summary>
    /// 转换为字节数组
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <returns>The created byte array</returns>
    public static byte[] ToBytes(this string value)
    {
        return value.ToBytes(null);
    }

    /// <summary>
    /// 转换为字节数组
    /// </summary>
    /// <param name = "value">The input string.</param>
    /// <param name = "encoding">The encoding to be used.</param>
    /// <returns>The created byte array</returns> 
    public static byte[] ToBytes(this string value, Encoding encoding)
    {
        encoding = (encoding ?? Encoding.Default);
        return encoding.GetBytes(value);
    }

    #endregion


    /// <summary>
    /// 日文编码
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static string JAPEncode(this string txt)
    {
        int[] ascLists = {92, 304, 305, 430, 431, 437, 438, 12460, 12461, 12462, 12463, 12464, 12465,
                                 12466, 12467, 12468, 12469, 12470, 12471, 12472, 12473, 12474, 12475, 12476,
                                 12477, 12478, 12479, 12480, 12481, 12482, 12483, 12485, 12486, 12487, 12488,
                                 12489, 12490, 12496, 12497, 12498, 12499, 12500, 12501, 12502, 12503, 12504,
                                 12505, 12506, 12507, 12508, 12509, 12510, 12521, 12532, 12533, 65340};

        string key = string.Empty;
        for (int i = 0; i < ascLists.Length; i++)
        {
            key = ((char)ascLists[i]).ToString();
            if (txt.IndexOf(key) >= 0)
            {
                txt = txt.Replace(key, "&#" + ascLists[i].ToString() + ";");
            }
        }

        return txt;
    }

    /// <summary>
    /// 日文解码
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static string JAPDecode(this string txt)
    {
        int[] ascLists = {92, 304, 305, 430, 431, 437, 438, 12460, 12461, 12462, 12463, 12464, 12465,
                                 12466, 12467, 12468, 12469, 12470, 12471, 12472, 12473, 12474, 12475, 12476,
                                 12477, 12478, 12479, 12480, 12481, 12482, 12483, 12485, 12486, 12487, 12488,
                                 12489, 12490, 12496, 12497, 12498, 12499, 12500, 12501, 12502, 12503, 12504,
                                 12505, 12506, 12507, 12508, 12509, 12510, 12521, 12532, 12533, 65340};

        string key = string.Empty;
        for (int i = 0; i < ascLists.Length; i++)
        {
            key = "&#" + ascLists[i].ToString() + ";";
            if (txt.IndexOf(key) >= 0)
            {
                txt = txt.Replace(key, ((char)ascLists[i]).ToString());
            }
        }

        return txt;
    }
    #endregion

    /// <summary>
    /// 根据条件截取字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>A string.</returns>
    public static string Extract(this string @this, Func<char, bool> predicate)
    {
        return new string(@this.ToCharArray().Where(predicate).ToArray());
    }
    /// <summary>
    /// 截取所有字母字符
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The extracted letter.</returns>
    public static string ExtractLetter(this string @this)
    {
        return new string(@this.ToCharArray().Where(x => Char.IsLetter(x)).ToArray());
    }
    /// <summary>
    ///  截取所有数字字符
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The extracted number.</returns>
    public static string ExtractNumber(this string @this)
    {
        return new string(@this.ToCharArray().Where(x => Char.IsNumber(x)).ToArray());
    }
    /// <summary>
    /// 移除所哟字母
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    public static string RemoveLetter(this string @this)
    {
        return new string(@this.ToCharArray().Where(x => !Char.IsLetter(x)).ToArray());
    }
    /// <summary>
    ///  移除所有变音符字符
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The string without diacritics character.</returns>
    public static string RemoveDiacritics(this string @this)
    {
        string normalizedString = @this.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char t in normalizedString)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(t);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
    /// <summary>
    ///  移除所有数字字符
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    public static string RemoveNumber(this string @this)
    {
        return new string(@this.ToCharArray().Where(x => !Char.IsNumber(x)).ToArray());
    }
    /// <summary>
    ///  根据条件移除字符并组成新字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>A string.</returns>
    public static string RemoveWhere(this string @this, Func<char, bool> predicate)
    {
        return new string(@this.ToCharArray().Where(x => !predicate(x)).ToArray());
    }
    /// <summary>
    /// 重复拼接字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="repeatCount">Number of repeats.</param>
    /// <returns>The repeated string.</returns>
    public static string Repeat(this string @this, int repeatCount)
    {
        if (@this.Length == 1)
        {
            return new string(@this[0], repeatCount);
        }

        var sb = new StringBuilder(repeatCount * @this.Length);
        while (repeatCount-- > 0)
        {
            sb.Append(@this);
        }

        return sb.ToString();
    }
    /// <summary>
    /// 替换指定位置字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="length">The length.</param>
    /// <param name="value">The value.</param>
    /// <returns>A string.</returns>
    public static string Replace(this string @this, int startIndex, int length, string value)
    {
        @this = @this.Remove(startIndex, length).Insert(startIndex, value);

        return @this;
    }

    /// <summary>
    /// 替换所有空字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="values">A variable-length parameters list containing values.</param>
    /// <returns>A string with all specified values replaced by an empty string.</returns>
    public static string ReplaceByEmpty(this string @this, params string[] values)
    {
        foreach (string value in values)
        {
            @this = @this.Replace(value, "");
        }

        return @this;
    }
    /// <summary>
    ///  替换第一次出现的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The string with the first occurence of old value replace by new value.</returns>
    public static string ReplaceFirst(this string @this, string oldValue, string newValue)
    {
        int startindex = @this.IndexOf(oldValue);

        if (startindex == -1)
        {
            return @this;
        }

        return @this.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
    }

    /// <summary>
    ///  替换第一次出现的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="number">Number of.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The string with the numbers of occurences of old value replace by new value.</returns>
    public static string ReplaceFirst(this string @this, int number, string oldValue, string newValue)
    {
        List<string> list = @this.Split(oldValue).ToList();
        int old = number + 1;
        IEnumerable<string> listStart = list.Take(old);
        IEnumerable<string> listEnd = list.Skip(old);

        return string.Join(newValue, listStart) +
               (listEnd.Any() ? oldValue : "") +
               string.Join(oldValue, listEnd);
    }
    /// <summary>
    /// 替换最后一次出现的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The string with the last occurence of old value replace by new value.</returns>
    public static string ReplaceLast(this string @this, string oldValue, string newValue)
    {
        int startindex = @this.LastIndexOf(oldValue);

        if (startindex == -1)
        {
            return @this;
        }

        return @this.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
    }

    /// <summary>
    /// 替换最后一次出现的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="number">Number of.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The string with the last numbers occurences of old value replace by new value.</returns>
    public static string ReplaceLast(this string @this, int number, string oldValue, string newValue)
    {
        List<string> list = @this.Split(oldValue).ToList();
        int old = Math.Max(0, list.Count - number - 1);
        IEnumerable<string> listStart = list.Take(old);
        IEnumerable<string> listEnd = list.Skip(old);

        return string.Join(oldValue, listStart) +
               (old > 0 ? oldValue : "") +
               string.Join(newValue, listEnd);
    }
    /// <summary>
    ///  如果与oldValue相等，则返回newValue，否则返回自己的值
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The new value if the string equal old value; Otherwise old value.</returns>
    public static string ReplaceWhenEquals(this string @this, string oldValue, string newValue)
    {
        return @this == oldValue ? newValue : @this;
    }

    /// <summary>
    /// 指定值是否和某个值排序后相同
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="otherString">The other string</param>
    /// <returns>true if the @this is anagram of the otherString, false if not.</returns>
    public static bool IsAnagram(this string @this, string otherString)
    {
        return @this
            .OrderBy(c => c)
            .SequenceEqual(otherString.OrderBy(c => c));
    }
    /// <summary>
    ///   是否不为空字符串 包括: null, '' 
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if '@this' is not (null or empty), false if not.</returns>
    public static bool IsNotNullOrEmpty(this string @this)
    {
        return !string.IsNullOrEmpty(@this);
    }
    /// <summary>
    /// 是否不为空字符串 包括: null, '', '空格'
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>true if the  parameter is null or , or if  consists exclusively of white-space characters.</returns>
    public static Boolean IsNotNullOrWhiteSpace(this string value)
    {
        return !String.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    ///  截取左边指定长度的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="length">The length.</param>
    /// <returns>A string.</returns>
    public static string LeftSafe(this string @this, int length)
    {
        return @this.Substring(0, Math.Min(length, @this.Length));
    }
    /// <summary>
    /// 截取右边指定长度的字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="length">The length.</param>
    /// <returns>A string.</returns>
    public static string RightSafe(this string @this, int length)
    {
        return @this.Substring(Math.Max(0, @this.Length - length));
    }


    /// <summary>
    /// 转换为Pascal格式字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToPascalString(this string value)
    {
        if (value.IsEmptyOrWhiteSpace()) return string.Empty;

        return Regex.Replace(value, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
    }
    /// <summary>
    /// 转换为Camel格式字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToCamelString(this string value)
    {
        if (value.IsEmptyOrWhiteSpace()) return string.Empty;
        string word = value.ToPascalString();
        return word.Substring(0, 1).ToLower() + word.Substring(1);
    }

    /// <summary>
    /// 转换为下划线'_'字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToUnderscoreString(this string value)
    {
        if (value.IsEmptyOrWhiteSpace()) return string.Empty;
        return Regex.Replace(
            Regex.Replace(
                Regex.Replace(value, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
    }

    /// <summary>
    ///  替换所有'_'和'-'为空格
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToNoUDSString(this string input)
    {
        return string.Join(" ", input.Split(new[] { '_', '-' }));
    }


    /// <summary>
    /// 过滤所有Html的标签
    /// </summary>
    /// <param name="Htmlstring"></param>
    /// <returns></returns>
    public static string ToFilterHtml(this string Htmlstring)
    {
        Htmlstring = Regex.Replace(Htmlstring, @"<script[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<noscript[\s\S]*?</noscript>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<style[\s\S]*?</style>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", " ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", " ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", " ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", " ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", " ", RegexOptions.IgnoreCase);
        return Htmlstring;
    }




    public static int[] IndexOfMany (this string self, string substr) {
        var ret = new List<int> ();
        var ch1 = self.ToCharArray ();
        var ch2 = substr.ToCharArray ();
        for (var i = 0; i <= ch1.Length - ch2.Length; i++) {
            var flag = true;
            for (var j = 0; j < ch2.Length; i++) {
                if (ch1[i + j] != ch2[j]) {
                    flag = false;
                    break;
                }
            }
            if (flag)
                ret.Add (i);
        }
        return ret.ToArray ();
    }

    public static int CountSubString (this string self, string substr) {
        return self.IndexOfMany (substr).Count ();
    }

    public static string PopFrontMatch (this string self, string str) {
        if (str.Length > self.Length)
            return self;
        else if (self.IndexOf (str) == 0)
            return self.Substring (str.Length);
        else
            return self;
    }

    public static string PopBackMatch (this string self, string str) {
        if (str.Length > self.Length)
            return self;
        else if (self.LastIndexOf (str) == self.Length - str.Length)
            return self.Substring (0, self.Length - str.Length);
        else
            return self;
    }

    public static string PopFront (this string self, int count = 1) {
        if (count > self.Length || count < 0)
            throw new IndexOutOfRangeException ();
        return self.Substring (count);
    }

    public static string PopBack (this string self, int count = 1) {
        if (count > self.Length || count < 0)
            throw new IndexOutOfRangeException ();
        return self.Substring (0, self.Length - count);
    }


}