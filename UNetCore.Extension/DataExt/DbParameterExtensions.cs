using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
/// <summary>
/// DbParameter Extensions
/// </summary>
    public static class DbParameterExtensions
    {
        /// <summary>
        ///     An IDictionary&lt;string,object&gt; extension method that converts this object to a database parameters.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="command">The command.</param>
        /// <returns>The given data converted to a DbParameter[].</returns>
        public static DbParameter[] ToDbParameters(this IDictionary<string, object> @this, DbCommand command)
        {
            return @this.Select(x =>
            {
                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = x.Key;
                parameter.Value = x.Value;
                return parameter;
            }).ToArray();
        }

        /// <summary>
        ///     An IDictionary&lt;string,object&gt; extension method that converts this object to a database parameters.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>The given data converted to a DbParameter[].</returns>
        public static DbParameter[] ToDbParameters(this IDictionary<string, object> @this, DbConnection connection)
        {
            DbCommand command = connection.CreateCommand();

            return @this.Select(x =>
            {
                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = x.Key;
                parameter.Value = x.Value;
                return parameter;
            }).ToArray();
        }

        

        /// <summary>
        ///     An IDictionary&lt;string,object&gt; extension method that converts the @this to a SQL parameters.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a SqlParameter[].</returns>
        public static SqlParameter[] ToSqlParameters(this IDictionary<string, object> @this)
        {
            return @this.Select(x => new SqlParameter(x.Key, x.Value)).ToArray();
        }
        /// <summary>
        ///     A SqlParameterCollection extension method that adds a range with value to 'values'.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">The values.</param>
        public static void AddRangeWithValue(this SqlParameterCollection @this, Dictionary<string, object> values)
        {
            foreach (var keyValuePair in values)
            {
                @this.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            }
        }

    }