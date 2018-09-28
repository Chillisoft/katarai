using System;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(13)]
    public class FizzBuzz_AtLevel_013 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input % (3 * 5) == 0) return "FizzBuzz";
            if (input % 5 == 0) return "Buzz";
            if (input % 3 == 0) return "Fizz";
            return Convert.ToString(input);
        }
    }
}