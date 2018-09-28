using System;
using System.Collections.Generic;
using System.Linq;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.Acceptance.StringCalculator.Tests.SpecificTestNegativeNumbers
{
    public class StringCalculator : IStringCalculator
    {
        private static readonly List<string> Delimiters = new List<string>{",","\n"};

        public int Add(string input)
        {
            var numbersSection = input;
            if (HasCustomDelimiter(input))
            {
                var customDelimiter = GetCustomDelimiter(input);
                numbersSection = GetNumbersSection(input);
                Delimiters.Add(customDelimiter);
            }
            var numbers = GetNumbers(numbersSection);
            if (numbers.Any(number => number < 0))
            {
                throw new NegativesNotAllowedException(numbers.ToArray());
            }
            return numbers.Sum();
        }

        private static string GetNumbersSection(string input)
        {
            var numbersSection = input.Substring(input.IndexOf("\n", StringComparison.Ordinal) + 1);
            return numbersSection;
        }

        private static string GetCustomDelimiter(string input)
        {
            var customDelimiter = input.Substring(2, input.IndexOf("\n", StringComparison.Ordinal) - 2);
            return customDelimiter;
        }

        private static bool HasCustomDelimiter(string input)
        {
            return input.StartsWith("//");
        }

        private static IEnumerable<int> GetNumbers(string input)
        {
            var numbers = input.Split(Delimiters.ToArray(),StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            return numbers;
        }
    }
}