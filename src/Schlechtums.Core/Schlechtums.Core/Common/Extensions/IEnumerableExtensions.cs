using System;
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
    }
}