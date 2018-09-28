using System;
using Engine;

namespace Katarai.Wpf.Monitor
{
    public class KataLogPlayerNotifier : IPlayerNotifier
    {
        private readonly ILogTarget _logTarget;
        private bool _displayedCritialError;
        public event EventHandler KataStarted;
        public event EventHandler KataCompleted;

        public KataLogPlayerNotifier(ILogTarget logTarget)
        {
            if (logTarget == null) throw new ArgumentNullException("logTarget");
            _logTarget = logTarget;
        }

        public void DisplayMessage(string title, string msg, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon)
        {
            _displayedCritialError = false;
            _logTarget.LogInfo(string.Format("({0}) {1} - {2}", kataStateIcon, title, msg));
        }

        public void DisplayMessage(string title, string msg)
        {
            
        }

        public void DisplayErrorMessage(string errorMessage)
        {
            if (_displayedCritialError) return;
            _logTarget.LogError(errorMessage);
            _displayedCritialError = true;
        }

        public void Dispose()
        {
            _logTarget.Dispose();
        }

        public void SetKataStarted()
        {

        }

        public void SetKataCompleted()
        {
            
        }
    }
}