using Atto.Common.Core.Attributes;
using Atto.Common.Core.Hystrixs.Models;
using Hello.Client.Models.Data;
using Hello.Client.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hello.Client.Services.JokeServices.Impl
{
    [HystrixInterceptor]
    public class JokeService : IJokeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JokeService> _logger;

        public JokeService(IHttpClientFactory httpClientFactory, ILogger<JokeService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("joke-service");
            _logger = logger;
        }

        public void AddJoke()
        {
            _logger.LogInformation("this method need to be implemented to record a joke");
        }
        public void AddJoke(HystrixFallback hystrixFallback)
        {
            _logger.LogInformation("this method need to be implemented to record a joke");
        }

        public Task AddJokeAsync()
        {
            _logger.LogInformation("this method need to be implemented to record a joke");
            return Task.CompletedTask;
        }
        public Task AddJokeAsync(HystrixFallback hystrixFallback)
        {
            _logger.LogInformation("this method need to be implemented to record a joke");
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<JokeResponse>> GetAllJokeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = Enumerable.Range(0, 3).Select(async x =>
            {
                var response = await _httpClient.GetAsync("api/LegacyJoke");

                var content = await response.Content.ReadAsStringAsync();
                var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(content);

                _logger.LogInformation(content);

                return jokeResponse;
            }).ToArray();

            return await Task.WhenAll(result);
        }
        public Task<IEnumerable<JokeResponse>> GetAllJokeAsync(HystrixFallback hystrixFallback, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = Enumerable.Range(0, 20).Select((x, i) => DefaultFallback(i.ToString()));
            return Task.FromResult(result);
        }

        public JokeResponse GetJoke()
        {
            var joke = GetJokeAsync().GetAwaiter().GetResult();
            joke.Type = "this is a joke from sync method";
            return joke;
        }
        public JokeResponse GetJoke(HystrixFallback hystrixFallback)
        {
            return DefaultFallback("teste");
        }

        public JokeResponse GetJoke(string id)
        {
            var joke = GetJokeAsync().GetAwaiter().GetResult();
            joke.Type = "this is a joke from sync method with id" + id;
            return joke;
        }
        public JokeResponse GetJoke(HystrixFallback hystrixFallback, string id)
        {
            return DefaultFallback(id);
        }

        public async Task<JokeResponse> GetJokeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _httpClient.GetAsync("api/LegacyJoke");

            var content = await response.Content.ReadAsStringAsync();
            var jokeResponse = JsonConvert.DeserializeObject<JokeResponse>(content);

            _logger.LogInformation(content);

            return jokeResponse;
        }
        public Task<JokeResponse> GetJokeAsync(HystrixFallback hystrixFallback, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = DefaultFallback("Fallback");
            return Task.FromResult(result);
        }


        public Task<JokeResponse> GetJokeAsync(string id)
        {
            return Task.FromResult(new JokeResponse { Type = id, Value = new Models.Data.JokeData { Joke = "Joke From async" } });
        }
        public Task<JokeResponse> GetJokeAsync(HystrixFallback hystrixFallback, string id)
        {
            var result = DefaultFallback(id);
            return Task.FromResult(result);
        }

        private JokeResponse DefaultFallback(string type) =>
            new JokeResponse()
            {
                Type = type,
                Value = new JokeData
                {
                    Joke = "We don't have a joke in this momment, try later",
                    categories = new List<string> { "fallback", "error" },
                    Id = -1
                }
            };
    }
}