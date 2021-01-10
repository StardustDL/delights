using Microsoft.Extensions.DependencyInjection;
using System;

namespace Delights.Modules
{
    public static class ModuleExtensions
    {
        public static IModuleCollection AddModules(this IServiceCollection services)
        {
            IModuleCollection modules = new ModuleCollection(services);
            services.AddSingleton(modules);
            return modules;
        }

        public static IModuleCollection GetModules(this IServiceProvider services)
        {
            return services.GetRequiredService<IModuleCollection>();
        }

        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;
    }
}
