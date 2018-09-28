using System;
using Caliburn.Micro;
using Engine;
using Katarai.Wpf.Events;
using Katarai.Wpf.Extensions;

namespace Katarai.Wpf.Monitor
{
    public interface IPlayerNotifier: IDisposable
    {
        void DisplayErrorMessage(string errorMessage);
        void DisplayMessage(string title, string msg, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon);
        void DisplayMessage(string title, string msg);
    }

    public class PlayerNotifier : IPlayerNotifier
    {
        private readonly IToastDisplayer _notifyIcon;
        private readonly IEventAggregator _eventAggregator;
        private bool _displayedCritialError;
        

        public PlayerNotifier(IToastDisplayer notifyIcon, IEventAggregator eventAggregator)
        {
            if (notifyIcon == null) throw new ArgumentNullException(nameof(notifyIcon));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            _notifyIcon = notifyIcon;
            _eventAggregator = eventAggregator;
        }

        private void PublishFeedbackEventFor(string message)
        {
            _eventAggregator.Publish<DisplayFeedbackEvent>(message);
        }

        public void DisplayErrorMessage(string errorMessage)
        {
            if (_displayedCritialError) return;
            PublishFeedbackEventFor("Error :: " + errorMessage);
            _displayedCritialError = true;
        }

        public void DisplayMessage(string title, string msg, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon)
        {
            _displayedCritialError = false;
            PublishFeedbackEventFor(msg);
            _notifyIcon.ShowNotificationBubble(title, msg, kataStateIcon, playerTestStateIcon);

        }

        public void DisplayMessage(string title, string msg)
        {
            _displayedCritialError = false;
            PublishFeedbackEventFor(msg);
            _notifyIcon.ShowNotificationBubble(title, msg);

        }

        public void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Dispose();
            }
        }
    }
}