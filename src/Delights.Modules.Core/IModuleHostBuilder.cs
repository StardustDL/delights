using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Delights.Modules
{
    public interface IModuleHostBuilder
    {
        IReadOnlyList<IModule> Modules { get; }

        IModuleHostBuilder AddModule(Type type, IModule module);

        IModule? GetModule(Type type);

        void Build(IServiceCollection services);

        public IModuleHostBuilder AddModule<T>(T module) where T : class, IModule => AddModule(typeof(T), module);

        public IModuleHostBuilder AddModule<T>()
            where T : class, IModule, new() => AddModule(new T());

        public IModuleHostBuilder AddModule<T, TOption>(T module, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption> where TOption : class
        {
            if (configureOptions is not null)
            {
                module.ConfigureOptions(configureOptions);
            }
            AddModule(module);
            return this;
        }

        public IModuleHostBuilder AddModule<T, TOption>(Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption>, new() where TOption : class => AddModule(new T(), configureOptions);

        public IModuleHostBuilder TryAddModule(Type type, Func<IModule> module)
        {
            if (GetModule(type) is null)
            {
                AddModule(type, module());
            }
            return this;
        }

        public IModuleHostBuilder TryAddModule<T>(Func<T> module)
            where T : class, IModule => TryAddModule(typeof(T), () => module());

        public IModuleHostBuilder TryAddModule<T>()
            where T : class, IModule, new() => TryAddModule(() => new T());

        public IModuleHostBuilder TryAddModule<T, TOption>(Func<T> module, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption> where TOption : class
        {
            var type = typeof(T);
            if (GetModule(type) is null)
            {
                var mod = module();
                if (configureOptions is not null)
                {
                    mod.ConfigureOptions(configureOptions);
                }
                AddModule(type, mod);
            }
            return this;
        }

        public IModuleHostBuilder TryAddModule<T, TOption>(Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption>, new() where TOption : class => TryAddModule(() => new T(), configureOptions);
    }
}