using Delights.Modules.Persons.GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Modulight.Modules.Client.RazorComponents.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules.Persons
{
    [ModuleUIRootPath("persons")]
    class ModuleUI : Modulight.Modules.Client.RazorComponents.UI.ModuleUI
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
