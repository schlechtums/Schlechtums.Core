using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schlechtums.Core.Common.Extensions
{
    public static class FileExtensionFilter
    {
        public enum FileExtensionFilterType : ulong
        {
            All = 0x8000000000000000,
            Xml = 0x1,
            Txt = 0x4,
            Csv = 0x8,
            Html = 0x10,
            AllMdl = 0x20
        }

        internal static Dictionary<FileExtensionFilterType, FileExtensionFilterTypeDescriptor> extensionDescriptions = new Dictionary<FileExtensionFilterType, FileExtensionFilterTypeDescriptor>
        {
            { FileExtensionFilterType.All, new FileExtensionFilterTypeDescriptor("All Files", "*.*") },
            { FileExtensionFilterType.Csv, new FileExtensionFilterTypeDescriptor("Comma Delimited Files", "*.csv") },
            { FileExtensionFilterType.Txt, new FileExtensionFilterTypeDescriptor("TXT Files", "*.txt") },
            { FileExtensionFilterType.Xml, new FileExtensionFilterTypeDescriptor("XML Files", "*.xml") },
            { FileExtensionFilterType.Html, new FileExtensionFilterTypeDescriptor("Html Files", "*.htm; *.html") }
        };

        public static string ToFilterString(this FileExtensionFilterType feft)
        {
            var types = new List<string>();
            foreach (var feftt in (Enum.GetValues(typeof(FileExtensionFilterType)) as FileExtensionFilterType[]).OrderBy(v =>
            {
                if (v == FileExtensionFilterType.All)
                    return int.MaxValue;
                else if (v.ToString().Count(c => char.IsUpper(c)) > 1)
                    return 0;
                else
                    return 1;
            }))
            {
                if ((feft & feftt) == feftt)
                    types.Add(string.Format("{0} ({1})|{2}", FileExtensionFilter.extensionDescriptions[feftt].Description, FileExtensionFilter.extensionDescriptions[feftt].Extension.Replace(";", ", "), FileExtensionFilter.extensionDescriptions[feftt].Extension));
            }
            return types.Join("|");
        }

        internal class FileExtensionFilterTypeDescriptor
        {
            internal FileExtensionFilterTypeDescriptor(string description, string extension)
            {
                this.Description = description;
                this.Extension = extension;
            }

            public string Description { get; set; }
            public string Extension { get; set; }
        }
    }
}