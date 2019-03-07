using System;
using Hello.Client.Configurations;
using Hello.Client.Controllers;
using Hello.Client.Services.JokeServices;
using Hello.Client.Services.JokeServices.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Http.Discovery;

namespace Hello.Client
{
    public class AppAutoConfig 
    {
        private readonly IConfiguration _configuration;

        public AppAutoConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var jokeOptions = _configuration.GetSection(JokeOptions.key).Get<JokeOptions>();

            services.AddScoped<IJokeService, JokeService>();
            services.AddHttpClient("joke-service", c => c.BaseAddress = new Uri(jokeOptions.Url)).AddHttpMessageHandler<DiscoveryHttpMessageHandler>();

            return services;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
