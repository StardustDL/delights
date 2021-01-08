using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;

namespace Delights.Modules
{
    public abstract class Module
    {
        protected Module(string name, string[]? assemblies = null)
        {
            Name = name;
            if (assemblies is null)
            {
                string? assembly = GetType().Assembly.GetName().Name + ".dll";
                assemblies = assembly is not null ? new string[] { assembly } : Array.Empty<string>();
            }
            Assemblies = assemblies;
        }

        public string Name { get; }

        public virtual string[] Assemblies { get; }

        public virtual void RegisterService(IServiceCollection services)
        {
        }

        public abstract ModuleService GetService(IServiceProvider provider);
    }

    public abstract class Module<TService> : Module where TService : ModuleService
    {
        protected Module(string name, string[]? assemblies = null) : base(name, assemblies) { }

        public override void RegisterService(IServiceCollection services)
        {
            services.AddScoped<TService>();
        }

        public override ModuleService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();
    }
}
