using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Client.RazorComponents
{
    internal sealed class RazorComponentClientModulePlugin : ModuleHostBuilderPlugin
    {
        public override void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services)
        {
            services.AddSingleton<IRazorComponentClientModuleCollection>(sp => new RazorComponentClientModuleCollection(sp.GetRequiredService<IModuleHost>()));
            services.AddScoped<LazyAssemblyLoader>();
            base.AfterBuild(modules, services);
        }

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
