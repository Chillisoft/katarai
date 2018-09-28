using System.ComponentModel;
using Caliburn.Micro;

namespace Katarai.Wpf.ViewModels
{
    public interface IViewModel : IScreen, IViewAware
    {
        void OnClose(object sender, CancelEventArgs eventArgs);
    }
}