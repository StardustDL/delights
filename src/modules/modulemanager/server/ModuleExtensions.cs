using System;
using Modulight.Modules;

namespace Delights.Modules.ModuleManager.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddModuleManagerModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ModuleManagerServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
