using System;
using System.Windows.Threading;

namespace Katarai.Wpf.Extensions
{
    public class DispatcherFacade : IDispatcher
    {
        private Dispatcher _actual;

        public DispatcherFacade(Dispatcher actual)
        {
            _actual = actual;
        }

        public void Invoke(Action action)
        {
            _actual.Invoke(action);
        }
    }
}