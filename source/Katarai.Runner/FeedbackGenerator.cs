using System;
using System.Linq;
using Engine.Annotations;

namespace Katarai.Runner
{
    public class FeedbackGenerator
    {
        public PlayerFeedback GeneratePlayerFeedback(IOverallAnalysisResult overallAnalysisResult)
        {
            var resultAnalyser = new ResultAnalyser();
            var stateCode = resultAnalyser.GetStateCode(overallAnalysisResult);
            var doNotShowImplementedTooMuchMessage = GetDoNotShowImplementedTooMuchMessage(overallAnalysisResult);
            var progress = GetProgressMessage(stateCode, overallAnalysisResult, resultAnalyser, doNotShowImplementedTooMuchMessage);
            var debugInfo = GetDebugInfo(overallAnalysisResult);
            var hint = GetHint(overallAnalysisResult, stateCode, doNotShowImplementedTooMuchMessage);
            return
                new PlayerFeedback
                {
                    Progress = progress,
                    Hint = hint,
                    KataCompleted = IsKataCompleted(overallAnalysisResult),
                    KataStateCode = stateCode,
                    //StepShouldDo = testMethodMeta.StepShoudlDo, 
                    //EdgeCaseHint = testMethodMeta.EdgeCaseHint,
                    //SuggestedTestName = testMethodMeta.SuggestedTestName,
                    DebugInfo = debugInfo,
                    PlayerTestState = overallAnalysisResult.PlayerTestsPlayerImplementationRunResult.ToString()
                };
        }

        private bool GetDoNotShowImplementedTooMuchMessage(IOverallAnalysisResult overallAnalysisResult)
        {
            var goldenTestMethods = overallAnalysisResult.GoldenTestMethods;
            var level = overallAnalysisResult.PlayerImplementationRunResult.Level;
            var testMethods = goldenTestMethods.Where(method => method.Level == level);
            var doNotShowImplementedTooMuchMessage =
                testMethods.Any(method => method.HasDoNotShowImplementedTooMuchMessageAttribute);
            return doNotShowImplementedTooMuchMessage;
        }

        private string GetHint(IOverallAnalysisResult overallAnalysisResult, string stateCode, bool doNotShowImplementedTooMuchMessage)
        {
            var playerImplementationLevel = overallAnalysisResult.PlayerImplementationRunResult.Level;
            var playerTestFixtureLevel = overallAnalysisResult.PlayerTestsRunResult.Level;
            switch (stateCode)
            {
                case "[G] T=I (I=Last)":
                case "[G] T=I (TE=EC) (EC=EI) (I=Last)":
                    return "Try your Kata without hints next time.";
                case "[G] T=I (I=1)":
                {
                    var shouldDoText = GetShouldDoText(overallAnalysisResult, 2);
                    return "Your first test is: " + shouldDoText;
                }
                case "[R] T>I+1":
                case "[R] T>I+1 (I=1)":
                {
                    var currentlyTesting = GetShouldDoText(overallAnalysisResult, playerTestFixtureLevel);
                    var shouldBeTesting = GetShouldDoText(overallAnalysisResult, playerImplementationLevel + 1);
                    return "The test Katarai was expecting was: " + shouldBeTesting + Environment.NewLine +
                           "The test you wrote appears to be testing: " + currentlyTesting;
                }
                case "[G] T=I":
                case "[G] T=I (EC=EI)":
                case "[G] T=I (TE=EC) (EC=EI)":
                {
                    return GetNextTestHint(overallAnalysisResult, playerTestFixtureLevel);
                }
                case "[G] T<I":
                case "[G] T<I (I=Last)":
                {
                    if (doNotShowImplementedTooMuchMessage) return GetNextTestHint(overallAnalysisResult, playerTestFixtureLevel);
                    var implemented = GetShouldDoText(overallAnalysisResult, playerImplementationLevel);
                    var shouldBeImplementing = GetShouldDoText(overallAnalysisResult, playerTestFixtureLevel);
                    return "The test you are trying to get to pass is: " + shouldBeImplementing + Environment.NewLine +
                           "The code you wrote also gets the following test to pass: " + implemented;
                }
                case "[G] T=I (EC<EI) (I=Last)":
                case "[G] T=I (EC<EI)":
                case "[G] T=I+1 (EC=EI)":
                    //case "[G] T=I+1 (EC=EI) (I=Last)": test this
                case "[G] T=I+1 (EC<EI)":
                case "[G] T=I+1 (EC<EI) (I=1)":
                case "[G] T=I+1 (TE>EC)":
                case "[G] T>I+1 (TE>EC)":
                case "[R] T=I+1 (TE>EC)":
                case "[R] T>I+1 (TE>EC)":
                case "[G] T=I (TE>EC)":
                case "[G] T=I (TE>EC) (I=Last)":
                {
                    var playerTestsRunResult = overallAnalysisResult.PlayerTestsRunResult;
                    var messagesForEdgeCasesNotCovered = playerTestsRunResult.GetMessagesForEdgeCasesNotCovered();
                    return Environment.NewLine + string.Join(Environment.NewLine, messagesForEdgeCasesNotCovered);
                }                 
            }
            return string.Empty;
        }

