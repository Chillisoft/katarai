using System.Windows.Controls;

namespace Katarai.Wpf.ExtensionMethods
{
    public static class FileBrowserExtensionMethod
    {
        public static string OpenFileBrowserForLocation(this TextBox textBoxToUse, string defaultExt, string filter, bool allowMultiSelect = false)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog { DefaultExt = defaultExt, Filter = filter, Multiselect = allowMultiSelect };
            var fileName = dlg.ShowDialog();

            var result =  fileName != true ? textBoxToUse.Text : dlg.FileName;
            textBoxToUse.Text = result;

            return result;
        }
    }
}
