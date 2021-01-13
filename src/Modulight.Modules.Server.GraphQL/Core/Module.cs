using Modulight.Modules.Options;
using Modulight.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using HotChocolate.AspNetCore.Extensions;
using Modulight.Modules.Server.AspNet;
using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules.Server.GraphQL.Core
{
    public class Module : AspNetServerModule<ModuleService, ModuleOption>
    {
        Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? PostMapEndpoint { get; }

        public Module(Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null) : base()
        {
            Manifest = Manifest with
            {
                Name = "CoreGraphQLServer",
                DisplayName = "Core GraphQL Server",
                Description = "Provide controller for GraphQL server modules.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
            PostMapEndpoint = postMapEndpoint;
        }

        public override void MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            base.MapEndpoint(builder, provider);

            GetService(provider).MapEndpoints(builder, PostMapEndpoint);
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddAspNetServerModules();
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

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            foreach (var module in ModuleHost.Modules.AllSpecifyModules<IGraphQLServerModule>())
            {
                var gbuilder = module.MapEndpoint(builder, ServiceProvider);
                if (postMapEndpoint is not null)
                    postMapEndpoint(module, gbuilder);
            }
        }
    }
}
