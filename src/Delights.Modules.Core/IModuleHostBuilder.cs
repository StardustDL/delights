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
    }
}