using System;
using System.IO;
using System.Linq;
using CommandLineParse.App;
using Newtonsoft.Json;

namespace CSV2SQLite.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser.Parser(args);

            if (!parser.IsValidCommandLine())
            {
                Console.WriteLine("Error: Invalid command line parameters specified.");
                Console.WriteLine(parser.GetSummaryScreen());
                return;
            }

            ParserOptions options;
            try
            {
                options = parser.Parse();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }

            if (options.ShowHelpScreen)
            {
                Console.WriteLine(parser.GetHelpScreen());
                return;
            }

            var configuration = options.UseCustomConfig 
                ? SetConfiguration(args) 
                : SetDefaultConfiguration();

            var generator = new SQLiteGenerator();
            var sqlData = generator.Generate(configuration);
            Console.WriteLine("Generated {0} bytes", sqlData.Length);

            generator.Write(sqlData, configuration.OutputFilename);
            Console.WriteLine("Data written to {0}", configuration.OutputFilename);

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

            config.InputFilename = args[0];

            return config;
        }

        private static Configuration SetDefaultConfiguration()
        {
            return new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "default.sql"
            };
        }
    }
}
