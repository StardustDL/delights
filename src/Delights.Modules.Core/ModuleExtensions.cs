using Microsoft.Extensions.DependencyInjection;
using System;

namespace Delights.Modules
{
    public static class ModuleExtensions
    {
        public static ModuleCollection AddModules(this IServiceCollection services)
        {
            ModuleCollection modules = new(services);
            services.AddSingleton(modules);
            return modules;
        }

        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;
    }
}
