using System.Windows;
using System.Windows.Controls.Primitives;

namespace Katarai.Controls
{
    public interface IToast
    {
        void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout);
        void CloseBalloon();
        void Dispose();
        object DataContext { get; }
    }
}