using Delights.Modules.Options;
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
        public static IModuleHost AddGraphQLServerModules(this IModuleHost modules)
        {
            modules.AddModule<Core.Module>();
            return modules;
        }

        public static IRequestExecutorBuilder RegisterGraphQLServerModules(this IRequestExecutorBuilder builder, IModuleHost modules)
        {
            builder.AddQueryType(d => d.Name(nameof(RootObjectType.Query)))
                .AddMutationType(d => d.Name(nameof(RootObjectType.Mutation)))
                .AddSubscriptionType(d => d.Name(nameof(RootObjectType.Subscription)));

            foreach (var module in modules.AllSpecifyModules<IGraphQLServerModule>())
            {
                module.RegisterGraphQLTypes(builder);
            }
            return builder;
        }
    }

    public enum RootObjectType
    {
        Query,
        Mutation,
        Subscription
    }

    public interface IGraphQLServerModule : IModule
    {
        IRequestExecutorBuilder RegisterGraphQLTypes(IRequestExecutorBuilder builder);
    }

    public abstract class GraphQLServerModule<TService, TOption, TQuery, TMutation, TSubscription> : Module<TService, TOption>, IGraphQLServerModule where TService : class, IModuleService where TOption : class where TQuery : QueryRootObject where TMutation : MutationRootObject where TSubscription : SubscriptionRootObject
    {
        protected GraphQLServerModule(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        public virtual IRequestExecutorBuilder RegisterGraphQLTypes(IRequestExecutorBuilder builder)
        {
            return builder.AddTypeExtension<TQuery>()
                          .AddTypeExtension<TMutation>()
                          .AddTypeExtension<TSubscription>();
        }
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

    public sealed class EmptyQueryRootObject<T> : QueryRootObject
    {

    }

    public sealed class EmptySubscriptionRootObject<T> : SubscriptionRootObject
    {

    }

    public sealed class EmptyMutationRootObject<T> : MutationRootObject
    {

    }
}
