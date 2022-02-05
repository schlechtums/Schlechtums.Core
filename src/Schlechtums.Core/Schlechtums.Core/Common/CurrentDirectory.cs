using Schlechtums.Core.Common.Extensions;
using System;
using System.IO;

namespace Schlechtums.Core.Common
{
    /// <summary>
    /// Switches the current directory and restores on dispose
    /// </summary>
    public class CurrentDirectory : IDisposable
    {
        public CurrentDirectory(DirectoryInfo dir)
            : this(dir.FullName)
        { }

        public CurrentDirectory(string dir)
        {
            if (!dir.IsNullOrWhitespace())
                dir = Path.GetFullPath(dir);

            this.originalDirectory = Directory.GetCurrentDirectory();
            this.thisDirectory = dir;

            if (!this.thisDirectory.IsNullOrWhitespace())
                Directory.SetCurrentDirectory(this.thisDirectory);
        }

        private string originalDirectory;
        private string thisDirectory;

        public void Dispose()
        {
            Directory.SetCurrentDirectory(this.originalDirectory);
        }

        public static implicit operator string(CurrentDirectory cd)
        {
            return cd.thisDirectory;
        }
    }

}