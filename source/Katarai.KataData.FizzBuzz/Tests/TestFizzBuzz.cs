using System;
using System.Collections.Generic;
using Engine;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;
using NUnit.Framework;

namespace Katarai.KataData.FizzBuzz.Tests
{
    //[DoNotShowImplementedTooMuchMessage]
    public class TestFizzBuzz : ITestPack<IFizzBuzz>
    {
        public Func<IFizzBuzz> CreateSUT { get; set; }

        [SetUp]
        public void SetupTest()
        {
            CreateSUT = () => new Implementations.Final.FizzBuzz();
        }

        [Test]
        [TestStep(1)]
        public void Construct_ShouldConstructWithoutError()
        {
            CreateSUT();
        }

        [Test]
        [TestStep(2)]
        [NoRefactoring]
        [StepShouldDo("Given 1 should return string of 1")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_1_ShouldReturn_1")]
        public void GetFizzBuzz_WhenInputIs_1_ShouldReturn_1()
        {
            //---------------Set up test pack-------------------
            const string expected = "1";
            const int input = 1;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        } 

        [Test]
        [TestStep(3)]
        [NoRefactoring]
        [StepShouldDo("Given 2 should return string of 2")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_2_ShouldReturn_2")]
        public void GetFizzBuzz_WhenInputIs_2_ShouldReturn_2()
        {
            //---------------Set up test pack-------------------
            const string expected = "2";
            const int input = 2;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }         
        
        [Test]
        [TestStep(4)]
        [NoRefactoring]
        [StepShouldDo("Given 3 should return Fizz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_3_ShouldReturn_Fizz")]
        public void GetFizzBuzz_WhenInputIs_3_ShouldReturn_Fizz()
        {
            //---------------Set up test pack-------------------
            const string expected = "Fizz";
            const int input = 3;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }     
    
        [Test]
        [TestStep(5)]
        [NoRefactoring]
        [DoNotShowImplementedTooMuchMessage]
        [StepShouldDo("Given 4 should return string of 4")]
        [EdgeCaseHint("Have you refactored too early? You should try to refactor after the third occurence of a pattern.")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_4_ShouldReturn_4")]
        public void GetFizzBuzz_WhenInputIs_4_ShouldReturn_4()
        {
            //---------------Set up test pack-------------------
            const string expected = "4";
            const int input = 4;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }     

        [Test]
        [TestStep(6)]
        [NoRefactoring]
        [StepShouldDo("Given 5 should return Buzz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_5_ShouldReturn_Buzz")]
        public void GetFizzBuzz_WhenInputIs_5_ShouldReturn_Buzz()
        {
            //---------------Set up test pack-------------------
            const string expected = "Buzz";
            const int input = 5;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        } 

        [Test]
        [TestStep(7)]
        [NoRefactoring]
        [StepShouldDo("Given 6 should return Fizz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_6_ShouldReturn_Fizz")]
        public void GetFizzBuzz_WhenInputIs_6_ShouldReturn_Fizz()
        {
            //---------------Set up test pack-------------------
            const string expected = "Fizz";
            const int input = 6;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        } 

        [Test]
        [TestStep(8)]
        [NoRefactoring]
        [StepShouldDo("Given 9 should return Fizz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_9_ShouldReturn_Fizz")]
        public void GetFizzBuzz_WhenInputIs_9_ShouldReturn_Fizz()
        {
            //---------------Set up test pack-------------------
            const string expected = "Fizz";
            const int input = 9;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestStep(9)]
        [NoRefactoring]
        [StepShouldDo("Given 10 should return Buzz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_10_ShouldReturn_Buzz")]
        public void GetFizzBuzz_WhenInputIs_10_ShouldReturn_Buzz()
        {
            //---------------Set up test pack-------------------
            const string expected = "Buzz";
            const int input = 10;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestStep(10)]
        [NoRefactoring]
        [StepShouldDo("Given 15 should return FizzBuzz")]
        [EdgeCaseHint("Don't forget that 15 is a multiple of both 3 and 5.")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_15_ShouldReturn_FizzBuzz")]
        public void GetFizzBuzz_WhenInputIs_15_ShouldReturn_FizzBuzz()
        {
            //---------------Set up test pack-------------------
            const string expected = "FizzBuzz";
            const int input = 15;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestStep(11)]
        [NoRefactoring]
        [StepShouldDo("Given 20 should return Buzz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_20_ShouldReturn_Buzz")]
        public void GetFizzBuzz_WhenInputIs_20_ShouldReturn_Buzz()
        {
            //---------------Set up test pack-------------------
            const string expected = "Buzz";
            const int input = 20;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestStep(12)]
        [NoRefactoring]
        [StepShouldDo("Given 30 should return FizzBuzz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_30_ShouldReturn_FizzBuzz")]
        public void GetFizzBuzz_WhenInputIs_30_ShouldReturn_FizzBuzz()
        {
            //---------------Set up test pack-------------------
            const string expected = "FizzBuzz";
            const int input = 30;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestStep(13)]
        [NoRefactoring]
        [DoNotShowImplementedTooMuchMessage]
        [StepShouldDo("Given 75 should return FizzBuzz")]
        [SuggestedTestName("GetFizzBuzz_WhenInputIs_75_ShouldReturn_FizzBuzz")]
        public void GetFizzBuzz_WhenInputIs_75_ShouldReturn_FizzBuzz()
        {
            //---------------Set up test pack-------------------
            const string expected = "FizzBuzz";
            const int input = 75;
            var fb = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var actual = fb.GetFizzBuzz(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, actual);
        }

    }
}