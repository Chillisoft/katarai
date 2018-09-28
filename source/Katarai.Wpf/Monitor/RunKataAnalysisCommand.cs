using System;
using System.Collections.Generic;
using System.IO;
using Engine;
using Katarai.Runner;
using Katarai.Utils;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.Monitor
{
    public interface IRunKataAnalysisCommand
    {
        IAnalysisContainer AnalysisContainer { get; }
        void Execute();
    }

    public class RunKataAnalysisCommand : IRunKataAnalysisCommand
    {
        public IAnalysisContainer AnalysisContainer { get; private set; }

        private readonly ISettingsManager _settingsManager;
        private readonly IKataraiApp _kataraiApp;
        private readonly IAnalysisRunnerFactory _analysisRunnerFactory;

        private readonly Dictionary<string, KataLogPlayerNotifier> _kataLogNotifiers =
            new Dictionary<string, KataLogPlayerNotifier>();

        private readonly IPlayerNotifier _playerNotifier;
        private readonly IAnalysisResultProcessor _analysisResultProcessor;

        public RunKataAnalysisCommand(IPlayerNotifier playerNotifier, 
            ISettingsManager settingsManager, 
            IAnalysisContainer analysisContainer, 
            IAnalysisRunnerFactory analysisRunnerFactory, 
            IAnalysisResultProcessor analysisResultProcessor, 
            IKataraiApp kataraiApp)
        {
            if (playerNotifier == null) throw new ArgumentNullException("playerNotifier");
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            if (analysisContainer == null) throw new ArgumentNullException("analysisContainer");
            if (analysisRunnerFactory == null) throw new ArgumentNullException("analysisRunnerFactory");
            if (kataraiApp == null) throw new ArgumentNullException("kataraiApp");
            _playerNotifier = playerNotifier;
            _settingsManager = settingsManager;
            _analysisRunnerFactory = analysisRunnerFactory;
            AnalysisContainer = analysisContainer;
            _analysisResultProcessor = analysisResultProcessor;
            _kataraiApp = kataraiApp;
        }

        public void Execute()
        {
            var kataraiSettings = _settingsManager.FetchCurrentSettings();
            if (string.IsNullOrEmpty(kataraiSettings.PlayerPath)) return;
            if(!File.Exists(kataraiSettings.PlayerPath)) return;
            var playerAssemblyFolder = Path.GetDirectoryName(kataraiSettings.PlayerPath);
            if (playerAssemblyFolder == null) return;
            var kataLogNotifier = GetKataLogNotifier(playerAssemblyFolder);
            var notifier = new CompositePlayerNotifier(_playerNotifier, kataLogNotifier);
            try
            {
                string errorMessage;
                var analysisRunner = _analysisRunnerFactory.CreateAnalysisRunner(_settingsManager, AnalysisContainer);
                var result = analysisRunner.Run(out errorMessage);
                var attemptGameState = _kataraiApp.AttemptGameState;
                _analysisResultProcessor.ProcessAnalysisResult(notifier, result, errorMessage, attemptGameState);
                LogResult(result);
            }
            catch (Exception e)
            {
                notifier.DisplayMessage("Analysis Error", e.Message, NotifyIcon.Warning, NotifyIcon.Red);
            }
        }

        private void LogResult(Result result)
        {
            if (result != null)
            {
                var splunkLogger = new SplunkLogger(new SplunkAppender(new SplunkSettings()), new KataHelper());
                splunkLogger.Log(result.PlayerImplementationRunResult, result.PlayerTestsRunResult, result.PlayerFeedback,
                    _kataraiApp, new FileSystemAdapter());
            }
        }

        private KataLogPlayerNotifier GetKataLogNotifier(string playerAssemblyFolder)
        {
            if (playerAssemblyFolder == null) return null;
            var logFilePath = Path.Combine(playerAssemblyFolder, "Kata.log");

            KataLogPlayerNotifier kataLogPlayerNotifier;
            if (!_kataLogNotifiers.TryGetValue(logFilePath, out kataLogPlayerNotifier))
            {
                ClearLogNotifierCache();
                var log4NetLogTarget = new TextFileLogTarget(logFilePath,new FileSystemAdapter());
                kataLogPlayerNotifier = new KataLogPlayerNotifier(log4NetLogTarget);
                _kataLogNotifiers.Add(logFilePath, kataLogPlayerNotifier);
            }
            return kataLogPlayerNotifier;
        }

        private void ClearLogNotifierCache()
        {
            foreach (var logNotifier in _kataLogNotifiers)
            {
                logNotifier.Value.Dispose();
            }
            _kataLogNotifiers.Clear();
        }
    }
}