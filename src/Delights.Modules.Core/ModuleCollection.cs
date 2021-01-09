using Delights.Modules.Options;
using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delights.Modules
{

    public class ModuleCollection
    {
        public ModuleCollection(IServiceCollection services) => Services = services;

        protected IServiceCollection Services { get; }

        public IList<IModule> Modules { get; } = new List<IModule>();

        public ModuleCollection AddModule<T>()
            where T : class, IModule, new() => AddModule(new T());

        public ModuleCollection AddModule<T>(T module)
            where T : class, IModule
        {
            Modules.Add(module);
            Services.TryAddSingleton(module);
            module.RegisterOptions(Services);
            module.RegisterService(Services);
            return this;
        }

        public ModuleCollection AddModule<T, TOption>(Action<TOption>? configureOptions = null) where
            T : class, IModule, new() where TOption : ModuleOption
        {
            AddModule<T>();
            if (configureOptions is not null)
            {
                Services.Configure(configureOptions);
            }
            return this;
        }

        public IEnumerable<T> AllSpecifyModules<T>()
        {
            return Modules.Where(m => m is T).Select(m => (T)m);
        }

        public async Task Initialize(IServiceProvider provider)
        {
            foreach(var module in Modules)
            {
                await module.Initialize(provider);
            }
        }
    }
}
