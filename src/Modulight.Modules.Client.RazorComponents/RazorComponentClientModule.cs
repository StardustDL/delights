using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace Modulight.Modules.Client.RazorComponents
{
    public static class RazorComponentClientModuleExtensions
    {
        public static IModuleHostBuilder UseRazorComponentClientModules(this IModuleHostBuilder modules)
        {
            return modules.UsePostMiddleware((modules, services) =>
            {
                services.TryAddScoped<LazyAssemblyLoader>();
                services.AddSingleton<IRazorComponentClientModuleHost>(sp => new RazorComponentClientModuleHost(sp,
                    modules.Modules.AllSpecifyModules<IRazorComponentClientModule>().ToArray()));
            });
        }

        public static IRazorComponentClientModuleHost GetRazorComponentClientModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IRazorComponentClientModuleHost>();
    }

    public interface IRazorComponentClientModule : IModule
    {
        void RegisterUI(IServiceCollection services);

        void RegisterUIService(IServiceCollection services);

        IModuleUI GetUI(IServiceProvider provider);

        IModuleService GetUIService(IServiceProvider provider);
    }

    public interface IRazorComponentClientModule<out TUIService, out TOption, out TUI> : IModule<TUIService, TOption>, IRazorComponentClientModule where TUI : IModuleUI where TUIService : IModuleService
    {
        new TUI GetUI(IServiceProvider provider);

        new TUIService GetUIService(IServiceProvider provider);
    }

    public abstract class RazorComponentClientModule<TUIService, TOption, TUI> : Module<TUIService, TOption>, IRazorComponentClientModule<TUIService, TOption, TUI> where TUI : class, IModuleUI where TUIService : class, IModuleService where TOption : class
    {
        protected RazorComponentClientModule(ModuleManifest? manifest = null) : base(manifest)
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
            RegisterUIService(services);
            RegisterOptions(services);
            RegisterUI(services);
        }

        public TUI GetUI(IServiceProvider provider) => provider.GetRequiredService<TUI>();

        public TUIService GetUIService(IServiceProvider provider) => base.GetService(provider);

        IModuleUI IRazorComponentClientModule.GetUI(IServiceProvider provider) => GetUI(provider);

        IModuleService IRazorComponentClientModule.GetUIService(IServiceProvider provider) => GetUIService(provider);
    }
}
