using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Caliburn.Micro;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Events;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using Action = System.Action;

namespace Katarai.Wpf.Tests.ViewModels
{
    [TestFixture]
    public class TestTrayIconViewModel
    {
        [Test]
        public void Construct_GivenNullEventAggregator_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new TrayIconViewModel(null, null));

            //---------------Test Result -----------------------
            Assert.AreEqual("eventAggregator", ex.ParamName);
        }

        [Test]
        public void Construct_GivenNullKataPracticeMenuItemProvider_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new TrayIconViewModel(Substitute.For<IEventAggregator>(), null));

            //---------------Test Result -----------------------
            Assert.AreEqual("kataPracticeMenuItemProvider", ex.ParamName);
        }

        [Apartment(ApartmentState.STA)]
        [TestCase("Abandon Attempt", typeof(AbandonAttemptEvent))]
        [TestCase("About", typeof(ShowAboutWindowEvent))]
        [TestCase("Exit", typeof(ExitApplicationEvent))]
        public void Construct_ShouldCreateMenuItem_(string caption, Type expectedEvent)
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            object publishedEvent = null;
            eventAggregator.When(ea => ea.Publish(Arg.Any<object>(), Arg.Any<Action<Action>>()))
                .Do(ci =>
                {
                    publishedEvent = ci.Args()[0];
                });
            var sut = Create(eventAggregator);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var menuItem = sut.MenuItems.OfType<MenuItem>()
                .Single(i => i.Header as string == caption);
            menuItem.Command.Execute(null);

            //---------------Test Result -----------------------
            Assert.IsNotNull(publishedEvent);
            Assert.AreEqual(expectedEvent, publishedEvent.GetType());
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Construct_ShouldAddMenuItemsProvidedBy_KataPracticeMenuItemProvider()
        {
            //---------------Set up test pack-------------------
            var provider = Substitute.For<IKataPracticeMenuItemProvider>();
            var kataArchive = Substitute.For<IKataArchive>();
            var commandHelper = Substitute.For<ICommandHelper>();
            var expected = RandomValueGen.GetRandomCollection(() => new MenuItem()
            {
                Header = RandomValueGen.GetRandomString(),
                Command = new GenerateAndLaunchKataCommand(
                    RandomValueGen.GetRandomEnum<KataName>(),
                    kataArchive,
                    commandHelper,
                    Substitute.For<IEventAggregator>())
            }, 1, 2).ToArray();
            provider.GetKataPracticeMenuItems().Returns(expected);
            var sut = Create(kataPracticeMenuItemProvider: provider);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.MenuItems;

            //---------------Test Result -----------------------
            var kataItems = result.OfType<MenuItem>().Where(m => m.Command is GenerateAndLaunchKataCommand).ToArray();
            CollectionAssert.AreEqual(expected, kataItems);
        }
        
        [Apartment(ApartmentState.STA)]
        [Test]
        public void Construct_ShouldAddStatisticsSubMenu()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var sut = Create(eventAggregator);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.MenuItems.OfType<MenuItem>().FirstOrDefault(mi => mi.Header as string == "Statistics");

            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            result.ShouldHaveSubMenuEmitting<ShowAttemptsEvent>("Attempts", eventAggregator);
            result.ShouldHaveSubMenuEmitting<ShowCompletedKatasEvent>("Completed Katas", eventAggregator);
            result.ShouldHaveSubMenuEmitting<ShowAttemptsPerWeekEvent>("Attempts Per Week", eventAggregator);
        }

        private TrayIconViewModel Create(IEventAggregator eventAggregator = null,
                                            IKataPracticeMenuItemProvider kataPracticeMenuItemProvider = null)
        {
            return new TrayIconViewModel(eventAggregator ?? Substitute.For<IEventAggregator>(),
                kataPracticeMenuItemProvider ?? Substitute.For<IKataPracticeMenuItemProvider>());
        }
    }

    public static class MenuItemTestExtensions
    {
        public static void ShouldHaveSubMenuEmitting<TEvent>(this MenuItem parent, string childText, IEventAggregator testAggregator)
        {
            MenuItem match = null;
            foreach (var item in parent.Items)
            {
                var subMenuItem = item as MenuItem;
                if (subMenuItem == null)
                    continue;
                if (subMenuItem.Header as string == childText)
                {
                    match = subMenuItem;
                    break;
                }
            }
            Assert.IsNotNull(match, $"Could not find a child menu item with the Header '{childText}'");
            object published = null;
            testAggregator.When(ea => ea.Publish(Arg.Any<object>(), Arg.Any<Action<Action>>()))
                .Do(ci => published = ci.Args()[0]);
            match.Command.Execute(null);
            Assert.IsNotNull(published);
            Assert.AreEqual(typeof (TEvent), published.GetType(),
                $"Published event for {childText} was of type {published.GetType()} but {typeof (TEvent)} was expected");
        }
    }
}
