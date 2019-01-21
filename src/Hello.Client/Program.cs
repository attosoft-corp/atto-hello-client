using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Hello.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5010")
                .UseKestrel(options =>
            {
                options.AddServerHeader = false;
                options.ConfigureHttpsDefaults(x =>
                {
                    x.SslProtocols = SslProtocols.None;
                    x.CheckCertificateRevocation = false;
                    x.ClientCertificateMode = ClientCertificateMode.NoCertificate;
                    x.HandshakeTimeout = new TimeSpan(1, 0, 0);
                });
            })
                .UseStartup<Startup>();
    }
}
