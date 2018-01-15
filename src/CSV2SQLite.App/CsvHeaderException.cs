using System;

namespace CSV2SQLite.App
{
    public class CsvHeaderException : ApplicationException
    {
        public CsvHeaderException(string message)
            :base(message)
        {
        }
    }
}
