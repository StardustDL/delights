using Delights.Modules.Server.GraphQL;
using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace Delights.Modules.Hello.Server
{
    public class Module : GraphQLServerModule<ModuleService, ModuleQuery, ModuleMutation, ModuleSubscription>
    {
        public Module() : base("Hello")
        {
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
