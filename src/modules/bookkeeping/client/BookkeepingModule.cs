using Modulight.Modules.Client.RazorComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delights.Modules.Bookkeeping.GraphQL;
using Microsoft.Extensions.Options;
using StardustDL.RazorComponents.AntDesigns;
using Modulight.Modules;
using StardustDL.RazorComponents.MaterialDesignIcons;
using StardustDL.RazorComponents.Vditors;
using Delights.Modules.Client;

namespace Delights.Modules.Bookkeeping
{

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class BookkeepingModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public BookkeepingModule() : base()
        {
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
            services.AddHttpClient(
                "BookkeepingGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/{Manifest.Name}Server");
                });
            services.AddBookkeepingGraphQLClient();
        }

        public override void Setup(Modulight.Modules.IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddClientModule();
        }
    }
}
