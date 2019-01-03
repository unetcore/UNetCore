using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;

    /// <summary>
    /// 	Extension methods for Dictionary.
    /// </summary>
    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Guard.ArgumentNull(dictionary, "dictionary", null);
            dictionary[key] = value;
            return dictionary;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            Guard.ArgumentNull(dictionary, "dictionary", null);
            if (!dictionary.ContainsKey(key))
            {
                return defaultValue;
            }
            return dictionary[key];
        }
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetValue<TKey, TValue>(dictionary, key, default(TValue));
        }
     
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            Guard.ArgumentNull(dictionary, "dictionary", null);
            Guard.ArgumentNull(valueFactory, "valueFactory", null);
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, valueFactory());
                return true;
            }
            return false;
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Guard.ArgumentNull(dictionary, "dictionary", null);
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }
            return false;
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> func)
        {
            TValue local;
            if (!dictionary.TryGetValue(key, out local))
            {
                Guard.ArgumentNull(func, "func", null);
                local = func();
                dictionary.Add(key, local);
            }
            return local;
        }

        /// <summary>
        /// Sorts the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return new SortedDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Sorts the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to be sorted.</param>
        /// <param name="comparer">The comparer used to sort dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return new SortedDictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Sorts the dictionary by value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return (new SortedDictionary<TKey, TValue>(dictionary)).OrderBy(kvp => kvp.Value).ToDictionary(item => item.Key, item => item.Value);
        }

        /// <summary>
        /// Inverts the specified dictionary. (Creates a new dictionary with the values as key, and key as values)
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static IDictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            return dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Creates a (non-generic) Hashtable from the Dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static Hashtable ToHashTable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var table = new Hashtable();

            foreach (var item in dictionary)
                table.Add(item.Key, item.Value);

            return table;
        }

        /// <summary>
        /// Returns the value of the first entry found with one of the <paramref name="keys"/> received.
        /// <para>Returns <paramref name="defaultValue"/> if none of the keys exists in this collection </para>
        /// </summary>
        /// <param name="defaultValue">Default value if none of the keys </param>
        /// <param name="keys"> keys to search for (in order) </param>
        public static TValue GetFirstValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue defaultValue, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                if (dictionary.ContainsKey(key))
                    return dictionary[key];
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value associated with the specified key, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TKey">The key data type</typeparam>
        /// <typeparam name="TValue">The value data type</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key of interest.</param>
        /// <returns>The value associated with the specified key if the key is found, the default value for the value data type if the key is not found</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.GetOrDefault(key, default(TValue));
        }

        /// <summary>
        /// Returns the value associated with the specified key, or the specified default value if no element is found.
        /// </summary>
        /// <typeparam name="TKey">The key data type</typeparam>
        /// <typeparam name="TValue">The value data type</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key of interest.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the specified key if the key is found, the specified default value if the key is not found</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            TValue value;
            return source.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// Returns the value associated with the specified key, or throw the specified exception if no element is found.
        /// </summary>
        /// <typeparam name="TKey">The key data type</typeparam>
        /// <typeparam name="TValue">The value data type</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key of interest.</param>
        /// <param name="exception">The exception to throw if the key is not found.</param>
        /// <returns>The value associated with the specified key if the key is found, the specified exception is thrown if the key is not found</returns>
        public static TValue GetOrThrow<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Exception exception)
        {
            TValue value;
            if (source.TryGetValue(key, out value))
            {
                return value;
            }

            throw exception;
        }

        /// <summary>
        /// Tests if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        public static bool IsEmpty(this IDictionary collection)
        {
            collection.ExceptionIfNullOrEmpty("The collection cannot be null.", "collection");

            return collection.Count == 0;
        }

        /// <summary>
        /// Tests if the IDictionary is empty.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of 
        /// the IDictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values
        /// of the IDictionary.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            collection.ExceptionIfNullOrEmpty(
                "The collection cannot be null.",
                "collection");

            return collection.Count == 0;
        }


        #region From Z.Extensions

        /// <summary>
        ///     An IDictionary&lt;string,string&gt; extension method that converts the @this to a name value collection.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a NameValueCollection.</returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string> @this)
        {
            if (@this == null)
            {
                return null;
            }

            var col = new NameValueCollection();
            foreach (var item in @this)
            {
                col.Add(item.Key, item.Value);
            }
            return col;
        }

        /// <summary>
        ///     An IDictionary&lt;string,object&gt; extension method that converts the @this to an expando.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an ExpandoObject.</returns>
        public static ExpandoObject ToExpando(this IDictionary<string, object> @this)
        {
            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)expando;

            foreach (var item in @this)
            {
                if (item.Value is IDictionary<string, object>)
                {
                    var d = (IDictionary<string, object>)item.Value;
                    expandoDict.Add(item.Key, d.ToExpando());
                }
                else
                {
                    expandoDict.Add(item);
                }
            }

            return expando;
        }

        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that adds if not contains key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(key, value);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that adds if not contains key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TValue> valueFactory)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(key, valueFactory());
                return true;
            }

            return false;
        }

        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that adds if not contains key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(key, valueFactory(key));
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Uses the specified functions to add a key/value pair to the IDictionary&lt;TKey, TValue&gt; if the key does
        ///     not already exist, or to update a key/value pair in the IDictionary&lt;TKey, TValue&gt;> if the key already
        ///     exists.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="value">The value to be added or updated.</param>
        /// <returns>The new value for the key.</returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                @this[key] = value;
            }

            return @this[key];
        }

        /// <summary>
        ///     Uses the specified functions to add a key/value pair to the IDictionary&lt;TKey, TValue&gt; if the key does
        ///     not already exist, or to update a key/value pair in the IDictionary&lt;TKey, TValue&gt;> if the key already
        ///     exists.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="addValue">The value to be added for an absent key.</param>
        /// <param name="updateValueFactory">
        ///     The function used to generate a new value for an existing key based on the key's
        ///     existing value.
        /// </param>
        /// <returns>
        ///     The new value for the key. This will be either be addValue (if the key was absent) or the result of
        ///     updateValueFactory (if the key was present).
        /// </returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, addValue));
            }
            else
            {
                @this[key] = updateValueFactory(key, @this[key]);
            }

            return @this[key];
        }

        /// <summary>
        ///     Uses the specified functions to add a key/value pair to the IDictionary&lt;TKey, TValue&gt; if the key does
        ///     not already exist, or to update a key/value pair in the IDictionary&lt;TKey, TValue&gt;> if the key already
        ///     exists.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
        /// <param name="updateValueFactory">
        ///     The function used to generate a new value for an existing key based on the key's
        ///     existing value.
        /// </param>
        /// <returns>
        ///     The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or
        ///     the result of updateValueFactory (if the key was present).
        /// </returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, addValueFactory(key)));
            }
            else
            {
                @this[key] = updateValueFactory(key, @this[key]);
            }

            return @this[key];
        }
        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that query if '@this' contains all key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="keys">A variable-length parameters list containing keys.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAllKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, params TKey[] keys)
        {
            foreach (TKey value in keys)
            {
                if (!@this.ContainsKey(value))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that query if '@this' contains any key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="keys">A variable-length parameters list containing keys.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAnyKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, params TKey[] keys)
        {
            foreach (TKey value in keys)
            {
                if (@this.ContainsKey(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Adds a key/value pair to the IDictionary&lt;TKey, TValue&gt; if the key does not already exist.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value to be added, if the key does not already exist.</param>
        /// <returns>
        ///     The value for the key. This will be either the existing value for the key if the key is already in the
        ///     dictionary, or the new value if the key was not in the dictionary.
        /// </returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, value));
            }

            return @this[key];
        }

        /// <summary>
        ///     Adds a key/value pair to the IDictionary&lt;TKey, TValue&gt; by using the specified function, if the key does
        ///     not already exist.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">TThe function used to generate a value for the key.</param>
        /// <returns>
        ///     The value for the key. This will be either the existing value for the key if the key is already in the
        ///     dictionary, or the new value for the key as returned by valueFactory if the key was not in the dictionary.
        /// </returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (!@this.ContainsKey(key))
            {
                @this.Add(new KeyValuePair<TKey, TValue>(key, valueFactory(key)));
            }

            return @this[key];
        }

        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that removes if contains key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        public static void RemoveIfContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            if (@this.ContainsKey(key))
            {
                @this.Remove(key);
            }
        }
        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that converts the @this to a sorted dictionary.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a SortedDictionary&lt;TKey,TValue&gt;</returns>
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IDictionary<TKey, TValue> @this)
        {
            return new SortedDictionary<TKey, TValue>(@this);
        }

        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that converts the @this to a sorted dictionary.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>@this as a SortedDictionary&lt;TKey,TValue&gt;</returns>
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IDictionary<TKey, TValue> @this, IComparer<TKey> comparer)
        {
            return new SortedDictionary<TKey, TValue>(@this, comparer);
        }
        /// <summary>
        ///     An IDictionary extension method that converts the @this to a hashtable.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Hashtable.</returns>
        public static Hashtable ToHashtable(this IDictionary @this)
        {
            return new Hashtable(@this);
        }
        /// <summary>
        ///     A NameValueCollection extension method that converts the @this to a dictionary.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an IDictionary&lt;string,object&gt;</returns>
        public static IDictionary<string, object> ToDictionary(this NameValueCollection @this)
        {
            var dict = new Dictionary<string, object>();

            if (@this != null)
            {
                foreach (string key in @this.AllKeys)
                {
                    dict.Add(key, @this[key]);
                }
            }

            return dict;
        }
        #endregion


    }