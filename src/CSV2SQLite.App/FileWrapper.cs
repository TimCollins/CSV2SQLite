using System.IO;

namespace CSV2SQLite.App
{
    internal class FileWrapper : IFileWrapper
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
