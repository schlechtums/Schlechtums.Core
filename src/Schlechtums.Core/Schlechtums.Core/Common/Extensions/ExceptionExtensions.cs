using System;
using System.IO;
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
    }
}