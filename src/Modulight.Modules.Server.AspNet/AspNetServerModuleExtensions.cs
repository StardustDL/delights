﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Server.AspNet
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class AspNetServerModuleExtensions
    {
        /// <summary>
        /// Use building middlewares for aspnet modules.
        /// It will register <see cref="IAspNetServerModuleCollection"/> service.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        [Obsolete("No need to use this method. The plugin has been removed.")]
        public static IModuleHostBuilder UseAspNetServerModules(this IModuleHostBuilder modules) => modules;

        /// <summary>
        /// Get aspnet module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IAspNetServerModuleCollection GetAspNetServerModuleCollection(this IServiceProvider provider) => provider.GetRequiredService<IAspNetServerModuleCollection>();

        /// <summary>
        /// Use all registered aspnet server module's middlewares.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAspNetServerModuleMiddlewares(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetAspNetServerModuleCollection().UseMiddlewares(builder);
            return builder;
        }

        /// <summary>
        /// Map all registered aspnet server module's endpoints.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapAspNetServerModuleEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.ServiceProvider.GetAspNetServerModuleCollection().MapEndpoints(builder);
            return builder;
        }
    }
}
