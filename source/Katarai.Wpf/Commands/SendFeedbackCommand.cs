using System;
using System.Windows.Input;

namespace Katarai.Wpf.Commands
{
    public interface ISendFeedbackCommand : ICommand
    {
    }

    public class SendFeedbackCommand : ISendFeedbackCommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var feedbackWindow = new Views.FeedbackWindow();
            feedbackWindow.Show();
        }

        public event EventHandler CanExecuteChanged;
    }
}