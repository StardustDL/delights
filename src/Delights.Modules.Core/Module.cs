using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;

namespace Delights.Modules
{
    public record ModuleMetadata
    {
        public string Name { get; init; } = "";

        public string EntryAssembly { get; init; } = "";

        public string[] Assemblies { get; init; } = Array.Empty<string>();

        public string DisplayName { get; init; } = "";

        public string Description { get; init; } = "";
    }

    public abstract class Module
    {
        protected Module(ModuleMetadata? metadata = null)
        {
            if (metadata is null)
            {
                metadata = new ModuleMetadata
                {
                    Name = GetType().Name,
                    EntryAssembly = GetType().GetAssemblyName(),
                    DisplayName = GetType().Name,
                };
            }
            Metadata = metadata;
        }

        public ModuleMetadata Metadata { get; protected set; }

        public virtual void RegisterService(IServiceCollection services)
        {
        }

        public abstract ModuleService GetService(IServiceProvider provider);
    }

    public abstract class Module<TService> : Module where TService : ModuleService
    {
        public override void RegisterService(IServiceCollection services)
        {
            services.AddScoped<TService>();
        }

        public override TService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();
    }
}
