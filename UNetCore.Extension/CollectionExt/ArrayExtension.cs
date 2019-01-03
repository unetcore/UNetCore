using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
/// <summary>
/// 数组类型拓展
/// </summary>
public static class ArrayExtension
{

    /// <summary>
    /// 验证数组是否为空
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this Array source)
    {
        return source == null || source.Length == 0;
    }

    ///<summary>
    ///	验证指定的索引是否在数组中
    ///</summary>
    ///<param name = "source"></param>
    ///<param name = "index"></param>
    ///<returns></returns>
    /// <remarks> 
    /// </remarks>
    public static bool WithinIndex(this Array source, int index)
    {
        return source != null && index >= 0 && index < source.Length;
    }

    ///<summary>
    ///	验证指定的索引是否在数组中
    ///</summary>
    ///<param name = "source"></param>
    ///<param name = "index"></param>
    ///<param name="dimension"></param>
    ///<returns></returns>
    /// <remarks> 
    /// </remarks>
    public static bool WithinIndex(this Array source, int index, int dimension = 0)
    {
        return source != null && index >= source.GetLowerBound(dimension) && index <= source.GetUpperBound(dimension);
    }


    /// <summary>
    /// 组合两个数组到一个新的数组
    /// </summary>
    /// <typeparam name="T">Type of Array</typeparam>
    /// <param name="combineWith">Base array in which arrayToCombine will add.</param>
    /// <param name="arrayToCombine">Array to combine with Base array.</param>
    /// <returns></returns>
    /// <example>
    /// 	<code>
    /// 		int[] arrayOne = new[] { 1, 2, 3, 4 };
    /// 		int[] arrayTwo = new[] { 5, 6, 7, 8 };
    /// 		Array combinedArray = arrayOne.CombineArray<int>(arrayTwo);
    /// 	</code>
    /// </example> 
    public static T[] CombineArray<T>(this T[] combineWith, T[] arrayToCombine)
    {
        if (combineWith != default(T[]) && arrayToCombine != default(T[]))
        {
            int initialSize = combineWith.Length;
            Array.Resize<T>(ref combineWith, initialSize + arrayToCombine.Length);
            Array.Copy(arrayToCombine, arrayToCombine.GetLowerBound(0), combineWith, initialSize, arrayToCombine.Length);
        }
        return combineWith;
    }

    /// <summary>
    /// 清空数组内容
    /// </summary>
    /// <param name="clear"> The array to clear</param>
    /// <returns>Cleared array</returns>
    /// <example>
    ///     <code>
    ///         Array array = Array.CreateInstance(typeof(string), 2);
    ///         array.SetValue("One", 0); array.SetValue("Two", 1);
    ///         Array arrayToClear = array.ClearAll();
    ///     </code>
    /// </example>
    /// <remarks>
    /// </remarks>
    public static Array ClearAll(this Array clear)
    {
        if (clear != null)
            Array.Clear(clear, 0, clear.Length);
        return clear;
    }

    /// <summary>
    /// 清空数组内容
    /// </summary>
    /// <typeparam name="T">The type of array</typeparam>
    /// <param name="clear"> The array to clear</param>
    /// <returns>Cleared array</returns>
    /// <example>
    ///     <code>
    ///         int[] result = new[] { 1, 2, 3, 4 }.ClearAll<int>();
    ///     </code>
    /// </example>
    /// <remarks> 
    /// </remarks>
    public static T[] ClearAll<T>(this T[] arrayToClear)
    {
        if (arrayToClear != null)
            for (int i = arrayToClear.GetLowerBound(0); i <= arrayToClear.GetUpperBound(0); ++i)
                arrayToClear[i] = default(T);
        return arrayToClear;
    }

    /// <summary>
    /// 移除指定索引的数组项
    /// </summary>
    /// <param name="arrayToClear">The array in where to clean the item.</param>
    /// <param name="at">Which element to clear.</param>
    /// <returns></returns>
    /// <example>
    ///     <code>
    ///         Array array = Array.CreateInstance(typeof(string), 2);
    ///         array.SetValue("One", 0); array.SetValue("Two", 1);
    ///         Array result = array.ClearAt(2);
    ///     </code>
    /// </example>
    /// <remarks> 
    /// </remarks>
    public static Array ClearAt(this Array arrayToClear, int at)
    {
        if (arrayToClear != null)
        {
            int arrayIndex = at.GetArrayIndex();
            if (arrayIndex.IsIndexInArray(arrayToClear))
                Array.Clear(arrayToClear, arrayIndex, 1);
        }
        return arrayToClear;
    }

    /// <summary>
    /// 移除指定索引的数组项
    /// </summary>
    /// <typeparam name="T">The type of array</typeparam>
    /// <param name="arrayToClear">Array to clear.</param>
    /// <param name="at">Which element to clear.</param>
    /// <returns></returns>
    /// <example>
    ///     <code>
    ///           string[] clearString = new[] { "A" }.ClearAt<string>(0);
    ///     </code>
    /// </example>
    /// <remarks> 
    /// </remarks>
    public static T[] ClearAt<T>(this T[] arrayToClear, int at)
    {
        if (arrayToClear != null)
        {
            int arrayIndex = at.GetArrayIndex();
            if (arrayIndex.IsIndexInArray(arrayToClear))
                arrayToClear[arrayIndex] = default(T);
        }
        return arrayToClear;
    }

    /// <summary>
    /// 检验数组是否为空，为空会抛出异常
    /// </summary>
    /// <param name="array">The array to test.</param>
    /// <returns>True if the array is empty.</returns>
    public static bool IsEmpty(this Array array)
    {
        array.ExceptionIfNullOrEmpty(
            "The array cannot be null.",
            "array");

        return array.Length == 0;
    }

    #region BlockCopy

    /// <summary>
    /// 复制数组内容到新数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <remarks>Contributed by Chris Gessler</remarks>
    public static T[] BlockCopy<T>(this T[] array, int index, int length)
    {
        return BlockCopy(array, index, length, false);
    }

    /// <summary>
    /// 复制数组内容到新数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <param name="padToLength"></param>
    /// <returns></returns> 
    public static T[] BlockCopy<T>(this T[] array, int index, int length, bool padToLength)
    {
        if (array == null) throw new NullReferenceException();

        int n = length;
        T[] b = null;

        if (array.Length < index + length)
        {
            n = array.Length - index;
            if (padToLength)
            {
                b = new T[length];
            }
        }

        if (b == null) b = new T[n];
        Array.Copy(array, index, b, 0, n);
        return b;
    }

    /// <summary>
    /// 复制数组内容到新数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="count"></param>
    /// <param name="padToLength"></param>
    /// <returns></returns>
    /// <remarks>Contributed by Chris Gessler</remarks>
    public static IEnumerable<T[]> BlockCopy<T>(this T[] array, int count, bool padToLength = false)
    {
        for (int i = 0; i < array.Length; i += count)
            yield return array.BlockCopy(i, count, padToLength);
    }


    #region Z.Extensions
    /// <summary>
    ///     Sets a range of elements in the  to zero, to false, or to null, depending on the element type.
    /// </summary>
    /// <param name="array">The  whose elements need to be cleared.</param>
    /// <param name="index">The starting index of the range of elements to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    public static void Clear(this Array array, Int32 index, Int32 length)
    {
        Array.Clear(array, index, length);
    }
    /// <summary>
    ///     Searches an entire one-dimensional sorted  for a specific element, using the  interface implemented by each
    ///     element of the  and by the specified object.
    /// </summary>
    /// <param name="array">The sorted one-dimensional  to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <returns>
    ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
    ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
    ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
    ///     bitwise complement of (the index of the last element plus 1).
    /// </returns>
    public static Int32 BinarySearch(this Array array, Object value)
    {
        return Array.BinarySearch(array, value);
    }

    /// <summary>
    ///     Searches a range of elements in a one-dimensional sorted  for a value, using the  interface implemented by
    ///     each element of the  and by the specified value.
    /// </summary>
    /// <param name="array">The sorted one-dimensional  to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <returns>
    ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
    ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
    ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
    ///     bitwise complement of (the index of the last element plus 1).
    /// </returns>
    public static Int32 BinarySearch(this Array array, Int32 index, Int32 length, Object value)
    {
        return Array.BinarySearch(array, index, length, value);
    }

    /// <summary>
    ///     Searches an entire one-dimensional sorted  for a value using the specified  interface.
    /// </summary>
    /// <param name="array">The sorted one-dimensional  to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">
    ///     The  implementation to use when comparing elements.-or- null to use the  implementation
    ///     of each element.
    /// </param>
    /// <returns>
    ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
    ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
    ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
    ///     bitwise complement of (the index of the last element plus 1).
    /// </returns>
    public static Int32 BinarySearch(this Array array, Object value, IComparer comparer)
    {
        return Array.BinarySearch(array, value, comparer);
    }

    /// <summary>
    ///     Searches a range of elements in a one-dimensional sorted  for a value, using the specified  interface.
    /// </summary>
    /// <param name="array">The sorted one-dimensional  to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">
    ///     The  implementation to use when comparing elements.-or- null to use the  implementation
    ///     of each element.
    /// </param>
    /// <returns>
    ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
    ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
    ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
    ///     bitwise complement of (the index of the last element plus 1).
    /// </returns>
    public static Int32 BinarySearch(this Array array, Int32 index, Int32 length, Object value, IComparer comparer)
    {
        return Array.BinarySearch(array, index, length, value, comparer);
    }
    /// <summary>
    ///     Copies a range of elements from an  starting at the specified source index and pastes them to another
    ///     starting at the specified destination index.  Guarantees that all changes are undone if the copy does not
    ///     succeed completely.
    /// </summary>
    /// <param name="sourceArray">The  that contains the data to copy.</param>
    /// <param name="sourceIndex">A 32-bit integer that represents the index in the  at which copying begins.</param>
    /// <param name="destinationArray">The  that receives the data.</param>
    /// <param name="destinationIndex">A 32-bit integer that represents the index in the  at which storing begins.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    public static void ConstrainedCopy(this Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length)
    {
        Array.ConstrainedCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
    }
    /// <summary>
    ///     Copies a range of elements from an  starting at the first element and pastes them into another  starting at
    ///     the first element. The length is specified as a 32-bit integer.
    /// </summary>
    /// <param name="sourceArray">The  that contains the data to copy.</param>
    /// <param name="destinationArray">The  that receives the data.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    public static void Copy(this Array sourceArray, Array destinationArray, Int32 length)
    {
        Array.Copy(sourceArray, destinationArray, length);
    }

    /// <summary>
    ///     Copies a range of elements from an  starting at the specified source index and pastes them to another
    ///     starting at the specified destination index. The length and the indexes are specified as 32-bit integers.
    /// </summary>
    /// <param name="sourceArray">The  that contains the data to copy.</param>
    /// <param name="sourceIndex">A 32-bit integer that represents the index in the  at which copying begins.</param>
    /// <param name="destinationArray">The  that receives the data.</param>
    /// <param name="destinationIndex">A 32-bit integer that represents the index in the  at which storing begins.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    public static void Copy(this Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length)
    {
        Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
    }

    /// <summary>
    ///     Copies a range of elements from an  starting at the first element and pastes them into another  starting at
    ///     the first element. The length is specified as a 64-bit integer.
    /// </summary>
    /// <param name="sourceArray">The  that contains the data to copy.</param>
    /// <param name="destinationArray">The  that receives the data.</param>
    /// <param name="length">
    ///     A 64-bit integer that represents the number of elements to copy. The integer must be between
    ///     zero and , inclusive.
    /// </param>
    public static void Copy(this Array sourceArray, Array destinationArray, Int64 length)
    {
        Array.Copy(sourceArray, destinationArray, length);
    }

    /// <summary>
    ///     Copies a range of elements from an  starting at the specified source index and pastes them to another
    ///     starting at the specified destination index. The length and the indexes are specified as 64-bit integers.
    /// </summary>
    /// <param name="sourceArray">The  that contains the data to copy.</param>
    /// <param name="sourceIndex">A 64-bit integer that represents the index in the  at which copying begins.</param>
    /// <param name="destinationArray">The  that receives the data.</param>
    /// <param name="destinationIndex">A 64-bit integer that represents the index in the  at which storing begins.</param>
    /// <param name="length">
    ///     A 64-bit integer that represents the number of elements to copy. The integer must be between
    ///     zero and , inclusive.
    /// </param>
    public static void Copy(this Array sourceArray, Int64 sourceIndex, Array destinationArray, Int64 destinationIndex, Int64 length)
    {
        Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
    }

    /// <summary>
    ///     Searches for the specified object and returns the index of the first occurrence within the entire one-
    ///     dimensional .
    /// </summary>
    /// <param name="array">The one-dimensional  to search.</param>
    /// <param name="value">The object to locate in .</param>
    /// <returns>
    ///     The index of the first occurrence of  within the entire , if found; otherwise, the lower bound of the array
    ///     minus 1.
    /// </returns>
    public static Int32 IndexOf(this Array array, Object value)
    {
        return Array.IndexOf(array, value);
    }

    /// <summary>
    ///     Searches for the specified object and returns the index of the first occurrence within the range of elements
    ///     in the one-dimensional  that extends from the specified index to the last element.
    /// </summary>
    /// <param name="array">The one-dimensional  to search.</param>
    /// <param name="value">The object to locate in .</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <returns>
    ///     The index of the first occurrence of  within the range of elements in  that extends from  to the last element,
    ///     if found; otherwise, the lower bound of the array minus 1.
    /// </returns>
    public static Int32 IndexOf(this Array array, Object value, Int32 startIndex)
    {
        return Array.IndexOf(array, value, startIndex);
    }

    /// <summary>
    ///     Searches for the specified object and returns the index of the first occurrence within the range of elements
    ///     in the one-dimensional  that starts at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="array">The one-dimensional  to search.</param>
    /// <param name="value">The object to locate in .</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <returns>
    ///     The index of the first occurrence of  within the range of elements in  that starts at  and contains the
    ///     number of elements specified in , if found; otherwise, the lower bound of the array minus 1.
    /// </returns>
    public static Int32 IndexOf(this Array array, Object value, Int32 startIndex, Int32 count)
    {
        return Array.IndexOf(array, value, startIndex, count);
    }

    /// <summary>
    ///     Searches for the specified object and returns the index of the last occurrence within the entire one-
    ///     dimensional .
    /// </summary>
    /// <param name="array">The one-dimensional  to search.</param>
    /// <param name="value">The object to locate in .</param>
    /// <returns>
    ///     The index of the last occurrence of  within the entire , if found; otherwise, the lower bound of the array
    ///     minus 1.
    /// </returns>
    public static Int32 LastIndexOf(this Array array, Object value)
    {
        return Array.LastIndexOf(array, value);
    }

    /// <summary>
    ///     Searches for the specified object and returns the index of the last occurrence within the range of elements
    ///     in the one-dimensional  that extends from the first element to the specified index.
    /// </summary>
    /// <param name="array">The one-dimensional  to search.</param>
    /// <param name="value">The object to locate in .</param>
    /// <param name="startIndex">The starting index of the backward search.</param>
    /// <returns>
    ///     The index of the last occurrence of  within the range of elements in  that extends from the first element to ,
    ///     if found; otherwise, the lower bound of the array minus 1.
    /// </returns>
    public static Int32 LastIndexOf(this Array array, Object value, Int32 startIndex)
    {
        return Array.LastIndexOf(array, value, startIndex);
    }

    /// <summary>
    ///     Searches for the specified object and returns the index of the last occurrence within the range of elements
    ///     in the one-dimensional  that contains the specified number of elements and ends at the specified index.
    /// </summary>
    /// <param name="array">The one-dimensional  to search.</param>
    /// <param name="value">The object to locate in .</param>
    /// <param name="startIndex">The starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <returns>
    ///     The index of the last occurrence of  within the range of elements in  that contains the number of elements
    ///     specified in  and ends at , if found; otherwise, the lower bound of the array minus 1.
    /// </returns>
    public static Int32 LastIndexOf(this Array array, Object value, Int32 startIndex, Int32 count)
    {
        return Array.LastIndexOf(array, value, startIndex, count);
    }
    /// <summary>
    ///     Reverses the sequence of the elements in the entire one-dimensional .
    /// </summary>
    /// <param name="array">The one-dimensional  to reverse.</param>
    public static void Reverse(this Array array)
    {
        Array.Reverse(array);
    }

    /// <summary>
    ///     Reverses the sequence of the elements in a range of elements in the one-dimensional .
    /// </summary>
    /// <param name="array">The one-dimensional  to reverse.</param>
    /// <param name="index">The starting index of the section to reverse.</param>
    /// <param name="length">The number of elements in the section to reverse.</param>
    public static void Reverse(this Array array, Int32 index, Int32 length)
    {
        Array.Reverse(array, index, length);
    }

    /// <summary>
    ///     Sorts the elements in an entire one-dimensional  using the  implementation of each element of the .
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    public static void Sort(this Array array)
    {
        Array.Sort(array);
    }

    /// <summary>
    ///     Sorts a pair of one-dimensional  objects (one contains the keys and the other contains the corresponding
    ///     items) based on the keys in the first  using the  implementation of each key.
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="items">
    ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
    ///     null to sort only the .
    /// </param>
    /// ###
    /// <param name="keys">The one-dimensional  that contains the keys to sort.</param>
    public static void Sort(this Array array, Array items)
    {
        Array.Sort(array, items);
    }

    /// <summary>
    ///     Sorts the elements in a range of elements in a one-dimensional  using the  implementation of each element of
    ///     the .
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    public static void Sort(this Array array, Int32 index, Int32 length)
    {
        Array.Sort(array, index, length);
    }

    /// <summary>
    ///     Sorts a range of elements in a pair of one-dimensional  objects (one contains the keys and the other contains
    ///     the corresponding items) based on the keys in the first  using the  implementation of each key.
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="items">
    ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
    ///     null to sort only the .
    /// </param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// ###
    /// <param name="keys">The one-dimensional  that contains the keys to sort.</param>
    public static void Sort(this Array array, Array items, Int32 index, Int32 length)
    {
        Array.Sort(array, items, index, length);
    }

    /// <summary>
    ///     Sorts the elements in a one-dimensional  using the specified .
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="comparer">
    ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
    ///     each element.
    /// </param>
    public static void Sort(this Array array, IComparer comparer)
    {
        Array.Sort(array, comparer);
    }

    /// <summary>
    ///     Sorts a pair of one-dimensional  objects (one contains the keys and the other contains the corresponding
    ///     items) based on the keys in the first  using the specified .
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="items">
    ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
    ///     null to sort only the .
    /// </param>
    /// <param name="comparer">
    ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
    ///     each element.
    /// </param>
    /// ###
    /// <param name="keys">The one-dimensional  that contains the keys to sort.</param>
    public static void Sort(this Array array, Array items, IComparer comparer)
    {
        Array.Sort(array, items, comparer);
    }

    /// <summary>
    ///     Sorts the elements in a range of elements in a one-dimensional  using the specified .
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">
    ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
    ///     each element.
    /// </param>
    public static void Sort(this Array array, Int32 index, Int32 length, IComparer comparer)
    {
        Array.Sort(array, index, length, comparer);
    }

    /// <summary>
    ///     Sorts a range of elements in a pair of one-dimensional  objects (one contains the keys and the other contains
    ///     the corresponding items) based on the keys in the first  using the specified .
    /// </summary>
    /// <param name="array">The one-dimensional  to sort.</param>
    /// <param name="items">
    ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
    ///     null to sort only the .
    /// </param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">
    ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
    ///     each element.
    /// </param>
    /// ###
    /// <param name="keys">The one-dimensional  that contains the keys to sort.</param>
    public static void Sort(this Array array, Array items, Int32 index, Int32 length, IComparer comparer)
    {
        Array.Sort(array, items, index, length, comparer);
    }


    /// <summary>
    ///     Copies a specified number of bytes from a source array starting at a particular offset to a destination array
    ///     starting at a particular offset.
    /// </summary>
    /// <param name="src">The source buffer.</param>
    /// <param name="srcOffset">The zero-based byte offset into .</param>
    /// <param name="dst">The destination buffer.</param>
    /// <param name="dstOffset">The zero-based byte offset into .</param>
    /// <param name="count">The number of bytes to copy.</param>
    public static void BlockCopy(this Array src, Int32 srcOffset, Array dst, Int32 dstOffset, Int32 count)
    {
        Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
    }
    /// <summary>
    ///     Returns the number of bytes in the specified array.
    /// </summary>
    /// <param name="array">An array.</param>
    /// <returns>The number of bytes in the array.</returns>
    public static Int32 ByteLength(this Array array)
    {
        return Buffer.ByteLength(array);
    }
    /// <summary>
    ///     Retrieves the byte at a specified location in a specified array.
    /// </summary>
    /// <param name="array">An array.</param>
    /// <param name="index">A location in the array.</param>
    /// <returns>Returns the  byte in the array.</returns>
    public static Byte GetByte(this Array array, Int32 index)
    {
        return Buffer.GetByte(array, index);
    }
    /// <summary>
    ///     Assigns a specified value to a byte at a particular location in a specified array.
    /// </summary>
    /// <param name="array">An array.</param>
    /// <param name="index">A location in the array.</param>
    /// <param name="value">A value to assign.</param>
    public static void SetByte(this Array array, Int32 index, Byte value)
    {
        Buffer.SetByte(array, index, value);
    }

    /// <summary>
    ///     A T[] extension method that true for all.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static Boolean TrueForAll<T>(this T[] array, Predicate<T> match)
    {
        return Array.TrueForAll(array, match);
    }
    /// <summary>
    ///     A T[] extension method that converts an array to a read only.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <returns>A list of.</returns>
    public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
    {
        return Array.AsReadOnly(array);
    }
    /// <summary>
    ///     A T[] extension method that exists.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static Boolean Exists<T>(this T[] array, Predicate<T> match)
    {
        return Array.Exists(array, match);
    }
    /// <summary>
    ///     A T[] extension method that searches for the first match.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>A T.</returns>
    public static T Find<T>(this T[] array, Predicate<T> match)
    {
        return Array.Find(array, match);
    }
    /// <summary>
    ///     A T[] extension method that searches for the first all.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found all.</returns>
    public static T[] FindAll<T>(this T[] array, Predicate<T> match)
    {
        return Array.FindAll(array, match);
    }
    /// <summary>
    ///     A T[] extension method that searches for the first index.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found index.</returns>
    public static Int32 FindIndex<T>(this T[] array, Predicate<T> match)
    {
        return Array.FindIndex(array, match);
    }

    /// <summary>
    ///     A T[] extension method that searches for the first index.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found index.</returns>
    public static Int32 FindIndex<T>(this T[] array, Int32 startIndex, Predicate<T> match)
    {
        return Array.FindIndex(array, startIndex, match);
    }

    /// <summary>
    ///     A T[] extension method that searches for the first index.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="count">Number of.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found index.</returns>
    public static Int32 FindIndex<T>(this T[] array, Int32 startIndex, Int32 count, Predicate<T> match)
    {
        return Array.FindIndex(array, startIndex, count, match);
    }
    /// <summary>
    ///     A T[] extension method that searches for the first last.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found last.</returns>
    public static T FindLast<T>(this T[] array, Predicate<T> match)
    {
        return Array.FindLast(array, match);
    }
    /// <summary>
    ///     A T[] extension method that searches for the last index.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found index.</returns>
    public static Int32 FindLastIndex<T>(this T[] array, Predicate<T> match)
    {
        return Array.FindLastIndex(array, match);
    }

    /// <summary>
    ///     A T[] extension method that searches for the last index.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found index.</returns>
    public static Int32 FindLastIndex<T>(this T[] array, Int32 startIndex, Predicate<T> match)
    {
        return Array.FindLastIndex(array, startIndex, match);
    }

    /// <summary>
    ///     A T[] extension method that searches for the last index.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="array">The array to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="count">Number of.</param>
    /// <param name="match">Specifies the match.</param>
    /// <returns>The found index.</returns>
    public static Int32 FindLastIndex<T>(this T[] array, Int32 startIndex, Int32 count, Predicate<T> match)
    {
        return Array.FindLastIndex(array, startIndex, count, match);
    }
    #endregion

    #endregion



    #region 转换一维数组为二维数组
    /// <summary>
    /// 转换一维数组为二维数组
    /// </summary>
    /// <typeparam name="T">泛型用于数组</typeparam>
    /// <param name="Array">一维数组</param>
    /// <param name="iColumnCount">目标二维数组列数</param>
    /// <returns></returns>
    public static T[,] ToOneDimensionalArray<T>(this T[] Array, int iColumnCount)
    {
        int n = Array.Length;
        T[] a = new T[n];
        a = Array;
        int c = iColumnCount;
        int r = (int)Math.Ceiling((double)n / iColumnCount);
        T[,] b = new T[r, c];

        for (int i = 0; i < n; i++)
            b[i / c, i % c] = a[i];
        return b;
    }

    /// <summary>
    /// 转换一维数组为二维数组
    /// </summary>
    /// <typeparam name="T">泛型用于数组</typeparam>
    /// <param name="Array">一维数组</param>
    /// <param name="iColumnCount">目标二维数组列数</param>
    /// <returns></returns>
    public static T[,] ToTwoDimensionalArray<T>(this T[] Array, int iColumnCount)
    {
        int n = Array.Length;
        T[] a = new T[n];
        a = Array;
        int c = iColumnCount;
        int r = (int)Math.Ceiling((double)n / iColumnCount);
        T[,] b = new T[r, c];
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                b[i, j] = a[i * c + j];
            }
        }
        return b;
    }

    /// <summary>
    /// 转换二维数组为一维数组
    /// </summary>
    /// <typeparam name="T">泛型用于数组</typeparam>
    /// <param name="twoArray">二维数组</param>
    /// <returns></returns>
    public static T[] ToOneDimensionalArray<T>(this T[,] twoArray)
    {
        int r = twoArray.GetLength(0);//Row
        int c = twoArray.GetLength(1);//Column
        T[,] a = new T[r, c];
        a = twoArray;
        T[] b = new T[r * c];
        for (int i = 0; i < b.Length; i++)
            b[i] = a[i / c, i % c];
        return b;
    }
    #endregion


    #region 转换一个M行N列的二维数组为DataTable
    /// <summary>  
    /// 转换一个M行N列的二维数组为DataTable  
    /// </summary>  
    /// <param name="Arrays">M行N列的二维数组</param>  
    /// <returns>返回DataTable</returns>  
    public static DataTable ToDataTable<T>(this T[,] Arrays)
    {
        DataTable dt = new DataTable();

        int a = Arrays.GetLength(0);
        for (int i = 0; i < Arrays.GetLength(1); i++)
        {
            dt.Columns.Add("column" + i.ToString(), typeof(string));
        }

        for (int i1 = 0; i1 < Arrays.GetLength(0); i1++)
        {
            DataRow dr = dt.NewRow();
            for (int i = 0; i < Arrays.GetLength(1); i++)
            {
                dr[i] = Arrays[i1, i].ToString();
            }
            dt.Rows.Add(dr);
        }

        return dt;

    }
    #endregion

    #region 转换一个指定列M行N列的二维数组为DataTable
    /// <summary>  
    /// 转换一个M行N列的二维数组为DataTable  
    /// </summary>  
    /// <param name="ColumnNames">一维数组，代表列名，不能有重复值</param>  
    /// <param name="Arrays">M行N列的二维数组</param>  
    /// <returns>返回DataTable</returns>  
    public static DataTable ToDataTable<T>(this T[,] Arrays, string[] ColumnNames)
    {
        DataTable dt = new DataTable();

        foreach (string ColumnName in ColumnNames)
        {
            dt.Columns.Add(ColumnName, typeof(string));
        }
        for (int i1 = 0; i1 < Arrays.GetLength(0); i1++)
        {
            DataRow dr = dt.NewRow();
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                dr[i] = Arrays[i1, i].ToString();
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }
    #endregion

    #region 转换DataTable第一维数据到一维字符串数组
    /// <summary>
    /// 转换DataTable第一维数据到一维字符串数组
    /// </summary>
    /// <param name="dt">DataTable数据源</param>
    /// <returns></returns>
    public static object[] ToArray(this DataTable dt)
    {
        if (dt == null || dt.Rows.Count == 0) return new string[0];
        object[] sr = new object[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (Convert.IsDBNull(dt.Rows[i][0])) sr[i] = "";
            else sr[i] = dt.Rows[i][0];
        }
        return sr;

    }
    #endregion

    #region 转换DataTable指定维数据到一维字符串数组
    /// <summary>
    /// 转换DataTable指定维数据到一维字符串数组
    /// </summary>
    /// <param name="dt">DataTable数据源</param>
    /// <param name="sColumn">列名</param>
    /// <returns></returns>
    public static object[] ToArray(this DataTable dt, string sColumn)
    {
        if (dt == null || dt.Rows.Count == 0) return new string[0];
        object[] sr = new object[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (Convert.IsDBNull(dt.Rows[i][0])) sr[i] = "";
            else sr[i] = dt.Rows[i][sColumn];
        }
        return sr;

    }
    #endregion

    #region 转换DataTable第几维数据到一维字符串数组
    /// <summary>
    /// 转换DataTable第几维数据到一维字符串数组
    /// </summary>
    /// <param name="dt">DataTable数据源</param>
    /// <param name="iColumn">第几列</param>
    /// <returns></returns>
    public static object[] ToArray(this DataTable dt, int iColumn)
    {
        if (dt == null || dt.Rows.Count == 0) return new string[0];
        object[] sr = new object[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (Convert.IsDBNull(dt.Rows[i][0])) sr[i] = "";
            else sr[i] = dt.Rows[i][iColumn];
        }
        return sr;

    }
    #endregion


    #region 转换DataTable所有数据到二维字符串数组
    /// <summary>
    /// 转换DataTable所有数据到二维字符串数组
    /// </summary>
    /// <param name="dt">DataTable数据源</param>
    /// <returns></returns>
    public static object[,] ToTwoDimensionalArray(this DataTable dt)
    {
        if (dt == null || dt.Rows.Count == 0) return new string[0, 0];
        object[,] sr = new object[dt.Rows.Count, dt.Columns.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (Convert.IsDBNull(dt.Rows[i][j])) sr[i, j] = "";
                else sr[i, j] = dt.Rows[i][j];
            }

        }
        return sr;

    }
    #endregion

    #region 转换一维数组为DataTable
    /// <summary>  
    /// 转换一维数组为DataTable  
    /// </summary>  
    /// <param name="ColumnName">列名</param>  
    /// <param name="Array">一维数组</param>  
    /// <returns>返回DataTable</returns>  
    public static DataTable ToDataTable(this string[] Array, string ColumnName)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(ColumnName, typeof(string));
        for (int i = 0; i < Array.Length; i++)
        {
            DataRow dr = dt.NewRow();
            dr[ColumnName] = Array[i].ToString();
            dt.Rows.Add(dr);
        }
        return dt;
    }
    #endregion

    /// <summary>
    /// 格式化清空数组null或者empty元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Arrays"></param>
    /// <returns></returns>
    public static T[] RemoveNullItemInArray<T>(this T[] Arrays)
    {
        if (Arrays != null && Arrays.Length > 0)
        {

            int length = 0;
            T[] Tmps = Arrays;
            foreach (T item in Tmps)
            {
                if (!item.IsNullOrEmpty())
                {
                    length++;
                }
            }
            int i = 0;
            Arrays = new T[length];
            foreach (T item1 in Tmps)
            {
                if (!string.IsNullOrEmpty(item1.ToString()))
                {
                    Arrays[i++] = item1;
                }
            }
            return Arrays;

        }
        else
        {
            return Arrays;
        }
    }


}

