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
using Delights.Modules.Hello.GraphQL;
using Microsoft.Extensions.Options;
using Modulight.Modules;

namespace Delights.Modules.Hello
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddHelloModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
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
                "HelloGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/{SharedManifest.Raw.Name}");
                });
            services.AddHelloGraphQLClient();
        }
    }

    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Modulight.Modules.Client.RazorComponents.UI.ModuleUI> logger) : base(jsRuntime, logger, "hello")
        {
        }

        public override RenderFragment Icon => Fragments.Icon;

        public async ValueTask Prompt(string message)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("showPrompt", message);
        }
    }

    public class ModuleService : Modulight.Modules.Services.IModuleService
    {
        public IHelloGraphQLClient GraphQLClient { get; }

        public ModuleService(IHelloGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
