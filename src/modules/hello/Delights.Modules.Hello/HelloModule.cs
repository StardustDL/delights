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
using Modulight.Modules.Hosting;
using Modulight.Modules.Client.RazorComponents.UI;

namespace Delights.Modules.Hello
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddHelloModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<HelloModule>();
            if (configureOptions is not null)
            {
                modules.ConfigureServices(services =>
                {
                    services.AddOptions<ModuleOption>().Configure(configureOptions);
                });
            }
            return modules;
        }
    }

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleAssembly("Delights.Modules.Hello.UI")]
    [ModuleStartup(typeof(Startup))]
    [ModuleService(typeof(ModuleService))]
    [ModuleUI(typeof(ModuleUI))]
    //TODO: [ModuleDependency(typeof(StardustDL.RazorComponents.MaterialDesignIcons.Module))]
    public class HelloModule : RazorComponentClientModule<HelloModule>
    {
        public HelloModule(IModuleHost host) : base(host)
        {
        }
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(
                "HelloGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/Hello");
                });
            services.AddHelloGraphQLClient();
            base.ConfigureServices(services);
        }
    }

    [ModuleUIRootPath("hello")]
    public class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base(jsRuntime, logger)
        {
        }

        public override RenderFragment Icon => Fragments.Icon;

        public async ValueTask Prompt(string message)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("showPrompt", message);
        }
    }

    public class ModuleService
    {
        public IHelloGraphQLClient GraphQLClient { get; }

        public ModuleService(IHelloGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
