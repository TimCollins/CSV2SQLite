using CSV2SQLite.App.Implementation;
using CSV2SQLite.App.Interfaces;

namespace CSV2SQLite.App
{
    public class SQLiteGenerator
    {
        private readonly IFileWrapper _fileWrapper;
        //private readonly ICsvParser _csvParser;

        public SQLiteGenerator()
        {
            _fileWrapper = new FileWrapper();
            //_csvParser = new CsvParser();
        }

        public SQLiteGenerator(IFileWrapper fileWrapper)
        //public SQLiteGenerator(IFileWrapper fileWrapper, ICsvParser csvParser)
        {
            _fileWrapper = fileWrapper;
            //_csvParser = csvParser;
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

        public void Generate(string input)
        {
        }
    }
}
