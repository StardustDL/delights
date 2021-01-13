using Modulight.Modules.Options;
using Modulight.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using HotChocolate.AspNetCore.Extensions;
using Modulight.Modules.Server.AspNet;
using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules.Server.GraphQL.Bridges
{
    internal class BridgeAspNetModule : AspNetServerModule<BridgeAspNetModuleService, BridgeAspNetModuleOption>
    {
        Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? PostMapEndpoint { get; }

        public BridgeAspNetModule(Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null) : base()
        {
            Manifest = Manifest with
            {
                Name = "BridgeGraphQLAspNet",
                DisplayName = "Bridge GraphQL AspNet",
                Description = "Bridge GraphQL server modules to AspNet server module.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
            PostMapEndpoint = postMapEndpoint;
        }

        public override void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            base.MapEndpoint(builder, provider);

            provider.GetGraphQLServerModuleHost().MapEndpoints(builder, PostMapEndpoint);
        }
    }

    public class BridgeAspNetModuleService : IModuleService
    {

    }

    public class BridgeAspNetModuleOption
    {

    }
}
