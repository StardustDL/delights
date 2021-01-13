using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    public interface IModuleHost
    {
        IReadOnlyList<IModule> Modules { get; }
    }

    internal class ModuleHost : IModuleHost
    {
        public ModuleHost(IServiceProvider services, IReadOnlyList<IModule> modules)
        {
            Services = services;
            Modules = modules;
        }

        public IServiceProvider Services { get; }

        public IReadOnlyList<IModule> Modules { get; }
    }
}