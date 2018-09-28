using System;

namespace Katarai.Wpf.Extensions
{
    public class AutoDispatcher: IDispatcher
    {
        public IDispatcher Actual
        {
            get { return _actual; }
        }
        private IDispatcher _actual;

        public AutoDispatcher()
        {
            _actual = Application.Current != null && Application.Current.Dispatcher != null 
                ? new DispatcherFacade(Application.Current.Dispatcher) as IDispatcher
                : new ImmediateDispatcher() as IDispatcher;
        }

        public void Invoke(Action action)
        {
            _actual.Invoke(action);
        }
    }
}