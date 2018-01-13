using System;

namespace CSV2SQLite.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new SQLiteGenerator();

            if (!generator.IsValidCommandLine(args))
            {
                DisplayDefaultHelpText();
            }

            Console.WriteLine("Working on {0}", args[0]);
            generator.Generate(args[0], "output.sql");

            ConsoleUtils.WaitForEscape();
        }

        private static void DisplayDefaultHelpText()
        {
            const string text = "Usage: csv2sqlite <input.csv> [output.sql] [config.json]\n\n" +
                                "The input file is required, the other parameters are optional.";

            Console.WriteLine(text);
        }
    }
}
