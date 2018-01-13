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

                tableDefinition.Append(")" + Environment.NewLine);

                var data = stream.ReadLine();
                while (!string.IsNullOrEmpty(data))
                {
                    var insert = new StringBuilder("INSERT INTO test (");
                    for (int i = 0; i < columns.Length; i++)
                    {
                        insert.Append(columns[i]);
                        if (i < columns.Length - 1)
                        {
                            insert.Append(",");
                        }
                    }

                    insert.Append(")" + Environment.NewLine);

                    var rowValues = data.Split(',');
                    insert.Append("VALUES (");

                    for (var i = 0; i < rowValues.Length; i++)
                    {
                        insert.Append(string.Format("'{0}'", rowValues[i]));
                        if (i < rowValues.Length - 1)
                        {
                            insert.Append(", ");
                        }
                    }

                    insert.Append(");" + Environment.NewLine);
                    tableDefinition.Append(insert);

                    data = stream.ReadLine();
                }

                Console.WriteLine(tableDefinition.ToString());
            }
        }
    }
}
