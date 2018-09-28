using System;
using System.Collections.Generic;
using Engine.Runners;
using Katarai.Runner;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestAttemptGameState
    {
        [Test]
        public void CheckImplementationLevel_GivenPlayerStateIsRed_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var result = new Result(1, 1, new PlayerFeedback { PlayerTestState = "Player Test State: Red" });
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = result;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNull(progressEvent);
        }

        [Test]
        public void CheckImplementationLevel_GivenResultsContainImplmentationLevel_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var result1 = CreateResult(4,4,"Green");
            var result2 = CreateResult(5,5,"Green");
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1, result2 });
            attemptGameState.SetLatestResult(result2);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNull(progressEvent);
        }
        
        [Test]
        public void CheckImplementationLevel_GivenResultsContainImplmentationLevel_ButFirstPassingResultAtLevel_ShouldReturnProgressEvent()
        {
            //---------------Set up test pack-------------------
            var result1 = CreateResult(4, false, "Green");
            var result2 = CreateResult(5, false, "Red");
            var result3 = CreateResult(5, false, "Green");
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1, result2 });
            attemptGameState.LatestResult = result3;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
        }

        [Test]
        public void CheckImplementationLevel_GivenResultsSkipsPreviousLevel_ShouldReturnProgressFromLastLevel()
        {
            //---------------Set up test pack-------------------
            var result1 = CreateResult(3, false, "Green");
            var result2 = CreateResult(5, false, "Green");
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1 });
            attemptGameState.LatestResult = result2;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent, "Has a Progress Event");
            Assert.AreEqual(3, progressEvent.FromLevel);
            Assert.AreEqual(5, progressEvent.ToLevel);
        }

        [Test]
        public void CheckImplementationLevel_GivenHasManyPassingResultsForPreviousLevel_ShouldReturnProgressWithTimeDifferenceFromFirstPassingResult()
        {
            //---------------Set up test pack-------------------
            var dateTime = DateTime.Now;
            var result1 = CreateResult(3, false, "Green");
            var result2 = SetTime(CreateResult(4, false, "Red"), dateTime.AddSeconds(-90));
            var result3 = SetTime(CreateResult(4, false, "Green"), dateTime.AddSeconds(-45));
            var result4 = SetTime(CreateResult(4, false, "Green"), dateTime.AddSeconds(-60));
            var result5 = SetTime(CreateResult(4, false, "Green"), dateTime.AddSeconds(-30));
            var result6 = SetTime(CreateResult(5, false, "Green"), dateTime.AddSeconds(0));
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1, result2, result3, result4, result5 });
            attemptGameState.LatestResult = result6;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent, "Has a Progress Event");
            Assert.AreEqual(4, progressEvent.FromLevel);
            Assert.AreEqual(5, progressEvent.ToLevel);
            Assert.AreEqual(60, progressEvent.DurationInSeconds);
        }


        [Test]
        public void CheckImplementationLevel_GivenResultsContainPreviousImplmentationLevel_ShouldReturnProgressEvent()
        {
            //---------------Set up test pack-------------------
            var result1 = CreateResult(3, false);
            var result2 = CreateResult(4, false);
            var result3 = CreateResult(5, false);
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1, result2 });
            attemptGameState.LatestResult = result3;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual(4, progressEvent.FromLevel);
            Assert.AreEqual(5, progressEvent.ToLevel);
            Assert.AreEqual("Do Level 4", progressEvent.FromLevelDescription);
            Assert.AreEqual("Do Level 5", progressEvent.ToLevelDescription);
            Assert.IsFalse(progressEvent.KataCompleted);
            Assert.AreEqual(DateTime.MinValue, progressEvent.KataCompletedTime);
        }

        [Test]
        public void CheckImplementationLevel_GivenKataCompleted_ShouldReturnProgressEventAsCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 10);
            results.Add(CreateResult(11, true, "Red"));
            var result3 = CreateResult(11, true);
            var attemptGameState = CreateAttemptGameState(results);
            attemptGameState.KataName = KataName.StringCalculator.ToString();
            attemptGameState.SetLatestResult(result3);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var progressEvent = attemptGameState.CheckImplementationLevel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual(10, progressEvent.FromLevel);
            Assert.AreEqual(11, progressEvent.ToLevel);
            Assert.AreEqual("Do Level 10", progressEvent.FromLevelDescription);
            Assert.AreEqual("Do Level 11", progressEvent.ToLevelDescription);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.AreNotEqual(DateTime.MinValue, progressEvent.KataCompletedTime);
        }

        [Test]
        public void IsWarningIcon_GivenBothImplementationAndTestLevelAreEqual_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = CreateResult(3, 3);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isWarningIcon = attemptGameState.IsWarningIcon();
            //---------------Test Result -----------------------
            Assert.IsFalse(isWarningIcon);
        }

        [Test]
        public void IsWarningIcon_GivenBothImplementationHasBeenWrittenWithNoFailingTests_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = CreateResult(5, 5);
            attemptGameState.LatestResult = CreateResult(6, 6);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            var isWarningIcon = attemptGameState.IsWarningIcon();
            //---------------Test Result -----------------------
            Assert.IsTrue(isWarningIcon);
        }

        [Test]
        public void IsWarningIcon_GivenImplementationLevelGTTestLevel_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = CreateResult(4, 3);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isWarningIcon = attemptGameState.IsWarningIcon();
            //---------------Test Result -----------------------
            Assert.IsTrue(isWarningIcon);
        }

        [Test]
        public void IsWarningIcon_GivenTestLevelGTImplementationLevelByOne_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = CreateResult(4, 5);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isWarningIcon = attemptGameState.IsWarningIcon();
            //---------------Test Result -----------------------
            Assert.IsFalse(isWarningIcon);
        }

        [Test]
        public void IsWarningIcon_GivenTestLevelGTImplementationLevelByTwo_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = CreateResult(3, 5);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isWarningIcon = attemptGameState.IsWarningIcon();
            //---------------Test Result -----------------------
            Assert.IsTrue(isWarningIcon);
        }

        [Test]
        public void IsWarningIcon_GivenTestLevelGTImplementationLevelByMany_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.LatestResult = CreateResult(3, 6);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isWarningIcon = attemptGameState.IsWarningIcon();
            //---------------Test Result -----------------------
            Assert.IsTrue(isWarningIcon);
        }

        [Test]
        public void CheckKataStarted_WhenLatestTestResultIsLevel1_AndResultsCountEQZero_ShouldSetKataAsStarted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            var kataStarted = false;
            attemptGameState.KataStarted += (sender, args) => { kataStarted = true; };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(1, 1, "Green"));
            //---------------Test Result -----------------------
            Assert.IsTrue(kataStarted);
        }

        [Test]
        public void CheckKataStarted_WhenLatestTestResultIsLevel2_AndResultsCountEQZero_ShouldSetKataAsStarted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            var kataStarted = false;
            attemptGameState.KataStarted += (sender, args) => { kataStarted = true; };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(1, 2, "Red"));
            //---------------Test Result -----------------------
            Assert.IsTrue(kataStarted);
        }

        [Test]
        public void CheckKataStarted_WhenLatestTestResultIsLevel2_AndResultsCountEQOne_ShouldNotSetKataAsStarted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>{CreateResult(1,1)});
            var kataStarted = false;
            attemptGameState.KataStarted += (sender, args) => { kataStarted = true; };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(1, 2, "Red"));
            //---------------Test Result -----------------------
            Assert.IsFalse(kataStarted);
        }

        [Test]
        public void CheckKataStarted_WhenLatestTestResultIsLevel1_AndResultsCountEQOne_ShouldNotSetKataAsStarted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>{CreateResult(1,1)});
            var kataStarted = false;
            attemptGameState.KataStarted += (sender, args) => { kataStarted = true; };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(1, 1, "Green"));
            //---------------Test Result -----------------------
            Assert.IsFalse(kataStarted);
        }

        [Test]
        public void CheckKataStarted_WhenLatestTestResultIsLevel3_AndResultsCountEQZero_ShouldNotSetKataAsStarted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            var kataStarted = false;
            attemptGameState.KataStarted += (sender, args) => { kataStarted = true; };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(2, 3, "Red"));
            //---------------Test Result -----------------------
            Assert.IsFalse(kataStarted);
        }

        [Test]
        public void SetLatestResult_GivenResultsContainImplementationLevel_ShouldNotCreateProgressEvent()
        {
            //---------------Set up test pack-------------------
            var result1 = CreateResult(4, 4,"Green");
            var result2 = CreateResult(5, 5,"Green");
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1, result2 });
            attemptGameState.LatestResult = result1;
            var progressEventCreated = false;
            attemptGameState.KataProgress += (sender, args) => { progressEventCreated = true; };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(result2);
            //---------------Test Result -----------------------
            Assert.IsFalse(progressEventCreated);
        }
        
        [Test]
        public void SetLatestResult_GivenResultsDoesNotContainImplementationLevel_ShouldCreateProgressEvent()
        {
            //---------------Set up test pack-------------------
            var result1 = CreateResult(4, 4,"Green");
            var result2 = CreateResult(5, 5,"Green");
            var attemptGameState = CreateAttemptGameState(new List<Result> { result1 });
            attemptGameState.LatestResult = result1;
            var progressEventCreated = false;
            attemptGameState.KataProgress += (sender, args) => { progressEventCreated = true; };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(result2);
            //---------------Test Result -----------------------
            Assert.IsTrue(progressEventCreated);
        }
        
        [Test]
        public void SetLatestResult_GivenResultsStartAtLevel2_ShouldCreateProgressEventAsNotCompleted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.SetLatestResult(CreateResult(1, 2,"Red"));
            attemptGameState.SetLatestResult(CreateResult(2, 2,"Green"));
            attemptGameState.SetLatestResult(CreateResult(2, 3,"Red"));
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(3, 3,"Green"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual("2,3", progressEvent.LevelsCompleted);
            Assert.IsFalse(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
        }
        
        [Test]
        public void SetLatestResult_GivenResultsStartAtLevel1_ShouldCreateProgressEventAsNotCompleted()
        {
            //---------------Set up test pack-------------------
            var attemptGameState = CreateAttemptGameState(new List<Result>());
            attemptGameState.SetLatestResult(CreateResult(1, 1,"Green"));
            attemptGameState.SetLatestResult(CreateResult(1, 2,"Red"));
            attemptGameState.SetLatestResult(CreateResult(2, 2,"Green"));
            attemptGameState.SetLatestResult(CreateResult(2, 3,"Red"));
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(3, 3,"Green"));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual("1,2,3", progressEvent.LevelsCompleted);
            Assert.IsFalse(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
        }
        
        [Test]
        public void SetLatestResult_GivenAllLevelsCompletedFromLevel1_ShouldCreateProgressEventAsCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 10);
            var attemptGameState = CreateAttemptGameState(results);
            attemptGameState.SetLatestResult(CreateResult(10, 11,"Red"));
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(11, 11,"Green", true));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,10,11", progressEvent.LevelsCompleted);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
        }

        [Test]
        public void SetLatestResult_GivenAllLevelsCompletedFromLevel2_ShouldCreateProgressEventAsCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(2, 10);
            var attemptGameState = CreateAttemptGameState(results);
            attemptGameState.SetLatestResult(CreateResult(10, 11,"Red"));
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(11, 11,"Green", true));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual("2,3,4,5,6,7,8,9,10,11", progressEvent.LevelsCompleted);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
        }

        [Test]
        public void SetLatestResult_GivenLevelMissing_ShouldCreateProgressEventAsNotCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 9);
            var attemptGameState = CreateAttemptGameState(results);
            attemptGameState.SetLatestResult(CreateResult(10, 11,"Red"));
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(11, 11,"Green", true));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,11", progressEvent.LevelsCompleted);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.IsFalse(progressEvent.AllLevelsCompleted);
        }

        [Test]
        public void SetLatestResult_GivenMissingFailingLevel11Result_ShouldCreateProgressEventAsCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 10);
            var attemptGameState = CreateAttemptGameState(results);
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(11, 11, "Green", true));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,10,11", progressEvent.LevelsCompleted);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
        }

        [Test]
        public void SetLatestResult_GivenHasPassingLastLevelAndThenPassingPreviousLevel_ShouldCreateProgressEventAsCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 9);
            var attemptGameState = CreateAttemptGameState(results);
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.SetLatestResult(CreateResult(11, 11, "Green", true));
            attemptGameState.SetLatestResult(CreateResult(10, 10, "Green", true));
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(11, 11, "Green", true));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,11,10", progressEvent.LevelsCompleted);
        }

        [Test]
        public void SetLatestResult_GivenRerunForLatestResultWhenCompleted_ShouldCreateProgressEventAsCompleted()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 9);
            var attemptGameState = CreateAttemptGameState(results);
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.SetLatestResult(CreateResult(11, 11, "Green", true));
            attemptGameState.SetLatestResult(CreateResult(10, 10, "Green", true));
            attemptGameState.SetLatestResult(CreateResult(11, 11, "Green", true));
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(11, 11, "Green", true));
            //---------------Test Result -----------------------
            Assert.IsNotNull(progressEvent);
            Assert.IsTrue(progressEvent.KataCompleted);
            Assert.IsTrue(progressEvent.AllLevelsCompleted);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,11,10", progressEvent.LevelsCompleted);
        }

        [Test]
        public void SetLatestResult_GivenRerunForLatestResultWhenNotCompleted_ShouldNotCreateProgressEvent()
        {
            //---------------Set up test pack-------------------
            var results = GetResultsl(1, 10);
            var attemptGameState = CreateAttemptGameState(results);
            var progressEventCreated = false;
            ProgressEvent progressEvent = null;
            attemptGameState.SetLatestResult(CreateResult(10, 10, "Green", false));
            attemptGameState.KataProgress += (sender, args) =>
            {
                progressEventCreated = true;
                progressEvent = args.ProgressEvent;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(progressEventCreated);
            //---------------Execute Test ----------------------
            attemptGameState.SetLatestResult(CreateResult(10, 10, "Green", false));
            //---------------Test Result -----------------------
            Assert.IsNull(progressEvent);
        }

        [Test]
        public void HasImplementationBeenWrittenWithoutFailingTest_WhenTrue_ShouldRevertToPreviousResult()
        {
            //---------------Set up test pack-------------------
            var previousResult = CreateResult(2, false, "Green");
            var latestResult = CreateResult(3, false, "Green");
            var results = new List<Result>();
            var attemptGameState = CreateAttemptGameState(results);
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(latestResult);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var implementationHasBeenWrittenWithoutFailingTests = attemptGameState.HasImplementationBeenWrittenWithoutFailingTest();
            //---------------Test Result -----------------------
            Assert.IsTrue(implementationHasBeenWrittenWithoutFailingTests);
            Assert.AreEqual(2, attemptGameState.LatestResult.PlayerImplementationLevel);
        }
        [Test]
        public void HasTestBeenWrittenWithoutRunningTestsAfterWritingPreviousImplementation_WhenTrue_ShouldRevertToPreviousResult()
        {
            //---------------Set up test pack-------------------
            var previousResult = CreateResult(2, false, "Red");
            var latestResult = CreateResult(3, false, "Red");
            var results = new List<Result>();
            var attemptGameState = CreateAttemptGameState(results);
            attemptGameState.SetLatestResult(previousResult);
            attemptGameState.SetLatestResult(latestResult);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var hasTestBeenWrittenWithoutRunningTestsForPreviousImplementation = attemptGameState.HasTestBeenWrittenWithoutAPassingTestForPreviousImplementation();
            //---------------Test Result -----------------------
            Assert.IsTrue(hasTestBeenWrittenWithoutRunningTestsForPreviousImplementation);
            Assert.AreEqual(2, attemptGameState.LatestResult.PlayerImplementationLevel);
        }


        private List<Result> GetResultsl(int fromLevel, int toLevel)
        {
            var results = new List<Result>();
            for (var i = fromLevel; i <= toLevel; i++)
            {
                results.Add(CreateResult(i, false, "Red"));
            }
            for (var i = fromLevel; i <= toLevel; i++)
            {
                results.Add(CreateResult(i, false, "Green"));
            }
            return results;
        }

        private Result SetTime(Result result, DateTime dateTime)
        {
            var resultFake = (ResultFake)result;
            resultFake._SetTime(dateTime);
            return result;
        }

        private static AttemptGameState CreateAttemptGameState(List<Result> results)
        {
            var attemptGameState = new AttemptGameState(results, new KataHelper())
            {
                KataName = KataName.StringCalculator.ToString()
            };
            return attemptGameState;
        }
        
        private List<Result> CreateResults(int noOfResults)
        {
            var results = new List<Result>();
            for (var i = 0; i < noOfResults + 1; i++)
            {
                results.Add(CreateResult(i + 1, false));
            }
            return results;
        }

        private static Result CreateResult(int implementationLevel, bool kataCompleted, string testState = null)
        {
            testState = testState ?? "Green";
            return new ResultFake(
                new PlayerImplementationRunResult { Level = implementationLevel, StepShouldDo = "Do Level " + implementationLevel },
                new PlayerTestsRunResult { Level = implementationLevel },
                new PlayerFeedback { PlayerTestState = "Player Test State: " + testState, KataCompleted = kataCompleted });

        }

        private static Result CreateResult(int implementationLevel, int testLevel, string testState = null, bool kataCompleted = false)
        {
            testState = testState ?? "Green";
            return new ResultFake(
                new PlayerImplementationRunResult { Level = implementationLevel, StepShouldDo = "Do Level " + implementationLevel },
                new PlayerTestsRunResult { Level = testLevel },
                new PlayerFeedback { PlayerTestState = "Player Test State: " + testState, KataCompleted = kataCompleted });

        }

        private class ResultFake : Result
        {
            public ResultFake(int playerImplementationlevel, int playerTestLevel, PlayerFeedback playerFeedback) 
                : base(playerImplementationlevel, playerTestLevel, playerFeedback)
            {
            }

            public ResultFake(PlayerImplementationRunResult playerImplementationRunResult, PlayerTestsRunResult playerTestsRunResult, PlayerFeedback playerFeedback) 
                : base(playerImplementationRunResult, playerTestsRunResult, playerFeedback)
            {
            }
            
            /// <summary>
            /// Horrible method for testing, we need to find a better way!
            /// </summary>
            /// <param name="dateTime"></param>
            public void _SetTime(DateTime dateTime)
            {
                Time = dateTime;
            }
        }
    }
}