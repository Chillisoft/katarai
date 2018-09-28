using System;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(6)]
    public class FizzBuzz_AtLevel_006 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input == 3) return "Fizz";
            if (input == 5) return "Buzz";
            return Convert.ToString(input);
        }
    }
}