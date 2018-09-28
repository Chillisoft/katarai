using System.Collections.Generic;

namespace Katarai.Utils
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
        string Combine(string path1, string path2);
        bool FileExists(string path);
        string GetDirectoryName(string path);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        void AppendAllLines(string path, IEnumerable<string> contents);
        string[] GetFiles(string path, string searchPattern);
    }
}