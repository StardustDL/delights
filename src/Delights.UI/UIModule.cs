using Delights.Modules;
using Delights.Modules.Client.RazorComponents;
using Delights.Modules.Client.RazorComponents.UI;
using Delights.Modules.Options;
using Delights.Modules.Services;
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

namespace Delights.UI
{
    public class UIModule : RazorComponentClientModule<EmptyModuleService<UIModule>, EmptyModuleOption<UIModule>, MainModuleUI>
    {
        public UIModule() : base()
        {
            Manifest = Manifest with
            {
                Name = "ClientUI",
                DisplayName = "Home",
                Description = "Provide user interfaces for client module hosting.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }

        public override void RegisterUI(IServiceCollection services)
        {
            base.RegisterUI(services);
            services.AddAntDesign();
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            services.AddHttpClient();
        }

        public override void Setup(IModuleHostBuilder host)
        {
            host.AddAntDesignModule();
        }
    }

    public class MainModuleUI : Modules.Client.RazorComponents.UI.ModuleUI
    {
        public MainModuleUI(IJSRuntime jsRuntime, ILogger<Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger, "home")
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet, "_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css"),
                new UIResource(UIResourceType.StyleSheet, "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css"),
                new UIResource(UIResourceType.Script,"https://code.jquery.com/jquery-3.3.1.slim.min.js"),
            };
        }

        public override RenderFragment? Icon => Components.Fragments.Icon;
    }
}
