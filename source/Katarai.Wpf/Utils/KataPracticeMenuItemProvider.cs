using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Katarai.Wpf.Commands;
using Katarai.Wpf.PackagedKata;

namespace Katarai.Wpf.Utils
{
    public interface IKataPracticeMenuItemProvider
    {
        IEnumerable<MenuItem> GetKataPracticeMenuItems();
    }

    public class KataPracticeMenuItemProvider: IKataPracticeMenuItemProvider
    {
        private readonly IKataArchive _kataArchive;
        private readonly ICommandHelper _commandHelper;
        private readonly IEventAggregator _eventAggregator;

        public KataPracticeMenuItemProvider(IKataArchive kataArchive, 
                                            ICommandHelper commandHelper,
                                            IEventAggregator eventAggregator)
        {
            if (kataArchive == null) throw new ArgumentNullException(nameof(kataArchive));
            if (commandHelper == null) throw new ArgumentNullException(nameof(commandHelper));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            _kataArchive = kataArchive;
            _commandHelper = commandHelper;
            _eventAggregator = eventAggregator;
        }

        public IEnumerable<MenuItem> GetKataPracticeMenuItems()
        {
            return new[]
            {
                MenuItemFor(KataName.FizzBuzz, "FizzBuzz"),
                MenuItemFor(KataName.StringCalculator, "String Calculator")
            };
        }

        private MenuItem MenuItemFor(KataName kataName, string title)
        {
            return new MenuItem()
            {
                Header = title,
                Command = new GenerateAndLaunchKataCommand(kataName, _kataArchive, _commandHelper, _eventAggregator)
            };
        }
    }

}
