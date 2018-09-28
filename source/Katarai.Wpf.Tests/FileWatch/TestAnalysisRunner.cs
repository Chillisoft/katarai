using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Katarai.Runner;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.FileWatch;
using Katarai.Wpf.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.FileWatch
{
    [TestFixture]
    public class TestAnalysisRunner
    {
        [Test]
        public void Constructor_GivenNullSettingManageer_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AnalysisRunner(null, Substitute.For<IAnalysisContainer>(), new FileHashesManager()));
            //---------------Test Result -----------------------
            Assert.AreEqual("settingsManager", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullAnalysisContainer_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AnalysisRunner(Substitute.For<ISettingsManager>(), null, new FileHashesManager()));
            //---------------Test Result -----------------------
            Assert.AreEqual("analysisContainer", exception.ParamName);
        }

        [Test]
        public void Run_GivenKataPathIsNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var analysisRunner = CreateAnalysisRunner(null, Path.GetTempPath());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string errorMessage;
            var result = analysisRunner.Run(out errorMessage);
            //---------------Test Result -----------------------
            Assert.IsNull(result);
            Assert.AreEqual("Could not locate Player or Kata Assembly", errorMessage);
        }

        [Test]
        public void Run_GivenPlayerPathIsNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var analysisRunner = CreateAnalysisRunner(Path.GetTempPath(), null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string errorMessage;
            var result = analysisRunner.Run(out errorMessage);
            //---------------Test Result -----------------------
            Assert.IsNull(result);
            Assert.AreEqual("Could not locate Player or Kata Assembly", errorMessage);
        }
        [Test]
        public void Run_GivenKataPathDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var analysisRunner = CreateAnalysisRunner(Guid.NewGuid().ToString(), Path.GetTempPath());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string errorMessage;
            var result = analysisRunner.Run(out errorMessage);
            //---------------Test Result -----------------------
            Assert.IsNull(result);
            Assert.AreEqual("Could not locate Player or Kata Assembly", errorMessage);
        }

        [Test]
        public void Run_GivenPlayerPathDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var analysisRunner = CreateAnalysisRunner(Path.GetTempPath(), Guid.NewGuid().ToString());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string errorMessage;
            var result = analysisRunner.Run(out errorMessage);
            //---------------Test Result -----------------------
            Assert.IsNull(result);
            Assert.AreEqual("Could not locate Player or Kata Assembly", errorMessage);
        }

        [Test]
        public void Run_GivenThatThereAreNoFileChanges_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var kataPath = Path.GetTempFileName();
            var playerPath = Path.GetTempFileName();
            var kataraiSettings = CreateKataraiSettings(kataPath, playerPath);
            var analysisRunner = CreateAnalysisRunner(kataraiSettings);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string errorMessage;
            var result = analysisRunner.Run(out errorMessage);
            //---------------Test Result -----------------------
            Assert.IsEmpty(errorMessage);
            Assert.IsNull(result);
        }

        [Test]
        public void Run_GivenThatThereAreFileChanges_ShouldReturnResult()
        {
            //---------------Set up test pack-------------------
            var kataPath = Path.GetTempFileName();
            var playerPath = Path.GetTempFileName();
            var kataraiSettings = CreateKataraiSettings(kataPath, playerPath);
            var fileHashes = CreateFileHashes(kataraiSettings);
            var analysisRunner = CreateAnalysisRunner(kataraiSettings, fileHashes);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string errorMessage;
            var result = analysisRunner.Run(out errorMessage);
            //---------------Test Result -----------------------
            Assert.IsEmpty(errorMessage);
            Assert.IsNotNull(result);
        }

        private static IFileHashesManager CreateFileHashes(KataraiSettings kataraiSettings)
        {
            var fileHashes = Substitute.For<IFileHashesManager>();
            fileHashes.GetFileHashes(kataraiSettings).Returns(CreateFileMonitorInformation());
            return fileHashes;
        }

        private static FileMonitorInformation CreateFileMonitorInformation()
        {
            var fileMonitorInformation = new FileMonitorInformation
            {
                KataHash = Guid.NewGuid().ToString(),
                OldKataHash = Guid.NewGuid().ToString(),
                OldPlayerHash = Guid.NewGuid().ToString(),
                PlayerHash = Guid.NewGuid().ToString()
            };
            return fileMonitorInformation;
        }

        private static KataraiSettings CreateKataraiSettings(string kataPath, string playerPath)
        {
            var kataraiSettings = new KataraiSettings
            {
                KataPath = kataPath,
                PlayerPath = playerPath,
                KataHash = CreateFileHash(kataPath),
                PlayerHash = CreateFileHash(playerPath)
            };
            return kataraiSettings;
        }

        private static string CreateFileHash(string kataPath)
        {
            return File.GetLastWriteTime(kataPath).ToLongTimeString().GetHashCode().ToString(CultureInfo.InvariantCulture);
        }

        private static AnalysisRunner CreateAnalysisRunner(string kataPath, string playerPath)
        {
            var settingsManager = Substitute.For<ISettingsManager>();
            var kataraiSettings = new KataraiSettings { KataPath = kataPath, PlayerPath = playerPath };
            settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            var analysisContainer = Substitute.For<IAnalysisContainer>();
            return new AnalysisRunner(settingsManager, analysisContainer, new FileHashesManager());
        }

        private static AnalysisRunner CreateAnalysisRunner(KataraiSettings kataraiSettings)
        {
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            var analysisContainer = Substitute.For<IAnalysisContainer>();
            return new AnalysisRunner(settingsManager, analysisContainer, new FileHashesManager());
        }

        private static AnalysisRunner CreateAnalysisRunner(KataraiSettings kataraiSettings, IFileHashesManager fileHashesManager)
        {
            var settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.FetchCurrentSettings().Returns(kataraiSettings);
            var analysisContainer = Substitute.For<IAnalysisContainer>();
            IEnumerable<PlayerFeedback> messages = new List<PlayerFeedback>
            {
                Capacity = 1
            };
            analysisContainer.Execute(kataraiSettings).Returns(new Result(1, 1, messages.FirstOrDefault()));
            return new AnalysisRunner(settingsManager, analysisContainer, fileHashesManager);
        }
    }
}