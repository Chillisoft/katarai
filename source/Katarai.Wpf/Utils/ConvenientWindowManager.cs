using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Utils
{
    public interface IConvenientWindowManager: IWindowManager
    {
        void RaiseWindow(IViewModel viewModel);
        void ToggleWindow(IViewModel viewModel);
        void HideWindow(IViewModel viewModel);
        void LaunchHiddenWindow(IViewModel viewModel);
    }

    public class ConvenientWindowManager: WindowManager, IConvenientWindowManager
    {
        private IDispatcher _dispatcher = new AutoDispatcher();
        private List<IViewModel> _closeAttachedViewModels = new List<IViewModel>();

        public void RaiseWindow(IViewModel viewModel)
        {
            _dispatcher.Invoke(() =>
            {
                if (viewModel.IsActive)
                {
                    var window = viewModel.GetView() as Window;
                    if (window == null)
                        return;
                    if (!window.IsVisible)
                        window.Show();
                    window.Activate();
                }
                else
                {
                    ShowWindow(viewModel);
                    if (!_closeAttachedViewModels.Contains(viewModel))
                        viewModel.ViewAttached += (s, e) =>
                        {
                            var window = viewModel.GetWindow();
                            if (window == null)
                                return;
                            window.Closing += viewModel.OnClose;
                        };
                }
            });
        }

        public void ToggleWindow(IViewModel viewModel)
        {
            _dispatcher.Invoke(() =>
            {
                if (viewModel.IsVisible())
                {
                    System.Diagnostics.Debug.WriteLine("Hide window...");
                    HideWindowNoDispatcher(viewModel);
                }
                else
                {
                    RaiseWindow(viewModel);
                }
            });
        }

        public void HideWindow(IViewModel viewModel)
        {
            _dispatcher.Invoke(() =>
            {
                HideWindowNoDispatcher(viewModel);
            });
        }

        private void HideWindowNoDispatcher(IViewModel viewModel)
        {
            var window = viewModel.GetWindow();
            if (window == null)
                viewModel.TryClose();
            else
                window.Hide();
        }

        public void LaunchHiddenWindow(IViewModel viewModel)
        {
            var window = CreateWindow(viewModel, false, null, null);
            window.Hide();
        }

    }
}
