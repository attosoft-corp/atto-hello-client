using System;
using Hello.Client.Services.JokeServices;
using Hello.Client.Services.JokeServices.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.CircuitBreaker.Hystrix;
using Hello.Client.Commands.JokeCommands;
using Steeltoe.Common.Http.Discovery;
using Pivotal.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Hello.Client.Configurations;
using RawRabbit.Instantiation;
using System.IO;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit;

namespace Hello.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jokeOptions = Configuration.GetSection(JokeOptions.key).Get<JokeOptions>();

            // Optional:  Adds IConfiguration and IConfigurationRoot to service container
            services.AddConfiguration(Configuration);

            //Add Eureka for Discovery services
            services.AddDiscoveryClient(Configuration);

            //Add Command Joke to container
            services.AddHystrixCommand<GetJokeCommand>("JokeCommand", Configuration);

            //Add Service
            services.AddScoped<IJokeService, JokeService>();

            // Add Steeltoe handler to container
            services.AddTransient<DiscoveryHttpMessageHandler>();

            //Add HttpClientFactor to the container
            services.AddHttpClient("joke-service", c =>
                c.BaseAddress = new Uri(jokeOptions.Url)).AddHttpMessageHandler<DiscoveryHttpMessageHandler>();

            //Add Mvc with compatibility
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Hystrix metrics stream to enable monitoring 
            services.AddHystrixMetricsStream(Configuration);



            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = Configuration.GetSection("RawRabbit").Get<RawRabbitConfiguration>(),
                Plugins = p => p
                    .UseGlobalExecutionId()
            });

            services.AddSingleton<IBusClient>(client);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHystrixRequestContext();

            app.UseHttpsRedirection();

            app.UseDiscoveryClient();

            app.UseMvc();

            app.UseHystrixMetricsStream();
        }
    }
}
