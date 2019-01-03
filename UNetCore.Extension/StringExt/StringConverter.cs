using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Security;

/// <summary>
/// 	Universal conversion and parsing methods for strings.
/// 	These methods are avaiblable throught the generic object.ConvertTo method:
/// 	Feel free to provide additional converns for string or any other object type.
/// </summary>
/// <example>
/// 	<code>
/// 		var value = "123";
/// 		var numeric = value.ConvertTo().ToInt32();
/// 	</code>
/// </example>
public static class StringConverter
{

    /// <summary>
    ///     Converts a regular string into SecureString
    /// </summary>
    /// <param name="u">String value.</param>
    /// <param name="makeReadOnly">Makes the text value of this secure string read-only.</param>
    /// <returns>Returns a SecureString containing the value of a transformed object. </returns>
    public static SecureString ToSecureString(this string u, bool makeReadOnly = true)
    {
        if (u.IsNull()) { return null; }

        SecureString s = new SecureString();

        foreach (char c in u) { s.AppendChar(c); }

        if (makeReadOnly) { s.MakeReadOnly(); }

        return s;
    }

    // TODO: Evaluate if this method needs to be in this .cs file, otherwise create the new one and move the method (public static string ToString(this SecureString s))

    /// <summary>
    ///     Coverts the SecureString to a regular string.
    /// </summary>
    /// <param name="s">Object value.</param>
    /// <returns>Content of secured string.</returns>
    public static string ToUnsecureString(this SecureString s)
    {
        if (s.IsNull())
            return null;

        IntPtr unmanagedString = IntPtr.Zero;

        try
        {
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(s);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }


    #region string型转换为DbType
    /// <summary>
    /// string型转换为
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="dbtype"></param>
    /// <returns></returns>
    public static object ToValueByDbType(this string value, DbType dbtype)
    {
        object result = null;
        switch (dbtype)
        {
            case DbType.Boolean:
                bool v;
                value = value.Trim();
                Boolean.TryParse(value, out v);
                result = v;
                break;
            case DbType.Date:
            case DbType.DateTime:
            case DbType.Time:
            case DbType.DateTime2:
                value = value.Trim();
                DateTime dt;
                DateTime.TryParse(value, out dt);
                result = dt;
                break;
            case DbType.DateTimeOffset:
                break;
            case DbType.Decimal:
            case DbType.Currency:
            case DbType.Double:
                value = value.Trim();
                Double dv;
                Double.TryParse(value, out dv);
                result = dv;
                break;
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.UInt16:
            case DbType.UInt32:
            case DbType.UInt64:
            case DbType.Byte:
            case DbType.SByte:
            case DbType.Single:
            case DbType.VarNumeric:
                value = value.Trim();
                int i;
                Int32.TryParse(value, out i);
                result = i;
                break;
            case DbType.Object:
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
                result = value;
                break;
            case DbType.Xml:
                break;
            case DbType.Binary:
                break;
            case DbType.Guid:
                break;
            default:
                result = value;
                break;
        }
        return result;
    }

    #endregion

    #region string转换为TypeCode
    /// <summary>
    /// string转换为TypeCode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object ToValueByTypeCode(this string value, TypeCode type)
    {
        object result = null;
        switch (type)
        {
            case TypeCode.Boolean:
                Boolean b;
                Boolean.TryParse(value, out b);
                result = b;
                break;
            case TypeCode.Byte:
                Byte bt;
                Byte.TryParse(value, out bt);
                result = bt;
                break;
            case TypeCode.Char:
                Char c;
                Char.TryParse(value, out c);
                result = c;
                break;
            case TypeCode.DBNull:
                result = DBNull.Value;
                break;
            case TypeCode.DateTime:
                DateTime dt;
                DateTime.TryParse(value, out dt);
                result = dt;
                break;
            case TypeCode.Decimal:
                Decimal dec;
                Decimal.TryParse(value, out dec);
                result = dec;
                break;
            case TypeCode.Double:
                Double db;
                Double.TryParse(value, out db);
                result = db;
                break;
            case TypeCode.Empty:
                result = null;
                break;
            case TypeCode.Int16:
                Int16 i16;
                Int16.TryParse(value, out i16);
                result = i16;
                break;
            case TypeCode.Int32:
                Int32 i32;
                Int32.TryParse(value, out i32);
                result = i32;
                break;
            case TypeCode.Int64:
                Int64 i64;
                Int64.TryParse(value, out i64);
                result = i64;
                break;
            case TypeCode.Object:
                result = value;
                break;
            case TypeCode.SByte:
                SByte sb;
                SByte.TryParse(value, out sb);
                result = sb;
                break;
            case TypeCode.Single:
                Single sng;
                Single.TryParse(value, out sng);
                result = sng;
                break;
            case TypeCode.String:
                result = String.IsNullOrEmpty(value) ? String.Empty : value;
                break;
            case TypeCode.UInt16:
                UInt16 ui16;
                UInt16.TryParse(value, out ui16);
                result = ui16;
                break;
            case TypeCode.UInt32:
                UInt32 ui32;
                UInt32.TryParse(value, out ui32);
                result = ui32;
                break;
            case TypeCode.UInt64:
                UInt64 ui64;
                UInt64.TryParse(value, out ui64);
                result = ui64;
                break;
            default:
                result = value;
                break;
        }
        return result;
    }
    #endregion
    #region 常用类型转换为DbType类型
    /// <summary>
    /// 常用类型转换为DbType类型
    /// </summary>
    /// <param name="type">常用类型</param>
    /// <returns>System.Data.DbType</returns>
    public static System.Data.DbType ToDbType(this Type type)
    {
        System.Data.DbType result = System.Data.DbType.String;
        if (type.Equals(typeof(int)) || type.IsEnum)
            result = System.Data.DbType.Int32;
        else if (type.Equals(typeof(long)))
            result = System.Data.DbType.Int32;
        else if (type.Equals(typeof(double)) || type.Equals(typeof(Double)))
            result = System.Data.DbType.Decimal;
        else if (type.Equals(typeof(DateTime)))
            result = System.Data.DbType.DateTime;
        else if (type.Equals(typeof(bool)))
            result = System.Data.DbType.Boolean;
        else if (type.Equals(typeof(string)))
            result = System.Data.DbType.String;
        else if (type.Equals(typeof(decimal)))
            result = System.Data.DbType.Decimal;
        else if (type.Equals(typeof(byte[]))) result = System.Data.DbType.Binary;
        else if (type.Equals(typeof(Guid)))
            result = System.Data.DbType.Guid;
        return result;
    }
    #endregion

    #region 常用类型转换为SqlDbType类型
    /// <summary>
    /// 常用类型转换为SqlDbType类型
    /// </summary>
    /// <param name="type">常用类型</param>
    /// <returns>System.Data.SqlDbType</returns>
    public static System.Data.SqlDbType ToSqlDbType(this Type type)
    {
        System.Data.SqlDbType result = System.Data.SqlDbType.NVarChar;
        if (type.Equals(typeof(int)) || type.IsEnum)
            result = System.Data.SqlDbType.Int;
        else if (type.Equals(typeof(long)))
            result = System.Data.SqlDbType.Int;
        else if (type.Equals(typeof(double)) || type.Equals(typeof(Double)))
            result = System.Data.SqlDbType.Decimal;
        else if (type.Equals(typeof(DateTime)))
            result = System.Data.SqlDbType.DateTime;
        else if (type.Equals(typeof(bool)))
            result = System.Data.SqlDbType.Bit;
        else if (type.Equals(typeof(string)))
            result = System.Data.SqlDbType.NVarChar;
        else if (type.Equals(typeof(decimal)))
            result = System.Data.SqlDbType.Decimal;
        else if (type.Equals(typeof(byte[])))
            result = System.Data.SqlDbType.Binary;
        else if (type.Equals(typeof(Guid)))
            result = System.Data.SqlDbType.UniqueIdentifier;
        else if (type.Equals(typeof(char)))
            result = System.Data.SqlDbType.NVarChar;
        else if (type.Equals(typeof(Single)))
            result = System.Data.SqlDbType.Decimal;
        else if (type.Equals(typeof(Int64)))
            result = System.Data.SqlDbType.BigInt;
        else if (type.Equals(typeof(object)))
            result = System.Data.SqlDbType.Variant;
        else if (type.Equals(typeof(Decimal)))
            result = System.Data.SqlDbType.Decimal;
        else if (type.Equals(typeof(Int16)))
            result = System.Data.SqlDbType.SmallInt;
        else if (type.Equals(typeof(Byte)))
            result = System.Data.SqlDbType.TinyInt;

        return result;
    }
    #endregion

    #region SqlDbType类型转换为常用类型
    /// <summary>
    /// SqlDbType类型转换为常用类型
    /// </summary>
    /// <param name="strSqlType">System.Data.SqlDbType类型</param>
    /// <returns>Type</returns>
    public static Type ToCommonType(this System.Data.SqlDbType strSqlType)
    {
        switch (strSqlType)
        {
            case System.Data.SqlDbType.BigInt:
                return typeof(Int64);
            case System.Data.SqlDbType.Binary:
                return typeof(Object);
            case System.Data.SqlDbType.Bit:
                return typeof(Boolean);
            case System.Data.SqlDbType.Char:
                return typeof(String);
            case System.Data.SqlDbType.DateTime:
                return typeof(DateTime);
            case System.Data.SqlDbType.Decimal:
                return typeof(Decimal);
            case System.Data.SqlDbType.Float:
                return typeof(Double);
            case System.Data.SqlDbType.Image:
                return typeof(Object);
            case System.Data.SqlDbType.Int:
                return typeof(Int32);
            case System.Data.SqlDbType.Money:
                return typeof(Decimal);
            case System.Data.SqlDbType.NChar:
                return typeof(String);
            case System.Data.SqlDbType.NText:
                return typeof(String);
            case System.Data.SqlDbType.NVarChar:
                return typeof(String);
            case System.Data.SqlDbType.Real:
                return typeof(Single);
            case System.Data.SqlDbType.SmallDateTime:
                return typeof(DateTime);
            case System.Data.SqlDbType.SmallInt:
                return typeof(Int16);
            case System.Data.SqlDbType.SmallMoney:
                return typeof(Decimal);
            case System.Data.SqlDbType.Text:
                return typeof(String);
            case System.Data.SqlDbType.Timestamp:
                return typeof(Object);
            case System.Data.SqlDbType.TinyInt:
                return typeof(Byte);
            case System.Data.SqlDbType.Udt://自定义的数据类型
                return typeof(Object);
            case System.Data.SqlDbType.UniqueIdentifier:
                return typeof(Object);
            case System.Data.SqlDbType.VarBinary:
                return typeof(Object);
            case System.Data.SqlDbType.VarChar:
                return typeof(String);
            case System.Data.SqlDbType.Variant:
                return typeof(Object);
            case System.Data.SqlDbType.Xml:
                return typeof(Object);
            default:
                return null;
        }
    }


    #endregion

    #region SQL Server数据类型字符串转换为SqlDbType类型
    /// <summary>
    /// SQL Server数据类型字符串转换为SqlDbType类型
    /// </summary>
    /// <param name="strSqlTypeString">SQL Server数据类型字符串（如："varchar"）</param>
    /// <returns> System.Data.SqlDbType</returns>
    /*  调用：
        description
     */
    public static System.Data.SqlDbType ToSqlType(this string strSqlTypeString)
    {
        System.Data.SqlDbType dbType = System.Data.SqlDbType.Variant;//默认为Object

        switch (strSqlTypeString)
        {
            case "int":
                dbType = System.Data.SqlDbType.Int;
                break;
            case "varchar":
                dbType = System.Data.SqlDbType.VarChar;
                break;
            case "bit":
                dbType = System.Data.SqlDbType.Bit;
                break;
            case "datetime":
                dbType = System.Data.SqlDbType.DateTime;
                break;
            case "decimal":
                dbType = System.Data.SqlDbType.Decimal;
                break;
            case "float":
                dbType = System.Data.SqlDbType.Float;
                break;
            case "image":
                dbType = System.Data.SqlDbType.Image;
                break;
            case "money":
                dbType = System.Data.SqlDbType.Money;
                break;
            case "ntext":
                dbType = System.Data.SqlDbType.NText;
                break;
            case "nvarchar":
                dbType = System.Data.SqlDbType.NVarChar;
                break;
            case "smalldatetime":
                dbType = System.Data.SqlDbType.SmallDateTime;
                break;
            case "smallint":
                dbType = System.Data.SqlDbType.SmallInt;
                break;
            case "text":
                dbType = System.Data.SqlDbType.Text;
                break;
            case "bigint":
                dbType = System.Data.SqlDbType.BigInt;
                break;
            case "binary":
                dbType = System.Data.SqlDbType.Binary;
                break;
            case "char":
                dbType = System.Data.SqlDbType.Char;
                break;
            case "nchar":
                dbType = System.Data.SqlDbType.NChar;
                break;
            case "numeric":
                dbType = System.Data.SqlDbType.Decimal;
                break;
            case "real":
                dbType = System.Data.SqlDbType.Real;
                break;
            case "smallmoney":
                dbType = System.Data.SqlDbType.SmallMoney;
                break;
            case "sql_variant":
                dbType = System.Data.SqlDbType.Variant;
                break;
            case "timestamp":
                dbType = System.Data.SqlDbType.Timestamp;
                break;
            case "tinyint":
                dbType = System.Data.SqlDbType.TinyInt;
                break;
            case "uniqueidentifier":
                dbType = System.Data.SqlDbType.UniqueIdentifier;
                break;
            case "varbinary":
                dbType = System.Data.SqlDbType.VarBinary;
                break;
            case "xml":
                dbType = System.Data.SqlDbType.Xml;
                break;
        }
        return dbType;
    }
    #endregion

    #region string转换为System.Type
    /// <summary>
    /// string转换为System.Type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static System.Type ToType(this string type)
    {
        System.Type result;
        switch (type)
        {
            case "string":
                result = typeof(string);
                break;
            case "String":
                result = typeof(String);
                break;
            case "int":
                result = typeof(int);
                break;
            case "Int16":
                result = typeof(Int16);
                break;
            case "Int32":
                result = typeof(Int32);
                break;
            case "Int64":
                result = typeof(Int64);
                break;
            case "short":
                result = typeof(short);
                break;
            case "long":
                result = typeof(long);
                break;
            case "bool":
                result = typeof(bool);
                break;
            case "Boolean":
                result = typeof(Boolean);
                break;
            case "object":
                result = typeof(object);
                break;
            case "Object":
                result = typeof(Object);
                break;
            case "char":
                result = typeof(char);
                break;
            case "Char":
                result = typeof(Char);
                break;
            case "DBNull":
                result = typeof(DBNull);
                break;
            case "DateTime":
                result = typeof(DateTime);
                break;
            case "decimal":
                result = typeof(decimal);
                break;
            case "Decimal":
                result = typeof(Decimal);
                break;
            case "double":
                result = typeof(double);
                break;
            case "Double":
                result = typeof(Double);
                break;
            case "float":
                result = typeof(float);
                break;
            case "byte":
                result = typeof(byte);
                break;
            case "Byte":
                result = typeof(Byte);
                break;
            case "sbyte":
                result = typeof(sbyte);
                break;
            case "SByte":
                result = typeof(SByte);
                break;
            case "Single":
                result = typeof(Single);
                break;
            case "uint":
                result = typeof(uint);
                break;
            case "UInt16":
                result = typeof(UInt16);
                break;
            case "UInt32":
                result = typeof(UInt32);
                break;
            case "UInt64":
                result = typeof(UInt64);
                break;
            case "ulong":
                result = typeof(ulong);
                break;
            case "ushort":
                result = typeof(ushort);
                break;
            default:
                result = typeof(string);
                break;
        }
        return result;
    }
    #endregion

    #region 根据类全名在当前程序集里获取System.Type
    /// <summary>
    /// 根据类全名在当前程序集里获取System.Type
    /// </summary>
    /// <param name="p_TypeFullName">类全名 System.Windows.Forms.TextBox</param>
    /// <returns>Type</returns>
    public static Type GetTypeByFullName(this string p_TypeFullName)
    {
        Type _TypeInfo = Type.GetType(p_TypeFullName);
        if (_TypeInfo != null) return _TypeInfo;
        System.Reflection.Assembly[] _Assembly = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i != _Assembly.Length; i++)
        {
            _TypeInfo = _Assembly[i].GetType(p_TypeFullName);
            if (_TypeInfo != null) return _TypeInfo;
        }
        return null;
    }
    #endregion



