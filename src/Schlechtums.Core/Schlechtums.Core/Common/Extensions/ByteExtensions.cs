using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Gets a string from bytes using Encoding.UTF8.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>The string.</returns>
        public static String GetString(this Byte[] bytes)
        {
            return bytes.GetString(Encoding.UTF8);
        }

        /// Gets a string from bytes using Encoding.UTF8.  Returns null if bytes are null.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>The string.</returns>
        public static String GetStringSafe(this Byte[] bytes)
        {
            return bytes.GetStringSafe(Encoding.UTF8);
        }

        /// <summary>
        /// Gets a string from bytes using the specified encoding.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="enc">The encoding.</param>
        /// <returns>The string.</returns>
        public static String GetString(this Byte[] bytes, Encoding enc)
        {
            return enc.GetString(bytes);
        }

        /// <summary>
        /// Gets a string from bytes using the specified encoding.  Returns null if bytes are null.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="enc">The encoding.</param>
        /// <returns>The string.</returns>
        public static String GetStringSafe(this Byte[] bytes, Encoding enc)
        {
            if (bytes == null)
                return null;
            else
                return bytes.GetString(enc);
        }
    }
}