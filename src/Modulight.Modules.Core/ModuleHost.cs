using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the contract for module hosts.
    /// </summary>
    public interface IModuleHost
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
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