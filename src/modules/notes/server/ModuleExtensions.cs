using System;
using Modulight.Modules;

namespace Delights.Modules.Notes.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddNotesModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<NotesServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
