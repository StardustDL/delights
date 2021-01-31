using Modulight.Modules.Client.RazorComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delights.Modules.Persons.GraphQL;
using Microsoft.Extensions.Options;
using StardustDL.RazorComponents.AntDesigns;
using Modulight.Modules;
using StardustDL.RazorComponents.MaterialDesignIcons;
using StardustDL.RazorComponents.Vditors;
using Delights.Modules.Client;
using Modulight.Modules.Hosting;

namespace Delights.Modules.Persons
{

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(PersonsModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ClientModule))]
    [ModuleUI(typeof(ModuleUI))]
    public class PersonsModule : RazorComponentClientModule<PersonsModule>
    {
        public PersonsModule(IModuleHost host) : base(host)
        {
        }
    }

    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddPersonsModule(this IModuleHostBuilder builder, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<PersonsModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureServices(services =>
                {
                    services.AddOptions<ModuleOption>().Configure(configureOptions);
                });
            }

            return builder;
        }
    }

    public class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(
                "PersonsGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/Persons");
                });
            services.AddPersonsGraphQLClient();
            base.ConfigureServices(services);
        }
    }
}
