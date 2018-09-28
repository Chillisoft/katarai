using System;
using System.Windows;

namespace Katarai.Wpf.Commands
{
    public interface IShowMainWindowCommand : IKataraiCommand
    {
    }

    public class ShowMainWindowCommand : IShowMainWindowCommand
    {
        public string Description
        {
            get { return "Show Application Window"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var mainWindow = Application.Current.MainWindow;
            mainWindow.Show();
            mainWindow.Activate();
        }

        public event EventHandler CanExecuteChanged;
    }
}