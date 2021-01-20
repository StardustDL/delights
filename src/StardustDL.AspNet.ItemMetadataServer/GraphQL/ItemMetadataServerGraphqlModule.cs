using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Server.GraphQL;
using StardustDL.AspNet.ItemMetadataServer.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    [Module(Description = "Provide GraphQL endpoints for item metadata server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class ItemMetadataServerGraphqlModule : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public override Type? MutationType => typeof(ModuleMutation);

        public ItemMetadataServerGraphqlModule() : base()
        {
        }

        public override GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            return base.MapEndpoint(builder, provider);
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddItemMetadataServerModule();
        }
    }
}
