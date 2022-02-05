using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public class TypeFromAssemblyInfo
    {
        public string Fullpath { get; set; }
        public string AssemblyFullName { get; set; }
        public Type Type { get; set; }
    }
}