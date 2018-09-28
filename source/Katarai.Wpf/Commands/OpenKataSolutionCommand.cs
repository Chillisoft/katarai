using System;
using Caliburn.Micro;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.Commands
{
    public interface IOpenKataSolutionCommand : IKataraiCommand
    {
    }

    public class OpenKataSolutionCommand : IOpenKataSolutionCommand
    {
        private readonly IKataArchive _kataArchive;

        public OpenKataSolutionCommand(IKataArchive kataArchive)
        {
            if (kataArchive == null) throw new ArgumentNullException(nameof(kataArchive));
            _kataArchive = kataArchive;
        }

        public string Description
        {
            get { return "Open Kata Solution"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var kataAttempt = parameter as KataAttempt;
            if (kataAttempt == null) return;
            var launchAction = _kataArchive.GenerateLaunchActionFor(kataAttempt.Location);
            launchAction.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}