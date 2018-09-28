using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Katarai.Runner;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.AnalysisContainers.Delegates
{
    internal class SandboxDelegate : MarshalByRefObject
    {
        public Result ExecuteInDelegate(KataraiSettings settings, string shadowLocation)
        {
            var newPlayerAssemblyPath = ShadowCopyPlayerAssembly(shadowLocation, settings.PlayerPath);
            var runner = new Runner.Runner(settings.KataPath, newPlayerAssemblyPath, newPlayerAssemblyPath);
            return runner.Run();
        }

        private static string ShadowCopyPlayerAssembly(string shadowLocation, string playerAssemblyPath)
        {
            var outputDirectory = Path.GetDirectoryName(playerAssemblyPath);

            if (outputDirectory == null) return null;

            var filesToCopy = Directory.EnumerateFiles(outputDirectory,"*.*").ToList();
            foreach (var file in filesToCopy)
            {

                try
                {
                    CopyFile(shadowLocation, file);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return GenereatePlayerPath(shadowLocation, playerAssemblyPath);
        }

        private static void CopyFile(string shadowLocation, string file)
        {
            var fileName = Path.GetFileName(file);
            if (fileName == null) return;
            var newPath = Path.Combine(shadowLocation, fileName);
            File.Copy(file, newPath);
        }

        private static string GenereatePlayerPath(string shadowLocation, string playerAssemblyPath)
        {
            var playerAssemblyName = Path.GetFileName(playerAssemblyPath);

            if (playerAssemblyName != null)
            {
                return Path.Combine(shadowLocation, playerAssemblyName);
            }

            return null;
        }
    }
}
