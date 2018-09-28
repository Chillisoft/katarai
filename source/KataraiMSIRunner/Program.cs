using System;

namespace KataraiMSIRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.Write(args[0]);
            }
            else
            {
                Console.Write("Nothing");
            }

            Console.ReadKey();
        }
    }
}
