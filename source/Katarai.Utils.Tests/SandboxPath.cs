using System;
using System.IO;

namespace Katarai.Utils.Tests
{
    public class SandboxPath : IDisposable
    {
        public string FullPath { get; set; }

        public SandboxPath()
        {
            FullPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()); 
            Directory.CreateDirectory(FullPath);
        }

        public void Dispose()
        {
            if (Directory.Exists(FullPath))
            {
                Directory.Delete(FullPath, recursive: true);
            }
        }
    }
}