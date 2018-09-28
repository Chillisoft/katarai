using System;
using System.Windows.Threading;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.Monitor
{
    public class KataFilesMonitor : IKataFilesMonitor
    {
        private readonly ISettingsManager _settingsManager;
        private DispatcherTimer _fileWatcherTimer;
        private readonly IRunKataAnalysisCommand _runKataAnalysisRunKataAnalysisCommand;
        
        internal DispatcherTimer FileWatcherTimer
        {
            get { return _fileWatcherTimer; }
        }

        public KataFilesMonitor(ISettingsManager settingsManager, 
                                        IRunKataAnalysisCommand runKataAnalysisRunKataAnalysisCommand)
        {
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            if (runKataAnalysisRunKataAnalysisCommand == null) throw new ArgumentNullException("runKataAnalysisRunKataAnalysisCommand");
            _settingsManager = settingsManager;
            _runKataAnalysisRunKataAnalysisCommand = runKataAnalysisRunKataAnalysisCommand;
        }

        private void CreateFileWatcher()
        {
            _settingsManager.LoadSettings();

            _fileWatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            FileWatcherTimer.Tick += ((sender, e1) =>
            {
                _runKataAnalysisRunKataAnalysisCommand.Execute();
            });
        }

        public void Start()
        {
            if (FileWatcherTimer == null)
            {
                CreateFileWatcher();

                if (FileWatcherTimer != null)
                {
                    FileWatcherTimer.Start();
                }
            }
        }

        public void Stop()
        {
            _settingsManager.PersistSettings();

            if (FileWatcherTimer != null)
            {
                FileWatcherTimer.Stop();
            }
        }
    }
}