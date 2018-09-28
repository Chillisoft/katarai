using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    [TestFixture]
    public class TestCustomDelimiterParser
    {
        private static CustomDelimiterParser CreateCustomDelimiterParser()
        {
            return new CustomDelimiterParser();
        }

        [TestCase("//%\n1")]
        [TestCase("//%\n1,2%3,4")]
        [TestCase("//*\n1*2*3")]
        [TestCase("//[***]\n1***2")]
        public void IsDelimiterMatch_WhenMatch_ShouldReturnTrue(string input)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.IsDelimiterMatch(input);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [TestCase("")]
        [TestCase("/%\n1")]
        [TestCase("1,2,3,4")]
        [TestCase("1,2/n3,4")]
        [TestCase("1*2*3")]
        public void IsDelimiterMatch_WhenNotMatch_ShouldReturnFalse(string input)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.IsDelimiterMatch(input);
            //---------------Test Result -----------------------
            Assert.IsFalse(result);
        }

        [TestCase("//%\n", "%")]
        [TestCase("//%\n1%2", "%")]
        [TestCase("//*\n", "*")]
        public void GetDelimiters_WhenHasSingleCustomDelimiter_ShouldReturnDefaultsWithDelimiter(string input, string delimiter)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            var expectedDelimiters = new List<string> { ",", "\n", delimiter };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.GetDelimiters(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedDelimiters, result);
        }

        [TestCase("")]
        [TestCase("/%\n1")]
        [TestCase("1,2")]
        public void GetDelimiters_WhenHasNoCustomDelimiter_ShouldThrowError(string input)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentException>(() => customDelimiterParser.GetDelimiters(input));
            //---------------Test Result -----------------------
            Assert.AreEqual("input", exception.ParamName);
            StringAssert.Contains("Parser not matched: no custom delimiter found.", exception.Message);
        }

        [TestCase("//%\n", "")]
        [TestCase("//%\n1%2", "1%2")]
        [TestCase("//*\n2*3", "2*3")]
        public void GetBody_WhenHasCustomDelimiter_ShouldReturnRemainingString(string input, string remainingString)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.GetBody(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(remainingString, result);
        }

        [TestCase("")]
        [TestCase("/%\n1")]
        [TestCase("1,2")]
        public void GetBody_WhenHasNoCustomDelimiter_ShouldThrowError(string input)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentException>(() => customDelimiterParser.GetBody(input));
            //---------------Test Result -----------------------
            Assert.AreEqual("input", exception.ParamName);
            StringAssert.Contains("Parser not matched: no custom delimiter found.", exception.Message);
        }

        [TestCase("//[%%%]\n", "%%%")]
        [TestCase("//[%%%]\n1%%%2", "%%%")]
        [TestCase("//[*#]\n", "*#")]
        public void GetDelimiters_WhenHasLongCustomDelimiter_ShouldReturnDefaultsWithDelimiter(string input, string delimiter)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            var expectedDelimiters = new List<string> { ",", "\n", delimiter };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.GetDelimiters(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedDelimiters, result);
        }

        [TestCase("//[%%%]\n", "")]
        [TestCase("//[%%%]\n1%%%2", "1%%%2")]
        [TestCase("//[*#]\n1*#2", "1*#2")]
        public void GetBody_WhenHasLongCustomDelimiter_ShouldReturnRemainingString(string input, string remainingString)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.GetBody(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(remainingString, result);
        }

        [TestCase("//[***][%%]\n1%%2***3", "***", "%%")]
        [TestCase("//[*#][*]\n1*#2*4", "*#", "*")]
        public void GetDelimiters_WhenHasManyCustomDelimiters_ShouldReturnDefaultsWithDelimiters(string input, params string[] expectedCustomDelimiters)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateCustomDelimiterParser();
            var expectedDelimiters = new List<string> { ",", "\n" };
            expectedDelimiters.AddRange(expectedCustomDelimiters);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.GetDelimiters(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedDelimiters, result);
        }

        [TestCase("//[***][%%]\n1%%2***3", "1%%2***3")]
        [TestCase("//[*#][*]\n1*#2*4", "1*#2*4")]
        public void GetBody_WhenHasManyCustomDelimiters_ShouldReturnRemainingString(string input, string remainingString)
        {
            //---------------Set up test pack-------------------
            var customDelimiterParser = CreateCustomDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = customDelimiterParser.GetBody(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(remainingString, result);
        }
    }
}