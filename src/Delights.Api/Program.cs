using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Modulight.Modules.Hosting;
using System.Threading.Tasks;

namespace Delights.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await using var _ = await host.Services.UseModuleHost();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
