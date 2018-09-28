using System;

namespace Katarai.Wpf.Extensions
{
    public class ImmediateDispatcher: IDispatcher
    {
        public void Invoke(Action action)
        {
            action();
        }
    }
}