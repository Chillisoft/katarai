using System.Windows;
using System.Windows.Controls.Primitives;
using Hardcodet.Wpf.TaskbarNotification;

namespace Katarai.Controls
{
    public class Toast : IToast
    {
        private readonly TaskbarIcon _taskbarIcon;

        public void CloseBalloon()
        {
            _taskbarIcon.CloseBalloon();
        }

        public void Dispose()
        {
            _taskbarIcon.Dispose();
        }

        public object DataContext
        {
            get { return _taskbarIcon.DataContext; }
        }

        public bool IsDisposed
        {
            get { return _taskbarIcon.IsDisposed; }
        }

        public Toast(TaskbarIcon taskbarIcon)
        {
            _taskbarIcon = taskbarIcon;
        }
        public void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout)
        {
            _taskbarIcon.ShowCustomBalloon(balloon, animation, timeout);
        }
    }
}