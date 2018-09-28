using System;
using System.Globalization;
using System.IO;
using Katarai.Runner;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.FileWatch
{
    public class FileHashesManager:IFileHashesManager
    {
        public FileMonitorInformation GetFileHashes(KataraiSettings settings)
        {
            var result = new FileMonitorInformation
            {
                OldKataHash = settings.KataHash,
                OldPlayerHash = settings.PlayerHash,
                KataHash = FetchFileLastModifedTime(settings.KataPath),
                PlayerHash = FetchFileLastModifedTime(settings.PlayerPath)
            };

            return result;
        }

        private static string FetchFileLastModifedTime(string path)
        {
            return !string.IsNullOrEmpty(path) ? File.GetLastWriteTime(path).ToLongTimeString().GetHashCode().ToString(CultureInfo.InvariantCulture) : string.Empty;
        }
    }
}