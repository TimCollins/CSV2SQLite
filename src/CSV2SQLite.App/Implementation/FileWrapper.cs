using System.IO;
using CSV2SQLite.App.Interfaces;

namespace CSV2SQLite.App.Implementation
{
    internal class FileWrapper : IFileWrapper
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public StreamReader Open(string path)
        {
            return new StreamReader(path);
        }

        public string[] GetHeaderData(string path)
        {
            var reader = File.OpenText(path);
            var header = reader.ReadLine();

            if (header == null)
            {
                return new string[0];
            }

            var columns = header.Split(',');

            return columns;
        }
    }
}
