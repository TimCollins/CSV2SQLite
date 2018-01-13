using CSV2SQLite.App.Interfaces;

namespace CSV2SQLite.App.Implementation
{
    public class CsvParser: ICsvParser
    {
        public string[] GetHeaderData(string input)
        {
            return new[] {"Hi"};
        }
    }
}
