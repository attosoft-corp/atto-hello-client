using Hello.Client.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hello.Client.Services.JokeServices
{
    public interface IJokeService
    {
        void AddJoke();

        Task AddJokeAsync();

        JokeResponse GetJoke();

        JokeResponse GetJoke(string id);

        Task<JokeResponse> GetJokeAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<JokeResponse> GetJokeAsync(string id);

        Task<IEnumerable<JokeResponse>> GetAllJokeAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}