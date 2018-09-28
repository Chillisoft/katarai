using System.Windows;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ReminderSettings.xaml
    /// </summary>
    public partial class ReminderSettings : Window
    {
        public ReminderSettings()
        {
            InitializeComponent();
            var viewModel = this.DataContext as ReminderSettingsViewModel;
            viewModel.RequestClose += (sender, args) => this.Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
