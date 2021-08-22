using System;
using System.Reflection;

namespace Schlechtums.Core.Common.Extensions
{
    public static class PropertyExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo p)
            where TAttribute : Attribute
        {
            return Attribute.GetCustomAttribute(p, typeof(TAttribute)) as TAttribute;
        }
    }
}