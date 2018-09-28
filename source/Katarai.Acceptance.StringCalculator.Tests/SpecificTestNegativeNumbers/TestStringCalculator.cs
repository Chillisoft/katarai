using System;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.StringCalculator.Tests.SpecificTestNegativeNumbers
{
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
            //---------------Arrange-------------------
            
            //---------------Act----------------------
            var calculator = CreateCalculator();
            //---------------Assert-----------------------
            Assert.IsNotNull(calculator);
        }

        [Test]
        public void Add_GivenEmptyString_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sum = calculator.Add("");
            //---------------Test Result -----------------------
            Assert.AreEqual(0,sum);
        }

        [Test]
        public void Add_GivenOneNumber_ShouldReturnNumber()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sum = calculator.Add("30");
            //---------------Test Result -----------------------
            Assert.AreEqual(30,sum);
        }

        [Test]
        public void Add_GivenTwoNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sum = calculator.Add("1,2");
            //---------------Test Result -----------------------
            Assert.AreEqual(3,sum);
        }

        [Test]
        public void Add_GivenThreeNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sum = calculator.Add("1,2,3");
            //---------------Test Result -----------------------
            Assert.AreEqual(6,sum);
        }

        [Test]
        public void Add_GivenNewLineDelimiter_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sum = calculator.Add("1,2\n3");
            //---------------Test Result -----------------------
            Assert.AreEqual(6,sum);
        }

        [Test]
        public void Add_GivenCustomDelimiter_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sum = calculator.Add("//;\n1;2;3");
            //---------------Test Result -----------------------
            Assert.AreEqual(6,sum);
        }

        [Test]
        public void Add_GivenNegativeNumber_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => calculator.Add("-1,2,3"));
            //---------------Test Result -----------------------
            CollectionAssert.Contains(exception.NegativeNumbers,-1);
        }

        [Test]
        public void Add_GivenNegativeNumbers_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var calculator = CreateCalculator();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => calculator.Add("-1,-2,-3"));
            //---------------Test Result -----------------------
            CollectionAssert.Contains(exception.NegativeNumbers, -1);
            CollectionAssert.Contains(exception.NegativeNumbers, -2);
            CollectionAssert.Contains(exception.NegativeNumbers, -3);
        }

    }
}
