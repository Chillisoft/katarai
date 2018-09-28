using System;
using Katarai.Utils;

namespace Katarai.Wpf.Monitor
{
    public class TextFileLogTarget : ILogTarget
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _fileName;

        public TextFileLogTarget(string fileName, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if (fileSystem == null) throw new ArgumentNullException("fileSystem");
            _fileName = fileName;
            _fileSystem = fileSystem;
        }

        public void LogInfo(string message)
        {
            WriteMessageToFile("INFO", message);
        }

        private void WriteMessageToFile(string level, string message)
        {
            var text = FormatMessage(level, message);
            var folder = _fileSystem.GetDirectoryName(_fileName);
            if (string.IsNullOrEmpty(folder)) return;
            if (!_fileSystem.DirectoryExists(folder))
            {
                _fileSystem.CreateDirectory(folder);
            }
            _fileSystem.AppendAllLines(_fileName, new[] {text});
        }

        private static string FormatMessage(string level, string message)
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}: [{1}] - {2}", DateTime.Now, level, message);
        }

        public void LogError(string message)
        {
            WriteMessageToFile("ERROR", message);
        }

        public void Dispose()
        {
            
        }
    }
}