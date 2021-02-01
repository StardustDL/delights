using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
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
            services.AddScoped(typeof(IJSModuleProvider<>), typeof(JSModuleProvider<>));
            services.AddScoped<ModuleUILoader>();
            services.AddScoped<LazyAssemblyLoader>();
            base.AfterBuild(modules, services);
        }
    }
}
