using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.AspNet
{
    /// <summary>
    /// Specifies the contract for aspnet modules.
    /// </summary>
    public interface IAspNetServerModule : IModule
    {
        /// <summary>
        /// Use all registered module's middlewares.
        /// Used for <see cref="UseMiddlewareExtensions.UseMiddleware(IApplicationBuilder, Type, object[])"/> 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        void UseMiddleware(IApplicationBuilder builder);

        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        void MapEndpoint(IEndpointRouteBuilder builder);
    }

    /// <summary>
    /// Basic implement for <see cref="IAspNetServerModule"/>
    /// </summary>
    public abstract class AspNetServerModule : Module, IAspNetServerModule
    {
        /// <inheritdoc/>
        public virtual void MapEndpoint(IEndpointRouteBuilder builder) { }

        /// <inheritdoc/>
        public virtual void UseMiddleware(IApplicationBuilder builder) { }
    }
}
