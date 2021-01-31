using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule"/>.
    /// </summary>
    public abstract class RazorComponentClientModule<TModule> : Module<TModule>, IRazorComponentClientModule
    {
        Lazy<ModuleUIAttribute?> ModuleUIAttribute;

        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected RazorComponentClientModule(IModuleHost host) : base(host)
        {
            ModuleUIAttribute = new Lazy<ModuleUIAttribute?>(() => GetType().GetCustomAttribute<ModuleUIAttribute>());
        }

        /// <summary>
        /// <inheritdoc/>
        /// Use <see cref="Modulight.Modules.Client.RazorComponents.ModuleUIAttribute"/> to detect module UI.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual IModuleUI? GetUI(IServiceProvider provider)
        {
            var uiAttr = ModuleUIAttribute.Value;
            if (uiAttr is null)
                return default;
            uiAttr.UIType.EnsureModuleUI();
            return (IModuleUI)provider.GetRequiredService(uiAttr.UIType);
        }
    }
}
