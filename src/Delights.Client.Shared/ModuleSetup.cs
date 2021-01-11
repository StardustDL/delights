using Delights.Modules;
using Delights.Modules.Client.RazorComponents;
using Delights.Modules.Hello;
using Delights.Modules.ModuleManager;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Delights.Client.Shared
{
    public static class ModuleSetup
    {
        public static IModuleHostBuilder CreateDefaultBuilder()
        {
            var graphqlEndpoint = "https://localhost:5001/graphql";

            var builder = ModuleHostBuilder.CreateDefaultBuilder();
                builder.AddRazorComponentClientModules((o, _) =>
                {
                    o.Validation = true;
                })
                .AddModule<UI.UIModule>()
                .AddHelloModule((o, _) =>
                {
                    o.GraphQLEndpoint = graphqlEndpoint;
                })
                .AddModuleManagerModule((o, _) =>
                {
                    o.GraphQLEndpoint = graphqlEndpoint;
                });

            return builder;
        }
    }
}
