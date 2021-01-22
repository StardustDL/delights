using Delights.Modules.Bookkeeping.GraphQL;
using Modulight.Modules.Services;

namespace Delights.Modules.Bookkeeping
{
    public class ModuleService : IModuleService
    {
        public IBookkeepingGraphQLClient GraphQLClient { get; }

        public UrlGenerator UrlGenerator { get; }

        public ModuleService(IBookkeepingGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
            UrlGenerator = new UrlGenerator();
        }
    }
}
