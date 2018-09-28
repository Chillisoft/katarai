using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Katarai.Utils;
using Katarai.Wpf.ExtensionMethods;
using Katarai.Wpf.PackagedKata;

namespace Katarai.Wpf
{
    public class FileSystemKatasAttemptRepository : IKataAttemptRepository
    {
        private readonly IKataSolutionExtractor _kataSolutionExtractor;

        public FileSystemKatasAttemptRepository()
        {
            _kataSolutionExtractor = new KataSolutionExtractor(); //TODO mark 09 Feb 2015: Convert to injection!
        }

        public IList<IKataAttempt> GetKataInfos()
        {
            var katasFolder = EnsureFolderExists(GetKataraiKatasPath());

            var katasFolders = katasFolder.GetDirectories();
            return katasFolders.Select(directoryInfo => new KataAttempt(new FileSystemAdapter())
            {
                Name = directoryInfo.Name,
                Location = directoryInfo.FullName
            } as IKataAttempt).ToList();
        }

        private static string GetKataraiKatasPath()
        {
            var roamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var localAppData = roamingAppData.Replace("Roaming", "Local");
            return Path.Combine(localAppData, "Katarai");
        }

        public IKataAttempt CreateNewKataAttempt(KataName selectedKata)
        {
            var katasFolder = EnsureFolderExists(GetKataraiKatasPath());

            var attemptName = GenereateAttemptNameFor(selectedKata);
            var attemptPath = Path.Combine(katasFolder.FullName, attemptName);
            var attemptFolder = EnsureFolderExists(attemptPath);

            return _kataSolutionExtractor.ExtractKataTo(attemptPath, selectedKata)
                ? LoadKataAttemptFrom(attemptFolder)
                : null;
        }

        private DirectoryInfo EnsureFolderExists(string folderLocation)
        {
            if (!Directory.Exists(folderLocation))
            {
                return Directory.CreateDirectory(folderLocation);
            }
            return new DirectoryInfo(folderLocation);
        }

        private string GenereateAttemptNameFor(KataName selectedKata)
        {
            var kataName = selectedKata + "-";
            return kataName.AppendTimeStamp();
        }

        public string GetMasterSolutionAssemblyPath(IKataAttempt kataAttempt)
        {
            return kataAttempt.Config.MasterDllPath;
        }

        public string GetPlayerSolutionAssemblyPath(IKataAttempt kataAttempt)
        {
            return kataAttempt.Config.PlayerDllPath;
        }

        public IKataAttempt LoadKataAttemptFrom(string unpackLocation)
        {
            var kataAttemptFolder = new DirectoryInfo(unpackLocation);
            return LoadKataAttemptFrom(kataAttemptFolder);
        }

        private static KataAttempt LoadKataAttemptFrom(DirectoryInfo kataAttemptFolder)
        {
            return new KataAttempt(new FileSystemAdapter())
            {
                Name = kataAttemptFolder.Name,
                Location = kataAttemptFolder.FullName
            };
    }
    }
}