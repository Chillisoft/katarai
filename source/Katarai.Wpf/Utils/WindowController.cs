using System;
using Caliburn.Micro;
using Katarai.Wpf.Events;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Utils
{
    public interface IWindowController:
                        IHandle<ShowMainWindowEvent>,
                        IHandle<HideMainWindowEvent>,
                        IHandle<ToggleMainWindowEvent>,
                        IHandle<IncrementNotificationEvent>,
                        IHandle<ShowAboutWindowEvent>,
                        IHandle<ShowFeedbackWindowEvent>
    {
    }

    public class WindowController: IWindowController
    {
        private readonly IConvenientWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMainWindowViewModel _mainWindowViewModel;
        private readonly IAboutViewModel _aboutViewModel;
        private readonly IAttemptsPerWeekViewModel _attemptsPerWeekViewModel;
        private readonly IAttemptsViewModel _attemptsViewModel;
        private readonly ICompletedLengthsViewModel _completedLengthsViewModel;
        private readonly IKataCompletedViewModel _kataCompletedViewModel;
        private readonly IReminderSettingsViewModel _reminderSettingsViewModel;
        private readonly IFeedbackViewModel _feedbackViewModel;

        public WindowController(IConvenientWindowManager windowManager, 
                                IEventAggregator eventAggregator,
                                IMainWindowViewModel mainWindowViewModel,
                                IAboutViewModel aboutViewModel,
                                IAttemptsPerWeekViewModel attemptsPerWeekViewModel,
                                IAttemptsViewModel attemptsViewModel,
                                ICompletedLengthsViewModel completedLengthsViewModel,
                                IKataCompletedViewModel kataCompletedViewModel,
                                IReminderSettingsViewModel reminderSettingsViewModel,
                                IFeedbackViewModel feedbackViewModel)
        {
            if (windowManager == null) throw new ArgumentNullException(nameof(windowManager));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (mainWindowViewModel == null) throw new ArgumentNullException(nameof(mainWindowViewModel));
            if (aboutViewModel == null) throw new ArgumentNullException(nameof(aboutViewModel));
            if (attemptsPerWeekViewModel == null) throw new ArgumentNullException(nameof(attemptsPerWeekViewModel));
            if (attemptsViewModel == null) throw new ArgumentNullException(nameof(attemptsViewModel));
            if (completedLengthsViewModel == null) throw new ArgumentNullException(nameof(completedLengthsViewModel));
            if (kataCompletedViewModel == null) throw new ArgumentNullException(nameof(kataCompletedViewModel));
            if (reminderSettingsViewModel == null) throw new ArgumentNullException(nameof(reminderSettingsViewModel));
            if (feedbackViewModel == null) throw new ArgumentNullException(nameof(feedbackViewModel));
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _mainWindowViewModel = mainWindowViewModel;
            _aboutViewModel = aboutViewModel;
            _attemptsPerWeekViewModel = attemptsPerWeekViewModel;
            _attemptsViewModel = attemptsViewModel;
            _completedLengthsViewModel = completedLengthsViewModel;
            _kataCompletedViewModel = kataCompletedViewModel;
            _reminderSettingsViewModel = reminderSettingsViewModel;
            _feedbackViewModel = feedbackViewModel;

            _eventAggregator.Subscribe(this);
        }

        public void Handle(ShowMainWindowEvent message)
        {
            _windowManager.RaiseWindow(_mainWindowViewModel);
        }

        public void Handle(HideMainWindowEvent message)
        {
            _windowManager.HideWindow(_mainWindowViewModel);
        }

        public void Handle(ToggleMainWindowEvent message)
        {
            _windowManager.ToggleWindow(_mainWindowViewModel);
        }

        public void Handle(IncrementNotificationEvent message)
        {
            _mainWindowViewModel.NotificationVisibilityTimeSeconds++;
        }

        public void Handle(ShowAboutWindowEvent message)
        {
            _windowManager.RaiseWindow(_aboutViewModel);
        }

        public void Handle(ShowFeedbackWindowEvent message)
        {
            _windowManager.RaiseWindow(_feedbackViewModel);
        }
    }
}
