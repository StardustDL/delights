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

        public static ModuleManifest GetManifest<TModule>(this IModuleHost host) where TModule : IModule => host.GetManifest(typeof(TModule));

        public static T GetService<TModule, T>(this IModuleHost host, IServiceProvider provider) where T : notnull where TModule : IModule => host.GetService<T>(provider, typeof(TModule));

        public static T GetOption<TModule, T>(this IModuleHost host, IServiceProvider provider) where T : class where TModule : IModule => host.GetOption<T>(provider, typeof(TModule));

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