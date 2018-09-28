using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(4)]
    public class FizzBuzz_AtLevel_004 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input==1) return "1";
            if (input == 2) return "2";
            return "Fizz";
        }
    }
}