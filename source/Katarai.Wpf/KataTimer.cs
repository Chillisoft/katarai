using System;
using System.Windows.Threading;
using Katarai.Controls;

namespace Katarai.Wpf
{
    public class KataTimer : IKataTimer
    {
        private string _kataDuration;
        private DateTime _startTime;
        internal DispatcherTimer Timer { get; private set; }

        public KataTimer()
        {
            _startTime = DateTime.MinValue;
            Timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            Timer.Tick += TimerOnTick;
            SetKataDuration(0);
        }

        public void StartTimer()
        {
            Timer.Start();
            _startTime = DateTime.Now;
        }

        public void StopTimer()
        {
            Timer.Stop();
            KataEndTime = DateTime.Now;
            var timeElapsed = (KataEndTime.GetValueOrDefault() - StartTime).TotalSeconds;
            SetKataDuration(timeElapsed);
        }

        public void ResetTimer()
        {
            Timer.Stop();
            SetKataDuration(0);
            KataEndTime = null;
        }

        public DateTime? KataEndTime { get; set; }
        public string KataDuration
        {
            get { return _kataDuration; }
            private set
            {
                _kataDuration = value;
                if (KataDurationChanged != null)
                {
                    KataDurationChanged(this, KataDuration);
                }
            }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var timeElapsed = (DateTime.Now - StartTime).TotalSeconds;
            SetKataDuration(timeElapsed);
        }

        private void SetKataDuration(double timeElapsedInSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(timeElapsedInSeconds);
            this.KataDuration = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);
        }
        public event EventHandler<string> KataDurationChanged;

    }
}