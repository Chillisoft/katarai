using System.Collections.Generic;
using System.Linq;
using Engine;
using Katarai.Acceptance.StringCalculator.Tests.NewTestWrittenWithoutAPassingTestForPreviousImplementation;
using Katarai.KataData.StringCalculator;
using Katarai.Runner;
using Katarai.StringCalculator.Interfaces;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;
using TestStringCalculator = Katarai.Acceptance.StringCalculator.Tests.NewTestWrittenWithoutAPassingTestForPreviousImplementation.TestStringCalculator;

namespace Katarai.Acceptance.StringCalculator.Tests
{
    [TestFixture]
    public class TestRunner
    {
        [Test]
        public void GetResult_GivenFailingTestForTwoNumbers_ShouldReturnContinueResult()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = runner.GetResult(typeof (IStringCalculator),
                typeof (KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(UnknownStateTwoNumbers.StringCalculator),
                typeof(UnknownStateTwoNumbers.TestStringCalculator), 
                kataImplementationTypes.ToArray());
            //---------------Test Result -----------------------
            Assert.AreEqual(4, result.PlayerImplementationLevel);
            Assert.AreEqual(4, result.PlayerTestLevel);
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", result.PlayerFeedback.Progress);
        }

        [Test]
        public void GetResult_GivenPassingTestForTwoNumbers_ShouldReturnNotAllEdgeCasesTestedResult()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = runner.GetResult(typeof (IStringCalculator),
                typeof (KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(UnknownStateTwoNumbers2.StringCalculator),
                typeof(UnknownStateTwoNumbers2.TestStringCalculator), 
                kataImplementationTypes.ToArray());
            //---------------Test Result -----------------------
            Assert.AreEqual(4, result.PlayerImplementationLevel);
            Assert.AreEqual(4, result.PlayerTestLevel);
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, result.PlayerFeedback.Progress);
        }

