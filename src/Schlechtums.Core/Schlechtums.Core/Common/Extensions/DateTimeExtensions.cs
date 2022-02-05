using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Safe ToString.  If the date time is null it returns null
        /// </summary>
        /// <param name="obj">The date time to call Tostring on</param>
        /// <returns>obj.ToString() or null</returns>
        public static string ToStringSafe(this DateTime? obj)
        {
            if (obj == null)
                return null;
            else
                return obj.Value.ToString();
        }

        /// <summary>
        /// Safe ToString.  If the date time is null it returns null
        /// </summary>
        /// <param name="obj">The date time to call Tostring on</param>
        /// <param name="format">The date time format</param>
        /// <returns>obj.ToString() or null</returns>
        public static string ToStringSafe(this DateTime? obj, string format)
        {
            if (obj == null)
                return null;
            else
                return obj.Value.ToString(format);
        }
    }
}