using System;

namespace CSV2SQLite.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new SQLiteGenerator();

            try
            {
                generator.ValidateCommandLine(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                DisplayDefaultHelpText();
                return;
            }

            Console.WriteLine("Working on {0}", args[0]);
            var sqlData = generator.Generate(args[0]);
            const string output = "output.sql";
            Console.WriteLine("Generated {0} bytes", sqlData.Length);
            generator.Write(sqlData, output);
            Console.WriteLine("Data written to {0}", output);

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
