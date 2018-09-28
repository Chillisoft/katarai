using System;
using System.IO;
using Katarai.Runner;
using Katarai.Utils;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.FileWatch
{
    public class AnalysisRunner : IAnalysisRunner
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IAnalysisContainer _analysisContainer;
        private readonly IFileHashesManager _fileHashesManager;

        public AnalysisRunner(ISettingsManager settingsManager, IAnalysisContainer analysisContainer, IFileHashesManager fileHashesManager)
        {
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            if(analysisContainer == null) throw new ArgumentNullException("analysisContainer");
            _settingsManager = settingsManager;
            _analysisContainer = analysisContainer;
            _fileHashesManager = fileHashesManager;
        }

        public Result Run(out string errorMessage)
        {
            var settings = _settingsManager.FetchCurrentSettings();
            
            errorMessage = GetKataFileErrors(settings);
            if (!string.IsNullOrEmpty(errorMessage)) return null;

            var result = ExecuteAnalysis(settings);
            return result;
        }

        private static string GetKataFileErrors(KataraiSettings settings)
        {
            if (string.IsNullOrEmpty(settings.KataPath) || string.IsNullOrEmpty(settings.PlayerPath) ||
                !File.Exists(settings.KataPath) || !File.Exists(settings.PlayerPath))
            {
                return "Could not locate Player or Kata Assembly";
            }
            return string.Empty;
        }

        private Result ExecuteAnalysis(KataraiSettings settings)
        {
            if (!AreTherePlayerFileChanges(settings)) return null;
            return _analysisContainer.Execute(settings);
        }

        private bool AreTherePlayerFileChanges(KataraiSettings settings)
        {
            var fileHashes = _fileHashesManager.GetFileHashes(settings);
            _settingsManager.UpdateSettings(fileHashes);
            return fileHashes.AreThereChanges();
        }
    }
}
