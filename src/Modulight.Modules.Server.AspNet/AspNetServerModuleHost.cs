using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.Server.AspNet
{
    public interface IAspNetServerModuleHost : IModuleHost
    {
        new IReadOnlyList<IAspNetServerModule> Modules { get; }

        void MapEndpoints(IEndpointRouteBuilder builder, Action<IAspNetServerModule, IEndpointRouteBuilder>? postMapEndpoint = null);

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