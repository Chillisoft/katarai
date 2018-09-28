using Engine.Runners;
using Katarai.Runner;
using Katarai.Utils;
using Katarai.Wpf.Monitor;

namespace Katarai.Wpf
{
    public interface ISplunkLogger
    {
        void Log(PlayerImplementationRunResult playerImplementationRunResult, PlayerTestsRunResult playerTestsRunResult, 
            PlayerFeedback playerFeedback, IKataraiApp kataraiApp, IFileSystem fileSystem);
        void LogFeedback(string message);
        void Log(MonitorEvent monitorEvent);
    }
}