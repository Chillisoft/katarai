using System;
using System.Windows;
using Caliburn.Micro;
using Katarai.Wpf.Events;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.PackagedKata;

namespace Katarai.Wpf.Utils
{
    public interface ICommandHelper
    {
        IKataAttempt GenerateAndLaunchKata(object parameter, IKataArchive kataArchive);
    }

    public class CommandHelper : ICommandHelper
    {
        private readonly ITraceLoggerHelper _traceLoggerHelper;
        private readonly IEventAggregator _eventAggregator;

        public CommandHelper(ITraceLoggerHelper traceLoggerHelper,
                                IEventAggregator eventAggregator)
        {
            if (traceLoggerHelper == null) throw new ArgumentNullException(nameof(traceLoggerHelper));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            _traceLoggerHelper = traceLoggerHelper;
            _eventAggregator = eventAggregator;
        }

        public IKataAttempt GenerateAndLaunchKata(object parameter, IKataArchive kataArchive)
        {
            KataName selectedKata;

            var kataNameString = parameter.ToString();
            if (Enum.TryParse(kataNameString, true, out selectedKata))
            {
                return GenerateAndLaunchKata(kataArchive, selectedKata);
            }
            else
            {
                _traceLoggerHelper.LogToUi("Unable to find Kata type [ " + kataNameString + " ]");
                return null;
            }
        }

        private IKataAttempt GenerateAndLaunchKata(IKataArchive kataArchive, KataName selectedKata)
        {
            var kataAttempt = kataArchive.GenerateSolutionForAttempt(selectedKata);

            if (kataAttempt == null)
            {
                _traceLoggerHelper.LogToUi("Failed to find solution template for [ " + kataArchive + " ]");
                return null;
            }

            var launchAction = kataArchive.GenerateLaunchActionFor(kataAttempt.Location);

            launchAction.Invoke();
            _eventAggregator.Publish<ShowMainWindowEvent>();
            return kataAttempt;
        }
    }
}
