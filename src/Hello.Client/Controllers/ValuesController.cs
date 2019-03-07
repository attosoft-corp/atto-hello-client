using Hello.Client.Services.JokeServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hello.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(IConfiguration configuration, ILogger<ValuesController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var config = _configuration["info:description"];

            _logger.LogInformation("Hello, {Name}, from NLog!", Environment.UserName);

            return await Task.FromResult(Ok(new List<string> { config }));
        }
    }
}