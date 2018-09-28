using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using Katarai.Runner;
using Katarai.Wpf.Events;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.Icons;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using Katarai.Wpf.ViewModels;
using PeanutButter.TrayIcon;
using static Katarai.Wpf.Application;
using Application = System.Windows.Application;
using Action = System.Action;

namespace Katarai.Wpf
{
    public interface IApplicationController:
            IHandle<ExitApplicationEvent>
    {
        void Start();
    }

    public class ApplicationController: IApplicationController
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IConvenientWindowManager _windowManager;
        private readonly IWindowController _windowController;
        private readonly IGameMonitor _gameMonitor;
        private readonly IPlayerNotifier _playerNotifier;
        private readonly ISplunkLogger _splunkLogger;
        private readonly IKataFilesMonitor _kataFilesMonitor;
        private readonly IReminderTimer _reminderTimer;
        private readonly IMonitorTimer _monitorTimer;

        public ApplicationController(IEventAggregator eventAggregator, 
                                        IConvenientWindowManager windowManager,
                                        IWindowController windowController,
                                        IGameMonitor gameMonitor,
                                        IPlayerNotifier playerNotifier,
                                        ISplunkLogger splunkLogger,
                                        IKataFilesMonitor kataFilesMonitor,
                                        IReminderTimer reminderTimer,
                                        IMonitorTimer monitorTimer)
        {
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (windowManager == null) throw new ArgumentNullException(nameof(windowManager));
            if (windowController == null) throw new ArgumentNullException(nameof(windowController));
            if (gameMonitor == null) throw new ArgumentNullException(nameof(gameMonitor));
            if (playerNotifier == null) throw new ArgumentNullException(nameof(playerNotifier));
            if (splunkLogger == null) throw new ArgumentNullException(nameof(splunkLogger));
            if (kataFilesMonitor == null) throw new ArgumentNullException(nameof(kataFilesMonitor));
            if (reminderTimer == null) throw new ArgumentNullException(nameof(reminderTimer));
            if (monitorTimer == null) throw new ArgumentNullException(nameof(monitorTimer));
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _windowController = windowController;
            _gameMonitor = gameMonitor;
            _playerNotifier = playerNotifier;
            _splunkLogger = splunkLogger;
            _kataFilesMonitor = kataFilesMonitor;
            _reminderTimer = reminderTimer;
            _monitorTimer = monitorTimer;
            eventAggregator.Subscribe(this);
        }

        public void Start()
        {
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

            _gameMonitor.StartMonitoring();
            _splunkLogger.Log(new MonitorEvent {Description = "Katarai Started", Logged = DateTime.Now});
            _kataFilesMonitor.Start();
            _reminderTimer.Start();
            _monitorTimer.Start();
        }

        public void Handle(ExitApplicationEvent message)
        {
            ExitApplication();
        }

        private void ExitApplication()
        {
            var shutDownActions = new Action[]
            {
                () => _kataFilesMonitor?.Stop(),
                () => _playerNotifier?.Dispose(),
                () => _gameMonitor?.Dispose(),
                () => _reminderTimer?.Stop(),
                () => _monitorTimer?.Stop(),
            };
            var cancellationTokenSource = new CancellationTokenSource(5000);
            var tasks = shutDownActions
                            .Select(action => Task.Run(() => TryDo(action), cancellationTokenSource.Token))
                            .ToArray();
            Task.WaitAll(tasks);

            System.Windows.Application.Current.Shutdown();
        }

        private void TryDo(Action todo)
        {
            try
            {
                todo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Suppressed exception in wind-down: " + ex.Message);
            }
        }
    }

}
