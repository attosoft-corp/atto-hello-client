using Hello.Client.Models.Response;
using Hello.Client.Services.JokeServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Hello.Client.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : Controller
    {
        private readonly IJokeService _jokeService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jokeService"></param>
        /// <param name="logger"></param>
        public JokeController(IJokeService jokeService, IConfiguration configuration)
        {
            _jokeService = jokeService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<JokeResponse>> GetJokeAsync()
        {
            return await _jokeService.GetJokeAsync();
        }
    }
}