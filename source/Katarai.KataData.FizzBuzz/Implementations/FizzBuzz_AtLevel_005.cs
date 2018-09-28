using System;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(5)]
    public class FizzBuzz_AtLevel_005 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input == 3) return "Fizz";
            return Convert.ToString(input);
        }
    }
}