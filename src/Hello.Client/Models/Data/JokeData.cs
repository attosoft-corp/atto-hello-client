using System.Collections.Generic;

namespace Hello.Client.Models.Data
{
    public class JokeData
    {
        public int Id { get; set; }
        public string Joke { get; set; }
        public List<string> categories { get; set; } = new List<string>();
    }
}