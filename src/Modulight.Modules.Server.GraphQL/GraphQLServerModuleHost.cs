using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the contract for graphql module hosts.
    /// </summary>
    public interface IGraphQLServerModuleHost : IModuleHost
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
        new IReadOnlyList<IGraphQLServerModule> Modules { get; }

        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null);
    }

    internal class GraphQLServerModuleHost : IGraphQLServerModuleHost
    {
        public GraphQLServerModuleHost(IServiceProvider services, IReadOnlyList<IGraphQLServerModule> modules)
        {
            Services = services;
            Modules = modules;
        }

        public IServiceProvider Services { get; }

        public IReadOnlyList<IGraphQLServerModule> Modules { get; }

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            foreach (var module in Modules)
            {
                var gbuilder = module.MapEndpoint(builder, Services);
                if (postMapEndpoint is not null)
                    postMapEndpoint(module, gbuilder);
            }
        }

        IReadOnlyList<IModule> IModuleHost.Modules => Modules;
    }
}