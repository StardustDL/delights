using System;
using Modulight.Modules;

namespace Delights.Modules.Bookkeeping.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddBookkeepingModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<BookkeepingServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
