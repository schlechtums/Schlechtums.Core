using System;
using System.Collections.Generic;

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
    }
}