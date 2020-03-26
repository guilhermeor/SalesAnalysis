namespace SalesAnalysis.Workers
{
    public class Settings
    {
        public PathSettings PathSettings { get; set; }
    }
    public class PathSettings
    {
        public string Default { get; set; }
        public string In { get; set; }
        public string Out { get; set; }
    }
}
