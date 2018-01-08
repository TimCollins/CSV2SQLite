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

            ConsoleUtils.WaitForEscape();
        }

        private static void DisplayDefaultHelpText()
        {
            const string text = "Usage: csv2sqlite <input.csv> [output.csv] [config.json]\n\n" +
                                "The input file is required, the other parameters are optional.";

            Console.WriteLine(text);
        }
    }
}
