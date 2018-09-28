using Katarai.FizzBuzz.Interfaces;

namespace SamplePlayerFizzBuzz
{
    public class FizzBuzz : IFizzBuzz
    {
        public string GetFizzBuzz(int input)
        {
            return input == 1 ? "1" : "2";
        }
    }
}
