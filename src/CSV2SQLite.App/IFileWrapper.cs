namespace CSV2SQLite.App
{
    public interface IFileWrapper
    {
        bool Exists(string path);
    }
}
