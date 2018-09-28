using System;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Katarai.Controls;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using Katarai.Wpf.Utils;
using Katarai.Wpf.ViewModels;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TestContainerConfigurator
    {
        [TestCase(typeof(IAboutViewModel), typeof(AboutViewModel))]
        [TestCase(typeof(IAttemptsPerWeekViewModel), typeof(AttemptsPerWeekViewModel))]
        [TestCase(typeof(IAttemptsViewModel), typeof(AttemptsViewModel))]
        [TestCase(typeof(ICompletedLengthsViewModel), typeof(CompletedLengthsViewModel))]
        [TestCase(typeof(IKataCompletedViewModel), typeof(KataCompletedViewModel))]
        [TestCase(typeof(IMainWindowViewModel), typeof(MainWindowViewModel))]
        [TestCase(typeof(IReminderSettingsViewModel), typeof(ReminderSettingsViewModel))]
        public void Configure_ShouldRegisterMultiInstance_ViewModelFor_(Type interfaceType, Type expected)
        {
            //---------------Set up test pack-------------------
            var container = new SimpleContainer();
            var sut = Create();

            //---------------Assert Precondition----------------
            Assert.IsNull(container.GetInstance(interfaceType, null));

            //---------------Execute Test ----------------------
            sut.Configure(container);
            var result = container.GetInstance(interfaceType, null);
            var otherResult = container.GetInstance(interfaceType, null);

            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.GetType());
            Assert.AreNotEqual(result, otherResult);
        }

        [TestCase(typeof(IEventAggregator), typeof(EventAggregator))]
        [TestCase(typeof(IConvenientWindowManager), typeof(ConvenientWindowManager))]
        [TestCase(typeof(ISettingsManager), typeof(SettingsManager))]
        [TestCase(typeof(IKataraiApp), typeof(KataraiApp))]
        [TestCase(typeof(IPlayerNotifier), typeof(PlayerNotifier))]
        [TestCase(typeof(IToast), typeof(Toast))]

        public void Configure_ShouldRegisterSingletonFor_(Type interfaceType, Type concreteType)
        {
            //---------------Set up test pack-------------------
            var container = new SimpleContainer();
            var sut = Create();

            //---------------Assert Precondition----------------
            Assert.IsNull(container.GetInstance(interfaceType, null));

            //---------------Execute Test ----------------------
            sut.Configure(container);
            var result = container.GetInstance(interfaceType, null);
            var secondResult = container.GetInstance(interfaceType, null);

            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), concreteType);
            Assert.AreEqual(result, secondResult);
        }

        private ContainerConfigurator Create()
        {
            return new ContainerConfigurator();
        }
    }

    [TestFixture]
    public class TestEventAggregatorExtensions
    {

        public class SimpleEvent
        {
        }

        public class SimpleEventHandler : IHandle<SimpleEvent>
        {
            public SimpleEvent ReceivedMessage { get; set; }

            public void Handle(SimpleEvent message)
            {
                ReceivedMessage = message;
            }
        }

        [Test]
        public async Task Publish_UsingGenericInterfaceWithNoArguments_ShouldPublishAwaitable()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var handler = new SimpleEventHandler();
            sut.Subscribe(handler);

            //---------------Assert Precondition----------------
            Assert.IsNull(handler.ReceivedMessage);

            //---------------Execute Test ----------------------
            await sut.Publish<SimpleEvent>();

            //---------------Test Result -----------------------
            Assert.IsNotNull(handler.ReceivedMessage);
        }

        public class ComplexEvent
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public ComplexEvent(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        public class ComplexEventHandler : IHandle<ComplexEvent>
        {
            public ComplexEvent ReceivedMessage { get; set; }

            public void Handle(ComplexEvent message)
            {
                ReceivedMessage = message;
            }
        }


        [Test]
        public async Task Publish_UsingGenericInterfaceWithArguments_ShouldPublishAwaitable()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var handler = new ComplexEventHandler();
            sut.Subscribe(handler);
            var id = RandomValueGen.GetRandomInt();
            var name = RandomValueGen.GetRandomString();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            await sut.Publish<ComplexEvent>(id, name);

            //---------------Test Result -----------------------
            Assert.IsNotNull(handler.ReceivedMessage);
            Assert.AreEqual(id, handler.ReceivedMessage.Id);
            Assert.AreEqual(name, handler.ReceivedMessage.Name);
        }


        private IEventAggregator Create()
        {
            return new EventAggregator();
        }
    }
}
