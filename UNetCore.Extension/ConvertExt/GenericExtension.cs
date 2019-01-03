
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// 通用拓展
/// </summary>
public static class GenericExtension
{
    #region IsAssignableFrom
    /// <summary>
    /// 是否继承某个类型
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if assignable from, false if not.</returns>
    public static bool IsAssignableFrom<T>(this object @this)
    {
        Type type = @this.GetType();
        return type.IsAssignableFrom(typeof(T));
    }

    /// <summary>
    ///  是否继承某个类型
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <returns>true if assignable from, false if not.</returns>
    public static bool IsAssignableFrom(this object @this, Type targetType)
    {
        Type type = @this.GetType();
        return type.IsAssignableFrom(targetType);
    }
    #endregion

    #region As or default
    /// <summary>
    /// 强制转换为指定类型，如果出错则返回默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A T.</returns>
    public static T AsOrDefault<T>(this object @this)
    {
        try
        {
            return (T)@this;
        }
        catch (Exception)
        {
            return default(T);
        }
    }

    /// <summary>
    /// 强制转换为指定类型，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>A T.</returns>
    public static T AsOrDefault<T>(this object @this, T defaultValue)
    {
        try
        {
            return (T)@this;
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 强制转换为指定类型，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param> 
    public static T AsOrDefault<T>(this object @this, Func<T> defaultValueFactory)
    {
        try
        {
            return (T)@this;
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///  强制转换为指定类型，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param> 
    public static T AsOrDefault<T>(this object @this, Func<object, T> defaultValueFactory)
    {
        try
        {
            return (T)@this;
        }
        catch (Exception)
        {
            return defaultValueFactory(@this);
        }
    }
    #endregion
    /// <summary>
    /// 将对象按类型转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T As<T>(this object obj) where T : class
    {
        return (obj as T);
    }
    /// <summary>
    /// 将对象按类型转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static T As<T>(this object obj, Action<T> action, Action other = null) where T : class
    {
        T local = obj as T;
        if ((local != null) && (action != null))
        {
            action(local);
            return local;
        }
        if (other != null)
        {
            other();
        }
        return local;
    }
    /// <summary>
    /// 断言不为空
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="value"></param>
    /// <param name="action"></param>
    public static void AssertNotNull<TSource>(this TSource value, Action<TSource> action)
    {
        if ((value != null) && (action != null))
        {
            action(value);
        }
    }
    /// <summary>
    /// 断言不为空
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="value"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TReturn AssertNotNull<TSource, TReturn>(this TSource? value, Func<TSource, TReturn> func) where TSource : struct
    {
        if (value.HasValue && (func != null))
        {
            return func(value.Value);
        }
        return default(TReturn);
    }
    /// <summary>
    /// 断言不为空
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="value"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TReturn AssertNotNull<TSource, TReturn>(this TSource value, Func<TSource, TReturn> func) where TSource : class
    {
        if ((value != null) && (func != null))
        {
            return func(value);
        }
        return default(TReturn);
    }

    private static bool FromConvertible<TTarget>(IConvertible converter, out TTarget result2)
    {
        object result = null;
        bool hasChange = false;
        switch (Type.GetTypeCode(typeof(TTarget)))
        {
            case TypeCode.Boolean:
                result = converter.ToBoolean(CultureInfo.CurrentCulture);
                hasChange = true;
                break;
            case TypeCode.Char:
                result = converter.ToChar(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.SByte:
                result = converter.ToSByte(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Byte:
                result = converter.ToByte(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Int16:
                result = converter.ToInt16(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.UInt16:
                result = converter.ToUInt16(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Int32:
                result = converter.ToInt32(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.UInt32:
                result = converter.ToUInt32(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Int64:
                result = converter.ToInt64(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.UInt64:
                result = converter.ToUInt64(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Single:
                result = converter.ToSingle(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Double:
                result = converter.ToDouble(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.Decimal:
                result = converter.ToDecimal(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.DateTime:
                result = converter.ToDateTime(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

            case TypeCode.String:
                result = converter.ToString(CultureInfo.CurrentCulture);
                hasChange = true;
                break;

        }
        result2 = result == null ? default(TTarget) : (TTarget)result;
        return hasChange;
    }


    /// <summary>
    /// 判断是否某个类型元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool Is<T>(this object obj)
    {
        return (obj is T);
    }
    /// <summary>
    /// 判断是否在集合里面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static bool IsIn<T>(this T obj, params T[] arr)
    {
        return arr.Contains(obj);
    }
    /// <summary>
    /// 判断是否在某个元素范围里面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="lowerBound"></param>
    /// <param name="upperBound"></param>
    /// <param name="comparer"></param>
    /// <param name="includeLowerBound"></param>
    /// <param name="includeUpperBound"></param>
    /// <returns></returns>
    public static bool IsBetween<T>(this T value, T lowerBound, T upperBound, IComparer<T> comparer, bool includeLowerBound = false, bool includeUpperBound = false)
    {
        Guard.ArgumentNull(comparer, "comparer", null);
        int num = comparer.Compare(value, lowerBound);
        int num2 = comparer.Compare(value, upperBound);
        if ((!includeLowerBound || (num != 0)) && (!includeUpperBound || (num2 != 0)))
        {
            return ((num > 0) && (num2 < 0));
        }
        return true;
    }

    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this object source)
    {
        if ((source != null) && !(source is DBNull))
        {
            return string.IsNullOrEmpty(source.ToString());
        }
        return true;
    }
    /// <summary>
    /// 判断是否为空字符串
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);

    }
    /// <summary>
    /// 判断是否为空字符串
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string source)
    {
        return string.IsNullOrWhiteSpace(source);

    }
    private static object[] ReadObjectValues<TS, TO>(TS source, TO other, IEnumerable<PropertyInfo> sourceProperties, IEnumerable<PropertyInfo> otherProperties)
    {
        ArrayList list = new ArrayList();
        foreach (PropertyInfo info in sourceProperties)
        {
            list.Add(info.GetValue(source, null));
        }
        foreach (PropertyInfo info2 in otherProperties)
        {
            list.Add(info2.GetValue(other, null));
        }
        return list.ToArray();
    }
    #region 类型转换
    /// <summary>
    /// 将数组转换为指定类型的数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static TTarget[] ToAll<T, TTarget>(this T[] arr)
    {
        if (arr.IsNull())
        {
            return null;
        }
        return Array.ConvertAll<T, TTarget>(arr, s => s.To<TTarget>());
    }
    /// <summary>
    /// 将迭代器转换为指定类型的迭代器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static IEnumerable<TTarget> ToAll<T, TTarget>(this IEnumerable<T> arr)
    {
        if (arr.IsNull())
        {
            return null;
        }
        return Array.ConvertAll<T, TTarget>(arr.ToArray(), s => s.To<TTarget>()).AsEnumerable();
    }
    /// <summary>
    /// 将列表转换为指定类型的列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static IList<TTarget> ToAll<T, TTarget>(this IList<T> arr)
    {
        if (arr.IsNull())
        {
            return null;
        }
        return Array.ConvertAll<T, TTarget>(arr.ToArray(), s => s.To<TTarget>()).ToList();
    }
    #endregion

    /// <summary>
    /// 转换到指定类型值
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TTarget To<TTarget>(this object value)
    {
        if (value.IsNullOrEmpty())
        {
            return default(TTarget);

        }
        else
        {
            return (TTarget)value.ToType(typeof(TTarget), default(TTarget));

        }
    }
    /// <summary>
    /// 转换到指定类型值
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TTarget To<TTarget>(this object value, TTarget defaultValue)
    {
        if (value.IsNullOrEmpty())
        {
            return defaultValue;

        }
        else
        {
            return (TTarget)value.ToType(typeof(TTarget), defaultValue);
        }
    }
    /// <summary>
    /// 转换到指定类型值
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="source"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TTarget To<TSource, TTarget>(this TSource source, TTarget defaultValue)
    {
        if (source.IsNullOrEmpty())
        {
            return defaultValue;

        }
        else
        {

            TTarget local;
            IConvertible converter = source.As<IConvertible>();
            if ((converter != null) && FromConvertible<TTarget>(converter, out local))
            {
                return local;
            }
            return (TTarget)source.ToType(typeof(TTarget), defaultValue);
        }
    }

    /// <summary>
    /// 转换到指定类型值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="conversionType"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static object ToType(this object value, Type conversionType, object defaultValue = null)
    {
        Guard.ArgumentNull(conversionType, "conversionType", null);
        if (value.IsNullOrEmpty())
        {
            if (!conversionType.IsNullOrEmpty() && (defaultValue == null))
            {
                return conversionType.GetTypeDefaultValue();
            }
            return null;
        }
        if (value.GetType() == conversionType)
        {
            return value;
        }
        try
        {
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString(), true);
            }
            if ((conversionType == typeof(bool?)) && (Convert.ToInt32(value) == -1))
            {
                return null;
            }
            if (conversionType.IsNullOrEmpty())
            {

                if (value == null || value.IsNullOrEmpty())
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                //将convertsionType转换为nullable对的基础基元类型
                conversionType = nullableConverter.UnderlyingType;

                return value == null ? Activator.CreateInstance(conversionType) : Convert.ChangeType(value, conversionType);

            }
            if (conversionType == typeof(bool))
            {
                if (value is string)
                {
                    string str = ((string)value).ToLower();
                    return ((((str == "true") || (str == "t")) || ((str == "1") || (str == "yes"))) ? ((object)1) : ((object)(str == "on")));
                }
                return (Convert.ToInt32(value) == 1);
            }
            if (value is bool)
            {
                if (conversionType == typeof(string))
                {
                    return (Convert.ToBoolean(value) ? "true" : "false");
                }
                return (Convert.ToBoolean(value) ? 1 : 0);
            }
            if (conversionType == typeof(Type))
            {
                return Type.GetType(value.ToString(), false, true);
            }
            if ((value is Type) && (conversionType == typeof(string)))
            {
                return ((Type)value).FullName;
            }
            if (typeof(IConvertible).IsAssignableFrom(conversionType))
            {
                return Convert.ChangeType(value, conversionType, null);
            }
            return value;
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }
    /// <summary>
    /// 尝试释放对象资源
    /// </summary>
    /// <param name="disobj"></param>
    public static void TryDispose(this object disobj)
    {
        IDisposable disposable = disobj as IDisposable;
        if (disposable != null)
        {
            disposable.Dispose();
        }
    }


    #region value comparsion
    /// <summary>
    ///  判断值是否在指定区间范围内
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
    public static bool Between<T>(this T @this, T minValue, T maxValue) where T : IComparable<T>
    {
        return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
    }
    /// <summary>
    ///  判断值是否在指定数组内
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The object to be compared.</param>
    /// <param name="values">The value list to compare with the object.</param>
    /// <returns>true if the values list contains the object, else false.</returns>
    public static bool In<T>(this T @this, params T[] values)
    {
        return Array.IndexOf(values, @this) != -1;
    }
    /// <summary>
    ///  判断值是否在指定区间范围内
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
    public static bool InRange<T>(this T @this, T minValue, T maxValue) where T : IComparable<T>
    {
        return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
    }
    /// <summary>
    /// 判断值是否数据库空类型
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="value">An object.</param>
    /// <returns>true if  is of type ; otherwise, false.</returns>
    public static Boolean IsDBNull<T>(this T value) where T : class
    {
        return Convert.IsDBNull(value);
    }

    /// <summary>
    /// 判断值是否默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="source">The source to act on.</param>
    /// <returns>true if default, false if not.</returns>
    public static bool IsDefault<T>(this T source)
    {
        return source.Equals(default(T));
    }

    /// <summary>
    /// 判断值是否不在指定区间范围内
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The object to be compared.</param>
    /// <param name="values">The value list to compare with the object.</param>
    /// <returns>true if the values list doesn't contains the object, else false.</returns>
    public static bool NotIn<T>(this T @this, params T[] values)
    {
        return Array.IndexOf(values, @this) == -1;
    }

    #endregion


    /// <summary>
    /// 链式动作拓展
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="action">The action.</param>
    /// <returns>The @this acted on.</returns>
    public static T Chain<T>(this T @this, Action<T> action)
    {
        action(@this);

        return @this;
    }
    /// <summary>
    /// 深拷贝数据
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>the copied object.</returns>
    public static T DeepClone<T>(this T @this)
    {
        IFormatter formatter = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, @this);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
    /// <summary>
    /// 浅拷贝数据
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A T.</returns>
    public static T ShallowCopy<T>(this T @this)
    {
        MethodInfo method = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)method.Invoke(@this, null);
    }


    #region To

    /// <summary>
    /// 转换为指定类型
    /// </summary>
    /// <param name="this"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object To(this Object @this, Type type)
    {
        if (@this != null)
        {
            Type targetType = type;

            if (@this.GetType() == targetType)
            {
                return @this;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(@this);
            if (converter != null)
            {
                if (converter.CanConvertTo(targetType))
                {
                    return converter.ConvertTo(@this, targetType);
                }
            }

            converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null)
            {
                if (converter.CanConvertFrom(@this.GetType()))
                {
                    return converter.ConvertFrom(@this);
                }
            }

            if (type.IsNullOrEmpty())
            {

                if (@this == null || @this.IsNullOrEmpty())
                {
                    return null;
                }

                NullableConverter nullableConverter = new NullableConverter(type);
                type = nullableConverter.UnderlyingType;

                return @this == null ? Activator.CreateInstance(type) : Convert.ChangeType(@this, type);

            }

            if (@this == DBNull.Value)
            {
                return null;
            }
        }

        return @this;
    }

    /// <summary>
    /// 获取可空类型中的基本类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetNotNullableType(this Type type)
    {
        if (type.IsNullOrEmpty())
        {
            NullableConverter nullableConverter = new NullableConverter(type);
            type = nullableConverter.UnderlyingType;
            return type;
        }

        return type;
    }

    /// <summary>
    ///     A System.Object extension method that converts this object to an or default.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">this.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a T.</returns> 
    public static T ToOrDefault<T>(this Object @this, Func<object, T> defaultValueFactory)
    {
        try
        {
            if (@this != null)
            {
                Type targetType = typeof(T);

                if (@this.GetType() == targetType)
                {
                    return (T)@this;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(@this);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return (T)converter.ConvertTo(@this, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return (T)converter.ConvertFrom(@this);
                    }
                }

                if (@this == DBNull.Value)
                {
                    return (T)(object)null;
                }
            }

            return (T)@this;
        }
        catch (Exception)
        {
            return defaultValueFactory(@this);
        }
    }

    /// <summary>
    /// 转换为指定类型，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">this.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a T.</returns>
    public static T ToOrDefault<T>(this Object @this, Func<T> defaultValueFactory)
    {
        return @this.ToOrDefault(x => defaultValueFactory());
    }

    /// <summary>
    ///  转换为指定类型，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">this.</param>
    /// <returns>The given data converted to a T.</returns>
    public static T ToOrDefault<T>(this Object @this)
    {
        return @this.ToOrDefault(x => default(T));
    }

    /// <summary>
    /// 转换为指定类型，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">this.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a T.</returns>
    public static T ToOrDefault<T>(this Object @this, T defaultValue)
    {
        return @this.ToOrDefault(x => defaultValue);
    }
    #endregion

    #region Get value or default
    /// <summary>
    ///  获取指定类型的值，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <returns>The value or default.</returns>
    public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func)
    {
        try
        {
            return func(@this);
        }
        catch (Exception)
        {
            return default(TResult);
        }
    }

    /// <summary>
    ///  获取指定类型的值，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The value or default.</returns>
    public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue)
    {
        try
        {
            return func(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///  获取指定类型的值，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The value or default.</returns>
 
    public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory)
    {
        try
        {
            return func(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///  获取指定类型的值，如果出错则返回指定默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The value or default.</returns> 
    public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, Func<T, TResult> defaultValueFactory)
    {
        try
        {
            return func(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory(@this);
        }
    }
    #endregion
    /// <summary>
    /// 判断是否不为空，如果是则执行指定动作
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="action">The action.</param>
    public static void IfNotNull<T>(this T @this, Action<T> action) where T : class
    {
        if (@this != null)
        {
            action(@this);
        }
    }

    /// <summary>
    ///  判断是否不为空，如果是则执行指定动作，并返回值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <returns>The function result if @this is not null otherwise default value.</returns>
    public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func) where T : class
    {
        return @this != null ? func(@this) : default(TResult);
    }

    /// <summary>
    ///  判断是否不为空，如果是则执行指定动作，并返回值 ，如果不是则返回默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The function result if @this is not null otherwise default value.</returns>
    public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue) where T : class
    {
        return @this != null ? func(@this) : defaultValue;
    }

    /// <summary>
    ///  判断是否不为空，如果是则执行指定动作，并返回值 ，如果不是则返回默认值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The function result if @this is not null otherwise default value.</returns>
    public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory) where T : class
    {
        return @this != null ? func(@this) : defaultValueFactory();
    }
    /// <summary>
    ///  判断是否为空，如果是则执行指定动作，并返回值 
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>A T.</returns>
    public static T NullIf<T>(this T @this, Func<T, bool> predicate) where T : class
    {
        if (predicate(@this))
        {
            return null;
        }
        return @this;
    }
    /// <summary>
    ///   判断两个值相等，如果是则返回null， 否则范围自己的值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="value">The value.</param>
    /// <returns>A T.</returns>
    public static T NullIfEquals<T>(this T @this, T value) where T : class
    {
        if (@this.Equals(value))
        {
            return null;
        }
        return @this;

    }
    /// <summary>
    ///   判断是否在数组中，如果是则返回null， 否则范围自己的值
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="values">A variable-length parameters list containing values.</param>
    /// <returns>A T.</returns>
    public static T NullIfEqualsAny<T>(this T @this, params T[] values) where T : class
    {
        if (Array.IndexOf(values, @this) != -1)
        {
            return null;
        }
        return @this;
    }
    /// <summary>
    /// 如果为null则返回空字符串，否则执行ToString()
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a string or empty if the value is null.</returns>
    public static string ToStringSafe(this object @this)
    {
        return @this == null ? "" : @this.ToString();
    }

    #region Try
    /// <summary>
    /// 尝试转换为指定类型的值
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryFunction">The try function.</param>
    /// <returns>A TResult.</returns>
    public static TResult Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction)
    {
        try
        {
            return tryFunction(@this);
        }
        catch
        {
            return default(TResult);
        }
    }

    /// <summary>
    /// 尝试转换为指定类型的值
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryFunction">The try function.</param>
    /// <param name="catchValue">The catch value.</param>
    /// <returns>A TResult.</returns>
    public static TResult Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, TResult catchValue)
    {
        try
        {
            return tryFunction(@this);
        }
        catch
        {
            return catchValue;
        }
    }

    /// <summary>
    /// 尝试转换为指定类型的值
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryFunction">The try function.</param>
    /// <param name="catchValueFactory">The catch value factory.</param>
    /// <returns>A TResult.</returns>
    public static TResult Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, Func<TType, TResult> catchValueFactory)
    {
        try
        {
            return tryFunction(@this);
        }
        catch
        {
            return catchValueFactory(@this);
        }
    }

    /// <summary>
    /// 尝试转换为指定类型的值
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryFunction">The try function.</param>
    /// <param name="result">[out] The result.</param>
    /// <returns>A TResult.</returns>
    public static bool Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, out TResult result)
    {
        try
        {
            result = tryFunction(@this);
            return true;
        }
        catch
        {
            result = default(TResult);
            return false;
        }
    }

    /// <summary>
    /// 尝试转换为指定类型的值
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryFunction">The try function.</param>
    /// <param name="catchValue">The catch value.</param>
    /// <param name="result">[out] The result.</param>
    /// <returns>A TResult.</returns>
    public static bool Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, TResult catchValue, out TResult result)
    {
        try
        {
            result = tryFunction(@this);
            return true;
        }
        catch
        {
            result = catchValue;
            return false;
        }
    }

    /// <summary>
    /// 尝试转换为指定类型的值
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryFunction">The try function.</param>
    /// <param name="catchValueFactory">The catch value factory.</param>
    /// <param name="result">[out] The result.</param>
    /// <returns>A TResult.</returns>
    public static bool Try<TType, TResult>(this TType @this, Func<TType, TResult> tryFunction, Func<TType, TResult> catchValueFactory, out TResult result)
    {
        try
        {
            result = tryFunction(@this);
            return true;
        }
        catch
        {
            result = catchValueFactory(@this);
            return false;
        }
    }

    /// <summary>
    /// 尝试执行指定类型的动作
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryAction">The try action.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool Try<TType>(this TType @this, Action<TType> tryAction)
    {
        try
        {
            tryAction(@this);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 尝试执行指定类型的动作
    /// </summary>
    /// <typeparam name="TType">Type of the type.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="tryAction">The try action.</param>
    /// <param name="catchAction">The catch action.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool Try<TType>(this TType @this, Action<TType> tryAction, Action<TType> catchAction)
    {
        try
        {
            tryAction(@this);
            return true;
        }
        catch
        {
            catchAction(@this);
            return false;
        }
    }
    #endregion
}
