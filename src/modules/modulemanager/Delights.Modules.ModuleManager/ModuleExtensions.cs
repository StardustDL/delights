using System;
using Modulight.Modules;

namespace Delights.Modules.ModuleManager
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddModuleManagerModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ModuleManagerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
