using Delights.Modules;
using Delights.Modules.Services;
using Delights.Modules.UI;
using Delights.UI.Services;
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
    public class MainModule : Module<MainModuleService, MainModuleUI>
    {
        public MainModule() : base("Main")
        {
        }

        public override void RegisterUI(IServiceCollection services)
        {
            base.RegisterUI(services);
            services.AddAntDesign();
        }
    }

    public class MainModuleUI : ModuleUI
    {
        public MainModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base("", jsRuntime, logger)
        {
            Resources = new UIResource[]
            {
                new UIResource(UIResourceType.StyleSheet,"_content/AntDesign/css/ant-design-blazor.css"),
                new UIResource(UIResourceType.Script,"_content/AntDesign/js/ant-design-blazor.js"),
            };
        }

        public async ValueTask Prompt(string message)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("showPrompt", message);
        }
    }

    public class MainModuleService : ModuleService
    {
        public WeatherForecastService WeatherForecastService = new WeatherForecastService();
    }
}
