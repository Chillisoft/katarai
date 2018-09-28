using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Engine;

namespace Katarai.Controls
{
    public class FeedbackDisplayViewModel : INotifyPropertyChanged
    {
        private string _message;
        private string _title;
        private string _kataDuration;
        private IKataTimer _kataTimer;
        private double _progressLevel;
        private NotifyIcon _kataState;
        private NotifyIcon _playerTestState;

        public ICommand NavigateToUrlCommand { get; set; }

        public string Url
        {
            get { return "http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Message
        {
            get { return _message; }
            set
            {
                var message =
                    @"<html><head><style type=""text/css"">body { font-size: 12px; font-family:'Century Gothic'; color:#352063;} p {width: 400px;word-wrap: normal;} .hint{font-weight: bold;}</style></head><body><p>";
                message += value;
                message = message.Replace("Uncle Bob’s Three Rules of TDD",
                    "<a href='http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd'>Uncle Bob’s Three Rules of TDD</a>");
                message = message.Replace("(http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd)", "");
                message = message.Replace("Hint:", @"<div class=""hint"">Hint: </div>");
                message += "</p></body></html>";
                _message = message;

                OnPropertyChanged("Message");
            }
        }

        public IKataTimer KataTimer
        {
            get { return _kataTimer; }
            set
            {
                if (_kataTimer != null)
                {
                    _kataTimer.KataDurationChanged -= KataTimerOnKataDurationChanged;
                }
                _kataTimer = value;
                if (_kataTimer != null)
                {
                    this.KataDuration = _kataTimer.KataDuration;
                    _kataTimer.KataDurationChanged += KataTimerOnKataDurationChanged;
                }
            }
        }

        private void KataTimerOnKataDurationChanged(object sender, string kataDuration)
        {
            KataDuration = kataDuration;
        }

        public string KataDuration
        {
            get { return _kataDuration; }
            set
            {
                _kataDuration = value;
                OnPropertyChanged("KataDuration");
            }
        }

        public double ProgressLevel
        {
            get { return _progressLevel; }
            set
            {
                _progressLevel = value;
                OnPropertyChanged("ProgressLevel");
            }
        }

        public NotifyIcon KataState
        {
            get { return _kataState; }
            set
            {
                _kataState = value;
                OnPropertyChanged("KataState");
            }
        }


        public FeedbackDisplayViewModel() : this(new NavigateToUrlCommand())
        {
        }

        public FeedbackDisplayViewModel(INavigateToUrlCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            NavigateToUrlCommand = command;
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public NotifyIcon PlayerTestState
        {
            get { return _playerTestState; }
            set
            {
                _playerTestState = value;
                OnPropertyChanged("PlayerTestState");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        
    }
}