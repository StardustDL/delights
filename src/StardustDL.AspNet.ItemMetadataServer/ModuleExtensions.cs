using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddItemMetadataServerModule(this IModuleHostBuilder builder, Action<ItemMetadataServerModuleStartupOption>? configureStartupOption = null)
        {
            builder.AddModule<ItemMetadataServerModule>();
            if (configureStartupOption is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.Configure(configureStartupOption);
                });
            }
            return builder;
        }

        public static IModuleHostBuilder AddItemMetadataServerGraphqlModule(this IModuleHostBuilder modules)
        {
            modules.AddModule<GraphQL.ItemMetadataServerGraphqlModule>();
            return modules;
        }
    }
}
