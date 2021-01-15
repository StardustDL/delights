using System;

namespace StardustDL.AspNet.ObjectStorage
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddObjectStorageModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ObjectStorageModuleOption>? setupOptions = null, Action<ObjectStorageModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ObjectStorageModule, ObjectStorageModuleOption>(setupOptions, configureOptions);
            return modules;
        }

        public static Modulight.Modules.IModuleHostBuilder AddObjectStorageApiModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ObjectStorageApiModuleOption>? setupOptions = null, Action<ObjectStorageApiModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ObjectStorageApiModule, ObjectStorageApiModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
