using System.Collections.Generic;
using System.Linq;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.Acceptance.StringCalculator.Tests.UnknownStateMultipleCustomDelimiters
{
    public class StringCalculator : IStringCalculator
    {
        public int Add(string input)
        {
            if (IsNullOrEmpty(input))
            {
                return DefaultValue();
            }
            var delimiters = Delimiters();
            if (HasCustomDelimiter(input))
            {
                input = GetValues(input, ref delimiters);
            }
            return SplitAndSumAll(input,delimiters);
        }

        private static bool HasCustomDelimiter(string input)
        {
            return input.StartsWith("//");
        }

        private static string GetValues(string input, ref string delimiters)
        {
            var indexOf = input.IndexOf("\n");
          
            delimiters += input.Substring(2, indexOf - 1);
            input = input.Substring(indexOf + 1,input.Length - indexOf - 1);
      
            return input;
        }

        private static string Delimiters()
        {
            return "\n|,";
        }

        private static int SplitAndSumAll(string input, string delimiters)
        {
            input = input.Replace("***", "*");
            
            var splitAndSumAll = input.Split(delimiters.ToCharArray()).Select(int.Parse).Where( n=>n <= 1000);
            CheckNegatives(splitAndSumAll.Where(x=> x < 0));
            return splitAndSumAll.Sum();
        }

        private static void CheckNegatives(IEnumerable<int> splitAndSumAll)
        {
            var negatives = splitAndSumAll.ToList();

            if (negatives.Count > 0)
            {
                throw new NegativesNotAllowedException(negatives.ToArray());
            }
        }

        private static int DefaultValue()
        {
            return 0;
        }

        private static bool IsNullOrEmpty(string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}

