using System.ComponentModel;
using System.Reflection;
using Caliburn.Micro;

namespace Katarai.Wpf.ViewModels
{
    public interface IAboutViewModel: IViewModel
    {
        string ApplicationVersion { get; }
    }

    public class AboutViewModel: SingleInstanceViewModel, IAboutViewModel
    {
        public string ApplicationVersion { get; private set; }

        public AboutViewModel()
        {
            this.ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}