using System;

namespace Katarai.Wpf.Commands
{
    public interface IShowCompletedKatasCommand : IKataraiCommand
    {
    }

    public class ShowCompletedKatasCommand : IShowCompletedKatasCommand
    {
        public string Description
        {
            get { return "Show Completed Katas"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var completedLengthsView = new Views.CompletedLengthsView();
            completedLengthsView.Show();
        }

        public event EventHandler CanExecuteChanged;
    }
}