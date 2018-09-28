using System;

namespace Katarai.Wpf.Commands
{
    public interface IShowAttemptsCommand: IKataraiCommand
    {
    }

    public class ShowAttemptsCommand : IShowAttemptsCommand
    {
        public string Description
        {
            get { return "Show Attempts"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var attemptsView = new Views.AttemptsView();
            attemptsView.Show();
        }

        public event EventHandler CanExecuteChanged;

    }
}