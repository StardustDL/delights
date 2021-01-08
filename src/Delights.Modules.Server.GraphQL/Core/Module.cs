using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace Delights.Modules.Server.GraphQL.Core
{
    public class Module : GraphQLServerModule<ModuleService<Module>, ModuleQuery, ModuleMutation, ModuleSubscription>
    {
        public Module() : base("CoreGraphQLServer")
        {
        }
    }

    public class ModuleQuery : QueryRootObject
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

    public class ModuleMutation : MutationRootObject
    {
        public string Hello() => "world";
    }

    public class ModuleSubscription : SubscriptionRootObject
    {
        public string Hello() => "world";
    }
}
