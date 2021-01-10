using Delights.Modules;
using Delights.Modules.Client.RazorComponents;
using Delights.Modules.Client.RazorComponents.UI;
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

namespace Delights.Client.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Delights.UI.App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var graphqlEndpoint = "https://localhost:5001/graphql";

            var modules = builder.Services.AddModules()
                .AddClientModules()
                .AddModule<UI.UIModule>()
                .AddHelloModule(o =>
                {
                    o.GraphQLEndpoint = graphqlEndpoint;
                })
                .AddModuleManagerModule(o =>
                {
                    o.GraphQLEndpoint = graphqlEndpoint;
                });


            await using (var provider = builder.Services.BuildServiceProvider())
            {
                await modules.Initialize(provider);
            }

            await builder.Build().RunAsync();
        }
    }
}
