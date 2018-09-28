namespace Katarai.KataData.StringCalculator.Implementations.Final
{
    internal class InputSections
    {
        public string[] Delimiters { get; private set; }
        public string Body { get; private set; }

        public InputSections(string input)
        {
            Body = input;
            Delimiters = new[] {",", "\n"};
            if (HasDelimiterHeader(input))
            {
                var sections = SplitOnNewline(input);
                var delimitersString = sections[0];
                Delimiters = new[] {delimitersString.Substring(2)};
                Body = sections[1];
            }
        }

        private static string[] SplitOnNewline(string input)
        {
            return input.Split(new[] {'\n'}, 2);
        }

        private static bool HasDelimiterHeader(string input)
        {
            return input.StartsWith("//");
        }
    }
}