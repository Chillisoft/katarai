using System;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.KataData.StringCalculator.Tests
{
    [TestFixture]
    public class TestStringCalculator : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        [SetUp]
        public void SetupTest()
        {
            CreateSUT = () => new Implementations.Final.StringCalculator();
        }

        [Test]
        [TestStep(1)]
        [NoRefactoring]
        [StepShouldDo("Construct your String Calculator without any exception")]
        public void Level01_Construct_ShouldConstructWithoutError()
        {
            Assert.DoesNotThrow(()=>CreateSUT());
        }

        [Test]
        [TestStep(2)]
        [NoRefactoring]
        [StepShouldDo("Return zero when an empty string is passed in")]
        public void Level02_Add_WhenEmptyString_ShouldReturn0()
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = stringCalculator.Add("");
            //---------------Test Result -----------------------
            Assert.AreEqual(0,result);
        }

        [Test]
        [TestStep(3)]
        [NoRefactoring]
        [StepShouldDo("Return the integer value of the numeric string")]
        public void Level03_Add_WhenSingleNumber_ShouldReturnValue()
        {
            //---------------Set up test pack-------------------
            const string input = "97";
            const int expectedResult = 97;
            var stringCalculator = CreateSUT();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestStep(4)]
        [StepShouldDo("Sum numbers separated by comma")]
        public void Level04_Add_WhenManyNumbers_CommaOnly_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "3,9";
            const int expectedResult = 12;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }
        

        [Test]
        [TestStep(4)]
        [StepShouldDo("Sum numbers separated by comma")]
        [EdgeCaseHint("Have you summed two numbers separated by a comma(',')?")]
        public void Level04_Add_WhenTwoNumbers_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "852,927";
            const int expectedResult = 1779;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestStep(4)]
        [StepShouldDo("Sum numbers separated by comma")]
        [EdgeCaseHint("Have you summed three numbers separated by a comma(',')?")]
        public void Level04_Add_WhenManyNumbers_CommaOnly_WithManyDigits_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "726,953,368";
            const int expectedResult = 2047;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        [TestStep(5)]
        [StepShouldDo("Handle both commas(',') and new lines('\\n') as delimiters")]
        public void Level05_Add_WhenManyNumbers_WithNewline_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "639,846\n287";
            const int expectedResult = 1772;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestStep(6)]
        [StepShouldDo("Handle a single custom delimiter e.g. //;\\n1;2")]
        public void Level06_Add_WhenSingleCustomDelimiter_ShouldReturnCorrectSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//*\n968*462*971";
            const int expectedResult = 2401;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestStep(7)]
        [InvalidTestHint("Are you testing that the provided NegativesNotAllowedException is thrown?\r\nAre you verifying that the negative numbers are contained in the NegativeNumbers property?")]
        [StepShouldDo("Return the negative number using the provided NegativesNotAllowedException")]
        public void Level07_Add_WhenHasOneNegative_ShouldThrowErrorWithMessage()
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            const string input = "56,-671,647";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => stringCalculator.Add(input));
           //---------------Test Result -----------------------
            Assert.AreEqual(1, exception.NegativeNumbers.Count);
            CollectionAssert.Contains(exception.NegativeNumbers, -671);
        }

        [Test]
        [TestStep(8)]
        [InvalidTestHint("Are you testing that the provided NegativesNotAllowedException is thrown?\r\nAre you verifying that the negative numbers are contained in the NegativeNumbers property?")]
        [StepShouldDo("Return all negative numbers using the provided NegativesNotAllowedException")]
        [SuggestedTestName("Add_WhenHasManyNegatives_ShouldThrowErrorWithMessage")]
        public void Level08_Add_WhenHasManyNegatives_ShouldThrowErrorWithMessage()
        {
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            const string input = "956,-641,647,-974,-167,-26";
            //---------------Assert Precondition----------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => stringCalculator.Add(input));
            //---------------Execute Test ----------------------
            Assert.AreEqual(4, exception.NegativeNumbers.Count);
            CollectionAssert.Contains(exception.NegativeNumbers, -641);
            CollectionAssert.Contains(exception.NegativeNumbers, -974);
            CollectionAssert.Contains(exception.NegativeNumbers, -167);
            CollectionAssert.Contains(exception.NegativeNumbers, -26);
        }

        [Test]
        [TestStep(9)]
        [StepShouldDo("Filter numbers > 1000")]
        public void Level09_Add_WhenHasNumbersGreaterThan1000_ShouldFilterThemOut()
        {
            const string input = "1531,656,999";
            const int expected = 1655;
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestStep(9)]
        [StepShouldDo("Filter numbers > 1000")]
        [EdgeCaseHint("Have you also filtered out the number 1000 by mistake?")]
        [SuggestedTestName("Add_WhenHasNumberEqual1000_ShouldNotFilterItOut")]
        public void Level09_Add_WhenHasNumberEqual1000_ShouldNotFilterItOut()
        {
            const string input = "1000,652,1006,314";
            const int expected = 1966;
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestStep(9)]
        [StepShouldDo("Filter numbers > 1000")]
        [EdgeCaseHint("Have you specifically checked that 1001 is filtered out?")]
        [SuggestedTestName("Add_WhenHasNumberEqual1001_ShouldNotFilterItOut")]
        public void Level09_Add_WhenHasNumberEqual1001_ShouldFilterItOut()
        {
            const string input = "1001,655,137,1002,1645";
            const int expected = 792;
            //---------------Set up test pack-------------------
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestStep(10)]
        [InvalidTestHint("Your input string should be like //[***]\\n1***2, for multi character delimiters")]
        [StepShouldDo("Handle a custom delimiter with length > 1 character e.g. //[***]\\n1***2***3")]
        public void Add_WhenLongCustomDelimiter_ShouldReturnCorrectSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[*#]\n653*#1*#349*#135";
            const int expectedResult = 1138;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestStep(11)]
        [StepShouldDo("Handle many custom delimiters with length > 1 character e.g. //[*][%]\\n1*2%3")]
        public void Level11_Add_WhenManyCustomDelimiters_ShouldReturnCorrectSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[***][%%]\n597%%616***971***1231%%654";
            const int expectedResult = 2838;
            var stringCalculator = CreateSUT();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }
    }
}
