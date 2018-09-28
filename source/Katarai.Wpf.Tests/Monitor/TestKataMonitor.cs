using System;
using System.Threading;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestKataMonitor
    {
        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_ShouldNotThrowError()
        {
            //---------------Set up test pack-------------------
            var analysisContainer = Substitute.For<IAnalysisContainer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var kataMonitor = new KataFilesMonitor(Substitute.For<ISettingsManager>(), Substitute.For<IRunKataAnalysisCommand>()); 
            //---------------Test Result -----------------------
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_GivenNullSettingsManager_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new KataFilesMonitor(null, Substitute.For<IRunKataAnalysisCommand>());
            });
            //---------------Test Result -----------------------
            Assert.AreEqual("settingsManager", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullNotifier_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new KataFilesMonitor(Substitute.For<ISettingsManager>(), null);
            });
            //---------------Test Result -----------------------
            Assert.AreEqual("runKataAnalysisRunKataAnalysisCommand", exception.ParamName);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Start_ShouldCallLoadSettingsOnSettingsManager()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataMonitor = CreateKataMonitor(settingsManager: settingsManager);
            //---------------Assert Precondition----------------
            settingsManager.DidNotReceive().LoadSettings();
            //---------------Execute Test ----------------------
            kataMonitor.Start();
            //---------------Test Result -----------------------
            settingsManager.Received(1).LoadSettings();
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Start_ShouldCreateFileWatcherTimer()
        {
            //---------------Set up test pack-------------------
            var kataMonitor = CreateKataMonitor();
            //---------------Assert Precondition----------------
            Assert.IsNull(kataMonitor.FileWatcherTimer);
            //---------------Execute Test ----------------------
            kataMonitor.Start();
            //---------------Test Result -----------------------
            Assert.IsNotNull(kataMonitor.FileWatcherTimer);
            var expected = new TimeSpan(0, 0, 1);
            Assert.AreEqual(expected, kataMonitor.FileWatcherTimer.Interval);
        }

        [Ignore("Because nunit 3.x needs this populated")]
        [Apartment(ApartmentState.STA)]
        [Test]
        public void Start_ShouldCallGenerateFileMonitorOnAnalysisRunnerFactory()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var command = Substitute.For<IRunKataAnalysisCommand>();
            var kataMonitor = CreateKataMonitor(settingsManager: settingsManager, runKataAnalysisCommand: command);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            kataMonitor.Start();
            //---------------Test Result -----------------------
            var aTimer = new System.Timers.Timer(2000);
            aTimer.Elapsed += (sender, args) => command.Received().Execute();
        }


        [Apartment(ApartmentState.STA)]
        [Test]
        public void Stop_ShouldCallPersistSettingsOnSettingsManager()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataMonitor = CreateKataMonitor(settingsManager: settingsManager);
            //---------------Assert Precondition----------------
            settingsManager.DidNotReceive().PersistSettings();
            //---------------Execute Test ----------------------
            kataMonitor.Stop();
            //---------------Test Result -----------------------
            settingsManager.Received(1).PersistSettings();
        }


        [Apartment(ApartmentState.STA)]
        [Test]
        public void Stop_ShouldStopFileWatcherTimer()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataMonitor = CreateKataMonitor(settingsManager: settingsManager);
            kataMonitor.Start();
            //---------------Assert Precondition----------------
            Assert.IsTrue(kataMonitor.FileWatcherTimer.IsEnabled);
            //---------------Execute Test ----------------------
            kataMonitor.Stop();
            //---------------Test Result -----------------------
            Assert.IsFalse(kataMonitor.FileWatcherTimer.IsEnabled);
        }

        private KataFilesMonitor CreateKataMonitor(ISettingsManager settingsManager = null, IRunKataAnalysisCommand runKataAnalysisCommand = null)
        {
            if (settingsManager == null) settingsManager = Substitute.For<ISettingsManager>();
            runKataAnalysisCommand = runKataAnalysisCommand ?? Substitute.For<IRunKataAnalysisCommand>();
            return new KataFilesMonitor(settingsManager,runKataAnalysisCommand);
        }
    }
}