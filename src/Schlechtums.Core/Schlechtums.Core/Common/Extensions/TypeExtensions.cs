using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns true/false if the specified type either derives from or implements a base type interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseType">The base type.</param>
        /// <returns>True/False.</returns>
        public static bool IsType(this Type type, Type baseType)
        {
            return type.DerivesFromType(baseType) || type.ImplementsInterface(baseType);
        }

        public static bool IsGenericType(this Object obj, Type genericType)
        {
            var t = obj.GetType();
            return (t.IsGenericType && t.GetGenericTypeDefinition() == genericType);
        }

        public static bool IsGenericType(this Type t, Type genericType)
        {
            return (t.IsGenericType && t.GetGenericTypeDefinition() == genericType);
        }

        /// <summary>
        /// Returns if the type derives from a base type.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <param name="baseType">The base type.</param>
        /// <returns>True or False.</returns>
        public static bool DerivesFromType(this Type type, Type baseType)
        {
            if (type == baseType) return true;

            Type bType = type.BaseType;
            while (bType != null)
            {
                if (bType == baseType) return true;
                bType = bType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Returns if the type derives an interface.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="interfaceType">The interface type.</param>
        /// <returns>True or False.</returns>
        /// <exception cref="ArgumentException">Thrown if the interface type passed is not an interface.</exception>
        public static bool ImplementsInterface(this Object obj, Type interfaceType)
        {
            return obj.GetType().ImplementsInterface(interfaceType);
        }

        /// <summary>
        /// Returns if the type derives an interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">The interface type.</param>
        /// <returns>True or False.</returns>
        /// <exception cref="ArgumentException">Thrown if the interface type passed is not an interface.</exception>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(i => i == interfaceType);
        }
    }
}