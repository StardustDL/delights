using Modulight.Modules;
using System;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddItemMetadataServerModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ItemMetadataServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }

        public static IModuleHostBuilder AddItemMetadataServerGraphqlModule(this IModuleHostBuilder modules, Action<GraphQL.ModuleOption>? setupOptions = null, Action<GraphQL.ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<GraphQL.ItemMetadataServerGraphqlModule, GraphQL.ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
