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
        public static String ToVSProjectRootPath(this DirectoryInfo dInfo)
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
        public static String ToVSProjectRootPath(this FileInfo fInfo)
        {
            var dInfo = fInfo.Directory;
            while (dInfo.FullName.Contains("bin"))
                dInfo = dInfo.Parent;
            return Path.Combine(dInfo.FullName, fInfo.ToString()).EnsureDoesNotStartWith("\\").ToAbsolutePathFromCurrentDirectory();
        }

        private static String ToAbsolutePath(this String relativeDir, String startingDir = null)
        {
            using (new CurrentDirectory(startingDir ?? Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)))
            {
                return new DirectoryInfo(relativeDir).FullName;
            }
        }

        private static String ToAbsolutePathFromCurrentDirectory(this String relativeDir)
        {
            return relativeDir.ToAbsolutePath(Directory.GetCurrentDirectory());
        }
    }
}