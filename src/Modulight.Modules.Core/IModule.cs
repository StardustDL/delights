using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the contract for modules.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Manifest for module.
        /// </summary>
        ModuleManifest Manifest { get; set; }

        /// <summary>
        /// Register services for module.
        /// </summary>
        /// <param name="services">Service collection</param>
        void RegisterService(IServiceCollection services);

        /// <summary>
        /// Get module service from service provider.
        /// </summary>
        /// <param name="provider">Service provider.</param>
        /// <returns>Module service.</returns>
        IModuleService GetService(IServiceProvider provider);

        /// <summary>
        /// Setup module when it be added to module host.
        /// Install other dependent module in this method.
        /// </summary>
        /// <param name="host">Module host.</param>
        void Setup(IModuleHostBuilder host);
    }

    /// <summary>
    /// Specifies the contract for the modules with typed service.
    /// </summary>
    /// <typeparam name="TService">Service type.</typeparam>
    public interface IModule<out TService> : IModule where TService : IModuleService
    {
        /// <summary>
        /// Get module typed service from service provider.
        /// </summary>
        /// <param name="provider">Service provider.</param>
        /// <returns>Typed module service.</returns>
        new TService GetService(IServiceProvider provider);
    }

    /// <summary>
    /// Specifies the contract for the modules with typed service and option.
    /// </summary>
    /// <typeparam name="TService">Service type.</typeparam>
    /// <typeparam name="TOption">Option type.</typeparam>
    public interface IModule<out TService, out TOption> : IModule<TService> where TService : IModuleService
    {
        /// <summary>
        /// Register options for module.
        /// </summary>
        /// <param name="services">Service collection</param>
        void RegisterOptions(IServiceCollection services);

        /// <summary>
        /// Setup for initial options.
        /// Action from this can be used for service registering and other methods in module.
        /// </summary>
        /// <param name="setupOptions">Action to setup initial options. This will replace the old setup action.</param>
        void SetupOptions(Action<TOption> setupOptions);

        /// <summary>
        /// Configure for options.
        /// Actions from this can be used for the module service after building the service provider.
        /// </summary>
        /// <param name="configureOptions">Action to configure options. This will be executed after the existed action.</param>
        void ConfigureOptions(Action<TOption, IServiceProvider> configureOptions);

        /// <summary>
        /// Get module typed option from service provider.
        /// </summary>
        /// <param name="provider">Service provider.</param>
        /// <returns>Typed module option.</returns>
        TOption GetOption(IServiceProvider provider);
    }
}
