using System;

namespace Katarai.Wpf.Commands
{
    public interface IShowAttemptsPerWeekCommand : IKataraiCommand
    {
    }

    public class ShowAttemptsPerWeekCommand : IShowAttemptsPerWeekCommand
    {
        public string Description
        {
            get { return " Show Attempts Per Week"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var attemptsPerWeekView = new Views.AttemptsPerWeekView();
            attemptsPerWeekView.Show();
        }

        public event EventHandler CanExecuteChanged;

    }
}