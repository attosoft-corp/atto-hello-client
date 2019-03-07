using Atto.Common.Core.Program;
using Microsoft.AspNetCore.Hosting;

namespace Hello.Client
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            HostCore.CreateWebHostBuilder(args)
                .UseStartup<Startup>();
    }
}