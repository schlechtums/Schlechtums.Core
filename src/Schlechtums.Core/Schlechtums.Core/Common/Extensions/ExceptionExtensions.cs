using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Schlechtums.Core.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static int GetHResult(this Exception ex)
        {
            var info = new SerializationInfo(typeof(IOException), new FormatterConverter());
            ex.GetObjectData(info, new StreamingContext());
            return info.GetInt32("HResult");
        }

        /// <summary>
        /// Gets this and all inner exceptions' messages.
        /// </summary>
        /// <param name="ex">The extension.</param>
        /// <param name="includeStackTrace">true/false to include the stacktrace or not.</param>
        /// <returns>The messages.</returns>
        public static IEnumerable<String> GetExceptionMessages(this Exception ex, Boolean includeStackTrace = false)
        {
            while (ex != null)
            {
                var aex = ex as AggregateException;
                if (aex != null)
                {
                    foreach (var aem in aex.InnerExceptions.SelectMany(ae => ae.GetExceptionMessages(includeStackTrace)))
                        yield return aem;
                }
                else
                {
                    yield return ex.Message;
                }

                if (includeStackTrace && ex.StackTrace != null)
                    yield return ex.StackTrace.ToString();

                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// Gets this and all inner exceptions' messages.
        /// </summary>
        /// <param name="ex">The extension.</param>
        /// <param name="includeStackTrace">true/false to include the stacktrace or not.</param>
        /// <returns>The messages delimited by newlines.</returns>
        public static String GetExceptionMessagesAsString(this Exception ex, Boolean includeStackTrace = false)
        {
            return ex.GetExceptionMessagesAsString(Environment.NewLine + Environment.NewLine, includeStackTrace);
        }

        /// <summary>
        /// Gets this and all inner exceptions' messages.
        /// </summary>
        /// <param name="ex">The extension.</param>
        /// <param name="delimiter">The message delimter.</param>
        /// <param name="includeStackTrace">true/false to include the stacktrace or not.</param>
        /// <returns>The messages delimited by newlines.</returns>
        public static String GetExceptionMessagesAsString(this Exception ex, String delimiter, Boolean includeStackTrace = false)
        {
            return ex.GetExceptionMessages(includeStackTrace).Distinct().Join(delimiter);
        }
    }
}