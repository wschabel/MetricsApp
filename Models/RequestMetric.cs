namespace MetricsApp.Models
{
    public class RequestMetric
    {
        public string Path { get; set; } = "";
        public long Duration { get; set; }

        public RequestMetric(string path, long duration)
        {
            Path = path;
            Duration = duration;
        }
    }
}
