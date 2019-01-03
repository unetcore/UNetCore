using System;
using System.Text.RegularExpressions;
/// <summary>
/// 正则表达式拓展类
/// </summary>
public static class RegexExtensions
{
    /// <summary>
    /// 是否匹配正则表达式
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
    public static Boolean IsMatch(this String input, String pattern)
    {
        return Regex.IsMatch(input, pattern);
    }

    /// <summary>
    ///  是否匹配正则表达式
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
    /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
    public static Boolean IsMatch(this String input, String pattern, RegexOptions options)
    {
        return Regex.IsMatch(input, pattern, options);
    }

    /// <summary>
    /// 获取匹配正则表达式的结果
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>An object that contains information about the match.</returns>
    public static Match Match(this String input, String pattern)
    {
        return Regex.Match(input, pattern);
    }

    /// <summary>
    ///  获取匹配正则表达式的结果
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
    /// <returns>An object that contains information about the match.</returns>
    public static Match Match(this String input, String pattern, RegexOptions options)
    {
        return Regex.Match(input, pattern, options);
    }
    /// <summary>
    ///  获取匹配正则表达式的结果列表
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>
    ///     A collection of the  objects found by the search. If no matches are found, the method returns an empty
    ///     collection object.
    /// </returns>
    public static MatchCollection Matches(this String input, String pattern)
    {
        return Regex.Matches(input, pattern);
    }

    /// <summary>
    ///    获取匹配正则表达式的结果列表
    /// </summary>
    /// <param name="input">The string to search for a match.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specify options for matching.</param>
    /// <returns>
    ///     A collection of the  objects found by the search. If no matches are found, the method returns an empty
    ///     collection object.
    /// </returns>
    public static MatchCollection Matches(this String input, String pattern, RegexOptions options)
    {
        return Regex.Matches(input, pattern, options);
    }

    #region validation