        [Test]
        public void GetResult_GivenAnotherPassingTestForTwoNumbers_ShouldReturnNotAllEdgeCasesTestedResult()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = runner.GetResult(typeof (IStringCalculator),
                typeof (KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(UnknownStateTwoNumbers3.StringCalculator),
                typeof(UnknownStateTwoNumbers3.TestStringCalculator), 
                kataImplementationTypes.ToArray());
            //---------------Test Result -----------------------
            Assert.AreEqual(4, result.PlayerImplementationLevel);
            Assert.AreEqual(4, result.PlayerTestLevel);
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, result.PlayerFeedback.Progress);
        }

        [Test]
        public void ProcessAnalysisResult_GivenNewTestWithoutRunningTestsAfterPreviousImplementation_ShouldDisplayMessage()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            var notifier = Substitute.For<IPlayerNotifier>();
            var previousResult = runner.GetResult(typeof(IStringCalculator),
                typeof(KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(NewTestWrittenWithoutAPassingTestForPreviousImplementation.StringCalculator),
                typeof(TestStringCalculator),
                kataImplementationTypes.ToArray());
            var result = runner.GetResult(typeof(IStringCalculator),
                typeof(KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(StringCalculator_002),
                typeof(TestStringCalculator_002),
                kataImplementationTypes.ToArray());
            var processor = new AnalysisResultProcessor(Substitute.For<ISettingsManager>());
            var attemptGameState = new AttemptGameState(new List<Result>(),Substitute.For<IKataHelper>() );
            attemptGameState.SetLatestResult(previousResult);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, previousResult.PlayerImplementationLevel);
            Assert.AreEqual(2, previousResult.PlayerTestLevel);
            //---------------Execute Test ----------------------
            processor.ProcessAnalysisResult(notifier,result,null,attemptGameState);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, result.PlayerImplementationLevel);
            Assert.AreEqual(3, result.PlayerTestLevel);
            const string expectedMessage = "You have written a test without first running tests after writing your previous implementation. Please undo or comment out your current test and run your tests";
            notifier.Received().DisplayMessage("Test State", expectedMessage, NotifyIcon.Warning, NotifyIcon.Red);
        }

        [Test]
        public void ProcessAnalysisResult_GivenNewTestWrittenWithoutFailingTestForPreviousImplementation_ShouldDisplayMessage()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            var notifier = Substitute.For<IPlayerNotifier>();
            var previousResult = runner.GetResult(typeof(IStringCalculator),
                typeof(KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(NewTestWrittenWithoutAFailingTestForPreviousImplementation.StringCalculator),
                typeof(NewTestWrittenWithoutAFailingTestForPreviousImplementation.TestStringCalculator),
                kataImplementationTypes.ToArray());
            var result = runner.GetResult(typeof(IStringCalculator),
                typeof(KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(NewTestWrittenWithoutAFailingTestForPreviousImplementation.StringCalculator_002),
                typeof(NewTestWrittenWithoutAFailingTestForPreviousImplementation.TestStringCalculator_002),
                kataImplementationTypes.ToArray());
            var processor = new AnalysisResultProcessor(Substitute.For<ISettingsManager>());
            var attemptGameState = new AttemptGameState(new List<Result>(),Substitute.For<IKataHelper>() );
            attemptGameState.SetLatestResult(previousResult);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, previousResult.PlayerImplementationLevel);
            Assert.AreEqual(1, previousResult.PlayerTestLevel);
            //---------------Execute Test ----------------------
            processor.ProcessAnalysisResult(notifier,result,null,attemptGameState);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, result.PlayerImplementationLevel);
            Assert.AreEqual(3, result.PlayerTestLevel);
            const string expectedMessage = "You have written a test without having a failing test for your previous implementation. Please undo or comment out your test and the implementation for your previous test and run your tests";
            notifier.Received().DisplayMessage("Test State", expectedMessage, NotifyIcon.Warning, NotifyIcon.Red);
        }

        [Test]
        public void GetResult_GivenPassingTestForMultipleCustomDelimiters_ShouldSpecificImplementationResult()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = runner.GetResult(typeof (IStringCalculator),
                typeof (KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(UnknownStateMultipleCustomDelimiters.StringCalculator),
                typeof(UnknownStateMultipleCustomDelimiters.TestStringCalculator), 
                kataImplementationTypes.ToArray());
            //---------------Test Result -----------------------
            Assert.AreEqual(9, result.PlayerImplementationLevel);
            Assert.AreEqual(11, result.PlayerTestLevel);
            var expectedMessage = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
            Assert.AreEqual(expectedMessage, result.PlayerFeedback.Progress);
        }

        [Test]
        public void GetResult_GivenFailingTestForMulipleCustomDelimiters_ShouldReturnNotAllEdgeCasesTestedResult()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = runner.GetResult(typeof (IStringCalculator),
                typeof (KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(UnknownStateMultipleCustomDelimiters2.StringCalculator),
                typeof(UnknownStateMultipleCustomDelimiters2.TestStringCalculator), 
                kataImplementationTypes.ToArray());
            //---------------Test Result -----------------------
            Assert.AreEqual(11, result.PlayerImplementationLevel);
            Assert.AreEqual(11, result.PlayerTestLevel);
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, result.PlayerFeedback.Progress);
        }

        [Test]
        public void GetResult_GivenPassingTestForMultiCharCustomDelimiter_ShouldReturnNotAllEdgeCasesTestedResult()
        {
            //---------------Set up test pack-------------------
            var configuration = new Configuration();
            var kataImplementationTypes = configuration.GetKataImplementationTypes();
            var runner = new Runner.Runner("", "", "");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = runner.GetResult(typeof (IStringCalculator),
                typeof (KataData.StringCalculator.Tests.TestStringCalculator),
                typeof(SpecificTestNegativeNumbers.StringCalculator),
                typeof(SpecificTestNegativeNumbers.TestStringCalculator), 
                kataImplementationTypes.ToArray());
            //---------------Test Result -----------------------
            Assert.AreEqual(6, result.PlayerImplementationLevel);
            Assert.AreEqual(8, result.PlayerTestLevel);
            var expectedMessage = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
            Assert.AreEqual(expectedMessage, result.PlayerFeedback.Progress);
        }

    }
}