using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Modulight.Modules.Hosting;
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
        new IEnumerable<Type> DefinedModules { get; }

        /// <summary>
        /// Get all registered modules.
        /// </summary>
        new IEnumerable<IGraphQLServerModule> LoadedModules { get; }

        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null);
    }

    internal class GraphQLServerModuleHost : ModuleHostFilter<IGraphQLServerModule>, IGraphQLServerModuleHost
    {
        public GraphQLServerModuleHost(IModuleHost host) : base(host)
        {
        }

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            foreach (var module in LoadedModules)
            {
                var gbuilder = module.MapEndpoint(builder);
                if (gbuilder is not null && postMapEndpoint is not null)
                    postMapEndpoint(module, gbuilder);
            }
        }
    }
}