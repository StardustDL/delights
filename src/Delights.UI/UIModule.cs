using Modulight.Modules.Services;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Delights.UI.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StardustDL.RazorComponents.AntDesigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modulight.Modules.Options;
using StardustDL.RazorComponents.MaterialDesignIcons;

namespace Delights.UI
{
    [Modulight.Modules.Module(Description = "Provide user interfaces for client module hosting.", Url = Modules.Shared.SharedManifest.Url, Author = Modules.Shared.SharedManifest.Author)]
    public class UiModule : RazorComponentClientModule<EmptyModuleService<UiModule>, EmptyModuleOption<UiModule>, MainModuleUI>
    {
        public UiModule() : base()
        {
        }

        public override void RegisterUI(IServiceCollection services)
        {
            base.RegisterUI(services);
            services.AddAntDesign();
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
            services.AddHttpClient();
        }

        public override void Setup(Modulight.Modules.IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddAntDesignModule();
            host.AddMaterialDesignIconModule();
        }
    }

    public class MainModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public MainModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger, "home")
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet, "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css"),
                new UIResource(UIResourceType.Script,"https://code.jquery.com/jquery-3.3.1.slim.min.js"),
            };
        }

        public override RenderFragment? Icon => Components.Fragments.Icon;
    }
}
