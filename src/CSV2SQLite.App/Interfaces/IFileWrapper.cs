using System.Collections.Generic;
using System.IO;

namespace CSV2SQLite.App.Interfaces
{
    public interface IFileWrapper
    {
        bool Exists(string path);
        StreamReader Open(string path);
        void WriteText(string tableDefinition, string outputFile);
        IEnumerable<string> GetFiles(string path, string searchPattern);
    }
}
