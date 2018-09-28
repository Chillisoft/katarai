using System;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Settings;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestReminderSettingsViewModel
    {
        [Test]
        public void Constructor_GivenNullSaveReminderSettingsCommand_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new ReminderSettingsViewModel(null, Substitute.For<ISettingsManager>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("saveReminderSettingsCommand", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullSettingsManager_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new ReminderSettingsViewModel(Substitute.For<ISaveReminderSettingsCommand>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("settingsManager", exception.ParamName);
        }

        [Test]
        public void ViewModel_WhenSaved_ShouldSendNotification()
        {
            //---------------Set up test pack-------------------
            var command = Substitute.For<ISaveReminderSettingsCommand>();
            var wasCalled = false;
            var viewModel = CreateViewModel(command);
            viewModel.RequestClose += (sender, args) => { wasCalled = true; };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            command.Saved += Raise.EventWith(command, EventArgs.Empty);
            //---------------Test Result -----------------------
            Assert.IsTrue(wasCalled);

        }

        private ReminderSettingsViewModel CreateViewModel(ISaveReminderSettingsCommand saveReminderSettingsCommand = null, ISettingsManager settingsManager = null)
        {
            saveReminderSettingsCommand = saveReminderSettingsCommand ?? Substitute.For<ISaveReminderSettingsCommand>();
            settingsManager = settingsManager ?? Substitute.For<ISettingsManager>();
            return new ReminderSettingsViewModel(saveReminderSettingsCommand, settingsManager);
        }

    }
}