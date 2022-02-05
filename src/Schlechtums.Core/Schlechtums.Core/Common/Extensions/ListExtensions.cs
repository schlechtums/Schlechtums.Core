using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Schlechtums.Core.Common.Extensions
{
    public static class ListExtensions
    {
        public static T TryGetIndex<T>(this Collection<T> source, int index, T defaultValue = default(T))
        {
            if (source.AnySafe() && source.Count > index)
                return source[index];
            else
                return defaultValue;
        }

        public static T TryGetIndex<T>(this IList<T> source, int index, T defaultValue = default(T))
        {
            if (source.AnySafe() && source.Count > index)
                return source[index];
            else
                return defaultValue;
        }

        /// <summary>
        /// Returns if the collection contains the given item.  If the collection is null it returns false instead of throwing an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="item">The item to find.</param>
        /// <returns>True/False.</returns>
        public static bool ContainsSafe<T>(this ICollection<T> source, T item)
        {
            if (source == null)
                return false;

            return source.Contains(item);
        }

        /// <summary>
        /// Returns if the collection contains an item which matches the selector function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static bool ContainsSafe<T>(this ICollection<T> source, Func<T, bool?> selector)
        {
            return source.AnySafe(i => selector(i));
        }
    }
}