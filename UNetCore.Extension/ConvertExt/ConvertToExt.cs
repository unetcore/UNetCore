using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Extensions to Convert object to T
/// </summary>
public static class ConvertToExt
{


    /// <summary>
    ///  将IDataReader转换为DataTable
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static DataTable ToDataTable(this IDataReader reader)
    {
        DataTable objDataTable = new DataTable("Table");
        int intFieldCount = reader.FieldCount;
        for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
        {
            objDataTable.Columns.Add(reader.GetName(intCounter).ToUpper(), reader.GetFieldType(intCounter));
        }
        objDataTable.BeginLoadData();
        object[] objValues = new object[intFieldCount];
        while (reader.Read())
        {
            reader.GetValues(objValues);
            objDataTable.LoadDataRow(objValues, true);
        }
        reader.Close();
        objDataTable.EndLoadData();
        return objDataTable;
    }

    /// <summary>
    /// 转换为实体类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dr"></param>
    /// <returns></returns>
    public static T ToModel<T>(this IDataReader dr)
    {
        try
        {
            using (dr)
            {
                if (dr.Read())
                {
                    List<string> list = new List<string>(dr.FieldCount);
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        list.Add(dr.GetName(i).ToLower());
                    }
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (list.Contains(pi.Name.ToLower()))
                        {
                            if (!IsNullOrDBNull(dr[pi.Name]))
                            {
                                pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                            }
                        }
                    }
                    return model;
                }
            }
            return default(T);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 转换为列表对象类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dr"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this IDataReader dr)
    {
        using (dr)
        {
            List<string> field = new List<string>(dr.FieldCount);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                field.Add(dr.GetName(i).ToLower());
            }
            List<T> list = new List<T>();
            while (dr.Read())
            {
                T model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (field.Contains(property.Name.ToLower()))
                    {
                        if (!IsNullOrDBNull(dr[property.Name]))
                        {
                            property.SetValue(model, HackType(dr[property.Name], property.PropertyType), null);
                        }
                    }
                }
                list.Add(model);
            }
            return list;
        }
    }

    /// <summary>
    ///     Enumerates to entities in this collection.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an IEnumerable&lt;T&gt;</returns>
    public static IEnumerable<T> ToEntities<T>(this IDataReader @this) where T : new()
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        var list = new List<T>();

        var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
            .Select(@this.GetName));

        while (@this.Read())
        {
            var entity = new T();

            foreach (PropertyInfo property in properties)
            {
                if (hash.Contains(property.Name))
                {
                    Type valueType = property.PropertyType;
                    property.SetValue(entity, @this[property.Name].To(valueType), null);
                }
            }

            foreach (FieldInfo field in fields)
            {
                if (hash.Contains(field.Name))
                {
                    Type valueType = field.FieldType;
                    field.SetValue(entity, @this[field.Name].To(valueType));
                }
            }

            list.Add(entity);
        }

        return list;
    }

    /// <summary>
    ///     An IDataReader extension method that converts the @this to an entity.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a T.</returns>
    public static T ToEntity<T>(this IDataReader @this) where T : new()
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        var entity = new T();

        var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
            .Select(@this.GetName));

        foreach (PropertyInfo property in properties)
        {
            if (hash.Contains(property.Name))
            {
                Type valueType = property.PropertyType;
                property.SetValue(entity, @this[property.Name].To(valueType), null);
            }
        }

        foreach (FieldInfo field in fields)
        {
            if (hash.Contains(field.Name))
            {
                Type valueType = field.FieldType;
                field.SetValue(entity, @this[field.Name].To(valueType));
            }
        }

        return entity;
    }
    /// <summary>
    ///     An IDataReader extension method that converts the @this to an expando object.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a dynamic.</returns>
    public static dynamic ToExpandoObject(this IDataReader @this)
    {
        Dictionary<int, KeyValuePair<int, string>> columnNames = Enumerable.Range(0, @this.FieldCount)
            .Select(x => new KeyValuePair<int, string>(x, @this.GetName(x)))
            .ToDictionary(pair => pair.Key);

        dynamic entity = new ExpandoObject();
        var expandoDict = (IDictionary<string, object>)entity;

        Enumerable.Range(0, @this.FieldCount)
            .ToList()
            .ForEach(x => expandoDict.Add(columnNames[x].Value, @this[x]));

        return entity;
    }
    /// <summary>
    ///     Enumerates to expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an IEnumerable&lt;dynamic&gt;</returns>
    public static IEnumerable<dynamic> ToExpandoObjects(this IDataReader @this)
    {
        Dictionary<int, KeyValuePair<int, string>> columnNames = Enumerable.Range(0, @this.FieldCount)
            .Select(x => new KeyValuePair<int, string>(x, @this.GetName(x)))
            .ToDictionary(pair => pair.Key);

        var list = new List<dynamic>();

        while (@this.Read())
        {
            dynamic entity = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)entity;

            Enumerable.Range(0, @this.FieldCount)
                .ToList()
                .ForEach(x => expandoDict.Add(columnNames[x].Value, @this[x]));

            list.Add(entity);
        }

        return list;
    }

    private static object HackType(object value, Type conversionType)
    {
        if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
                return null;

            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
            conversionType = nullableConverter.UnderlyingType;
        }
        return Convert.ChangeType(value, conversionType);
    }

    private static bool IsNullOrDBNull(object obj)
    {
        return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
    }

    #region DataTable to List
    /// <summary>
    /// 转换数据表类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IList<T> list)
    {
        DataTable table = CreateTable<T>();
        Type entityType = typeof(T);
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

        foreach (T item in list)
        {
            DataRow row = table.NewRow();

            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(item);
            }

            table.Rows.Add(row);
        }

        return table;
    }

    /// <summary>
    /// 转换为数据列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows"></param>
    /// <returns></returns>
    public static IList<T> ToList<T>(this IList<DataRow> rows)
    {
        IList<T> list = null;

        if (rows != null)
        {
            list = new List<T>();

            foreach (DataRow row in rows)
            {
                T item = CreateItem<T>(row);
                list.Add(item);
            }
        }

        return list;
    }
    /// <summary>
    /// 转换为数据列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <returns></returns>
    public static IList<T> ToList<T>(this DataTable table)
    {
        if (table == null)
        {
            return null;
        }

        List<DataRow> rows = new List<DataRow>();

        foreach (DataRow row in table.Rows)
        {
            rows.Add(row);
        }

        return ToList<T>(rows);
    }

    /// <summary>
    ///     A T[] extension method that converts the @this to a data table.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a DataTable.</returns>
    public static DataTable ToDataTable<T>(this T[] @this)
    {
        Type type = typeof(T);

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        var dt = new DataTable();

        foreach (PropertyInfo property in properties)
        {
            dt.Columns.Add(property.Name, property.PropertyType);
        }

        foreach (FieldInfo field in fields)
        {
            dt.Columns.Add(field.Name, field.FieldType);
        }

        foreach (T item in @this)
        {
            DataRow dr = dt.NewRow();

            foreach (PropertyInfo property in properties)
            {

                dr[property.Name] = property.GetValue(item, null) == null ? DBNull.Value : property.GetValue(item, null); //property.GetValue(item, null);
            }

            foreach (FieldInfo field in fields)
            {
                dr[field.Name] = field.GetValue(item);
            }

            dt.Rows.Add(dr);
        }

        return dt;
    }


    public static T CreateItem<T>(this DataRow row)
    {
        T obj = default(T);
        if (row != null)
        {


            obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                var type = typeof(T);
                PropertyInfo prop = type.GetProperty(column.ColumnName, BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
                try
                {
                    object value = row[column.ColumnName];
                    prop.SetValue(obj, HackType(value, prop.PropertyType), null);
                }
                catch
                {
                    // You can log something here  
                    //throw;
                }
            }
        }

        return obj;
    }

    public static DataTable CreateTable<T>()
    {
        Type entityType = typeof(T);
        DataTable table = new DataTable(entityType.Name);
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

        foreach (PropertyDescriptor prop in properties)
        {
            Type colType = prop.PropertyType;
            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(colType);
                //将convertsionType转换为nullable对的基础基元类型
                colType = nullableConverter.UnderlyingType;

                //colType = colType.GetGenericArguments()[0];

            }

            table.Columns.Add(new DataColumn(prop.Name, colType));

        }

        return table;
    }



    #endregion


}
