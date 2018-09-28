using System;
using System.IO;
using Caliburn.Micro;
using Katarai.Runner;
using Katarai.Wpf.Events;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Tests.Monitor;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests.PackagedKata
{
    [TestFixture]
    public class TestKataArchive
    {
        [Test]
        public void GlobalArchiveRoot_ReturnsValidKataArchiveDirectory()
        {
            //---------------Set up test pack-------------------
            var expectedRootDirectory = GetSpecialFolder();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actualRootDirectory = KataArchive.GlobalArchiveRoot;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedRootDirectory, actualRootDirectory);
        }


        [TestCase("FizzBuzz",KataName.FizzBuzz)]
        [TestCase("StringCalculator",KataName.StringCalculator)]
        public void GenerateSolutionForAttempt_ReturnsKataAttempt(string kataName, KataName kataEnum)
        {
            //---------------Set up test pack-------------------
            var kataAttemptRepo = Substitute.For<IKataAttemptRepository>();
            var kataAttempt = Substitute.For<IKataAttempt>();
            var expected = RandomValueGen.GetRandomString();
            kataAttempt.Location.Returns(expected);
            kataAttemptRepo.CreateNewKataAttempt(kataEnum).Returns(kataAttempt);
            kataAttemptRepo.LoadKataAttemptFrom(kataAttempt.Location).Returns(kataAttempt);
            var kataArchive = Create(kataAttemptRepository:kataAttemptRepo);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = kataArchive.GenerateSolutionForAttempt(kataEnum);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            kataAttemptRepo.Received(1).CreateNewKataAttempt(kataEnum);
            kataAttemptRepo.Received(1).LoadKataAttemptFrom(kataAttempt.Location);
            Assert.AreEqual(expected,result.Location);
        }

        [TestCase("FizzBuzz",KataName.FizzBuzz)]
        [TestCase("StringCalculator",KataName.StringCalculator)]
        public void GenerateSolutionForAttempt_ShouldEmitFeedback(string kataName, KataName kataEnum)
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var kataAttemptRepository = Substitute.For<IKataAttemptRepository>();
            var kataAttempt = Substitute.For<IKataAttempt>();
            var expected = RandomValueGen.GetRandomString();
            kataAttempt.Location.Returns(expected);
            kataAttemptRepository.CreateNewKataAttempt(kataEnum).Returns(kataAttempt);
            var kataArchive = Create(eventAggregator, kataAttemptRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            kataArchive.GenerateSolutionForAttempt(kataEnum);
            //---------------Test Result -----------------------
            kataAttemptRepository.Received().CreateNewKataAttempt(kataEnum);
            eventAggregator.ShouldHavePublished<DisplayFeedbackEvent>(
                ev => ev.Message ==
                string.Format("CREATED SOLUTION [ {0} ] FOR  [ {1} ]", expected, kataName));
        }

        [TestCase(KataName.FizzBuzz)]
        [TestCase(KataName.StringCalculator)]
        public void GenerateLaunchActionFor_ReturnsAction(KataName kataEnum)
        {
            //---------------Set up test pack-------------------
            var kataArchive = Create();
            var solutionPath = kataArchive.GenerateSolutionForAttempt(kataEnum);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var launchActionFor = kataArchive.GenerateLaunchActionFor(solutionPath.Location);
            //---------------Test Result -----------------------
            Assert.IsNotNull(launchActionFor);            
        }

        [TestCase(KataName.FizzBuzz)]
        [TestCase(KataName.StringCalculator)]
        public void GenerateLaunchActionFor_ReturnsActionWhichWouldLaunchTheSolution(KataName kataEnum)
        {
            //---------------Set up test pack-------------------
            var vsHelper = Substitute.For<IVisualStudioHelper>();
            vsHelper.LaunchProject(Arg.Any<string>()).Returns(true);
            var kataAttemptRepository = Substitute.For<IKataAttemptRepository>();
            var kataAttempt = Substitute.For<IKataAttempt>();
            var kataConfig = Substitute.For<IKataAttemptConfiguration>();
            kataAttempt.Config.Returns(kataConfig);
            var kataArchive = Create(kataAttemptRepository: kataAttemptRepository,
                                        visualStudioHelper:vsHelper);
            var solutionPath = kataArchive.GenerateSolutionForAttempt(kataEnum);
            kataAttemptRepository.LoadKataAttemptFrom(solutionPath.Location).Returns(kataAttempt);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var launchActionFor = kataArchive.GenerateLaunchActionFor(solutionPath.Location);
            //---------------Test Result -----------------------
            Assert.IsNotNull(launchActionFor);
            vsHelper.Received(0).LaunchProject(solutionPath.Location);
            launchActionFor();
            vsHelper.Received(1).LaunchProject(solutionPath.Location);
        }

        private static KataArchive Create(IEventAggregator eventAggregator = null,
                                            IKataAttemptRepository kataAttemptRepository = null,
                                            IVisualStudioHelper visualStudioHelper = null,
                                            ISettingsManager settingsManager = null)
        {
            return new KataArchive(eventAggregator ?? Substitute.For<IEventAggregator>(),
                                    kataAttemptRepository ?? CrateKataAttemptRepositoryWhichProvidesFakeAttempts(),
                                    visualStudioHelper ?? CreateAlwaysPositiveVisualStudioHelper(),
                                    settingsManager ?? CreateSettingsManagerWhichReturnsSettings());
        }

        private static ISettingsManager CreateSettingsManagerWhichReturnsSettings()
        {
            var result = Substitute.For<ISettingsManager>();
            result.FetchCurrentSettings().Returns(new KataraiSettings());
            return result;
        }

        private static IKataAttemptRepository CrateKataAttemptRepositoryWhichProvidesFakeAttempts()
        {
            var result = Substitute.For<IKataAttemptRepository>();
            result.CreateNewKataAttempt(Arg.Any<KataName>()).ReturnsForAnyArgs(ci =>
            {
                var attempt = Substitute.For<IKataAttempt>();
                return attempt;
            });
            return result;
        }

        private static IVisualStudioHelper CreateAlwaysPositiveVisualStudioHelper()
        {
            var result = Substitute.For<IVisualStudioHelper>();
            result.LaunchProject(Arg.Any<string>()).Returns(true);
            return result;
        }

        private string GetSpecialFolder()
        {
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "Local");
            var fullPath = Path.Combine(rootPath, "Katarai");
            return fullPath;
        }
    }
}