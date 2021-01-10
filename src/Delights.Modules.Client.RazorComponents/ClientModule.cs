using Delights.Modules.Client.RazorComponents.UI;
using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Client.RazorComponents
{
    public static class ClientModuleExtensions
    {
        public static IModuleHost AddClientModules(this IModuleHost modules, Action<Core.ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<Core.Module, Core.ModuleService, Core.ModuleOption>(configureOptions);
            return modules;
        }
    }

    public interface IClientModule : IModule
    {
        void RegisterUI(IServiceCollection services);

        void RegisterUIService(IServiceCollection services);

        IModuleUI GetUI(IServiceProvider provider);

        IModuleService GetUIService(IServiceProvider provider);
    }

    public abstract class ClientModule<TUIService, TOption, TUI> : Module<TUIService, TOption>, IClientModule where TUI : class, IModuleUI where TUIService : class, IModuleService where TOption : class
    {
        protected ClientModule(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        public virtual void RegisterUI(IServiceCollection services)
        {
            services.AddScoped<TUI>();
        }

        public virtual void RegisterUIService(IServiceCollection services)
        {
            services.AddScoped<TUIService>();
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterUI(services);
        }

        public TUI GetUI(IServiceProvider provider) => provider.GetRequiredService<TUI>();

        public TUIService GetUIService(IServiceProvider provider) => base.GetService(provider);

        IModuleUI IClientModule.GetUI(IServiceProvider provider) => GetUI(provider);

        IModuleService IClientModule.GetUIService(IServiceProvider provider) => GetUIService(provider);
    }
}
