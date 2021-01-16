using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.Server.AspNet
{
    /// <summary>
    /// Specifies the contract for aspnet module hosts.
    /// </summary>
    public interface IAspNetServerModuleHost : IModuleHost
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
        new IReadOnlyList<IAspNetServerModule> Modules { get; }

        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        void MapEndpoints(IEndpointRouteBuilder builder, Action<IAspNetServerModule, IEndpointRouteBuilder>? postMapEndpoint = null);

        /// <summary>
        /// Use all registered module's middlewares.
        /// Used for <see cref="UseMiddlewareExtensions.UseMiddleware(IApplicationBuilder, Type, object[])"/> 
        /// </summary>
        /// <param name="builder"></param>
        void UseMiddlewares(IApplicationBuilder builder);
    }

    internal class AspNetServerModuleHost : IAspNetServerModuleHost
    {
        public AspNetServerModuleHost(IServiceProvider services, IReadOnlyList<IAspNetServerModule> modules)
        {
            Services = services;
            Modules = modules;
        }

        public IServiceProvider Services { get; }

        public IReadOnlyList<IAspNetServerModule> Modules { get; }

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IAspNetServerModule, IEndpointRouteBuilder>? postMapEndpoint = null)
        {
            foreach (var module in Modules)
            {
                module.MapEndpoint(builder, Services);
                if (postMapEndpoint is not null)
                    postMapEndpoint(module, builder);
            }
        }

        public void UseMiddlewares(IApplicationBuilder builder)
        {
            foreach (var module in Modules)
            {
                module.UseMiddleware(builder, Services);
            }
        }

        IReadOnlyList<IModule> IModuleHost.Modules => Modules;
    }
}