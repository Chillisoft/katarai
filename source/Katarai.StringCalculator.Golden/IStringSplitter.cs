using System.Collections.Generic;

namespace Katarai.StringCalculator.Golden
{
    public interface IStringSplitter
    {
        IEnumerable<string> GetStrings(string input);
    }
}