using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.MaterialDesignIcons;

namespace Delights.UI
{
    [Modulight.Modules.Module(Description = "Provide user interfaces for client module hosting.", Url = Modules.Shared.SharedManifest.Url, Author = Modules.Shared.SharedManifest.Author)]
    [ModuleUI(typeof(MainModuleUI))]
    [ModuleStartup(typeof(Startup))]
    //TODO: Antdesign, mat icon
    public class UiModule : RazorComponentClientModule<UiModule>
    {
        public UiModule(IModuleHost host) : base(host)
        {
        }
    }

    [ModuleUIRootPath("home")]
    [ModuleUIResource(UIResourceType.StyleSheet, "https://cdn.bootcdn.net/ajax/libs/twitter-bootstrap/4.5.3/css/bootstrap.min.css")]
    [ModuleUIResource(UIResourceType.Script, "https://cdn.bootcdn.net/ajax/libs/jquery/3.5.1/jquery.slim.min.js")]
    [ModuleUIResource(UIResourceType.StyleSheet, "_content/AntDesign/css/ant-design-blazor.css")]
    [ModuleUIResource(UIResourceType.Script, "_content/AntDesign/js/ant-design-blazor.js")]
    public class MainModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public MainModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger)
        {
        }

        public override RenderFragment? Icon => Components.Fragments.Icon;
    }

    public class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAntDesign();
            base.ConfigureServices(services);
        }
    }
}
