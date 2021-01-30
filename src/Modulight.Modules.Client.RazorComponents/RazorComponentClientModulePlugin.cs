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
        public override Task AfterBuild(IReadOnlyDictionary<Type, ModuleManifest> modules, IServiceCollection services)
        {
            services.AddSingleton<IRazorComponentClientModuleHost>(sp => new RazorComponentClientModuleHost(sp, modules));
            return base.AfterBuild(modules, services);
        }

        /// <inheritdoc/>
        public override Task AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services)
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
            return base.AfterModule(module, manifest, startup, services);
        }
    }
}
