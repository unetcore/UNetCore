using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
/// <summary>
/// 	Extension methods for the root data type object
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// 	Determines whether the object is equal to any of the provided values.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "obj">The object to be compared.</param>
    /// <param name = "values">The values to compare with the object.</param>
    /// <returns></returns>
    public static bool EqualsAny<T>(this T obj, params T[] values)
    {
        return (Array.IndexOf(values, obj) != -1);
    }

    /// <summary>
    /// 	Determines whether the object is equal to none of the provided values.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "obj">The object to be compared.</param>
    /// <param name = "values">The values to compare with the object.</param>
    /// <returns></returns>
    public static bool EqualsNone<T>(this T obj, params T[] values)
    {
        return (obj.EqualsAny(values) == false);
    }

    /// <summary>
    /// 	Converts an object to the specified target type or returns the default value if
    ///     those 2 types are not convertible.
    ///     <para>
    ///     If the <paramref name="value"/> can't be convert even if the types are 
    ///     convertible with each other, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>The target type</returns>
    public static T ConvertTo<T>(this object value)
    {
        return value.ConvertTo(default(T));
    }

    /// <summary>
    /// 	Converts an object to the specified target type or returns the default value.
    ///     <para>Any exceptions are ignored. </para>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>The target type</returns>
    public static T ConvertToAndIgnoreException<T>(this object value)
    {
        return value.ConvertToAndIgnoreException(default(T));
    }

    /// <summary>
    /// 	Converts an object to the specified target type or returns the default value.
    ///     <para>Any exceptions are ignored. </para>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The target type</returns>
    public static T ConvertToAndIgnoreException<T>(this object value, T defaultValue)
    {
        return value.ConvertTo(defaultValue, true);
    }

    /// <summary>
    /// 	Converts an object to the specified target type or returns the default value if
    ///     those 2 types are not convertible.
    ///     <para>
    ///     If the <paramref name="value"/> can't be convert even if the types are 
    ///     convertible with each other, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <returns>The target type</returns>
    public static T ConvertTo<T>(this object value, T defaultValue)
    {
        if (value != null)
        {
            var targetType = typeof(T);

            if (value.GetType() == targetType) return (T)value;

            var converter = TypeDescriptor.GetConverter(value);
            if (converter != null)
            {
                if (converter.CanConvertTo(targetType))
                    return (T)converter.ConvertTo(value, targetType);
            }

            converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null)
            {
                if (converter.CanConvertFrom(value.GetType()))
                    return (T)converter.ConvertFrom(value);
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// 	Converts an object to the specified target type or returns the default value if
    ///     those 2 types are not convertible.
    ///     <para>Any exceptions are optionally ignored (<paramref name="ignoreException"/>).</para>
    ///     <para>
    ///     If the exceptions are not ignored and the <paramref name="value"/> can't be convert even if 
    ///     the types are convertible with each other, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <param name = "defaultValue">The default value.</param>
    /// <param name = "ignoreException">if set to <c>true</c> ignore any exception.</param>
    /// <returns>The target type</returns>
    public static T ConvertTo<T>(this object value, T defaultValue, bool ignoreException)
    {
        if (ignoreException)
        {
            try
            {
                return value.ConvertTo<T>();
            }
            catch
            {
                return defaultValue;
            }
        }
        return value.ConvertTo<T>();
    }

    /// <summary>
    /// 	Determines whether the value can (in theory) be converted to the specified target type.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can be convert to the specified target type; otherwise, <c>false</c>.
    /// </returns>
    public static bool CanConvertTo<T>(this object value)
    {
        if (value != null)
        {
            var targetType = typeof(T);

            var converter = TypeDescriptor.GetConverter(value);
            if (converter != null)
            {
                if (converter.CanConvertTo(targetType))
                    return true;
            }

            converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null)
            {
                if (converter.CanConvertFrom(value.GetType()))
                    return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 	Dynamically invokes a method using reflection
    /// </summary>
    /// <param name = "obj">The object to perform on.</param>
    /// <param name = "methodName">The name of the method.</param>
    /// <param name = "parameters">The parameters passed to the method.</param>
    /// <returns>The return value</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
    /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
    /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
    /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
    /// 		Console.WriteLine(reader.ReadToEnd());
    /// 		reader.Close();
    /// 		}
    /// 	</code>
    /// </example>
    public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
    {
        return InvokeMethod<object>(obj, methodName, parameters);
    }

    /// <summary>
    /// 	Dynamically invokes a method using reflection and returns its value in a typed manner
    /// </summary>
    /// <typeparam name = "T">The expected return data types</typeparam>
    /// <param name = "obj">The object to perform on.</param>
    /// <param name = "methodName">The name of the method.</param>
    /// <param name = "parameters">The parameters passed to the method.</param>
    /// <returns>The return value</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
    /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
    /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
    /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
    /// 		Console.WriteLine(reader.ReadToEnd());
    /// 		reader.Close();
    /// 		}
    /// 	</code>
    /// </example>
    public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
    {
        var type = obj.GetType();
        var method = type.GetMethod(methodName, parameters.Select(o => o.GetType()).ToArray());

        if (method == null)
            throw new ArgumentException(string.Format("Method '{0}' not found.", methodName), methodName);

        var value = method.Invoke(obj, parameters);
        return (value is T ? (T)value : default(T));
    }

    /// <summary>
    /// 	Dynamically retrieves a property value.
    /// </summary>
    /// <param name = "obj">The object to perform on.</param>
    /// <param name = "propertyName">The Name of the property.</param>
    /// <returns>The property value.</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
    /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
    /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
    /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
    /// 		Console.WriteLine(reader.ReadToEnd());
    /// 		reader.Close();
    /// 		}
    /// 	</code>
    /// </example>
    public static object GetPropertyValue(this object obj, string propertyName)
    {
        return GetPropertyValue<object>(obj, propertyName, null);
    }

    /// <summary>
    /// 	Dynamically retrieves a property value.
    /// </summary>
    /// <typeparam name = "T">The expected return data type</typeparam>
    /// <param name = "obj">The object to perform on.</param>
    /// <param name = "propertyName">The Name of the property.</param>
    /// <returns>The property value.</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
    /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
    /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
    /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
    /// 		Console.WriteLine(reader.ReadToEnd());
    /// 		reader.Close();
    /// 		}
    /// 	</code>
    /// </example>
    public static T GetPropertyValue<T>(this object obj, string propertyName)
    {
        return GetPropertyValue(obj, propertyName, default(T));
    }

    /// <summary>
    /// 	Dynamically retrieves a property value.
    /// </summary>
    /// <typeparam name = "T">The expected return data type</typeparam>
    /// <param name = "obj">The object to perform on.</param>
    /// <param name = "propertyName">The Name of the property.</param>
    /// <param name = "defaultValue">The default value to return.</param>
    /// <returns>The property value.</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
    /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
    /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
    /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
    /// 		Console.WriteLine(reader.ReadToEnd());
    /// 		reader.Close();
    /// 		}
    /// 	</code>
    /// </example>
    public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue)
    {
        var type = obj.GetType();
        var property = type.GetProperty(propertyName);

        if (property == null)
            throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);

        var value = property.GetValue(obj, null);
        return (value is T ? (T)value : defaultValue);
    }

    /// <summary>
    /// 	Dynamically sets a property value.
    /// </summary>
    /// <param name = "obj">The object to perform on.</param>
    /// <param name = "propertyName">The Name of the property.</param>
    /// <param name = "value">The value to be set.</param>
    public static void SetPropertyValue(this object obj, string propertyName, object value)
    {
        var type = obj.GetType();
        var property = type.GetProperty(propertyName);

        if (property == null)
            throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);
        if (!property.CanWrite)
            throw new ArgumentException(string.Format("Property '{0}' does not allow writes.", propertyName), propertyName);
        property.SetValue(obj, value, null);
    }

    /// <summary>
    /// 	Gets the first matching attribute defined on the data type.
    /// </summary>
    /// <typeparam name = "T">The attribute type tp look for.</typeparam>
    /// <param name = "obj">The object to look on.</param>
    /// <returns>The found attribute</returns>
    public static T GetAttribute<T>(this object obj) where T : Attribute
    {
        return GetAttribute<T>(obj, true);
    }

    /// <summary>
    /// 	Gets the first matching attribute defined on the data type.
    /// </summary>
    /// <typeparam name = "T">The attribute type tp look for.</typeparam>
    /// <param name = "obj">The object to look on.</param>
    /// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
    /// <returns>The found attribute</returns>
    public static T GetAttribute<T>(this object obj, bool includeInherited) where T : Attribute
    {
        var type = (obj as Type ?? obj.GetType());
        var attributes = type.GetCustomAttributes(typeof(T), includeInherited);
        return attributes.FirstOrDefault() as T;
    }

    /// <summary>
    /// 	Gets all matching attribute defined on the data type.
    /// </summary>
    /// <typeparam name = "T">The attribute type tp look for.</typeparam>
    /// <param name = "obj">The object to look on.</param>
    /// <returns>The found attributes</returns>
    public static IEnumerable<T> GetAttributes<T>(this object obj) where T : Attribute
    {
        return GetAttributes<T>(obj, false);
    }

    /// <summary>
    /// 	Gets all matching attribute defined on the data type.
    /// </summary>
    /// <typeparam name = "T">The attribute type tp look for.</typeparam>
    /// <param name = "obj">The object to look on.</param>
    /// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
    /// <returns>The found attributes</returns>
    public static IEnumerable<T> GetAttributes<T>(this object obj, bool includeInherited) where T : Attribute
    {
        return (obj as Type ?? obj.GetType()).GetCustomAttributes(typeof(T), includeInherited).OfType<T>().Select(attribute => attribute);
    }

    /// <summary>
    /// 	Determines whether the object is exactly of the passed generic type.
    /// </summary>
    /// <typeparam name = "T">The target type.</typeparam>
    /// <param name = "obj">The object to check.</param>
    /// <returns>
    /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOfType<T>(this object obj)
    {
        return obj.IsOfType(typeof(T));
    }

    /// <summary>
    /// 	Determines whether the object is excactly of the passed type
    /// </summary>
    /// <param name = "obj">The object to check.</param>
    /// <param name = "type">The target type.</param>
    /// <returns>
    /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOfType(this object obj, Type type)
    {
        return (obj.GetType().Equals(type));
    }

    /// <summary>
    /// 	Determines whether the object is of the passed generic type or inherits from it.
    /// </summary>
    /// <typeparam name = "T">The target type.</typeparam>
    /// <param name = "obj">The object to check.</param>
    /// <returns>
    /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOfTypeOrInherits<T>(this object obj)
    {
        return obj.IsOfTypeOrInherits(typeof(T));
    }

    /// <summary>
    /// 	Determines whether the object is of the passed type or inherits from it.
    /// </summary>
    /// <param name = "obj">The object to check.</param>
    /// <param name = "type">The target type.</param>
    /// <returns>
    /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOfTypeOrInherits(this object obj, Type type)
    {
        var objectType = obj.GetType();

        do
        {
            if (objectType.Equals(type))
                return true;
            if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
                return false;
            objectType = objectType.BaseType;
        } while (true);
    }

    /// <summary>
    /// 	Determines whether the object is assignable to the passed generic type.
    /// </summary>
    /// <typeparam name = "T">The target type.</typeparam>
    /// <param name = "obj">The object to check.</param>
    /// <returns>
    /// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsAssignableTo<T>(this object obj)
    {
        return obj.IsAssignableTo(typeof(T));
    }

    /// <summary>
    /// 	Determines whether the object is assignable to the passed type.
    /// </summary>
    /// <param name = "obj">The object to check.</param>
    /// <param name = "type">The target type.</param>
    /// <returns>
    /// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsAssignableTo(this object obj, Type type)
    {
        var objectType = obj.GetType();
        return type.IsAssignableFrom(objectType);
    }

    /// <summary>
    /// 	Gets the type default value for the underlying data type, in case of reference types: null
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>The default value</returns>
    public static T GetTypeDefaultValue<T>(this T value)
    {
        return default(T);
    }

    /// <summary>
    /// 	Converts the specified value to a database value and returns DBNull.Value if the value equals its default.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns></returns>
    public static object ToDatabaseValue<T>(this T value)
    {
        return (value.Equals(value.GetTypeDefaultValue()) ? DBNull.Value : (object)value);
    }



    /// <summary>
    /// 	Returns TRUE, if specified target reference is equals with null reference.
    /// 	Othervise returns FALSE.
    /// </summary>
    /// <param name = "target">Target reference. Can be null.</param>
    /// <remarks>
    /// 	Some types has overloaded '==' and '!=' operators.
    /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
    /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
    /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
    /// 

    /// </remarks>
    /// <example>
    /// 	object someObject = GetSomeObject();
    /// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
    /// 	else { /* the someObject is not null */ }
    /// </example>
    public static bool IsNull(this object target)
    {
        var ret = IsNull<object>(target);
        return ret;
    }

    /// <summary>
    /// 	Returns TRUE, if specified target reference is equals with null reference.
    /// 	Othervise returns FALSE.
    /// </summary>
    /// <typeparam name = "T">Type of target.</typeparam>
    /// <param name = "target">Target reference. Can be null.</param>
    /// <remarks>
    /// 	Some types has overloaded '==' and '!=' operators.
    /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
    /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
    /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
    /// 

    /// </remarks>
    /// <example>
    /// 	MyClass someObject = GetSomeObject();
    /// 	if ( someObject.IsNull() ) { /* the someObject is null */ }
    /// 	else { /* the someObject is not null */ }
    /// </example>
    public static bool IsNull<T>(this T target)
    {
        var result = ReferenceEquals(target, null);
        return result;
    }

    /// <summary>
    /// 	Returns TRUE, if specified target reference is equals with null reference.
    /// 	Othervise returns FALSE.
    /// </summary>
    /// <param name = "target">Target reference. Can be null.</param>
    /// <remarks>
    /// 	Some types has overloaded '==' and '!=' operators.
    /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
    /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
    /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
    /// 
    /// </remarks>
    /// <example>
    /// 	object someObject = GetSomeObject();
    /// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
    /// 	else { /* the someObject is null */ }
    /// </example>
    public static bool IsNotNull(this object target)
    {
        var ret = IsNotNull<object>(target);
        return ret;
    }

    /// <summary>
    /// 	Returns TRUE, if specified target reference is equals with null reference.
    /// 	Othervise returns FALSE.
    /// </summary>
    /// <typeparam name = "T">Type of target.</typeparam>
    /// <param name = "target">Target reference. Can be null.</param>
    /// <remarks>
    /// 	Some types has overloaded '==' and '!=' operators.
    /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
    /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(object, object)" method.
    /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
    /// 

    /// </remarks>
    /// <example>
    /// 	MyClass someObject = GetSomeObject();
    /// 	if ( someObject.IsNotNull() ) { /* the someObject is not null */ }
    /// 	else { /* the someObject is null */ }
    /// </example>
    public static bool IsNotNull<T>(this T target)
    {
        var result = !ReferenceEquals(target, null);
        return result;
    }

    /// <summary>
    /// 	If target is null, returns null.
    /// 	Othervise returns string representation of target using current culture format provider.
    /// </summary>
    /// <param name = "target">Target transforming to string representation. Can be null.</param>
    /// <example>
    /// 	float? number = null;
    /// 	string text1 = number.AsString();
    /// 
    /// 	number = 15.7892;
    /// 	string text2 = number.AsString();
    /// </example>
    /// <remarks>

    /// </remarks>
    public static string AsString(this object target)
    {
        return ReferenceEquals(target, null) ? null : string.Format("{0}", target);
    }

    /// <summary>
    /// 	If target is null, returns null.
    /// 	Othervise returns string representation of target using specified format provider.
    /// </summary>
    /// <param name = "target">Target transforming to string representation. Can be null.</param>
    /// <param name = "formatProvider">Format provider used to transformation target to string representation.</param>
    /// <example>
    /// 	CultureInfo czech = new CultureInfo("cs-CZ");
    /// 
    /// 	float? number = null;
    /// 	string text1 = number.AsString( czech );
    /// 
    /// 	number = 15.7892;
    /// 	string text2 = number.AsString( czech );
    /// </example>
    /// <remarks>

    /// </remarks>
    public static string AsString(this object target, IFormatProvider formatProvider)
    {
        var result = string.Format(formatProvider, "{0}", target);
        return result;
    }

    /// <summary>
    /// 	If target is null, returns null.
    /// 	Othervise returns string representation of target using invariant format provider.
    /// </summary>
    /// <param name = "target">Target transforming to string representation. Can be null.</param>
    /// <example>
    /// 	float? number = null;
    /// 	string text1 = number.AsInvariantString();
    /// 
    /// 	number = 15.7892;
    /// 	string text2 = number.AsInvariantString();
    /// </example>
    /// <remarks>

    /// </remarks>
    public static string AsInvariantString(this object target)
    {
        var result = string.Format(CultureInfo.InvariantCulture, "{0}", target);
        return result;
    }

    /// <summary>
    /// 	If target is null reference, returns notNullValue.
    /// 	Othervise returns target.
    /// </summary>
    /// <typeparam name = "T">Type of target.</typeparam>
    /// <param name = "target">Target which is maybe null. Can be null.</param>
    /// <param name = "notNullValue">Value used instead of null.</param>
    /// <example>
    /// 	const int DEFAULT_NUMBER = 123;
    /// 
    /// 	int? number = null;
    /// 	int notNullNumber1 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 123
    /// 
    /// 	number = 57;
    /// 	int notNullNumber2 = number.NotNull( DEFAULT_NUMBER ).Value; // returns 57
    /// </example>
    /// <remarks>

    /// </remarks>
    public static T NotNull<T>(this T target, T notNullValue)
    {
        return ReferenceEquals(target, null) ? notNullValue : target;
    }

    /// <summary>
    /// 	If target is null reference, returns result from notNullValueProvider.
    /// 	Othervise returns target.
    /// </summary>
    /// <typeparam name = "T">Type of target.</typeparam>
    /// <param name = "target">Target which is maybe null. Can be null.</param>
    /// <param name = "notNullValueProvider">Delegate which return value is used instead of null.</param>
    /// <example>
    /// 	int? number = null;
    /// 	int notNullNumber1 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns random number from 10 to 20
    /// 
    /// 	number = 57;
    /// 	int notNullNumber2 = number.NotNull( ()=> GetRandomNumber(10, 20) ).Value; // returns 57
    /// </example>
    /// <remarks>

    /// </remarks>
    public static T NotNull<T>(this T target, Func<T> notNullValueProvider)
    {
        return ReferenceEquals(target, null) ? notNullValueProvider() : target;
    }

    /// <summary>
    /// 	get a string representation of a given object.
    /// </summary>
    /// <param name = "o">the object to dump</param>
    /// <param name = "flags">BindingFlags to use for reflection</param>
    /// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
    /// <returns></returns>
    public static string ToStringDump(this object o, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
    {
        return ToStringDumpInternal(o.ToXElement(flags, maxArrayElements)).Aggregate(new StringBuilder(), (sb, el) => sb.Append(el)).ToString();
    }

    static IEnumerable<string> ToStringDumpInternal(XContainer toXElement)
    {
        foreach (var xElement in toXElement.Elements().OrderBy(o => o.Name.ToString()))
        {
            if (xElement.HasElements)
            {
                foreach (var el in ToStringDumpInternal(xElement))
                    yield return "{" + String.Format("{0}={1}", xElement.Name, el) + "}";
            }
            else
                yield return "{" + String.Format("{0}={1}", xElement.Name, xElement.Value) + "}";
        }
    }

    /// <summary>
    /// 	get a html-table representation of a given object.
    /// </summary>
    /// <param name = "o">the object to dump</param>
    /// <param name = "flags">BindingFlags to use for reflection</param>
    /// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
    /// <returns></returns>
    public static string ToHTMLTable(this object o, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
    {
        return ToHTMLTableInternal(o.ToXElement(flags, maxArrayElements), 0).Aggregate(String.Empty, (str, el) => str + el);
    }

    static IEnumerable<string> ToHTMLTableInternal(XContainer xel, int padding)
    {
        yield return FormatHTMLLine("<table>", padding);
        yield return FormatHTMLLine("<tr><th>Attribute</th><th>Value</th></tr>", padding + 1);
        foreach (var xElement in xel.Elements().OrderBy(o => o.Name.ToString()))
        {
            if (xElement.HasElements)
            {
                yield return FormatHTMLLine(String.Format("<tr><td>{0}</td><td>", xElement.Name), padding + 1);
                foreach (var el in ToHTMLTableInternal(xElement, padding + 2))
                    yield return el;
                yield return FormatHTMLLine("</td></tr>", padding + 1);
            }
            else
                yield return FormatHTMLLine(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", xElement.Name, (xElement.Value)), padding + 1);
        }
        yield return FormatHTMLLine("</table>", padding);
    }

    static string FormatHTMLLine(string tag, int padding)
    {
        return String.Format("{0}{1}{2}", String.Empty.PadRight(padding, '\t'), tag, Environment.NewLine);
    }

    /// <summary>
    /// 	get a XElement representation of a given object.
    /// </summary>
    /// <param name = "o">the object to dump</param>
    /// <param name = "flags">BindingFlags to use for reflection</param>
    /// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
    /// <returns></returns>
    public static XElement ToXElement(this object o, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
    {
        try
        {
            return ToXElementInternal(o, new HashSet<object>(), flags, maxArrayElements);
        }
        catch
        {
            return new XElement(o.GetType().Name);
        }
    }

    // todo: Please document these methods
    static XElement ToXElementInternal(object o, ICollection<object> visited, BindingFlags flags, int maxArrayElements)
    {
        if (o == null)
            return new XElement("null");
        if (visited.Contains(o))
            return new XElement("cyclicreference");

        if (!o.GetType().IsValueType)
            visited.Add(o);

        var type = o.GetType();
        var elems = new XElement(CleanName(type.Name, type.IsArray));

        if (!NeedRecursion(type, o))
        {
            elems.Add(new XElement(CleanName(type.Name, type.IsArray), String.Empty + o));
            return elems;
        }
        if (o is IEnumerable)
        {
            var i = 0;
            foreach (var el in o as IEnumerable)
            {
                var subtype = el.GetType();
                elems.Add(NeedRecursion(subtype, el) ? ToXElementInternal(el, visited, flags, maxArrayElements) : new XElement(CleanName(subtype.Name, subtype.IsArray), el));
                if (i++ >= maxArrayElements)
                    break;
            }
            return elems;
        }
        foreach (var propertyInfo in from propertyInfo in type.GetProperties(flags)
                                     where propertyInfo.CanRead
                                     select propertyInfo)
        {
            var value = GetValue(o, propertyInfo);
            elems.Add(NeedRecursion(propertyInfo.PropertyType, value)
                                                            ? new XElement(CleanName(propertyInfo.Name, propertyInfo.PropertyType.IsArray), ToXElementInternal(value, visited, flags, maxArrayElements))
                                                            : new XElement(CleanName(propertyInfo.Name, propertyInfo.PropertyType.IsArray), String.Empty + value));
        }
        foreach (var fieldInfo in type.GetFields())
        {
            var value = fieldInfo.GetValue(o);
            elems.Add(NeedRecursion(fieldInfo.FieldType, value)
                                                            ? new XElement(CleanName(fieldInfo.Name, fieldInfo.FieldType.IsArray), ToXElementInternal(value, visited, flags, maxArrayElements))
                                                            : new XElement(CleanName(fieldInfo.Name, fieldInfo.FieldType.IsArray), String.Empty + value));
        }
        return elems;
    }

    static bool NeedRecursion(Type type, object o)
    {
        return o != null && (!type.IsPrimitive && !(o is String || o is DateTime || o is DateTimeOffset || o is TimeSpan || o is Delegate || o is Enum || o is Decimal || o is Guid));
    }

    static object GetValue(object o, PropertyInfo propertyInfo)
    {
        object value;
        try
        {
            value = propertyInfo.GetValue(o, null);
        }
        catch
        {
            try
            {
                value = propertyInfo.GetValue(o,
                        new object[]
                {
                        0
                });
            }
            catch
            {
                value = null;
            }
        }
        return value;
    }

    static string CleanName(IEnumerable<char> name, bool isArray)
    {
        var sb = new StringBuilder();
        foreach (var c in name.Where(c => Char.IsLetterOrDigit(c) && c != '`').Select(c => c))
            sb.Append(c);
        if (isArray)
            sb.Append("Array");
        return sb.ToString();
    }

    /// <summary>
    /// 	Cast an object to the given type. Usefull especially for anonymous types.
    /// </summary>
    /// <param name="obj">The object to be cast</param>
    /// <param name="targetType">The type to cast to</param>
    /// <returns>
    /// 	the casted type or null if casting is not possible.
    /// </returns>
    /// <remarks>

    /// </remarks>
    public static object DynamicCast(this object obj, Type targetType)
    {
        // First, it might be just a simple situation
        if (targetType.IsAssignableFrom(obj.GetType()))
            return obj;

        // If not, we need to find a cast operator. The operator
        // may be explicit or implicit and may be included in
        // either of the two types...
        const BindingFlags pubStatBinding = BindingFlags.Public | BindingFlags.Static;
        var originType = obj.GetType();
        String[] names = { "op_Implicit", "op_Explicit" };

        var castMethod =
                targetType.GetMethods(pubStatBinding).Union(originType.GetMethods(pubStatBinding)).FirstOrDefault(
                        itm => itm.ReturnType.Equals(targetType) && itm.GetParameters().Length == 1 && itm.GetParameters()[0].ParameterType.IsAssignableFrom(originType) && names.Contains(itm.Name));
        if (null != castMethod)
            return castMethod.Invoke(null, new[] { obj });
        throw new InvalidOperationException(
                String.Format(
                        "No matching cast operator found from {0} to {1}.",
                        originType.Name,
                        targetType.Name));
    }

    /// <summary>
    /// Cast an object to the given type. Usefull especially for anonymous types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object to be cast</param>
    /// <returns>
    /// the casted type or null if casting is not possible.
    /// </returns>
    /// <remarks>
    /// </remarks>
    public static T CastAs<T>(this object obj) where T : class, new()
    {
        return obj as T;
    }

    /// <summary>
    /// Counts and returns the recursive execution of the passed function until it returns null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item to start peforming on.</param>
    /// <param name="function">The function to be executed.</param>
    /// <returns>The number of executions until the function returned null</returns>
    public static int CountLoopsToNull<T>(this T item, Func<T, T> function) where T : class
    {
        var num = 0;
        while ((item = function(item)) != null)
        {
            num++;
        }
        return num;
    }

    /// <summary>
    /// Finds a type instance using a recursive call. The method is useful to find specific parents for example.
    /// </summary>
    /// <typeparam name="T">The source type to perform on.</typeparam>
    /// <typeparam name="K">The targte type to be returned</typeparam>
    /// <param name="item">The item to start performing on.</param>
    /// <param name="function">The function to be executed.</param>
    /// <returns>An target type instance or null.</returns>
    /// <example><code>
    /// var tree = ...
    /// var node = tree.FindNodeByValue("");
    /// var parentByType = node.FindTypeByRecursion%lt;TheType&gt;(n => n.Parent);
    /// </code></example>
    public static K FindTypeByRecursion<T, K>(this T item, Func<T, T> function)
        where T : class
        where K : class, T
    {
        do
        {
            if (item is K) return (K)item;
        }
        while ((item = function(item)) != null);
        return null;
    }

    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T Clone<T>(this T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    /// <summary>
    /// Casts the specified object to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to cast to</typeparam>
    /// <param name="o">The Object being casted</param>
    /// <returns>returns the object as casted type.</returns>
    public static T CastTo<T>(this object o)
    {
        if (o == null)
            throw new NullReferenceException();
        return (T)Convert.ChangeType(o, typeof(T));
    }

    /// <summary>
    /// Casts the specified object. If the object is null a return type can be specified.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="o">The Object being casted</param>
    /// <param name="defaultValue">The default Type.</param>
    /// <returns>returns the object as casted type. If null the default type is returned.</returns>
    public static T CastTo<T>(this object o, T defaultValue)
    {
        if (o == null)
            return defaultValue;
        return (T)Convert.ChangeType(o, typeof(T));
    }

    /// <summary>
    /// Copies the readable and writable public property values from the source object to the target
    /// </summary>
    /// <remarks>The source and target objects must be of the same type.</remarks>
    /// <param name="target">The target object</param>
    /// <param name="source">The source object</param>
    public static void CopyPropertiesFrom(this object target, object source)
    {
        CopyPropertiesFrom(target, source, string.Empty);
    }

    /// <summary>
    /// Copies the readable and writable public property values from the source object to the target
    /// </summary>
    /// <remarks>The source and target objects must be of the same type.</remarks>
    /// <param name="target">The target object</param>
    /// <param name="source">The source object</param>
    /// <param name="ignoreProperty">A single property name to ignore</param>
    public static void CopyPropertiesFrom(this object target, object source, string ignoreProperty)
    {
        CopyPropertiesFrom(target, source, new[] { ignoreProperty });
    }

    /// <summary>
    /// Copies the readable and writable public property values from the source object to the target
    /// </summary>
    /// <remarks>The source and target objects must be of the same type.</remarks>
    /// <param name="target">The target object</param>
    /// <param name="source">The source object</param>
    /// <param name="ignoreProperties">An array of property names to ignore</param>
    public static void CopyPropertiesFrom(this object target, object source, string[] ignoreProperties)
    {
        // Get and check the object types
        Type type = source.GetType();
        if (target.GetType() != type)
        {
            throw new ArgumentException("The source type must be the same as the target");
        }

        // Build a clean list of property names to ignore
        var ignoreList = new List<string>();
        foreach (string item in ignoreProperties)
        {
            if (!string.IsNullOrEmpty(item) && !ignoreList.Contains(item))
            {
                ignoreList.Add(item);
            }
        }

        // Copy the properties
        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanWrite
                && property.CanRead
                && !ignoreList.Contains(property.Name))
            {
                object val = property.GetValue(source, null);
                property.SetValue(target, val, null);
            }
        }
    }

    /// <summary>
    /// Returns a string representation of the objects property values
    /// </summary>
    /// <param name="source">The object for the string representation</param>
    /// <returns>A string</returns>
    public static string ToPropertiesString(this object source)
    {
        return ToPropertiesString(source, Environment.NewLine);
    }

    /// <summary>
    /// Returns a string representation of the objects property values
    /// </summary>
    /// <param name="source">The object for the string representation</param>
    /// <param name="delimiter">The line terminstor string to use between properties</param>
    /// <returns>A string</returns>
    public static string ToPropertiesString(this object source, string delimiter)
    {
        if (source == null)
        {
            return string.Empty;
        }

        Type type = source.GetType();

        var sb = new StringBuilder(type.Name);
        sb.Append(delimiter);

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanWrite
                && property.CanRead)
            {
                object val = property.GetValue(source, null);
                sb.Append(property.Name);
                sb.Append(": ");
                sb.Append(val == null ? "[NULL]" : val.ToString());
                sb.Append(delimiter);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Serializes the object into an XML string, using the encoding method specified in
    /// <see cref="ExtensionMethodsSettings.DefaultEncoding"/>
    /// </summary>
    /// <remarks>
    /// The object to be serialized should be decorated with the 
    /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    /// <param name="source">The object to serialize</param>
    /// <returns>An XML encoded string representation of the source object</returns>
    public static string ToXml(this object source)
    {
        return ToXml(source, ExtensionMethodSetting.DefaultEncoding);
    }

    /// <summary>
    /// Serializes the object into an XML string
    /// </summary>
    /// <remarks>
    /// The object to be serialized should be decorated with the 
    /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    /// <param name="source">The object to serialize</param>
    /// <param name="encoding">The Encoding scheme to use when serializing the data to XML</param>
    /// <returns>An XML encoded string representation of the source object</returns>
    public static string ToXml(this object source, Encoding encoding)
    {
        if (source == null)
        {
            throw new ArgumentException("The source object cannot be null.");
        }

        if (encoding == null)
        {
            throw new Exception("You must specify an encoder to use for serialization.");
        }

        using (var stream = new MemoryStream())
        {
            var serializer = new XmlSerializer(source.GetType());
            serializer.Serialize(stream, source);
            stream.Position = 0;
            return encoding.GetString(stream.ToArray());
        }
    }

    /// <summary>
    /// Serializes the object into an XML string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static string ToXml<T>(this T @this)
    {
        if (@this == null) throw new NullReferenceException();

        XmlSerializer ser = new XmlSerializer(typeof(T));

        using (StringWriter writer = new StringWriter())
        {
            ser.Serialize(writer, @this);
            return writer.ToString();
        }
    }

    /// <summary>
    /// Throws an <see cref="System.ArgumentNullException"/> 
    /// if the the value is null.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="message">The message to display if the value is null.</param>
    /// <param name="name">The name of the parameter being tested.</param>
    public static void ExceptionIfNullOrEmpty(this object value, string message, string name)
    {
        if (value == null)
            throw new ArgumentNullException(name, message);
    }

    /// <summary>
    ///     Returns the  for the specified object.
    /// </summary>
    /// <param name="value">An object that implements the  interface.</param>
    /// <returns>The  for , or  if  is null.</returns>
    public static TypeCode GetTypeCode(this Object value)
    {
        return Convert.GetTypeCode(value);
    }
    /// <summary>
    ///     Gets the types of the objects in the specified array.
    /// </summary>
    /// <param name="args">An array of objects whose types to determine.</param>
    /// <returns>An array of  objects representing the types of the corresponding elements in .</returns>
    public static Type[] GetTypeArray(this Object[] args)
    {
        return Type.GetTypeArray(args);
    }


    #region 通用转换方法
    /// <summary>
    ///     An object extension method that converts the @this to a boolean.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a bool.</returns>
    public static bool ToBoolean(this object @this)
    {
        return Convert.ToBoolean(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a bool.</returns>
    public static bool ToBooleanOrDefault(this object @this)
    {
        try
        {
            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return default(bool);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">true to default value.</param>
    /// <returns>The given data converted to a bool.</returns>
    public static bool ToBooleanOrDefault(this object @this, bool defaultValue)
    {
        try
        {
            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }
    /// <summary>
    /// An object extension method that converts this object to a boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">true to default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a bool.</returns>
    public static bool ToBooleanOrDefault(this object @this, bool defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a bool.</returns>
    public static bool ToBooleanOrDefault(this object @this, Func<bool> defaultValueFactory)
    {
        try
        {
            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a bool.</returns>
    public static bool ToBooleanOrDefault(this object @this, Func<bool> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a byte.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a byte.</returns>
    public static byte ToByte(this object @this)
    {
        return Convert.ToByte(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a byte.</returns>
    public static byte ToByteOrDefault(this object @this)
    {
        try
        {
            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return default(byte);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a byte.</returns>
    public static byte ToByteOrDefault(this object @this, byte defaultValue)
    {
        try
        {
            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>An object extension method that converts this object to a byte or default.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a byte.</returns>
    public static byte ToByteOrDefault(this object @this, byte defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a byte.</returns>
    public static byte ToByteOrDefault(this object @this, Func<byte> defaultValueFactory)
    {
        try
        {
            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>An object extension method that converts this object to a byte or default.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a byte.</returns>
    public static byte ToByteOrDefault(this object @this, Func<byte> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a character.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a char.</returns>
    public static char ToChar(this object @this)
    {
        return Convert.ToChar(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a char.</returns>
    public static char ToCharOrDefault(this object @this)
    {
        try
        {
            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return default(char);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a char.</returns>
    public static char ToCharOrDefault(this object @this, char defaultValue)
    {
        try
        {
            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a char.</returns>
    public static char ToCharOrDefault(this object @this, char defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a char.</returns>
    public static char ToCharOrDefault(this object @this, Func<char> defaultValueFactory)
    {
        try
        {
            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a char.</returns>
    public static char ToCharOrDefault(this object @this, Func<char> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a date time.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a DateTime.</returns>
    public static DateTime ToDateTime(this object @this)
    {
        return Convert.ToDateTime(@this);
    }

    /// <summary>
    ///     An object extension method that converts the @this to a date time off set.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a DateTimeOffset.</returns>
    public static DateTimeOffset ToDateTimeOffSet(this object @this)
    {
        return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a DateTimeOffset.</returns>
    public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this)
    {
        try
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return default(DateTimeOffset);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a DateTimeOffset.</returns>
    public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, DateTimeOffset defaultValue)
    {
        try
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a DateTimeOffset.</returns>
    public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, DateTimeOffset defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a DateTimeOffset.</returns>
    public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset> defaultValueFactory)
    {
        try
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a DateTimeOffset.</returns>
    public static DateTimeOffset ToDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this)
    {
        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return default(DateTime);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this, DateTime defaultValue)
    {
        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this, DateTime defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this, Func<DateTime> defaultValueFactory)
    {
        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this, Func<DateTime> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a decimal.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a decimal.</returns>
    public static decimal ToDecimal(this object @this)
    {
        return Convert.ToDecimal(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a decimal.</returns>
    public static decimal ToDecimalOrDefault(this object @this)
    {
        try
        {
            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return default(decimal);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a decimal.</returns>
    public static decimal ToDecimalOrDefault(this object @this, decimal defaultValue)
    {
        try
        {
            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a decimal.</returns>
    public static decimal ToDecimalOrDefault(this object @this, decimal defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a decimal.</returns>
    public static decimal ToDecimalOrDefault(this object @this, Func<decimal> defaultValueFactory)
    {
        try
        {
            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a decimal.</returns>
    public static decimal ToDecimalOrDefault(this object @this, Func<decimal> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a double.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a double.</returns>
    public static double ToDouble(this object @this)
    {
        return Convert.ToDouble(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a double.</returns>
    public static double ToDoubleOrDefault(this object @this)
    {
        try
        {
            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return default(double);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a double.</returns>
    public static double ToDoubleOrDefault(this object @this, double defaultValue)
    {
        try
        {
            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a double.</returns>
    public static double ToDoubleOrDefault(this object @this, double defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a double.</returns>
    public static double ToDoubleOrDefault(this object @this, Func<double> defaultValueFactory)
    {
        try
        {
            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a double.</returns>
    public static double ToDoubleOrDefault(this object @this, Func<double> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a float.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a float.</returns>
    public static float ToFloat(this object @this)
    {
        return Convert.ToSingle(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToFloatOrDefault(this object @this)
    {
        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return default(float);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToFloatOrDefault(this object @this, float defaultValue)
    {
        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToFloatOrDefault(this object @this, float defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToFloatOrDefault(this object @this, Func<float> defaultValueFactory)
    {
        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToFloatOrDefault(this object @this, Func<float> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a unique identifier.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a GUID.</returns>
    public static Guid ToGuid(this object @this)
    {
        return new Guid(@this.ToString());
    }
    /// <summary>
    ///     An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this)
    {
        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this, Guid defaultValue)
    {
        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this, Guid defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this, Func<Guid> defaultValueFactory)
    {
        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this, Func<Guid> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an int 16.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a short.</returns>
    public static short ToInt16(this object @this)
    {
        return Convert.ToInt16(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return default(short);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this, short defaultValue)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this, short defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this, Func<short> defaultValueFactory)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this, Func<short> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an int 32.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an int.</returns>
    public static int ToInt32(this object @this)
    {
        return Convert.ToInt32(@this);
    }


    /// <summary>
    ///     An object extension method that converts this object to an int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an int.</returns>
    public static int ToInt32OrDefault(this object @this)
    {
        try
        {
            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return default(int);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an int.</returns>
    public static int ToInt32OrDefault(this object @this, int defaultValue)
    {
        try
        {
            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an int.</returns>
    public static int ToInt32OrDefault(this object @this, int defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an int.</returns>
    public static int ToInt32OrDefault(this object @this, Func<int> defaultValueFactory)
    {
        try
        {
            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an int.</returns>
    public static int ToInt32OrDefault(this object @this, Func<int> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an int 64.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a long.</returns>
    public static long ToInt64(this object @this)
    {
        return Convert.ToInt64(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to an int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToInt64OrDefault(this object @this)
    {
        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return default(long);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToInt64OrDefault(this object @this, long defaultValue)
    {
        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToInt64OrDefault(this object @this, long defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToInt64OrDefault(this object @this, Func<long> defaultValueFactory)
    {
        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToInt64OrDefault(this object @this, Func<long> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a long.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a long.</returns>
    public static long ToLong(this object @this)
    {
        return Convert.ToInt64(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToLongOrDefault(this object @this)
    {
        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return default(long);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToLongOrDefault(this object @this, long defaultValue)
    {
        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>An object extension method that converts this object to a long or default.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToLongOrDefault(this object @this, long defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToLongOrDefault(this object @this, Func<long> defaultValueFactory)
    {
        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>An object extension method that converts this object to a long or default.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a long.</returns>
    public static long ToLongOrDefault(this object @this, Func<long> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable boolean.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a bool?</returns>
    public static bool? ToNullableBoolean(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToBoolean(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a bool?</returns>
    public static bool? ToNullableBooleanOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return default(bool);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a bool?</returns>
    public static bool? ToNullableBooleanOrDefault(this object @this, bool? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable boolean or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a bool?</returns>
    public static bool? ToNullableBooleanOrDefault(this object @this, Func<bool?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToBoolean(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable byte.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a byte?</returns>
    public static byte? ToNullableByte(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToByte(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a byte?</returns>
    public static byte? ToNullableByteOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return default(byte);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a byte?</returns>
    public static byte? ToNullableByteOrDefault(this object @this, byte? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a byte?</returns>
    public static byte? ToNullableByteOrDefault(this object @this, Func<byte?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToByte(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable character.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a char?</returns>
    public static char? ToNullableChar(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToChar(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a char?</returns>
    public static char? ToNullableCharOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return default(char);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a char?</returns>
    public static char? ToNullableCharOrDefault(this object @this, char? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable character or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a char?</returns>
    public static char? ToNullableCharOrDefault(this object @this, Func<char?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToChar(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable date time.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a DateTime?</returns>
    public static DateTime? ToNullableDateTime(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToDateTime(@this);
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable date time off set.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a DateTimeOffset?</returns>
    public static DateTimeOffset? ToNullableDateTimeOffSet(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a DateTimeOffset?</returns>
    public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return default(DateTimeOffset);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a DateTimeOffset?</returns>
    public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this, DateTimeOffset? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable date time off set or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a DateTimeOffset?</returns>
    public static DateTimeOffset? ToNullableDateTimeOffSetOrDefault(this object @this, Func<DateTimeOffset?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new DateTimeOffset(Convert.ToDateTime(@this), TimeSpan.Zero);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a DateTime?</returns>
    public static DateTime? ToNullableDateTimeOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return default(DateTime);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a DateTime?</returns>
    public static DateTime? ToNullableDateTimeOrDefault(this object @this, DateTime? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a DateTime?</returns>
    public static DateTime? ToNullableDateTimeOrDefault(this object @this, Func<DateTime?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable decimal.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a decimal?</returns>
    public static decimal? ToNullableDecimal(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToDecimal(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a decimal?</returns>
    public static decimal? ToNullableDecimalOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return default(decimal);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a decimal?</returns>
    public static decimal? ToNullableDecimalOrDefault(this object @this, decimal? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable decimal or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a decimal?</returns>
    public static decimal? ToNullableDecimalOrDefault(this object @this, Func<decimal?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable double.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a double?</returns>
    public static double? ToNullableDouble(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToDouble(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a double?</returns>
    public static double? ToNullableDoubleOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return default(double);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a double?</returns>
    public static double? ToNullableDoubleOrDefault(this object @this, double? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable double or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a double?</returns>
    public static double? ToNullableDoubleOrDefault(this object @this, Func<double?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable float.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a float?</returns>
    public static float? ToNullableFloat(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToSingle(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a float?</returns>
    public static float? ToNullableFloatOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return default(float);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a float?</returns>
    public static float? ToNullableFloatOrDefault(this object @this, float? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable float or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a float?</returns>
    public static float? ToNullableFloatOrDefault(this object @this, Func<float?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable unique identifier.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a Guid?</returns>
    public static Guid? ToNullableGuid(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return new Guid(@this.ToString());
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a Guid?</returns>
    public static Guid? ToNullableGuidOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a Guid?</returns>
    public static Guid? ToNullableGuidOrDefault(this object @this, Guid? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a Guid?</returns>
    public static Guid? ToNullableGuidOrDefault(this object @this, Func<Guid?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable int 16.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a short?</returns>
    public static short? ToNullableInt16(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToInt16(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a short?</returns>
    public static short? ToNullableInt16OrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return default(short);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a short?</returns>
    public static short? ToNullableInt16OrDefault(this object @this, short? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a short?</returns>
    public static short? ToNullableInt16OrDefault(this object @this, Func<short?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable int 32.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an int?</returns>
    public static int? ToNullableInt32(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToInt32(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an int?</returns>
    public static int? ToNullableInt32OrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return default(int);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an int?</returns>
    public static int? ToNullableInt32OrDefault(this object @this, int? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an int?</returns>
    public static int? ToNullableInt32OrDefault(this object @this, Func<int?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable int 64.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a long?</returns>
    public static long? ToNullableInt64(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToInt64(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a long?</returns>
    public static long? ToNullableInt64OrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return default(long);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a long?</returns>
    public static long? ToNullableInt64OrDefault(this object @this, long? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a long?</returns>
    public static long? ToNullableInt64OrDefault(this object @this, Func<long?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable long.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a long?</returns>
    public static long? ToNullableLong(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToInt64(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a long?</returns>
    public static long? ToNullableLongOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return default(long);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a long?</returns>
    public static long? ToNullableLongOrDefault(this object @this, long? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a long?</returns>
    public static long? ToNullableLongOrDefault(this object @this, Func<long?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable s byte.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a sbyte?</returns>
    public static sbyte? ToNullableSByte(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToSByte(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a sbyte?</returns>
    public static sbyte? ToNullableSByteOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return default(sbyte);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a sbyte?</returns>
    public static sbyte? ToNullableSByteOrDefault(this object @this, sbyte? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a sbyte?</returns>
    public static sbyte? ToNullableSByteOrDefault(this object @this, Func<sbyte?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable short.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a short?</returns>
    public static short? ToNullableShort(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToInt16(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a short?</returns>
    public static short? ToNullableShortOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return default(short);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a short?</returns>
    public static short? ToNullableShortOrDefault(this object @this, short? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a short?</returns>
    public static short? ToNullableShortOrDefault(this object @this, Func<short?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable single.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a float?</returns>
    public static float? ToNullableSingle(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToSingle(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a float?</returns>
    public static float? ToNullableSingleOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return default(float);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a float?</returns>
    public static float? ToNullableSingleOrDefault(this object @this, float? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a float?</returns>
    public static float? ToNullableSingleOrDefault(this object @this, Func<float?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable u int 16.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ushort?</returns>
    public static ushort? ToNullableUInt16(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToUInt16(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ushort?</returns>
    public static ushort? ToNullableUInt16OrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return default(ushort);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ushort?</returns>
    public static ushort? ToNullableUInt16OrDefault(this object @this, ushort? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ushort?</returns>
    public static ushort? ToNullableUInt16OrDefault(this object @this, Func<ushort?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable u int 32.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an uint?</returns>
    public static uint? ToNullableUInt32(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToUInt32(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an uint?</returns>
    public static uint? ToNullableUInt32OrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return default(uint);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an uint?</returns>
    public static uint? ToNullableUInt32OrDefault(this object @this, uint? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an uint?</returns>
    public static uint? ToNullableUInt32OrDefault(this object @this, Func<uint?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to a nullable u int 64.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ulong?</returns>
    public static ulong? ToNullableUInt64(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToUInt64(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ulong?</returns>
    public static ulong? ToNullableUInt64OrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return default(ulong);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ulong?</returns>
    public static ulong? ToNullableUInt64OrDefault(this object @this, ulong? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ulong?</returns>
    public static ulong? ToNullableUInt64OrDefault(this object @this, Func<ulong?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable u long.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ulong?</returns>
    public static ulong? ToNullableULong(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToUInt64(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ulong?</returns>
    public static ulong? ToNullableULongOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return default(ulong);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ulong?</returns>
    public static ulong? ToNullableULongOrDefault(this object @this, ulong? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ulong?</returns>
    public static ulong? ToNullableULongOrDefault(this object @this, Func<ulong?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a nullable u short.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ushort?</returns>
    public static ushort? ToNullableUShort(this object @this)
    {
        if (@this == null || @this == DBNull.Value)
        {
            return null;
        }

        return Convert.ToUInt16(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a nullable u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ushort?</returns>
    public static ushort? ToNullableUShortOrDefault(this object @this)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return default(ushort);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ushort?</returns>
    public static ushort? ToNullableUShortOrDefault(this object @this, ushort? defaultValue)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a nullable u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ushort?</returns>
    public static ushort? ToNullableUShortOrDefault(this object @this, Func<ushort?> defaultValueFactory)
    {
        try
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to the s byte.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a sbyte.</returns>
    public static sbyte ToSByte(this object @this)
    {
        return Convert.ToSByte(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to the s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a sbyte.</returns>
    public static sbyte ToSByteOrDefault(this object @this)
    {
        try
        {
            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return default(sbyte);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to the s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a sbyte.</returns>
    public static sbyte ToSByteOrDefault(this object @this, sbyte defaultValue)
    {
        try
        {
            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to the s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a sbyte.</returns>
    public static sbyte ToSByteOrDefault(this object @this, sbyte defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to the s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a sbyte.</returns>
    public static sbyte ToSByteOrDefault(this object @this, Func<sbyte> defaultValueFactory)
    {
        try
        {
            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to the s byte or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a sbyte.</returns>
    public static sbyte ToSByteOrDefault(this object @this, Func<sbyte> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToSByte(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a short.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a short.</returns>
    public static short ToShort(this object @this)
    {
        return Convert.ToInt16(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToShortOrDefault(this object @this)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return default(short);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToShortOrDefault(this object @this, short defaultValue)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToShortOrDefault(this object @this, short defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToShortOrDefault(this object @this, Func<short> defaultValueFactory)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToShortOrDefault(this object @this, Func<short> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to a single.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a float.</returns>
    public static float ToSingle(this object @this)
    {
        return Convert.ToSingle(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToSingleOrDefault(this object @this)
    {
        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return default(float);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToSingleOrDefault(this object @this, float defaultValue)
    {
        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToSingleOrDefault(this object @this, float defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToSingleOrDefault(this object @this, Func<float> defaultValueFactory)
    {
        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a single or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a float.</returns>
    public static float ToSingleOrDefault(this object @this, Func<float> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToSingle(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that convert this object into a string representation.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string that represents this object.</returns>
    public static string ToString(this object @this)
    {
        return Convert.ToString(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this)
    {
        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return default(string);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this, string defaultValue)
    {
        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this, string defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this, Func<string> defaultValueFactory)
    {
        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this, Func<string> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an u int 16.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ushort.</returns>
    public static ushort ToUInt16(this object @this)
    {
        return Convert.ToUInt16(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUInt16OrDefault(this object @this)
    {
        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return default(ushort);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUInt16OrDefault(this object @this, ushort defaultValue)
    {
        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUInt16OrDefault(this object @this, ushort defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUInt16OrDefault(this object @this, Func<ushort> defaultValueFactory)
    {
        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUInt16OrDefault(this object @this, Func<ushort> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an u int 32.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an uint.</returns>
    public static uint ToUInt32(this object @this)
    {
        return Convert.ToUInt32(@this);
    }
    /// <summary>
    ///     An object extension method that converts this object to an u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an uint.</returns>
    public static uint ToUInt32OrDefault(this object @this)
    {
        try
        {
            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return default(uint);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an uint.</returns>
    public static uint ToUInt32OrDefault(this object @this, uint defaultValue)
    {
        try
        {
            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an uint.</returns>
    public static uint ToUInt32OrDefault(this object @this, uint defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an uint.</returns>
    public static uint ToUInt32OrDefault(this object @this, Func<uint> defaultValueFactory)
    {
        try
        {
            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u int 32 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an uint.</returns>
    public static uint ToUInt32OrDefault(this object @this, Func<uint> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToUInt32(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an u int 64.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ulong.</returns>
    public static ulong ToUInt64(this object @this)
    {
        return Convert.ToUInt64(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToUInt64OrDefault(this object @this)
    {
        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return default(ulong);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToUInt64OrDefault(this object @this, ulong defaultValue)
    {
        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToUInt64OrDefault(this object @this, ulong defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToUInt64OrDefault(this object @this, Func<ulong> defaultValueFactory)
    {
        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u int 64 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToUInt64OrDefault(this object @this, Func<ulong> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
    /// <summary>
    ///     An object extension method that converts the @this to an u long.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ulong.</returns>
    public static ulong ToULong(this object @this)
    {
        return Convert.ToUInt64(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to an u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToULongOrDefault(this object @this)
    {
        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return default(ulong);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToULongOrDefault(this object @this, ulong defaultValue)
    {
        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToULongOrDefault(this object @this, ulong defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToULongOrDefault(this object @this, Func<ulong> defaultValueFactory)
    {
        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u long or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ulong.</returns>
    public static ulong ToULongOrDefault(this object @this, Func<ulong> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToUInt64(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    ///     An object extension method that converts the @this to an u short.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an ushort.</returns>
    public static ushort ToUShort(this object @this)
    {
        return Convert.ToUInt16(@this);
    }

    /// <summary>
    ///     An object extension method that converts this object to an u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUShortOrDefault(this object @this)
    {
        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return default(ushort);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUShortOrDefault(this object @this, ushort defaultValue)
    {
        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUShortOrDefault(this object @this, ushort defaultValue, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValue;
        }

        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUShortOrDefault(this object @this, Func<ushort> defaultValueFactory)
    {
        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    /// <summary>
    /// An object extension method that converts this object to an u short or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <param name="useDefaultIfNull">true to use default if null.</param>
    /// <returns>The given data converted to an ushort.</returns>
    public static ushort ToUShortOrDefault(this object @this, Func<ushort> defaultValueFactory, bool useDefaultIfNull)
    {
        if (useDefaultIfNull && @this == null)
        {
            return defaultValueFactory();
        }

        try
        {
            return Convert.ToUInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }

    #endregion



}