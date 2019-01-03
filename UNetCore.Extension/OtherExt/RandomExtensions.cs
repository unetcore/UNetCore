using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
/// <summary>
/// RandomExtensions
/// </summary>
public static class RandomExtensions
{

    /// <summary>
    /// 随机获取指定长度列表项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IList<T> GetRandItem<T>(this T[] source, int length = 1, bool isUnique = false)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < length; i++)
        {
            T item = source.GetRandItem();
            if (isUnique == true)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                    var listSource = source.ToList();
                    listSource.Where(p => p.Equals(item)).ToList().ForEach((t) => listSource.Remove(t));
                    source = listSource.ToArray();
                }
            }
            else
            {
                list.Add(item);

            }
        }
        return list;
    }

    /// <summary>
    /// 生成指定长度和分隔符，且不存在同一字符的字符串
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="length"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string GetRandItemUniqueString(this string pool, int length = 1, string separator = "")
    {
        return string.Join(separator, pool.ToArray().GetRandItem(length, true));
    }

    public static Random NewRandom()
    {
        //return new Random(unchecked((int)DateTime.Now.Ticks)); 
        return new Random(new RNGCryptoServiceProvider().GetHashCode());
    }

    /// <summary>
    /// 随机获取项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T GetRandItem<T>(this T[] source)
    {
        T[] list = ShuffleEx(source);
        Random rnd = NewRandom();
        int index = rnd.Next(0, list.Length);
        return list[index];
    }
    /// <summary>
    /// 随机获取不在指定范围的项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="notInRand"></param>
    /// <returns></returns>
    public static T GetRandItem<T>(this T[] source, params T[] notInRand)
    {
        List<T> newPool = new List<T>();

        foreach (T item in source)
        {
            if (!notInRand.Contains(item))
            {
                newPool.Add(item);
            }
        }
        return GetRandItem<T>(newPool.ToArray());
    }

    /// <summary>
    /// 根据权重随机获取项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="notInRand"></param>
    /// <returns></returns>
    public static T GetRandItemWithWeight<T>(this T[] source, double[] weights)
    {
        if (source.Length != weights.Length)
        {
            throw new Exception("Chance: length of array and weights must match");
        }

        // scan weights array and sum valid entries
        double sum = 0;
        double val;
        for (var weightIndex = 0; weightIndex < weights.Length; weightIndex++)
        {
            val = weights[weightIndex];
            if (val > 0)
            {
                sum += val;
            }
        }

        if (sum == 0)
        {
            throw new Exception("Chance: no valid entries in array weights");
        }

        // select a value within range
        var selected = Roll() * sum;

        // find array entry corresponding to selected value
        double total = 0;
        var lastGoodIdx = -1;
        int chosenIdx = 0;
        for (int weightIndex = 0; weightIndex < weights.Length; weightIndex++)
        {
            val = weights[weightIndex];
            total += val;
            if (val > 0)
            {
                if (selected <= total)
                {
                    chosenIdx = weightIndex;
                    break;
                }
                lastGoodIdx = weightIndex;
            }

            // handle any possible rounding error comparison to ensure something is picked
            if (weightIndex == (weights.Length - 1))
            {
                chosenIdx = lastGoodIdx;
            }
        }

        var chosen = source[chosenIdx];

        return chosen;

    }

    /// <summary>
    /// 随机获取字符串中的字符
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static char GetRandItem(this string source)
    {
        if (!string.IsNullOrEmpty(source))
        {
            char[] list = source.ToCharArray();
            return GetRandItem<char>(list);
        }
        else
        {
            throw new ArgumentException("source不能为空");
        }
    }
    /// <summary>
    /// 获取指定长度的随机字符串
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GetRandItem(this string pool, int length)
    {
        StringBuilder sb = new StringBuilder();

        if (!string.IsNullOrEmpty(pool))
        {
            for (int i = 0; i < length; i++)
            {
                sb.Append(GetRandItem(pool));
            }

            return sb.ToString();
        }
        else
        {
            //throw new ArgumentException("pool不能为空");
        }
        return sb.ToString();
    }

    /// <summary>
    /// 随机返回一个0 - 1的数
    /// </summary>
    /// <returns></returns>
    public static double Roll()
    {
        System.Random random = NewRandom(); //new Random(new RNGCryptoServiceProvider().GetHashCode());
        return random.NextDouble();
    }

    /// <summary>
    /// 随机计算概率是否命中
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static bool HitTestInRand(double probability)
    {
        if (probability == 1 || probability > 1)
        {
            return true;
        }
        if (probability == 0 || probability < 0)
        {
            return false;
        }
        //if (probability > 1 || probability < 0)
        //{
        //    throw new ArgumentException("probability参数确保在0~1之间");
        //}

        double roll = Roll();
        if (roll <= probability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 随机获取范围数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int GetRandItemInRange(int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException("max参数不能小于min参数");
        }
        System.Random random = NewRandom(); //new Random(new RNGCryptoServiceProvider().GetHashCode());
        return random.Next(min, max + 1);
       
    }
    /// <summary>
    /// 随机索取指定范围内的double值
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="decimalPlace"></param>
    /// <returns></returns>
    public static double GetRandItemInRangeDouble(double min, double max, int decimalPlace)
    {
        double temp = Math.Pow(10, decimalPlace); //计算相该数的位数
        System.Random random = NewRandom(); //new Random(new RNGCryptoServiceProvider().GetHashCode());
        return (double)random.Next((int)Math.Floor(min * temp), (int)Math.Floor(max * temp)) / temp;
    }
    /// <summary>
    /// 随机索取指定范围内的float值
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="decimalPlace"></param>
    /// <returns></returns>
    public static float GetRandItemInRangeFloat(double min, double max, int decimalPlace)
    {
        float temp = (float)Math.Pow(10, decimalPlace); //计算相该数的位数
        System.Random random = NewRandom(); //new Random(new RNGCryptoServiceProvider().GetHashCode());
        return (float)random.Next((int)Math.Floor(min * temp), (int)Math.Floor(max * temp)) / temp;
    }

    /// <summary>
    /// 利用时间依赖的Random 随机排序 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="size"></param>
    /// <returns></returns>
    public static T[] Shuffle<T>(this T[] source)
    {
        T[] list = source;
        System.Random random = NewRandom();
        int var;
        T temp;
        for (int i = 0; i < list.Length; ++i)
        {
            var = random.Next(0, list.Length);
            temp = list[i];
            list[i] = list[var];
            list[var] = temp;
        }

        return list;
    }

    /// <summary>
    /// 利用RNGCryptoServiceProvider 随机排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T[] ShuffleEx<T>(this T[] source)
    {
        T[] list = source;
        System.Random random = NewRandom(); //new Random(new RNGCryptoServiceProvider().GetHashCode());
        int var;
        T temp;
        for (int i = 0; i < list.Length; ++i)
        {
            var = random.Next(0, list.Length);
            temp = list[i];
            list[i] = list[var];
            list[var] = temp;
        }

        return list;
    }


    /// <summary>
    /// 利用RNGCryptoServiceProvider 随机排序
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IList<T> ShufflePro<T>(this IList<T> source)
    {
        IList<T> list = source;
        int var, randomSeed;
        T temp;
        byte[] randomBytes = new Byte[4];
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        for (int i = 0; i < list.Count; ++i)
        {
            rng.GetNonZeroBytes(randomBytes);
            randomSeed = (randomBytes[0] << 24) | (randomBytes[1] << 16) | (randomBytes[2] << 8) | randomBytes[3];
            var = randomSeed % list.Count;
            //var = System.Math.Abs(var);
            if (var < 0) var *= -1;
            temp = list[i];
            list[i] = list[var];
            list[var] = temp;
        }
        return list;
    }


}
