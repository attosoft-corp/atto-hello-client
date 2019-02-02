namespace Hello.Client.Configurations
{
    public class JokeOptions
    {
        public readonly static string key = "atto-joke-config";
        public string Url { get; set; } = "http://localhost:8080";
    }
}