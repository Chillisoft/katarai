using System;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    [TestFixture]
    public class TestStringCalculator : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        [SetUp]
        public void SetupTest()
        {
            CreateSUT = () => SUTFactory.CreateCalculator();
        }

        [Test]
        [TestStep(1)]
        public void Construct_ShouldConstructWithoutError()
        {
            CreateSUT();
        }

        [Test]
        [TestStep(2)]
        public void Add_WhenEmptyString_ShouldReturn0()
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = stringCalculator.Add("");
            //---------------Test Result -----------------------
            Assert.AreEqual(0,result);
        }

        [TestCase("5", 5)]
        [Test]
        [TestStep(3)]
        public void Add_WhenSingleNumber_ShouldReturnValue(string input = "5", int expectedResult = 5)
        {
            //---------------Set up test pack-------------------
            //string input = "5";
            //int expectedResult = 5;
            var stringCalculator = CreateSUT();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("154", 154)]
        [TestCase("987", 987)]
        [Test]
        [TestStep(3)]
        //[EdgeCase] // TODO: future: implement the idea of edge case tests and feedback to player about missing edge cases
        public void Add_WhenSingleNumber_WithManyDigits_ShouldReturnValue(string input = "154", int expectedResult = 154)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1,1", 2)]
        [TestCase("1,2", 3)]
        [Test]
        [TestStep(4)]
        public void Add_WhenTwoNumbers_ShouldReturnSum(string input = "1,1", int expectedResult = 2)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("11,21", 32)]
        [TestCase("123,321", 444)]
        [Test]
        [TestStep(4)]
        public void Add_WhenTwoNumbers_WithManyDigits_ShouldReturnSum(string input = "11,21", int expectedResult = 32)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1,1,1", 3)]
        [TestCase("1,2,3", 6)]
        [Test]
        [TestStep(5)]
        public void Add_WhenManyNumbers_ShouldReturnSum(string input = "1,1,1", int expectedResult = 3)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("10,20,30", 60)]
        [TestCase("1,2,5,3,4", 15)]
        [Test]
        [TestStep(5)]
        public void Add_WhenManyNumbers_WithManyDigits_ShouldReturnSum(string input = "10,20,30", int expectedResult = 60)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }
        
        [TestCase("1,1\n2", 4)]
        [TestCase("1,2,5\n3,4", 15)]
        [Test]
        [TestStep(6)]
        public void Add_WhenManyNumbers_WithNewline_ShouldReturnSum(string input = "1,1\n2", int expectedResult = 4)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("//*\n1*2,3", 6)]
        [TestCase("//;\n1;3,4", 8)]
        [Test]
        [TestStep(7)]
        public void Add_WhenSingleCustomDelimiter_ShouldReturnCorrectSum(string input = "//*\n1*2*3", int expectedResult = 6)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("//*\n1*2,3", 6)]
        [TestCase("//;\n1;3,4", 8)]
        [Test]
        [TestStepFailure(7, "It looks like you are supporting the old delimiters as well as the new. This is not required by the spec. Are you over-implementaing?")]
        public void Add_WhenSingleCustomDelimiterx_ShouldReturnCorrectSum(string input = "//*\n1*2,3", int expectedResult = 6)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestStep(8)]
        public void Add_WhenHasNegatives_ShouldThrowErrorWithMessage()
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            const string input = "1,-2,3,-4";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => stringCalculator.Add(input));
            //---------------Test Result -----------------------
            CollectionAssert.Contains(exception.NegativeNumbers, -2);
            CollectionAssert.Contains(exception.NegativeNumbers, -4);
        }

        [Test]
        [TestStep(8)]
        public void Add_WhenHasOtherNegatives_ShouldThrowErrorWithMessage()
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            const string input = "1,2,-3,-4";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => stringCalculator.Add(input));
            //---------------Test Result -----------------------
            CollectionAssert.Contains(exception.NegativeNumbers, -3);
            CollectionAssert.Contains(exception.NegativeNumbers, -4);
        }

        [TestCase("1001,2,4", 6)]
        [TestCase("1002,2,1", 3)]
        [TestCase("1000,1,4", 1005)]
        [Test]
        [TestStep(9)]
        public void Add_WhenHasNumbersGreaterThan1000_ShouldFilterThemOut(string input = "1001,2,4", int expectedResult = 6)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("//[*#]\n1*#2*#4", 7)]
        [TestCase("//[***]\n1***2***3", 6)]
        [Test]
        [TestStep(15)]
        public void Add_WhenLongCustomDelimiter_ShouldReturnCorrectSum(string input = "//[*#]\n1*#2*#4", int expectedResult = 7)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("//[***][%%]\n1%%2***3", 6)]
        [TestCase("//[*#][*]\n1*#2*4", 7)]
        [Test]
        [TestStep(16)]
        public void Add_WhenManyCustomDelimiters_ShouldReturnCorrectSum(string input = "//[***][%%]\n1%%2***3", int expectedResult = 6)
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

    }
}
