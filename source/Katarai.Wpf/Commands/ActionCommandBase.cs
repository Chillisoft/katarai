using System;
using System.Windows.Input;

namespace Katarai.Wpf.Commands
{
    public abstract class ActionCommandBase: ICommand
    {
        private Action _toRun;
        Func<bool> _executeCheck;

        protected void Init(Action toRun, Func<bool> executeCheck = null)
        {
            _toRun = toRun;
            _executeCheck = executeCheck ?? AlwaysCanExecute;
        }

        protected void RunHandlersFor(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }


        private bool AlwaysCanExecute()
        {
            return true;
        }

        public bool CanExecute(object parameter)
        {
            return _executeCheck();
        }

        public void Execute(object parameter)
        {
            _toRun();
        }

        public event EventHandler CanExecuteChanged;
    }
}