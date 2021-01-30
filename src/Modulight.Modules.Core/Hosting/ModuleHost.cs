using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
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

        /// <summary>
        /// Get manifest for the module.
        /// </summary>
        ModuleManifest GetManifest(IModule module);

        Task Initialize();

        Task Shutdown();

        T GetService<T>(IServiceProvider provider, IModule module) where T : notnull;

        T GetOption<T>(IServiceProvider provider, IModule module) where T : class;
    }

    public class DefaultModuleHost : IModuleHost
    {
        protected IReadOnlyDictionary<IModule, ModuleManifest> LoadedModules { get; set; }

        protected IServiceProvider Services { get; }

        public DefaultModuleHost(IServiceProvider services, IReadOnlyDictionary<Type, ModuleManifest> moduleTypes)
        {
            Services = services;
            var modules = new Dictionary<IModule, ModuleManifest>();
            foreach (var (type, manifest) in moduleTypes)
            {
                modules.Add((IModule)Services.GetRequiredService(type), manifest);
            }
            LoadedModules = modules;
            Modules = modules.Keys.ToArray();
        }

        /// <inheritdoc/>
        public virtual IReadOnlyList<IModule> Modules { get; protected set; }

        /// <inheritdoc/>
        public virtual ModuleManifest GetManifest(IModule module)
        {
            if (LoadedModules.TryGetValue(module, out var value))
            {
                return value;
            }
            else
            {
                throw new Exception($"No such module: {module.GetType().FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual T GetService<T>(IServiceProvider provider, IModule module) where T : notnull
        {
            var manifest = GetManifest(module);
            var type = typeof(T);
            if (manifest.Services.Any(x => x.Type == type))
            {
                return provider.GetRequiredService<T>();
            }
            else
            {
                throw new Exception($"No such service for the module {module.GetType().FullName}: {type.FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual T GetOption<T>(IServiceProvider provider, IModule module) where T : class
        {
            var manifest = GetManifest(module);
            var type = typeof(T);
            if (manifest.Options.Any(x => x == type))
            {
                return provider.GetRequiredService<IOptionsSnapshot<T>>().Value;
            }
            else
            {
                throw new Exception($"No such option for the module {module.GetType().FullName}: {type.FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual async Task Initialize()
        {
            foreach(var module in Modules)
            {
                await module.Initialize();
            }
        }

        /// <inheritdoc/>
        public virtual async Task Shutdown()
        {
            foreach (var module in Modules)
            {
                await module.Shutdown();
            }
        }
    }
}