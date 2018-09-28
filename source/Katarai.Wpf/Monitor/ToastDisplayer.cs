using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Engine;
using Katarai.Controls;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Monitor
{
    public interface IToastDisplayer : IDisposable
    {
        void ShowNotificationBubble(string title, string message, NotifyIcon theIcon);
        void ShowNotificationBubble(string title, string message, int timeout = 0);
        void ShowNotificationBubble(string title, string message, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon);
    }

    //TODO zuko 09 Feb 2015: Create Tests for this

    public class ToastDisplayer : IToastDisplayer
    {
        private readonly IToast _toast;

        public ToastDisplayer(IToast toast)
        {
            if (toast == null) throw new ArgumentNullException("toast");
            _toast = toast;
        }

        public void ShowNotificationBubble(string title, string message, NotifyIcon theIcon)
        {
            try
            {
                var feedbackDisplay = CreateFeedbackDisplay(title, message, theIcon);
                if (ViewModel == null)
                {
                    _toast.ShowCustomBalloon(feedbackDisplay, PopupAnimation.Fade, 60000);
                }
                else
                {
                    UpdateTimer(feedbackDisplay);
                    UpdateProgressLevel(feedbackDisplay);
                    _toast.ShowCustomBalloon(feedbackDisplay, PopupAnimation.Fade,
                        ViewModel.NotificationVisibilityTimeSeconds*1000);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An Error Occured : " + e.Message + Environment.NewLine + "Stack Trace : " +
                                e.StackTrace);
            }
        }

        public void ShowNotificationBubble(string title, string message, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon)
        {
            try
            {
                var feedbackDisplay = CreateFeedbackDisplay(title, message, kataStateIcon,playerTestStateIcon);
                if (ViewModel == null)
                {
                    _toast.ShowCustomBalloon(feedbackDisplay, PopupAnimation.Fade, 60000);
                }
                else
                {
                    UpdateTimer(feedbackDisplay);
                    UpdateProgressLevel(feedbackDisplay);
                    _toast.ShowCustomBalloon(feedbackDisplay, PopupAnimation.Fade,
                        ViewModel.NotificationVisibilityTimeSeconds * 1000);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An Error Occured : " + e.Message + Environment.NewLine + "Stack Trace : " +
                                e.StackTrace);
            }
        }

        private FeedbackDisplay CreateFeedbackDisplay(string title, string message, NotifyIcon theIcon)
        {
            var feedbackDisplay = new FeedbackDisplay(_toast);
            feedbackDisplay.SetTitle(title);
            feedbackDisplay.SetMessage(message);
            feedbackDisplay.SetFeedbackType(theIcon.ToString());
            return feedbackDisplay;
        }

        private FeedbackDisplay CreateFeedbackDisplay(string title, string message, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon)
        {
            var feedbackDisplay = new FeedbackDisplay(_toast);
            feedbackDisplay.SetTitle(title);
            feedbackDisplay.SetMessage(message);
            feedbackDisplay.SetKataState(kataStateIcon);
            feedbackDisplay.SetPlayerTestState(playerTestStateIcon);
            return feedbackDisplay;
        }

        public void ShowNotificationBubble(string title, string message, int timeout = 0)
        {
            try
            {
                var welcomeDisplay = CreateWelcomeDisplay(title, message);
                if (ViewModel == null)
                    _toast.ShowCustomBalloon(welcomeDisplay, PopupAnimation.Slide, 60000);
                else
                {
                    if (timeout == 0)
                    {
                        timeout = ViewModel.NotificationVisibilityTimeSeconds*1000;
                    }
                    _toast.ShowCustomBalloon(welcomeDisplay, PopupAnimation.Fade, timeout);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An Error Occured : " + e.Message + Environment.NewLine + "Stack Trace : " +
                                e.StackTrace);
            }
        }

        private WelcomeDisplay CreateWelcomeDisplay(string title, string message)
        {
            var feedbackDisplay = new WelcomeDisplay(_toast, title, message);
            return feedbackDisplay;
        }

        public void Dispose()
        {
            if (_toast != null) _toast.Dispose();
        }

        private MainWindowViewModel ViewModel
        {
            get { return _toast.DataContext as MainWindowViewModel; }
        }

        private void UpdateTimer(FeedbackDisplay feedbackDisplay)
        {
            if (ViewModel.AttemptGameState == null) return;
            var kataTimer = ViewModel.AttemptGameState.KataTimer;
            if (kataTimer != null) feedbackDisplay.SetKataTimer(kataTimer);
        }

        private void UpdateProgressLevel(FeedbackDisplay feedbackDisplay)
        {
            if (ViewModel.AttemptGameState == null) return;
            if (ViewModel.AttemptGameState.LatestResult == null) return;
            var progressLevel = GetProgressLevel(ViewModel.AttemptGameState);
            feedbackDisplay.SetProgressLevel(progressLevel);
        }

        private static double GetProgressLevel(AttemptGameState attemptGameState)
        {
            var playerImplementationLevel = attemptGameState.LatestResult.PlayerImplementationLevel;
            var playerTestLevel = attemptGameState.LatestResult.PlayerTestLevel;
            var playerLevel = playerImplementationLevel + playerTestLevel;
            var kataMaxLevel = attemptGameState.KataMaxLevel*2;
            var progressLevel = Math.Round((((double) playerLevel/kataMaxLevel)*100), MidpointRounding.AwayFromZero);
            return progressLevel;
        }
    }
}