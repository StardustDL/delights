using Delights.Modules;
using Delights.Modules.Client.RazorComponents;
using Delights.Modules.Hello;
using Delights.Modules.ModuleManager;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Delights.Client.Shared
{
    public static class ModuleSetupExtensions
    {
        public static IModuleHost AddAllModules(this IServiceCollection collection)
        {
            var graphqlEndpoint = "https://localhost:5001/graphql";

            return collection.AddModuleHost()
                .AddClientModules((o, _) =>
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
        }
    }
}
