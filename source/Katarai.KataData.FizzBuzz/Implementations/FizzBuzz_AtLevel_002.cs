using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;

namespace Katarai.KataData.FizzBuzz.Implementations
{
    [TestStep(2)]
    public class FizzBuzz_AtLevel_002 : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            return "1";
        }
    }
}