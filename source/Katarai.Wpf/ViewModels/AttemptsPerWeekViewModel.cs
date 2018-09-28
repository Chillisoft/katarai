using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.ViewModels
{
    public interface IAttemptsPerWeekViewModel
    {
        ILogRepository LogRepository { get; }
        string Title { get; }
        List<IAttemptLog> AttemptLogs { get; }
        List<WeekAttempt> StringCalculatorDataPoints { get; }
        List<WeekAttempt> FizzBuzzDataPoints { get; }
        List<WeekAttempt> WeekAttempts { get; }
        string LoadingMessage { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }

    public class AttemptsPerWeekViewModel : INotifyPropertyChanged, IAttemptsPerWeekViewModel
    {
        private List<IAttemptLog> _attemptLogs;
        private string _loadingMessage;
        private List<WeekAttempt> _weekAttempts;
        private List<WeekAttempt> _stringCalculatorDataPoints;
        private List<WeekAttempt> _fizzBuzzDataPoints;
        public ILogRepository LogRepository { get; private set; }

        public AttemptsPerWeekViewModel() : this(new LogRepository(new SplunkSearch(new SplunkSettings()), new KataHelper()))
        {
        }

        public AttemptsPerWeekViewModel(ILogRepository logRepository)
        {
            if (logRepository == null) throw new ArgumentNullException("logRepository");
            LogRepository = logRepository;
            this.Title = "Attempts Per Week";
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

        public List<WeekAttempt> StringCalculatorDataPoints
        {
            get { return _stringCalculatorDataPoints; }
            internal set
            {
                _stringCalculatorDataPoints = value;
                OnPropertyChanged("StringCalculatorDataPoints");
            }
        }
        
        public List<WeekAttempt> FizzBuzzDataPoints
        {
            get { return _fizzBuzzDataPoints; }
            internal set
            {
                _fizzBuzzDataPoints = value;
                OnPropertyChanged("FizzBuzzDataPoints");
            }
        }
        public List<WeekAttempt> WeekAttempts
        {
            get { return _weekAttempts; }
            internal set
            {
                _weekAttempts = value;
                OnPropertyChanged("WeekAttempts");
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
            var attemptLogs = await LogRepository.GetKataAttemptLogs(userName);
            AttemptLogs = attemptLogs;
            LoadingMessage = "";
        }

        private void SetDataPoints()
        {
            if (_attemptLogs == null) return;
            var stringCalculatorAttemptLogs = _attemptLogs.Where(log => log.KataName == KataName.StringCalculator.ToString());
            var fizzBuzzAttemptLogs = _attemptLogs.Where(log => log.KataName == KataName.FizzBuzz.ToString());
            var stringCalculatorAttempts = stringCalculatorAttemptLogs.GroupBy(log => log.AttemptDate.GetValueOrDefault().Date);
            var fizzBuzzAttempts = fizzBuzzAttemptLogs.GroupBy(log => log.AttemptDate.GetValueOrDefault().Date);
            var stringCalculatorDataPoints = GetDayAttempts(stringCalculatorAttempts);
            var fizzBuzzDataPoints = GetDayAttempts(fizzBuzzAttempts);
            var dayAttempts = new List<DayAttempt>();
            dayAttempts.AddRange(stringCalculatorDataPoints);
            dayAttempts.AddRange(fizzBuzzDataPoints);
            var weekAttempts = GetWeeks();
            var stringCalcWeekAttempts = CreateWeekAttempts(weekAttempts, stringCalculatorDataPoints);
            var fizzBuzzWeekAttempts = CreateWeekAttempts(weekAttempts, fizzBuzzDataPoints);

            var orderBy = weekAttempts.OrderBy(attempt => attempt.WeekStartDate);
            WeekAttempts = new List<WeekAttempt>(orderBy);
            StringCalculatorDataPoints = new List<WeekAttempt>(stringCalcWeekAttempts.OrderBy(attempt => attempt.WeekStartDate));
            FizzBuzzDataPoints = new List<WeekAttempt>(fizzBuzzWeekAttempts.OrderBy(attempt => attempt.WeekStartDate));
        }

        private List<WeekAttempt> CreateWeekAttempts(List<WeekAttempt> weekAttempts, List<DayAttempt> dayAttempts)
        {
            var kataAttempts = new List<WeekAttempt>();
            foreach (var weekAttempt in weekAttempts)
            {
                var attempt1 = weekAttempt;
                var attempts = dayAttempts.Where(attempt =>
                    attempt.AttemptDate >= attempt1.WeekStartDate &&
                    attempt.AttemptDate <= attempt1.WeekEndDate).ToList();
                var weekAttempt1 = new WeekAttempt
                {
                    WeekStartDate = weekAttempt.WeekStartDate,
                    WeekEndDate = weekAttempt.WeekEndDate,
                    TotalAttempts = attempts.Sum(dayAttempt => dayAttempt.TotalAttempts),
                    CompletedAttempts = attempts.Sum(attempt => attempt.CompletedAttempts)
                };
                kataAttempts.Add(weekAttempt1);
            }
            return kataAttempts;
        }

        private List<WeekAttempt> GetWeeks()
        {
            var weekAttempts = new List<WeekAttempt>();
            var dateTime = DateTime.Today;
            for (int i = 0; i < 4; i++)
            {
                var weekAttempt = GetWeek(dateTime);
                dateTime = weekAttempt.WeekStartDate.AddDays(-7);
                weekAttempts.Add(weekAttempt);
            }
            return weekAttempts;
        }

        private WeekAttempt GetWeek(DateTime dateTime)
        {
            var firstDateOfWeek = dateTime.GetFirstDayOfWeek();
            var lastDateOfWeek = firstDateOfWeek.AddDays(6);
            return new WeekAttempt{WeekStartDate = firstDateOfWeek, WeekEndDate = lastDateOfWeek};
        }

        private static List<DayAttempt> GetDayAttempts(IEnumerable<IGrouping<DateTime, IAttemptLog>> stringCalculatorAttempts)
        {
            var dayAttempts = new List<DayAttempt>();
            foreach (var attempt in stringCalculatorAttempts)
            {
                var totalAttempts = attempt.Count();
                var completedAttempts = attempt.Count(log => log.Completed);
                dayAttempts.Add(new DayAttempt
                {
                    AttemptDate = attempt.Key,
                    TotalAttempts = totalAttempts,
                    CompletedAttempts = completedAttempts
                });
            }
            return dayAttempts;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DayAttempt
    {
        public DateTime AttemptDate { get; set; }
        public int TotalAttempts{ get; set; }
        public int CompletedAttempts{ get; set; }
    }

    public class WeekAttempt
    {
        public string WeekDates  
        {
            get { return WeekStartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "-" + WeekEndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); }
        }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public int TotalAttempts{ get; set; }
        public int CompletedAttempts{ get; set; }
    }

    public static class DateTimeExtension
    {
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            //var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            const DayOfWeek firstDayOfWeek = DayOfWeek.Sunday;

            while (date.DayOfWeek != firstDayOfWeek)
            {
                date = date.AddDays(-1);
            }
            return date;
        }
    }
}