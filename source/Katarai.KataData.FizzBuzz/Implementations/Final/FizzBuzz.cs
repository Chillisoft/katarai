using System;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations.Final
{
    public class FizzBuzz : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input % (3*5) == 0) return "FizzBuzz";
            if (input % 5 == 0) return "Buzz";
            if (input % 3 == 0) return "Fizz";
            return Convert.ToString(input);
        }

    }
}