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
    }
}
