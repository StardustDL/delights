using Modulight.Modules.Client.RazorComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delights.Modules.ModuleManager.GraphQL;
using Microsoft.Extensions.Options;
using StardustDL.RazorComponents.AntDesigns;
using Modulight.Modules.Services;
using Modulight.Modules;
using StardustDL.RazorComponents.MaterialDesignIcons;

namespace Delights.Modules.ModuleManager
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddModuleManagerModule(this Modulight.Modules.IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ModuleManagerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class ModuleManagerModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public ModuleManagerModule() : base()
        {
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            services.AddHttpClient(
                "ModuleManagerGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/{Manifest.Name}Server");
                });
            services.AddModuleManagerGraphQLClient();
        }

        public override void Setup(Modulight.Modules.IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddAntDesignModule();
            host.AddMaterialDesignIconModule();
        }
    }

    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger, "modules")
        {
        }

        public override RenderFragment Icon => Fragments.Icon;

        public async ValueTask Prompt(string message)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("showPrompt", message);
        }
    }

    public class ModuleService : IModuleService
    {
        public IModuleManagerGraphQLClient GraphQLClient { get; }

        public ModuleService(IModuleManagerGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
