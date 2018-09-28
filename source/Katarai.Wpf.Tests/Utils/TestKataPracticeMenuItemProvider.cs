using System;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using Katarai.Wpf.Commands;
using Katarai.Wpf.PackagedKata;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace Katarai.Wpf.Tests.Utils
{
    [TestFixture]
    public class TestKataPracticeMenuItemProvider
    {
        [Test]
        public void Type_ShouldImplement_IKataPracticeMenuItemProvider()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (KataPracticeMenuItemProvider);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IKataPracticeMenuItemProvider>();

            //---------------Test Result -----------------------
        }

        [Apartment(ApartmentState.STA)]
        [TestCase(KataName.FizzBuzz, "FizzBuzz")]
        [TestCase(KataName.StringCalculator, "String Calculator")]
        public void GetKataPracticeMenuItems_UntilDynamicModuleLoadingIsImplemented_ShouldProvideMenuItemFor_(KataName expectedName, string expectedTitle)
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = sut.GetKataPracticeMenuItems();

            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any(i => (i.Command as GenerateAndLaunchKataCommand).Kata == expectedName &&
                                            i.Header as string == expectedTitle));
        }

        private IKataPracticeMenuItemProvider Create(IKataArchive kataArchive = null,
                                                        ICommandHelper commandHelper = null,
                                                        IEventAggregator eventAggregator = null)
        {
            return new KataPracticeMenuItemProvider(kataArchive ?? Substitute.For<IKataArchive>(),
                                                    commandHelper ?? Substitute.For<ICommandHelper>(),
                                                    eventAggregator ?? Substitute.For<IEventAggregator>());
        }
    }
}
