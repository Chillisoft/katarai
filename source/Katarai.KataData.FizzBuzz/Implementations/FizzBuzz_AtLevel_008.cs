using System;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(8)]
    public class FizzBuzz_AtLevel_008 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input % 3 == 0) return "Fizz";
            if (input == 5) return "Buzz";
            return Convert.ToString(input);
        }
    }
}