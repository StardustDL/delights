using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Delights.Modules
{
    public static class ModuleExtensions
    {
        public static IEnumerable<T> AllSpecifyModules<T>(this IEnumerable<IModule> @this) => @this.Where(m => m is T).Select(m => (T)m);

        public static T GetModule<T>(this IEnumerable<IModule> @this) => AllSpecifyModules<T>(@this).First();

        public static bool TryGetModule<T>(this IEnumerable<IModule> @this, [NotNullWhen(true)] out T? module)
        {
            var result = @this.AllSpecifyModules<T>().FirstOrDefault();
            if (result is not null)
            {
                module = result;
                return true;
            }
            else
            {
                module = default;
                return false;
            }
        }

        public static IModuleHost GetModuleHost(this IServiceProvider services)
        {
            return services.GetRequiredService<IModuleHost>();
        }

        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;
    }
}
