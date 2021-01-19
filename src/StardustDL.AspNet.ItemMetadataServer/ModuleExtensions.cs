using Modulight.Modules;
using System;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddItemMetadataServerModule(this IModuleHostBuilder modules, Action<ItemMetadataServerModuleOption>? setupOptions = null, Action<ItemMetadataServerModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ItemMetadataServerModule, ItemMetadataServerModuleOption>(setupOptions, configureOptions);
            return modules;
        }

        public static IModuleHostBuilder AddItemMetadataServerGraphQLModule(this IModuleHostBuilder modules, Action<ItemMetadataServerGraphqlModuleOption>? setupOptions = null, Action<ItemMetadataServerGraphqlModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ItemMetadataServerGraphqlModule, ItemMetadataServerGraphqlModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
