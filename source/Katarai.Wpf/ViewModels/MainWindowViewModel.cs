using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using Caliburn.Micro;
using Katarai.Controls;
using Katarai.Runner;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Events;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using ICommand = System.Windows.Input.ICommand;

namespace Katarai.Wpf.ViewModels
{
    public interface IMainWindowViewModel: IViewModel,
        IHandle<DisplayFeedbackEvent>,
        IHandle<KataAttemptCreatedEvent>,
        IHandle<KataAttemptAbandonedEvent>
    {
        IList<IKataAttempt> Katas { get; }
        ICommand GenerateKataSolution { get; }
        ICommand SendFeedbackCommand { get; }
        ICommand ShowWindowCommand { get; }
        ICommand HideWindowCommand { get; }
        ICommand ExitApplicationCommand { get; }
        ICommand ShowAboutWindowCommand { get; }
        ICommand OpenKataSolutionCommand { get; }
        ICommand ShowReminderSettingsCommand { get; }
        ICommand ShowAttemptsCommand { get; }
        ICommand ShowCompletedKatasCommand { get; }
        ICommand ShowAttemptsPerWeekCommand { get; }
        ICommand AbandonAttemptCommand { get; }
        IKataTimer KataTimer { get; }
        AttemptGameState AttemptGameState { get; }
        bool IsAlwaysOnTop { get; set; }
        IKataAttempt SelectedKataAttempt { get; set; }
        string KataPath { get; }
        string PlayerPath { get; }
        bool ShouldShowHint { get; set; }
        IKataraiApp KataraiApp { get; set; }
        int NotificationVisibilityTimeSeconds { get; set; }
        string KataDuration { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }


    public class MainWindowViewModel : SingleInstanceViewModel, IToggleShowHint, IMainWindowViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IKataAttemptRepository _attemptRepository;
        private IKataAttempt _selectedKataAttempt;
        private readonly ISettingsManager _settingsManager;
        private bool _shouldShowHint;
        private bool _isAlwaysOnTop;
        private int _notificationVisibilityTimeSeconds;
        private string _kataDuration;
        private IKataraiApp _kataraiApp;
        private readonly IPlayerNotifier _playerNotifier;

        public IList<IKataAttempt> Katas { get; private set; }
        public ICommand GenerateKataSolution { get; private set; }
        public ICommand SendFeedbackCommand { get; private set; }
        public ICommand ShowWindowCommand { get; private set; }
        public ICommand HideWindowCommand { get; private set; }
        public ICommand ExitApplicationCommand { get; private set; }
        public ICommand ShowAboutWindowCommand { get; private set; }
        public ICommand OpenKataSolutionCommand { get; private set; }
        public ICommand ShowReminderSettingsCommand { get; private set; }
        public ICommand ShowAttemptsCommand { get; private set; }
        public ICommand ShowCompletedKatasCommand { get; private set; }
        public ICommand ShowAttemptsPerWeekCommand { get; private set; }
        public ICommand AbandonAttemptCommand { get; private set; }
        public IKataTimer KataTimer { get; private set; }

        public ObservableCollection<MenuItem> KataMenuItems { get; private set; }
        public ObservableCollection<string> FeedbackItems { get; private set; } 

        public AttemptGameState AttemptGameState
        {
            get { return this.KataraiApp != null ? KataraiApp.AttemptGameState : null; }
        }

        public MainWindowViewModel(
            IEventAggregator eventAggregator,
            IKataraiApp kataraiApp,
            IPlayerNotifier playerNotifier,
            IKataAttemptRepository attemptRepository, 
            IGenerateAndLaunchKataCommand generateKataCommand,
            ISendFeedbackCommand sendFeedbackCommand, 
            IShowMainWindowCommand showMainWindowCommand, 
            IHideMainWindowCommand hideMainWindowCommand,
            IExitApplicationCommand exitApplicationCommand, 
            ISettingsManager settingsManager,
            IOpenKataSolutionCommand openKataSolutionCommand, 
            IShowReminderSettingsCommand showReminderSettingsCommand,
            IShowAttemptsCommand showAttemptsCommand, 
            IShowCompletedKatasCommand showCompletedKatasCommand,
            IShowAttemptsPerWeekCommand showAttemptsPerWeekCommand, 
            IAbandonAttemptCommand abandonAttemptCommand, 
            IShowAboutWindowCommand showAboutWindowCommand)
        {
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (kataraiApp == null) throw new ArgumentNullException(nameof(kataraiApp));
            if (playerNotifier == null) throw new ArgumentNullException(nameof(playerNotifier));
            if (attemptRepository == null) throw new ArgumentNullException(nameof(attemptRepository));
            if (generateKataCommand == null) throw new ArgumentNullException(nameof(generateKataCommand));
            if (sendFeedbackCommand == null) throw new ArgumentNullException(nameof(sendFeedbackCommand));
            if (showMainWindowCommand == null) throw new ArgumentNullException(nameof(showMainWindowCommand));
            if (hideMainWindowCommand == null) throw new ArgumentNullException(nameof(hideMainWindowCommand));
            if (exitApplicationCommand == null) throw new ArgumentNullException(nameof(exitApplicationCommand));
            if (openKataSolutionCommand == null) throw new ArgumentNullException(nameof(openKataSolutionCommand));
            if (showReminderSettingsCommand == null) throw new ArgumentNullException(nameof(showReminderSettingsCommand));
            if (showAttemptsCommand == null) throw new ArgumentNullException(nameof(showAttemptsCommand));
            if (showCompletedKatasCommand == null) throw new ArgumentNullException(nameof(showCompletedKatasCommand));
            if (showAttemptsPerWeekCommand == null) throw new ArgumentNullException(nameof(showAttemptsPerWeekCommand));
            if (abandonAttemptCommand == null) throw new ArgumentNullException(nameof(abandonAttemptCommand));
            if (showAboutWindowCommand == null) throw new ArgumentNullException(nameof(showAboutWindowCommand));
            _eventAggregator = eventAggregator;
            _kataraiApp = kataraiApp;
            _playerNotifier = playerNotifier;
            _attemptRepository = attemptRepository;
            _settingsManager = settingsManager;
            ShowReminderSettingsCommand = WrapWithLogging(showReminderSettingsCommand);
            ShowAttemptsCommand = WrapWithLogging(showAttemptsCommand);
            ShowCompletedKatasCommand = WrapWithLogging(showCompletedKatasCommand);
            ShowAttemptsPerWeekCommand = WrapWithLogging(showAttemptsPerWeekCommand);
            AbandonAttemptCommand = WrapWithLogging(abandonAttemptCommand);
            GenerateKataSolution = WrapWithLogging(generateKataCommand);
            RegisterNewKataAttemptEvent();
            SendFeedbackCommand = sendFeedbackCommand;
            ShowWindowCommand = WrapWithLogging(showMainWindowCommand);
            HideWindowCommand = WrapWithLogging(hideMainWindowCommand);
            ExitApplicationCommand = exitApplicationCommand;
            ShowAboutWindowCommand = showAboutWindowCommand;
            OpenKataSolutionCommand = WrapWithLogging(openKataSolutionCommand);

            Katas = _attemptRepository.GetKataInfos();
            LoadSettings();
            IsAlwaysOnTop = settingsManager.IsAlwaysOnTopOn();
            ShouldShowHint = settingsManager.IsShowHintOn();
            NotificationVisibilityTimeSeconds = settingsManager.GetNotificationVisibilityTime();
            RegisterAbandonAttemptEvent();

            Initialize();
            KataMenuItems = new ObservableCollection<MenuItem>();
            FeedbackItems = new ObservableCollection<string>();
            SetupKataMenuItems();
            _eventAggregator.Subscribe(this);
        }

        private void SetupKataMenuItems()
        {
            KataMenuItems.Add(new MenuItem()
                {
                    Header = "String Kata",
                    Command = new ActionCommand(() => GenerateKataSolution.Execute("StringCalculator"))
                });
        }

        private void Initialize()
        {
            this.DisplayName = string.Format("Katarai.{0}", Assembly.GetExecutingAssembly().GetName().Version);
            SetCompanyLogo();
            SetWindowIcon();
            ConfigureTraceLogger();
        }

        private void SetCompanyLogo()
        {
            using (Bitmap bmp = Icons.Resources.ChilliLogo.ToBitmap())
            {
                var stream = new MemoryStream();
                bmp.Save(stream, ImageFormat.Png);
                // TODO: fix company logo
                //LogoImage.Source = BitmapFrame.Create(stream);
            }
        }

        private void SetWindowIcon()
        {
            using (Bitmap bmp = Icons.Resources.Logo.ToBitmap())
            {
                var stream = new MemoryStream();
                bmp.Save(stream, ImageFormat.Png);
                // TODO: fix window icon
                //Icon = BitmapFrame.Create(stream);
            }
        }

        private void ConfigureTraceLogger()
        {
            // TODO: fix this to not depend on having access to the window
            //var loggingListBox = KataFeedbackListBox;
            //_traceListener = new ListBoxTraceLogger(loggingListBox);
            //Trace.Listeners.Add(_traceListener);
        }

        public void ClearFeedback()
        {
            // TODO: implement feedback clearing
        }

        private CommandWithLogging WrapWithLogging(IKataraiCommand kataraiCommand)
        {
            return new CommandWithLogging(kataraiCommand);
        }

        private void RegisterAbandonAttemptEvent()
        {
            var abandonAttemptCommand = ((CommandWithLogging)AbandonAttemptCommand).WrappedCommand as AbandonAttemptCommand;
            if (abandonAttemptCommand == null) return;
            abandonAttemptCommand.EndPractice += (sender, args) =>
            {
                Handle(new KataAttemptAbandonedEvent());    // todo: remove this event handler altogether
            };
        }

        private void RegisterNewKataAttemptEvent()
        {
            var generateAndLaunchKataCommand = ((CommandWithLogging)GenerateKataSolution).WrappedCommand as IGenerateAndLaunchKataCommand;
            if (generateAndLaunchKataCommand == null) return;
            generateAndLaunchKataCommand.KataAttemptCreated += (sender, args) =>
            {
                Handle(new KataAttemptCreatedEvent(args.KataAttempt));
            };
        }

        private string GetInstruction(IKataAttempt kataAttempt)
        {
            var instructions = kataAttempt.Config.Instructions;
            if (instructions == null) return string.Empty;
            var content = string.Join(Environment.NewLine, instructions.Content);
            return instructions.Header + Environment.NewLine + content;
        }

        private void LoadSettings()
        {
            _settingsManager.LoadSettings();
        }

        public bool IsAlwaysOnTop
        {
            get { return _isAlwaysOnTop; }
            set
            {
                _isAlwaysOnTop = value;
                var settings = _settingsManager.FetchCurrentSettings();
                settings.IsAlwaysOnTop = _isAlwaysOnTop;
                SaveSettings(settings);
                NotifyOfPropertyChange();
            }
        }

        public IKataAttempt SelectedKataAttempt
        {
            get { return _selectedKataAttempt; }
            set
            {
                _selectedKataAttempt = value;
                if (_kataraiApp != null) _kataraiApp.SetCurrentKataAttempt(_selectedKataAttempt);
                NotifyOfPropertyChange();
            }
        }
        
        private void SaveSettings(KataraiSettings settings)
        {
            if (_settingsManager.PersistSettings(settings)) return;
            RaiseErrorNotification("Your Assembly Paths Where Not Saved");
        }

        private void RaiseErrorNotification(string message)
        {
            //if (ErrorNotification != null) ErrorNotification(this, new MessageEventArgs(message));
        }
        

        public string KataPath
        {
            get { return _attemptRepository.GetMasterSolutionAssemblyPath(SelectedKataAttempt); }
        }

        public string PlayerPath
        {
            get { return _attemptRepository.GetPlayerSolutionAssemblyPath(SelectedKataAttempt); }
        }

        public bool ShouldShowHint
        {
            get { return _shouldShowHint; }
            set
            {
                _shouldShowHint = value;
                var settings = _settingsManager.FetchCurrentSettings();
                settings.ShowHint = _shouldShowHint;
                SaveSettings(settings);
                NotifyOfPropertyChange();
            }
        }

        public IKataraiApp KataraiApp
        {
            get { return _kataraiApp; }
            set
            {
                //TODO mark 01 Apr 2015: Sori - to write tests
                if (_kataraiApp != null)
                {
                    _kataraiApp.AttemptGameStateChanged -= KataraiApp_OnAttemptGameStateChanged;
                }
                _kataraiApp = value;
                if (_kataraiApp != null)
                {
                    _kataraiApp.AttemptGameStateChanged += KataraiApp_OnAttemptGameStateChanged;
                }
            }
        }

        private void KataraiApp_OnAttemptGameStateChanged(object sender, EventArgs eventArgs)
        {
            //TODO mark 01 Apr 2015: Sori - to write tests
            var kataraiApp = _kataraiApp;
            if (kataraiApp == null) return;
            if (KataTimer != null)
            {
                KataTimer.KataDurationChanged -= KataTimer_OnKataDurationChanged;
            }
            KataTimer = kataraiApp.AttemptGameState.KataTimer;
            if (KataTimer != null)
            {
                KataTimer.KataDurationChanged += KataTimer_OnKataDurationChanged;
            }
        }

        private void KataTimer_OnKataDurationChanged(object sender1, string kataDuration)
        {
            KataDuration = kataDuration;
        }

        public int NotificationVisibilityTimeSeconds
        {
            get { return _notificationVisibilityTimeSeconds; }
            set
            {
                _notificationVisibilityTimeSeconds = value;
                var settings = _settingsManager.FetchCurrentSettings();
                settings.NotificationVisibility = _notificationVisibilityTimeSeconds;
                SaveSettings(settings);
                NotifyOfPropertyChange();
            }
        }

        public string KataDuration
        {
            get { return _kataDuration; }
            set
            {
                _kataDuration = value;
                NotifyOfPropertyChange();
            }
        }


        public void Handle(DisplayFeedbackEvent message)
        {
            lock(FeedbackItems)
            {
                FeedbackItems.Add(message.Message);
            }
        }

        public void Handle(KataAttemptCreatedEvent message)
        {
            var newKataAttempt = message.KataAttempt;
            Katas.Add(newKataAttempt);
            SelectedKataAttempt = newKataAttempt;
            _playerNotifier.DisplayMessage(
                string.Format("Starting {0} Kata...",newKataAttempt.Config.KataName),
                GetInstruction(newKataAttempt));
        }

        public void Handle(KataAttemptAbandonedEvent message)
        {
            if (KataTimer == null) return;
            KataTimer.StopTimer();
            if (KataraiApp != null)
            {
                KataraiApp.SetAttemptAbandoned();
            }
        }
    }
}