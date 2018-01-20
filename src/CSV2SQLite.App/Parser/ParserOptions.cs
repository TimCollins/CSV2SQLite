namespace CommandLineParse.App
{
    public class ParserOptions
    {
        public bool ShowHelpScreen { get; set; }
        public bool UseCustomConfig { get; set; }
        public string CustomConfig { get; set; }
        public bool UseCustomOutputFile { get; set; }
        public string CustomOutputFile { get; set; }
    }
}
