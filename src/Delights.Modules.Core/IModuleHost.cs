using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules
{
    public interface IModuleHost
    {
        IList<IModule> Modules { get; }

        IServiceCollection Services { get; }

        IModuleHost AddModule<T>(T module)
            where T : class, IModule;

        IEnumerable<T> AllSpecifyModules<T>();

        Task Initialize(IServiceProvider provider);
    }
}