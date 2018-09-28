using System;
using Katarai.Runner;

namespace Katarai.Wpf
{
    public class CommandLogger
    {
        private readonly SplunkLogger _splunkLogger;

        public CommandLogger(SplunkLogger splunkLogger)
        {
            if (splunkLogger == null) throw new ArgumentNullException("splunkLogger");
            _splunkLogger = splunkLogger;
        }

        public void LogCommandExecuted(string command)
        {
            var commandEvent = new CommandEvent {Description = command, Logged = DateTime.Now};
            _splunkLogger.Log(commandEvent);
        }
    }
}