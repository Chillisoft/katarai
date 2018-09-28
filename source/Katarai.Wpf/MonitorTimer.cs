using System;
using System.Windows.Threading;
using Katarai.Runner;

namespace Katarai.Wpf
{
    public interface IMonitorTimer
    {
        void Start();
        void Stop();
        bool IsTimerRunning();
    }

    public class MonitorTimer : IMonitorTimer
    {
        private readonly ISplunkLogger _splunkLogger;
        private DispatcherTimer _timer;

        public MonitorTimer(ISplunkLogger splunkLogger)
        {
            if (splunkLogger == null) throw new ArgumentNullException("splunkLogger");
            _splunkLogger = splunkLogger;
            SetupTimer();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public bool IsTimerRunning()
        {
            return _timer.IsEnabled;
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer { Interval = new TimeSpan(6, 0, 0) };
            _timer.Tick += ((sender, e1) => LogEvent());
        }

        private void LogEvent()
        {
            _splunkLogger.Log(new MonitorEvent { Description = "Katarai ticking", Logged = DateTime.Now });
        }
    }
}