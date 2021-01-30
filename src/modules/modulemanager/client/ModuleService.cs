using Delights.Modules.ModuleManager.GraphQL;

namespace Delights.Modules.ModuleManager
{
    public class ModuleService
    {
        public IModuleManagerGraphQLClient GraphQLClient { get; }

        public ModuleService(IModuleManagerGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
