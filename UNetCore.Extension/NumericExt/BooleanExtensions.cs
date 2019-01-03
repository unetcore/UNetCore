using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public static class BooleanExtensions
    {
        /// <summary>
        /// Converts the value of this instance to its equivalent string representation (either "Yes" or "No").
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns>string</returns>
        public static string ToYesNoString(this Boolean boolean)
        {
            return boolean ? "Yes" : "No";
        }

        /// <summary>
        /// Converts the value in number format {1 , 0}.
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns>int</returns>
        /// <example>
        /// 	<code>
        /// 		int result= default(bool).ToBinaryTypeNumber()
        /// 	</code>
        /// </example>
        /// <remarks>
        /// </remarks>
        public static int ToBinaryTypeNumber(this Boolean boolean)
        {
            return boolean ? 1 : 0;
        }
        /// <summary>
        ///     A bool extension method that execute an Action if the value is false.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action to execute.</param>
        public static void IfFalse(this bool @this, Action action)
        {
            if (!@this)
            {
                if (action != null)
                {
                    action();

                }
            }
        }

        /// <summary>
        ///     A bool extension method that execute an Action if the value is true.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action to execute.</param>
        public static void IfTrue(this bool @this, Action action)
        {
            if (@this)
            {
                action();
            }
        }

        /// <summary>
        ///     A bool extension method that convert this object into a binary representation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A binary represenation of this object.</returns>
        public static byte ToBinary(this bool @this)
        {
            return Convert.ToByte(@this);
        }

        /// <summary>
        ///     A bool extension method that show the trueValue when the @this value is true; otherwise show the falseValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="trueValue">The true value to be returned if the @this value is true.</param>
        /// <param name="falseValue">The false value to be returned if the @this value is false.</param>
        /// <returns>A string that represents of the current boolean value.</returns>
        public static string ToString(this bool @this, string trueValue, string falseValue)
        {
            return @this ? trueValue : falseValue;
        }


    }