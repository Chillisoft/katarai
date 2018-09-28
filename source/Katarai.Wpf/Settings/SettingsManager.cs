using System;
using System.Collections.Generic;
using System.IO;
using Katarai.Runner;
using Katarai.Wpf.FileWatch;
using Katarai.Wpf.Utils;
using Newtonsoft.Json;

namespace Katarai.Wpf.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private const string SettingsFileName = "Katarai.settings";

        private static KataraiSettings _theSettings;

        private static SettingsManager _instance;
        private string _kataraiAppDataFolder;
        private string _settingsFileFullPath;

        public SettingsManager()
        {
            _kataraiAppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Katarai");
            _settingsFileFullPath = Path.Combine(_kataraiAppDataFolder, SettingsFileName);
        }
        public void SetSettings(KataraiSettings settings)
        {
            _theSettings = settings;
        }

        public void UpdateSettings(FileMonitorInformation fileHashes)
        {
            _theSettings.KataHash = fileHashes.KataHash;
            _theSettings.PlayerHash = fileHashes.PlayerHash;
        }

        public KataraiSettings FetchCurrentSettings()
        {
            return _theSettings;
        }

        public void LoadSettings()
        {
            if (!File.Exists(_settingsFileFullPath))
            {
                _theSettings =  new KataraiSettings {ShowHint = true, IsAlwaysOnTop = true, NotificationVisibility = 60};
                SetDefaultReminders(_theSettings);
                return;
            }

            var jsonSettings = File.ReadAllText(_settingsFileFullPath);
            var settings = JsonConvert.DeserializeObject<KataraiSettings>(jsonSettings);
            settings.KataHash = null;
            settings.KataPath = null;
            settings.PlayerHash = null;
            settings.PlayerPath = null;
            if (settings.NotificationVisibility <= 0)
            {
                settings.NotificationVisibility = 60;
            }
            SetDefaultReminders(settings);
            _theSettings = settings;

        }

        private void SetDefaultReminders(KataraiSettings settings)
        {
            if (settings.Reminders != null) return;
            var todaysDate = DateTime.Today;
            var dailyReminderTime = new DateTime(todaysDate.Year, todaysDate.Month, todaysDate.Day, 9, 0, 0);
            settings.Reminders = new List<KataReminder>
            {
                new KataReminder {Day = DayOfWeek.Sunday.ToString(), ShowReminder = false},
                new KataReminder {Day = DayOfWeek.Monday.ToString(), ShowReminder = true, Time = dailyReminderTime},
                new KataReminder {Day = DayOfWeek.Tuesday.ToString(), ShowReminder = true, Time = dailyReminderTime},
                new KataReminder {Day = DayOfWeek.Wednesday.ToString(), ShowReminder = true, Time = dailyReminderTime},
                new KataReminder {Day = DayOfWeek.Thursday.ToString(), ShowReminder = true, Time = dailyReminderTime},
                new KataReminder {Day = DayOfWeek.Friday.ToString(), ShowReminder = true, Time = dailyReminderTime},
                new KataReminder {Day = DayOfWeek.Saturday.ToString(), ShowReminder = false},
            };
        }

        public bool PersistSettings()
        {
            var result = true;

            try
            {
                if (_theSettings != null)
                {
                    var newJsonSettings = JsonConvert.SerializeObject(_theSettings);
                    var hasSettingsChanged = HasSettingsChanged(newJsonSettings);
                    if (hasSettingsChanged)
                    {
                        File.WriteAllText(_settingsFileFullPath, newJsonSettings);
                        var splunkLogger = new SplunkLogger(new SplunkAppender(new SplunkSettings()), new KataHelper());
                        splunkLogger.Log(new MonitorEvent { Description = "Settings changed", Logged = DateTime.Now, Settings = _theSettings});
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        private bool HasSettingsChanged(string newJsonSettings)
        {
            if (!File.Exists(_settingsFileFullPath)) return true;
            var settingsFromFile = File.ReadAllText(_settingsFileFullPath);
            return settingsFromFile != newJsonSettings;
        }

        public void ToggleHint()
        {
            _theSettings.ShowHint = !_theSettings.ShowHint;
        }

        public void ToggleIsAlwaysOnTop()
        {
            _theSettings.IsAlwaysOnTop = !_theSettings.IsAlwaysOnTop;
        }

        public bool IsAlwaysOnTopOn()
        {
            return _theSettings.IsAlwaysOnTop;
        }

        public int GetNotificationVisibilityTime()
        {
            return _theSettings.NotificationVisibility;
        }

        public bool PersistSettings(KataraiSettings settings)
        {
            SetSettings(settings);
            return PersistSettings();
        }

        public bool IsShowHintOn()
        {
            return _theSettings.ShowHint;
        }
    }
}
