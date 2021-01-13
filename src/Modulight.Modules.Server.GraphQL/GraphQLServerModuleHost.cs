using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.Server.GraphQL
{
    public interface IGraphQLServerModuleHost : IModuleHost
    {
        new IReadOnlyList<IGraphQLServerModule> Modules { get; }

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