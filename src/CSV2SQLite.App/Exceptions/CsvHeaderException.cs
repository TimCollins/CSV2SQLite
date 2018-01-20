using System;

namespace CSV2SQLite.App.Exceptions
{
    public class CsvHeaderException : ApplicationException
    {
        public CsvHeaderException(string message)
            :base(message)
        {
        }
    }
}
