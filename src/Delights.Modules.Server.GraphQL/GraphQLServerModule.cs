using Delights.Modules.Services;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delights.Modules.Server.GraphQL
{
    public static class GraphQLServerModuleExtensions
    {
        public static ModuleCollection AddGraphQLServerModules(this ModuleCollection modules)
        {
            modules.AddModule<CoreGraphQLServerModule>();
            return modules;
        }

        public static IRequestExecutorBuilder RegisterGraphQLServerModules(this IRequestExecutorBuilder builder, ModuleCollection modules)
        {
            builder.AddQueryType(d => d.Name(nameof(RootObjectType.Query)))
                .AddMutationType(d => d.Name(nameof(RootObjectType.Mutation)))
                .AddSubscriptionType(d => d.Name(nameof(RootObjectType.Subscription)));

            foreach (var module in modules.AllGraphQLServerModules())
            {
                module.RegisterGraphQLTypes(builder);
            }
            return builder;
        }

        public static IEnumerable<GraphQLServerModule> AllGraphQLServerModules(this ModuleCollection modules)
        {
            return modules.Modules.Where(m => m is GraphQLServerModule).Select(m => (m as GraphQLServerModule)!);
        }
    }

    public enum RootObjectType
    {
        Query,
        Mutation,
        Subscription
    }

    public abstract class GraphQLServerModule : Module
    {
        protected GraphQLServerModule(string name, string[]? assemblies = null) : base(name, assemblies)
        {
        }

        public virtual IRequestExecutorBuilder RegisterGraphQLTypes(IRequestExecutorBuilder builder)
        {
            return builder;
        }
    }

    public abstract class GraphQLServerModule<TService, TQuery, TMutation, TSubscription> : GraphQLServerModule where TService : ModuleService where TQuery : QueryRootObject where TMutation : MutationRootObject where TSubscription : SubscriptionRootObject
    {
        protected GraphQLServerModule(string name, string[]? assemblies = null) : base(name, assemblies)
        {
        }

        public override IRequestExecutorBuilder RegisterGraphQLTypes(IRequestExecutorBuilder builder)
        {
            return builder.AddTypeExtension<TQuery>()
                          .AddTypeExtension<TMutation>()
                          .AddTypeExtension<TSubscription>();
        }

        public override void RegisterService(IServiceCollection services)
        {
            services.AddScoped<TService>();
        }

        public override ModuleService GetService(IServiceProvider provider) => provider.GetRequiredService<TService>();
    }

    [ExtendObjectType(Name = nameof(RootObjectType.Query))]
    public abstract class QueryRootObject
    {

    }

    [ExtendObjectType(Name = nameof(RootObjectType.Subscription))]
    public abstract class SubscriptionRootObject
    {

    }

    [ExtendObjectType(Name = nameof(RootObjectType.Mutation))]
    public abstract class MutationRootObject
    {

    }

    public class QueryRootObject<T> : QueryRootObject
    {

    }

    public class SubscriptionRootObject<T> : SubscriptionRootObject
    {

    }

    public class MutationRootObject<T> : MutationRootObject
    {

    }
}
