using Delights.Modules.Client;
using Delights.Modules.Persons.GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.MaterialDesignIcons;
using StardustDL.RazorComponents.Vditors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules.Persons
{
    /// <summary>
    /// Client module for persons.
    /// </summary>
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(PersonsModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ClientModule))]
    [ModuleUIRootPath("persons")]
    public class PersonsModule : RazorComponentClientModule<PersonsModule>
    {
        public PersonsModule(IModuleHost host) : base(host)
        {
        }

        public override RenderFragment Icon => Fragments.Icon;
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

    class Startup : ModuleStartup
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

    public class ModuleOption
    {
        public string GraphQLEndpoint { get; set; } = "";
    }

    public class PersonsModuleService
    {
        public IPersonsGraphQLClient GraphQLClient { get; }

        public UrlGenerator UrlGenerator { get; }

        public PersonsModuleService(IPersonsGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
            UrlGenerator = new UrlGenerator();
        }

        public async Task<IData> GetPerson(string id)
        {
            var result = await GraphQLClient.GetDataByIdAsync(id);
            result.EnsureNoErrors();
            return result.Data!.DataById;
        }

        public async IAsyncEnumerable<IData> GetAllPersons()
        {
            var result = await GraphQLClient.GetMetadataIdsAsync();
            result.EnsureNoErrors();
            foreach (var item in result.Data!.Metadata.Nodes)
            {
                var pr = await GraphQLClient.GetDataByMetadataIdAsync(item.Id);
                pr.EnsureNoErrors();
                yield return pr.Data!.DataByMetadataId;
            }
        }
    }
}
