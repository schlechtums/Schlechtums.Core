﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Schlechtums.Core.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Wrapper for String.Join so it can be called directly off of en enumeration of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="delimiter">The delimiter with which to join the string.</param>
        /// <returns>The objects joined with the delimiter.</returns>
        public static String Join<T>(this IEnumerable<T> objects, String delimiter)
        {
            return String.Join(delimiter, objects);
        }

        /// <summary>
        /// Wrapper for String.Join so it can be called directly off of en enumeration of objects.  Returns null if the enumeration is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="delimiter">The delimiter with which to join the string.</param>
        /// <returns>The objects joined with the delimiter or null if the enumeration is null.</returns>
        public static String JoinSafe<T>(this IEnumerable<T> objects, String delimiter)
        {
            if (objects == null)
                return null;

            return objects.Join(delimiter);
        }

        /// <summary>
        /// Returns if none of the elements match a condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns>True if no elements match the condition.</returns>
        public static Boolean None<T>(this IEnumerable<T> source, Func<T, Boolean> func)
        {
            return source.All(s => !func(s));
        }

        /// <summary>
        /// Returns true if an IEnumerable has no elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>True if there are no elements, false otherwise.</returns>
        public static Boolean None<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        /// <summary>
        /// Returns if none of the elements match a condition.  Returns true if the IEnumerable is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns>True if no elements match the condition or the IEnumerable is null.</returns>
        public static Boolean NoneSafe<T>(this IEnumerable<T> source, Func<T, Boolean> func)
        {
            if (source == null)
                return true;

            return source.None(func);
        }

        /// <summary>
        /// Returns true if an IEnumerable has no elements or if the IEnumerable is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>True if there are no elements or the IEnumerable is null.</returns>
        public static Boolean NoneSafe<T>(this IEnumerable<T> source)
        {
            return !source.AnySafe();
        }

        /// <summary>
        /// Returns if a collection has any matching elements.  If the source is null, returns false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Boolean AnySafe<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return false;

            return source.Any();
        }

        /// <summary>
        /// Returns if a collection has any matching elements.  If the source is null, returns false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Boolean AnySafe<T>(this IEnumerable<T> source, Func<T, Boolean> selector)
        {
            if (source == null)
                return false;

            return System.Linq.Enumerable.Any<T>(source, selector);
        }

        /// <summary>
        /// Returns if a collection has any matching elements based on a selector with a nullable Boolean return.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Boolean Any<T>(this IEnumerable<T> source, Func<T, Boolean?> selector)
        {
            return System.Linq.Enumerable.Any<T>(source, s => selector(s) == true);
        }

        /// <summary>
        /// Returns if a collection has any matching elements based on a selector with a nullable Boolean return.  If the source is null, returns false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Boolean AnySafe<T>(this IEnumerable<T> source, Func<T, Boolean?> selector)
        {
            if (source == null)
                return false;

            return source.Any(s => selector(s) == true);
        }

        /// <summary>
        /// Returns a Select, but if the source is null it returns new List&lt;TSelector&gt;.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TSelector> SelectSafe<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector)
        {
            if (source == null)
                return new List<TSelector>();

            return source.Select(selector);
        }

        /// <summary>
        /// Returns a filtered collection of elements based on the where selector.  If the source is null, returns a new List&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereSafe<T>(this IEnumerable<T> source, Func<T, Boolean> selector)
        {
            if (source == null)
                return new List<T>();

            return System.Linq.Enumerable.Where<T>(source, selector);
        }

        /// <summary>
        /// Returns a filtered collection of elements based on a where selector with a nullable Boolean return.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, Boolean?> selector)
        {
            return System.Linq.Enumerable.Where<T>(source, s => selector(s) == true);
        }

        /// <summary>
        /// Returns a filtered collection of elements based on a where selector with a nullable Boolean return.    If the source is null, returns a new List&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereSafe<T>(this IEnumerable<T> source, Func<T, Boolean?> selector)
        {
            if (source == null)
                return new List<T>();

            return source.Where(s => selector(s) == true);
        }

        /// <summary>
        /// Finds distinct elements based on a key selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            foreach (var g in source.GroupBy(selector))
                yield return g.First();
        }

        /// <summary>
        /// Returns the collection safe.  Can use in a foreach loop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list</param>
        /// <returns>The collection if not null, otherwise an empty List</returns>
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return new List<T>();
            else
                return source;
        }

        /// <summary>
        /// Performs a select based off of a distinct using a key selector.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector">Function to return the values to be distincted and then selected</param>
        /// <returns>An enumeration of the distinct values from the collection</returns>
        public static IEnumerable<TResult> SelectDistinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Distinct(selector).Select(selector);
        }

        /// <summary>
        /// SQL keyword IN.  Returns false if the array is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="set"></param>
        /// <returns>True/False</returns>
        public static Boolean InSafe<T>(this T obj, params T[] set)
        {
            if (set == null)
                return false;
            else
                return obj.In(set);
        }

        /// <summary>
        /// SQL keyword IN
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="set"></param>
        /// <returns>True/False</returns>
        public static Boolean In<T>(this T obj, params T[] set)
        {
            return set.Contains(obj);
        }

        /// <summary>
        /// SQL keyword IN.  Returns false if the HashSest is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="set"></param>
        /// <returns>True/False</returns>
        public static Boolean InSafe<T>(this T obj, HashSet<T> set)
        {
            if (set == null)
                return false;
            else
                return obj.In(set);
        }

        /// <summary>
        /// SQL keyword IN
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="set"></param>
        /// <returns>True/False</returns>
        public static Boolean In<T>(this T obj, HashSet<T> set)
        {
            return set.Contains(obj);
        }

        /// <summary>
        /// SQL keyword IN.  Returns false if the list is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="set"></param>
        /// <returns>True/False</returns>
        public static Boolean InSafe<T>(this T obj, List<T> set)
        {
            if (set == null)
                return false;
            else
                return obj.In(set);
        }

        /// <summary>
        /// SQL keyword IN
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="set"></param>
        /// <returns>True/False</returns>
        public static Boolean In<T>(this T obj, List<T> set)
        {
            return set.Contains(obj);
        }

        /// <summary>
        /// Groups an enumerable by a key selector and then creates a dictionary of TKey, List&lt;TValue&gt; from the groups.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">The key selector function.</param>
        /// <returns>The dictionary.</returns>
        public static Dictionary<TKey, List<TValue>> ToListDictionary<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
        {
            return source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Groups an enumerable by a key selector and then creates a dictionary of TKey, List&lt;TValue&gt; from the groups.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">The key selector function.</param>
        /// <param name="valueSelector">A function which modifies each value before being placed in a List</param>
        /// <returns>The dictionary.</returns>
        public static Dictionary<TKey, List<TListValue>> ToListDictionary<TKey, TValue, TListValue>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TListValue> valueSelector)
        {
            return source.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.Select(gs => valueSelector(gs)).ToList());
        }

        public static DictionaryEx<TKey, TValue> ToDictionary<TKey, TSource, TValue>(IEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TValue> valueSelector = null)
        {
            var ret = new DictionaryEx<TKey, TValue>();
            if (source.NoneSafe())
                return ret;

            if (valueSelector == null)
                valueSelector = (s, i) => (TValue)(dynamic)s;

            var curr = 0;
            foreach (var s in source)
            {
                ret[keySelector(s, curr)] = valueSelector(s, curr);
                curr++;
            }

            return ret;
        }

        public static Dictionary<String, String> ToDictionarySafe(this IEnumerable<String[]> source, Func<String[], String> keySelector, Func<String[], String> elementSelector, String separator)
        {
            if (source == null)
                return null;

            var ret = new Dictionary<String, String>();

            foreach (var s in source)
            {
                var key = keySelector(s);
                if (ret.ContainsKey(key))
                {
                    ret[key] += String.Format("{0}{1}", separator, elementSelector(s));
                }
                else
                {
                    ret.Add(key, elementSelector(s));
                }
            }

            return ret;
        }

        public static Dictionary<TValue, TKey> ToReverseDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            var ret = new Dictionary<TValue, TKey>();
            foreach (var kvp in dict)
                ret.Add(kvp.Value, kvp.Key);
            return ret;
        }

        public static List<TKey> FindDuplicateKeys<TValue, TKey>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        }

        /// <summary>
        /// Returns the count of a collection.  If the source is null, returns 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int CountSafe<T>(this IEnumerable<T> source, Func<T, Boolean> selector)
        {
            if (source == null)
                return 0;

            return System.Linq.Enumerable.Count<T>(source, selector);
        }

        /// <summary>
        /// Returns the count of a collection.  If the source is null, returns 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int CountSafe<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return 0;

            return System.Linq.Enumerable.Count<T>(source);
        }

        /// <summary>
        /// Returns the number of items of a collection that match a selector with a nullable Boolean return.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int Count<T>(this IEnumerable<T> source, Func<T, Boolean?> selector)
        {
            return System.Linq.Enumerable.Count<T>(source, s => selector(s) == true);
        }

        public static List<T> ToListSafe<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return new List<T>();
            else
                return source.ToList();
        }
    }
}