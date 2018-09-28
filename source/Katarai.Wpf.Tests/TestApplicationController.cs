using System;
using Caliburn.Micro;
using Katarai.Wpf.Events;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestApplicationController
    {
        [Test]
        public void Construct__WhenNullEventAgregator_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(()=>new ApplicationController(null, windowManager,windowControler,gameMonitor,playerNotifiation,logger,kataFilesMonitor,reminderTimer,monitorTimer));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct__WhenNullWindowManager_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, null, windowControler, gameMonitor, playerNotifiation, logger, kataFilesMonitor, reminderTimer, monitorTimer));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct__WhenNullWindowController_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, windowManager, null, gameMonitor, playerNotifiation, logger, kataFilesMonitor, reminderTimer, monitorTimer));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct__WhenNullGameMonitor_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, windowManager, windowControler, null, playerNotifiation, logger, kataFilesMonitor, reminderTimer, monitorTimer));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct__WhenNullPlayerNotifier_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, windowManager, windowControler, gameMonitor, null, logger, kataFilesMonitor, reminderTimer, monitorTimer));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct__WhenNullLogger_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, windowManager, windowControler, gameMonitor, playerNotifiation, null, kataFilesMonitor, reminderTimer, monitorTimer));

            //---------------Test Result -----------------------
        }

    
        [Test]
        public void Construct__WhenNullKataFilesMonitor_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() =>new ApplicationController(eventAggregator, windowManager, windowControler, gameMonitor, playerNotifiation, logger, null, reminderTimer, monitorTimer));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct__WhenNullReminderTimer_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var monitorTimer = Substitute.For<IMonitorTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, windowManager, windowControler, gameMonitor, playerNotifiation, logger, kataFilesMonitor, null, monitorTimer));

            //---------------Test Result -----------------------

        }

        [Test]
        public void Construct__WhenNullMonitorTimer_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var windowControler = Substitute.For<IWindowController>();
            var gameMonitor = Substitute.For<IGameMonitor>();
            var playerNotifiation = Substitute.For<IPlayerNotifier>();
            var logger = Substitute.For<ISplunkLogger>();
            var kataFilesMonitor = Substitute.For<IKataFilesMonitor>();
            var reminderTimer = Substitute.For<IReminderTimer>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new ApplicationController(eventAggregator, windowManager, windowControler, gameMonitor, playerNotifiation, logger, kataFilesMonitor, reminderTimer, null));

            //---------------Test Result -----------------------

        }

        [Test]
        public void Construct_ShouldInstructEventAggregatorToSubscribe()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();

            //---------------Assert Precondition----------------
            eventAggregator.DidNotReceive().Subscribe(Arg.Any<object>());

            //---------------Execute Test ----------------------
            var sut = Create(eventAggregator: eventAggregator);

            //---------------Test Result -----------------------
            eventAggregator.Received().Subscribe(sut);
        }

        private IApplicationController Create(
            IEventAggregator eventAggregator = null,
            IConvenientWindowManager windowManager = null,
            IWindowController windowController = null,
            IGameMonitor gameMonitor = null,
            IPlayerNotifier playerNotifier = null,
            ISplunkLogger splunkLogger = null,
            IKataFilesMonitor kataFilesMonitor = null,
            IReminderTimer reminderTimer = null,
            IMonitorTimer monitorTimer = null)
        {
            return new ApplicationController(
                eventAggregator ?? Substitute.For<IEventAggregator>(),
                windowManager ?? Substitute.For<IConvenientWindowManager>(),
                windowController ?? Substitute.For<IWindowController>(),
                gameMonitor ?? Substitute.For<IGameMonitor>(),
                playerNotifier ?? Substitute.For<IPlayerNotifier>(),
                splunkLogger ?? Substitute.For<ISplunkLogger>(),
                kataFilesMonitor ?? Substitute.For<IKataFilesMonitor>(),
                reminderTimer ?? Substitute.For<IReminderTimer>(),
                monitorTimer ?? Substitute.For<IMonitorTimer>()
            );
        }


        [Test]
        public void Type_ShouldImplement_IApplicationController()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (ApplicationController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IApplicationController>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void InterfaceType_ShouldImplement_IHandleExitApplicationEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IApplicationController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<ExitApplicationEvent>>();

            //---------------Test Result -----------------------
        }




    }
}
