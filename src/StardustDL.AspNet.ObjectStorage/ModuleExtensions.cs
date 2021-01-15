using System;

namespace StardustDL.AspNet.ObjectStorage
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddObjectStorageModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ObjectStorageModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ObjectStorageModule, ObjectStorageModuleOption>(configureOptions);
            return modules;
        }

        public static Modulight.Modules.IModuleHostBuilder AddObjectStorageApiModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ObjectStorageApiModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule(() => new ObjectStorageApiModule(), configureOptions);
            return modules;
        }
    }
}
