using System;
using System.Collections.Generic;
using System.Linq;
using Engine;

namespace Katarai.Wpf.Monitor
{
    public class CompositePlayerNotifier : IPlayerNotifier
    {
        private readonly IPlayerNotifier[] _playerNotifiers;

        public CompositePlayerNotifier(params IPlayerNotifier[] playerNotifiers)
        {
            if (playerNotifiers == null) throw new ArgumentNullException("playerNotifiers");
            var snapshotOfNotifiers = playerNotifiers.ToArray();
            CheckForNullNotifiers(snapshotOfNotifiers);
            _playerNotifiers = snapshotOfNotifiers;
        }

        private static void CheckForNullNotifiers(IEnumerable<IPlayerNotifier> notifiers)
        {
            var nullNotifiers = notifiers.Select((notifier, i) => notifier == null ? string.Format("playerNotifier({0})", i) : null)
                .Where(s => !String.IsNullOrEmpty(s))
                .ToArray();
            if (nullNotifiers.Any())
            {
                var paramName = String.Join(", ", nullNotifiers);
                throw new ArgumentNullException(paramName);
            }
        }

        public void DisplayMessage(string title, string msg, NotifyIcon kataStateIcon, NotifyIcon playerTestStateIcon = NotifyIcon.Green)
        {
            foreach (var notifier in _playerNotifiers)
            {
                notifier.DisplayMessage(title, msg, kataStateIcon, playerTestStateIcon);
            }
        }

        public void DisplayMessage(string title, string msg)
        {
            
        }

        public void DisplayErrorMessage(string errorMessage)
        {
            foreach (var notifier in _playerNotifiers)
            {
                notifier.DisplayErrorMessage(errorMessage);
            }
        }

        public void Dispose()
        {
            foreach (var notifier in _playerNotifiers)
            {
                notifier.Dispose();
            }
        }
    }
}