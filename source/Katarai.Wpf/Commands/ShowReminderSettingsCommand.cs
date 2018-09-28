using System;

namespace Katarai.Wpf.Commands
{
    public interface IShowReminderSettingsCommand : IKataraiCommand
    {
    }

    public class ShowReminderSettingsCommand : IShowReminderSettingsCommand
    {
        public string Description
        {
            get { return "Show Reminder Settings"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var reminderSettings = new Views.ReminderSettings();
            reminderSettings.Show();
        }

        public event EventHandler CanExecuteChanged;
    }
}