using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

            var config = SetConfiguration(args);

            Console.WriteLine("Working on {0}", args[0]);

            var sqlData = generator.Generate(args[0]);
            Console.WriteLine("Generated {0} bytes", sqlData.Length);

            generator.Write(sqlData, config.OutputFilename);
            Console.WriteLine("Data written to {0}", config.OutputFilename);

            ConsoleUtils.WaitForEscape();
        }

        private static Configuration SetConfiguration(string[] args)
        {
            // Find the one that ends in .json
            var arg = args.FirstOrDefault(a => a.EndsWith(".json"));
            if (string.IsNullOrEmpty(arg) || !File.Exists(arg))
            {
                return SetDefaultConfiguration();
            }

            Configuration config;
            using (var stream = new StreamReader(arg))
            {
                var json = stream.ReadToEnd();
                config = JsonConvert.DeserializeObject<Configuration>(json);
            }

            return config;
        }

        private static Configuration SetDefaultConfiguration()
        {
            return new Configuration
            {
                OutputFilename = "default.sql"
            };
        }

        private static void DisplayDefaultHelpText()
        {
            const string text = "Usage: csv2sqlite <input.csv> [output.sql] [config.json]\n\n" +
                                "The input file is required, the other parameters are optional.";

            Console.WriteLine(text);
        }
    }
}
