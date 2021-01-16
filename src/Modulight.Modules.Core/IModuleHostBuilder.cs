using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Modulight.Modules
{
    /// <summary>
    /// Executing order for module host builder middlewares.
    /// </summary>
    public enum ModuleHostBuilderMiddlewareType
    {
        /// <summary>
        /// Execute before building.
        /// </summary>
        Pre,
        /// <summary>
        /// Execute after building.
        /// </summary>
        Post
    }

    /// <summary>
    /// Specifies the contract for module host builder.
    /// </summary>
    public interface IModuleHostBuilder
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
        IReadOnlyList<IModule> Modules { get; }

        /// <summary>
        /// Add module in given type.
        /// </summary>
        /// <param name="type">Target module type.</param>
        /// <param name="module">Module instance</param>
        /// <returns></returns>
        IModuleHostBuilder AddModule(Type type, IModule module);

        /// <summary>
        /// Get module in given type.
        /// </summary>
        /// <param name="type">Target module type.</param>
        /// <returns>Module instance, null when no module in the type.</returns>
        IModule? GetModule(Type type);

        /// <summary>
        /// Use a building middleware.
        /// </summary>
        /// <param name="type">Executing order type.</param>
        /// <param name="middleware">Middleware.</param>
        /// <returns></returns>
        IModuleHostBuilder UseMiddleware(ModuleHostBuilderMiddlewareType type, Action<IModuleHostBuilder, IServiceCollection> middleware);

        /// <summary>
        /// Build the modules and the module host in service collection.
        /// </summary>
        /// <param name="services">Target service collection.</param>
        void Build(IServiceCollection services);
    }
}