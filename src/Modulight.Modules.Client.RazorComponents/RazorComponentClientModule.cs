using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule{TUIService, TOption, TUI}"/>.
    /// </summary>
    /// <typeparam name="TUIService"></typeparam>
    /// <typeparam name="TOption"></typeparam>
    /// <typeparam name="TUI"></typeparam>
    public abstract class RazorComponentClientModule<TUIService, TOption, TUI> : Module<TUIService, TOption>, IRazorComponentClientModule<TUIService, TOption, TUI> where TUI : class, IModuleUI where TUIService : class, IModuleService where TOption : class, new()
    {
        /// <inheritdoc/>
        protected RazorComponentClientModule(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// Auto-register <typeparamref name="TUI"/> as scoped service.
        /// </summary>
        /// <param name="services"></param>
        public virtual void RegisterUI(IServiceCollection services)
        {
            services.AddScoped<TUI>();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Auto-register <typeparamref name="TUIService"/> as scoped service.
        /// </summary>
        /// <param name="services"></param>
        public virtual void RegisterUIService(IServiceCollection services)
        {
            services.AddScoped<TUIService>();
        }

        /// <inheritdoc/>
        public override void RegisterService(IServiceCollection services)
        {
            RegisterUIService(services);
            RegisterOptions(services);
            RegisterUI(services);
        }

        /// <inheritdoc/>
        public TUI GetUI(IServiceProvider provider) => provider.GetRequiredService<TUI>();

        /// <inheritdoc/>
        public TUIService GetUIService(IServiceProvider provider) => base.GetService(provider);

        IModuleUI IRazorComponentClientModule.GetUI(IServiceProvider provider) => GetUI(provider);

        IModuleService IRazorComponentClientModule.GetUIService(IServiceProvider provider) => GetUIService(provider);
    }
}
