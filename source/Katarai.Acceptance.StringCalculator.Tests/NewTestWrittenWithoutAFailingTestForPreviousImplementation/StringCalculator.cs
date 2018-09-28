using System;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.Acceptance.StringCalculator.Tests.NewTestWrittenWithoutAFailingTestForPreviousImplementation
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
