using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace UNetCore.Helper.DB
{
	/*
     * 此数据库操作抽象基类DbHandleBase，主要依赖Dapper的实现(支持所有Ado.net Providers下工作，包括sqlite, sqlce, firebird, oracle, MySQL, PostgreSQL and SQL Server)。
     * 抽象类DbHandleBase不可实例化，需要子类继承并重写CreateConnection返回具体的数据库类型连接对象。
     * 当你系统有多库时（比如常见的主从库等），可以创建多个实例。
     */

	/// <summary>
	/// 数据库操作抽象基类
	/// </summary>
	public abstract class DbBase
    {
        #region 属性、构造函数..

        /// <summary>
        /// 默认超时时间(秒数)
        /// </summary>
        public const int DefaultTimeout = 60;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数，需要传入连接字符串
        /// </summary>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public DbBase(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new Exception("connectionstring is empty");
            }
            ConnectionString = connectionstring;
        }

        /// <summary>
        /// 创建连接对象，并打开连接        
        /// </summary>
        /// <returns></returns>
        public virtual IDbConnection CreateConnectionAndOpen()
        {
            IDbConnection connection = CreateConnection();
            connection.Open();
            return connection;
        }

        #endregion

        #region 抽象方法，子类需重写:CreateConnection

        /// <summary>
        /// 创建连接对象。子类需要重写，根据自己的数据库类型实例化自己的数据库连接对象(支持IDbConnection接口)。
        /// Dapper可以在所有Ado.net Providers下工作，包括sqlite, sqlce, firebird, oracle, MySQL, PostgreSQL and SQL Server。
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection CreateConnection();
        #endregion

        #region 非sql函数:Get、Insert、Update、Delete、GetList
        /// <summary>
        /// 根据实体类int主键返回实体类对象
        /// </summary>
        /// <typeparam name="T">实体类，数据库需要有对应名称的表</typeparam>
        /// <param name="id">int主键</param>
        /// <returns></returns>
        public T Get<T>(int id, int? cmdTimeout = DefaultTimeout) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                var result = conn.Get<T>(id, null, cmdTimeout);
                return (T)result;
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }
        /// <summary>
        /// 根据实体类string主键返回实体类对象
        /// </summary>
        /// <typeparam name="T">实体类，数据库需要有对应名称的表</typeparam>
        /// <param name="id">string主键，需要CheckSql</param>
        /// <returns></returns>
        public T Get<T>(string id, int? cmdTimeout = DefaultTimeout) where T : class
        {
            id = CheckSql(id);
            var conn = CreateConnectionAndOpen();
            try
            {
                var result = conn.Get<T>(id, null, cmdTimeout);
                return (T)result;
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        public dynamic Insert<T>(T entity, IDbTransaction transaction = null, int? cmdTimeout = DefaultTimeout) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Insert<T>(entity, transaction, cmdTimeout);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 批量插入多条记录。
        /// (内部是通过同一个连接去循环插入，没有拼成一条语句)
        /// </summary>
        public void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null, int? cmdTimeout = DefaultTimeout) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                conn.Insert<T>(entities, transaction, cmdTimeout);
            }
            finally
            {
                conn.CloseIfOpen();
            }

        }

        /// <summary>
        /// 更新记录
        /// (会产生所有字段的update set，因此要慎用，一般只能在基础表上使用此函数)
        /// </summary>
        public bool Update<T>(T entity, IDbTransaction transaction = null, int? cmdTimeout = DefaultTimeout) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Update<T>(entity, transaction, cmdTimeout);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 根据主键删除一条记录
        /// (entity对象只要主键属性有值就可以，它产生的sql类似: delete from T where 主键=**)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="cmdTimeout"></param>
        /// <returns></returns>
        public bool Delete<T>(T entity, IDbTransaction transaction = null, int? cmdTimeout = DefaultTimeout) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Delete<T>(entity, transaction, cmdTimeout);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }
        /// <summary>
        /// 根据predicate条件删除记录
        /// </summary>
        public bool Delete<T>(object predicate, IDbTransaction transaction = null, int? cmdTimeout = DefaultTimeout) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Delete<T>(predicate, transaction, cmdTimeout);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 根据predicate条件获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">筛选条件 e.g. predicate = Predicates.Field<TbUser>(f => f.Id, Operator.Lt, 100);</param>
        /// <param name="sort">指定排序 e.g. sort = new List<ISort> { Predicates.Sort<TbUser>(f => f.Id, false) };//false降序</param>
        /// <param name="cmdTimeout"></param>
        /// <param name="buffered">默认值改为true，一次性取完断开连接。如果想自行一笔一笔取，可设置为false。</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, int? cmdTimeout = 60, bool buffered = true) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.GetList<T>(predicate, sort, null, cmdTimeout, buffered);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }
        #endregion

        #region 支持sql写法函数:Query、QueryDynamics、QueryMultipleDynamic、QueryHashtables、QueryMultipleHashtables

        /// <summary>
        /// 获取T列表。
        /// e.g.: dbHandleObj.Query(("select * from TbUser where Id = @Id", new { Id = 10 });
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">e.g.: select * from TbUser where Id = @Id</param>
        /// <param name="param">e.g.: new { Id = 10 }</param>
        /// <param name="cmdTimeout"></param>
        /// <param name="buffered">默认值改为true，一次性取完断开连接。如果想自行一笔一笔取，可设置为false。</param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, bool buffered = true, CommandType? cmdType = null) where T : class
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Query<T>(sql, (object)param, null, buffered, cmdTimeout, cmdType);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 获取dynamic列表。
        /// e.g.: dbHandleObj.QueryDynamic(("select Id,Name from TbUser where Id = @Id", new { Id = 10 });
        /// </summary>
        /// <param name="sql">e.g.: select Id,Name from TbUser where Id = @Id</param>
        /// <param name="param">e.g.: new { Id = 10 }</param>
        /// <param name="cmdTimeout"></param>
        /// <param name="buffered">默认值改为true，一次性取完断开连接。如果想自行一笔一笔取，可设置为false。</param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryDynamics(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, bool buffered = true, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Query(sql, (object)param, null, buffered, cmdTimeout, cmdType);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 查询多个dynamic列表结果集。
        /// e.g.: dbHandleObj.QueryMultipleDynamic(("select Id,Name from TbUser where Id = @Id;select * from TbOrder", new { Id = 10 });
        /// </summary>
        /// <param name="sql">e.g.: select Id,Name from TbUser where Id = @Id;select * from TbOrder</param>
        /// <param name="param">e.g.: new { Id = 10 }</param>
        public List<List<dynamic>> QueryMultipleDynamic(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, IDbTransaction tran = null, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                var gridReader = conn.QueryMultiple(sql, (object)param, tran, cmdTimeout, cmdType);
                var result = new List<List<dynamic>>();
                while (!gridReader.IsConsumed)
                {
                    var items = gridReader.Read();
                    result.Add(items.ToList());
                }
                return result;
            }
            finally
            {
                conn.CloseIfOpen();
            }

        }

        #region 适用于需要把结果集转为json的场景（解决结果集是dynamic列表不能转为json的问题) 
        /// <summary>
        /// 获取Hashtable列表。 可以方便转为json对象。
        /// (注意：类似count(*)要给字段名，比如 select count(*) as recordCount from TbUser。)
        /// e.g.: dbHandleObj.QueryHashtables(("select Id,Name from TbUser where Id = @Id", new { Id = 10 });
        /// </summary>
        /// <param name="sql">e.g.: select Id,Name from TbUser where Id = @Id</param>
        /// <param name="param">e.g.: new { Id = 10 }</param>
        /// <param name="cmdTimeout"></param>
        /// <param name="buffered">注意，这边默认值为false。因为它在此函数内的DapperRowsToHashList(result)中才真正取数遍历。</param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public List<Hashtable> QueryHashtables(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, bool buffered = false, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                var result = conn.Query(sql, (object)param, null, buffered, cmdTimeout, cmdType);
                return DapperRowsToHashList(result);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }
        /// <summary>
        /// 获取多个Hashtable列表结果集。 可以方便转为json对象
        /// (注意：类似count(*)要给字段名，比如 select count(*) as recordCount from TbUser。)
        /// e.g.: dbHandleObj.QueryMultipleHashtables(("select Id,Name from TbUser where Id = @Id;select count(*) as recordCount from TbUser", new { Id = 10 });
        /// </summary>
        /// <param name="sql">e.g.: select Id,Name from TbUser where Id = @Id;select * from TbOrder</param>
        /// <param name="param">e.g.: new { Id = 10 }</param>
        /// <param name="tran"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public List<List<Hashtable>> QueryMultipleHashtables(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, IDbTransaction tran = null, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                var gridReader = conn.QueryMultiple(sql, (object)param, tran, cmdTimeout, cmdType);
                var result = new List<List<Hashtable>>();
                while (!gridReader.IsConsumed)
                {
                    var items = gridReader.Read();
                    result.Add(DapperRowsToHashList(items));
                }
                return result;
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 辅助Dapper返回的dynamic结果集转为Hashtable列表，方便转为json。
        /// (没有实体类对应的Query时，Dapper是返回dynamic类型对象，这个dynamic对象其实是它自己内部Private级别的DapperRow列表对象。)
        /// </summary>
        /// <param name="rows">必须是Dapper返回的IEnumerable<dynamic>对象</param>
        /// <returns></returns>
        protected List<Hashtable> DapperRowsToHashList(IEnumerable<dynamic> rows)
        {
            var list = new List<Hashtable>();
            foreach (var row in rows)
            {
                var obj = new Hashtable();
                foreach (var prop in row)
                {
                    obj.Add(prop.Key, prop.Value);//含有Key,Value的dynamic对象（其实就是DapperRow，但它的访问级别是Private，因此用dynamic访问）
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion
        #endregion

        #region ExecuteNonQuery、ExecuteScalar、ExecuteDataTable、ExecuteDataset、GetPageByModel
        //（这边使用Dapper的函数，Dapper比ADO.Net有多做了Command的缓存等处理。）
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">匿名对象的参数，可以简单写，比如 new {Name="user1"} 即可，会统一处理参数和参数的size</param>
        public virtual int ExecuteNonQuery(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, IDbTransaction tran = null, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.Execute(sql, (object)param, tran, cmdTimeout, cmdType);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 执行sql语句，并返回单个数值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">匿名对象的参数，可以简单写，比如 new {Name="user1"} 即可，会统一处理参数和参数的size</param>
        public virtual object ExecuteScalar(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, IDbTransaction tran = null, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.ExecuteScalar(sql, (object)param, tran, cmdTimeout, cmdType);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /*
         * 说明:winform中经常会用到DataTable的DataView，方便Grid展示与过滤，因此开放ExecuteDataTable和ExecuteDataset；
         * 如果是B/S系统，建议用以上的Query系列函数。
         */

        /// <summary>
        /// 执行sql语句，并返回DataTable(适用于Dapper支持的所有数据库类型)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">匿名对象的参数，可以简单写，比如 new {Name="user1"} 即可，会统一处理参数和参数的size</param>
        public virtual DataTable ExecuteDataTable(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, IDbTransaction tran = null, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                //这边用Dapper的ExecuteReader，统一了函数参数写法，不用使用SqlParameter。
                using (var reader = conn.ExecuteReader(sql, (object)param, tran, cmdTimeout, cmdType))
                {
                    var dataTable = DataReaderToDataTable(reader);
                    return dataTable;
                }
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }

        /// <summary>
        /// 执行sql语句，并返回DataSet(适用于一条sql可以返回多个数据集的数据库类型)
        /// </summary>
        /// <param name="sql">一条sql可以返回多个数据集DataTable的数据库都可以直接使用，比如sqlserver可以多条语句用;隔开</param>
        /// <param name="param">匿名对象的参数，可以简单写，比如 new {Name="user1"} 即可，Dapper会统一处理参数和参数的size</param>
        public virtual DataSet ExecuteDataSet(string sql, dynamic param = null, int? cmdTimeout = DefaultTimeout, IDbTransaction tran = null, CommandType? cmdType = null)
        {
            sql = CheckSql(sql);
            var conn = CreateConnectionAndOpen();
            try
            {
                DataSet dataSet = new DataSet();
                //这边用Dapper的ExecuteReader，统一了函数参数写法，不用使用SqlParameter。
                using (var reader = conn.ExecuteReader(sql, (object)param, tran, cmdTimeout, cmdType))
                {
                    do
                    {
                        var dataTable = DataReaderToDataTable(reader);
                        dataSet.Tables.Add(dataTable);
                    }
                    while (reader.NextResult());
                    return dataSet;
                }
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }


        /// <summary>
        /// 对单个表分页取数。
        /// (如果要多个表关联分页，建议直接写sql)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="sort">排序字段</param>
        /// <param name="startPageIndex">起始页，第1页从0开始</param>
        /// <param name="resultsPerPage">每页多少笔</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>        
        public IEnumerable<T> GetPageByModel<T>(object predicate, IList<ISort> sort, int startPageIndex, int resultsPerPage
            , IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = true) where T : class
        {
            var conn = CreateConnectionAndOpen();
            try
            {
                return conn.GetPage<T>(predicate, sort, startPageIndex, resultsPerPage, transaction, commandTimeout, buffered);
            }
            finally
            {
                conn.CloseIfOpen();
            }
        }
        #endregion

        #region 事务-代码示例
        /*
           var conn = CreateConnection();
           var tran = conn.BeginTransaction();
           try
           {
               conn.Execute("update TbUser set .. where Id=@Id",new {Id=1}, tran);
               conn.Execute("update TbOrder set .. where Id=@Id",new {Id=10}, tran);
               tran.Commit();
           }
           catch
           {
               tran.Rollback();
           }
           finally
           {
               conn.CloseIfOpen();
           }
        */
        #endregion

        #region 如果你的系统会使用不同数据库类型，或者今后会更改数据库类型，则建议重写的以下相关方法。否则，忽视之。
        /// <summary>
        /// 获取分页查询sql，支持多语句查询，详见otherSql参数
        /// (这边只负责组合分页sql)
        /// </summary>
        /// <param name="selectSql">待分页的主体sql，不可包含order by。e.g. select * from TbUser A</param>
        /// <param name="orderSql">分页依据的排序，order by在这边传入。e.g. order by A.Id</param>
        /// <param name="startPageIndex">起始页，第1页从0开始</param>
        /// <param name="resultsPerPage">每页记录数</param>
        /// <param name="otherSql">同时要执行的其它sql语句。比如需要同时返回表记录总数，可在这边传入" select count(1) from TbUser"。
        /// 其实就是支持多条sql执行，比如也可以传:select count(1) from TbUser;select * from TbOrder。即多条sql用分号隔开</param>
        /// <param name="param">匿名对象的参数，可以简单写，比如 new {Name="user1"} 即可，会统一处理参数和参数的size</param>
        /// <returns></returns>
        public virtual string GetPageSql(string selectSql, string orderSql, int startPageIndex, int resultsPerPage, string otherSql = "")
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取前几笔数据的sql
        /// (sqlservert:top关键字,mysql:limit,oracle: rownum... )
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public virtual string GetTopSql(string selectSql, int top)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行sql前对sql字符串的处理事件。可以检查sql安全性，也可以按不同数据库sql写法做调整(sqlserver是用@，oracle的参数是用冒号)。
        /// </summary>
        public virtual string CheckSql(string strSql)
        {
            return strSql;
            //if (如果有可疑sql(根据不同数据库类型)) 则记录日志并抛异常
        }
        #endregion

        #region 静态辅助函数:DataReaderToDataTable

        /// <summary>
        /// 根据DataReader生成DataTable，轻量级。      
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DataTable DataReaderToDataTable(IDataReader reader)
        {
            DataTable dataTable = new DataTable();
            int fieldCount = reader.FieldCount;
            for (int i = 0; i <= fieldCount - 1; i++)
            {
                dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            //populate datatable
            dataTable.BeginLoadData();
            object[] fieldValues = new object[fieldCount];
            while (reader.Read())
            {
                reader.GetValues(fieldValues);
                dataTable.LoadDataRow(fieldValues, true);
            }
            dataTable.EndLoadData();
            return dataTable;
        }

        #endregion
    }

    public static class DbHandleExtension
    {
        /// <summary>
        /// 如果连接是Open状态，则关闭连接
        /// </summary>
        /// <param name="connection"></param>
        public static void CloseIfOpen(this IDbConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

}