    /// <summary>
    /// 实现各进制数间的转换。ToScale("15",10,16)表示将十进制数15转换为16进制的数。
    /// </summary>
    /// <param name="strValue">要转换的值,即原值</param>
    /// <param name="iFromScale">原值的进制,只能是2,8,10,16四个值。</param>
    /// <param name="iToScale">要转换到的目标进制，只能是2,8,10,16四个值。</param>
    public static string ToScale(this string strValue, int iFromScale, int iToScale)
    {
        int intValue = Convert.ToInt32(strValue, iFromScale);  //先转成10进制
        string result = Convert.ToString(intValue, iToScale);  //再转成目标进制
        if (iToScale == 2)
        {
            int resultLength = result.Length;  //获取二进制的长度
            switch (resultLength)
            {
                case 7:
                    result = "0" + result;
                    break;
                case 6:
                    result = "00" + result;
                    break;
                case 5:
                    result = "000" + result;
                    break;
                case 4:
                    result = "0000" + result;
                    break;
                case 3:
                    result = "00000" + result;
                    break;
            }
        }
        return result;
    }

    public static string TrimLower(this String str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return "";

        return str.Trim().ToLowerInvariant();
    }
    public static string TrimUpper(this String str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return "";

        return str.Trim().ToUpperInvariant();
    }



}