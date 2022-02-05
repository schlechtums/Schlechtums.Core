using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Schlechtums.Core.Common.Extensions
{
    public static class StringExtensions
    {
        private static Random _Random = new Random();

        /// <summary>
        /// Returns a pluralized string ending based on the count of objects in a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pluralForm">The plural form.  Ex. Computer: s (ToPlural("s"), Entry: ies (ToPlural("ies")</param>
        /// <param name="baseForm">The base ending.  Ex. Computer: (empty string) (ToPlural("s", ["" or omit]), Entry: y. (ToPlural("ies", "y").</param>
        /// <returns>The word ending.  Ex. if the word is entry/entries, then the string being pluralized should only be "entr", and it will get an ending of either "y" or "ies".</returns>
        public static string ToPlural<T>(this IEnumerable<T> source, string pluralForm = "s", string baseForm = "")
        {
            //short circuit the count since we only need to know if it's 1 or something else to determine if it's plural or not
            int count = 0;
            if (source.Any())
            {
                foreach (var s in source)
                {
                    count++;
                    if (count == 2)
                        break;
                }
            }

            return count.ToPlural(pluralForm, baseForm);
        }

        /// <summary>
        /// Returns a pluralized ending based on the count of objects in a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="word">The word.</param>
        /// <param name="pluralForm">The plural form.  Ex. Computer: s (ToPlural("s"), Entry: ies (ToPlural("ies")</param>
        /// <param name="baseForm">The base ending.  Ex. Computer: (empty string) (ToPlural("s", ["" or omit]), Entry: y. (ToPlural("ies", "y").</param>
        /// <returns>The word pluralized.  Ex. if the word is entry/entries, then the string being pluralized should only be "entr", and it will get an ending of either "y" or "ies".</returns>
        public static string ToPlural<T>(this IEnumerable<T> source, string word, string pluralForm = "s", string baseForm = "")
        {
            return word + source.ToPlural(pluralForm, baseForm);
        }

        /// <summary>
        /// Ensures a string ends with the given character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ending">The desired ending.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely ends with the given character.</returns>
        public static string EnsureEndsWith(this string str, char ending, bool caseSensitive = true)
        {
            return str.EnsureEndsWith(ending.ToString(), caseSensitive);
        }

        /// <summary>
        /// Ensures a string ends with the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ending">The desired ending.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely ends with the given string.</returns>
        public static string EnsureEndsWith(this string str, string ending, bool caseSensitive = true)
        {
            if (str == null)
                return ending;

            if (caseSensitive)
            {
                if (!str.EndsWith(ending))
                    str += ending;
            }
            else
            {
                if (!str.ToLower().EndsWith(ending.ToLower()))
                    str += ending;
            }

            return str;
        }

        /// <summary>
        /// Ensures a string does not end with the given character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ending">The undesired ending.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely does not end with the given character.</returns>
        public static string EnsureDoesNotEndWith(this string str, char ending, bool caseSensitive = true)
        {
            return str.EnsureDoesNotEndWith(ending.ToString(), caseSensitive);
        }

        /// <summary>
        /// Ensures a string does not end with a given string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ending">The undesired ending.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely does not end with the given string.</returns>
        public static string EnsureDoesNotEndWith(this string str, string ending, bool caseSensitive = true)
        {
            if (str == null)
                return str;

            if (caseSensitive)
            {
                if (str.EndsWith(ending))
                    str = str.Substring(0, str.Length - ending.Length);
            }
            else
            {
                if (str.ToLower().EndsWith(ending.ToLower()))
                    str = str.Substring(0, str.Length - ending.Length);
            }

            return str;
        }

        /// <summary>
        /// Ensures a string starts with the given character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The desired start.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely starts with the given character.</returns>
        public static string EnsureStartsWith(this string str, char start, bool caseSensitive = true)
        {
            return str.EnsureStartsWith(start.ToString(), caseSensitive);
        }

        /// <summary>
        /// Ensures a string starts with the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The desired start.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely starts with the given string.</returns>
        public static string EnsureStartsWith(this string str, string start, bool caseSensitive = true)
        {
            if (str == null)
                return start;

            if (caseSensitive)
            {
                if (!str.StartsWith(start))
                    return start + str;
            }
            else
            {
                if (!str.ToLower().StartsWith(start.ToLower()))
                    return start + str;
            }

            return str;
        }

        /// <summary>
        /// Ensures a string does not start with the given character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The undesired character.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely does not start with the given character.</returns>
        public static string EnsureDoesNotStartWith(this string str, char start, bool caseSensitive = true)
        {
            return str.EnsureDoesNotStartWith(start.ToString(), caseSensitive);
        }

        /// <summary>
        /// Ensures a string does not start with the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The undesired string.</param>
        /// <param name="caseSensitive">True/False for a case sensitive comparison.</param>
        /// <returns>The string which definitely does not start with the given string.</returns>
        public static string EnsureDoesNotStartWith(this string str, string start, bool caseSensitive = true)
        {
            if (str == null)
                return null;

            if (caseSensitive)
            {
                if (str.StartsWith(start))
                    str = str.Substring(start.Length);
            }
            else
            {
                if (str.ToLower().StartsWith(start.ToLower()))
                    str = str.Substring(start.Length);
            }

            return str;
        }

        /// <summary>
        /// Joins strings separated by Environment.NewLine
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="num">The number of newlines to use as the separator</param>
        /// <returns>The separated string</returns>
        public static string JoinWithNewline(this IEnumerable<string> strings, int num = 1)
        {
            return strings.Join(Environment.NewLine.CreateList(num).Join(""));
        }

        /// <summary>
        /// Joins strings separated by Environment.NewLine, or null if the original string was null
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="num">The number of newlines to use as the separator</param>
        /// <returns>The separated string</returns>
        public static string JoinWithNewlineSafe(this IEnumerable<string> strings, int num = 1)
        {
            if (strings == null)
                return null;

            return strings.JoinWithNewline(num);
        }

        /// <summary>
        /// Gets whether or not the string is empty or null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsNullOrWhitespace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Replicates SQL Server ISNULL function.  Uses IsNullOrWhitespace
        /// </summary>
        /// <param name="str">The string to test if null or whitespace.</param>
        /// <param name="ifNull">The string to return if null.</param>
        /// <returns>Either str, or ifNull if str.IsNullOrWhitespace()</returns>
        public static string IsNullOrWhitespace(this string str, string ifNullOrWhitespace)
        {
            if (str.IsNullOrWhitespace())
                return ifNullOrWhitespace;
            else
                return str;
        }

        /// <summary>
        /// Returns !str.IsNullOrWhitespace();
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>True/False</returns>
        public static bool IsValued(this string str)
        {
            return !str.IsNullOrWhitespace();
        }

        /// <summary>
        /// Shortcut method to create an XElement from a string of xml
        /// </summary>
        /// <param name="xml">string containing XML to parse.</param>
        /// <returns>An XElement for this XML.</returns>
        public static XElement ToXElement(this string xml)
        {
            using (var sr = new StringReader(xml))
            {
                return XElement.Load(sr);
            }
        }

        /// <summary>
        /// Shortcut method to create an XElement from a string of xml, but safely returning null if the string is null or whitespace
        /// </summary>
        /// <param name="xml">string containing XML to parse.</param>
        /// <returns>An XElement for this XML or null</returns>
        public static XElement ToXElementSafe(this string xml)
        {
            if (xml.IsNullOrWhitespace())
                return null;
            else
            {
                try
                {
                    return xml.ToXElement();
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Converts a string to a byte array using Encoding.UTF8.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The byte array.</returns>
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string to a byte array using Encoding.UTF8.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The byte array.</returns>
        public static byte[] GetBytesSafe(this string str)
        {
            return str.GetBytesSafe(Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string to a byte array using the specified encoding.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static byte[] GetBytes(this string str, Encoding enc)
        {
            return enc.GetBytes(str);
        }

        /// <summary>
        /// Converts a string to a byte array using the specified encoding.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static byte[] GetBytesSafe(this string str, Encoding enc)
        {
            if (str == null)
                return null;
            else
                return str.GetBytes(enc);
        }

        /// <summary>
        /// Converts a string to a short.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The short.</returns>
        public static short ToShort(this string str)
        {
            short.TryParse(str, out short num);
            return num;
        }

        /// <summary>
        /// Converts a string to a nullable short.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable short.</returns>
        public static short? ToNullableShort(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToShort();
        }

        /// <summary>
        /// Converts a string to an int.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The int.</returns>
        public static int ToInt(this string str)
        {
            int.TryParse(str, out int num);
            return num;
        }

        /// <summary>
        /// Converts a string to a nullable int.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable int.</returns>
        public static int? ToNullableInt(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToInt();
        }

        /// <summary>
        /// Converts a string to a DateTime.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Parse(str);
        }

        /// <summary>
        /// Converts a string to a DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTime(this string str, string format)
        {
            return str.ToDateTime(format, null);
        }

        /// <summary>
        /// Converts a string to a DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTime(this string str, string format, IFormatProvider formatProvider)
        {
            return DateTime.ParseExact(str, format, formatProvider);
        }

        public static DateTime ToUniversalDateTime(this string str)
        {
            return str.ToDateTime().ToUniversalTime();
        }

        /// <summary>
        /// Converts a string to a universal DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToUniversalDateTime(this string str, string format)
        {
            return str.ToDateTime(format, null).ToUniversalTime();
        }

        /// <summary>
        /// Converts a string to a universal DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToUniversalDateTime(this string str, string format, IFormatProvider formatProvider)
        {
            return str.ToDateTime(format, formatProvider).ToUniversalTime();
        }

        /// <summary>
        /// Converts a string to a DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeIgnoreOffset(this string str)
        {
            return str.ToDateTimeIgnoreOffset(null);
        }

        /// <summary>
        /// Converts a string to a DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        ///<param name="formatProvider"></param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTimeIgnoreOffset(this string str, IFormatProvider formatProvider)
        {
            return str.ToDateTimeIgnoreOffset(formatProvider, DateTimeStyles.None);
        }

        /// <summary>
        /// Converts a string to a DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        ///<param name="formatProvider"></param>
        ///<param name="styles"></param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTimeIgnoreOffset(this string str, IFormatProvider formatProvider, DateTimeStyles styles)
        {
            return DateTimeOffset.Parse(str, formatProvider, styles).DateTime;
        }

        /// <summary>
        /// Converts a string to a nullable DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTimeIgnoreOffset(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;

            return str.ToDateTimeIgnoreOffset();
        }

        /// <summary>
        /// Converts a string to a nullable DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        ///<param name="formatProvider"></param>
        /// <returns>The DateTime.</returns>
        public static DateTime? ToNullableDateTimeIgnoreOffset(this string str, IFormatProvider formatProvider)
        {
            if (str.IsNullOrWhitespace())
                return null;

            return str.ToDateTimeIgnoreOffset(formatProvider);
        }

        /// <summary>
        /// Converts a string to a nullable DateTime ignoring any time zone offset.
        /// </summary>
        /// <param name="str"></param>
        ///<param name="formatProvider"></param>
        ///<param name="styles"></param>
        /// <returns>The DateTime.</returns>
        public static DateTime? ToNullableDateTimeIgnoreOffset(this string str, IFormatProvider formatProvider, DateTimeStyles styles)
        {
            if (str.IsNullOrWhitespace())
                return null;

            return str.ToDateTimeIgnoreOffset(formatProvider, styles);
        }

        /// <summary>
        /// Converts a string to a nullable DateTime.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableDateTime(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToDateTime();
        }

        /// <summary>
        /// Converts a string to a nullable Universal DateTime.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableUniversalDateTime(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToUniversalDateTime();
        }

        /// <summary>
        /// Converts a string to a nullable DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableDateTime(this string str, string format)
        {
            return str.ToNullableDateTime(format, null);
        }

        /// <summary>
        /// Converts a string to a nullable DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableDateTime(this string str, string format, IFormatProvider formatProvider)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToDateTime(format, formatProvider);
        }

        /// <summary>
        /// Converts a string to a nullable Universal DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableUniversalDateTime(this string str, string format)
        {
            return str.ToNullableUniversalDateTime(format, null);
        }

        /// <summary>
        /// Converts a string to a nullable Universal DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableUniversalDateTime(this string str, string format, IFormatProvider formatProvider)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToUniversalDateTime(format, formatProvider);
        }

        /// <summary>
        /// Converts a string to a boolean.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The boolean.</returns>
        public static bool ToBoolean(this string str)
        {
            bool.TryParse(str, out bool ret);
            return ret;
        }

        /// <summary>
        /// Converts a string to a nullable boolean.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable boolean.</returns>
        public static bool? ToNullableBoolean(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToBoolean();
        }

        /// <summary>
        /// Converts a string to a long.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The long.</returns>
        public static long ToLong(this string str)
        {
            long.TryParse(str, out long num);
            return num;
        }

        /// <summary>
        /// Converts a string to a nullable long.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable long.</returns>
        public static long? ToNullableLong(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;
            else
                return str.ToLong();
        }

        /// <summary>
        /// Gets whether or not a string represents an integer.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsShort(this string str)
        {
            return short.TryParse(str, out short _);
        }

        /// <summary>
        /// Gets whether or not a string represents an integer.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsInt(this string str)
        {
            return int.TryParse(str, out int _);
        }

        /// <summary>
        /// Determines whether or not a string represents a single precision floating point number
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsSingle(this string str)
        {
            return float.TryParse(str, out float _);
        }

        /// <summary>
        /// Determines whether or not a string represents a double precision floating point number
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsDouble(this string str)
        {
            return double.TryParse(str, out double _);
        }

        /// <summary>
        /// Gets whether or not a string represents a byte.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsByte(this string str)
        {
            return byte.TryParse(str, out byte _);
        }

        /// <summary>
        /// Gets whether or not a string represents a signed byte.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsSByte(this string str)
        {
            return sbyte.TryParse(str, out sbyte _);
        }

        /// <summary>
        /// Gets whether or not a string represents a long.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsLong(this string str)
        {
            return long.TryParse(str, out long _);
        }

        /// <summary>
        /// Gets whether or not a string represents a decimal.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static bool IsDecimal(this string str)
        {
            return decimal.TryParse(str, out decimal _);
        }

        /// <summary>
        /// Converts a string to a decimal.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The decimal.</returns>
        public static decimal ToDecimal(this string str)
        {
            return decimal.Parse(str);
        }

        /// <summary>
        /// Converts a string to a nullable decimal.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable decimal.</returns>
        public static decimal? ToNullableDecimal(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;

            return str.ToDecimal();
        }

        /// <summary>
        /// Converts a string to a float.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The float.</returns>
        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }

        /// <summary>
        /// Converts a string to a nullable float.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable float.</returns>
        public static float? ToNullableFloat(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;

            return str.ToFloat();
        }

        /// <summary>
        /// Converts a string to a double.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The double.</returns>
        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }

        /// <summary>
        /// Converts a string to a nullable double.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable double.</returns>
        public static double? ToNullableDouble(this string str)
        {
            if (str.IsNullOrWhitespace())
                return null;

            return str.ToDouble();
        }

        /// <summary>
        /// Returns true/false if the string can be parsed as a DateTime
        /// </summary>
        /// <param name="str">Thes string</param>
        /// <returns>true/false</returns>
        public static bool IsDateTime(this string str)
        {
            return DateTime.TryParse(str, out DateTime _);
        }

        /// <summary>
        /// REturns true/false if the string can be parsed as a DateTime
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="format">The DateTime format</param>
        /// <returns>true/false</returns>
        public static bool IsDateTimeExact(this string str, string format)
        {
            return DateTime.TryParseExact(str, format, null, System.Globalization.DateTimeStyles.None, out DateTime dt);
        }

        /// <summary>
        /// Returns a substring of the string.  If the string is null it returns null or if the length of the string is too long it will not crash
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The index at which to start</param>
        /// <returns>The substring</returns>
        public static string SubstringSafe(this string str, int start)
        {
            if (str == null)
                return null;

            return new string(str.Skip(start).ToArray());
        }

        /// <summary>
        /// Returns a substring of the string.  If the string is null it returns null or if the length of the string is too long it will not crash
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The index at which to start</param>
        /// <param name="length">The length to retrieve</param>
        /// <returns>The substring</returns>
        public static string SubstringSafe(this string str, int start, int length)
        {
            if (str == null)
                return null;

            return new string(str.Skip(start).Take(length).ToArray());
        }

        /// <summary>
        /// Removes all leading and trailing whitespace characters from the string.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The trimmed string.</returns>
        public static string TrimSafe(this string str)
        {
            if (str == null)
                return null;

            return str.Trim();
        }

        /// <summary>
        /// Removes all leading and trailing instances of the specified characters from the string.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The trimmed string.</returns>
        public static string TrimSafe(this string str, params char[] trimChars)
        {
            if (str == null)
                return null;

            return str.Trim(trimChars);
        }

        /// <summary>
        /// Returns the string.ToLower, or null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Returns the string.ToLower() or null if it was null</returns>
        public static string ToLowerSafe(this string str)
        {
            if (str == null)
                return null;

            return str.ToLower();
        }

        /// <summary>
        /// Conditionally returns the lowercase version of the string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="doUpper">True/False to perform the conversion or not</param>
        /// <returns>The lowercase representation of the string if requested</returns>
        public static string ToLower(this string str, bool doLower)
        {
            if (doLower)
                return str.ToLower();
            else
                return str;
        }

        /// <summary>
        /// Conditionally returns the uppercase version of the string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="doUpper">True/False to perform the conversion or not</param>
        /// <returns>The uppercase representation of the string if requested</returns>
        public static string ToUpper(this string str, bool doUpper)
        {
            if (doUpper)
                return str.ToUpper();
            else
                return str;
        }

        /// <summary>
        /// Returns the string.ToUpper, or null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Returns the string.ToUpper() or null if it was null</returns>
        public static string ToUpperSafe(this string str)
        {
            if (str == null)
                return null;

            return str.ToUpper();
        }

        /// <summary>
        /// Returns the string.Contains(value), or false if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">The value to search for.</param>
        /// <returns>True or false.</returns>
        public static bool ContainsSafe(this string str, string value)
        {
            if (str == null || value == null)
                return false;

            else return str.Contains(value);
        }

        /// <summary>
        /// Removes all instances of a string from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toRemove">The string to remove</param>
        /// <returns>The resulting string</returns>
        public static string Remove(this string text, string toRemove)
        {
            return text.Replace(toRemove, "");
        }

        /// <summary>
        /// Removes all instances of a character from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toRemove">The character to remove</param>
        /// <returns>The resulting string or null if the string was null</returns>
        public static string RemoveSafe(this string text, char toRemove)
        {
            return text.RemoveSafe(toRemove.ToString());
        }

        /// <summary>
        /// Removes all instances of a string from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toRemove">The string to remove</param>
        /// <returns>The resulting string or null if the string was null</returns>
        public static string RemoveSafe(this string text, string toRemove)
        {
            return text.ReplaceSafe(toRemove, "");
        }

        /// <summary>
        /// Replaces all instances of the old character with the new character.  Returns null if the string is null.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="oldChar">The character to replace.</param>
        /// <param name="newChar">The new character.</param>
        /// <returns>The string with replaced values, or null if the string was null.</returns>
        public static string ReplaceSafe(this string text, char oldChar, char newChar)
        {
            if (text == null)
                return null;
            else
                return text.Replace(oldChar, newChar);
        }

        /// <summary>
        /// Replaces all instances of the old value with the new value.  Returns null if the string is null.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The new string.</param>
        /// <returns>The string with replaced values, or null if the string was null.</returns>
        public static string ReplaceSafe(this string text, string oldValue, string newValue)
        {
            if (text == null)
                return null;
            else
                return text.Replace(oldValue, newValue);
        }

        /// <summary>
        /// Gets a random string of the allowed characters of the specified length.
        /// </summary>
        /// <param name="allowedChars">A string containing the allowed characters</param>
        /// <param name="length">The length of the string</param>
        /// <returns>A random string</returns>
        public static string GetRandomString(this string allowedChars, int length)
        {
            var stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = allowedChars[StringExtensions._Random.Next(allowedChars.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Gets a random string of just letters
        /// </summary>
        /// <param name="length">The length</param>
        /// <returns>A random string</returns>
        public static string GetRandomAlphaString(int length = 1)
        {
            return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".GetRandomString(length);
        }

        /// <summary>
        /// Gets a guid for a string
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>The guid</returns>
        public static Guid ToGuid(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(str));
                return new Guid(hash);
            }
        }

        /// <summary>
        /// Surrounds a string with a character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c">The character with which to surround the string</param>
        /// <returns>The string surrounded by the character</returns>
        public static string SurroundWith(this string str, char c)
        {
            return str.SurroundWith(c.ToString());
        }

        /// <summary>
        /// Surrounds a string with a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s">The string with which to surround the string</param>
        /// <returns>The string surrounded by the string</returns>
        public static string SurroundWith(this string str, string s)
        {
            return s + str + s;
        }

        /// <summary>
        /// Returns the pascal case version of a delimited string
        /// </summary>
        /// <param name="str">The delimited string</param>
        /// <param name="separator">The delimiter</param>
        /// <returns>The camel cased string</returns>
        public static string ToPascalCase(this string str, char separator)
        {
            if (str.IsNullOrWhitespace())
            {
                return str;
            }

            var pascalStringBuilder = new StringBuilder();

            var lastWasSeparator = true;

            foreach (var c in str)
            {
                if (c != separator)
                {
                    if (lastWasSeparator)
                    {
                        lastWasSeparator = false;
                        pascalStringBuilder.Append(char.ToUpper(c));
                    }
                    else
                    {
                        pascalStringBuilder.Append(char.ToLower(c));
                    }
                }
                else
                {
                    lastWasSeparator = true;
                }
            }

            return pascalStringBuilder.ToString();
        }

        /// <summary>
        /// Returns the camel cased version of a snake cased string
        /// </summary>
        /// <param name="str">The snake cased string</param>
        /// <returns>The camel cased string</returns>
        public static string SnakeCaseToPascalCase(this string str)
        {
            return str.ToPascalCase('_');
        }

        /// <summary>
        /// Capitalizes a string by making the first character after each instance of the delimiter a capital letter
        /// </summary>
        /// <param name="str">The string to capitalize</param>
        /// <param name="keepDelimiter">True or False to keep the delimiter in the returned string</param>
        /// <param name="delimiter">The separator character</param>
        /// <returns>The capitalized string</returns>
        public static string Capitalize(this string str, bool keepDelimiter, char delimiter = ' ')
        {
            str = str.ToLower();
            var capitalizeNext = true;

            var capitalizedStringBuilder = new StringBuilder();

            foreach (var c in str)
            {
                if (c == delimiter)
                {
                    if (keepDelimiter)
                    {
                        capitalizedStringBuilder.Append(c);
                    }
                    capitalizeNext = true;
                }
                else if (capitalizeNext)
                {
                    capitalizedStringBuilder.Append(char.ToUpper(c));
                    capitalizeNext = false;
                }
                else
                {
                    capitalizedStringBuilder.Append(c);
                }
            }

            return capitalizedStringBuilder.ToString();
        }

        /// <summary>
        /// Deserializes a JSON string back into an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="includeNonPublic">Deserialize non public members.  Defaults to false.</param>
        /// <returns>The object.</returns>
        public static T FromJSON<T>(this string json)
        {
            if (typeof(T) == typeof(XElement))
                return (T)(dynamic)new XElement(json.FromJSON<string>());
            else
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
                return JsonSerializer.Deserialize<T>(json, options);
            }
        }

        //https://docs.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support#custom-support-for--and--in-
        public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var str = reader.GetString();
                return str.ToDateTime();
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        public class IntConverterWithStringSupport : JsonConverter<int>
        {
            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                    return reader.GetString().ToInt();
                else
                    return reader.GetInt32();
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
    }
}