using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using Katarai.Runner;
using Katarai.Utils;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Events;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using PeanutButter.TestUtils.Generic;
using KataraiApp = Katarai.Wpf.Monitor.KataraiApp;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestMainWindowViewModel
    {
        [TestCase("attemptRepository", typeof(IKataAttemptRepository))]
        [TestCase("generateKataCommand", typeof(IGenerateAndLaunchKataCommand))]
        [TestCase("sendFeedbackCommand", typeof(ISendFeedbackCommand))]
        [TestCase("showMainWindowCommand", typeof(IShowMainWindowCommand))]
        [TestCase("hideMainWindowCommand", typeof(IHideMainWindowCommand))]
        [TestCase("exitApplicationCommand", typeof(IExitApplicationCommand))]
        [TestCase("openKataSolutionCommand", typeof(IOpenKataSolutionCommand))]
        [TestCase("showReminderSettingsCommand", typeof(IShowReminderSettingsCommand))]
        [TestCase("showAttemptsCommand", typeof(IShowAttemptsCommand))]
        [TestCase("showAttemptsPerWeekCommand", typeof(IShowAttemptsPerWeekCommand))]
        [TestCase("showCompletedKatasCommand", typeof(IShowCompletedKatasCommand))]
        [TestCase("abandonAttemptCommand", typeof(IAbandonAttemptCommand))]
        [TestCase("showAboutWindowCommand", typeof(IShowAboutWindowCommand))]
        public void Constructor_GivenNullRepository(string parameterName, Type parameterType)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ConstructorTestUtils.ShouldExpectNonNullParameterFor<MainWindowViewModel>(
                                                                        parameterName, parameterType);
            //---------------Test Result -----------------------
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_ShouldInitializeKatas()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IKataAttemptRepository>();
            var kataInfos = new List<IKataAttempt>();
            repository.GetKataInfos().Returns(kataInfos);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var viewModel = Create(attemptRepository:repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(viewModel.Katas);
            Assert.AreSame(kataInfos, viewModel.Katas);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_ShouldSetIsAlwaysOnTopToSettingsValue()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(new KataraiSettings());
            var isAlwaysOnTopOn = settingsManager.IsAlwaysOnTopOn();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager:settingsManager);
            //---------------Test Result -----------------------
            Assert.AreEqual(viewModel.IsAlwaysOnTop, isAlwaysOnTopOn);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_ShouldSetNotificationVisibilityToSettingsValue()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(new KataraiSettings());
            settingsManager.GetNotificationVisibilityTime().Returns(10);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager:settingsManager);
            //---------------Test Result -----------------------
            Assert.AreEqual(10, viewModel.NotificationVisibilityTimeSeconds);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void KataPath_ShouldReturnMasterSolutionBinaryPath()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IKataAttemptRepository>();
            var kataInfos = new List<IKataAttempt>();
            repository.GetKataInfos().Returns(kataInfos);
            var kataInfo = new KataAttempt(Substitute.For<IFileSystem>()) {Name = "Kata", Location = "Location"};
            const string expectedKataPath = @"Location\Kata\Master.dll";
            repository.GetMasterSolutionAssemblyPath(kataInfo).Returns(expectedKataPath);
            var viewModel = Create(attemptRepository: repository);
            viewModel.SelectedKataAttempt = kataInfo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var kataPath = viewModel.KataPath;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedKataPath, kataPath);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void PlayerPath_ShouldReturnPlayerSolutionBinaryPath()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<IKataAttemptRepository>();
            var kataInfos = new List<IKataAttempt>();
            repository.GetKataInfos().Returns(kataInfos);
            var kataInfo = new KataAttempt(Substitute.For<IFileSystem>()) {Name = "Kata", Location = "Location"};
            const string expectedKataPath = @"Location\Kata\Player.dll";
            repository.GetPlayerSolutionAssemblyPath(kataInfo).Returns(expectedKataPath);
            var viewModel = Create(attemptRepository: repository);
            viewModel.SelectedKataAttempt = kataInfo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var playerPath = viewModel.PlayerPath;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedKataPath, playerPath);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void ShouldShowHint_WhenSet_ShouldInvokeOnPropertyChanged()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(new KataraiSettings());
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager:settingsManager);
            bool isFired = false;
            viewModel.PropertyChanged += (sender, args) => { isFired = true; };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.ShouldShowHint = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void ShouldShowHint_WhenSet_ShouldPersistSettings()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataraiSettings = new KataraiSettings();
            settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager:settingsManager);
            settingsManager.ClearReceivedCalls();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.ShouldShowHint = true;
            //---------------Test Result -----------------------
            settingsManager.Received().FetchCurrentSettings();
            settingsManager.Received().PersistSettings(kataraiSettings);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void NotificationVisibilityTimeSeconds_WhenSet_ShouldInvokeOnPropertyChanged()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(new KataraiSettings());
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager:settingsManager);
            bool isFired = false;
            viewModel.PropertyChanged += (sender, args) => { isFired = true; };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.NotificationVisibilityTimeSeconds = 10;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void NotificationVisibilityTimeSeconds_WhenSet_ShouldPersistSettings()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataraiSettings = new KataraiSettings();
            settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager:settingsManager);
            settingsManager.ClearReceivedCalls();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.NotificationVisibilityTimeSeconds = 10;
            //---------------Test Result -----------------------
            settingsManager.Received().FetchCurrentSettings();
            settingsManager.Received().PersistSettings(kataraiSettings);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void IsAlwaysOnTop_WhenSet_ShouldInvokeOnPropertyChanged()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(new KataraiSettings());
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager: settingsManager);
            bool isFired = false;
            viewModel.PropertyChanged += (sender, args) => { isFired = true; };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.IsAlwaysOnTop = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void IsAlwaysOnTop_WhenSet_ShouldPersistSettings()
        {
            //---------------Set up test pack-------------------
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataraiSettings = new KataraiSettings();
            settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            var viewModel = Create(attemptRepository: Substitute.For<IKataAttemptRepository>(), settingsManager: settingsManager);
            settingsManager.ClearReceivedCalls();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.IsAlwaysOnTop = true;
            //---------------Test Result -----------------------
            settingsManager.Received().FetchCurrentSettings();
            settingsManager.Received().PersistSettings(kataraiSettings);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void ViewModel_WhenNewKataAttempt_ShouldSendNotification()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            var command = Substitute.For<IGenerateAndLaunchKataCommand>();

            var playerNotifier = Substitute.For<IPlayerNotifier>();
            Create(generateAndLaunchKataCommand:command, playerNotifier:playerNotifier);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            command.KataAttemptCreated += Raise.EventWith(command, new KataAttemptEventArgs(kataAttempt));
            //---------------Test Result -----------------------
            var expectedMessage = string.Format("{1}{0}{0}{2}{0}{0}{0}{3}{0}{0}", Environment.NewLine,
                "Please note the following:", "Do not use test cases", 
                "Please READ the instructions as they are slightly different from what you may have previously encountered.");
            playerNotifier.Received()
                .DisplayMessage("Starting String Calculator Kata...", expectedMessage);
        }

        [Test]
        public void InterfaceType_ShouldHandle_DisplayMessageEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IMainWindowViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<DisplayFeedbackEvent>>();

            //---------------Test Result -----------------------
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Handle_DisplayLogMessage_ShouldAddLogMessageTo()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var expected = RandomValueGen.GetRandomString();

            //---------------Assert Precondition----------------
            CollectionAssert.IsEmpty(sut.FeedbackItems);

            //---------------Execute Test ----------------------
            sut.Handle(new DisplayFeedbackEvent(expected));

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, sut.FeedbackItems.Single());
        }

        [Test]
        public void InterfaceType_ShouldHandle_KataAttemptCreatedEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IMainWindowViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<KataAttemptCreatedEvent>>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void InterfaceType_ShouldHandle_KataAttemptAbandonedEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IMainWindowViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<KataAttemptAbandonedEvent>>();

            //---------------Test Result -----------------------
        }





        private static MainWindowViewModel Create(
            IEventAggregator eventAggregator = null,
            IKataAttemptRepository attemptRepository = null,
            IGenerateAndLaunchKataCommand generateAndLaunchKataCommand = null,
            ISettingsManager settingsManager = null,
            IPlayerNotifier playerNotifier = null,
            IKataraiApp kataraiApp = null)
        {
            settingsManager = settingsManager ?? CreateSubstituteSettingsManager();
            var viewModel = new MainWindowViewModel(
                eventAggregator ?? Substitute.For<IEventAggregator>(),
                kataraiApp ?? Substitute.For<IKataraiApp>(),
                playerNotifier ?? Substitute.For<IPlayerNotifier>(),
                attemptRepository ?? Substitute.For<IKataAttemptRepository>(),
                generateAndLaunchKataCommand ?? Substitute.For<IGenerateAndLaunchKataCommand>(),
                Substitute.For<ISendFeedbackCommand>(),
                Substitute.For<IShowMainWindowCommand>(),
                Substitute.For<IHideMainWindowCommand>(),
                Substitute.For<IExitApplicationCommand>(),
                settingsManager,
                Substitute.For<IOpenKataSolutionCommand>(),
                Substitute.For<IShowReminderSettingsCommand>(),
                Substitute.For<IShowAttemptsCommand>(),
                Substitute.For<IShowCompletedKatasCommand>(),
                Substitute.For<IShowAttemptsPerWeekCommand>(),
                Substitute.For<IAbandonAttemptCommand>(),
                Substitute.For<IShowAboutWindowCommand>());
            viewModel.KataraiApp = new KataraiApp(settingsManager);
            return viewModel;
        }

        private static ISettingsManager CreateSubstituteSettingsManager()
        {
            var result = Substitute.For<ISettingsManager>();
            result.FetchCurrentSettings().Returns(new KataraiSettings());
            return result;
        }

        private KataAttempt CreateKataAttempt()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.FileExists(Arg.Any<string>()).Returns(true);
            fileSystem.ReadAllText(Arg.Any<string>())
                .Returns(@"/*directory structure must remain as MasterSolution, PlayerSolution*/
                {
                ""masterSolutionAssembly"":""Katarai.KataData.StringCalculator.dll"",
                ""playerSolutionName"":""PlayerStringKata.sln"",
                ""playerSolutionAssembly"":""PlayerStringKata.dll"",
                ""kataName"":""String Calculator"",
                ""instructions"":{
		                ""header"":""Please note the following:"",
		                ""content"":[
			                ""Do not use test cases"",
			                ""Please READ the instructions as they are slightly different from what you may have previously encountered.""
		                ]
	                }
                }");
            fileSystem.Combine(Arg.Any<string>(), Arg.Any<string>()).Returns("sample/location");
            return new KataAttempt(fileSystem)
            {
                Location = string.Empty
            };
        }
    }
}