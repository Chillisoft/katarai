using System;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Katarai.Wpf.Extensions
{
    public static class EventAggregatorExtensions
    {
        public static Task Publish<T>(this IEventAggregator eventAggregator, params object[] eventParameters)
        {
            var ev = (T) Activator.CreateInstance(typeof(T), eventParameters);
            // publishes in the background but is awaitable. UI consumers still
            //  need to invoke a dispatcher
            return eventAggregator.PublishOnUIThreadAsync(ev);
        }
    }
}
