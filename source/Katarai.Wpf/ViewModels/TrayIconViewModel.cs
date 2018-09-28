using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Castle.Core.Internal;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Events;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.ViewModels
{
    public interface ITrayIconViewModel: IViewModel
    {
        ObservableCollection<Control> MenuItems { get; }
        string ToolTipText { get; set; }
    }

    public class TrayIconViewModel: SingleInstanceViewModel, ITrayIconViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IKataPracticeMenuItemProvider _kataPracticeMenuItemProvider;
        private ObservableCollection<Control> _menuItems;
        public ObservableCollection<Control> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyOfPropertyChange();
            }
        }
        public string ToolTipText { get; set; }

        public ICommand ToggleMainWindow { get; private set; }

        public TrayIconViewModel(IEventAggregator eventAggregator, 
                                IKataPracticeMenuItemProvider kataPracticeMenuItemProvider)
        {
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            if (kataPracticeMenuItemProvider == null) throw new ArgumentNullException(nameof(kataPracticeMenuItemProvider));

            _eventAggregator = eventAggregator;
            _kataPracticeMenuItemProvider = kataPracticeMenuItemProvider;
            ToolTipText = "Katarai!";
            SetupMenu();
        }

        private void SetupMenu()
        {
            MenuItems = new ObservableCollection<Control>();

            SetupPracticeMenu();
            AddSeparator();
            AddEventMenuItemFor<AbandonAttemptEvent>("Abandon Attempt");
            AddSeparator();
            SetupStatisticsMenu();
            AddSeparator();
            AddEventMenuItemFor<ShowAboutWindowEvent>("About");
            AddEventMenuItemFor<ExitApplicationEvent>("Exit");
        }

        private void AddStatisticsSubsTo(MenuItem menuItem)
        {
            new[]
            {
                EventMenuItemFor<ShowAttemptsEvent>("Attempts"),
                EventMenuItemFor<ShowCompletedKatasEvent>("Completed Katas"),
                EventMenuItemFor<ShowAttemptsPerWeekEvent>("Attempts Per Week")
            }.ForEach(m => menuItem.Items.Add(m));
        }

        private void SetupStatisticsMenu()
        {
            var parent = new MenuItem()
            {
                Header = "Statistics"
            };
            MenuItems.Add(parent);
            AddStatisticsSubsTo(parent);
        }


        private void SetupPracticeMenu()
        {
            MenuItems.Add(new Label() {FontWeight = FontWeights.Bold, Content = "Practice"});
            _kataPracticeMenuItemProvider.GetKataPracticeMenuItems().ForEach(MenuItems.Add);
        }

        private void AddSeparator()
        {
            MenuItems.Add(new Separator());
        }

        private void AddEventMenuItemFor<TEvent>(string text)
        {
            MenuItems.Add(EventMenuItemFor<TEvent>(text));
        }

        private MenuItem EventMenuItemFor<TEvent>(string text)
        {
            return new MenuItem()
            {
                Header = text,
                Command = new ActionCommand(() => _eventAggregator.Publish<TEvent>())
            };
        }


    }
}
