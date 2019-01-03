using System;

    /// <summary>
    /// 	Extension methods for the Double data type
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        ///     A T extension method that check if the value is between (exclusif) the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
        /// ###
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool Between(this Double @this, Double minValue, Double maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }
        /// <summary>
        ///     A Double extension method that converts the @this to a money.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Double.</returns>
        public static Double ToMoney(this Double @this)
        {
            return Math.Round(@this, 2);
        }
        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        /// ###
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool In(this Double @this, params Double[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }
        /// <summary>Checks whether the value is in range</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        public static bool InRange(this double value, double minValue, double maxValue)
        {
            return (value >= minValue && value <= maxValue);
        }

        /// <summary>Checks whether the value is in range or returns the default value</summary>
        /// <param name="value">The Value</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <param name="defaultValue">The default value</param>
        public static double InRange(this double value, double minValue, double maxValue, double defaultValue)
        {
            return value.InRange(minValue, maxValue) ? value : defaultValue;
        }

       

        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        /// ###
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool NotIn(this Double @this, params Double[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }
        /// <summary>
        ///     Returns a value that indicates whether the specified value is not a number ().
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>true if  evaluates to ; otherwise, false.</returns>
        public static Boolean IsNaN(this Double d)
        {
            return Double.IsNaN(d);
        }
        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to negative or positive infinity.
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>true if  evaluates to  or ; otherwise, false.</returns>
        public static Boolean IsInfinity(this Double d)
        {
            return Double.IsInfinity(d);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to negative infinity.
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>true if  evaluates to ; otherwise, false.</returns>
        public static Boolean IsNegativeInfinity(this Double d)
        {
            return Double.IsNegativeInfinity(d);
        }
        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to positive infinity.
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>true if  evaluates to ; otherwise, false.</returns>
        public static Boolean IsPositiveInfinity(this Double d)
        {
            return Double.IsPositiveInfinity(d);
        }
    }