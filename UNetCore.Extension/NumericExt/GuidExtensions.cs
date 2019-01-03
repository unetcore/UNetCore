using System;
/// <summary>
/// GUID 拓展类
/// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        /// ###
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool NotIn(this Guid @this, params Guid[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }
        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        /// ###
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool In(this Guid @this, params Guid[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }
        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsEmpty(this Guid @this)
        {
            return @this == Guid.Empty;
        }

        /// <summary>A GUID extension method that queries if a not is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if a not is empty, false if not.</returns>
        public static bool IsNotEmpty(this Guid @this)
        {
            return @this != Guid.Empty;
        }

        /// <summary>
        /// 返回不含分隔符的GUID 字符串
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToStringWithoutSeparator(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
