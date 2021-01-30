using System;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Hosting;

namespace Delights.Modules.ModuleManager
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddModuleManagerModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<ModuleManagerModule>();
            if (configureOptions is not null)
            {
                modules.ConfigureServices(services =>
                {
                    services.AddOptions<ModuleOption>().Configure(configureOptions);
                });
            }
            return modules;
        }
    }
}
