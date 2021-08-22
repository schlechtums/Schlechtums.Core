using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Does a TryGetValue on a dictionary.  If it doesn't exist, returns defaultValue, otherwise returns the value that was found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict">The dictionary in which to search.</param>
        /// <param name="key">The key value to find.</param>
        /// <param name="defaultValue">The default value to return if the key isn't found.  Defaults to default(TValue).</param>
        /// <returns>The TValue found at key or the default value provided.</returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue ret;
            if (dict.TryGetValue(key, out ret))
                return ret;
            else
                return defaultValue;
        }

        /// <summary>
        /// Does a TryGetValue on a dictionary.  If it doesn't exist or the dictionary is null, returns defaultValue, otherwise returns the value that was found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key">The key value to find.</param>
        /// <param name="defaultValue">The default value to return if the key isn't found or the dictionary is null.  Defaults to default(TValue).</param>
        /// <returns>The TValue found at key or the default value provided.</returns>
        public static TValue TryGetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            if (dict == null)
                return defaultValue;

            return dict.TryGetValue(key, defaultValue);
        }
    }
}