using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules
{
    public interface IModule
    {
        ModuleManifest Manifest { get; set; }

        void RegisterService(IServiceCollection services);

        IModuleService GetService(IServiceProvider provider);

        public void Setup(IModuleHostBuilder host) { }
    }

    public interface IModule<out TService> : IModule where TService : IModuleService
    {
        new TService GetService(IServiceProvider provider);
    }

    public interface IModule<out TService, out TOption> : IModule<TService> where TService : IModuleService
    {
        void ConfigureOptions(Action<TOption, IServiceProvider> configureOptions);

        TOption GetOption(IServiceProvider provider);
    }
}
