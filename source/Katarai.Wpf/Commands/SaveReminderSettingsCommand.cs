using System;
using System.Collections.Generic;
using Katarai.Runner;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.Commands
{
    public interface ISaveReminderSettingsCommand: IKataraiCommand
    {
        event EventHandler Saved;
    }

    public class SaveReminderSettingsCommand : ISaveReminderSettingsCommand
    {
        private readonly ISettingsManager _settingsManager;

        public string Description
        {
            get { return "Save Reminder Settings"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public SaveReminderSettingsCommand(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public void Execute(object parameter)
        {
            var kataReminders = parameter as List<KataReminder>;
            if (kataReminders == null) return;
            var currentSettings = _settingsManager.FetchCurrentSettings();
            currentSettings.Reminders = kataReminders;
            _settingsManager.PersistSettings();
            if (Saved != null)
            {
                Saved(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler Saved;
    }
}