using Delights.Modules.Options;
using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace Delights.Modules.Server.GraphQL.Core
{
    public class Module : GraphQLServerModule<EmptyModuleService<Module>, EmptyModuleOption<Module>, ModuleQuery, ModuleMutation, ModuleSubscription>
    {
        public Module() : base()
        {
        }
    }

    public class ModuleQuery : QueryRootObject
    {
        public string Hello() => "world";

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ModuleMetadata> GetModules([Service] ModuleCollection collection)
        {
            return collection.Modules.Select(m => m.Metadata).AsQueryable();
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
