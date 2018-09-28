using System;
using System.Windows;

namespace Katarai.Wpf.Commands
{
    public interface IHideMainWindowCommand : IKataraiCommand
    {
    }

    public class HideMainWindowCommand : IHideMainWindowCommand
    {
        public string Description
        {
            get { return "Hide Application Window"; }
        }

        public bool CanExecute(object parameter)
        {
            return Application.Current.MainWindow != null;

        }

        public void Execute(object parameter)
        {
            Application.Current.MainWindow.Hide();
        }

        public event EventHandler CanExecuteChanged;
    }
}