    /// <summary>
    /// 是否为邮箱格式
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <returns>true if valid email, false if not.</returns>
    public static bool IsValidEmail(this string obj)
    {
        return Regex.IsMatch(obj, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }
    /// <summary>
    ///  是否为Ip地址
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <returns>true if valid ip, false if not.</returns>
    public static bool IsValidIP(this string obj)
    {
        return Regex.IsMatch(obj, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
    }

    //邮件正则表达式
    private static Regex _emailregex = new Regex(@"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$", RegexOptions.IgnoreCase);
    //手机号正则表达式
    private static Regex _mobileregex = new Regex("^(13|15|18)[0-9]{9}$");
    //固话号正则表达式
    private static Regex _phoneregex = new Regex(@"^(\d{3,4}-?)?\d{7,8}$");
    //IP正则表达式
    private static Regex _ipregex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
    //日期正则表达式
    private static Regex _dateregex = new Regex(@"(\d{4})-(\d{1,2})-(\d{1,2})");
    //数值(包括整数和小数)正则表达式
    private static Regex _numericregex = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");
    //邮政编码正则表达式
    private static Regex _zipcoderegex = new Regex(@"^\d{6}$");

    /// <summary>
    /// 是否为邮箱名
    /// </summary>
    public static bool IsEmail(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return true;
        return _emailregex.IsMatch(s);
    }

    /// <summary>
    /// 是否为手机号
    /// </summary>
    public static bool IsMobile(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return true;
        return _mobileregex.IsMatch(s);
    }

    /// <summary>
    /// 是否为固话号
    /// </summary>
    public static bool IsPhone(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return true;
        return _phoneregex.IsMatch(s);
    }

    /// <summary>
    /// 是否为IP
    /// </summary>
    public static bool IsIP(this string s)
    {
        return _ipregex.IsMatch(s);
    }

    /// <summary>
    /// 是否是身份证号
    /// </summary>
    public static bool IsIdCard(this string id)
    {
        if (string.IsNullOrEmpty(id))
            return true;
        if (id.Length == 18)
            return CheckIDCard18(id);
        else if (id.Length == 15)
            return CheckIDCard15(id);
        else
            return false;
    }

    /// <summary>
    /// 是否为18位身份证号
    /// </summary>
    private static bool CheckIDCard18(this string Id)
    {
        long n = 0;
        if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            return false;//数字验证

        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
            return false;//省份验证

        string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
            return false;//生日验证

        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = Id.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

        int y = -1;
        Math.DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            return false;//校验码验证

        return true;//符合GB11643-1999标准
    }

    /// <summary>
    /// 是否为15位身份证号
    /// </summary>
    private static bool CheckIDCard15(this string Id)
    {
        long n = 0;
        if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            return false;//数字验证

        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
            return false;//省份验证

        string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
            return false;//生日验证

        return true;//符合15位身份证标准
    }

    /// <summary>
    /// 是否为日期
    /// </summary>
    public static bool IsDate(this string s)
    {
        return _dateregex.IsMatch(s);
    }


    /// <summary>
    /// 是否为邮政编码
    /// </summary>
    public static bool IsZipCode(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return true;
        return _zipcoderegex.IsMatch(s);
    }

    /// <summary>
    /// 是否是图片文件名
    /// </summary>
    /// <returns> </returns>
    public static bool IsImgFileName(this string fileName)
    {
        if (fileName.IndexOf(".") == -1)
            return false;

        string tempFileName = fileName.Trim().ToLower();
        string extension = tempFileName.Substring(tempFileName.LastIndexOf("."));
        return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
    }

    /// <summary>
    /// 判断一个ip是否在另一个ip内
    /// </summary>
    /// <param name="sourceIP">检测ip</param>
    /// <param name="targetIP">匹配ip</param>
    /// <returns></returns>
    public static bool InIP(this string sourceIP, string targetIP)
    {
        if (string.IsNullOrEmpty(sourceIP) || string.IsNullOrEmpty(targetIP))
            return false;

        string[] sourceIPBlockList = sourceIP.Split(@".");
        string[] targetIPBlockList = targetIP.Split(@".");

        int sourceIPBlockListLength = sourceIPBlockList.Length;

        for (int i = 0; i < sourceIPBlockListLength; i++)
        {
            if (targetIPBlockList[i] == "*")
                return true;

            if (sourceIPBlockList[i] != targetIPBlockList[i])
            {
                return false;
            }
            else
            {
                if (i == 3)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断一个ip是否在另一个ip内
    /// </summary>
    /// <param name="sourceIP">检测ip</param>
    /// <param name="targetIPList">匹配ip列表</param>
    /// <returns></returns>
    public static bool InIPList(this string sourceIP, string[] targetIPList)
    {
        if (targetIPList != null && targetIPList.Length > 0)
        {
            foreach (string targetIP in targetIPList)
            {
                if (InIP(sourceIP, targetIP))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断一个ip是否在另一个ip内
    /// </summary>
    /// <param name="sourceIP">检测ip</param>
    /// <param name="targetIPStr">匹配ip</param>
    /// <returns></returns>
    public static bool InIPList(this string sourceIP, string targetIPStr)
    {
        string[] targetIPList = targetIPStr.Split("\n");
        return InIPList(sourceIP, targetIPList);
    }

    /// <summary>
    /// 是否为字母a-zA-Z
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if Alpha, false if not.</returns>
    public static bool IsAlpha(this string @this)
    {
        return !Regex.IsMatch(@this, "[^a-zA-Z]");
    }
    /// <summary>
    /// 是否为字母数字：a-zA-Z0-9
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if Alphanumeric, false if not.</returns>
    public static bool IsAlphaNumeric(this string @this)
    {
        return !Regex.IsMatch(@this, "[^a-zA-Z0-9]");
    }
    /// <summary>
    ///  是否为数字0-9
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if numeric, false if not.</returns>
    public static bool IsNumeric(this string @this)
    {
        return !Regex.IsMatch(@this, "[^0-9]");
    }


    /// <summary>
    /// 是否是数值(包括整数和小数)
    /// </summary>
    public static bool IsNumericArray(this string[] numericStrList)
    {
        if (numericStrList != null && numericStrList.Length > 0)
        {
            foreach (string numberStr in numericStrList)
            {
                if (!IsNumeric(numberStr))
                    return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否是数值(包括整数和小数)
    /// </summary>
    public static bool IsNumericRule(this string numericRuleStr, string splitChar)
    {
        return IsNumericArray(numericRuleStr.Split(splitChar));
    }

    /// <summary>
    /// 是否是数值(包括整数和小数)
    /// </summary>
    public static bool IsNumericRule(this string numericRuleStr)
    {
        return IsNumericRule(numericRuleStr, ",");
    }

    /// <summary>
    /// 是否有效Base64字符串
    /// </summary>
    /// <param name="base64String"></param>
    /// <returns></returns>
    public static bool IsValidBase64String(this string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
            return true;

        foreach (char c in base64String)
        {
            if (c >= 'a' && c <= 'z')
                continue;
            if (c >= 'A' && c <= 'Z')
                continue;
            if (c >= '0' && c <= '9')
                continue;
            if (c == '=' || c == '+' || c == '/')
                continue;
            return false;
        }

        return true;
    }
    #endregion

}