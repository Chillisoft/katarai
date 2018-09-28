using System;
using System.IO;
using Caliburn.Micro;
using Katarai.Wpf.Events;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using Katarai.Wpf.Extensions;
using Action = System.Action;

namespace Katarai.Wpf.PackagedKata
{
    public interface IKataArchive
    {
        string ArchiveRoot { get; }
        IKataAttempt GenerateSolutionForAttempt(KataName selectedKata);
        Action GenerateLaunchActionFor(string unpackLocation);
    }

    public class KataArchive:IKataArchive
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IKataAttemptRepository _kataAttemptRepository;
        private readonly IVisualStudioHelper _visualStudioHelper;
        private readonly ISettingsManager _settingsManager;

        public static string GlobalArchiveRoot
        {
            get {
                var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "Local");
                var fullPath = Path.Combine(rootPath, "Katarai");
                return fullPath;
            }
        }

        public KataArchive(IEventAggregator eventAggregator,
                            IKataAttemptRepository kataAttemptRepository,
                            IVisualStudioHelper visualStudioHelper,
                            ISettingsManager settingsManager)
        {
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (kataAttemptRepository == null) throw new ArgumentNullException(nameof(kataAttemptRepository));
            if (visualStudioHelper == null) throw new ArgumentNullException(nameof(visualStudioHelper));
            if (settingsManager == null) throw new ArgumentNullException(nameof(settingsManager));
            _eventAggregator = eventAggregator;
            _kataAttemptRepository = kataAttemptRepository;
            _visualStudioHelper = visualStudioHelper;
            _settingsManager = settingsManager;
        }

        public string ArchiveRoot
        {
            get { return GlobalArchiveRoot; }
        }

        public IKataAttempt GenerateSolutionForAttempt(KataName selectedKata)
        {
            var newKataAttempt = _kataAttemptRepository.CreateNewKataAttempt(selectedKata);
            var attemptLocation = newKataAttempt.Location;
                _eventAggregator.Publish<DisplayFeedbackEvent>(string.Format("CREATED SOLUTION [ {0} ] FOR  [ {1} ]",
                    attemptLocation, selectedKata));
            return _kataAttemptRepository.LoadKataAttemptFrom(attemptLocation);
;
        }

        public Action GenerateLaunchActionFor(string unpackLocation)
        {
            var kataAttempt = _kataAttemptRepository.LoadKataAttemptFrom(unpackLocation);
            return GenerateReturnAction(kataAttempt);

        }

        private Action GenerateReturnAction(IKataAttempt kataAttempt)
        {
            Action result = () =>
            {
                var attemptConfig = kataAttempt.Config;
                if (string.IsNullOrEmpty(attemptConfig.PlayerDll))
                {
                    _eventAggregator.Publish<DisplayFeedbackEvent>("Error Reading Configuration For Kata Package");
                }
                SetKataAndPlayPaths(attemptConfig.PlayerDllPath, attemptConfig.MasterDllPath);
                LaunchSolution(attemptConfig.SolutionFilePath);
            };

            return result;
        }

        private void LaunchSolution(string playerProject)
        {
            if (!_visualStudioHelper.LaunchProject(playerProject))
            {
                _eventAggregator.Publish<DisplayFeedbackEvent>("Failed to launch selected kata");
            }
        }

        private void SetKataAndPlayPaths(string assemblyPath, string kataPath)
        {
            lock(_settingsManager)
            {
                var settings = _settingsManager.FetchCurrentSettings();
                settings.PlayerPath = assemblyPath;
                settings.KataPath = kataPath;
                _settingsManager.SetSettings(settings);
            }
        }
    }
}
