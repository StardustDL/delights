using Delights.Modules.ModuleManager.GraphQL;
using Modulight.Modules.Services;

namespace Delights.Modules.ModuleManager
{
    public class ModuleService : IModuleService
    {
        public IModuleManagerGraphQLClient GraphQLClient { get; }

        public ModuleService(IModuleManagerGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
