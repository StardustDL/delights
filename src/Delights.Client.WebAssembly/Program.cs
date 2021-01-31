using Delights.Client.Shared;
using Delights.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Delights.Modules.Hello;
using Delights.Modules.ModuleManager;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Modulight.Modules;

namespace Delights.Client.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Delights.UI.App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddServerConfiguration();
            ModuleSetup.CreateDefaultBuilder(false).Build(builder.Services);

            await using(var provider = builder.Services.BuildServiceProvider())
            {
                await provider.GetRazorComponentClientModuleCollection().LoadResources();

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
            }

            await builder.Build().RunAsync();
        }
    }
}
