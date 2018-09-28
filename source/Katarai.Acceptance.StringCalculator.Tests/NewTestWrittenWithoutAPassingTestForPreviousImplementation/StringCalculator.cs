using System;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.Acceptance.StringCalculator.Tests.NewTestWrittenWithoutAPassingTestForPreviousImplementation
{
    public class StringCalculator : IStringCalculator
    {
        public int Add(string input)
        {
            throw new NotImplementedException();
        }
    }
    public class StringCalculator_002 : IStringCalculator
    {
        public int Add(string input)
        {
            return 0;
        }
    }
}
