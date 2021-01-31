using Delights.Modules.ModuleManager.GraphQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Hosting;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.MaterialDesignIcons;
using System;

namespace Delights.Modules.ModuleManager
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleStartup(typeof(Startup))]
    [ModuleOption(typeof(ModuleOption))]
    [ModuleService(typeof(ModuleService))]
    [ModuleUI(typeof(ModuleUI))]
    //TODO: antd icon
    public class ModuleManagerModule : RazorComponentClientModule<ModuleManagerModule>
    {
        public ModuleManagerModule(IModuleHost host) : base(host)
        {
        }
    }

    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddModuleManagerModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<ModuleManagerModule>();
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

    public class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(
                "ModuleManagerGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/ModuleManager");
                });
            services.AddModuleManagerGraphQLClient();
            base.ConfigureServices(services);
        }
    }
}
