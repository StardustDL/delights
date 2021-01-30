using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{

    /// <summary>
    /// Specifies the contract for module host builder.
    /// </summary>
    public interface IModuleHostBuilder
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
        IReadOnlyList<Type> Modules { get; }

        IReadOnlyList<Type> Plugins { get; }

        /// <summary>
        /// Add module in given type.
        /// </summary>
        /// <param name="type">Target module type.</param>
        /// <returns></returns>
        IModuleHostBuilder AddModule(Type type);

        IModuleHostBuilder UsePlugin(Type type);

        /// <summary>
        /// Build the modules and the module host in service collection.
        /// </summary>
        /// <param name="services">Target service collection.</param>
        /// <param name="builderServices">Service collection for builder.</param>
        Task Build(IServiceCollection services, IServiceCollection? builderServices = null);
    }
}