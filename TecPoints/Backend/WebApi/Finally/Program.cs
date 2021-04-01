
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace DemoWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        // Handle requests up to 500 MB
                        options.Limits.MaxRequestBodySize = 52428800;
                    }).UseStartup<Startup>();
                });
    }
}