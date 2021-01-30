using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Default methods for module host builders.
    /// </summary>
    public static class ModuleHostBuilder
    {
        /// <summary>
        /// Create a defualt module host builder.
        /// </summary>
        /// <returns></returns>
        public static IModuleHostBuilder CreateDefaultBuilder()
        {
            return new DefaultModuleHostBuilder();
        }
    }

    /// <summary>
    /// Default implement for <see cref="IModuleHostBuilder"/>.
    /// </summary>
    public class DefaultModuleHostBuilder : IModuleHostBuilder
    {
        /// <summary>
        /// Module descriptors for registered modules.
        /// </summary>
        protected List<Type> ModuleDescriptors { get; } = new List<Type>();

        /// <summary>
        /// Plugin descriptors for registered modules.
        /// </summary>
        protected List<Type> PluginDescriptors { get; } = new List<Type>();

        protected List<Action<IServiceCollection>> BuilderServiceConfiguration { get; } = new List<Action<IServiceCollection>>();

        /// <inheritdoc />
        public virtual IReadOnlyList<Type> Modules => ModuleDescriptors.AsReadOnly();

        /// <inheritdoc />
        public virtual IReadOnlyList<Type> Plugins => PluginDescriptors.AsReadOnly();

        /// <inheritdoc />
        public virtual IModuleHostBuilder AddModule(Type type)
        {
            type.EnsureModule();
            if (!ModuleDescriptors.Contains(type))
            {
                ModuleDescriptors.Add(type);
            }
            return this;
        }

        /// <inheritdoc />
        public virtual IModuleHostBuilder UsePlugin(Type type)
        {
            type.EnsureHostBuilderPlugin();
            if (!PluginDescriptors.Contains(type))
            {
                PluginDescriptors.Add(type);
            }
            return this;
        }

        protected virtual async Task BeforeBuild(IServiceCollection services, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                await plugin.BeforeBuild(this, services).ConfigureAwait(false);
            }
        }

        protected virtual async Task AfterBuild(IServiceCollection services, IReadOnlyDictionary<Type, ModuleManifest> modules, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                await plugin.AfterBuild(modules, services).ConfigureAwait(false);
            }
        }

        protected virtual async Task BeforeModule(IServiceCollection services, Type module, ModuleManifest manifest, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                await plugin.BeforeModule(module, manifest, services).ConfigureAwait(false);
            }
        }

        protected virtual async Task AfterModule(IServiceCollection services, Type module, ModuleManifest manifest, IModuleStartup? startup, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                await plugin.AfterModule(module, manifest, startup, services).ConfigureAwait(false);
            }
        }

        List<(Type Module, ModuleManifest Manifest, Type? Startup)> ResolveModuleDependency()
        {
            var result = new List<(Type Module, ModuleManifest Manifest, Type? Startup)>();
            Dictionary<Type, ModuleManifest> modules = new Dictionary<Type, ModuleManifest>();
            Dictionary<Type, int> inDegrees = new Dictionary<Type, int>();

            Queue<Type> queue = new Queue<Type>();
            ModuleDescriptors.ForEach(t =>
            {
                modules.Add(t, ModuleManifest.Generate(t));
                inDegrees.Add(t, 0);
                queue.Enqueue(t);
            });
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var manifest = modules[cur];
                foreach (var dep in manifest.Dependencies)
                {
                    if (!modules.ContainsKey(dep))
                    {
                        modules.Add(dep, ModuleManifest.Generate(dep));
                        inDegrees.Add(dep, 0);
                        queue.Enqueue(dep);
                    }
                    inDegrees[dep] += 1;
                }
            }

            foreach (var item in inDegrees.Where(x => x.Value is 0))
                queue.Enqueue(item.Key);

            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var manifest = modules[cur];

                var startupAttr = cur.GetCustomAttribute<ModuleStartupAttribute>(true);
                Type? startup = null;
                if (startupAttr is not null)
                {
                    startupAttr.StartupType.EnsureModuleStartup();
                    startup = startupAttr.StartupType;
                }
                result.Add((cur, manifest, startup));

                foreach (var dep in manifest.Dependencies)
                {
                    Debug.Assert(inDegrees[dep] >= 1);

                    inDegrees[dep] -= 1;
                    if (inDegrees[dep] is 0)
                    {
                        queue.Enqueue(dep);
                    }
                }
            }
            result.Reverse();
            return result;
        }

        /// <inheritdoc />
        public async Task Build(IServiceCollection services, IServiceCollection? builderServices = null)
        {
            var modules = ResolveModuleDependency();
            var plugins = new List<IModuleHostBuilderPlugin>();

            Dictionary<Type, Type> moduleStartups = new Dictionary<Type, Type>();

            builderServices ??= new ServiceCollection();
            builderServices.AddLogging().AddOptions();
            foreach(var configure in BuilderServiceConfiguration)
            {
                configure(builderServices);
            }

            PluginDescriptors.ForEach(plugin => builderServices.AddSingleton(plugin));
            modules.ForEach(item =>
            {
                if (item.Startup is not null)
                    builderServices.AddSingleton(item.Startup);
            });

            using var builderService = builderServices.BuildServiceProvider();

            var logger = builderService.GetRequiredService<ILogger<DefaultModuleHostBuilder>>();

            PluginDescriptors.ForEach(type =>
            {
                plugins.Add((IModuleHostBuilderPlugin)builderService.GetRequiredService(type));
                logger.LogInformation($"Loaded plugin {type.FullName}.");
            });

            services.AddLogging().AddOptions();

            await BeforeBuild(services, plugins).ConfigureAwait(false);

            foreach (var (type, manifest, startupType) in modules)
            {
                logger.LogInformation($"Processing module {type.FullName}.");

                await BeforeModule(services, type, manifest, plugins);

                services.AddSingleton(type);
                foreach (var service in manifest.Services)
                {
                    services.Add(new ServiceDescriptor(service.Type, service.Type, service.Lifetime));
                }
                IModuleStartup? startup = null;
                if (startupType is not null)
                {
                    startup = (IModuleStartup)builderService.GetRequiredService(startupType);
                    await startup.ConfigureServices(services).ConfigureAwait(false);
                }

                await AfterModule(services, type, manifest, startup, plugins);

                logger.LogInformation($"Processed module {type.FullName}.");
            }

            var moduleDictionary = new Dictionary<Type, ModuleManifest>(
                modules.Select(x => new KeyValuePair<Type, ModuleManifest>(x.Item1, x.Item2)));

            services.AddSingleton<IModuleHost>(sp => new DefaultModuleHost(sp, moduleDictionary));

            await AfterBuild(services, moduleDictionary, plugins).ConfigureAwait(false);
        }

        public IModuleHostBuilder ConfigureBuilderServices(Action<IServiceCollection> configureBuilderServices)
        {
            /// <inheritdoc />
            BuilderServiceConfiguration.Add(configureBuilderServices);
            return this;
        }
    }
}