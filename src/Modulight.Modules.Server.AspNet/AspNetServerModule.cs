using Modulight.Modules.Options;
using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.AspNet
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class AspNetServerModuleExtensions
    {
        /// <summary>
        /// Use building middlewares for aspnet modules.
        /// It will register <see cref="IAspNetServerModuleHost"/> service.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseAspNetServerModules(this IModuleHostBuilder modules)
        {
            return modules.UsePostMiddleware((modules, services) =>
            {
                services.AddSingleton<IAspNetServerModuleHost>(sp => new AspNetServerModuleHost(sp,
                    modules.Modules.AllSpecifyModules<IAspNetServerModule>().ToArray()));
            });
        }

        /// <summary>
        /// Get aspnet module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IAspNetServerModuleHost GetAspNetServerModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IAspNetServerModuleHost>();
    }

    /// <summary>
    /// Specifies the contract for aspnet modules.
    /// </summary>
    public interface IAspNetServerModule : IModule
    {
        /// <summary>
        /// Register aspnet related services.
        /// </summary>
        /// <param name="services"></param>
        void RegisterAspNetServices(IServiceCollection services);

        /// <summary>
        /// Use all registered module's middlewares.
        /// Used for <see cref="UseMiddlewareExtensions.UseMiddleware(IApplicationBuilder, Type, object[])"/> 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        void UseMiddleware(IApplicationBuilder builder, IServiceProvider provider);

        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider);
    }

    /// <summary>
    /// Basic implement for <see cref="IAspNetServerModule"/>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TOption"></typeparam>
    public abstract class AspNetServerModule<TService, TOption> : Module<TService, TOption>, IAspNetServerModule where TService : class, IModuleService where TOption : class, new()
    {
        /// <inheritdoc/>
        protected AspNetServerModule(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        /// <inheritdoc/>
        public virtual void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider) { }

        /// <inheritdoc/>
        public virtual void UseMiddleware(IApplicationBuilder builder, IServiceProvider provider) { }

        /// <inheritdoc/>
        public virtual void RegisterAspNetServices(IServiceCollection services) { }

        /// <inheritdoc/>
        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterAspNetServices(services);
        }
    }
}
