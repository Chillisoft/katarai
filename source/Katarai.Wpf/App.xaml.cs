using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using Katarai.Controls;
using Katarai.Runner;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using UpdateBootstrapperLibrary;
using UpdateBootstrapperLibrary.Contracts;
using UpdateBootstrapperLibrary.Services;
using UpdateBootstrapperLibrary.WrapperObjects;

namespace Katarai.Wpf
{
    public partial class App
    {
        private IKataFilesMonitor _kataFilesMonitor;
        private PlayerNotifier _playerNotifier;
        private ILogger _splunkLogger;
        private KataraiApp _kataraiApp;
        private GameMonitor _gameMonitor;
        private ReminderTimer _reminderTimer;
        private MonitorTimer _monitorTimer;
        private UpdateProcess _updateProcess;

        protected override void OnStartup(StartupEventArgs e)
        {
            MsiInstallerBootstrap(e);

            base.OnStartup(e);
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            BootstrapMainWindows();
            //BootstrapAutoUpdate();
            Bootstrap();

        }

        private void MsiInstallerBootstrap(StartupEventArgs e)
        {
            if (e.Args.Length > 0 && e.Args[0] == "InstallerBootstrap")
            {
                var serviceFactory = new ServiceFactory();
                var settingService = serviceFactory.CreateSettingsService();
                var rootInstallDirectory = settingService.FetchAppRootInstallDirectory();
                var exeName = settingService.FetchAppExeName();
                var exePath = Path.Combine(rootInstallDirectory, exeName);
                var processService = serviceFactory.CreateProcessService();

                var lanuchableProcess = processService.CreateInstalledAppLaunchableProcess(exePath, rootInstallDirectory);

                lanuchableProcess.Start();

                Application.Current.Shutdown();
            }
        }

        private void BootstrapAutoUpdate()
        {
            var serviceFactory = new ServiceFactory();
            var timerService = serviceFactory.CreateTimerService();
            var httpService = serviceFactory.CreateHttpService();
            var processService = serviceFactory.CreateProcessService();
            var conversionService = serviceFactory.CreateConversionService();
            var settingService = serviceFactory.CreateSettingsService();
            
            _updateProcess = new UpdateProcess(timerService, httpService, processService, conversionService, settingService);

            var args = CreateUpdateProcessArgs(settingService, conversionService);

            _updateProcess.Start(args);
        }

        private UpdateProcessArgs CreateUpdateProcessArgs(ISettingsService settingsService, IConversionService conversionService)
        {
            var updateUrl = settingsService.FetchValue("UpdateUrl");
            var pollIntervalHours = settingsService.FetchValue("PollIntervalHours");
            var appVersion = settingsService.FetchAppVersion();

            var args = new UpdateProcessArgs
            {
                AppVersion = conversionService.ConvertVersionToString(appVersion),
                PollIntervalHours = conversionService.ConvertToInt(pollIntervalHours),
                UpdateUrl = updateUrl,
                ExitMainAppAction = new ActionWrapper(() => { Current.Shutdown(1); })
            };

            return args;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            if (eventArgs.Exception == null) return;
            MessageBox.Show("An exception has occurred: " + eventArgs.Exception.Message);
        }

        private void BootstrapMainWindows()
        {
            Current.MainWindow = new Views.MainWindow();
        }

        private void Bootstrap()
        {
            _kataraiApp = new KataraiApp();

            var mainWindow = Current.MainWindow as Views.MainWindow;
            if (mainWindow == null) return;

            var playerNotifier = GetPlayerNotifier(mainWindow);
            if (playerNotifier == null) return;

            _playerNotifier = playerNotifier;

            mainWindow.ViewModel.KataraiApp = _kataraiApp;
            mainWindow.ViewModel.PlayerNotifier = _playerNotifier;

            _playerNotifier.DisplayMessage("Welcome to Katarai",
                "Katarai is a tool to help you do code katas. In this version the supported katas are Roy Osherove’s String Calculator kata." +
                "To the katas, select Practice from the System Tray menu – Katarai will open Visual Studio with a prepared solution to get you started and will track your progress as you code." +
                Environment.NewLine + Environment.NewLine +
                "Why is TDD important? It allows us to break the negative feedback loop and maintain a constant cost of change." +
                "Only by actively driving down the defects in our code are we able to deliver new functionality, and modify existing functionality with a reasonably constant cost of change." +
                Environment.NewLine + Environment.NewLine +
                "To configure Katarai’s options click Show Window in the System Tray menu." + Environment.NewLine +
                Environment.NewLine +
                "To view your previous kata attempt statistics, use the Statistics menu in the System Tray menu."
                );

            StartSplunkLogging();

            StartReminderTimer();
            StartMonitorTimer();
            StartGameMonitor();

            StartFileMonitor();
        }

        private void StartGameMonitor()
        {
            _gameMonitor = new GameMonitor(_kataraiApp);
            _gameMonitor.StartMonitoring();
        }

        private void StartSplunkLogging()
        {
            _splunkLogger = new SplunkLogger(new SplunkAppender(), new KataHelper());
            _splunkLogger.Log(new MonitorEvent {Description = "Katarai Started", Logged = DateTime.Now});
        }

        private void StartFileMonitor()
        {
            _kataFilesMonitor = KataMonitorFactory.GenerateKataFilesMonitor(_playerNotifier, SettingsManager.Instance,
                new AnalysisRunnerFactory(), _kataraiApp);
            _kataFilesMonitor.Start();
        }

        private void StartReminderTimer()
        {
            _reminderTimer = new ReminderTimer(_splunkLogger, SettingsManager.Instance);
            _reminderTimer.Start();
        }

        private void StartMonitorTimer()
        {
            _monitorTimer = new MonitorTimer(_splunkLogger);
            _monitorTimer.Start();
        }

        private PlayerNotifier GetPlayerNotifier(Views.MainWindow mainWindow)
        {
            PlayerNotifier playerNotifier = null;
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            var notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");
            if (notifyIcon == null) return null;
            notifyIcon.Icon = Icons.Resources.Logo;
            var toast = new Toast(notifyIcon);
            var taskbarIconAdapter = new ToastDisplayer(toast);

            playerNotifier = new PlayerNotifier(taskbarIconAdapter);
            notifyIcon.DataContext = mainWindow.ViewModel;
            return playerNotifier;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Terminate();

            base.OnExit(e);
        }

        private void Terminate()
        {
            _kataFilesMonitor?.Stop();
            _playerNotifier?.Dispose();
            _updateProcess?.Stop();
            _gameMonitor?.Dispose();
            _reminderTimer?.Stop();
            _monitorTimer?.Stop();

            var mainWindow = Current.MainWindow;

            mainWindow?.Close();
        }
    }
}