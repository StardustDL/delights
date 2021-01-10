using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Delights.Modules
{
    public record ModuleManifest
    {
        public string Name { get; init; } = "";

        public string EntryAssembly { get; init; } = "";

        public string[] Assemblies { get; init; } = Array.Empty<string>();

        public string DisplayName { get; init; } = "";

        public string Description { get; init; } = "";

        public string Version { get; init; } = "";

        public string Author { get; init; } = "";

        public string Url { get; init; } = "";
    }

    public interface IModule
    {
        ModuleManifest Manifest { get; set; }

        void RegisterService(IServiceCollection services);

        void Setup(IModuleHost modules, IServiceCollection services);

        Task Initialize(IServiceProvider provider);

        IModuleService GetService(IServiceProvider provider);
    }

    public abstract class Module<TService> : IModule where TService : class, IModuleService
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
            services.AddScoped<TService>();
        }

        public virtual void Setup(IModuleHost modules, IServiceCollection services)
        {

        }

        public virtual Task Initialize(IServiceProvider provider)
        {
            return Task.CompletedTask;
        }

        public virtual TService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();

        IModuleService IModule.GetService(IServiceProvider provider) => GetService(provider);
    }

    public abstract class Module<TService, TOption> : Module<TService> where TService : class, IModuleService where TOption : class
    {
        protected Module(ModuleManifest? manifest = null) : base(manifest)
        {

        }

        protected OptionsBuilder<TOption>? OptionsBuilder { get; set; }

        public virtual void RegisterOptions(IServiceCollection services)
        {
            OptionsBuilder = services.AddOptions<TOption>();
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterOptions(services);
        }

        public virtual void ConfigureOptions(Action<TOption, IServiceProvider> configureOptions)
        {
            if(OptionsBuilder is null)
            {
                throw new NullReferenceException("Option configuring must be after registering.");
            }

            OptionsBuilder.Configure(configureOptions);
        }

        public virtual TOption GetOption(IServiceProvider provider) => provider.GetRequiredService<IOptions<TOption>>().Value;
    }
}
