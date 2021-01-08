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
                string assembly = GetType().Assembly.GetName().Name!;
                assemblies = new string[] { assembly };
            }
            Assemblies = assemblies;
        }

        public string Name { get; }

        public virtual string[] Assemblies { get; protected set; }

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

        public override TService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();
    }
}
