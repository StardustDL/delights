using Modulight.Modules.Client.RazorComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delights.Modules.ModuleManager.GraphQL;
using Microsoft.Extensions.Options;
using StardustDL.RazorComponents.AntDesigns;
using Modulight.Modules;
using StardustDL.RazorComponents.MaterialDesignIcons;

namespace Delights.Modules.ModuleManager
{

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class ModuleManagerModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public ModuleManagerModule() : base()
        {
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
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
}
