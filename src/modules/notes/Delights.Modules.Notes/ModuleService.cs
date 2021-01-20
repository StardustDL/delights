using Delights.Modules.Notes.GraphQL;
using Modulight.Modules.Services;

namespace Delights.Modules.Notes
{
    public class ModuleService : IModuleService
    {
        public INotesGraphQLClient GraphQLClient { get; }

        public ModuleService(INotesGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
