using System.Diagnostics;

namespace Katarai.Controls
{
    public class ProcessLauncher : IStartProcess
    {
        public void StartProcess(string url)
        {
            Process.Start(new ProcessStartInfo(url));
        }
    }
}