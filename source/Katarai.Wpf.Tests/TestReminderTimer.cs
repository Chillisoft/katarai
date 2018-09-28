using System;
using System.Collections.Generic;
using Katarai.Runner;
using Katarai.Wpf.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestReminderTimer
    {
        [Test]
        public void Constructor_GivenNullLogger_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>  new ReminderTimer(null, Substitute.For<ISettingsManager>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("splunkLogger", exception.ParamName);
        }

        [Test]
        public void Constructor_ShouldCreateTimer()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var reminderTimer = new ReminderTimer(Substitute.For<ISplunkLogger>(), Substitute.For<ISettingsManager>());
            //---------------Test Result -----------------------
            Assert.IsFalse(reminderTimer.IsTimerRunning());
        }

        [Test]
        public void Start_ShouldStartTimer()
        {
            //---------------Set up test pack-------------------
            var reminderTimer = CreateReminderTimer();
            //---------------Assert Precondition----------------
            Assert.IsFalse(reminderTimer.IsTimerRunning());
            //---------------Execute Test ----------------------
            reminderTimer.Start();
            //---------------Test Result -----------------------
            Assert.IsTrue(reminderTimer.IsTimerRunning());
        }

        [Test]
        public void Stop_ShouldStopTimer()
        {
            //---------------Set up test pack-------------------
            var reminderTimer = CreateReminderTimer();
            reminderTimer.Start();
            //---------------Assert Precondition----------------
            Assert.IsTrue(reminderTimer.IsTimerRunning());
            //---------------Execute Test ----------------------
            reminderTimer.Stop();
            //---------------Test Result -----------------------
            Assert.IsFalse(reminderTimer.IsTimerRunning());
        }

        [Test]
        public void ShowReminder_WhenRemindersIsNull_ShouldNotLogMonitorEvent()
        {
            //---------------Set up test pack-------------------
            var splunkLogger = Substitute.For<ISplunkLogger>();
            var settingsManager = Substitute.For<ISettingsManager>();
            var reminderTimer = new ReminderTimer(splunkLogger, settingsManager);
            settingsManager.FetchCurrentSettings().Returns(new  KataraiSettings());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            reminderTimer.ShowReminder();
            //---------------Test Result -----------------------
            splunkLogger.DidNotReceive().Log(Arg.Any<MonitorEvent>());
        }

        [Test]
        public void ShowReminder_WhenZeroReminders_ShouldNotLogMonitorEvent()
        {
            //---------------Set up test pack-------------------
            var splunkLogger = Substitute.For<ISplunkLogger>();
            var settingsManager = Substitute.For<ISettingsManager>();
            var reminderTimer = new ReminderTimer(splunkLogger, settingsManager);
            settingsManager.FetchCurrentSettings().Returns(new  KataraiSettings{Reminders = new List<KataReminder>()});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            reminderTimer.ShowReminder();
            //---------------Test Result -----------------------
            splunkLogger.DidNotReceive().Log(Arg.Any<MonitorEvent>());
        }

        [Test]
        public void ShowReminder_WhenReminderForTodayNotSet_ShouldNotLogMonitorEvent()
        {
            //---------------Set up test pack-------------------
            var splunkLogger = Substitute.For<ISplunkLogger>();
            var settingsManager = Substitute.For<ISettingsManager>();
            var reminderTimer = new ReminderTimer(splunkLogger, settingsManager);
            var kataReminders = new List<KataReminder>
            {
                new KataReminder {Day = DateTime.Now.AddDays(1).DayOfWeek.ToString(), ShowReminder = true, Time = DateTime.Now}
            };
            settingsManager.FetchCurrentSettings().Returns(new  KataraiSettings{Reminders = kataReminders});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            reminderTimer.ShowReminder();
            //---------------Test Result -----------------------
            splunkLogger.DidNotReceive().Log(Arg.Any<MonitorEvent>());
        }

        [Test]
        public void ShowReminder_WhenShowReminderForTodayIsFalse_ShouldNotLogMonitorEvent()
        {
            //---------------Set up test pack-------------------
            var splunkLogger = Substitute.For<ISplunkLogger>();
            var settingsManager = Substitute.For<ISettingsManager>();
            var reminderTimer = new ReminderTimer(splunkLogger, settingsManager);
            var kataReminders = new List<KataReminder>
            {
                new KataReminder {Day = DateTime.Now.DayOfWeek.ToString(), ShowReminder = false, Time = DateTime.Now}
            };
            settingsManager.FetchCurrentSettings().Returns(new  KataraiSettings{Reminders = kataReminders});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            reminderTimer.ShowReminder();
            //---------------Test Result -----------------------
            splunkLogger.DidNotReceive().Log(Arg.Any<MonitorEvent>());
        }

        [Test]
        public void ShowReminder_WhenReminderForToday_ShouldLogMonitorEvent()
        {
            //---------------Set up test pack-------------------
            var splunkLogger = Substitute.For<ISplunkLogger>();
            var settingsManager = Substitute.For<ISettingsManager>();
            var reminderTimer = new ReminderTimer(splunkLogger, settingsManager);
            var kataReminders = new List<KataReminder>
            {
                new KataReminder {Day = DateTime.Now.DayOfWeek.ToString(), ShowReminder = true, Time = DateTime.Now}
            };
            settingsManager.FetchCurrentSettings().Returns(new  KataraiSettings{Reminders = kataReminders});
            //---------------Assert Precondition----------------
            splunkLogger.DidNotReceive().Log(Arg.Any<MonitorEvent>());
            //---------------Execute Test ----------------------
            reminderTimer.ShowReminder();
            //---------------Test Result -----------------------
            splunkLogger.Received(1).Log(Arg.Any<MonitorEvent>());
        }

        private ReminderTimer CreateReminderTimer()
        {
            return CreateReminderTimer(Substitute.For<ISplunkLogger>(), Substitute.For<ISettingsManager>());
        }

        private ReminderTimer CreateReminderTimer(ISplunkLogger splunkLogger, ISettingsManager settingsManager)
        {
            return new ReminderTimer(splunkLogger, settingsManager);
        }

    }
}