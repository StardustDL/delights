using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modulight.Modules.Hosting;
using System.Reflection;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace Modulight.Modules.Client.RazorComponents
{
    internal sealed class RazorComponentClientModulePlugin : ModuleHostBuilderPlugin
    {
        /// <inheritdoc/>
        public override void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services)
        {
            services.AddSingleton<IRazorComponentClientModuleCollection>(sp => new RazorComponentClientModuleCollection(sp.GetRequiredService<IModuleHost>()));
            services.AddScoped<LazyAssemblyLoader>();
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
