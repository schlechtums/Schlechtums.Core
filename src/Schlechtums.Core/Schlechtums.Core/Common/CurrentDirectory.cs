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

        public CurrentDirectory(String dir)
        {
            if (!dir.IsNullOrWhitespace())
                dir = Path.GetFullPath(dir);

            this.m_OriginalDirectory = Directory.GetCurrentDirectory();
            this.m_ThisDirectory = dir;

            if (!this.m_ThisDirectory.IsNullOrWhitespace())
                Directory.SetCurrentDirectory(this.m_ThisDirectory);
        }

        private String m_OriginalDirectory;
        private String m_ThisDirectory;

        public void Dispose()
        {
            Directory.SetCurrentDirectory(this.m_OriginalDirectory);
        }

        public static implicit operator String(CurrentDirectory cd)
        {
            return cd.m_ThisDirectory;
        }
    }

}