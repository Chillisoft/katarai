using System;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.StringCalculator.Tests.UnknownStateMultipleCustomDelimiters2
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
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var calculator = CreateCalculator();
            //---------------Test Result -----------------------
            Assert.IsNotNull(calculator);
        }

        [Test]
        public void Given_EmptyStringShouldReturn_Zero()
        {
            const string empty = "";
            const int expected = 0;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Given_SingleInputStringShouldReturn_SingleNumber()
        {
            const string input = "1";
            const int expected = 1;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_LargeSingleInputStringShouldReturn_LargeSingleNumber()
        {
            const string input = "120";
            const int expected = 120;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_TwoNumberInputStringShouldReturn_Sum()
        {
            const string input = "1,2";
            const int expected = 3;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_ThreeNumberInputStringShouldReturn_Sum()
        {
            const string input = "1,2,3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumberInputStringWithNewLineInBetweenShouldReturn_Sum()
        {
            const string input = "1\n2,3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumberInputStringWithCustormDelimeterInBetweenShouldReturn_Sum()
        {
            const string input = "//;\n1;2";
            const int expected = 3;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Given_SingleNegativeNumberInputStringShould_ThrowException()
        {
            const string input = "-1";
            const string expected = "negatives not allowed: -1";
            var stringCalculator = CreateCalculator();
            var actual = Assert.Throws<NegativesNotAllowedException>(() => stringCalculator.Add(input));
            Assert.AreEqual(expected, actual.Message);
        }

        [Test]
        public void Given_MultipleNegativeNumbersInputStringShould_ThrowException()
        {
            const string input = "-1,-2";
            const string expected = "negatives not allowed: -1,-2";
            var stringCalculator = CreateCalculator();
            var actual = Assert.Throws<NegativesNotAllowedException>(() => stringCalculator.Add(input));
            Assert.AreEqual(expected, actual.Message);
        }

        [Test]
        public void Given_NumberInputStringWithNumberGreaterThanThousandInBetweenShouldReturn_Sum()
        {
            const string input = "1001,3";
            const int expected = 3;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumberInputStringWithNumberNotGreaterThanThousandInBetweenShouldReturn_Sum()
        {
            const string input = "1000,3";
            const int expected = 1003;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Given_NumberInputStringWithCustormDelimiterWithLengthGreaterThanOneShouldReturn_Sum()
        {
            const string input = "//[***]\n1***2***3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Given_NumberInputStringWithManyCustormDelimiterGreaterThanOneShouldReturn_Sum()
        {
            const string input = "//[*][%]\n1*2%3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(input);
            Assert.AreEqual(expected, actual);
        }

    }
}