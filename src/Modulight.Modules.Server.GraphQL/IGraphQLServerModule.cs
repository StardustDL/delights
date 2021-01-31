using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

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
