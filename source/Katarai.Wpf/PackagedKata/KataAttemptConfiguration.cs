using System.Collections.Generic;
using System.IO;

namespace Katarai.Wpf.PackagedKata
{
    public class Instructions
    {
        public string Header { get; set; }
        public List<string> Content { get; set; }

        public Instructions()
        {
            Content = new List<string>();
        }
    }

    public interface IKataAttemptConfiguration
    {
        string MasterDll { get; }
        string SolutionName { get; }
        string PlayerDll { get; }
        Instructions Instructions { get; }
        string KataName { get; }
        string MasterDllPath { get; }
        string SolutionFilePath { get; }
        string PlayerDllPath { get; }
    }

    public class KataAttemptConfiguration : IKataAttemptConfiguration
    {
        private readonly string _location;

        public string MasterDll { get; private set; }
        public string SolutionName { get; private set; }
        public string PlayerDll { get; private set; }
        public Instructions Instructions { get; private set; }
        public string KataName { get; private set; }
        public KataAttemptConfiguration(string masterDll, string solutionName, string playerDll, string location = "", Instructions instructions = null, string kataName = null)
        {
            _location = location;
            Instructions = instructions;
            MasterDll = masterDll;
            SolutionName = solutionName;
            PlayerDll = playerDll;
            KataName = kataName;
        }

        public string MasterDllPath
        {
            get { return Path.Combine(_location, "MasterSolution", MasterDll); }
        }

        public string SolutionFilePath
        {
            get { return Path.Combine(_location, "PlayerSolution", SolutionName); }
        }

        public string PlayerDllPath
        {
            get { return BuildAssemblyPath(_location, PlayerDll); }
        }

        private string BuildAssemblyPath(string kataArchive, string playerDllName)
        {
            return Path.Combine(kataArchive, "PlayerSolution", "bin", "Debug", playerDllName);
        }
    }
}
