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
        ModuleManifest Metadata { get; set; }

        void RegisterOptions(IServiceCollection services);

        void RegisterService(IServiceCollection services);

        Task Initialize(IServiceProvider provider);

        IModuleService GetService(IServiceProvider provider);

        ModuleOption GetOption(IServiceProvider provider);
    }

    public abstract class Module<TService, TOption> : IModule where TService : class, IModuleService where TOption : ModuleOption
    {
        protected Module(ModuleManifest? metadata = null)
        {
            if (metadata is null)
            {
                metadata = new ModuleManifest
                {
                    Name = GetType().Name,
                    EntryAssembly = GetType().GetAssemblyName(),
                    DisplayName = GetType().Name,
                    Version = GetType().Assembly.GetName().Version?.ToString() ?? "",
                };
            }
            Metadata = metadata;
        }

        public ModuleManifest Metadata { get; set; }

        public virtual void RegisterOptions(IServiceCollection services)
        {
            services.AddOptions<TOption>();
        }

        public virtual void RegisterService(IServiceCollection services)
        {
            services.AddScoped<TService>();
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
