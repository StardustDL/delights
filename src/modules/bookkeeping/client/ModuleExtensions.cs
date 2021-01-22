using System;
using Modulight.Modules;

namespace Delights.Modules.Bookkeeping
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddBookkeepingModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<BookkeepingModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
