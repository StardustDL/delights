using Delights.Modules.Server.GraphQL;
using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace Delights.Modules.Hello.Server
{
    public static class ModuleExtensions
    {
        public static ModuleCollection AddHelloModule(this ModuleCollection collection)
        {
            return collection.AddModule<Module>();
        }
    }

    public class Module : GraphQLServerModule<ModuleService, ModuleQuery, ModuleMutation, ModuleSubscription>
    {
        public Module() : base()
        {
            Metadata = Metadata with
            {
                Name = SharedMetadata.Raw.Name,
                DisplayName = SharedMetadata.Raw.DisplayName,
                Description = SharedMetadata.Raw.Description,
            };
        }
    }

    public class ModuleQuery : QueryRootObject
    {
    }

    public class ModuleMutation : MutationRootObject
    {
    }

    public class ModuleSubscription : SubscriptionRootObject
    {
    }

    public class ModuleService : Services.ModuleService
    {

    }
}
