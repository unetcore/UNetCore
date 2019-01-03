using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNetCore.Helper.DB
{
    public class Sqlite : DbBase
    {
        #region 初始化必须
        /// <summary>
        ///初始化，需要传入连接字符串
        /// </summary>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public Sqlite(string connectionstring) : base(connectionstring)
        {
            //DapperExtensions默认是用SqlServer的标记，这边使用Sqlite的特殊语法标记，需要在初始化时给予指定。
            //DapperExtensions是静态的，全局只能支持一种数据库 
            DapperExtensions.SqlDialect = new Sql.SqliteDialect();
        }
		
        /// <summary>
        /// 创建连接对象。子类需要重写，根据自己的数据库类型实例化自己的数据库连接对象(支持IDbConnection接口)。
        /// Dapper可以在所有Ado.net Providers下工作，包括sqlite, sqlce, firebird, oracle, MySQL, PostgreSQL and SQL Server。
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection CreateConnection()
        {
            //todo 可用Nuget下载支持Sqlite的程序包，然后就可以了。
            IDbConnection connection = new SqliteConnection(ConnectionString);// System.Data.SQLite
            return connection;
            // throw new Exception("//todo:使用前请先下载支持SQLite的程序包System.Data.SQLite");
        }
        #endregion
    }
}
