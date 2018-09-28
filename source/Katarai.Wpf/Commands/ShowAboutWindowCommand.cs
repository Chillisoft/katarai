using System;
using System.Windows.Input;

namespace Katarai.Wpf.Commands
{
    public interface IShowAboutWindowCommand : ICommand
    {
    }

    public class ShowAboutWindowCommand : IShowAboutWindowCommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var aboutView = new Views.AboutView();
            aboutView.Show();
        }

        public event EventHandler CanExecuteChanged;
    }
}