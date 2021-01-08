using Delights.Modules.Services;
using Delights.Modules.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Client
{
    public static class ClientModuleExtensions
    {
        public static ModuleCollection AddClientModules(this ModuleCollection modules)
        {
            modules.AddModule<CoreClientModule>();
            return modules;
        }
    }

    public abstract class ClientModule : Module
    {
        protected ClientModule(string name, string[]? assemblies = null) : base(name, assemblies)
        {
        }

        public virtual void RegisterUI(IServiceCollection services)
        {
        }

        public virtual void RegisterUIService(IServiceCollection services)
        {
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterUIService(services);
            RegisterUI(services);
        }

        public abstract ModuleUI GetUI(IServiceProvider provider);
    }

    public abstract class ClientModule<TUIService, TUI> : ClientModule where TUI : ModuleUI where TUIService : ModuleService
    {
        protected ClientModule(string name, string[]? assemblies = null) : base(name, assemblies) { }

        public override void RegisterUI(IServiceCollection services)
        {
            services.AddScoped<TUI>();
        }

        public override void RegisterUIService(IServiceCollection services)
        {
            services.AddScoped<TUIService>();
        }

        public override ModuleUI GetUI(IServiceProvider provider) => provider.GetRequiredService<TUI>();

        public override ModuleService GetService(IServiceProvider provider) => provider.GetRequiredService<TUIService>();
    }
}
