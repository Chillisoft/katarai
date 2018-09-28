using System.Windows;
using System.Windows.Media;
using  Engine;

namespace Katarai.Controls
{
    /// <summary>
    ///     Interaction logic for FeedbackDisplay.xaml
    /// </summary>
    public partial class FeedbackDisplay
    {
        private readonly IToast _toast;

        public FeedbackDisplay(IToast toast)
        {
            _toast = toast;
            InitializeComponent();
        }

        private FeedbackDisplayViewModel ViewModel
        {
            get { return DataContext as FeedbackDisplayViewModel; }
        }

        public void SetTitle(string title)
        {
            ViewModel.Title = title;
        }

        public void SetMessage(string message)
        {
            ViewModel.Message = message;
        }

        public void SetKataTimer(IKataTimer kataTimer)
        {
            ViewModel.KataTimer = kataTimer;
        }


        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            _toast.CloseBalloon();
        }

        public void SetFeedbackType(string icon)
        {
            
        }

        public void SetProgressLevel(double progressLevel)
        {
            this.ProgressLevel.Foreground = progressLevel > 50 ? Brushes.White : new SolidColorBrush(Color.FromArgb(0xFF,0x35,0x20,0x63));
            ViewModel.ProgressLevel = progressLevel;
        }

        public void SetKataState(NotifyIcon kataState)
        {
            ViewModel.KataState = kataState;
        }

        public void SetPlayerTestState(NotifyIcon playerTestState)
        {
            ViewModel.PlayerTestState = playerTestState;
        }
    }
}