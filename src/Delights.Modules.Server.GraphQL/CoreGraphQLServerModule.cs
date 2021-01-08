using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace Delights.Modules.Server.GraphQL
{
    public class CoreGraphQLServerModule : GraphQLServerModule<ModuleService<CoreGraphQLServerModule>, CoreGraphQLServerModuleQuery, CoreGraphQLServerModuleQueryMutation, CoreGraphQLServerModuleQuerySubscription>
    {
        public CoreGraphQLServerModule() : base("CoreGraphQLServer")
        {
        }
    }

    public class CoreGraphQLServerModuleQuery : QueryRootObject
    {
        public record ModuleMetadata
        {
            public string Name { get; init; } = "";
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ModuleMetadata> GetModules([Service] ModuleCollection collection)
        {
            return collection.Modules.Select(m => new ModuleMetadata { Name = m.Name }).AsQueryable();
        }
    }

    public class CoreGraphQLServerModuleQueryMutation : MutationRootObject
    {
        public string Hello() => "world";
    }

    public class CoreGraphQLServerModuleQuerySubscription : SubscriptionRootObject
    {
        public string Hello() => "world";
    }
}
