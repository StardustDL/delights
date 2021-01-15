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
        public static IModuleHostBuilder BridgeGraphQLServerModuleToAspNet(this IModuleHostBuilder modules, Action<BridgeAspNetModuleOption>? setupOptions = null, Action<BridgeAspNetModuleOption, IServiceProvider>? configureOptions = null, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            modules.TryAddModule<BridgeAspNetModule, BridgeAspNetModuleOption>(() => new(postMapEndpoint), setupOptions, configureOptions);
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

    public interface IGraphQLServerModule : IModule
    {
        IRequestExecutorBuilder RegisterGraphQLService(IServiceCollection services);

        GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider);
    }

    public abstract class GraphQLServerModule<TService, TOption> : Module<TService, TOption>, IGraphQLServerModule where TService : class, IModuleService where TOption : class, IGraphQLServerModuleOption, new()
    {
        protected GraphQLServerModule(ModuleManifest? manifest = null) : base(manifest)
        {
            SetupOptions(_ => { });
        }

        public virtual Type? QueryType { get; }

        public virtual Type? MutationType { get; }

        public virtual Type? SubscriptionType { get; }

        public override void SetupOptions(Action<TOption> setupOptions)
        {
            base.SetupOptions(o =>
            {
                setupOptions(o);
                if (o.SchemaName is "")
                {
                    o.SchemaName = Manifest.Name;
                }
                if (o.Endpoint is "")
                {
                    o.Endpoint = $"/graphql/{o.SchemaName}";
                }
            });
        }

        public virtual IRequestExecutorBuilder RegisterGraphQLService(IServiceCollection services)
        {
            string schemaName = GetSetupOptions().SchemaName;

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
            var options = GetSetupOptions();
            string schemaName = options.SchemaName;
            string endpoint = options.Endpoint;

            return builder.MapGraphQL(endpoint.TrimEnd('/'), schemaName);
        }
    }
}
