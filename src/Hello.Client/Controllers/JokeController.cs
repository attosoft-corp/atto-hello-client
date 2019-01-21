using System.Threading.Tasks;
using Hello.Client.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hello.Client.Commands.JokeCommands;

namespace Hello.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : Controller
    {
        private readonly ILogger<JokeController> _logger;
        private readonly GetJokeCommand _getJokeCommand;

        public JokeController(GetJokeCommand getJokeCommand, ILogger<JokeController> logger)
        {
            _getJokeCommand = getJokeCommand;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<JokeResponse>> GetJoke()
        {
        
            return await _getJokeCommand.GetJoke();
        }
    }
}