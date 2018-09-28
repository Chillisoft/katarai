using System;
using System.Collections.Generic;
using System.Windows.Interop;
using Engine;
using Katarai.Runner;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.Monitor
{
    public interface IAnalysisResultProcessor
    {
        void ProcessAnalysisResult(IPlayerNotifier playerNotifier, Result result, string errorMessage,
            AttemptGameState attemptGameState = null);
    }

    public class Feedback
    {
        public string Message { get; set; }
        public NotifyIcon KataStateIcon { get; set; }
        public NotifyIcon PlayerTestStateIcon { get; set; }
    }

    public class AnalysisResultProcessor : IAnalysisResultProcessor
    {
        private readonly ISettingsManager _settingsManager;

        public AnalysisResultProcessor(ISettingsManager settingsManager)
        {
            if (settingsManager == null) throw new ArgumentNullException("settingsManager");
            _settingsManager = settingsManager;
        }

        public void ProcessAnalysisResult(IPlayerNotifier playerNotifier, Result result, string errorMessage,
            AttemptGameState attemptGameState)
        {
            if (result != null)
            {
                attemptGameState.SetLatestResult(result);
                var feedback = CreateFeedback(attemptGameState);
                var latestResult = attemptGameState.LatestResult;
                var kataFullyCompleted = latestResult.PlayerFeedback.KataCompleted &&
                                         !latestResult.PlayerFeedback.AllLevelsCompleted;
                if (kataFullyCompleted)
                {
                    var kataDuration = attemptGameState.KataTimer.KataDuration;

                    feedback.Message = feedback.Message + Environment.NewLine + Environment.NewLine + "Time taken: " + kataDuration;
                    var kataCompletedView = new Views.KataCompletedView();
                    kataCompletedView.SetMessage(feedback.Message);
                    kataCompletedView.ShowActivated = true;
                    kataCompletedView.Show();
                }
                else
                {
                    playerNotifier.DisplayMessage("Test State", feedback.Message, feedback.KataStateIcon, feedback.PlayerTestStateIcon);
                }
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                playerNotifier.DisplayErrorMessage(errorMessage);
            }
        }

        internal Feedback CreateFeedback(AttemptGameState attemptGameState)
        {
            var feedback = new Feedback();
            if (attemptGameState.KataTimerHasNotStarted())
            {
                feedback.Message =
                    "The timer has not started. If you have the default constructor test that comes with solution, please ensure that you build the solution before writing your first test";
                feedback.KataStateIcon = NotifyIcon.Warning;
                feedback.PlayerTestStateIcon = NotifyIcon.Red;
            }
            if (attemptGameState.ImplementationLevelHasFallen())
            {
                feedback.Message =
                    "A test that was previously passing has broken! Perhaps undo the change you made and try again, or try and see why the test broke.";
                feedback.KataStateIcon = NotifyIcon.Warning;
                feedback.PlayerTestStateIcon = NotifyIcon.Red;
            }
            if (attemptGameState.HasImplementationBeenWrittenWithoutFailingTest())
            {
                feedback.Message =
                    "You are not allowed to write any production code unless it is to make a failing unit test pass. Please undo or comment out your implementation for your test and run your tests";
                feedback.KataStateIcon = NotifyIcon.Warning;
                feedback.PlayerTestStateIcon = NotifyIcon.Red;
            }
            if (attemptGameState.HasTestBeenWrittenWithoutAFailingTestForPreviousImplementation())
            {
                feedback.Message =
                    "You have written a test without having a failing test for your previous implementation. Please undo or comment out your test and the implementation for your previous test and run your tests";
                feedback.KataStateIcon = NotifyIcon.Warning;
                feedback.PlayerTestStateIcon = NotifyIcon.Red;
            }
            if (attemptGameState.HasTestBeenWrittenWithoutAPassingTestForPreviousImplementation())
            {
                feedback.Message =
                    "You have written a test without first running tests after writing your previous implementation. Please undo or comment out your current test and run your tests";
                feedback.KataStateIcon = NotifyIcon.Warning;
                feedback.PlayerTestStateIcon = NotifyIcon.Red;
            }
            if (!string.IsNullOrEmpty(feedback.Message)) return feedback;
            feedback.Message = GenerateMessage(attemptGameState);
            feedback.PlayerTestStateIcon = GetPlayerTestStateIcon(attemptGameState);
            feedback.KataStateIcon = GetKataStateIcon(attemptGameState);
            return feedback;
        }

        private static NotifyIcon GetKataStateIcon(AttemptGameState attemptGameState)
        {
            var isWarningIcon = attemptGameState.IsWarningIcon();
            var myIcon = isWarningIcon ? NotifyIcon.Warning : NotifyIcon.TwoThumbs;
            return myIcon;
        }

        internal NotifyIcon GetPlayerTestStateIcon(IAttemptGameState attemptGameState)
        {
            return attemptGameState.IsPlayerTestStateGreen() ? NotifyIcon.Green : NotifyIcon.Red;
        }

        internal string GenerateMessage(AttemptGameState attemptGameState)
        {
            var result = attemptGameState.LatestResult;

            var feedback = result.PlayerFeedback;
            if (feedback == null) return "";
            var messages = new List<string>();
            messages.Add(feedback.Progress);
            if (_settingsManager.IsShowHintOn())
            {
                var hint = GenerateHint(result);
                if (!string.IsNullOrEmpty(hint))
                {
                    messages.Add("Hint:" + Environment.NewLine + hint);
                }
            }
            var finalMessage = string.Join(Environment.NewLine, messages);
            return finalMessage;
        }

        internal string GenerateDebugInfo(Result result)
        {
            if (result == null) return string.Empty;
            var playerFeedback = result.PlayerFeedback;
            if (playerFeedback == null) return string.Empty;
            return playerFeedback.DebugInfo ?? string.Empty;
        }

        internal string GenerateHint(Result result)
        {
            if (result == null) return string.Empty;
            var playerFeedback = result.PlayerFeedback;
            if (playerFeedback == null) return string.Empty;
            return playerFeedback.Hint ?? string.Empty;
        }
    }
}