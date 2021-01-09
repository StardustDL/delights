using Delights.Modules.Client;
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
using Delights.Modules.Client.Core;
using Microsoft.Extensions.Options;

namespace Delights.Modules.ModuleManager
{
    public static class ModuleExtensions
    {
        public static ModuleCollection AddModuleManagerModule(this ModuleCollection modules, Action<ModuleOption>? configureOptions = null)
        {
            modules.AddModule<Module, ModuleOption>(configureOptions);
            return modules;
        }
    }

    public class Module : ClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Metadata = Metadata with
            {
                Name = SharedMetadata.Raw.Name,
                DisplayName = SharedMetadata.Raw.DisplayName,
                Description = SharedMetadata.Raw.Description,
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
    }

    public class ModuleUI : Client.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Client.UI.ModuleUI> logger) : base(jsRuntime, logger, "modulemanager")
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
