using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Schlechtums.Core.Common.Extensions
{
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Determines whether or not a file has a lock on it.
        /// </summary>
        /// <param name="f">The FileInfo object.</param>
        /// <returns>True/False if it is locked.</returns>
        public static bool IsLocked(this FileInfo f)
        {
            try
            {
                using (var fs = File.Open(f.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    return false;
                }
            }
            catch (IOException e)
            {
                var errorCode = (uint)Marshal.GetHRForException(e);
                uint hrFileLocked = 0x80070020;
                uint hrPortionOfFileLocked = 0x80070021;

                if (!(errorCode == hrFileLocked || errorCode == hrPortionOfFileLocked))
                    throw;

                return true;
            }
        }
    }
}