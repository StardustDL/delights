using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Modulight.Modules
{
    public abstract class Module<TService> : IModule<TService> where TService : class, IModuleService
    {
        protected Module(ModuleManifest? manifest = null)
        {
            if (manifest is null)
            {
                manifest = new ModuleManifest
                {
                    Name = GetType().Name,
                    EntryAssembly = GetType().GetAssemblyName(),
                    DisplayName = GetType().Name,
                    Version = GetType().Assembly.GetName().Version?.ToString() ?? "",
                };
            }
            Manifest = manifest;
        }

        public ModuleManifest Manifest { get; set; }

        public virtual void RegisterService(IServiceCollection services)
        {
            services.AddSingleton<TService>();
        }

        public virtual void Setup(IModuleHostBuilder host) { }

        public virtual TService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();

        IModuleService IModule.GetService(IServiceProvider provider) => GetService(provider);
    }

    public abstract class Module<TService, TOption> : Module<TService>, IModule<TService, TOption> where TService : class, IModuleService where TOption : class
    {
        protected IList<Action<TOption, IServiceProvider>> OptionConfigurations { get; } = new List<Action<TOption, IServiceProvider>>();

        protected Module(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        public virtual void RegisterOptions(IServiceCollection services)
        {
            var builder = services.AddOptions<TOption>();
            foreach (var item in OptionConfigurations)
                builder.Configure(item);
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterOptions(services);
        }

        public virtual void ConfigureOptions(Action<TOption, IServiceProvider> configureOptions) => OptionConfigurations.Add(configureOptions);

        public virtual TOption GetOption(IServiceProvider provider) => provider.GetRequiredService<IOptions<TOption>>().Value;
    }
}
