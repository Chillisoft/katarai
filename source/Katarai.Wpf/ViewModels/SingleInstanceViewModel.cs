using System;
using System.ComponentModel;
using Caliburn.Micro;
using Katarai.Wpf.Extensions;

namespace Katarai.Wpf.ViewModels
{
    public abstract class SingleInstanceViewModel : Screen, IViewModel
    {
        public virtual void OnClose(object sender, CancelEventArgs eventArgs)
        {
            eventArgs.Cancel = true;
            try
            {
                this.GetWindow().Hide();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Can't hide: " + ex.Message);
            }
        }
    }
}