using System;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.FileWatch;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.Monitor
{
    public class AnalysisRunnerFactory : IAnalysisRunnerFactory
    {
        public IAnalysisRunner CreateAnalysisRunner(ISettingsManager settingsManager, IAnalysisContainer analysisContainer)
        {
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            if (analysisContainer == null) throw new ArgumentNullException("analysisContainer");
            return new AnalysisRunner(settingsManager, analysisContainer, new FileHashesManager());
        }
    }

    public interface IAnalysisRunnerFactory
    {
        IAnalysisRunner CreateAnalysisRunner(ISettingsManager settingsManager, IAnalysisContainer analysisContainer);
    }
}