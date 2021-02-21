using Delights.Client.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor;

namespace Delights.Client.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Modulight.UI.Blazor.App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddModules(builder =>
            {
                builder.AddBlazorUI<Delights.UI.DelightsBlazorUIProvider>()
                    .UseDefaults();
            });

            var host = builder.Build();

            await using var _ = await host.Services.UseModuleHost();

            /*{
                    var service = provider.GetRequiredService<Modules.Persons.ModuleService>();
                    Console.WriteLine((await service.GraphQLClient.GetDumpAsync()).Data.Dump.Base64);
                }
                {
                    var service = provider.GetRequiredService<Modules.Bookkeeping.ModuleService>();
                    Console.WriteLine((await service.GraphQLClient.GetDumpAsync()).Data.Dump.Base64);
                }
                {
                    var service = provider.GetRequiredService<Modules.Notes.ModuleService>();
                    Console.WriteLine((await service.GraphQLClient.GetDumpAsync()).Data.Dump.Base64);
                }*/

            await host.RunAsync();
        }
    }
}
