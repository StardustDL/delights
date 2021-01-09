using Delights.Modules.Client.UI;
using Delights.Modules.Services;
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
            modules.AddModule<Core.Module>();
            return modules;
        }

        public static IEnumerable<ClientModule> AllClientModules(this ModuleCollection modules)
        {
            return modules.Modules.Where(m => m is ClientModule).Select(m => (m as ClientModule)!);
        }
    }

    public abstract class ClientModule : Modules.Module
    {
        protected ClientModule(ModuleMetadata? metadata = null) : base(metadata)
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
        protected ClientModule(ModuleMetadata? metadata = null) : base(metadata)
        {
        }

        public override void RegisterUI(IServiceCollection services)
        {
            services.AddScoped<TUI>();
        }

        public override void RegisterUIService(IServiceCollection services)
        {
            services.AddScoped<TUIService>();
        }

        public override TUI GetUI(IServiceProvider provider) => provider.GetRequiredService<TUI>();

        public override TUIService GetService(IServiceProvider provider) => provider.GetRequiredService<TUIService>();
    }
}
