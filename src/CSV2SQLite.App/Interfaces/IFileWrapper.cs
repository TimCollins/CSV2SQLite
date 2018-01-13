namespace CSV2SQLite.App.Interfaces
{
    public interface IFileWrapper
    {
        bool Exists(string path);
        string[] GetHeaderData(string path);
    }
}
