using System;
using Engine;
using Katarai.FizzBuzz.Interfaces;
using NUnit.Framework;

namespace PlayerFizzBuzz
{
    [TestFixture]
    public class TestFizzBuzz : ITestPack<IFizzBuzz>
    {
        public Func<IFizzBuzz> CreateSUT { get; set; }

	    [TestFixtureSetUp]
	    public void FixtureSetup()
	    {
	        CreateSUT = () => new FizzBuzz();
	    }

        private IFizzBuzz CreateFizzBuzz()
        {
            return CreateSUT();
        }

        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Arrange-------------------

            //---------------Act----------------------
            Assert.DoesNotThrow(() => CreateFizzBuzz());
            //---------------Assert -----------------------
        }

    }
}
