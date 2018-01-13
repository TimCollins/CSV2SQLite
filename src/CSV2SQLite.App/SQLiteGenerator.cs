using System;
using System.Text;
using CSV2SQLite.App.Implementation;
using CSV2SQLite.App.Interfaces;

namespace CSV2SQLite.App
{
    public class SQLiteGenerator
    {
        private readonly IFileWrapper _fileWrapper;

        public SQLiteGenerator()
        {
            _fileWrapper = new FileWrapper();
        }

        public SQLiteGenerator(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public bool IsValidCommandLine(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            if (args.Length > 3)
            {
                return false;
            }

            if (_fileWrapper.Exists(args[0]))
            {
                return true;
            }

            return false;
        }

        public void Generate(string inputFile, string outputFile)
        {
            var tableDefinition = new StringBuilder();

            using (var stream = _fileWrapper.Open(inputFile))
            {
                var header = stream.ReadLine();
                if (string.IsNullOrEmpty(header))
                {
                    return;
                }
                tableDefinition.Append("CREATE TABLE test (" + Environment.NewLine);
                tableDefinition.Append("\tid integer PRIMARY KEY," + Environment.NewLine);

                var columns = header.Split(',');

                for (int i = 0; i < columns.Length; i++)
                {
                    tableDefinition.Append(string.Format("\t{0} text", columns[i]));
                    if (i < columns.Length - 1)
                    {
                        tableDefinition.Append(",");
                    }
                    tableDefinition.Append(Environment.NewLine);
                }

                tableDefinition.Append(")");
                Console.WriteLine(tableDefinition.ToString());
                // Then loop through the rest of the file adding the lines as insert statements.
            }
        }
    }
}
