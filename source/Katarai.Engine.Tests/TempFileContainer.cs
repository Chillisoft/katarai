using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Katarai.Engine.Tests
{
    public class TempFileContainer
    {
        private readonly List<string> _tempFiles = new List<string>();

        public void ClearTempFiles()
        {
            lock (this)
            {
                foreach (var f in _tempFiles)
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch
                    {
                    }
                }
                _tempFiles.Clear();
            }
        }

        public string GetTempAssembly(string subDirectory, string asmName, bool validatePath)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var dllName = asmName + ".dll";
            var source = Path.Combine(currentDir, subDirectory, dllName);
            if (!validatePath) return source;

            if (!File.Exists(source))
                throw new Exception("unable to find test data source at: " + source);
            var tempFile = Path.GetTempFileName() + ".dll";
            File.WriteAllBytes(tempFile, File.ReadAllBytes(source));
            _tempFiles.Add(tempFile);
            return tempFile;
        }
    }
}