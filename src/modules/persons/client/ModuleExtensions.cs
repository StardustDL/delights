using System;
using Modulight.Modules;

namespace Delights.Modules.Persons
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddPersonsModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<PersonsModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
