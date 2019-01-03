using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;
/// <summary>
/// DataTable Extesions
/// </summary>
    public static class DataTableExtesions
    {
        /// <summary>
        ///     A DataTable extension method that return the first row.
        /// </summary>
        /// <param name="this">The table to act on.</param>
        /// <returns>The first row of the table.</returns>
        public static DataRow FirstRow(this DataTable @this)
        {
            return @this.Rows[0];
        }

        /// <summary>A DataTable extension method that last row.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DataRow.</returns>
        public static DataRow LastRow(this DataTable @this)
        {
            return @this.Rows[@this.Rows.Count - 1];
        }
         

     
    }
