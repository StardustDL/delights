using Modulight.Modules.Options;
using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.AspNet
{
    public static class AspNetServerModuleExtensions
    {
        public static IModuleHostBuilder AddAspNetServerModules(this IModuleHostBuilder modules, Action<Core.ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<Core.Module, Core.ModuleOption>(configureOptions);
            return modules;
        }

        public static Core.Module GetCoreAspNetServerModule(this IServiceProvider provider) => provider.GetRequiredService<Core.Module>();
    }

    public interface IAspNetServerModule : IModule
    {
        void RegisterAspNetService(IServiceCollection services);

        void UseMiddleware(IApplicationBuilder builder, IServiceProvider provider);

        void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider);
    }

    public abstract class AspNetServerModule<TService, TOption> : Module<TService, TOption>, IAspNetServerModule where TService : class, IModuleService where TOption : class
    {
        protected AspNetServerModule(ModuleManifest? manifest = null) : base(manifest)
        {
        }

        public virtual void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider) { }

        public virtual void UseMiddleware(IApplicationBuilder builder, IServiceProvider provider) { }

        public virtual void RegisterAspNetService(IServiceCollection services) { }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            RegisterAspNetService(services);
        }
    }
}
