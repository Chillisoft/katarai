using System;
using System.Collections.Generic;
using System.ComponentModel;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.ViewModels
{
    public interface IAttemptsViewModel
    {
        ILogRepository LogRepository { get; }
        List<IAttemptLog> AttemptLogs { get; }
        string LoadingMessage { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }

    public class AttemptsViewModel : INotifyPropertyChanged, IAttemptsViewModel
    {
        private List<IAttemptLog> _attemptLogs;
        private string _loadingMessage;
        public ILogRepository LogRepository { get; private set; }

        public AttemptsViewModel() : this(new LogRepository(new SplunkSearch(new SplunkSettings()), new KataHelper()))
        {
        }

        public AttemptsViewModel(ILogRepository logRepository)
        {
            if (logRepository == null) throw new ArgumentNullException("logRepository");
            LogRepository = logRepository;
            var userName = Environment.UserDomainName + "\\" + Environment.UserName;
            LoadingMessage = "Loading Data...";
            GetKataAttemptLogs(userName);
        }

        public List<IAttemptLog> AttemptLogs
        {
            get { return _attemptLogs; }
            internal set
            {
                _attemptLogs = value;
                OnPropertyChanged("AttemptLogs");
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
            var attemptLogs = await LogRepository.GetKataAttemptLogs(userName, 20);
            AttemptLogs = attemptLogs;
            LoadingMessage = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
