using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
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
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageModule(this IModuleHostBuilder modules, Action<ObjectStorageModuleOption>? configureOptions = null)
        {
            modules.AddModule<ObjectStorageModule>();
            if (configureOptions is not null)
            {
                modules.ConfigureServices(services =>
                {
                    services.Configure(configureOptions);
                });
            }
            return modules;
        }

        /// <summary>
        /// Add <see cref="ObjectStorageApiModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageApiModule(this IModuleHostBuilder modules, Action<ObjectStorageApiModuleOption>? configureOptions = null)
        {
            modules.AddModule<ObjectStorageApiModule>();
            if (configureOptions is not null)
            {
                modules.ConfigureServices(services =>
                {
                    services.Configure(configureOptions);
                });
            }
            return modules;
        }
    }
}
