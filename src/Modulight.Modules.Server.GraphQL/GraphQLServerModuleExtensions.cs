using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Routing;
using Modulight.Modules.Hosting;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Extension methods for graphql modules.
    /// </summary>
    public static class GraphQLServerModuleExtensions
    {
        /// <summary>
        /// Use building middlewares for graphql modules.
        /// It will register <see cref="IGraphQLServerModuleHost"/> service.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseGraphQLServerModules(this IModuleHostBuilder modules) => modules.UsePlugin<GraphQLServerModulePlugin>();

        /// <summary>
        /// Get graphql module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IGraphQLServerModuleHost GetGraphQLServerModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IGraphQLServerModuleHost>();

        /// <summary>
        /// Map all registered graphql server module's endpoints.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapGraphQLServerModuleEndpoints(this IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            builder.ServiceProvider.GetGraphQLServerModuleHost().MapEndpoints(builder, postMapEndpoint);
            return builder;
        }
    }
}
