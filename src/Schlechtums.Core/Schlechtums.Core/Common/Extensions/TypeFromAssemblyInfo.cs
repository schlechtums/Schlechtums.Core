using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public class TypeFromAssemblyInfo
    {
        public String Fullpath { get; set; }
        public String AssemblyFullName { get; set; }
        public Type Type { get; set; }
    }
}