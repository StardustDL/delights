using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Modulight.Modules
{
    /// <summary>
    /// Basic implement for <see cref="IModule{TService}"/>.
    /// </summary>
    /// <typeparam name="TService">Service type.</typeparam>
    public abstract class Module<TService> : IModule<TService> where TService : class, IModuleService
    {
        /// <summary>
        /// Initial module with manifest.
        /// </summary>
        /// <param name="manifest">Module manifest. If null, will use defualt manifest generation.</param>
        protected Module(ModuleManifest? manifest = null)
        {
            if (manifest is null)
            {
                manifest = ModuleManifest.Generate(GetType());
            }
            Manifest = manifest;
        }

        /// <inheritdoc/>
        public ModuleManifest Manifest { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// It will register <typeparamref name="TService"/> as scoped service as default.
        /// </summary>
        public virtual void RegisterService(IServiceCollection services)
        {
            services.AddScoped<TService>();
        }

        /// <inheritdoc/>
        public virtual void Setup(IModuleHostBuilder host) { }

        /// <inheritdoc/>
        public virtual TService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();

        IModuleService IModule.GetService(IServiceProvider provider) => GetService(provider);
    }

    /// <summary>
    /// Basic implement for <see cref="IModule{TService, TOption}"/>.
    /// </summary>
    /// <typeparam name="TService">Service type.</typeparam>
    /// <typeparam name="TOption">Option type.</typeparam>
    public abstract class Module<TService, TOption> : Module<TService>, IModule<TService, TOption> where TService : class, IModuleService where TOption : class
    {
        /// <summary>
        /// Action list for <see cref="ConfigureOptions(Action{TOption, IServiceProvider})"/>.
        /// </summary>
        protected IList<Action<TOption, IServiceProvider>> OptionConfigurations { get; } = new List<Action<TOption, IServiceProvider>>();

        /// <summary>
        /// Action for <see cref="SetupOptions(Action{TOption})"/>.
        /// </summary>
        protected Action<TOption>? OptionsSetup { get; set; }

        /// <summary>
        /// Get the options after applying <see cref="OptionsSetup"/>.
        /// </summary>
        /// <returns></returns>
        protected TOption GetSetupOptions(TOption initial)
        {
            if (OptionsSetup is not null)
                OptionsSetup(initial);
            return initial;
        }

        /// <inheritdoc/>
        protected Module(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        /// <inheritdoc/>
        public virtual void RegisterOptions(IServiceCollection services)
        {
            var builder = services.AddOptions<TOption>();
            if (OptionsSetup is not null)
                builder.Configure(OptionsSetup);
            foreach (var item in OptionConfigurations)
                builder.Configure(item);
        }

        /// <inheritdoc/>
        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterOptions(services);
        }

        /// <inheritdoc/>
        public virtual void SetupOptions(Action<TOption> setupOptions) => OptionsSetup = setupOptions;

        /// <inheritdoc/>
        public virtual void ConfigureOptions(Action<TOption, IServiceProvider> configureOptions) => OptionConfigurations.Add(configureOptions);

        /// <inheritdoc/>
        public virtual TOption GetOption(IServiceProvider provider) => provider.GetRequiredService<IOptions<TOption>>().Value;
    }
}
