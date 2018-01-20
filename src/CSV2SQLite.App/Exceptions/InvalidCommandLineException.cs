using System;

namespace CSV2SQLite.App.Exceptions
{
    public class InvalidCommandLineException : Exception
    {
        public InvalidCommandLineException(string message)
            :base(message)
        {
        }
    }
}
