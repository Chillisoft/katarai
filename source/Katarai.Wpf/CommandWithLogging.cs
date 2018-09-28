using System;
using System.Windows.Input;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf
{
    public interface IKataraiCommand : ICommand
    {
        string Description { get; }
    }

    public interface ICommandWithLogging : ICommand
    {
        IKataraiCommand WrappedCommand { get; }
    }

    public class CommandWithLogging : ICommandWithLogging
    {
        private readonly IKataraiCommand _command;
        private readonly CommandLogger _commandLogger;

        public CommandWithLogging(IKataraiCommand command)
            : this(command, new CommandLogger(new SplunkLogger(new SplunkAppender(new SplunkSettings()), new KataHelper())))
        {
        }

        public CommandWithLogging(IKataraiCommand command, CommandLogger commandLogger)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandLogger == null) throw new ArgumentNullException("commandLogger");
            _command = command;
            _commandLogger = commandLogger;
        }

        public IKataraiCommand WrappedCommand
        {
            get { return _command; }
        }

        public bool CanExecute(object parameter)
        {
            return WrappedCommand.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { WrappedCommand.CanExecuteChanged += value; }
            remove { WrappedCommand.CanExecuteChanged -= value; }
        }

        public void Execute(object parameter)
        {
            _commandLogger.LogCommandExecuted(WrappedCommand.Description);
            WrappedCommand.Execute(parameter);
        }
    }
}