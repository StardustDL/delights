using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delights.Modules
{

    internal class ModuleHost : IModuleHost
    {
        public ModuleHost(IServiceCollection services) => Services = services;

        public IServiceCollection Services { get; }

        public IList<IModule> Modules { get; } = new List<IModule>();

        public IModuleHost AddModule<T>(T module)
            where T : class, IModule
        {
            Modules.Add(module);
            Services.AddSingleton(module);
            module.RegisterService(Services);
            module.Setup(this, Services);
            return this;
        }

        public IEnumerable<T> AllSpecifyModules<T>()
        {
            return Modules.Where(m => m is T).Select(m => (T)m);
        }

        public async Task Initialize(IServiceProvider provider)
        {
            foreach (var module in Modules)
            {
                await module.Initialize(provider);
            }
        }
    }
}
