using Delights.Modules.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules
{
    public interface IModuleCollection
    {
        IList<IModule> Modules { get; }

        IModuleCollection AddModule<T, TOption>(Action<TOption>? configureOptions = null)
            where T : class, IModule, new()
            where TOption : ModuleOption;

        IModuleCollection AddModule<T>()
            where T : class, IModule, new();

        IModuleCollection AddModule<T>(T module)
            where T : class, IModule;

        IEnumerable<T> AllSpecifyModules<T>();

        Task Initialize(IServiceProvider provider);
    }
}