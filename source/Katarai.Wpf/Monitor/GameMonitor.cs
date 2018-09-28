using System;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.Monitor
{
    //TODO mark 01 Apr 2015: Sori to test this
    public interface IGameMonitor: IDisposable
    {
        void StartMonitoring();
    }

    public class GameMonitor : IGameMonitor
    {
        private readonly IKataraiApp _kataraiApp;
        private AttemptGameState _attemptGameState;

        public GameMonitor(IKataraiApp kataraiApp)
        {
            if (kataraiApp == null) throw new ArgumentNullException("kataraiApp");
            _kataraiApp = kataraiApp;
        }

        public void StartMonitoring()
        {
            _kataraiApp.AttemptGameStateChanged += KataraiApp_OnAttemptGameStateChanged;
            RegisterForKataProgress();
        }

        private void KataraiApp_OnAttemptGameStateChanged(object sender, EventArgs eventArgs)
        {
            DeregisterForKataProgress();
            _attemptGameState = _kataraiApp.AttemptGameState;
            RegisterForKataProgress();
        }

        private void RegisterForKataProgress()
        {
            if (_attemptGameState != null) _attemptGameState.KataProgress += AttemptGameStateOnKataProgress;
        }

        private void DeregisterForKataProgress()
        {
            if (_attemptGameState != null) _attemptGameState.KataProgress -= AttemptGameStateOnKataProgress;
        }

        internal void StopMonitoring()
        {
            _kataraiApp.AttemptGameStateChanged -= KataraiApp_OnAttemptGameStateChanged;
            DeregisterForKataProgress();
        }

        ~GameMonitor()
        {
            Dispose();
        }

        public void Dispose()
        {
            StopMonitoring();
        }

        private void AttemptGameStateOnKataProgress(object sender, KataProgressEventArgs kataProgressEventArgs)
        {
            var progressEvent = kataProgressEventArgs.ProgressEvent;
            var logger = new SplunkLogger(new SplunkAppender(new SplunkSettings()), new KataHelper());
            logger.Log(progressEvent, _kataraiApp);
        }
    }
}