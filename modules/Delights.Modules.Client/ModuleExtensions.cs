using System;
using Modulight.Modules;

namespace Delights.Modules.Client
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddClientModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ClientModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
