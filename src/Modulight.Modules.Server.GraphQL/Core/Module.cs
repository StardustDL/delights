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

namespace Modulight.Modules.Server.GraphQL.Core
{
    public class Module : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type? QueryType => typeof(ModuleQuery);

        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = "CoreGraphQLServer",
                DisplayName = "Core GraphQL Server",
                Description = "Provide heartbeat and other services for GraphQL server.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }
    }

    public class ModuleQuery
    {
        public string Heartbeat() => "ok";
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

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMap = null)
        {
            foreach (var module in ModuleHost.Modules.AllSpecifyModules<IGraphQLServerModule>())
            {
                var gbuilder = module.MapEndpoint(builder, ServiceProvider);
                if (postMap is not null)
                    postMap(module, gbuilder);
            }
        }
    }
}
