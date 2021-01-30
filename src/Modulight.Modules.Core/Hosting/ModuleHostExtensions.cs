using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules.Hosting
{
    public static class ModuleHostExtensions
    {
        /// <summary>
        /// Get default module host from service provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IModuleHost GetModuleHost(this IServiceProvider services)
        {
            return services.GetRequiredService<IModuleHost>();
        }

        /// <summary>
        /// Add a typed module.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <returns></returns>
        public static IModuleHostBuilder AddModule<T>(this IModuleHostBuilder builder) where T : IModule => builder.AddModule(typeof(T));

        public static IModuleHostBuilder UsePlugin<T>(this IModuleHostBuilder builder) where T : IModuleHostBuilderPlugin => builder.UsePlugin(typeof(T));

        internal static bool IsHostBuilderPlugin(this Type type) => type.IsAssignableTo(typeof(IModuleHostBuilderPlugin));

        internal static void EnsureHostBuilderPlugin(this Type type)
        {
            if (!type.IsHostBuilderPlugin())
                throw new Exception($"{type.FullName} is not a module host builder plugin.");
        }
    }
}