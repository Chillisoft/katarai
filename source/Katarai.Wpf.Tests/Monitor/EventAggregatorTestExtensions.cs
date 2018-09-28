using System;
using Caliburn.Micro;
using NSubstitute;
using Action = System.Action;

namespace Katarai.Wpf.Tests.Monitor
{
    public static class EventAggregatorTestExtensions
    {
        public static void ShouldHavePublished<T>(this IEventAggregator eventAggregator, Func<T,bool> matcher = null, int howMany = 1)
        {
            matcher = matcher ?? (o => true);
            eventAggregator.Received(howMany).Publish(Arg.Is<T>(item => matcher(item)), Arg.Any<Action<Action>>());
        }

        public static void ShouldNotHavePublished<T>(this IEventAggregator eventAggregator, Func<T,bool> matcher = null)
        {
            eventAggregator.ShouldHavePublished(matcher, 0);
        }
    }
}