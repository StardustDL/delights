using System;
using Modulight.Modules;

namespace Delights.Modules.Notes
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddNotesModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<NotesModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
