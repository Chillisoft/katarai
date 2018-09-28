using System;
using System.Threading;
using Katarai.Runner;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestRunKataAnalysisCommand
    {

        [Test]
        public void Constructor_GivenNullNotifier_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new RunKataAnalysisCommand(null, Substitute.For<ISettingsManager>(),
                Substitute.For<IAnalysisContainer>(), Substitute.For<IAnalysisRunnerFactory>(), Substitute.For<IAnalysisResultProcessor>(), Substitute.For<IKataraiApp>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("playerNotifier", exception.ParamName);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_GivenNullSettingsManager_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new RunKataAnalysisCommand(Substitute.For<IPlayerNotifier>(), null, Substitute.For<IAnalysisContainer>(), Substitute.For<IAnalysisRunnerFactory>(), Substitute.For<IAnalysisResultProcessor>(), Substitute.For<IKataraiApp>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("settingsManager", exception.ParamName);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_GivenNullAnalysisContainer_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new RunKataAnalysisCommand(Substitute.For<IPlayerNotifier>(), Substitute.For<ISettingsManager>(), null, Substitute.For<IAnalysisRunnerFactory>(), Substitute.For<IAnalysisResultProcessor>(), Substitute.For<IKataraiApp>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("analysisContainer", exception.ParamName);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_GivenNullAnalysisRunnerFactory_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new RunKataAnalysisCommand(Substitute.For<IPlayerNotifier>(), Substitute.For<ISettingsManager>(), Substitute.For<IAnalysisContainer>(), null, Substitute.For<IAnalysisResultProcessor>(), Substitute.For<IKataraiApp>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("analysisRunnerFactory", exception.ParamName);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Constructor_GivenNullKataraiAppFactory_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new RunKataAnalysisCommand(Substitute.For<IPlayerNotifier>(), Substitute.For<ISettingsManager>(), Substitute.For<IAnalysisContainer>(), Substitute.For<IAnalysisRunnerFactory>(), Substitute.For<IAnalysisResultProcessor>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("kataraiApp", exception.ParamName);
        }

        [Test]
        public void Execute_WithNullPlayerPath_ShouldNotRunAnalysis()
        {
            //---------------Set up test pack-------------------
            var kataraiSettings = new KataraiSettings {PlayerPath = null};
            var analysisRunnerFactory = Substitute.For<IAnalysisRunnerFactory>();
            var runKataAnalysisCommand = CreateRunKataAnalysisCommand(kataraiSettings: kataraiSettings, analysisRunnerFactory: analysisRunnerFactory);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            runKataAnalysisCommand.Execute();
            //---------------Test Result -----------------------
            analysisRunnerFactory.CreateAnalysisRunner(null, null).DidNotReceiveWithAnyArgs();
        }

        private static RunKataAnalysisCommand CreateRunKataAnalysisCommand(IPlayerNotifier playerNotifier = null, ISettingsManager settingsManager = null, IAnalysisContainer analysisContainer = null, IAnalysisRunnerFactory analysisRunnerFactory = null, IAnalysisResultProcessor analysisResultProcessor = null, KataraiSettings kataraiSettings = null, IKataraiApp kataraiApp = null)
        {
            playerNotifier = playerNotifier ?? Substitute.For<IPlayerNotifier>();
            kataraiSettings = kataraiSettings ?? new KataraiSettings();
            if (settingsManager == null)
            {
                settingsManager = Substitute.For<ISettingsManager>();
                settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            }
            analysisContainer = analysisContainer ?? Substitute.For<IAnalysisContainer>();
            analysisResultProcessor = analysisResultProcessor ?? Substitute.For<IAnalysisResultProcessor>();
            analysisRunnerFactory = analysisRunnerFactory ?? Substitute.For<IAnalysisRunnerFactory>();
            kataraiApp = kataraiApp ?? Substitute.For<IKataraiApp>();
            var runKataAnalysisCommand = new RunKataAnalysisCommand(playerNotifier, settingsManager, analysisContainer, analysisRunnerFactory, analysisResultProcessor, kataraiApp);
            return runKataAnalysisCommand;
        }
    }
}