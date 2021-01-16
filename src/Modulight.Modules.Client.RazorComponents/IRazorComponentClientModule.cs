using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies the contract for razor component modules.
    /// </summary>
    public interface IRazorComponentClientModule : IModule
    {
        /// <summary>
        /// Register module UI.
        /// </summary>
        /// <param name="services"></param>
        void RegisterUI(IServiceCollection services);

        /// <summary>
        /// Register module UI related services.
        /// </summary>
        /// <param name="services"></param>
        void RegisterUIService(IServiceCollection services);

        /// <summary>
        /// Get module UI from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        IModuleUI GetUI(IServiceProvider provider);

        /// <summary>
        /// Get module UI service from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        IModuleService GetUIService(IServiceProvider provider);
    }

    /// <summary>
    /// Specifies the contract for razor component modules with typed services.
    /// </summary>
    public interface IRazorComponentClientModule<out TUIService, out TOption, out TUI> : IModule<TUIService, TOption>, IRazorComponentClientModule where TUI : IModuleUI where TUIService : IModuleService
    {
        /// <summary>
        /// Get typed module UI from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        new TUI GetUI(IServiceProvider provider);

        /// <summary>
        /// Get typed module UI service from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        new TUIService GetUIService(IServiceProvider provider);
    }
}
