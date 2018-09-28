using System;

namespace Katarai.Wpf.Commands
{
    public interface ILogFeedbackCommand : IKataraiCommand
    {
        event EventHandler FeedbackLogged;
    }

    public class LogFeedbackCommand : ILogFeedbackCommand
    {
        private readonly ISplunkLogger _splunkLogger;
        
        public string Description
        {
            get { return "Log Feedback"; }
        }

        public LogFeedbackCommand(ISplunkLogger splunkLogger)
        {
            if (splunkLogger == null) throw new ArgumentNullException("splunkLogger");
            _splunkLogger = splunkLogger;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var message = parameter as string;
            if (string.IsNullOrEmpty(message)) return;
            _splunkLogger.LogFeedback(message);
            if (FeedbackLogged != null)
            {
                FeedbackLogged(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler FeedbackLogged;
    }
}