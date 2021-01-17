using Modulight.Modules;
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
        /// <param name="setupOptions"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageModule(this IModuleHostBuilder modules, Action<ObjectStorageModuleOption>? setupOptions = null, Action<ObjectStorageModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ObjectStorageModule, ObjectStorageModuleOption>(setupOptions, configureOptions);
            return modules;
        }

        /// <summary>
        /// Add <see cref="ObjectStorageApiModule"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="setupOptions"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddObjectStorageApiModule(this IModuleHostBuilder modules, Action<ObjectStorageApiModuleOption>? setupOptions = null, Action<ObjectStorageApiModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ObjectStorageApiModule, ObjectStorageApiModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
