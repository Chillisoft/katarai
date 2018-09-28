using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using Katarai.Controls;
using Katarai.Runner;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf
{
    public interface IReminderTimer
    {
        void Start();
        void Stop();
        bool IsTimerRunning();
    }

    public class ReminderTimer : IReminderTimer
    {
        private DispatcherTimer _reminderTimer;
        private readonly ISplunkLogger _splunkLogger;
        private readonly ISettingsManager _settingsManager;

        public ReminderTimer(ISplunkLogger splunkLogger, ISettingsManager settingsManager)
        {
            if (splunkLogger == null) throw new ArgumentNullException("splunkLogger");
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            _splunkLogger = splunkLogger;
            _settingsManager = settingsManager;
            SetupTimer();
        }

        public void Start()
        {
            _reminderTimer.Start();
        }

        public void Stop()
        {
            _reminderTimer.Stop();
        }

        public bool IsTimerRunning()
        {
            return _reminderTimer.IsEnabled;
        }

        private void SetupTimer()
        {
            _reminderTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            _reminderTimer.Tick += ((sender, e1) => ShowReminder());
        }

        internal void ShowReminder()
        {
            var todayReminders = GetTodaysReminders();
            if (todayReminders == null || todayReminders.Count == 0) return;
            var todaysReminder = todayReminders.FirstOrDefault();
            if (!MustShowReminder(todaysReminder)) return;
            ShowReminderNotification();
            _splunkLogger.Log(new MonitorEvent { Description = "Kata Reminder", Logged = DateTime.Now });
        }

        private static void ShowReminderNotification()
        {
            var application = Application.Current;
            if (application != null)
            {
                var notifyIcon = (TaskbarIcon) application.FindResource("NotifyIcon");
                if (notifyIcon == null) return;
                notifyIcon.Icon = Icons.Resources.Logo;
                var toast = new Toast(notifyIcon);
                var toastDisplayer = new ToastDisplayer(toast);
                toastDisplayer.ShowNotificationBubble("Reminder", Environment.NewLine + "This is your daily reminder."
                                                                      + Environment.NewLine +
                                                                      "The reminder schedule can be accessed from the tray icon context menu.",
                    7200000);
            }
        }

        private List<KataReminder> GetTodaysReminders()
        {
            var currentSettings = _settingsManager.FetchCurrentSettings();
            if (currentSettings == null) return null;
            var reminders = currentSettings.Reminders;
            if (reminders == null) return null;
            var todayReminders = reminders.Where(reminder => reminder.Day == DateTime.Today.DayOfWeek.ToString() && reminder.ShowReminder)
                    .ToList();
            return todayReminders;
        }

        private bool MustShowReminder(KataReminder todaysReminder)
        {
            if (todaysReminder == null) return false;
            var diff = DateTime.Now.TimeOfDay.TotalSeconds - todaysReminder.Time.TimeOfDay.TotalSeconds;
            return diff >= 0 && diff < 0.999;
        }
    }
}
