using System;
using System.Collections.Generic;
using Katarai.Runner;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.Monitor
{
    public interface IKataraiApp
    {
        event EventHandler AttemptGameStateChanged;
        AttemptGameState AttemptGameState { get; set; }
        ISettingsManager SettingsManager { get; }

        void SetCurrentKataAttempt(IKataAttempt currentKataAttempt);
        void SetAttemptAbandoned();
    }

    public class KataraiApp : IKataraiApp
    {
        private readonly ISettingsManager _settingsManager;
        private IKataAttempt _currentKataAttempt;
        private AttemptGameState _attemptGameState;
        public event EventHandler AttemptGameStateChanged;


        public virtual ISettingsManager SettingsManager
        {
            get { return _settingsManager; }
        }

        public AttemptGameState AttemptGameState
        {
            get { return _attemptGameState; }
            set
            {
                //TODO mark 01 Apr 2015: Sori test this
                _attemptGameState = value;
                FireAttemptGameStateChanged();
            }
        }

        public KataraiApp(ISettingsManager settingsManager)
        {
            if (settingsManager == null) throw new ArgumentNullException(nameof(settingsManager));
            _settingsManager = settingsManager;
        }

        private void FireAttemptGameStateChanged()
        {
            var handler = AttemptGameStateChanged;
            if (handler != null) handler(this, new EventArgs());
        }

        public void SetCurrentKataAttempt(IKataAttempt currentKataAttempt)
        {
            if (_currentKataAttempt == currentKataAttempt) return;
            _currentKataAttempt = currentKataAttempt;
            UpdateCurrentKataAttemptSettings(currentKataAttempt);
            AttemptGameState = new AttemptGameState(new List<Result>(), new KataHelper());
            AttemptGameState.KataName = currentKataAttempt.Config.KataName;
        }

        private void UpdateCurrentKataAttemptSettings(IKataAttempt currentKataAttempt)
        {
            var settings = SettingsManager.FetchCurrentSettings();
            settings.KataHash = string.Empty;
            settings.PlayerHash = string.Empty;
            if (currentKataAttempt != null)
            {
                settings.KataPath = currentKataAttempt.Config.MasterDllPath;
                settings.PlayerPath = currentKataAttempt.Config.PlayerDllPath;
            }
            else
            {
                settings.KataPath = null;
                settings.PlayerPath = null;
            }
            if (!SettingsManager.PersistSettings(settings))
            {
                RaiseErrorNotification("Your Assembly Paths Were Not Saved");
            }
        }

        private void RaiseErrorNotification(string message)
        {
            
            //if (ErrorNotification != null) ErrorNotification(this, new MessageEventArgs(message));
        }

        public void SetAttemptAbandoned()
        {
            if (this.AttemptGameState==null) return;
            this.AttemptGameState.SetAttemptAbandoned();
        }
    }
}