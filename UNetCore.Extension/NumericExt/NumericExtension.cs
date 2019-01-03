
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
using System.Text.RegularExpressions;

    public static class NumericExtension
    {
        public static decimal GetDecimalPart(this decimal value)
        {
            return ((double) value).GetDecimalPart();
        }

        public static decimal GetDecimalPart(this double value)
        {
            string source = value.ToString();
            if (!source.IsNumeric())
            {
                return 0M;
            }
            int index = source.IndexOf(".");
            if (index == -1)
            {
                return 0M;
            }
            return Convert.ToDecimal(source.Substring(index));
        }

        public static int GetIntegerPart(this decimal value)
        {
            return ((double) value).GetIntegerPart();
        }

        public static int GetIntegerPart(this double value)
        {
            string source = value.ToString();
            if (!source.IsNumeric())
            {
                return 0;
            }
            int index = source.IndexOf(".");
            if (index == -1)
            {
                return value.To<double, int>(0);
            }
            return Convert.ToInt32(source.Substring(0, index));
        }

        private static void ParseMoneySection(StringBuilder output, bool isJiaoFen, string digit, ref long value, ref bool isAllZero, ref bool isPreZero)
        {
            string[] strArray = isJiaoFen ? new string[] { "分", "角" } : new string[] { "", "拾", "佰", "仟" };
            isAllZero = true;
            for (int i = 0; (i < strArray.Length) && (value > 0L); i++)
            {
                int num2 = (int) (value % 10L);
                if (num2 != 0)
                {
                    if (isPreZero && !isAllZero)
                    {
                        output.Append(digit[0]);
                    }
                    output.AppendFormat("{0}{1}", strArray[i], digit[num2]);
                    isAllZero = false;
                }
                isPreZero = num2 == 0;
                value /= 10L;
            }
        }

        private static string ReverseMoneyString(StringBuilder output)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = output.Length - 1; i >= 0; i--)
            {
                builder.Append(output[i]);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 转换为大写金额
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToUpperMoney(this decimal money)
        {
            return ((double)money).ToUpperMoney();
        }
        /// <summary>
        /// 转换为大写金额
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToUpperMoney2(this double money)
        {
            long num;
            if (!money.ToString().IsNumeric())
            {
                return string.Empty;
            }
            try
            {
                num = (long) (money.To<double>(0.0) * 100.0);
            }
            catch
            {
                throw new OverflowException();
            }
            switch (num)
            {
                case -9223372036854775808L:
                    throw new OverflowException();

                case 0L:
                    return "零元";
            }
            bool isAllZero = true;
            bool isPreZero = true;
            StringBuilder output = new StringBuilder();
            string[] strArray = new string[] { "元", "万", "亿", "万", "亿亿" };
            long num2 = Math.Abs(num);
            ParseMoneySection(output, true, "零壹贰叁肆伍陆柒捌玖", ref num2, ref isAllZero, ref isPreZero);
            for (int i = 0; (i < strArray.Length) && (num2 > 0L); i++)
            {
                if (isPreZero && !isAllZero)
                {
                    output.Append("零壹贰叁肆伍陆柒捌玖"[0]);
                }
                if ((i == 4) && output.ToString().EndsWith(strArray[2]))
                {
                    output.Remove(output.Length - strArray[2].Length, strArray[2].Length);
                }
                output.Append(strArray[i]);
                ParseMoneySection(output, false, "零壹贰叁肆伍陆柒捌玖", ref num2, ref isAllZero, ref isPreZero);
                if (((i % 2) == 1) && isAllZero)
                {
                    output.Remove(output.Length - strArray[i].Length, strArray[i].Length);
                }
            }
            if (num < 0L)
            {
                output.Append("负");
            }
            return ReverseMoneyString(output);
        }

        /// <summary>
        /// 转换为中文大写金额
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToUpperMoney(this double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        } 
    }
