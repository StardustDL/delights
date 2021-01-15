using Modulight.Modules.Options;
using Modulight.Modules.Services;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Modulight.Modules.Server.GraphQL.Bridges;

namespace Modulight.Modules.Server.GraphQL
{
    public static class GraphQLServerModuleExtensions
    {
        public static IModuleHostBuilder BridgeGraphQLServerModuleToAspNet(this IModuleHostBuilder modules, Action<BridgeAspNetModuleOption, IServiceProvider>? configureOptions = null, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            modules.TryAddModule<BridgeAspNetModule, BridgeAspNetModuleOption>(() => new(postMapEndpoint), configureOptions);
            return modules;
        }

        public static IModuleHostBuilder UseGraphQLServerModules(this IModuleHostBuilder modules)
        {
            return modules.UsePostMiddleware((modules, services) =>
            {
                services.AddSingleton<IGraphQLServerModuleHost>(sp => new GraphQLServerModuleHost(sp,
                    modules.Modules.AllSpecifyModules<IGraphQLServerModule>().ToArray()));
            });
        }

        public static IGraphQLServerModuleHost GetGraphQLServerModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IGraphQLServerModuleHost>();
    }

    public enum RootObjectType
    {
        Query,
        Mutation,
        Subscription
    }

    public interface IGraphQLServerModule : IModule
    {
        string? SchemaName { get; }

        string? Endpoint { get; }

        IRequestExecutorBuilder RegisterGraphQLService(IServiceCollection services);

        GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider);
    }

    public abstract class GraphQLServerModule<TService, TOption> : Module<TService, TOption>, IGraphQLServerModule where TService : class, IModuleService where TOption : class
    {
        protected GraphQLServerModule(string? schemaName = null, string? endpoint = null, ModuleManifest? manifest = null) : base(manifest)
        {
            SchemaName = schemaName;
            Endpoint = endpoint;
        }

        /// <summary>
        /// SchemaName, default to Manifest.Name
        /// </summary>
        public virtual string? SchemaName { get; protected set; }

        /// <summary>
        /// Endpoint route, default to /graphql/{SchemaName}
        /// </summary>
        public virtual string? Endpoint { get; protected set; }

        public virtual Type? QueryType { get; }

        public virtual Type? MutationType { get; }

        public virtual Type? SubscriptionType { get; }

        public virtual IRequestExecutorBuilder RegisterGraphQLService(IServiceCollection services)
        {
            string schemaName = SchemaName ?? Manifest.Name;

            var builder = services.AddGraphQLServer(schemaName);
            if (QueryType is not null)
            {
                builder.AddQueryType(QueryType);
            }
            if (MutationType is not null)
            {
                builder.AddMutationType(MutationType);
            }
            if (SubscriptionType is not null)
            {
                builder.AddSubscriptionType(SubscriptionType);
            }
            builder.AddFiltering().AddSorting().AddProjections();
            return builder;
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterGraphQLService(services);
        }

        public virtual GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            string schemaName = SchemaName ?? Manifest.Name;
            string endpoint = Endpoint ?? $"/graphql/{schemaName}";

            return builder.MapGraphQL(endpoint.TrimEnd('/'), schemaName);
        }
    }
}
