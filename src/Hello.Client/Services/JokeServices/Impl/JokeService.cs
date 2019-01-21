using System.Net.Http;
using System.Threading.Tasks;
using Hello.Client.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;

namespace Hello.Client.Services.JokeServices.Impl
{
    public class JokeService : IJokeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JokeService> _logger;

        public JokeService(IHttpClientFactory httpClientFactory, ILogger<JokeService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("joke-service");
            _logger = logger;

        }
        public async Task<JokeResponse> GetJokeAsync()
        {
            var response = await _httpClient.GetAsync("/api/LegacyJoke");

            var content = await response.Content.ReadAsStringAsync();
            var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(content);

            _logger.LogInformation(content);

            return jokeResponse;

        }
    }
}