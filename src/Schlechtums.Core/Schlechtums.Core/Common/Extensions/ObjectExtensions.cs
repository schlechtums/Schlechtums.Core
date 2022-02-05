using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Schlechtums.Core.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Creates a list of size 1 of repeated instances of the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>The list of size 1 of the object.</returns>
        public static List<T> CreateList<T>(this T obj)
        {
            return obj.CreateList(1);
        }

        /// <summary>
        /// Creates a list of a given size of repeated instances of a given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="size">The size of the list to create.</param>
        /// <returns>The list of instances of the object.</returns>
        public static List<T> CreateList<T>(this T obj, int size)
        {
            var retList = new List<T>(size);
            for (int i = 0; i < size; i++)
            {
                retList.Add(obj);
            }

            return retList;
        }

        /// <summary>
        /// Serializes an object to JSON.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="includeNonPublic">Include non public members.  Defaults to false.</param>
        /// <param name="ignoreReferenceLoops">Ignore reference loops.  Defaults to true.</param>
        /// <returns>The JSON string.</returns>
        public static string ToJSON(this Object obj)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IgnoreReadOnlyProperties = true;
            return JsonSerializer.Serialize(obj, obj.GetType(), options);
        }

        /// <summary>
        /// Safe ToString.  If the object is null it returns null
        /// </summary>
        /// <param name="obj">The object to call Tostring on</param>
        /// <returns>obj.ToString() or null</returns>
        public static string ToStringSafe(this Object obj)
        {
            if (obj == null)
                return null;
            else
                return obj.ToString();
        }
    }
}