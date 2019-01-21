using Hello.Client.Models.Data;

namespace Hello.Client.Models.Response
{
    public class JokeResponse
    {
        public string Type { get; set; } 
        public JokeData Value { get; set; }
    }
}