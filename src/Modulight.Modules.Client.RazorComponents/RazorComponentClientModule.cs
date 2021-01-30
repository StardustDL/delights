using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Modulight.Modules.Hosting;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule"/>.
    /// </summary>
    public abstract class RazorComponentClientModule<TModule> : Module<TModule>, IRazorComponentClientModule
    {
        Lazy<ModuleUIAttribute?> ModuleUIAttribute;

        protected RazorComponentClientModule(IModuleHost host) : base(host)
        {
            ModuleUIAttribute = new Lazy<ModuleUIAttribute?>(() => GetType().GetCustomAttribute<ModuleUIAttribute>());
        }

        /// <inheritdoc/>
        public IModuleUI? GetUI(IServiceProvider provider)
        {
            var uiAttr = ModuleUIAttribute.Value;
            if (uiAttr is null)
                return default;
            uiAttr.UIType.EnsureModuleUI();
            return (IModuleUI)provider.GetRequiredService(uiAttr.UIType);
        }
    }
}
