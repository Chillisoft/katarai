using System.Windows;

namespace Katarai.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void Dismiss_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
