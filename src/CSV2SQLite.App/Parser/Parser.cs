using System.Linq;
using System.Text;
using CommandLineParse.App;
using CSV2SQLite.App.Exceptions;
using CSV2SQLite.App.Implementation;
using CSV2SQLite.App.Interfaces;

namespace CSV2SQLite.App.Parser
{
    public class Parser
    {
        private string[] _args { get; set; }
        private IFileWrapper _wrapper { get; set; }

        private readonly string[] _switches = 
        {
            "-",
            "/"
        };
        
        public Parser(string[] args)
        {
            _args = args;
            _wrapper = new FileWrapper();
        }

        public Parser(string[] args, IFileWrapper wrapper)
        {
            _args = args;
            _wrapper = wrapper;
        }

        public bool IsValidCommandLine()
        {
            return _args.Length > 0;
        }

        public string GetHelpScreen()
        {
            return new StringBuilder("Usage: csv2sqlite <input.csv> [output.sql] [config.json]\n\n" +
                                     "The input file is required, the other parameters are optional.").ToString();
        }

        public ParserOptions Parse()
        {
            var options = new ParserOptions();

            for (var i = 0; i < _args.Length; i++)
            {
                var arg = _args[i];

                var switchFound = _switches.Any(s => arg.StartsWith(s));

                if (switchFound)
                {
                    if (arg.EndsWith("?") || arg.EndsWith("-help"))
                    {
                        options.ShowHelpScreen = true;
                    }
                    else if (arg.EndsWith("c"))
                    {
                        options.UseCustomConfig = true;

                        // The next arg should be the config file name
                        // and that file should exist

                        if (i > _args.Length - 2)
                        {
                            throw new InvalidCommandLineException("Custom configuration parameter specified but no configuration file supplied.");
                        }
                        var file = _args[i + 1];
                        if (!_wrapper.Exists(file))
                        {
                            throw new InvalidCommandLineException(string.Format("Custom configuration parameter specified but configuration file {0} was not found.", file));
                        }

                        options.CustomConfig = _args[i + 1];
                    }
                    else if (arg.EndsWith("o"))
                    {
                        options.UseCustomOutputFile = true;

                        // The next arg should be the output file name
                        // and that file should exist                    
                        if (i > _args.Length - 2)
                        {
                            throw new InvalidCommandLineException("Custom output filename parameter specified but no output file name supplied.");
                        }

                        var file = _args[i + 1];
                        if (!_wrapper.Exists(file))
                        {
                            throw new InvalidCommandLineException(string.Format("Custom output filename parameter specified but configuration file {0} was not found.", file));
                        }

                        options.CustomOutputFile = _args[i + 1];
                    }
                }
            }

            return options;
        }

        public string GetSummaryScreen()
        {
            return new StringBuilder("Usage: csv2sqlite <input.csv> [output.sql] [config.json]\n\n" +
                                     "The input file is required, the other parameters are optional.").ToString();
        }
    }
}
