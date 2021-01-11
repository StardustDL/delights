using Delights.Modules.Client.RazorComponents;
using Delights.Modules.Services;
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
using Delights.Modules.Client.RazorComponents.Core;
using Microsoft.Extensions.Options;
using StardustDL.RazorComponents.AntDesigns;

namespace Delights.Modules.ModuleManager
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddModuleManagerModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<Module, ModuleOption>(configureOptions);
            return modules;
        }
    }

    public class Module : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = SharedManifest.Raw.Name,
                DisplayName = SharedManifest.Raw.DisplayName,
                Description = SharedManifest.Raw.Description,
                Url = SharedManifest.Raw.Url,
                Author = SharedManifest.Raw.Author,
                Assemblies = new string[]
                {
                    $"{GetType().GetAssemblyName()}.UI"
                },
            };
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            services.AddHttpClient(
                "ModuleManagerGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint);
                });
            services.AddModuleManagerGraphQLClient();
        }

        public override void Setup(IModuleHostBuilder host)
        {
            host.AddAntDesignModule();
        }
    }

    public class ModuleUI : Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger, "modules")
        {
        }

        public override RenderFragment Icon => Fragments.Icon;

        public async ValueTask Prompt(string message)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("showPrompt", message);
        }
    }

    public class ModuleService : Services.IModuleService
    {
        public IModuleManagerGraphQLClient GraphQLClient { get; }

        public ModuleService(IModuleManagerGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
