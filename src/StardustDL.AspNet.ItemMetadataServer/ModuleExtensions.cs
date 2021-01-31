using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddItemMetadataServerModule(this IModuleHostBuilder builder, Action<ItemMetadataServerModuleStartupOption, IServiceProvider>? configureStartupOptions = null)
        {
            builder.AddModule<ItemMetadataServerModule>();
            if (configureStartupOptions is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.AddOptions<ItemMetadataServerModuleStartupOption>().Configure(configureStartupOptions);
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
