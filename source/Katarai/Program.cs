using System;
using System.Threading;
using System.Windows.Forms;
using Katarai.Runner;
using NUnit.Framework;
using PowerArgs;

namespace Katarai
{
    [TabCompletion]
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgExample("Katarai -s SomeString -i 50 -sw", "Shows how to use the shortcut version of the switch parameter")]
    public class ConsoleArgs
    {
        [ArgDescription("Shows the help documentation")]
        public bool Help { get; set; }

        [ArgDescription("Starts the application GUI")]
        [ArgActionMethod]
        public void GUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        [ArgDescription("Check the implementation level")]
        [ArgActionMethod]
        public void CheckImplementation(CheckImplementationArgs args)
        {
        }

        [ArgDescription("Description for a required string parameter")]
        public string StringArg { get; set; }

        [ArgDescription("Description for an optional integer parameter")]
        public int IntArg { get; set; }

        [ArgDescription("Description for an optional switch parameter")]
        public bool SwitchArg { get; set; }
    }

    public class CheckImplementationArgs
    {
        [ArgRequired, ArgExistingFile, ArgPosition(1)]
        [ArgDescription("The player implementation file")]
        public string File { get; set; }
    }
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [Apartment(ApartmentState.STA)]
        static void Main(string[] args)
        {
            try
            {
                //var consoleArgs = Args.Parse<ConsoleArgs>(args);
                //if (consoleArgs.Help)
                //{
                //    ArgUsage.GetStyledUsage<ConsoleArgs>().Write();
                //    return;
                //}
                //Console.WriteLine("You entered StringArg '{0}' and IntArg '{1}', switch was '{2}'", consoleArgs.StringArg, consoleArgs.IntArg, consoleArgs.SwitchArg);
                DeterminePlayerImplementationLevelOn(args[0], args[1]);
                           
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                ArgUsage.GetStyledUsage<ConsoleArgs>().Write();
            }
        }

        static void DeterminePlayerImplementationLevelOn(string kataDataAssemblyPath, string playerAssemblyPath, string playerTestsPath = null)
        {
            var runner = new Runner.Runner(kataDataAssemblyPath, playerAssemblyPath, playerTestsPath ?? playerAssemblyPath);
            var result = runner.Run();
            MessageBox.Show(string.Format("Player implementation is at level {0}\nPlayer tests are at level {1}", result.PlayerImplementationLevel, result.PlayerTestLevel), "Wheee!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
