using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using HotChocolate.AspNetCore.Extensions;
using Modulight.Modules.Server.GraphQL.Bridges;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Extension methods for graphql modules.
    /// </summary>
    public static class GraphQLServerModuleExtensions
    {
        /// <summary>
        /// Add bridge module to register graphql module services like aspnet modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="setupOptions"></param>
        /// <param name="configureOptions"></param>
        /// <param name="postMapEndpoint"></param>
        /// <returns></returns>
        public static IModuleHostBuilder BridgeGraphQLServerModuleToAspNet(this IModuleHostBuilder modules, Action<BridgeAspNetModuleOption>? setupOptions = null, Action<BridgeAspNetModuleOption, IServiceProvider>? configureOptions = null, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            modules.TryAddModule<BridgeAspNetModule, BridgeAspNetModuleOption>(() => new(postMapEndpoint), setupOptions, configureOptions);
            return modules;
        }

        /// <summary>
        /// Use building middlewares for graphql modules.
        /// It will register <see cref="IGraphQLServerModuleHost"/> service.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseGraphQLServerModules(this IModuleHostBuilder modules)
        {
            return modules.UsePostMiddleware((modules, services) =>
            {
                services.AddSingleton<IGraphQLServerModuleHost>(sp => new GraphQLServerModuleHost(sp,
                    modules.Modules.AllSpecifyModules<IGraphQLServerModule>().ToArray()));
            });
        }

        /// <summary>
        /// Get graphql module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IGraphQLServerModuleHost GetGraphQLServerModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IGraphQLServerModuleHost>();
    }
}
