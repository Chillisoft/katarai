using System;
using Caliburn.Micro;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Events;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Tests.Monitor;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Commands
{
    [TestFixture]
    public class TestGenerateAndLaunchKataCommand
    {
        [Test]
        public void Construct_WhenNullArchive_ExpectANE()
        {
            //---------------Set up test pack-------------------
            var commandHelper = Substitute.For<ICommandHelper>();
            var eventAggregator = Substitute.For<IEventAggregator>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new GenerateAndLaunchKataCommand(KataName.FizzBuzz, null, commandHelper, eventAggregator));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_WhenNullCommandHelper_ExpectANE()
        {
            //---------------Set up test pack-------------------
            var kataArchive = Substitute.For<IKataArchive>();
            var eventAggregator = Substitute.For<IEventAggregator>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new GenerateAndLaunchKataCommand(KataName.FizzBuzz, kataArchive, null, eventAggregator));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_WhenNullEventAgreegator_ExpectANE()
        {
            //---------------Set up test pack-------------------
            var kataArchive = Substitute.For<IKataArchive>();
            var commandHelper = Substitute.For<ICommandHelper>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.Throws<ArgumentNullException>(() => new GenerateAndLaunchKataCommand(KataName.FizzBuzz, kataArchive, commandHelper, null));
            //---------------Test Result -----------------------
        }

        [TestCase(null)]
        [TestCase("foo")]
        public void Execute_ShouldEmitKataAttemptCreatedEvent_WithKataAttemptProvidedByCommandHelper(string parameter)
        {
            //---------------Set up test pack-------------------
            var commandHelper = Substitute.For<ICommandHelper>();
            var kataArchive = Substitute.For<IKataArchive>();
            var eventAggregator = Substitute.For<IEventAggregator>();
            var sut = Create(commandHelper:commandHelper, kataArchive:kataArchive, eventAggregator:eventAggregator);
            var expected = Substitute.For<IKataAttempt>();
            commandHelper.GenerateAndLaunchKata(parameter ?? (object)KataName.Unknown, kataArchive).Returns(expected);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.Execute(parameter);

            //---------------Test Result -----------------------
            commandHelper.Received(1).GenerateAndLaunchKata(parameter ?? (object)KataName.Unknown, kataArchive);
            eventAggregator.ShouldHavePublished<KataAttemptCreatedEvent>(ev => ev.KataAttempt == expected);
        }

        private GenerateAndLaunchKataCommand Create(KataName kataName = KataName.Unknown,
                                                    IKataArchive kataArchive = null,
                                                    ICommandHelper commandHelper = null,
                                                    IEventAggregator eventAggregator = null)
        {
            return new GenerateAndLaunchKataCommand(kataName,
                kataArchive ?? Substitute.For<IKataArchive>(),
                commandHelper ?? Substitute.For<ICommandHelper>(),
                eventAggregator ?? Substitute.For<IEventAggregator>());
        }
    }

}
