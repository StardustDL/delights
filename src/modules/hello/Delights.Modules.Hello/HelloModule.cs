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
using StardustDL.RazorComponents.MaterialDesignIcons;

namespace Delights.Modules.Hello
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddHelloModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<HelloModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class HelloModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public HelloModule() : base()
        {
            Manifest = Manifest with
            {
                Assemblies = new string[]
                {
                    $"{GetType().GetAssemblyName()}.UI"
                },
            };
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
            services.AddHttpClient(
                "HelloGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/{Manifest.Name}Server");
                });
            services.AddHelloGraphQLClient();
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddMaterialDesignIconModule();
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
