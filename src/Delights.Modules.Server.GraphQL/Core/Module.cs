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
            Manifest = Manifest with
            {
                Name = "CoreGraphQLServer",
                DisplayName = "Core GraphQL Server",
                Description = "Provide heartbeat and other services for GraphQL server.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }
    }

    public class ModuleQuery : QueryRootObject
    {
        public string Heartbeat() => "ok";
    }

    public class ModuleMutation : MutationRootObject
    {
        public string Heartbeat() => "ok";
    }

    public class ModuleSubscription : SubscriptionRootObject
    {
        public string Heartbeat() => "ok";
    }
}
