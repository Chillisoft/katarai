using System.Windows;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Views
{
    /// <summary>
    /// Interaction logic for FeedbackWindow.xaml
    /// </summary>
    public partial class FeedbackWindow : Window
    {
        public FeedbackWindow()
        {
            InitializeComponent();
            FeedbackTextBox.Focus();
            var viewModel = this.DataContext as FeedbackViewModel;
            viewModel.RequestClose += (sender, args) => this.Close();
        }
    }
}
