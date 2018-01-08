namespace CSV2SQLite.App
{
    public class SQLiteGenerator
    {
        public bool IsValidCommandLine(string[] args)
        {
            if (args.Length > 0 && args.Length < 4)
            {
                return true;
            }

            return false;
        }
    }
}
