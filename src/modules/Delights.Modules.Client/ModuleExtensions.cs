using System;
using Modulight.Modules;
using Modulight.Modules.Hosting;

namespace Delights.Modules.Client
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddClientModule(this IModuleHostBuilder modules)
        {
            modules.AddModule<ClientModule>();
            return modules;
        }
    }
}
