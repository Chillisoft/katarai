using System;

namespace Katarai.Wpf.Commands
{
    public interface IAbandonAttemptCommand : IKataraiCommand
    {
        event EventHandler EndPractice;
    }

    public class AbandonAttemptCommand : ActionCommandBase, IAbandonAttemptCommand
    {
        public event EventHandler EndPractice;

        public AbandonAttemptCommand()
        {
            Init(() => RunHandlersFor(EndPractice));
        }

        public string Description { get; }
    }
}