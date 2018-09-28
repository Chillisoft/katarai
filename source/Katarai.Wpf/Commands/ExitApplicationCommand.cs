using System;
using System.Windows;
using System.Windows.Input;

namespace Katarai.Wpf.Commands
{
    public interface IExitApplicationCommand : ICommand
    {
    }

    public class ExitApplicationCommand : IExitApplicationCommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }

        public event EventHandler CanExecuteChanged;
    }
}