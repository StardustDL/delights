using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules
{

    internal class ModuleHost : IModuleHost
    {
        public ModuleHost(IServiceProvider services, IList<IModule> modules)
        {
            Services = services;
            Modules = modules;
        }

        public IServiceProvider Services { get; }

        public IList<IModule> Modules { get; }

        public IEnumerable<T> AllSpecifyModules<T>()
        {
            return Modules.Where(m => m is T).Select(m => (T)m);
        }

        public async Task Initialize()
        {
            using var scope = Services.CreateScope();
            foreach (var module in Modules)
            {
                await module.GetService(scope.ServiceProvider).Initialize();
            }
        }
    }
}
