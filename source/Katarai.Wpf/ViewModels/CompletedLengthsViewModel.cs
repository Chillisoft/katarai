using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;
using OxyPlot;
using DateTimeAxis = OxyPlot.Axes.DateTimeAxis;


namespace Katarai.Wpf.ViewModels
{
    public interface ICompletedLengthsViewModel
    {
        ILogRepository LogRepository { get; }
        string Title { get; }
        List<IAttemptLog> AttemptLogs { get; }
        List<DataPoint> StringCalculatorDataPoints { get; }
        List<DataPoint> FizzBuzzDataPoints { get; }
        string LoadingMessage { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }

    public class CompletedLengthsViewModel : INotifyPropertyChanged, ICompletedLengthsViewModel
    {
        private List<IAttemptLog> _attemptLogs;
        private string _loadingMessage;
        private List<DataPoint> _stringCalculatorDataPoints;
        private List<DataPoint> _fizzBuzzDataPoints;
        public ILogRepository LogRepository { get; private set; }

        public CompletedLengthsViewModel() : this(new LogRepository(new SplunkSearch(new SplunkSettings()), new KataHelper()))
        {
        }

        public CompletedLengthsViewModel(ILogRepository logRepository)
        {
            if (logRepository == null) throw new ArgumentNullException("logRepository");
            LogRepository = logRepository;
            this.Title = "Completed Durations";
            var userName = Environment.UserDomainName + "\\" + Environment.UserName;
            LoadingMessage = "Loading Data...";
            GetKataAttemptLogs(userName);
        }

        public string Title { get; private set; }

        public List<IAttemptLog> AttemptLogs
        {
            get { return _attemptLogs; }
            internal set
            {
                _attemptLogs = value;
                OnPropertyChanged("AttemptLogs");
                SetDataPoints();
                LoadingMessage = "";
            }
        }

        public List<DataPoint> StringCalculatorDataPoints
        {
            get { return _stringCalculatorDataPoints; }
            internal set
            {
                _stringCalculatorDataPoints = value;
                OnPropertyChanged("StringCalculatorDataPoints");
            }
        }
        
        public List<DataPoint> FizzBuzzDataPoints
        {
            get { return _fizzBuzzDataPoints; }
            internal set
            {
                _fizzBuzzDataPoints = value;
                OnPropertyChanged("FizzBuzzDataPoints");
            }
        }
        
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            internal set
            {
                _loadingMessage = value;
                OnPropertyChanged("LoadingMessage");
            }
        }

        private async void GetKataAttemptLogs(string userName)
        {
            var attemptLogs = await LogRepository.GetKataCompletedAttemptLogs(userName);
            AttemptLogs = attemptLogs;
            LoadingMessage = "";
        }
        
        private void SetDataPoints()
        {
            if (_attemptLogs == null) return;
            var stringCalculatorAttemptLogs = _attemptLogs.Where(log => log.KataName == KataName.StringCalculator.ToString());
            var fizzBuzzAttemptLogs = _attemptLogs.Where(log => log.KataName == KataName.FizzBuzz.ToString());
            StringCalculatorDataPoints = GetDataPoints(stringCalculatorAttemptLogs);
            FizzBuzzDataPoints = GetDataPoints(fizzBuzzAttemptLogs);
        }

        private List<DataPoint> GetDataPoints(IEnumerable<IAttemptLog> attemptLogs)
        {
            var dataPoints = new List<DataPoint>();
            foreach (var attemptLog in attemptLogs)
            {
                var dataPoint = DateTimeAxis.CreateDataPoint(attemptLog.AttemptDate.GetValueOrDefault(),
                    Convert.ToDouble(attemptLog.LengthInMinutes));
                dataPoints.Add(dataPoint);
            }
            return dataPoints;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}