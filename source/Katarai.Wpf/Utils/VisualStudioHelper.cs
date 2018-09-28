using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Katarai.Wpf.Utils
{
    public interface IVisualStudioHelper
    {
        bool LaunchProject(string projectPath);
    }

    public class VisualStudioHelper: IVisualStudioHelper
    {
        public bool LaunchProject(string projectPath)
        {
            var visualStudioPath = GetVisualStudioPath();
            if (!Launch(projectPath, visualStudioPath)) return false;

            return true;
        }

        private static bool Launch(string projectPath, string visualStudioPath)
        {
            if (!File.Exists(visualStudioPath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(projectPath))
            {
                throw new ArgumentException("projectPath");
            }

            var psi = new ProcessStartInfo(visualStudioPath, projectPath) {UseShellExecute = true };

            Process.Start(psi);
            return true;
        }

        private static string GetVisualStudioPath()
        {
            // 9 - 2008
            // 10 - 2010
            // 11 - 2012
            // 12 - 2013
            var devenvPath = GetDevenvPath("12.0");
            if (string.IsNullOrEmpty(devenvPath))
            {
                devenvPath = GetDevenvPath("11.0");
            }
            return devenvPath;
        }

        private static string GetDevenvPath(string vsVersion)
        {
           var vsInstallPath = (string)Registry.GetValue(string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio\\{0}", vsVersion), "InstallDir", "");
           if (string.IsNullOrEmpty(vsInstallPath)) return null;
           return Path.Combine(vsInstallPath, "devenv.exe");
        }
    }
}
