using Delights.Modules.Notes.GraphQL;
using Modulight.Modules.Services;

namespace Delights.Modules.Notes
{
    public class ModuleService : IModuleService
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
