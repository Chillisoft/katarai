using System.Windows;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for KataCompletedView.xaml
    /// </summary>
    public partial class KataCompletedView : Window
    {

        public KataCompletedView()
        {
            InitializeComponent();
        }

        private KataCompletedViewModel ViewModel
        {
            get { return DataContext as KataCompletedViewModel; }
        }

        public void SetMessage(string message)
        {
            ViewModel.Message = message;
        }


        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}