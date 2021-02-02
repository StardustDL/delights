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
                builder.ConfigureBuilderOptions(configureStartupOptions);
            }
            return builder;
        }

        public static IModuleHostBuilder AddItemMetadataServerGraphqlModule(this IModuleHostBuilder builder) => builder.AddModule<GraphQL.ItemMetadataServerGraphqlModule>();
    }
}
