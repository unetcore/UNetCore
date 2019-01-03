using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// Serialization Extensions
/// </summary>
public static class SerializationExtensions
{
    #region JSON
    /// <summary>
    /// 序列化为Json字符串
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The Json string.</returns>
    public static string ToSerializeJson<T>(this T @this)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));

        using (var memoryStream = new MemoryStream())
        {
            serializer.WriteObject(memoryStream, @this);
            return Encoding.Default.GetString(memoryStream.ToArray());
        }
    }

    /// <summary>
    /// 序列化为Json字符串
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The Json string.</returns>
    public static string ToSerializeJson<T>(this T @this, Encoding encoding)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));

        using (var memoryStream = new MemoryStream())
        {
            serializer.WriteObject(memoryStream, @this);
            return encoding.GetString(memoryStream.ToArray());
        }
    }
    /// <summary>
    /// 解析Json字符串为指定对象
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The object deserialized.</returns>
    public static T ToDeserializeJson<T>(this string @this)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));

        using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
        {
            return (T)serializer.ReadObject(stream);
        }
    }

    /// <summary>
    /// 解析Json字符串为指定对象
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The object deserialized.</returns>
    public static T ToDeserializeJson<T>(this string @this, Encoding encoding)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));

        using (var stream = new MemoryStream(encoding.GetBytes(@this)))
        {
            return (T)serializer.ReadObject(stream);
        }
    }
    #endregion

    #region XML
    /// <summary>
    /// 序列化为Xml字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The string representation of the Xml Serialization.</returns>
    public static string ToSerializeXml(this object @this)
    {
        var xmlSerializer = new XmlSerializer(@this.GetType());

        using (var stringWriter = new StringWriter())
        {
            xmlSerializer.Serialize(stringWriter, @this);
            using (var streamReader = new StringReader(stringWriter.GetStringBuilder().ToString()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
    /// <summary>
    /// 解析Xml字符串为指定对象
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The desieralize Xml as &lt;T&gt;</returns>
    public static T ToDeserializeXml<T>(this string @this)
    {
        var x = new XmlSerializer(typeof(T));
        var r = new StringReader(@this);

        return (T)x.Deserialize(r);
    }
    #endregion

    #region Binary
    /// <summary>
    /// 序列化为二进制字符串
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    public static string ToSerializeBinary<T>(this T @this)
    {
        var binaryWrite = new BinaryFormatter();

        using (var memoryStream = new MemoryStream())
        {
            binaryWrite.Serialize(memoryStream, @this);
            return Encoding.Default.GetString(memoryStream.ToArray());
        }
    }

    /// <summary>
    /// 序列化为二进制字符串
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>A string.</returns>
    public static string ToSerializeBinary<T>(this T @this, Encoding encoding)
    {
        var binaryWrite = new BinaryFormatter();

        using (var memoryStream = new MemoryStream())
        {
            binaryWrite.Serialize(memoryStream, @this);
            return encoding.GetString(memoryStream.ToArray());
        }
    }
    /// <summary>
    ///  解析二进制字符串为指定对象
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The desrialize binary as &lt;T&gt;</returns>
    public static T ToDeserializeBinary<T>(this string @this)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
        {
            var binaryRead = new BinaryFormatter();
            return (T)binaryRead.Deserialize(stream);
        }
    }

    /// <summary>
    /// 解析二进制字符串为指定对象
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The desrialize binary as &lt;T&gt;</returns>
    public static T ToDeserializeBinary<T>(this string @this, Encoding encoding)
    {
        using (var stream = new MemoryStream(encoding.GetBytes(@this)))
        {
            var binaryRead = new BinaryFormatter();
            return (T)binaryRead.Deserialize(stream);
        }
    }
    #endregion

    #region JavaScript
    //    /// <summary>
    /////     A T extension method that serialize java script.
    ///// </summary>
    ///// <typeparam name="T">Generic type parameter.</typeparam>
    ///// <param name="this">The @this to act on.</param>
    ///// <returns>A string.</returns>
    //public static string ToSerializeJavaScript<T>(this T @this)
    //{
    //    var serializer = new JavaScriptSerializer();
    //    return serializer.Serialize(@this);
    //}
    ///// <summary>
    /////     A string extension method that deserialize a string binary as &lt;T&gt;.
    ///// </summary>
    ///// <typeparam name="T">Generic type parameter.</typeparam>
    ///// <param name="this">The @this to act on.</param>
    ///// <returns>The desrialize binary as &lt;T&gt;</returns>
    //public static T ToDeserializeJavaScript<T>(this string @this)
    //{
    //    var serializer = new JavaScriptSerializer();
    //    return serializer.Deserialize<T>(@this);
    //}
    #endregion

}
