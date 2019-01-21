using System;
using System.Threading.Tasks;
using Hello.Client.Models.Response;
using Hello.Client.Services.JokeServices;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Hello.Client.Commands.JokeCommands
{
    public class GetJokeCommand : HystrixCommand<JokeResponse>
    {
        private readonly IJokeService _service;
        public GetJokeCommand(IHystrixCommandOptions commandOptions, IJokeService service) : base(commandOptions)
        {
            _service = service;
            this.IsFallbackUserDefined = true;
        }

        public async Task<JokeResponse> GetJoke()
        {
            return await ExecuteAsync();
        }

        protected async override Task<JokeResponse> RunAsync()
        {
            return await _service.GetJokeAsync();
        }
        protected async override Task<JokeResponse> RunFallbackAsync()
        {
            var joke= new JokeResponse();
            joke.Type = "-1";
            joke.Value = new Models.Data.JokeData(){ Id = -1, Joke = "Sorry we don't have a joke in this moment, try latter" };
            return await Task.FromResult(joke);
        }

    }
}