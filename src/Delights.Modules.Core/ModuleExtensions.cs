using Microsoft.Extensions.DependencyInjection;
using System;

namespace Delights.Modules
{
    public static class ModuleExtensions
    {
        public static IModuleHost AddModuleHost(this IServiceCollection services)
        {
            IModuleHost modules = new ModuleHost(services);
            services.AddSingleton(modules);
            return modules;
        }

        public static IModuleHost GetModuleHost(this IServiceProvider services)
        {
            return services.GetRequiredService<IModuleHost>();
        }

        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;
    }
}
