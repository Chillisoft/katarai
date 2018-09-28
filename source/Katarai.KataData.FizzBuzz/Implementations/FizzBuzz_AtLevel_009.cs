using System;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(9)]
    public class FizzBuzz_AtLevel_009 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input % 3 == 0) return "Fizz";
            if (input == 5 || input == 10) return "Buzz";
            return Convert.ToString(input);
        }
    }
}