using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor.Services;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.MaterialDesignIcons;
using System;

namespace Modulight.UI.Blazor
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddBlazorUIModule(this IModuleHostBuilder builder) => builder.AddModule<BlazorUiModule>();

        public static IModuleHostBuilder AddBlazorUIModule<TUIProvider>(this IModuleHostBuilder builder) where TUIProvider : class, IBlazorUIProvider
        {
            return builder.ConfigureServices(sc =>
            {
                sc.AddScoped<IBlazorUIProvider, TUIProvider>();
            }).AddBlazorUIModule();
        }
    }

    [ModuleService(typeof(BlazorUIProvider), ServiceType = typeof(IBlazorUIProvider), RegisterBehavior = ServiceRegisterBehavior.Optional)]
    [Module(Url = "https://github.com/StardustDL/delights", Author = "StardustDL", Description = "Provide user interfaces for blazor client module hosting.")]
    [ModuleUIResource(UIResourceType.StyleSheet, "https://cdn.bootcdn.net/ajax/libs/twitter-bootstrap/4.5.3/css/bootstrap.min.css")]
    [ModuleUIResource(UIResourceType.Script, "https://cdn.bootcdn.net/ajax/libs/jquery/3.5.1/jquery.slim.min.js")]
    [ModuleDependency(typeof(AntDesignModule))]
    [ModuleDependency(typeof(MaterialDesignIconModule))]
    [ModuleUIGlobalComponent(typeof(AntDesign.AntContainer))]
    public class BlazorUiModule : RazorComponentClientModule
    {
        public BlazorUiModule(IModuleHost host) : base(host)
        {
        }
    }
}
