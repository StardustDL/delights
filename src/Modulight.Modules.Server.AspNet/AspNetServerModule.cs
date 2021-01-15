using Modulight.Modules.Options;
using Modulight.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.AspNet
{
    public static class AspNetServerModuleExtensions
    {
        public static IModuleHostBuilder UseAspNetServerModules(this IModuleHostBuilder modules)
        {
            return modules.UsePostMiddleware((modules, services) =>
            {
                services.AddSingleton<IAspNetServerModuleHost>(sp => new AspNetServerModuleHost(sp,
                    modules.Modules.AllSpecifyModules<IAspNetServerModule>().ToArray()));
            });
        }

        public static IAspNetServerModuleHost GetAspNetServerModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IAspNetServerModuleHost>();
    }

    public interface IAspNetServerModule : IModule
    {
        void RegisterAspNetService(IServiceCollection services);

        void UseMiddleware(IApplicationBuilder builder, IServiceProvider provider);

        void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider);
    }

    public abstract class AspNetServerModule<TService, TOption> : Module<TService, TOption>, IAspNetServerModule where TService : class, IModuleService where TOption : class, new()
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
