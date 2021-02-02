using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// Extensions for object storage.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add <see cref="ObjectStorageModule"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageModule(this IModuleHostBuilder builder, Action<ObjectStorageModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<ObjectStorageModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureOptions(configureOptions);
            }
            return builder;
        }

        /// <summary>
        /// Add <see cref="ObjectStorageApiModule"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageApiModule(this IModuleHostBuilder builder, Action<ObjectStorageApiModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<ObjectStorageApiModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureOptions(configureOptions);
            }
            return builder;
        }
    }
}
