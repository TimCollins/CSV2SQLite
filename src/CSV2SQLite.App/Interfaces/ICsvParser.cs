namespace CSV2SQLite.App.Interfaces
{
    public interface ICsvParser
    {
        string[] GetHeaderData(string input);
    }
}
