namespace CSV2SQLite.App
{
    public class SQLiteGenerator
    {
        private readonly IFileWrapper _fileWrapper;

        public SQLiteGenerator()
        {
            _fileWrapper = new FileWrapper();
        }

        public SQLiteGenerator(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public bool IsValidCommandLine(string[] args)
        {
            if (args.Length == 0)
            {
                return false;
            }

            if (args.Length > 3)
            {
                return false;
            }

            if (_fileWrapper.Exists(args[0]))
            {
                return true;
            }

            return false;
        }
    }
}
