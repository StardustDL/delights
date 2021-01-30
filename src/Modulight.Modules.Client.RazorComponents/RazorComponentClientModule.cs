using Modulight.Modules.Client.RazorComponents.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule"/>.
    /// </summary>
    /// <typeparam name="TUIService"></typeparam>
    /// <typeparam name="TOption"></typeparam>
    /// <typeparam name="TUI"></typeparam>
    public abstract class RazorComponentClientModule<TUIService, TOption, TUI> : Module, IRazorComponentClientModule
    {
        Lazy<ModuleUIAttribute?> ModuleUIAttribute;

        protected RazorComponentClientModule()
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
