using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    public interface IModuleCollection<TModule> where TModule : IModule
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
        IEnumerable<TModule> LoadedModules { get; }

        IEnumerable<Type> DefinedModules { get; }
    }

    /// <summary>
    /// Specifies the contract for module hosts.
    /// </summary>
    public interface IModuleHost : IModuleCollection<IModule>
    {
        IServiceProvider Services { get; }

        /// <summary>
        /// Get manifest for the module.
        /// </summary>
        ModuleManifest GetManifest(Type moduleType);

        IModule GetModule(Type moduleType);

        Task Initialize();

        Task Shutdown();

        T GetService<T>(IServiceProvider provider, Type moduleType) where T : notnull;

        T GetOption<T>(IServiceProvider provider, Type moduleType) where T : class;

        ILogger<TModule> GetLogger<TModule>();
    }

    internal class DefaultModuleHost : IModuleHost
    {
        IReadOnlyDictionary<Type, IModule> _LoadedModules { get; set; } = new Dictionary<Type, IModule>();

        IReadOnlyDictionary<Type, ModuleManifest> _DefinedModules { get; set; }

        public DefaultModuleHost(IServiceProvider services, (Type, ModuleManifest)[] definedModules)
        {
            Services = services;

            _DefinedModules = new Dictionary<Type, ModuleManifest>(definedModules.Select(x => new KeyValuePair<Type, ModuleManifest>(x.Item1, x.Item2)));
            DefinedModules = definedModules.Select(x => x.Item1);
        }

        /// <inheritdoc/>
        public virtual IEnumerable<IModule> LoadedModules { get; protected set; } = Array.Empty<IModule>();

        /// <inheritdoc/>
        public virtual IEnumerable<Type> DefinedModules { get; protected set; }

        /// <inheritdoc/>
        public virtual IServiceProvider Services { get; protected set; }

        /// <inheritdoc/>
        public virtual ModuleManifest GetManifest(Type moduleType)
        {
            if (_DefinedModules.TryGetValue(moduleType, out var value))
            {
                return value;
            }
            else
            {
                throw new Exception($"No such defined module: {moduleType.FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual IModule GetModule(Type moduleType)
        {
            if (_LoadedModules.TryGetValue(moduleType, out var value))
            {
                return value;
            }
            else
            {
                throw new Exception($"No such loaded module: {moduleType.FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual T GetService<T>(IServiceProvider provider, Type moduleType) where T : notnull
        {
            var manifest = GetManifest(moduleType);
            var type = typeof(T);
            if (manifest.Services.Any(x => x.Type == type))
            {
                return provider.GetRequiredService<T>();
            }
            else
            {
                throw new Exception($"No such service for the module {moduleType.FullName}: {type.FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual T GetOption<T>(IServiceProvider provider, Type moduleType) where T : class
        {
            var manifest = GetManifest(moduleType);
            var type = typeof(T);
            if (manifest.Options.Any(x => x == type))
            {
                return provider.GetRequiredService<IOptionsSnapshot<T>>().Value;
            }
            else
            {
                throw new Exception($"No such option for the module {moduleType.FullName}: {type.FullName}.");
            }
        }

        /// <inheritdoc/>
        public virtual async Task Initialize()
        {
            var modules = new List<(Type, IModule)>();
            foreach (var type in DefinedModules)
            {
                modules.Add((type, (IModule)Services.GetRequiredService(type)));
            }
            _LoadedModules = new Dictionary<Type, IModule>(modules.Select(x => new KeyValuePair<Type, IModule>(x.Item1, x.Item2)));
            LoadedModules = modules.Select(x => x.Item2);

            foreach (var module in LoadedModules)
            {
                await module.Initialize();
            }
        }

        /// <inheritdoc/>
        public virtual async Task Shutdown()
        {
            foreach (var module in LoadedModules)
            {
                await module.Shutdown();
            }
        }

        /// <inheritdoc/>
        public virtual ILogger<TModule> GetLogger<TModule>()
        {
            var type = typeof(TModule);
            if (_DefinedModules.ContainsKey(type))
            {
                return Services.GetRequiredService<ILogger<TModule>>();
            }
            else
            {
                throw new Exception($"No such module: {type.FullName}.");
            }
        }
    }

    public class ModuleHostFilter<TModule> : IModuleCollection<TModule> where TModule : IModule
    {
        public ModuleHostFilter(IModuleHost host)
        {
            Host = host;
        }

        public IModuleHost Host { get; }

        /// <inheritdoc/>
        public IEnumerable<TModule> LoadedModules => Host.LoadedModules.Where(x => x is TModule).Select(x => (TModule)x);

        /// <inheritdoc/>
        public IEnumerable<Type> DefinedModules => Host.DefinedModules.Where(x => x.IsModule<TModule>());
    }
}