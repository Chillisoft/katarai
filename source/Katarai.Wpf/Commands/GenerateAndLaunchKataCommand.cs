using System;
using Caliburn.Micro;
using Katarai.Wpf.Events;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.Commands
{

    public interface IGenerateAndLaunchKataCommand : IKataraiCommand
    {
        // TODO: kill this event handler -- replace with event aggregator (has been started)
        event EventHandler<KataAttemptEventArgs> KataAttemptCreated;
    }

    public class GenerateAndLaunchKataCommand : IGenerateAndLaunchKataCommand
    {
        public IKataArchive KataArchive { get { return _kataArchive; } }
        public ICommandHelper CommandHelper { get { return _commandHelper; } }
        public IEventAggregator EventAggregator { get { return _eventAggregator; } }

        private readonly IKataArchive _kataArchive;
        private readonly ICommandHelper _commandHelper;
        private readonly IEventAggregator _eventAggregator;
        public KataName Kata { get { return _kata; } }
        private KataName _kata;
        public string Description
        {
            get { return "Generate And Launch Kata"; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public GenerateAndLaunchKataCommand(KataName kata, 
                                            IKataArchive kataArchive,
                                            ICommandHelper commandHelper,
                                            IEventAggregator eventAggregator)
        {
            if (kataArchive == null) throw new ArgumentNullException(nameof(kataArchive));
            if (commandHelper == null) throw new ArgumentNullException(nameof(commandHelper));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            _kata = kata;
            _kataArchive = kataArchive;
            _commandHelper = commandHelper;
            _eventAggregator = eventAggregator;
        }

        public void Execute(object parameter)
        {
            var kataAttempt = parameter == null
                                ? _commandHelper.GenerateAndLaunchKata(_kata, _kataArchive)
                                : _commandHelper.GenerateAndLaunchKata(parameter, _kataArchive);
            _eventAggregator.Publish<KataAttemptCreatedEvent>(kataAttempt);
            if (KataAttemptCreated != null)
            {
                KataAttemptCreated(this, new KataAttemptEventArgs(kataAttempt));
            }
        }
        
        public event EventHandler<KataAttemptEventArgs> KataAttemptCreated;
        public event EventHandler CanExecuteChanged;
    }

    public class KataAttemptEventArgs:EventArgs
    {
        public IKataAttempt KataAttempt { get; private set; }

        public KataAttemptEventArgs(IKataAttempt kataAttempt)
        {
            KataAttempt = kataAttempt;
        }
    }
}