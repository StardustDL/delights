using Modulight.Modules.Client.RazorComponents;
using Delights.Modules.Hello;
using Delights.Modules.ModuleManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Delights.Client.Shared
{
    public static class ModuleSetup
    {
        public static Modulight.Modules.IModuleHostBuilder CreateDefaultBuilder()
        {
            var builder = Modulight.Modules.ModuleHostBuilder.CreateDefaultBuilder()
                .AddRazorComponentClientModules((o, _) =>
                {
                    o.Validation = true;
                })
                .AddModule<UI.UIModule>()
                .AddHelloModule((o, sp) =>
                {
                    var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
                    o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
                })
                .AddModuleManagerModule((o, sp) =>
                {
                    var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
                    o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
                });

            return builder;
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection AddServerConfiguration(this IServiceCollection services)
        {
            services.AddOptions<ServerConfiguration>().Configure(o =>
            {
                o.GraphQLEndpoint = "https://localhost:5001/graphql";
            });
            return services;
        }
    }
}
