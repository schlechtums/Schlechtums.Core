using System;
using System.IO;
using System.Reflection;

namespace Schlechtums.Core.Common.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        /// Returns the file path for a file in the vs project root
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToVSProjectRootPath(this DirectoryInfo dInfo)
        {
            var name = dInfo.Name;
            while (dInfo.FullName.Contains("bin"))
                dInfo = dInfo.Parent;
            return Path.Combine(dInfo.FullName, name).EnsureDoesNotStartWith("\\").ToAbsolutePathFromCurrentDirectory();
        }

        /// <summary>
        /// Returns the file path for a file in the vs project root
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToVSProjectRootPath(this FileInfo fInfo)
        {
            var dInfo = fInfo.Directory;
            while (dInfo.FullName.Contains("bin"))
                dInfo = dInfo.Parent;
            return Path.Combine(dInfo.FullName, fInfo.ToString()).EnsureDoesNotStartWith("\\").ToAbsolutePathFromCurrentDirectory();
        }

        private static string ToAbsolutePath(this string relativeDir, string startingDir = null)
        {
            using (new CurrentDirectory(startingDir ?? Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)))
            {
                return new DirectoryInfo(relativeDir).FullName;
            }
        }

        private static string ToAbsolutePathFromCurrentDirectory(this string relativeDir)
        {
            return relativeDir.ToAbsolutePath(Directory.GetCurrentDirectory());
        }
    }
}