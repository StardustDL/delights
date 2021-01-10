using Delights.Modules.Options;
using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

        void RegisterOptions(IServiceCollection services);

        void RegisterService(IServiceCollection services);

        void Setup(IModuleCollection modules, IServiceCollection services);

        Task Initialize(IServiceProvider provider);

        IModuleService GetService(IServiceProvider provider);

        ModuleOption GetOption(IServiceProvider provider);
    }

    public abstract class Module<TService, TOption> : IModule where TService : class, IModuleService where TOption : ModuleOption
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

        public virtual void RegisterOptions(IServiceCollection services)
        {
            services.AddOptions<TOption>();
        }

        public virtual void RegisterService(IServiceCollection services)
        {
            services.AddScoped<TService>();
        }

        public virtual void Setup(IModuleCollection modules, IServiceCollection services)
        {

        }

        public virtual Task Initialize(IServiceProvider provider)
        {
            return Task.CompletedTask;
        }

        public virtual TService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();

        public virtual TOption GetOption(IServiceProvider provider) => provider.GetRequiredService<TOption>();

        IModuleService IModule.GetService(IServiceProvider provider) => GetService(provider);
        ModuleOption IModule.GetOption(IServiceProvider provider) => GetOption(provider);
    }
}
