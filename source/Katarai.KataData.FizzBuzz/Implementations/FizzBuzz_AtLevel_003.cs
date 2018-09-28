using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(3)]
    public class FizzBuzz_AtLevel_003 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            if (input==1) return "1";
            return "2";
        }
    }
}