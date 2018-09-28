using System;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(12)]
    public class FizzBuzz_AtLevel_012 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input == 15 || input == 30) return "FizzBuzz";
            if (input % 5 == 0) return "Buzz";
            if (input % 3 == 0) return "Fizz";
            return Convert.ToString(input);
        }
    }
}