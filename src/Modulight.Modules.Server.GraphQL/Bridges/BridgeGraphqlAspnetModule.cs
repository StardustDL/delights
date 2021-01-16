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
    [Module(
        Author = "StardustDL",
        Description = "Bridge GraphQL server modules to AspNet server module.",
        Url = "https://github.com/StardustDL/delights")]
    internal class BridgeGraphqlAspnetModule : AspNetServerModule<BridgeGraphqlAspnetModuleService, BridgeGraphqlAspnetModuleOption>
    {
        Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? PostMapEndpoint { get; }

        public BridgeGraphqlAspnetModule(Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null) : base()
        {
            PostMapEndpoint = postMapEndpoint;
        }

        public override void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            base.MapEndpoint(builder, provider);

            provider.GetGraphQLServerModuleHost().MapEndpoints(builder, PostMapEndpoint);
        }
    }

    internal class BridgeGraphqlAspnetModuleService : IModuleService
    {

    }

    /// <summary>
    /// Options for GraphQL-AspNet bridge module.
    /// </summary>
    public class BridgeGraphqlAspnetModuleOption
    {

    }
}