        private string GetNextTestHint(IOverallAnalysisResult overallAnalysisResult, int playerTestFixtureLevel)
        {
            var shouldDoText = GetShouldDoText(overallAnalysisResult, playerTestFixtureLevel + 1);
            return "Your next test is: " + shouldDoText;
        }

        private string GetShouldDoText(IOverallAnalysisResult overallAnalysisResult, int testLevel)
        {
            var testMethod = overallAnalysisResult.GoldenTestMethods.FirstOrDefault(method => method.Level == testLevel);
            if (testMethod == null) return string.Empty;
            var stepShouldDoAttributes = testMethod.KataAnnotations.OfType<StepShouldDoAttribute>();
            var firstStepShouldDoAttribute = stepShouldDoAttributes.FirstOrDefault();
            return firstStepShouldDoAttribute == null ? string.Empty : firstStepShouldDoAttribute.ShouldDoText;
        }

        private bool HasNoRefactoringAnnotation(IOverallAnalysisResult overallAnalysisResult)
        {
            var testLevel = overallAnalysisResult.PlayerTestsRunResult.Level;
            var testMethod = overallAnalysisResult.GoldenTestMethods.FirstOrDefault(method => method.Level == testLevel);
            if (testMethod == null) return false;
            var noRefactoringAttributes = testMethod.KataAnnotations.OfType<NoRefactoringAttribute>();
            var noRefactoringAttribute = noRefactoringAttributes.FirstOrDefault();
            return noRefactoringAttribute != null;
        }

        private bool IsKataCompleted(IOverallAnalysisResult overallAnalysisResult)
        {
            var playerImplementationLevel = overallAnalysisResult.PlayerImplementationRunResult.Level;
            var playerTestFixtureLevel = overallAnalysisResult.PlayerTestsRunResult.Level;
            var lastKataLevel = overallAnalysisResult.GoldenTestMethods.Max(method => method.Level);
            var kataCompleted = (playerTestFixtureLevel == playerImplementationLevel) &&
                                (lastKataLevel == playerImplementationLevel);
            return kataCompleted;
        }

        private string GetDebugInfo(IOverallAnalysisResult overallAnalysisResult)
        {
            return string.Format("{1}{0}{2}{0}", Environment.NewLine,
                overallAnalysisResult.PlayerImplementationRunResult, overallAnalysisResult.PlayerTestsRunResult);
        }

