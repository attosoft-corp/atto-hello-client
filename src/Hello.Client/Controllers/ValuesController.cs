using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace Hello.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ValuesController> _logger;
        private readonly IBusClient _client;


        public ValuesController(IConfiguration configuration, ILogger<ValuesController> logger, IBusClient client)
        {
            _configuration = configuration;
            _logger = logger;
            _client = client;
        }
        // GET api/values
        [HttpGet]
        public async ActionResult<IEnumerable<string>> Get()
        {
            var config = _configuration["info:description"];
            var exchange = _configuration["info:exchange"];
            var queue = _configuration["info:queue"];


            var message = new ValueRequest { Value = 6 };
            var list = new List<string>();
            for (int i = 0; i < 30; i++)
            {
                var response = await _client.RequestAsync<ValueRequest, ValueResponse>(message, x =>
                x.UseRequestConfiguration(r =>
                {
                    r.PublishRequest(pr => pr.OnExchange(exchange));
                    r.ConsumeResponse(cr =>
                    {
                        cr.FromDeclaredQueue(t => t.WithName(queue));
                        cr.OnDeclaredExchange(q => q.WithName(exchange));

                    });
                }));

                list.Add($"{response.Value} : {i} ");
                _logger.LogInformation($"{response.Value} : {i} ");
            }
            
            return list;


        }
    }

    public class ValueResponse
    {
        public string Value { get; set; }
    }

    public class ValueRequest
    {
        public int Value { get; set; }
    }
}
