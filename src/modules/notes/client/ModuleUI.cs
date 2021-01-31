using Delights.Modules.Notes.GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Modulight.Modules.Client.RazorComponents.UI;
using System.Threading.Tasks;

namespace Delights.Modules.Notes
{
    [ModuleUIRootPath("notes")]
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

    class ModuleService
    {
        public INotesGraphQLClient GraphQLClient { get; }

        public UrlGenerator UrlGenerator { get; }

        public ModuleService(INotesGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
            UrlGenerator = new UrlGenerator();
        }
    }
}
