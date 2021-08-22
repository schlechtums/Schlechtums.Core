using System.Collections.Generic;

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
    }
}