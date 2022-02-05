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

        public TemporaryDirectory(bool create)
            : this(Path.GetRandomFileName().Split('.')[0], create)
        { }

        /// <summary>
        /// Instantiate a temporary directory class.  Does not create the directory.
        /// </summary>
        /// <param name="dir">The name of the directory.</param>
        public TemporaryDirectory(string dir)
            : this(dir, false)
        { }

        public TemporaryDirectory(string dir, bool create)
        {
            this.dir = Path.GetFullPath(dir);
            if (!this.Exists())
                Directory.CreateDirectory(this.dir);
        }

        private string dir;

        #region <<< static System.IO.Directory methods >>>
        public DirectoryInfo GetParent()
        {
            return Directory.GetParent(this.dir);
        }

        public bool Exists()
        {
            return Directory.Exists(this.dir);
        }

        public void SetCreationTime(DateTime creationTime)
        {
            Directory.SetCreationTime(this.dir, creationTime);
        }

        public void SetCreationTimeUtc(DateTime creationTimeUtc)
        {
            Directory.SetCreationTimeUtc(this.dir, creationTimeUtc);
        }

        public DateTime GetCreationTime()
        {
            return Directory.GetCreationTime(this.dir);
        }

        public DateTime GetCreationTimeUtc()
        {
            return Directory.GetCreationTimeUtc(this.dir);
        }

        public void SetLastWriteTime(DateTime lastWriteTime)
        {
            Directory.SetLastWriteTime(this.dir, lastWriteTime);
        }

        public void SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
        {
            Directory.SetLastWriteTimeUtc(this.dir, lastWriteTimeUtc);
        }

        public DateTime GetLastWriteTime()
        {
            return Directory.GetLastWriteTime(this.dir);
        }

        public DateTime GetLastWriteTimeUtc()
        {
            return Directory.GetLastWriteTimeUtc(this.dir);
        }

        public void SetLastAccessTime(DateTime lastAccessTime)
        {
            Directory.SetLastAccessTime(this.dir, lastAccessTime);
        }

        public void SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
        {
            Directory.SetLastAccessTimeUtc(this.dir, lastAccessTimeUtc);
        }

        public DateTime GetLastAccessTime()
        {
            return Directory.GetLastAccessTime(this.dir);
        }

        public DateTime GetLastAccessTimeUtc()
        {
            return Directory.GetLastAccessTimeUtc(this.dir);
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(this.dir);
        }

        public string[] GetFiles(string searchPattern)
        {
            return Directory.GetFiles(this.dir, searchPattern);
        }

        public string[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(this.dir, searchPattern, searchOption);
        }

        public string[] GetDirectories()
        {
            return Directory.GetDirectories(this.dir);
        }

        public string[] GetDirectories(string searchPattern)
        {
            return Directory.GetDirectories(this.dir, searchPattern);
        }

        public string[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(this.dir, searchPattern, searchOption);
        }

        public string[] GetFileSystemEntries()
        {
            return Directory.GetFileSystemEntries(this.dir);
        }

        public string[] GetFileSystemEntries(string searchPattern)
        {
            return Directory.GetFileSystemEntries(this.dir, searchPattern);
        }

        public string[] GetFileSystemEntries(string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFileSystemEntries(this.dir, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateDirectories()
        {
            return Directory.EnumerateDirectories(this.dir);
        }

        public IEnumerable<string> EnumerateDirectories(string searchPattern)
        {
            return Directory.EnumerateDirectories(this.dir, searchPattern);
        }

        public IEnumerable<string> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(this.dir, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFiles()
        {
            return Directory.EnumerateFiles(this.dir);
        }

        public IEnumerable<string> EnumerateFiles(string searchPattern)
        {
            return Directory.EnumerateFiles(this.dir, searchPattern);
        }

        public IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(this.dir, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFileSystemEntries()
        {
            return Directory.EnumerateFileSystemEntries(this.dir);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(this.dir, searchPattern);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(this.dir, searchPattern, searchOption);
        }

        public string GetDirectoryRoot()
        {
            return Directory.GetDirectoryRoot(this.dir);
        }

        public void SetCurrentDirectory()
        {
            Directory.SetCurrentDirectory(this.dir);
        }

        public void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(this.dir, destDirName);
        }

        public void Delete()
        {
            Directory.Delete(this.dir);
        }

        public void Delete(bool recursive)
        {
            Directory.Delete(this.dir, recursive);
        }
        #endregion

        public static implicit operator string(TemporaryDirectory td)
        {
            return td.ToString();
        }

        public override string ToString()
        {
            return this.dir;
        }

        public string DirectoryName
        {
            get { return Path.GetFullPath(this.dir); }
        }

        public void Dispose()
        {
            if (Directory.Exists(this.dir))
            {
                try
                {
                    //sometimes Directory.Delete will not delete a directory, but rmdir on the command prompt will
                    ProcessExtensions.RunProcessInBackgroundAndGetStandardOutput("cmd", string.Format("/c rmdir /q /s \"{0}\"", this.dir));
                }
                catch { }
            }
        }
    }
}