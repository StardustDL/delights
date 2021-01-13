using Modulight.Modules.Options;
using Modulight.Modules.Services;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Modulight.Modules.Server.AspNet.Core
{
    public class Module : Module<ModuleService, ModuleOption>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = "CoreAspNetServer",
                DisplayName = "Core AspNet Server",
                Description = "Provide controller for AspNet server modules.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }
    }

    public class ModuleOption
    {

    }

    public class ModuleService : IModuleService
    {
        public ModuleService(IModuleHost moduleHost, IServiceProvider serviceProvider, IOptions<ModuleOption> options, ILogger<Module> logger)
        {
            ModuleHost = moduleHost;
            Options = options.Value;
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public IModuleHost ModuleHost { get; }

        public IServiceProvider ServiceProvider { get; }

        public ILogger<Module> Logger { get; }

        public ModuleOption Options { get; }

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IAspNetServerModule, IEndpointRouteBuilder>? postMap = null)
        {
            foreach (var module in ModuleHost.Modules.AllSpecifyModules<IAspNetServerModule>())
            {
                module.MapEndpoint(builder, ServiceProvider);
                if (postMap is not null)
                    postMap(module, builder);
            }
        }

        public void UseMiddlewares(IApplicationBuilder builder)
        {
            foreach (var module in ModuleHost.Modules.AllSpecifyModules<IAspNetServerModule>())
            {
                module.UseMiddleware(builder, ServiceProvider);
            }
        }
    }
}
