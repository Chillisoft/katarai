using System.ComponentModel;

namespace Katarai.Wpf.ViewModels
{
    public interface IKataCompletedViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;
        string Message { get; set; }
    }

    public class KataCompletedViewModel : INotifyPropertyChanged, IKataCompletedViewModel
    { 
        private string _message;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }


        private void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}