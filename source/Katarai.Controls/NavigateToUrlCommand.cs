using System;

namespace Katarai.Controls
{
    public class NavigateToUrlCommand:INavigateToUrlCommand
    {
        private readonly IStartProcess _launcher;

        internal IStartProcess Launcher
        {
            get { return _launcher; }
        }

        public NavigateToUrlCommand():this(new ProcessLauncher())
        {
            
        }

        public NavigateToUrlCommand(IStartProcess launcher)
        {
            _launcher = launcher;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
           _launcher.StartProcess(parameter.ToString());
        }

        public event EventHandler CanExecuteChanged;
    }
}