    
using System;
using Engine.Runners;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Runners
{
    [TestFixture]
    public class TestPlayerImplementationRunner
    {
        // ReSharper disable InconsistentNaming
        [Test]
        public void Constructor_GivenNullPlayerImplementationType_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new PlayerImplementationRunner<IStringCalculator>(null,GetType()));
            //---------------Test Result -----------------------
            Assert.AreEqual("playerImplementationType", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullGoldenTestType_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new PlayerImplementationRunner<IStringCalculator>(GetType(),null));
            //---------------Test Result -----------------------
            Assert.AreEqual("goldenTestType", exception.ParamName);
        }
    }
}