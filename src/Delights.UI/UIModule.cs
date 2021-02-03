using Delights.Modules.Client;
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
using System.Threading.Tasks;

namespace Delights.UI
{
    [Modulight.Modules.Module(Description = "Provide user interfaces for client module hosting.", Url = Modules.Shared.SharedManifest.Url, Author = Modules.Shared.SharedManifest.Author)]
    [ModuleUIRootPath("home")]
    [ModuleUIResource(UIResourceType.StyleSheet, "https://cdn.bootcdn.net/ajax/libs/twitter-bootstrap/4.5.3/css/bootstrap.min.css")]
    [ModuleUIResource(UIResourceType.Script, "https://cdn.bootcdn.net/ajax/libs/jquery/3.5.1/jquery.slim.min.js")]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ClientModule))]
    public class UiModule : RazorComponentClientModule
    {
        public UiModule(IModuleHost host) : base(host)
        {
        }

        public override RenderFragment? Icon => Components.Fragments.Icon;

        public override async Task Initialize()
        {
            await base.Initialize();
        }
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}
