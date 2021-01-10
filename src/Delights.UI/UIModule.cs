using Delights.Modules;
using Delights.Modules.Client;
using Delights.Modules.Client.UI;
using Delights.Modules.Options;
using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.UI
{
    public class UIModule : ClientModule<EmptyModuleService<UIModule>, EmptyModuleOption<UIModule>, MainModuleUI>
    {
        public UIModule() : base()
        {
            Manifest = Manifest with
            {
                Name = "ClientUI",
                DisplayName = "Client UI",
                Description = "Provide user interfaces for client module hosting."
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
            services.AddScoped<Microsoft.AspNetCore.Components.WebAssembly.Services.LazyAssemblyLoader>();
        }
    }

    public class MainModuleUI : ModuleUI
    {
        public MainModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base(jsRuntime, logger)
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet,"_content/AntDesign/css/ant-design-blazor.css"),
                new UIResource(UIResourceType.Script,"_content/AntDesign/js/ant-design-blazor.js"),
                new UIResource(UIResourceType.StyleSheet, "_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css"),
                new UIResource(UIResourceType.StyleSheet, "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css"),
                new UIResource(UIResourceType.Script,"https://code.jquery.com/jquery-3.3.1.slim.min.js"),
            };
        }
    }
}
