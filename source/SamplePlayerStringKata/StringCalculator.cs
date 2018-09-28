using System;
using System.Collections.Generic;
using System.Linq;
using Katarai.StringCalculator.Interfaces;

namespace SamplePlayerStringKata
{
    public class StringCalculator: IStringCalculator
    {
        // Red
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = BreakIntoParts(input);
            return AddParts(parts);

        }

        private static int AddParts(IEnumerable<string> parts)
        {
            return ConvertParts(parts).Sum();
        }

        private static IEnumerable<int> ConvertParts(IEnumerable<string> parts)
        {
            return parts.Select(int.Parse); 
        }

        private static IEnumerable<string> BreakIntoParts(string input)
        {
            return input.Split(FetchExpectedDelimiters(), StringSplitOptions.RemoveEmptyEntries);

        }

        private static char[] FetchExpectedDelimiters()
        {
            return new[] { ',', '\n' };
        }


        //// Green
        //public int Add(string input)
        //{
        //    if (input == "") return 0;
        //    var parts = BreakIntoParts(input);
        //    return AddParts(parts);

        //}

        //private static int AddParts(IEnumerable<string> parts)
        //{
        //    return ConvertParts(parts).Sum();
        //}

        //private static IEnumerable<int> ConvertParts(IEnumerable<string> parts)
        //{
        //    return parts.Select(int.Parse);
        //}

        //private static IEnumerable<string> BreakIntoParts(string input)
        //{
        //    var parts = input.Split(new[] { ',' }, 2);
        //    return parts;
        //}
    }
}
