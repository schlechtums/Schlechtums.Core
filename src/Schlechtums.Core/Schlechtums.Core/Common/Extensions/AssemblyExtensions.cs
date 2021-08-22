using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetTypesFromAllAssemblies<T>(IEnumerable<String> filter = null)
        {
            return GetTypesFromAllAssemblies(false, (t) => t.IsType(typeof(T)), filter);
        }

        /// <summary>
        /// Gets all types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetTypesFromSpecificAssembly<T>(String assemblyPath, IEnumerable<String> filter = null)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(assemblyPath, false, (t) => t.IsType(typeof(T)), filter);
        }

        /// <summary>
        /// Gets all instantiable types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetInstantiableTypesFromAllAssemblies<T>(IEnumerable<String> filter = null)
        {
            return GetTypesFromAllAssemblies(true, (t) => t.IsType(typeof(T)), filter);
        }

        /// <summary>
        /// Gets all instantiable types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetInstantiableTypesFromSpecificAssembly<T>(String assemblyPath, IEnumerable<String> filter = null)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(assemblyPath, true, (t) => t.IsType(typeof(T)), filter);
        }

        /// <summary>
        /// Gets all instantiable types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <param name="workingDirectory">Working directory in which to find the type.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetInstantiableTypesFromAllAssemblies<T>(String workingDirectory, IEnumerable<String> filter = null)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromAllAssemblies(true, (t) => t.IsType(typeof(T)), workingDirectory, filter);
        }

        /// <summary>
        /// Gets all instantiable types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <param name="workingDirectory">Working directory in which to find the type.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetInstantiableTypesFromAllAssemblies<T>(String assemblyPath, String workingDirectory, IEnumerable<String> filter = null)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(assemblyPath, true, (t) => t.IsType(typeof(T)), workingDirectory, filter).ToList();
        }

        /// <summary>
        /// Gets all types which implement the specified interface type or derive from the specified type.
        /// </summary>
        ///<param name="fullName">The full name of the type.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetTypesFromAllAssembliesByFullName(String fullName)
        {
            return GetTypesFromAllAssemblies(false, (t) => t.FullName == fullName);
        }

        /// <summary>
        /// Gets all types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        ///<param name="fullName">The full name of the type.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetTypesFromSpecificAssemblyByFullName(String assemblyPath, String fullName)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(assemblyPath, false, (t) => t.FullName == fullName);
        }

        /// <summary>
        /// Gets all instantiable types which implement the specified interface type or derive from the specified type.
        /// </summary>
        ///<param name="fullName">The full name of the type.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetInstantiableTypesFromAllAssembliesByFullName(String fullName)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromAllAssemblies(true, (t) => t.FullName == fullName);
        }

        /// <summary>
        /// Gets all instantiable types which implement the specified interface type or derive from the specified type.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        ///<param name="fullName">The full name of the type.</param>
        /// <returns>A list of types and some basic information about their assemblies.</returns>
        public static List<TypeFromAssemblyInfo> GetInstantiableTypesFromSpecificByFullName(String assemblyPath, String fullName)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(assemblyPath, true, (t) => t.FullName == fullName);
        }

        /// <summary>
        /// Gets types from all assemblies.
        /// </summary>
        /// <param name="onlyInstantiable">True/False to filter based on instantiable types vs all types.</param>
        /// <param name="match">A match function to determine which types to choose.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <returns>The types.</returns>
        private static List<TypeFromAssemblyInfo> GetTypesFromAllAssemblies(Boolean onlyInstantiable, Func<Type, Boolean> match, IEnumerable<String> filter = null)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromAllAssemblies(onlyInstantiable, match, Directory.GetCurrentDirectory(), filter);
        }

        /// <summary>
        /// Gets types from all assemblies.
        /// </summary>
        /// <param name="assemblyPath">The assembly path</param>
        /// <param name="onlyInstantiable">True/False to filter based on instantiable types vs all types.</param>
        /// <param name="match">A match function to determine which types to choose.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <returns>The types.</returns>
        private static List<TypeFromAssemblyInfo> GetTypesFromSpecificAssembly(String assemblyPath, Boolean onlyInstantiable, Func<Type, Boolean> match, IEnumerable<String> filter = null)
        {
            return Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(assemblyPath, onlyInstantiable, match, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filter).ToList();
        }

        /// <summary>
        /// Gets types from all assemblies.
        /// </summary>
        /// <param name="onlyInstantiable">True/False to filter based on instantiable types vs all types.</param>
        /// <param name="match">A match function to determine which types to choose.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <param name="workingDirectory">Working directory in which to find the type.</param>
        /// <returns>The types.</returns>
        public static List<TypeFromAssemblyInfo> GetTypesFromAllAssemblies(Boolean onlyInstantiable, Func<Type, Boolean> match, String workingDirectory, IEnumerable<String> filter = null)
        {
            var ret = new List<TypeFromAssemblyInfo>();

            var files = Directory.EnumerateFiles(workingDirectory, "*", SearchOption.AllDirectories).WhereSafe(f => Path.GetExtension(f).ToLower() == ".dll" || Path.GetExtension(f).ToLower() == ".exe").ToList();
            foreach (var f in files)
            {
                try
                {
                    ret.AddRange(Schlechtums.Core.Common.Extensions.AssemblyExtensions.GetTypesFromSpecificAssembly(f, onlyInstantiable, match, workingDirectory, filter));
                }
                catch
                { }
            }

            //return a list of type info of only the unique types
            return ret.Distinct(t => t.Type).ToList();
        }

        /// <summary>
        /// Gets types from a specific assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="onlyInstantiable">True/False to filter based on instantiable types vs all types.</param>
        /// <param name="match">A match function to determine which types to choose.</param>
        /// <param name="filter">An optional filter to only return types whose full name matches a string in the filter.</param>
        /// <param name="workingDirectory">Working directory in which to find the type.</param>
        /// <returns>The types.</returns>
        public static IEnumerable<TypeFromAssemblyInfo> GetTypesFromSpecificAssembly(String assemblyPath, Boolean onlyInstantiable, Func<Type, Boolean> match, String workingDirectory, IEnumerable<String> filter = null)
        {
            var typeFilter = filter == null ? null : new HashSet<String>(filter);

            Assembly asm = null;
            try
            {
                //try to load assembly in reflection only mode
                asm = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }
            catch
            {
                return new List<TypeFromAssemblyInfo>();
                //if it fails it's not a .NET assembly... ignore
            }

            Type[] assemblyTypes = null;
            AppDomain d = null;

            try
            {
                //create an app domain in which to load the assembly.. this is the only way to unload assemblies which had no ICCCModule types
                d = AppDomain.CreateDomain(assemblyPath);
                asm = d.Load(File.ReadAllBytes(assemblyPath));
                assemblyTypes = asm.GetTypes();
            }
            catch
            {
                if (d != null)
                {
                    AppDomain.Unload(d);

                    asm = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                    assemblyTypes = asm.GetTypes();
                }
            }

            //get all types which either
            //1) derive the base type or implement the interface
            //2) exclude abstract/interfaces if onlyInstantiable is true
            var types = assemblyTypes.WhereSafe(t =>
                ((!t.IsInterface && !t.IsAbstract) || !onlyInstantiable) && //1) derive the base type or implement the interface
                                                                            //2) exclude abstract/interfaces if onlyInstantiable is true
                match(t) &&                                                 //and match the passed in selector function
                (typeFilter == null || typeFilter.Contains(t.FullName))); //and the type filter contains this type or it is null (don't apply any filter)

            var ret = types.Select(t =>
                new TypeFromAssemblyInfo
                {
                    Fullpath = Path.GetFullPath(assemblyPath),
                    AssemblyFullName = asm.FullName,
                    Type = t
                });

            //unload the app domain if the types weren't found
            try
            {
                if (!types.Any() && d != null)
                    AppDomain.Unload(d);
            }
            catch { }

            return ret;
        }
    }
}