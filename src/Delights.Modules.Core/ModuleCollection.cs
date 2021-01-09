using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Delights.Modules
{

    public class ModuleCollection
    {
        public ModuleCollection(IServiceCollection services) => Services = services;

        IServiceCollection Services { get; }

        public IList<Module> Modules { get; } = new List<Module>();

        public ModuleCollection AddModule<T>()
            where T : Module, new() => AddModule(new T());

        public ModuleCollection AddModule<T>(T module)
            where T : Module
        {
            Modules.Add(module);
            Services.TryAddSingleton(module);
            module.RegisterService(Services);
            return this;
        }
    }
}
