using Schlechtums.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Schlechtums.Core.Common
{
    /// <summary>
    /// Creates a directory and deletes it when disposed.
    /// </summary>
    public class TemporaryDirectory : IDisposable
    {
        /// <summary>
        /// Does not create the directory
        /// </summary>
        public TemporaryDirectory()
            : this(Path.GetRandomFileName().Split('.')[0])
        { }

        public TemporaryDirectory(Boolean create)
            : this(Path.GetRandomFileName().Split('.')[0], create)
        { }

        /// <summary>
        /// Instantiate a temporary directory class.  Does not create the directory.
        /// </summary>
        /// <param name="dir">The name of the directory.</param>
        public TemporaryDirectory(String dir)
            : this(dir, false)
        { }

        public TemporaryDirectory(String dir, Boolean create)
        {
            this.m_Dir = Path.GetFullPath(dir);
            if (!this.Exists())
                Directory.CreateDirectory(this.m_Dir);
        }

        private String m_Dir;

        #region <<< static System.IO.Directory methods >>>
        public DirectoryInfo GetParent()
        {
            return Directory.GetParent(this.m_Dir);
        }

        public Boolean Exists()
        {
            return Directory.Exists(this.m_Dir);
        }

        public void SetCreationTime(DateTime creationTime)
        {
            Directory.SetCreationTime(this.m_Dir, creationTime);
        }

        public void SetCreationTimeUtc(DateTime creationTimeUtc)
        {
            Directory.SetCreationTimeUtc(this.m_Dir, creationTimeUtc);
        }

        public DateTime GetCreationTime()
        {
            return Directory.GetCreationTime(this.m_Dir);
        }

        public DateTime GetCreationTimeUtc()
        {
            return Directory.GetCreationTimeUtc(this.m_Dir);
        }

        public void SetLastWriteTime(DateTime lastWriteTime)
        {
            Directory.SetLastWriteTime(this.m_Dir, lastWriteTime);
        }

        public void SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
        {
            Directory.SetLastWriteTimeUtc(this.m_Dir, lastWriteTimeUtc);
        }

        public DateTime GetLastWriteTime()
        {
            return Directory.GetLastWriteTime(this.m_Dir);
        }

        public DateTime GetLastWriteTimeUtc()
        {
            return Directory.GetLastWriteTimeUtc(this.m_Dir);
        }

        public void SetLastAccessTime(DateTime lastAccessTime)
        {
            Directory.SetLastAccessTime(this.m_Dir, lastAccessTime);
        }

        public void SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
        {
            Directory.SetLastAccessTimeUtc(this.m_Dir, lastAccessTimeUtc);
        }

        public DateTime GetLastAccessTime()
        {
            return Directory.GetLastAccessTime(this.m_Dir);
        }

        public DateTime GetLastAccessTimeUtc()
        {
            return Directory.GetLastAccessTimeUtc(this.m_Dir);
        }

        public String[] GetFiles()
        {
            return Directory.GetFiles(this.m_Dir);
        }

        public String[] GetFiles(String searchPattern)
        {
            return Directory.GetFiles(this.m_Dir, searchPattern);
        }

        public String[] GetFiles(String searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(this.m_Dir, searchPattern, searchOption);
        }

        public String[] GetDirectories()
        {
            return Directory.GetDirectories(this.m_Dir);
        }

        public String[] GetDirectories(String searchPattern)
        {
            return Directory.GetDirectories(this.m_Dir, searchPattern);
        }

        public String[] GetDirectories(String searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(this.m_Dir, searchPattern, searchOption);
        }

        public String[] GetFileSystemEntries()
        {
            return Directory.GetFileSystemEntries(this.m_Dir);
        }

        public String[] GetFileSystemEntries(String searchPattern)
        {
            return Directory.GetFileSystemEntries(this.m_Dir, searchPattern);
        }

        public String[] GetFileSystemEntries(String searchPattern, SearchOption searchOption)
        {
            return Directory.GetFileSystemEntries(this.m_Dir, searchPattern, searchOption);
        }

        public IEnumerable<String> EnumerateDirectories()
        {
            return Directory.EnumerateDirectories(this.m_Dir);
        }

        public IEnumerable<String> EnumerateDirectories(String searchPattern)
        {
            return Directory.EnumerateDirectories(this.m_Dir, searchPattern);
        }

        public IEnumerable<String> EnumerateDirectories(String searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(this.m_Dir, searchPattern, searchOption);
        }

        public IEnumerable<String> EnumerateFiles()
        {
            return Directory.EnumerateFiles(this.m_Dir);
        }

        public IEnumerable<String> EnumerateFiles(String searchPattern)
        {
            return Directory.EnumerateFiles(this.m_Dir, searchPattern);
        }

        public IEnumerable<String> EnumerateFiles(String searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(this.m_Dir, searchPattern, searchOption);
        }

        public IEnumerable<String> EnumerateFileSystemEntries()
        {
            return Directory.EnumerateFileSystemEntries(this.m_Dir);
        }

        public IEnumerable<String> EnumerateFileSystemEntries(String searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(this.m_Dir, searchPattern);
        }

        public IEnumerable<String> EnumerateFileSystemEntries(String searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(this.m_Dir, searchPattern, searchOption);
        }

        public String GetDirectoryRoot()
        {
            return Directory.GetDirectoryRoot(this.m_Dir);
        }

        public void SetCurrentDirectory()
        {
            Directory.SetCurrentDirectory(this.m_Dir);
        }

        public void Move(String sourceDirName, String destDirName)
        {
            Directory.Move(this.m_Dir, destDirName);
        }

        public void Delete()
        {
            Directory.Delete(this.m_Dir);
        }

        public void Delete(Boolean recursive)
        {
            Directory.Delete(this.m_Dir, recursive);
        }
        #endregion

        public static implicit operator String(TemporaryDirectory td)
        {
            return td.ToString();
        }

        public override string ToString()
        {
            return this.m_Dir;
        }

        public String DirectoryName
        {
            get { return Path.GetFullPath(this.m_Dir); }
        }

        public void Dispose()
        {
            if (Directory.Exists(this.m_Dir))
            {
                try
                {
                    //sometimes Directory.Delete will not delete a directory, but rmdir on the command prompt will
                    ProcessExtensions.RunProcessInBackgroundAndGetStandardOutput("cmd", String.Format("/c rmdir /q /s \"{0}\"", this.m_Dir));
                }
                catch { }
            }
        }
    }
}