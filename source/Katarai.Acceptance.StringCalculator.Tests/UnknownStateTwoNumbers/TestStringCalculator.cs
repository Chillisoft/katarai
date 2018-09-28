using System;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.StringCalculator.Tests.UnknownStateTwoNumbers
{
    [Ignore("Because nunit 3.x needs this populated")]
    public class TestStringCalculator : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        [SetUp]
        public void SetupTest()
        {
            CreateSUT = () => new StringCalculator();
        }

        private IStringCalculator CreateCalculator()
        {
            return CreateSUT();
        }

        /// <summary>
        /// This is a sample test that shows how the StringCalculator 
        /// should be created in future tests. 
        /// </summary>
        [Test]
        public void Constructor()
        {
            //---------------Execute Test ----------------------
            var calculator = CreateCalculator();
            //---------------Test Result -----------------------
            Assert.IsNotNull(calculator);
        }

        [Test]
        public void Add_GivenEmptyString_ShouldReturn0()
        {
            //---------------Set up test pack-------------------
            const string numbers = "";
            const int expectedResult = 0;
            var calculator = CreateCalculator();
            //---------------Execute Test ----------------------
            var result = calculator.Add(numbers);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Add_GivenNumber_ShouldReturnThatNumber()
        {
            //---------------Set up test pack-------------------
            const string numbers = "5";
            const int expectedResult = 5;
            var calculator = CreateCalculator();
            //---------------Execute Test ----------------------
            var result = calculator.Add(numbers);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Add_Given2SingleDigitNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string numbers = "1,2";
            const int expectedResult = 3;
            var calculator = CreateCalculator();
            //---------------Execute Test ----------------------
            var result = calculator.Add(numbers);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Add_Given2DoubleDidgitNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string numbers = "11,22";
            const int expectedResult = 33;
            var calculator = CreateCalculator();
            //---------------Execute Test ----------------------
            var result = calculator.Add(numbers);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }
    }
}