/*
  BEFORE YOU START :
  This Kata is meant to teach TDD cadence, growing a solution using tests and speed. 
  It’s designed to be a bit long for the time you have when you start, and the idea 
  is to practice it until you can do it really fast, while rigorously practising TDD. 
  This will ingrain the cadence of TDD and the keyboard shortcuts and refactorings 
  you need to be fast. Remember, the only way to go fast is to go well. 
*/

/*
  RULES : 
  1.	Strictly practice TDD: Red, Green, Refactor
  2.	Clean Code is required:
    2.1.	Intention-revealing names
    2.2.	Verb/verb-phrase method names
    2.3.	Methods should do one thing and be short, with no side-effects
    2.4.	Methods should contain only one level of abstraction
    2.5.	Code should read like a top-down narrative
    2.6.	No unnecessary code
    2.7.	DRY
    2.8.	Unit tests that test pieces of the algorithm, not only acceptance level tests.
*/

/*
  GOALS : 
  +	Silver: 25 minutes
  +	Gold:   20 minutes
*/

/*
  THE KATA : 
  Return “fizz”, “buzz” or “fizzbuzz”.
  For a given natural number greater zero return
   +	“fizz” if the number is dividable by 3
   +	“buzz” if the number is dividable by 5
   +	“fizzbuzz” if the number is dividable by 15

  TEST CASES : 
 
  Input	Result
   1	1
   2	2
   3	fizz
   4	4
   5	buzz
   6	fizz
   10	buzz
   15	fizzbuzz

 */
using System;
using Engine;
using Katarai.FizzBuzz.Interfaces;
using NUnit.Framework;


namespace SamplePlayerFizzBuzz
{
    [TestFixture]
    public class FizzBuzzTest : ITestPack<IFizzBuzz>
    {
        public Func<IFizzBuzz> CreateSUT { get; set; }

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			CreateSUT = () => new FizzBuzz();
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

    }
}
