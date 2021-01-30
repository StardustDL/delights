using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the contract for graphql modules.
    /// </summary>
    public interface IGraphQLServerModule : IModule
    {
        /// <summary>
        /// Map graphql endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        GraphQLEndpointConventionBuilder? MapEndpoint(IEndpointRouteBuilder builder);
    }
}
