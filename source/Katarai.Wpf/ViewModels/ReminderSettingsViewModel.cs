using System;
using System.Collections.Generic;
using System.Linq;
using Katarai.Runner;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.ViewModels
{
    public interface IReminderSettingsViewModel
    {
        event EventHandler RequestClose;
        ICommandWithLogging SaveReminderSettingsCommand { get; }
        List<KataReminder> Reminders { get; }
        DateTime SundayTime { get; set; }
        DateTime MondayTime { get; set; }
        DateTime TuesdayTime { get; set; }
        DateTime WednesdayTime { get; set; }
        DateTime ThursdayTime { get; set; }
        DateTime FridayTime { get; set; }
        DateTime SaturdayTime { get; set; }
        bool SundayShowReminder { get; set; }
        bool MondayShowReminder { get; set; }
        bool TuesdayShowReminder { get; set; }
        bool WednesdayShowReminder { get; set; }
        bool ThursdayShowReminder { get; set; }
        bool FridayShowReminder { get; set; }
        bool SaturdayShowReminder { get; set; }
    }

    public class ReminderSettingsViewModel : IReminderSettingsViewModel
    {
        private DateTime _sundayTime;
        private DateTime _mondayTime;
        private DateTime _tuesdayTime;
        private DateTime _wednesdayTime;
        private DateTime _thursdayTime;
        private DateTime _fridayTime;
        private DateTime _saturdayTime;
        private bool _sundayShowReminder;
        private bool _mondayShowReminder;
        private bool _tuesdayShowReminder;
        private bool _wednesdayShowReminder;
        private bool _thursdayShowReminder;
        private bool _fridayShowReminder;
        private bool _saturdayShowReminder;
        public event EventHandler RequestClose;

        public ICommandWithLogging SaveReminderSettingsCommand { get; private set; }
        public List<KataReminder> Reminders { get; private set; }

        public ReminderSettingsViewModel(ISaveReminderSettingsCommand saveReminderSettingsCommand, ISettingsManager settingsManager)
        {
            if (saveReminderSettingsCommand == null) throw new ArgumentNullException("saveReminderSettingsCommand");
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            SaveReminderSettingsCommand = new CommandWithLogging(saveReminderSettingsCommand);
            SetupSavedReminderSettingsCommand();
            var currentSettings = settingsManager.FetchCurrentSettings();
            SetReminders(currentSettings);
            SetValuesFromSetting();
        }

        private void SetupSavedReminderSettingsCommand()
        {
            var saveReminderSettingsCommand = SaveReminderSettingsCommand.WrappedCommand as ISaveReminderSettingsCommand;
            if (saveReminderSettingsCommand == null) return;
            saveReminderSettingsCommand.Saved += (sender, args) =>
            {
                if (this.RequestClose != null)
                {
                    this.RequestClose(this, EventArgs.Empty);
                }
            };
        }

        private void SetReminders(KataraiSettings currentSettings)
        {
            if (currentSettings == null || currentSettings.Reminders == null)
            {
                Reminders = new List<KataReminder>();
            }
            else
            {
                Reminders = currentSettings.Reminders;
            }
        }

        private void SetValuesFromSetting()
        {
            SetValueFromSetting(DayOfWeek.Sunday, ref _sundayTime, ref _sundayShowReminder);
            SetValueFromSetting(DayOfWeek.Monday, ref _mondayTime, ref _mondayShowReminder);
            SetValueFromSetting(DayOfWeek.Tuesday, ref _tuesdayTime, ref _tuesdayShowReminder);
            SetValueFromSetting(DayOfWeek.Wednesday, ref _wednesdayTime, ref _wednesdayShowReminder);
            SetValueFromSetting(DayOfWeek.Thursday, ref _thursdayTime, ref _thursdayShowReminder);
            SetValueFromSetting(DayOfWeek.Friday, ref _fridayTime, ref _fridayShowReminder);
            SetValueFromSetting(DayOfWeek.Saturday, ref _saturdayTime, ref _saturdayShowReminder);
        }

        private void SetValueFromSetting(DayOfWeek dayOfWeek, ref DateTime reminderTime, ref bool showReminder)
        {
            var dayReminder = GetReminderDaySetting(dayOfWeek);
            SetTimeValue(dayReminder, ref reminderTime);
            SetShowReminderValue(dayReminder, ref showReminder);
        }

        private void SetShowReminderValue(KataReminder dayReminder, ref bool showReminder)
        {
            if (dayReminder == null) return;
            showReminder = dayReminder.ShowReminder;
        }

        private void SetTimeValue(KataReminder dayReminder, ref DateTime reminderTime)
        {
            if (dayReminder == null) return;
            if (dayReminder.Time.TimeOfDay.TotalSeconds > 0)
            {
                reminderTime = dayReminder.Time;
            }
        }

        private KataReminder GetReminderDaySetting(DayOfWeek dayOfWeek)
        {
            var reminderDaySetting = Reminders.FirstOrDefault(reminder => reminder.Day == dayOfWeek.ToString());
            if (reminderDaySetting == null)
            {
                reminderDaySetting = new KataReminder{Day = dayOfWeek.ToString()};
            }
            return reminderDaySetting;
        }

        public DateTime SundayTime
        {
            get { return _sundayTime; }
            set
            {
                _sundayTime = value;
                UpdateReminderTime(DayOfWeek.Sunday, value);
            }
        }

        public DateTime MondayTime
        {
            get { return _mondayTime; }
            set
            {
                _mondayTime = value;
                UpdateReminderTime(DayOfWeek.Monday, value);
            }
        }

        public DateTime TuesdayTime
        {
            get { return _tuesdayTime; }
            set
            {
                _tuesdayTime = value;
                UpdateReminderTime(DayOfWeek.Tuesday, value);
            }
        }

        public DateTime WednesdayTime
        {
            get { return _wednesdayTime; }
            set
            {
                _wednesdayTime = value;
                UpdateReminderTime(DayOfWeek.Wednesday, value);
            }
        }

        public DateTime ThursdayTime
        {
            get { return _thursdayTime; }
            set
            {
                _thursdayTime = value;
                UpdateReminderTime(DayOfWeek.Thursday, value);
            }
        }

        public DateTime FridayTime
        {
            get { return _fridayTime; }
            set
            {
                _fridayTime = value;
                UpdateReminderTime(DayOfWeek.Friday, value);
            }
        }

        public DateTime SaturdayTime
        {
            get { return _saturdayTime; }
            set
            {
                _saturdayTime = value;
                UpdateReminderTime(DayOfWeek.Saturday, value);
            }
        }

        public bool SundayShowReminder
        {
            get { return _sundayShowReminder; }
            set
            {
                _sundayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Sunday, value);
            }
        }

        public bool MondayShowReminder
        {
            get { return _mondayShowReminder; }
            set
            {
                _mondayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Monday, value);
            }
        }

        public bool TuesdayShowReminder
        {
            get { return _tuesdayShowReminder; }
            set
            {
                _tuesdayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Tuesday, value);
            }
        }

        public bool WednesdayShowReminder
        {
            get { return _wednesdayShowReminder; }
            set
            {
                _wednesdayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Wednesday, value);
            }
        }

        public bool ThursdayShowReminder
        {
            get { return _thursdayShowReminder; }
            set
            {
                _thursdayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Thursday, value);
            }
        }

        public bool FridayShowReminder
        {
            get { return _fridayShowReminder; }
            set
            {
                _fridayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Friday, value);
            }
        }

        public bool SaturdayShowReminder
        {
            get { return _saturdayShowReminder; }
            set
            {
                _saturdayShowReminder = value;
                UpdateShowReminder(DayOfWeek.Saturday, value);
            }
        }

        private void UpdateReminderTime(DayOfWeek dayOfWeek, DateTime value)
        {
            var reminderDaySetting = GetReminderDaySetting(dayOfWeek);
            reminderDaySetting.Time = value;
        }

        private void UpdateShowReminder(DayOfWeek dayOfWeek, bool showReminder)
        {
            var reminderDaySetting = GetReminderDaySetting(dayOfWeek);
            reminderDaySetting.ShowReminder = showReminder;
        }
        
    }

}
