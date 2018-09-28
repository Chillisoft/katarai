using System;
using System.Collections.Generic;
using Engine;
using Engine.Runners;
using Katarai.Runner;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestAnalysisResultProcessor
    {
        private AnalysisResultProcessor CreateAnalysisResultProcessor(
            ISettingsManager settingsManager = null,
            bool showHint = false)
        {
            if (settingsManager == null)
            {
                settingsManager = Substitute.For<ISettingsManager>();
                settingsManager.IsShowHintOn().Returns(showHint);
            }

            return new AnalysisResultProcessor(settingsManager);
        }

        [Test]
        public void Constructor_GivenNullSettingsManager_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new AnalysisResultProcessor(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("settingsManager", exception.ParamName);
        }

        [Test]
        public void GenerateHint_GivenNullResult_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var hint = processor.GenerateHint(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, hint);
        }

        [Test]
        public void GenerateHint_GivenResultWithNoMessages_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var result = new Result(3, 6, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var hint = processor.GenerateHint(result);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, hint);
        }

        [Test]
        public void GenerateHint_GivenResultWithNullHint_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var playerFeedback = new PlayerFeedback { Hint = null };
            var result = new Result(3, 6, playerFeedback);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var hint = processor.GenerateHint(result);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, hint);
        }

        [Test]
        public void GenerateHint_GivenResultWithEmptyHint_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var playerFeedback = new PlayerFeedback { Hint = "" };
            var result = new Result(3, 6, playerFeedback);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var hint = processor.GenerateHint(result);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, hint);
        }

        [Test]
        public void GenerateHint_GivenResultWithHint_ShouldReturnHint()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var playerFeedback = new PlayerFeedback { Hint = "Return the negative number in exception" };
            var result = new Result(3, 6, playerFeedback);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var hint = processor.GenerateHint(result);
            //---------------Test Result -----------------------
            var expected = "Return the negative number in exception";
            Assert.AreEqual(expected, hint);
        }

        [Test]
        public void GenerateDebugInfo_GivenNullResult_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var debugInfo = processor.GenerateDebugInfo(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, debugInfo);
        }

        [Test]
        public void GenerateDebugInfo_GivenResultWithNoPlayerFeedback_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var result = new Result(3, 6, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var debugInfo = processor.GenerateDebugInfo(result);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, debugInfo);
        }

        [Test]
        public void GenerateDebugInfo_GivenResultWithNullDebugInfo_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var playerFeedback = new PlayerFeedback { DebugInfo = null };
            var result = new Result(3, 6, playerFeedback);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var debugInfo = processor.GenerateDebugInfo(result);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, debugInfo);
        }

        [Test]
        public void GenerateDebugInfo_GivenResultWithEmptyDebugInfo_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var playerFeedback = new PlayerFeedback { DebugInfo = "" };
            var result = new Result(3, 6, playerFeedback);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var debugInfo = processor.GenerateDebugInfo(result);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, debugInfo);
        }

        [Test]
        public void GenerateDebugInfo_GivenResultWithDebugInfo_ShouldReturnDebugInfo()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var playerFeedback = new PlayerFeedback { DebugInfo = "L=1, T=5, and whatever else" };
            var result = new Result(3, 6, playerFeedback);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var debugInfo = processor.GenerateDebugInfo(result);
            //---------------Test Result -----------------------
            Assert.AreEqual("L=1, T=5, and whatever else", debugInfo);
        }

        [Test]
        public void GenerateMessage_GivenResultWithNoPlayerFeedback_ShouldReturnEmptyString()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var result = CreateResult(3, 6, false);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var message = processor.GenerateMessage(attemptGameState);
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Empty, message);
        }

        [Test]
        public void GenerateMessage_GivenIsShowHintOnIsFalse_ShouldReturnProgressMessageWithNoHint()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor(showHint: false);
            var playerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var result = CreateResult(3, 6, false, playerFeedback:playerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var message = processor.GenerateMessage(attemptGameState);
            //---------------Test Result -----------------------
            var expected = "Good";
            Assert.AreEqual(expected, message);
        }

        [Test]
        public void GenerateMessage_GivenPreviousResultIsGreenAndCurrentResultIsGreen_ShouldReturnProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var currentPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Green"
            };
            var previousPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle negative numbers",
                PlayerTestState = "Player Test State: Green"
            };
            var result = CreateResult(4, 7, false, playerFeedback: currentPlayerFeedback);
            var previousResult = CreateResult(3, 6, false, playerFeedback: previousPlayerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var feedbackMessage = processor.CreateFeedback(attemptGameState);
            //---------------Test Result -----------------------
            const string expected = "You are not allowed to write any production code unless it is to make a failing unit test pass. Please undo or comment out your implementation for your test and run your tests";
            Assert.AreEqual(expected, feedbackMessage.Message);
        }

        [Test]
        public void CreateFeedback_GivenPreviousResultIsRedAndCurrentResultIsRed_ShouldReturnProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var currentPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var previousPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle negative numbers",
                PlayerTestState = "Player Test State: Red"
            };
            var result = CreateResult(4, 7, false, playerFeedback: currentPlayerFeedback);
            var previousResult = CreateResult(3, 6, false, playerFeedback: previousPlayerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var feedbackMessage = processor.CreateFeedback(attemptGameState);
            //---------------Test Result -----------------------
            const string expected = "You have written a test without first running tests after writing your previous implementation. Please undo or comment out your current test and run your tests";
            Assert.AreEqual(expected, feedbackMessage.Message);
        }

        [Test]
        public void CreateFeedback_GivenPreviousResultIsGreenAndCurrentResultIsRed_ShouldReturnProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var currentPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var previousPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle negative numbers",
                PlayerTestState = "Player Test State: Green"
            };
            var result = CreateResult(2, 3, false, "Red", currentPlayerFeedback);
            var previousResult = CreateResult(1, 2, false, "Green", previousPlayerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var feedbackMessage = processor.CreateFeedback(attemptGameState);
            //---------------Test Result -----------------------
            const string expected = "You have written a test without having a failing test for your previous implementation. Please undo or comment out your test and the implementation for your previous test and run your tests";
            Assert.AreEqual(expected, feedbackMessage.Message);
        }

        [Test]
        public void CreateFeedback_GivenKataTimerHasNotStarted_ShouldReturnProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var currentPlayerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var result = CreateResult(2, 3, false, "Red", currentPlayerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var feedbackMessage = processor.CreateFeedback(attemptGameState);
            //---------------Test Result -----------------------
            const string expected = "The timer has not started. If you have the default constructor test that comes with solution, please ensure that you build the solution before writing your first test";
            Assert.AreEqual(expected, feedbackMessage.Message);
        }

        [Test]
        public void GenerateMessage_GivenIsShowHintOnIsTrue_ShouldReturnProgressMessageWithHint()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor(showHint: true);
            var playerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var result = CreateResult(3, 6, false, playerFeedback: playerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var message = processor.GenerateMessage(attemptGameState);
            //---------------Test Result -----------------------
            var expected = "Good" + Environment.NewLine + "Hint:"+ Environment.NewLine + "My Hint";
            Assert.AreEqual(expected, message);
        }

        [Test]
        public void GenerateMessage_GivenImplementationLevelHasFallenAndPlayerTestStateIsRed_ShouldReturnBrokenTestMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor(showHint: true);
            var playerFeedback = new PlayerFeedback
            {
                Hint = "My Hint",
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var previousResult = CreateResult(4, 6, false, playerFeedback: playerFeedback);
            var result = CreateResult(3, 6, false, playerFeedback: playerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var feedbackMessage = processor.CreateFeedback(attemptGameState);
            //---------------Test Result -----------------------
            var expected = "A test that was previously passing has broken! Perhaps undo the change you made and try again, or try and see why the test broke.";
            Assert.AreEqual(expected, feedbackMessage.Message);
        }

        [Test]
        public void GenerateMessage_GivenImplementationLevelHasFallenAndPlayerTestStateIsGreen_ShouldProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor(showHint: true);
            var playerFeedback = new PlayerFeedback
            {
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Green"
            };
            var previousResult = CreateResult(4, 6, false, playerFeedback: playerFeedback);
            var result = CreateResult(3, 6, false, playerFeedback: playerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var message = processor.GenerateMessage(attemptGameState);
            //---------------Test Result -----------------------
            var expected = "Good";
            Assert.AreEqual(expected, message);
        }

        [Test]
        public void GenerateMessage_GivenImplementationLevelIsEQToPrevious_ShouldReturnProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor(showHint: true);
            var playerFeedback = new PlayerFeedback
            {
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var previousResult = CreateResult(4, 6, false, playerFeedback: playerFeedback);
            var result = CreateResult(4, 6, false, playerFeedback: playerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var message = processor.GenerateMessage(attemptGameState);
            //---------------Test Result -----------------------
            var expected = "Good";
            Assert.AreEqual(expected, message);
        }

        [Test]
        public void GenerateMessage_GivenImplementationLevelIsGTPrevious_ShouldReturnProgressMessage()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor(showHint: true);
            var playerFeedback = new PlayerFeedback
            {
                Progress = "Good",
                DebugInfo = "single custom debug",
                StepShouldDo = "Handle a single custom delimiter",
                PlayerTestState = "Player Test State: Red"
            };
            var previousResult = CreateResult(4, 6, false, playerFeedback: playerFeedback);
            var result = CreateResult(5, 6, false, playerFeedback: playerFeedback);
            var attemptGameState = CreateAttemptGameState();
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(result);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var message = processor.GenerateMessage(attemptGameState);
            //---------------Test Result -----------------------
            var expected = "Good";
            Assert.AreEqual(expected, message);
        }

        [Test]
        public void GetPlayerTestStateIcon_GivenPlayerStateIsGreen_ShouldReturnGreenIcon()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var attemptGameState = Substitute.For<IAttemptGameState>();
            attemptGameState.IsPlayerTestStateGreen().Returns(true);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var playerTestStateIcon = processor.GetPlayerTestStateIcon(attemptGameState);
            //---------------Test Result -----------------------
            Assert.AreEqual(NotifyIcon.Green, playerTestStateIcon);
        }

        [Test]
        public void GetPlayerTestStateIcon_GivenPlayerStateIsRed_ShouldReturnRedIcon()
        {
            //---------------Set up test pack-------------------
            var processor = CreateAnalysisResultProcessor();
            var attemptGameState = Substitute.For<IAttemptGameState>();
            attemptGameState.IsPlayerTestStateGreen().Returns(false);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var playerTestStateIcon = processor.GetPlayerTestStateIcon(attemptGameState);
            //---------------Test Result -----------------------
            Assert.AreEqual(NotifyIcon.Red, playerTestStateIcon);
        }

        private Result CreateResult(int implementationLevel, int testLevel, bool kataCompleted, string testState = null, PlayerFeedback playerFeedback=null)
        {
            testState = testState ?? "Green";
            playerFeedback = playerFeedback ?? new PlayerFeedback { PlayerTestState = "Player Test State: " + testState, KataCompleted = kataCompleted };
            return new Result(
                new PlayerImplementationRunResult { Level = implementationLevel, StepShouldDo = "Do Level " + implementationLevel },
                new PlayerTestsRunResult { Level = testLevel },
                playerFeedback);

        }

        private AttemptGameState CreateAttemptGameState()
        {
            return new AttemptGameState(new List<Result>(), new KataHelper());
        }

    }
}