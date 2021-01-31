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
        /// <param name="modules"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageModule(this IModuleHostBuilder modules, Action<ObjectStorageModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<ObjectStorageModule>();
            if (configureOptions is not null)
            {
                modules.ConfigureServices(services =>
                {
                    services.AddOptions<ObjectStorageModuleOption>().Configure(configureOptions);
                });
            }
            return modules;
        }

        /// <summary>
        /// Add <see cref="ObjectStorageApiModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageApiModule(this IModuleHostBuilder modules, Action<ObjectStorageApiModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<ObjectStorageApiModule>();
            if (configureOptions is not null)
            {
                modules.ConfigureServices(services =>
                {
                    services.AddOptions<ObjectStorageApiModuleOption>().Configure(configureOptions);
                });
            }
            return modules;
        }
    }
}
