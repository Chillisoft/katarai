using System;
using System.IO;

namespace Katarai.Wpf.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"),
                Path.GetExtension(fileName)
                );
        }
    }
}
