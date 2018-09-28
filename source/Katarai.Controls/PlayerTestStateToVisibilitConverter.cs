using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using Engine;

namespace Katarai.Controls
{
    public class PlayerTestStateToVisibilitConverter : IValueConverter
    {
        private const string Green = "Green";
        private const string Red = "Red";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var notifyIcon = (NotifyIcon)value;
            if (parameter != null && (notifyIcon.Equals(NotifyIcon.Green) && parameter.ToString().Equals(Green)))
            {
                return Visibility.Visible;
            }
            if (parameter != null && (notifyIcon.Equals(NotifyIcon.Green) && parameter.ToString().Equals(Red)))
            {
                return Visibility.Collapsed;
            }
            if (parameter != null && (notifyIcon.Equals(NotifyIcon.Red) && parameter.ToString().Equals(Green)))
            {
                return Visibility.Collapsed;
            }
            if (parameter != null && (notifyIcon.Equals(NotifyIcon.Red) && parameter.ToString().Equals(Red)))
            {
                return Visibility.Visible;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}