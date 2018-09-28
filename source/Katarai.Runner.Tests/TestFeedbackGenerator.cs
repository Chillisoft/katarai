using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Analysers;
using Engine.Annotations;
using Engine.Runners;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Runner.Tests
{
    [TestFixture]
    public class TestFeedbackGenerator
    {
        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new FeedbackGenerator());
            //---------------Test Result -----------------------
        }

        [Test]
        public void GeneratePlayerFeedback_Ref1_GivenPlayerIsStartingKata_ShouldReturnKataStartMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 1, levelOfImpl: 1, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Great, Katarai has noticed you’ve built the solution, and your kata timer has started! Please go ahead and write the first test.", playerFeedback.Progress);
            Assert.AreEqual("Your first test is: Step should do 2", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref2_GivenPlayerAboutToWriteNextTest_AndTestDoesNotHaveNoRefactoringAttribute_ShouldReturnAboutToWriteNextTest()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 2, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expected = "Success! Your test is passing." + Environment.NewLine + "Now is your chance to refactor: if you can see anything you can improve in your implementation code then do so now. If you don’t see anything you can improve then go on to the next test.";
            Assert.AreEqual(expected, playerFeedback.Progress);
            var expectedHint = "Your next test is: Step should do 3";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref2_GivenPlayerAboutToWriteNextTest_AndTestDoesHaveNoRefactoringAttribute_ShouldReturnAboutToWriteNextTest()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 2, totalLevels: 10);
            AddNoRefactoringAttribute(overallAnalysisResult, 2);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! With that test passing, please continue on to writing the next one.", playerFeedback.Progress);
            var expectedHint = "Your next test is: Step should do 3";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref2a_GivenPlayerAboutToWriteNextTest_HasCompletedEdgeCases_ShouldReturnAboutToWriteNextTest()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 2, totalLevels: 10);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expected = "Success! Your test is passing." + Environment.NewLine + "Now is your chance to refactor: if you can see anything you can improve in your implementation code then do so now. If you don’t see anything you can improve then go on to the next test.";
            Assert.AreEqual(expected, playerFeedback.Progress); 
            var expectedHint = "Your next test is: Step should do 3";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref2a_GivenPlayerAboutToWriteNextTest_HasCompletedEdgeCases_WithNoRefactoringAttribute_ShouldReturnAboutToWriteNextTest()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 2, totalLevels: 10);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            });
            AddNoRefactoringAttribute(overallAnalysisResult, 2);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! With that test passing, please continue on to writing the next one.", playerFeedback.Progress);
            var expectedHint = "Your next test is: Step should do 3";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref2b_GivenPlayerHasWrittenInvalidTest_HasCompletedEdgeCases_ShouldReturnInvalidTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }
        
        [Test]
        public void GeneratePlayerFeedback_GivenPlayerHasWrittenFirstCorrectTest_ShouldReturnPlayerTestStateGreen()
        {
            //---------------Set up test pack-------------------
            const int playerImplementationLevel = 1;
            const int playerTestFixtureLevel = 2;
            var feedbackGenerator = CreateReporter();
            var overallAnalysisResult = CreateOverallAnalysisResult(playerImplementationLevel, playerTestFixtureLevel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Player Test State: Green", playerFeedback.PlayerTestState);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenInvalidTest_ShouldReturnInvalidTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenInvalidTest_ShouldReturnInvalidTestMessageWithInvalidTestHint()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: false);
            SetInvalidTestHintForTest(overallAnalysisResult, 5);
            SetInvalidTestHintForTest(overallAnalysisResult, 6);
            SetInvalidTestHintForTest(overallAnalysisResult, 7);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." 
                    + Environment.NewLine + "Invalid test hint 6";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }
        
        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenInvalidTestForEdgeCaseTest_ShouldReturnInvalidTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 6, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, false, "Check 1000 not filtered")
            });
            SetInvalidTestHintForTest(overallAnalysisResult, 7);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }
        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenInvalidTestForFirstLevel_ShouldReturnInvalidTestMessageWithInvalidTestHint()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 1, totalLevels: 10, playerTestsPassing: false);
            SetInvalidTestHintForTest(overallAnalysisResult, 1);
            SetInvalidTestHintForTest(overallAnalysisResult, 6);
            SetInvalidTestHintForTest(overallAnalysisResult, 7);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenInvalidTestForLastLevel_ShouldReturnInvalidTestMessageWithInvalidTestHint()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 10, totalLevels: 10, playerTestsPassing: false);
            SetInvalidTestHintForTest(overallAnalysisResult, 8);
            SetInvalidTestHintForTest(overallAnalysisResult, 9);
            SetInvalidTestHintForTest(overallAnalysisResult, 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenPassingInvalidTest_ShouldReturnInvalidTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your passing test – please correct the test before moving on.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenPassingInvalidTest_ShouldReturnInvalidTestMessageWithInvalidTestHint()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: true);
            SetInvalidTestHintForTest(overallAnalysisResult, 5);
            SetInvalidTestHintForTest(overallAnalysisResult, 6);
            SetInvalidTestHintForTest(overallAnalysisResult, 7);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your passing test – please correct the test before moving on.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenPassingInvalidTestForFirstLevel_ShouldReturnInvalidTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 1, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your passing test – please correct the test before moving on.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref3_GivenPlayerHasWrittenPassingInvalidTestForLasttLevel_ShouldReturnInvalidTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: -1, levelOfImpl: 10, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Katarai senses that there is an issue with your passing test – please correct the test before moving on.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        private static void SetInvalidTestHintForTest(OverallAnalysisResult overallAnalysisResult, int testLevel)
        {
            var testMethod = overallAnalysisResult.GoldenTestMethods.First(method => method.Level == testLevel);
            testMethod.KataAnnotations.Returns(new List<IKataAnnotation>
            {
                new StepShouldDoAttribute("Step should do " + testLevel),
                new InvalidTestHintAttribute("Invalid test hint " + testLevel),
            });
        }

        [Test]
        public void GeneratePlayerFeedback_Ref4_GivenPlayerHasWrittenFirstCorrectTest_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 1, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref4_GivenPlayerHasChosenCorrectNextTest_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 3, levelOfImpl: 2, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref4_GivenPlayerHasFailingTestForSameLevel_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 3, levelOfImpl: 3, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref4_GivenPlayerHasFailingTestForLastLevelWithEdgeCases_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 10, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref5_GivenPlayerHasImplementedTest_ButImplementationIsTooSpecific_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 3, levelOfImpl: 2, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }
        
        [Test]
        public void GeneratePlayerFeedback_Ref5_GivenPlayerHasImplementedTest_ButImplementationIsTooSpecific_AndFirstTest_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 1, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref5_GivenPlayerHasImplementedTest_ButImplementationIsTooSpecific_AndLastTest_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 9, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref5_GivenPlayerHasImplementedTestFoNextLevel_ButImplementationIsTooSpecific_AndFirstTest_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 8, totalLevels: 10, playerTestsPassing: true);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref6_GivenPlayerHasChosenIncorrectFirstTest_ShouldReturnFirstTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 3, levelOfImpl: 1, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You wrote a valid test, but there is a simpler first test that could be written. Remember that we are looking for the simplest test at all times to drive the code in tiny increments."
                            + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD"  + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)", playerFeedback.Progress);
            var expectedHint = "The test Katarai was expecting was: Step should do 2" + Environment.NewLine +
                               "The test you wrote appears to be testing: Step should do 3";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref7_GivenPlayerHasNotChosenCorrectNextTest_ShouldReturnNextBestTestPrompt()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 4, levelOfImpl: 2, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You wrote a valid test, but there is a simpler test that could be written first. Remember that we are looking for the next simplest test at all times to drive the code in tiny increments." 
                            + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD" + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)", playerFeedback.Progress);
            var expectedHint = "The test Katarai was expecting was: Step should do 3" + Environment.NewLine +
                               "The test you wrote appears to be testing: Step should do 4";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref8_GivenSUTImplementationIsAheadOfTestImplementation_ShouldReturnImplementationAhead
            ()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 3, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("The implementation you wrote could be simpler than it is – your test is passing but there is a simpler solution. Remember that you are trying to write the simplest/least possible production code to get your tests to pass."
                                + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD" + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)", playerFeedback.Progress);

            var expectedHint = "The test you are trying to get to pass is: Step should do 2" + Environment.NewLine +
                               "The code you wrote also gets the following test to pass: Step should do 3";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref8a_GivenSUTImplementationIsAheadOfTestImplementationLastLevel_ShouldReturnImplementationAhead
            ()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 9, levelOfImpl: 10, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("The implementation you wrote could be simpler than it is – your test is passing but there is a simpler solution. Remember that you are trying to write the simplest/least possible production code to get your tests to pass."
                                + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD" + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)", playerFeedback.Progress);

            var expectedHint = "The test you are trying to get to pass is: Step should do 9" + Environment.NewLine +
                               "The code you wrote also gets the following test to pass: Step should do 10";
            Assert.AreEqual(expectedHint, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref14_GivenPlayerHasCompletedKata_ShouldReturnKataCompletionMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 10, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Congratulations! You have successfully completed the Kata." +
                               Environment.NewLine + "Remember to review your code to check for any refactorings that could be done.", playerFeedback.Progress);
            Assert.AreEqual("Try your Kata without hints next time.", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref14a_GivenPlayerHasCompletedKataWithAllEdgeCasesCovered_ShouldReturnKataCompletionMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 10, totalLevels: 10);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Congratulations! You have successfully completed the Kata." +
                               Environment.NewLine + "Remember to review your code to check for any refactorings that could be done.", playerFeedback.Progress);
            Assert.AreEqual("Try your Kata without hints next time.", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_GivenPlayerHasCompletedKata_ShouldReturnKataCompletedAsTrue()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 10, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.IsTrue(playerFeedback.KataCompleted);
        }

        [Test]
        public void GeneratePlayerFeedback_GivenPlayerHasTestedButNotImplementedLastLevelOfKata_ShouldReturnKataCompletedAsFalse()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 9, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.IsFalse(playerFeedback.KataCompleted);
        }

        [Test]
        public void GeneratePlayerFeedback_GivenPlayerOnLastLevelOfKata_ShouldReturnKataCompletedAsFalse()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 9, levelOfImpl: 9, totalLevels: 10);
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.IsFalse(playerFeedback.KataCompleted);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref9_GivenEdgeCaseNotCoveredButOnPrimaryTest_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();

            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, false, "Edge Case Hint 6")
            }
            , new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, false, "Edge Case Hint 6")
            });

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref9a_GivenEdgeCaseNotCoveredButOnPrimaryTest_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 6, totalLevels: 10, playerTestsPassing: false);
            var feedbackGenerator = CreateReporter();

            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, false, "Edge Case Hint 6")
            }
            , new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Edge Case Hint 6")
            });

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Ignore("TODO: Discuss if this is valid test")] //TODO Soriya 10 Feb 2015: Ignored Test - TODO: Discuss if this is valid test
        [Test]
        //TODO mark 30 Jan 2015: Also do last level test of this....
        public void GeneratePlayerFeedback_Ref10_GivenEdgeCasesNotCoveredOrImplemented_WithGreenPlayerState_ShouldReturnMissedAnEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: true);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(6, false, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(6, false, "Check 1000 not filtered"),
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, false, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Good, but it  looks like you missed an edge case." +
                        Environment.NewLine + "Can you find the edge cases to test?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual("You are missing the edge case test(s):" + 
                Environment.NewLine + "Filter > 1000" +
                Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref11_GivenEdgeCaseCoveredButNotImplemented_ShouldReturnFoundAnEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 5, totalLevels: 6, playerTestsPassing: false);
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            };
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                    new EdgeCaseImplementationResult{IsImplemented = true, Level = 6},
                    new EdgeCaseImplementationResult{IsImplemented = false, Level = 6}
            };
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref11_GivenEdgeCaseCoveredButNotImplementedForFirstTest_ShouldReturnFoundAnEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 1, totalLevels: 6, playerTestsPassing: false);
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            };
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                    new EdgeCaseImplementationResult{IsImplemented = true, Level = 6},
                    new EdgeCaseImplementationResult{IsImplemented = false, Level = 6}
            };
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref11a_GivenEdgeCaseCoveredButNotImplemented_ButTestsPassing_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 5, totalLevels: 6, playerTestsPassing: true);
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            };
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                    new EdgeCaseImplementationResult{IsImplemented = false, Level = 6},
                    new EdgeCaseImplementationResult{IsImplemented = true, Level = 6}
            };
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref11a_GivenEdgeCaseCoveredButNotImplementedForFirstTest_ButTestsPassing_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 1, totalLevels: 6, playerTestsPassing: true);
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(6, true, "Check 1000 not filtered")
            };
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                    new EdgeCaseImplementationResult{IsImplemented = true, Level = 6},
                    new EdgeCaseImplementationResult{IsImplemented = false, Level = 6}
            };
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref11b_GivenEdgeCaseNotCoveredAndNotImplemented_ButTestsPassing_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 4, totalLevels: 6, playerTestsPassing: true);
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, false, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(6, false, "Check 1000 not filtered")
            };
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                    new EdgeCaseImplementationResult{IsImplemented = false, Level = 6},
                    new EdgeCaseImplementationResult{IsImplemented = false, Level = 6}
            };
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.", playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref11a_GivenEdgeCaseCoveredAndmplemented_AndTestsPassing_ShouldReturnWriteAnotherTestMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 4, levelOfImpl: 3, totalLevels: 6, playerTestsPassing: true);
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(3, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(3, true, "Check 1000 not filtered")
            };
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                    new EdgeCaseImplementationResult{IsImplemented = true, Level = 3},
                    new EdgeCaseImplementationResult{IsImplemented = true, Level = 3}
            };
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12_GivenEdgeCaseImplementedButNotCoveredForFirstLevel_ShouldReturnNotTestingEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 1, totalLevels: 10);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12_GivenEdgeCaseImplementedButNotCoveredForLast_ShouldReturnNotTestingEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 6, totalLevels: 6);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12a_GivenLevelTestPassing_ButEdgeCasesNotCoveredOrImplemented_ShouldReturnFindEdgeCasesMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 4, totalLevels: 10, playerTestsPassing: true);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12a_GivenFirstLevelTestPassing_ButEdgeCasesNotCoveredOrImplemented_ShouldReturnFindEdgeCasesMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 1, totalLevels: 10, playerTestsPassing: true);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12b_GivenLevelTestNotPassing_AndEdgeCasesNotCoveredOrImplemented_ShouldReturnWriteImplemenationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 4, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12c_GivenLevelTestPassingButEdgeTestNotPassing_ShouldReturnWriteImplemenationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, false, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12c_GivenLevelTestPassingButEdgeTestNotPassingLastLevel_ShouldReturnWriteImplemenationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 10, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(10, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(10, false, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(10, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(10, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }


        [Test]
        public void GeneratePlayerFeedback_Ref15_GivenNextLevelTestNotPassing_ButEdgeCasesNotCoveredForPreviousLevel_ShouldReturnMustCoverAllEdgeCasesMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 4, levelOfImpl: 3, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(3, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(3, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(3, false, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(3, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You must cover all edge cases before moving to the next step in the kata - can you find the edge cases that should be tested?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Filter > 1000" +
                Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref15_GivenHigherLevelTestNotPassing_ButEdgeCasesNotCoveredForLevelsBelow_ShouldReturnMustCoverAllEdgeCasesMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 3, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(3, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(3, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(3, false, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(3, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You must cover all edge cases before moving to the next step in the kata - can you find the edge cases that should be tested?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Filter > 1000" +
                Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref17_GivenAllEdgeCaseNotCoveredButOneEdgeCoveredAndImplemented_WithGreenPlayerState_ShouldReturnMissedAnEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 6, totalLevels: 10, playerTestsPassing: true);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>(), new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(6, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(6, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref17_GivenAllEdgeCaseNotCoveredButOneEdgeCoveredAndImplementedOnLastLevel_WithGreenPlayerState_ShouldReturnMissedAnEdgeCaseMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 10, levelOfImpl: 10, totalLevels: 10, playerTestsPassing: true);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>(), new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(10, true, "Filter > 1000"),
                CreatePlayerTestsEdgeCaseResult(10, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(Environment.NewLine + "Check 1000 not filtered", playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12d_GivenLevelTestNotPassingButEdgeTestCovered_ShouldReturnWriteImplemenationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 4, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, false, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, false, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_Ref12d_GivenLevelTest2NotPassingButEdgeTestCovered_ShouldReturnWriteImplemenationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 5, levelOfImpl: 5, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, false, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, false, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }
        [Test]
        public void GeneratePlayerFeedback_GivenAllEdgeCasesCoveredAndImplemented_WithRedPlayerState_ShouldReturnWriteImplementationMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 6, levelOfImpl: 6, totalLevels: 10, playerTestsPassing: false);
            SetupEdgeCaseStateFor(overallAnalysisResult, new List<EdgeCaseImplementationResult>
            {
                CreateEdgeCaseImplementationResult(5, true, "Filter > 1000"),
                CreateEdgeCaseImplementationResult(5, true, "Check 1000 not filtered")
            }, new List<EdgeCaseCoverageResult>
            {
                CreatePlayerTestsEdgeCaseResult(5, true, "Filter > 1000"), 
                CreatePlayerTestsEdgeCaseResult(5, true, "Check 1000 not filtered")
            });
            var feedbackGenerator = CreateReporter();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            var expectedMessage = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
            Assert.AreEqual(expectedMessage, playerFeedback.Progress);
            Assert.AreEqual(string.Empty, playerFeedback.Hint);
        }

        [Test]
        public void GeneratePlayerFeedback_GivenTestMethodHasTestCaseAttribute_ShouldReturnNoTestCasesAllowedMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 2, totalLevels: 10);
            overallAnalysisResult.PlayerTestsRunResult.HasTestCaseAttribute = true;
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Use of attribute [TestCase] is not permitted", playerFeedback.Progress);
        }

        [Test]
        public void GeneratePlayerFeedback_GivenTestMethodHasExpectedExceptionAttribute_ShouldReturnNoExpectedAttributeAllowedMessage()
        {
            //---------------Set up test pack-------------------
            var overallAnalysisResult = CreateOverallAnalysisResultWith(levelOfTests: 2, levelOfImpl: 2, totalLevels: 10);
            overallAnalysisResult.PlayerTestsRunResult.HasExpectedExceptionAttribute = true;
            var feedbackGenerator = CreateReporter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var playerFeedback = feedbackGenerator.GeneratePlayerFeedback(overallAnalysisResult);
            //---------------Test Result -----------------------
            Assert.AreEqual("Use of attribute [ExpectedException] is not permitted", playerFeedback.Progress);
        }

        private static void SetupEdgeCaseStateFor(OverallAnalysisResult overallAnalysisResult, List<EdgeCaseImplementationResult> edgeCaseImplementationResults, List<EdgeCaseCoverageResult> playerTestsEdgeCaseResults)
        {
            overallAnalysisResult.PlayerTestsRunResult.PlayerTestsEdgeCoverageResults = playerTestsEdgeCaseResults;
            overallAnalysisResult.PlayerImplementationRunResult.EdgeCaseImplementationResults = edgeCaseImplementationResults;
        }

        private static EdgeCaseCoverageResult CreatePlayerTestsEdgeCaseResult(int level, bool isCovered, string edgeCaseMessage)
        {
            var playerTestsEdgeCaseResult = new EdgeCaseCoverageResult
            {
                Level = level,
                IsCovered = isCovered,
                EdgeCaseMessage = edgeCaseMessage
            };
            return playerTestsEdgeCaseResult;
        }

        private static EdgeCaseImplementationResult CreateEdgeCaseImplementationResult(int level, bool isImplemented, string edgeCaseMessage)
        {
            var playerTestsEdgeCaseResult = new EdgeCaseImplementationResult
            {
                Level = level,
                IsImplemented = isImplemented,
                EdgeCaseMessage = edgeCaseMessage
            };
            return playerTestsEdgeCaseResult;
        }

        private OverallAnalysisResult CreateOverallAnalysisResultWith(string dummy = "Do Not Use", int levelOfTests = -1, int levelOfImpl = -1, int totalLevels = 10, bool playerTestsPassing = true)
        {
            if (dummy != "Do Not Use") throw new Exception("Do not use the dummy parameter, but rather specify the other params explicitly!");

            if (levelOfTests < -1) throw new Exception("Please provide the test level!");
            if (levelOfImpl < -1) throw new Exception("Please provide the implementation level!");
            var goldenTestMethods = CreateGoldenTestMethods(totalLevels);
            var overallAnalysisResult = CreateOverallAnalysisResult(levelOfImpl, levelOfTests, goldenTestMethods, playerTestsPassing);
            return overallAnalysisResult;
        }

        private OverallAnalysisResult CreateOverallAnalysisResult(int playerImplementationLevel,
            int playerTestFixtureLevel, ITestMethod[] goldenTestMethods = null, bool playerTestsPassing = true)
        {
            goldenTestMethods = goldenTestMethods ?? CreateGoldenTestMethods();
            return new OverallAnalysisResult(CreatePlayerImplementationRunResult(playerImplementationLevel),
                CreatePlayerTestFixtureRunResult(playerTestFixtureLevel),
                CreatePlayerTestsPlayerImplementationRunResult(playerTestsPassing), goldenTestMethods);
        }

        private PlayerImplementationRunResult CreatePlayerImplementationRunResult(int level)
        {
            return new PlayerImplementationRunResult
            {
                Level = level,
            };
        }

        private static PlayerTestsRunResult CreatePlayerTestFixtureRunResult(int level = 0, int noOfTests = 0)
        {
            return new PlayerTestsRunResult
            {
                Level = level,
                NumberOfTestsImplemented = noOfTests
            };
        }

        private static PlayerTestsPlayerImplementationRunResult CreatePlayerTestsPlayerImplementationRunResult(bool playerTestsPassing)
        {
            return new PlayerTestsPlayerImplementationRunResult
            {
                Passed = playerTestsPassing
            };
        }

        private static ITestMethod[] CreateGoldenTestMethods(int numberToCreate = 1)
        {
            return Enumerable.Repeat("", numberToCreate)
                .Select((s, index) => CreateFakeTestMethodFor(index + 1))
                .ToArray();
        }

        private static ITestMethod CreateFakeTestMethodFor(int level)
        {
            var testMethod = Substitute.For<ITestMethod>();
            testMethod.Level.Returns(level);
            testMethod.KataAnnotations.Returns(new List<IKataAnnotation>
            {
                new StepShouldDoAttribute("Step should do " + level)
            });
            return testMethod;
        }

        private static void AddNoRefactoringAttribute(OverallAnalysisResult overallAnalysisResult, int testLevel)
        {
            var testMethod = overallAnalysisResult.GoldenTestMethods.FirstOrDefault(method => method.Level == testLevel);
            testMethod.KataAnnotations.Returns(new List<IKataAnnotation>
            {
                new StepShouldDoAttribute("Step should do " + testLevel),
                new NoRefactoringAttribute()
            });
        }



        private static FeedbackGenerator CreateReporter()
        {
            return new FeedbackGenerator();
        }
    }
}