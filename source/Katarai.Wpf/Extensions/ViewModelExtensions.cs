using System.Windows;
using Katarai.Wpf.ViewModels;

namespace Katarai.Wpf.Extensions
{
    public static class ViewModelExtensions
    {
        public static Window GetWindow(this IViewModel viewModel)
        {
            return viewModel.GetView() as Window;
        }

        public static bool IsVisible(this IViewModel viewModel)
        {
            if (!viewModel.IsActive)
                return false;
            var window = viewModel.GetWindow();
            return window?.IsVisible ?? false;
        }
    }
}
