using Modulight.Modules.Services;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Routing;
using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Basic implement for <see cref="IGraphQLServerModule"/>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TOption"></typeparam>
    public abstract class GraphQLServerModule<TService, TOption> : Module<TService, TOption>, IGraphQLServerModule where TService : class, IModuleService where TOption : class, IGraphQLServerModuleOption, new()
    {
        /// <inheritdoc/>
        protected GraphQLServerModule(ModuleManifest? manifest = null) : base(manifest)
        {
            SetupOptions(_ => { });
        }

        /// <summary>
        /// Query type for <see cref="SchemaRequestExecutorBuilderExtensions.AddQueryType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public abstract Type QueryType { get; }

        /// <summary>
        /// Mutation type for <see cref="SchemaRequestExecutorBuilderExtensions.AddMutationType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public virtual Type? MutationType { get; }

        /// <summary>
        /// Subscription type for <see cref="SchemaRequestExecutorBuilderExtensions.AddSubscriptionType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public virtual Type? SubscriptionType { get; }

        /// <summary>
        /// <inheritdoc/>
        /// <para>
        /// When <see cref="IGraphQLServerModuleOption.SchemaName"/> is empty, it will use <see cref="ModuleManifest.Name"/> for default.
        /// </para>
        /// <para>
        /// When <see cref="IGraphQLServerModuleOption.Endpoint"/> is empty, it will use /graphql/<see cref="IGraphQLServerModuleOption.SchemaName"/> for default.
        /// </para>
        /// </summary>
        /// <param name="setupOptions"></param>
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

        /// <summary>
        /// <inheritdoc/>
        /// It use <see cref="QueryType"/>, <see cref="MutationType"/> and <see cref="SubscriptionType"/> to register graphql schema.
        /// And it enable support for <see cref="HotChocolateDataRequestBuilderExtensions.AddFiltering(IRequestExecutorBuilder)"/>, <see cref="HotChocolateDataRequestBuilderExtensions.AddSorting(IRequestExecutorBuilder)"/> and <see cref="HotChocolateDataRequestBuilderExtensions.AddProjections(IRequestExecutorBuilder)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IRequestExecutorBuilder RegisterGraphQLService(IServiceCollection services)
        {
            string schemaName = GetSetupOptions(new TOption()).SchemaName;

            var builder = services.AddGraphQLServer(schemaName);
            builder.AddQueryType(QueryType);
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

        /// <inheritdoc/>
        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
            RegisterGraphQLService(services);
        }

        /// <inheritdoc/>
        public virtual GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            var options = GetSetupOptions(new TOption());
            string schemaName = options.SchemaName;
            string endpoint = options.Endpoint;

            return builder.MapGraphQL(endpoint.TrimEnd('/'), schemaName);
        }
    }
}
