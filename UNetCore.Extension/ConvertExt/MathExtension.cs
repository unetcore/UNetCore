
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
/// <summary>
/// MathExtension
/// </summary>
public static class MathExtension
{
    /// <summary>
    /// 获取中位数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static double Median<T>(this T[] array) where T : IComparable<T>
    {
        Guard.ArgumentNull(array, "array", null);
        if (array.Length == 0)
        {
            return 0.0;
        }
        int index = array.Length / 2;
        T[] localArray = (from s in array
                          orderby s
                          select s).ToArray<T>();
        if ((array.Length % 2) == 0)
        {
            return ((localArray[index - 1].To<T, double>(0.0) + localArray[index].To<T, double>(0.0)) / 2.0);
        }
        return localArray[index].To<T, double>(0.0);
    }
    /// <summary>
    /// 四舍五入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <param name="roundType"></param>
    /// <returns></returns>
    public static double Round<T>(this T value, int decimals = 2, RoundType roundType = RoundType.FourFive)
    {
        string[] strArray = value.ToString().Split(new char[] { '.' });
        if (((decimals <= 0) || (strArray.Length < 2)) || (strArray[1].Length <= decimals))
        {
            return value.To<T, double>(0.0);
        }
        string str = strArray[1].Substring(0, decimals);
        int num = int.Parse(strArray[1].Substring(decimals, 1));
        double num2 = Convert.ToDouble(strArray[0] + "." + str);
        if ((roundType != RoundType.None) && (num >= 5))
        {
            string str2 = "0." + new string('0', decimals - 1) + "1";
            num2 += Convert.ToDouble(str2);
        }
        return num2;
    }
    /// <summary>
    /// 获取方差
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="weightFunc"></param>
    /// <returns></returns>
    public static double Variance<T>(this T[] array, Func<T, double, double> weightFunc = null) where T : IComparable<T>
    {
        Guard.ArgumentNull(array, "array", null);
        if (array.Length == 0)
        {
            return 0.0;
        }
        double d = 0.0;
        double num2 = array.Average<T>((Func<T, double>)(s => s.To<T, double>(0.0)));
        foreach (T local in array)
        {
            double x = local.To<T, double>(0.0) - num2;
            if (weightFunc != null)
            {
                x += weightFunc(local, num2);
            }
            d += Math.Pow(x, 2.0);
        }
        return Math.Sqrt(d);
    }
}

