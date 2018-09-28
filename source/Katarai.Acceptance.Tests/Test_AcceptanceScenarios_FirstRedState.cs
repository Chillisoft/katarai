using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.Tests
{
    [TestFixture]
    public class Test_AcceptanceScenarios_FirstRedState
    {
        [Test]
        [Ignore("This test currently tests nothing")] //TODO Andrew Russell 11 Aug 2014: Ignored Test - This test currently tests nothing
        public void TestRun_WhenPlayerTestLevel1_AndPlayerImplLevel0_ShouldBeRed()
        {
            //---------------Set up test pack-------------------
            var goldenImplementationTypes = new List<Type> {typeof (Golden_StringCalculator_AtLevel1)};
            var goldenTestType = typeof (Golden_TestStringCalculator);
            var playerTestType = typeof (Player_TestStringCalculator_AtLevel1);
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel0);
            var determiner = new KataStateDeterminer<IStringCalculator>(playerTestType, playerImplementationType, goldenTestType, goldenImplementationTypes);
            //---------------Assert Precondition----------------
            Assert.That(determiner.State.ToString(), Is.EqualTo(KataActivityLoopStates.Unknown.ToString()));
            //---------------Execute Test ----------------------
            determiner.Run();
            //---------------Test Result -----------------------
            Assert.That(determiner.State.ToString(), Is.EqualTo(KataActivityLoopStates.Red.ToString()));
        }
    }

    public enum KataActivityLoopStates
    {
        Unknown,
        Red
    }

    [Ignore("Because nunit 3.x needs this populated")]
    public class Player_TestStringCalculator_AtLevel1 : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        public Player_TestStringCalculator_AtLevel1()
        {
            CreateSUT = () => new Player_StringCalculator_AtLevel1();
        }

        [Test]
        public void Add_GivenEmptyString_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var sut = CreateSUT();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var value = sut.Add("");
            //---------------Test Result -----------------------
            Assert.That(0, Is.EqualTo(value));
        }
    }

    public class Player_StringCalculator_AtLevel0 : IStringCalculator
    {
        public Player_StringCalculator_AtLevel0()
        {
            throw new NotImplementedException();
        }

        public int Add(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Player_StringCalculator_AtLevel1 : IStringCalculator
    {
        public int Add(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Player_StringCalculator_AtLevel2 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "")
                return 0;
            throw new NotImplementedException("DIE");
        }
    }

    public class Player_StringCalculator_AtLevel3 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            return int.Parse(input);
        }
    }

    public class Player_StringCalculator_AtLevel4 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new [] {','}, 2);
            return parts.Select(int.Parse).Sum();
        }
    }

    public class Player_StringCalculator_AtLevel5 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new [] {','});
            return parts.Select(int.Parse).Sum();
        }
    }

    public class Player_StringCalculator_AtLevel6 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new[] { ',', '\n' });
            return parts.Select(int.Parse).Sum();
        }
    }

    internal class InputSections
    {
        public string[] Delimiters { get; private set; }
        public string Body { get; private set; }

        public InputSections(string input)
        {
            Body = input;
            Delimiters = new[] {",", "\n"};
            if (HasDelimiterHeader(input))
            {
                var sections = SplitOnNewline(input);
                var delimitersString = sections[0];
                Delimiters = new[] {delimitersString.Substring(2)};
                Body = sections[1];
            }
        }

        private static string[] SplitOnNewline(string input)
        {
            return input.Split(new[] {'\n'}, 2);
        }

        private static bool HasDelimiterHeader(string input)
        {
            return input.StartsWith("//");
        }
    }

    public class Player_StringCalculator_AtLevel7 : IStringCalculator
    {

        public int Add(string input)
        {
            if (input == "") return 0;
            var inputSections = new InputSections(input);
            var body = inputSections.Body;
            var parts = body.Split(inputSections.Delimiters, StringSplitOptions.None);
            var values = parts.Select(int.Parse);
            Validate(values);
            return values.Sum();
        }

        protected virtual void Validate(IEnumerable<int> values)
        {
            
        }
    }

    public class Player_StringCalculator_AtLevel8 : Player_StringCalculator_AtLevel7
    {
        protected override void Validate(IEnumerable<int> values)
        {
            var negatives = values.Where(i => i < 0).ToList();
            if (negatives.Any())
            {
                throw new NegativesNotAllowedException(negatives.ToArray());
            }
            
        }
    }

    public class Golden_StringCalculator_AtLevel1 : IStringCalculator
    {
        public int Add(string input)
        {
            return 0;
        }
    }

}
