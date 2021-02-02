using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
    class DefaultModuleHostBuilder : IModuleHostBuilder
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

        protected List<Action<IServiceCollection>> ServiceConfiguration { get; } = new List<Action<IServiceCollection>>();

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

        protected virtual void BeforeBuild(IServiceCollection services, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.BeforeBuild(this, services);
            }
        }

        protected virtual void AfterBuild(IServiceCollection services, (Type, ModuleManifest)[] modules, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.AfterBuild(modules, services);
            }
        }

        protected virtual void BeforeModule(IServiceCollection services, Type module, ModuleManifest manifest, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.BeforeModule(module, manifest, services);
            }
        }

        protected virtual void AfterModule(IServiceCollection services, Type module, ModuleManifest manifest, IModuleStartup? startup, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.AfterModule(module, manifest, startup, services);
            }
        }

        List<(Type Module, ModuleManifest Manifest, Type? Startup)> ResolveModuleDependency()
        {
            var result = new List<(Type Module, ModuleManifest Manifest, Type? Startup)>();
            Dictionary<Type, ModuleManifest> modules = new Dictionary<Type, ModuleManifest>();
            Dictionary<Type, int> inDegrees = new Dictionary<Type, int>();

            Queue<Type> queue = new Queue<Type>();
            foreach (var t in ModuleDescriptors.Reverse<Type>())
            {
                modules.Add(t, ModuleManifest.Generate(t));
                inDegrees.Add(t, 0);
                queue.Enqueue(t);
            }
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
        public void Build(IServiceCollection services, IServiceCollection? builderServices = null)
        {
            var modules = ResolveModuleDependency();
            var plugins = new List<IModuleHostBuilderPlugin>();

            Dictionary<Type, Type> moduleStartups = new Dictionary<Type, Type>();

            builderServices ??= new ServiceCollection();
            builderServices.AddLogging().AddOptions();
            foreach (var configure in BuilderServiceConfiguration)
            {
                configure(builderServices);
            }

            PluginDescriptors.ForEach(plugin => builderServices.AddSingleton(plugin));
            modules.ForEach(item =>
            {
                if (item.Startup is not null)
                    builderServices.AddScoped(item.Startup);
            });

            using var builderService = builderServices.BuildServiceProvider();

            var logger = builderService.GetRequiredService<ILogger<DefaultModuleHostBuilder>>();

            PluginDescriptors.ForEach(type =>
            {
                plugins.Add((IModuleHostBuilderPlugin)builderService.GetRequiredService(type));
                logger.LogInformation($"Loaded plugin {type.FullName}.");
            });

            services.AddLogging().AddOptions();
            foreach (var configure in ServiceConfiguration)
            {
                configure(services);
            }

            BeforeBuild(services, plugins);

            foreach (var (type, manifest, startupType) in modules)
            {
                logger.LogInformation($"Processing module {type.FullName}.");

                BeforeModule(services, type, manifest, plugins);

                services.AddSingleton(type);
                foreach (var service in manifest.Services)
                {
                    var des = new ServiceDescriptor(service.ServiceType, service.ImplementationType, service.Lifetime);
                    switch (service.RegisterBehavior)
                    {
                        case ServiceRegisterBehavior.Normal:
                            services.Add(des);
                            break;
                        case ServiceRegisterBehavior.Optional:
                            services.TryAdd(des);
                            break;
                    }
                }
                IModuleStartup? startup = null;
                if (startupType is not null)
                {
                    startup = (IModuleStartup)builderService.GetRequiredService(startupType);
                    startup.ConfigureServices(services);
                }

                AfterModule(services, type, manifest, startup, plugins);

                logger.LogInformation($"Processed module {type.FullName}.");
            }

            var definedModules = modules.Select(x => (x.Item1, x.Item2)).ToArray();

            services.AddSingleton<IModuleHost>(sp => new DefaultModuleHost(sp, definedModules));

            AfterBuild(services, definedModules, plugins);
        }

        public IModuleHostBuilder ConfigureBuilderServices(Action<IServiceCollection> configureBuilderServices)
        {
            BuilderServiceConfiguration.Add(configureBuilderServices);
            return this;
        }

        public IModuleHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            ServiceConfiguration.Add(configureServices);
            return this;
        }
    }
}