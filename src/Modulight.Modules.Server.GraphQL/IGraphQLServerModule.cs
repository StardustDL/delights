using Modulight.Modules.Options;
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
        /// Register graphql related services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IRequestExecutorBuilder RegisterGraphQLService(IServiceCollection services);

        /// <summary>
        /// Map graphql endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider);
    }
}
