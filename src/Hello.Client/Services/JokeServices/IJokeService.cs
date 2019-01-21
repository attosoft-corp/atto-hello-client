using System.Threading.Tasks;
using Hello.Client.Models.Response;

namespace Hello.Client.Services.JokeServices
{
    public interface IJokeService
    {
      Task<JokeResponse> GetJokeAsync();   
    }
}