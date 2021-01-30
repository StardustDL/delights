using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modulight.Modules.Hosting;
using System.Reflection;

namespace Modulight.Modules.Client.RazorComponents
{
    public sealed class RazorComponentClientModulePlugin : ModuleHostBuilderPlugin
    {
        /// <inheritdoc/>
        public override void AfterBuild(IReadOnlyDictionary<Type, ModuleManifest> modules, IServiceCollection services)
        {
            services.AddSingleton<IRazorComponentClientModuleHost>(sp => new RazorComponentClientModuleHost(sp, modules));
            base.AfterBuild(modules, services);
        }

        /// <inheritdoc/>
        public override void AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services)
        {
            if (module.IsModule<IRazorComponentClientModule>())
            {
                ModuleUIAttribute? uiAttr = module.GetCustomAttribute<ModuleUIAttribute>();
                if (uiAttr is not null)
                {
                    uiAttr.UIType.EnsureModuleUI();
                    services.AddScoped(uiAttr.UIType);
                }
            }
            base.AfterModule(module, manifest, startup, services);
        }
    }
}
