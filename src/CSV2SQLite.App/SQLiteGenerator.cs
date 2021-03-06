﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CSV2SQLite.App.Exceptions;
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

        public void ValidateCommandLine(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("No arguments provided");
            }

            if (args.Length > 3)
            {
                throw new ArgumentException("Too many arguments provided");
            }

            if (!_fileWrapper.Exists(args[0]))
            {
                throw new FileNotFoundException(string.Format("{0} was not found", args[0]));
            }

            if (!args[0].ToLower().EndsWith(".csv"))
            {
                throw new ArgumentException("Invalid filename");
            }
        }

        public string Generate(Configuration configuration)
        {
            var inputFiles = new List<string>();

            if (configuration.InputFilename.ToLower() == "*.csv")
            {
                foreach (var file in _fileWrapper.GetFiles(Environment.CurrentDirectory, "*.csv"))
                {
                    inputFiles.Add(file);
                }
            }
            else
            {
                inputFiles.Add(configuration.InputFilename);
            }

            var tableDefinitions = new StringBuilder();

            var tableAdded = false;

            foreach (var inputFile in inputFiles)
            {
                var tableDefinition = new StringBuilder();

                using (var stream = _fileWrapper.Open(inputFile))
                {
                    var header = stream.ReadLine();
                    if (string.IsNullOrEmpty(header))
                    {
                        throw new CsvHeaderException(string.Format("{0} is empty", inputFile));
                    }

                    var columns = header.Split(',');

                    if (!tableAdded)
                    {
                        tableDefinition.Append("CREATE TABLE test (" + Environment.NewLine);
                        tableDefinition.Append("\tid integer PRIMARY KEY," + Environment.NewLine);

                        tableDefinition.Append(AddColumns(columns));
                        tableDefinition.Append(");" + Environment.NewLine);
                    }

                    tableAdded = true;

                    var data = stream.ReadLine();
                    while (!string.IsNullOrEmpty(data))
                    {
                        var insert = new StringBuilder("INSERT INTO test (");
                        insert.Append(AddInsertColumns(columns));
                        insert.Append(")" + Environment.NewLine);

                        var rowValues = data.Split(',');

                        insert.Append("VALUES (");
                        insert.Append(AddValues(rowValues));
                        insert.Append(");" + Environment.NewLine);

                        tableDefinition.Append(insert);

                        data = stream.ReadLine();
                    }
                }

                tableDefinitions.Append(tableDefinition);
            }

            return tableDefinitions.ToString();
        }

        public void Write(string sqlData, string outputFile)
        {
            _fileWrapper.WriteText(sqlData, outputFile);
        }

        private string AddValues(IReadOnlyList<string> values)
        {
            var output = new StringBuilder();
            for (var i = 0; i < values.Count; i++)
            {
                var s = Regex.Replace(values[i].Trim(), "'", "''");

                output.Append(string.Format("'{0}'", s));
                if (i < values.Count - 1)
                {
                    output.Append(", ");
                }
            }

            return output.ToString();
        }

        private string AddInsertColumns(IReadOnlyList<string> columns)
        {
            var output = new StringBuilder();

            for (var i = 0; i < columns.Count; i++)
            {
                output.Append(columns[i]);
                if (i < columns.Count - 1)
                {
                    output.Append(", ");
                }
            }

            return output.ToString();
        }

        private string AddColumns(IReadOnlyList<string> columns)
        {
            var output = new StringBuilder();

            for (var i = 0; i < columns.Count; i++)
            {
                output.Append(string.Format("\t{0} text", columns[i]));
                if (i < columns.Count - 1)
                {
                    output.Append(",");
                }
                output.Append(Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
