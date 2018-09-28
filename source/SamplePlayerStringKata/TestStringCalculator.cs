using System;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace SamplePlayerStringKata
{
    [TestFixture]
    public class TestStringCalculator : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            CreateSUT = () => new StringCalculator();
        }

        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => CreateSUT());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Add_GivenEmptyString_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var sut = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.Add("");
            //---------------Test Result -----------------------
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Add_GivenSingleDigitString_ShouldReturnValue()
        {
            //---------------Set up test pack-------------------
            var sut = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.Add("1");
            //---------------Test Result -----------------------
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Add_GivenTwoDigitString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            var sut = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.Add("1,2");
            //---------------Test Result -----------------------
            Assert.That(result, Is.EqualTo(3));
        }
    }
}
