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
            {
                // here we can add services name from eureka.
                c.BaseAddress = new Uri("http://hello-service:5000");

            }).AddHttpMessageHandler<DiscoveryHttpMessageHandler>();

            //Add Mvc with compatibility
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Hystrix metrics stream to enable monitoring 
            services.AddHystrixMetricsStream(Configuration);

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
            app.UseDeveloperExceptionPage();
            app.UseHystrixRequestContext();

            app.UseHttpsRedirection();

            app.UseDiscoveryClient();

            app.UseMvc();

            app.UseHystrixMetricsStream();
        }
    }
}
