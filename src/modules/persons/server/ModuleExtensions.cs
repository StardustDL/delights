using System;
using Modulight.Modules;

namespace Delights.Modules.Persons.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddPersonsModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<PersonsServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
