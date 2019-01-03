using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Dynamic;
/// <summary>
/// DynamicExtensions
/// </summary>
public static class DynamicExtensions
{

    /// <summary>
    /// 转换为动态类型
    /// </summary>
    /// <param name="o">The object to convert.</param>
    /// <returns>a new expando object with the values of the passed in object</returns>
    public static dynamic ToDynamic(this object o)
    {
        if (o is ExpandoObject)
        {
            return o;
        }
        var result = new ExpandoObject();
        var d = (IDictionary<string, object>)result; //work with the Expando as a Dictionary
        if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
        {
            var nv = (NameValueCollection)o;
            nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
        }
        else
        {
            var props = o.GetType().GetProperties();
            foreach (var item in props)
            {
                d.Add(item.Name, item.GetValue(o, null));
            }
        }
        return result;
    }



}