        private string GetProgressMessage(string stateCode, IOverallAnalysisResult overallAnalysisResult, ResultAnalyser resultAnalyser, bool doNotShowImplementedTooMuchMessage)
        {

            string progress;

            switch (stateCode)
            {
                case "HTC":
                    progress = "Use of attribute [TestCase] is not permitted";
                    break;
                case "HEE":
                    progress = "Use of attribute [ExpectedException] is not permitted";
                    break;
                case "[G] T=I (I=1)":
                    progress = "Great, Katarai has noticed you’ve built the solution, and your kata timer has started! Please go ahead and write the first test.";
                    break;
                case "[G] T=I (I=Last)":
                case "[G] T=I (EC=EI) (I=Last)":
                case "[G] T=I (TE=EC) (EC=EI) (I=Last)":
                    progress = "Congratulations! You have successfully completed the Kata." +
                               Environment.NewLine + "Remember to review your code to check for any refactorings that could be done.";
                    break;
                case "[R] T=I+1":
                case "[R] T=I+1 (I=1)":
                case "[R] T=I+1 (EC=EI)":
                case "[R] T=I (EC=EI)":
                case "[R] T=I (EC>EI)":
                case "[R] T=I (EC>EI) (I=Last)":
                    //case "[R] T=I+1 (EC=EI) (I=1)": to test
                case "[R] T=I+1 (EC<EI)":
                case "[R] T=I+1 (EI=0)":
                case "[R] T=I":
                case "[R] T=I (TE>EC)":
                case "[R] T=I+1 (TE=EC) (EC=EI)":
                case "[R] T=I (TE=EC) (EC=EI)":
                case "[R] T=I (EI=0)":
                case "[R] T=I (TE=EC) (EC=EI) (I=Last)":
                    progress = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
                    break;
                case "[G] T=I+1 (I=1)":
                case "[G] T=I+1":
                case "[G] T=I+1 (EI=0)":
                case "[G] T=I+1 (TE=EC) (EC=EI)":
                case "[G] T>I+1":
                    progress = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
                    break;
                case "[R] T>I+1 (I=1)":
                    progress = "You wrote a valid test, but there is a simpler first test that could be written. Remember that we are looking for the simplest test at all times to drive the code in tiny increments." 
                            + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD" + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)";
                    break;
                case "[R] T>I+1":
                    progress = "You wrote a valid test, but there is a simpler test that could be written first. Remember that we are looking for the next simplest test at all times to drive the code in tiny increments." 
                            + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD" + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)";
                    break;
                case "[R] T=I+1 (TE>EC)":
                case "[R] T>I+1 (TE>EC)":
                    progress = "You must cover all edge cases before moving to the next step in the kata - can you find the edge cases that should be tested?";
                    break;
                case "[G] T=I":
                case "[G] T=I (EC=EI)":
                case "[G] T=I (TE=EC) (EC=EI)":
                    progress = GetNextTestProgress(overallAnalysisResult);
                    break;
                case "[G] T<I":
                case "[G] T<I (I=Last)":
                    if (doNotShowImplementedTooMuchMessage)
                    {
                        progress = GetNextTestProgress(overallAnalysisResult);
                    }
                    else
                    {
                        progress = "The implementation you wrote could be simpler than it is – your test is passing but there is a simpler solution. Remember that you are trying to write the simplest/least possible production code to get your tests to pass."
                                + Environment.NewLine + "For a bigger discussion of this, see Uncle Bob’s Three Rules of TDD" + Environment.NewLine + "(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)";
                    }
                    break;
                case "[G] T=I (EC<EI)":
                case "[G] T=I (EC<EI) (I=Last)":
                case "[G] T=I+1 (EC<EI)":
                case "[G] T=I+1 (EC<EI) (I=1)":
                case "[G] T=I+1 (TE>EC)":
                case "[G] T>I+1 (TE>EC)":
                case "[G] T=I (TE>EC)":
                case "[G] T=I (TE>EC) (I=Last)":
                    progress = "Success! Your test is passing. It is good practice to ensure that you are testing edge cases – can you find another edge case that should be tested in this same scenario?";
                    break;
                case "[R] T=I+1 (EC>EI)":
                case "[R] T=I+1 (EC>EI) (I=1)":
                    progress = "You’ve written a valid failing test; please continue to write the implementation that will get this test to pass.";
                    break;
                case "[G] T=I+1 (EC>EI)":
                case "[G] T=I+1 (EC>EI) (I=1)":
                    progress = "Success! Your test is passing. To improve your implementation to be more generic, write another test for the same scenario with different values and then go on to get that test to pass too.";
                    break;
                case "[G] T=I+1 (EC=EI)":
                    //case "T=I+1 (EC=EI) (I=Last)": test this
                    progress = "Success! Your test is passing but it looks like you missed an edge case."
                               + Environment.NewLine + "Can you find the edge cases that should be tested?";
                    break;
                case "[R] T=-1":
                case "[R] T=-1 (I=1)":
                case "[R] T=-1 (I=Last)":
                    progress = "Katarai senses that there is an issue with your failing test – please correct the test before you try to get it passing." + Environment.NewLine +
                               GetInvalidTestHint(overallAnalysisResult, resultAnalyser);
                    break;
                case "[G] T=-1":
                case "[G] T=-1 (I=1)":
                case "[G] T=-1 (I=Last)":
                    progress = "Katarai senses that there is an issue with your passing test – please correct the test before moving on.";
                    break;
                default:
                    progress = "Unknown kata state";
                    break;
            }
            return progress;
        }

        private string GetNextTestProgress(IOverallAnalysisResult overallAnalysisResult)
        {
            var progress = HasNoRefactoringAnnotation(overallAnalysisResult)
                ? "Success! With that test passing, please continue on to writing the next one."
                : "Success! Your test is passing." + Environment.NewLine + "Now is your chance to refactor: if you can see anything you can improve in your implementation code then do so now. If you don’t see anything you can improve then go on to the next test.";
            return progress;
        }

        private string GetInvalidTestHint(IOverallAnalysisResult overallAnalysisResult, ResultAnalyser resultAnalyser)
        {
            var implementationLevel = overallAnalysisResult.PlayerImplementationRunResult.Level;
            var playerTestsRunResult = overallAnalysisResult.PlayerTestsRunResult;
            var minLevelEdgeCasesNotCovered = resultAnalyser.GetMinLevelEdgeCasesNotCovered(playerTestsRunResult);
            if (minLevelEdgeCasesNotCovered != -1 && minLevelEdgeCasesNotCovered <= implementationLevel) return string.Empty;
            var testMethod = overallAnalysisResult.GoldenTestMethods.FirstOrDefault(method => method.Level == implementationLevel + 1);
            if (testMethod == null) return string.Empty;
            var invalidTestHintAttributes = testMethod.KataAnnotations.OfType<InvalidTestHintAttribute>();
            var invalidTestHintAttribute = invalidTestHintAttributes.FirstOrDefault();
            return invalidTestHintAttribute == null ? string.Empty : invalidTestHintAttribute.InvalidTestHint;
        }
    }
}