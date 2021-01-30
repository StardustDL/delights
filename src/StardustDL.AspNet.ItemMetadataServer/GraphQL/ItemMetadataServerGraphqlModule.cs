using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;
using StardustDL.AspNet.ItemMetadataServer.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    [Module(Description = "Provide GraphQL endpoints for item metadata server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [GraphQLModuleType("ItemMetadataServer", typeof(ModuleQuery), MutationType = typeof(ModuleMutation))]
    [ModuleDependency(typeof(ItemMetadataServerModule))]
    public class ItemMetadataServerGraphqlModule : GraphQLServerModule<ItemMetadataServerGraphqlModule>
    {
        public ItemMetadataServerGraphqlModule(IModuleHost host) : base(host)
        {
        }
    }
}
