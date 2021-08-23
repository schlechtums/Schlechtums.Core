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
        public static String ToPlural<T>(this IEnumerable<T> source, String pluralForm = "s", String baseForm = "")
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
        public static String ToPlural<T>(this IEnumerable<T> source, String word, String pluralForm = "s", String baseForm = "")
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
        public static String EnsureEndsWith(this String str, Char ending, Boolean caseSensitive = true)
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
        public static String EnsureEndsWith(this String str, String ending, Boolean caseSensitive = true)
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
        public static String EnsureDoesNotEndWith(this String str, Char ending, Boolean caseSensitive = true)
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
        public static String EnsureDoesNotEndWith(this String str, String ending, Boolean caseSensitive = true)
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
        public static String EnsureStartsWith(this String str, Char start, Boolean caseSensitive = true)
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
        public static String EnsureStartsWith(this String str, String start, Boolean caseSensitive = true)
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
        public static String EnsureDoesNotStartWith(this String str, Char start, Boolean caseSensitive = true)
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
        public static String EnsureDoesNotStartWith(this String str, String start, Boolean caseSensitive = true)
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
        public static String JoinWithNewline(this IEnumerable<String> strings, int num = 1)
        {
            return strings.Join(Environment.NewLine.CreateList(num).Join(""));
        }

        /// <summary>
        /// Joins strings separated by Environment.NewLine, or null if the original string was null
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="num">The number of newlines to use as the separator</param>
        /// <returns>The separated string</returns>
        public static String JoinWithNewlineSafe(this IEnumerable<String> strings, int num = 1)
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
        public static Boolean IsNullOrWhitespace(this String str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Replicates SQL Server ISNULL function.  Uses IsNullOrWhitespace
        /// </summary>
        /// <param name="str">The string to test if null or whitespace.</param>
        /// <param name="ifNull">The string to return if null.</param>
        /// <returns>Either str, or ifNull if str.IsNullOrWhitespace()</returns>
        public static String IsNullOrWhitespace(this String str, String ifNullOrWhitespace)
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
        public static Boolean IsValued(this String str)
        {
            return !str.IsNullOrWhitespace();
        }

        /// <summary>
        /// Shortcut method to create an XElement from a string of xml
        /// </summary>
        /// <param name="xml">String containing XML to parse.</param>
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
        /// <param name="xml">String containing XML to parse.</param>
        /// <returns>An XElement for this XML or null</returns>
        public static XElement ToXElementSafe(this String xml)
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
        public static Byte[] GetBytes(this String str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string to a byte array using Encoding.UTF8.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The byte array.</returns>
        public static Byte[] GetBytesSafe(this String str)
        {
            return str.GetBytesSafe(Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string to a byte array using the specified encoding.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static Byte[] GetBytes(this String str, Encoding enc)
        {
            return enc.GetBytes(str);
        }

        /// <summary>
        /// Converts a string to a byte array using the specified encoding.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static Byte[] GetBytesSafe(this String str, Encoding enc)
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
        public static short ToShort(this String str)
        {
            short.TryParse(str, out short num);
            return num;
        }

        /// <summary>
        /// Converts a string to a nullable short.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable short.</returns>
        public static short? ToNullableShort(this String str)
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
        public static int ToInt(this String str)
        {
            int.TryParse(str, out int num);
            return num;
        }

        /// <summary>
        /// Converts a string to a nullable int.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable int.</returns>
        public static int? ToNullableInt(this String str)
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
        public static DateTime ToDateTime(this String str)
        {
            return DateTime.Parse(str);
        }

        /// <summary>
        /// Converts a string to a DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTime(this String str, String format)
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
        public static DateTime ToDateTime(this String str, String format, IFormatProvider formatProvider)
        {
            return DateTime.ParseExact(str, format, formatProvider);
        }

        public static DateTime ToUniversalDateTime(this String str)
        {
            return str.ToDateTime().ToUniversalTime();
        }

        /// <summary>
        /// Converts a string to a universal DateTime using ParseExact.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format">The format.</param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToUniversalDateTime(this String str, String format)
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
        public static DateTime ToUniversalDateTime(this String str, String format, IFormatProvider formatProvider)
        {
            return str.ToDateTime(format, formatProvider).ToUniversalTime();
        }

        /// <summary>
        /// Converts a string to a DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeIgnoreOffset(this String str)
        {
            return str.ToDateTimeIgnoreOffset(null);
        }

        /// <summary>
        /// Converts a string to a DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        ///<param name="formatProvider"></param>
        /// <returns>The DateTime.</returns>
        public static DateTime ToDateTimeIgnoreOffset(this String str, IFormatProvider formatProvider)
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
        public static DateTime ToDateTimeIgnoreOffset(this String str, IFormatProvider formatProvider, DateTimeStyles styles)
        {
            return DateTimeOffset.Parse(str, formatProvider, styles).DateTime;
        }

        /// <summary>
        /// Converts a string to a nullable DateTime object ignoring any given offset.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTimeIgnoreOffset(this String str)
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
        public static DateTime? ToNullableDateTimeIgnoreOffset(this String str, IFormatProvider formatProvider)
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
        public static DateTime? ToNullableDateTimeIgnoreOffset(this String str, IFormatProvider formatProvider, DateTimeStyles styles)
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
        public static DateTime? ToNullableDateTime(this String str)
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
        public static DateTime? ToNullableUniversalDateTime(this String str)
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
        public static DateTime? ToNullableDateTime(this String str, String format)
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
        public static DateTime? ToNullableDateTime(this String str, String format, IFormatProvider formatProvider)
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
        public static DateTime? ToNullableUniversalDateTime(this String str, String format)
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
        public static DateTime? ToNullableUniversalDateTime(this String str, String format, IFormatProvider formatProvider)
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
        public static Boolean ToBoolean(this String str)
        {
            Boolean.TryParse(str, out Boolean ret);
            return ret;
        }

        /// <summary>
        /// Converts a string to a nullable boolean.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable boolean.</returns>
        public static Boolean? ToNullableBoolean(this String str)
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
        public static long ToLong(this String str)
        {
            long.TryParse(str, out long num);
            return num;
        }

        /// <summary>
        /// Converts a string to a nullable long.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable long.</returns>
        public static long? ToNullableLong(this String str)
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
        public static Boolean IsShort(this String str)
        {
            return short.TryParse(str, out short num);
        }

        /// <summary>
        /// Gets whether or not a string represents an integer.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsInt(this String str)
        {
            return int.TryParse(str, out int num);
        }

        /// <summary>
        /// Determines whether or not a string represents a single precision floating point number
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsSingle(this String str)
        {
            return float.TryParse(str, out float num);
        }

        /// <summary>
        /// Determines whether or not a string represents a double precision floating point number
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsDouble(this String str)
        {
            return double.TryParse(str, out double num);
        }

        /// <summary>
        /// Gets whether or not a string represents a byte.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsByte(this String str)
        {
            return Byte.TryParse(str, out Byte b);
        }

        /// <summary>
        /// Gets whether or not a string represents a signed byte.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsSByte(this String str)
        {
            return SByte.TryParse(str, out SByte b);
        }

        /// <summary>
        /// Gets whether or not a string represents a long.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsLong(this String str)
        {
            return long.TryParse(str, out long num);
        }

        /// <summary>
        /// Gets whether or not a string represents a decimal.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True/False</returns>
        public static Boolean IsDecimal(this String str)
        {
            return Decimal.TryParse(str, out Decimal num);
        }

        /// <summary>
        /// Converts a string to a decimal.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The decimal.</returns>
        public static Decimal ToDecimal(this String str)
        {
            return Decimal.Parse(str);
        }

        /// <summary>
        /// Converts a string to a nullable decimal.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable decimal.</returns>
        public static Decimal? ToNullableDecimal(this String str)
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
        public static float ToFloat(this String str)
        {
            return float.Parse(str);
        }

        /// <summary>
        /// Converts a string to a nullable float.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable float.</returns>
        public static float? ToNullableFloat(this String str)
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
        public static double ToDouble(this String str)
        {
            return double.Parse(str);
        }

        /// <summary>
        /// Converts a string to a nullable double.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The nullable double.</returns>
        public static double? ToNullableDouble(this String str)
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
        public static Boolean IsDateTime(this String str)
        {
            return DateTime.TryParse(str, out DateTime dt);
        }

        /// <summary>
        /// REturns true/false if the string can be parsed as a DateTime
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="format">The DateTime format</param>
        /// <returns>true/false</returns>
        public static Boolean IsDateTimeExact(this String str, String format)
        {
            return DateTime.TryParseExact(str, format, null, System.Globalization.DateTimeStyles.None, out DateTime dt);
        }

        /// <summary>
        /// Returns a substring of the string.  If the string is null it returns null or if the length of the string is too long it will not crash
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The index at which to start</param>
        /// <returns>The substring</returns>
        public static String SubstringSafe(this String str, int start)
        {
            if (str == null)
                return null;

            return new String(str.Skip(start).ToArray());
        }

        /// <summary>
        /// Returns a substring of the string.  If the string is null it returns null or if the length of the string is too long it will not crash
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">The index at which to start</param>
        /// <param name="length">The length to retrieve</param>
        /// <returns>The substring</returns>
        public static String SubstringSafe(this String str, int start, int length)
        {
            if (str == null)
                return null;

            return new String(str.Skip(start).Take(length).ToArray());
        }

        /// <summary>
        /// Removes all leading and trailing whitespace characters from the string.  Returns null if the string is null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The trimmed string.</returns>
        public static String TrimSafe(this String str)
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
        public static String TrimSafe(this String str, params Char[] trimChars)
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
        public static String ToLowerSafe(this String str)
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
        public static String ToLower(this String str, Boolean doLower)
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
        public static String ToUpper(this String str, Boolean doUpper)
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
        public static String ToUpperSafe(this String str)
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
        public static Boolean ContainsSafe(this String str, String value)
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
        public static String Remove(this String text, String toRemove)
        {
            return text.Replace(toRemove, "");
        }

        /// <summary>
        /// Removes all instances of a character from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toRemove">The character to remove</param>
        /// <returns>The resulting string or null if the string was null</returns>
        public static String RemoveSafe(this String text, Char toRemove)
        {
            return text.RemoveSafe(toRemove.ToString());
        }

        /// <summary>
        /// Removes all instances of a string from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toRemove">The string to remove</param>
        /// <returns>The resulting string or null if the string was null</returns>
        public static String RemoveSafe(this String text, String toRemove)
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
        public static String ReplaceSafe(this String text, Char oldChar, Char newChar)
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
        public static String ReplaceSafe(this String text, String oldValue, String newValue)
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
        public static String GetRandomString(this String allowedChars, int length)
        {
            var stringChars = new Char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = allowedChars[StringExtensions._Random.Next(allowedChars.Length)];
            }

            return new String(stringChars);
        }

        /// <summary>
        /// Gets a random string of just letters
        /// </summary>
        /// <param name="length">The length</param>
        /// <returns>A random string</returns>
        public static String GetRandomAlphaString(int length = 1)
        {
            return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".GetRandomString(length);
        }

        /// <summary>
        /// Gets a guid for a string
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>The guid</returns>
        public static Guid ToGuid(this String str)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(str));
                return new Guid(hash);
            }
        }

        /// <summary>
        /// Serializes an object to JSON.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="includeNonPublic">Include non public members.  Defaults to false.</param>
        /// <param name="ignoreReferenceLoops">Ignore reference loops.  Defaults to true.</param>
        /// <returns>The JSON string.</returns>
        public static String ToJSON(this Object obj)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IgnoreReadOnlyProperties = true;
            return JsonSerializer.Serialize(obj, obj.GetType(), options);
        }

        /// <summary>
        /// Deserializes a JSON string back into an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="includeNonPublic">Deserialize non public members.  Defaults to false.</param>
        /// <returns>The object.</returns>
        public static T FromJSON<T>(this String json)
        {
            if (typeof(T) == typeof(XElement))
                return (T)(dynamic)new XElement(json.FromJSON<String>());
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