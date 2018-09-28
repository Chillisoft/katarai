using System;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.StringCalculator.Tests.UnknownStateMultipleCustomDelimiters
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
        public void Given_EmptyInputStringShouldReturn_Zero()
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
            const string empty = "1";
            const int expected = 1;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_LargeSingleNumberShouldReturn_LargeSingleNumber()
        {
            const string empty = "150";
            const int expected = 150;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_TwoNumbersInputStringShouldReturn_Sum()
        {
            const string empty = "1,2";
            const int expected = 3;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_ManyNumbersInputStringShouldReturn_Sum()
        {
            const string empty = "1,2,3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumbersInputStringWithNewLineInBetweenShouldReturn_Sum()
        {
            const string empty = "1\n2,3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumbersInputStringWithSingleCustormDelimiterInBetweenShouldReturn_Sum()
        {
            const string empty = "//;\n1;2";
            const int expected = 3;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumbersInputStringWithSingleNegativeNumberInBetweenShouldReturn_Sum()
        {
            const string empty = "-1";
            const string expected = "negatives not allowed: -1";
            var stringCalculator = CreateCalculator();
            var actual = Assert.Throws<NegativesNotAllowedException>(
                                        () => stringCalculator.Add(empty));
            Assert.AreEqual(expected, actual.Message);
        }

        [Test]
        public void Given_NumbersInputStringWithManyNegativeNumberInBetweenShouldReturn_Sum()
        {
            const string empty = "-1,-2";
            const string expected = "negatives not allowed: -1,-2";
            var stringCalculator = CreateCalculator();
            var actual = Assert.Throws<NegativesNotAllowedException>(
                                        () => stringCalculator.Add(empty));
            Assert.AreEqual(expected, actual.Message);
        }

        [Test]
        public void Given_NumbersInputStringWithValueGreaterThanThousandInBetweenShouldReturn_Sum()
        {
            const string empty = "1001,3";
            const int expected = 3;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumbersInputStringWithValueNotGreaterThanThousandInBetweenShouldReturn_Sum()
        {
            const string empty = "1000,3";
            const int expected = 1003;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumbersInputStringWithCustormDelimitersOfLengthLongerThanOneInBetweenShouldReturn_Sum()
        {
            const string empty = "//[***]\n1***2***3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_NumbersInputStringWithMultipleCustormDelimitersInBetweenShouldReturn_Sum()
        {
            const string empty = "//[*][%]\n1*2%3";
            const int expected = 6;
            var stringCalculator = CreateCalculator();
            var actual = stringCalculator.Add(empty);
            Assert.AreEqual(expected, actual);
        }
    }
}