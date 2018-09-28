using System.Windows;
using System.Windows.Controls;

namespace Katarai.Controls
{
    /// <summary>
    ///     Interaction logic for FeedbackDisplay.xaml
    /// </summary>
    public partial class WelcomeDisplay : UserControl
    {
        private readonly IToast _toast;

        public WelcomeDisplay(IToast toast, string title, string message)
        {
            _toast = toast;
            InitializeComponent();
            Title.Content = title;
            Message.Text = message;
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            _toast.CloseBalloon();
        }
    }
}