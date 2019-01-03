using System;
    /// <summary>
    /// TimeSpan extensions
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        ///     Multiply a <c>System.TimeSpan</c> by a <paramref name="factor"/>
        /// </summary>
        /// <param name="source">The given <c>System.TimeSpan</c> to be multiplied</param>
        /// <param name="factor">The multiplier factor</param>
        /// <returns>The multiplication of the <paramref name="source"/> by <paramref name="factor"/></returns>
        public static TimeSpan MultiplyBy(this TimeSpan source, int factor)
        {
            TimeSpan result = TimeSpan.FromTicks(source.Ticks*factor);
            return result;
        }
        
        /// <summary>
        ///     Multiply a <c>System.TimeSpan</c> by a <paramref name="factor"/>
        /// </summary>
        /// <param name="source">The given <c>System.TimeSpan</c> to be multiplied</param>
        /// <param name="factor">The multiplier factor</param>
        /// <returns>The multiplication of the <paramref name="source"/> by <paramref name="factor"/></returns>
        public static TimeSpan MultiplyBy(this TimeSpan source, double factor)
        {
            TimeSpan result = TimeSpan.FromTicks((long)(source.Ticks*factor));
            return result;
        }
        /// <summary>
        ///     A TimeSpan extension method that substract the specified TimeSpan to the current DateTime.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current DateTime with the specified TimeSpan substracted from it.</returns>
        public static DateTime Ago(this TimeSpan @this)
        {
            return DateTime.Now.Subtract(@this);
        }
        /// <summary>
        ///     A TimeSpan extension method that add the specified TimeSpan to the current DateTime.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current DateTime with the specified TimeSpan added to it.</returns>
        public static DateTime FromNow(this TimeSpan @this)
        {
            return DateTime.Now.Add(@this);
        }
        /// <summary>
        ///     A TimeSpan extension method that substract the specified TimeSpan to the current UTC (Coordinated Universal
        ///     Time)
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan substracted from it.</returns>
        public static DateTime UtcAgo(this TimeSpan @this)
        {
            return DateTime.UtcNow.Subtract(@this);
        }
        /// <summary>
        ///     A TimeSpan extension method that add the specified TimeSpan to the current UTC (Coordinated Universal Time)
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan added to it.</returns>
        public static DateTime UtcFromNow(this TimeSpan @this)
        {
            return DateTime.UtcNow.Add(@this);
        }
    }